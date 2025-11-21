using ClosedXML.Excel;
using EcoStationManagerApplication.Core.Helpers;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    public class ImportService : BaseService, IImportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;

        public ImportService(
            IUnitOfWork unitOfWork,
            IOrderService orderService,
            ICustomerService customerService,
            IProductService productService)
            : base("ImportService")
        {
            _unitOfWork = unitOfWork;
            _orderService = orderService;
            _customerService = customerService;
            _productService = productService;
        }

        public async Task<Result<ImportResult>> ImportOrdersFromFileAsync(string filePath, OrderSource source)
        {
            var result = new ImportResult();

            try
            {
                if (!File.Exists(filePath))
                {
                    return Result<ImportResult>.Fail("File không tồn tại");
                }

                var extension = Path.GetExtension(filePath).ToLower();
                DataTable dataTable;

                if (extension == ".csv")
                {
                    dataTable = ReadCsvFile(filePath);
                }
                else if (extension == ".xlsx" || extension == ".xls")
                {
                    dataTable = ReadExcelFile(filePath);
                }
                else
                {
                    return Result<ImportResult>.Fail("Định dạng file không được hỗ trợ. Chỉ hỗ trợ .xlsx, .xls, .csv");
                }

                if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    return Result<ImportResult>.Fail("File không có dữ liệu");
                }

                // Parse và tạo đơn hàng
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    try
                    {
                        var row = dataTable.Rows[i];
                        var orderResult = await ParseAndCreateOrderAsync(row, source);

                        if (orderResult.Success)
                        {
                            result.SuccessCount++;
                            if (orderResult.Data > 0)
                            {
                                result.CreatedOrderIds.Add(orderResult.Data);
                            }
                        }
                        else
                        {
                            result.ErrorCount++;
                            result.Errors.Add($"Dòng {i + 2}: {orderResult.Message}");
                        }
                    }
                    catch (Exception ex)
                    {
                        result.ErrorCount++;
                        result.Errors.Add($"Dòng {i + 2}: Lỗi xử lý - {ex.Message}");
                    }
                }

                return Result<ImportResult>.Ok(result, 
                    $"Import hoàn tất: {result.SuccessCount} thành công, {result.ErrorCount} lỗi");
            }
            catch (Exception ex)
            {
                return HandleException<ImportResult>(ex, "import đơn hàng từ file");
            }
        }

        public async Task<Result<ImportResult>> ImportCustomersFromFileAsync(string filePath)
        {
            var result = new ImportResult();

            try
            {
                if (!File.Exists(filePath))
                {
                    return Result<ImportResult>.Fail("File không tồn tại");
                }

                var extension = Path.GetExtension(filePath).ToLower();
                DataTable dataTable;

                if (extension == ".csv")
                {
                    dataTable = ReadCsvFile(filePath);
                }
                else if (extension == ".xlsx" || extension == ".xls")
                {
                    dataTable = ReadExcelFile(filePath);
                }
                else
                {
                    return Result<ImportResult>.Fail("Định dạng file không được hỗ trợ");
                }

                if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    return Result<ImportResult>.Fail("File không có dữ liệu");
                }

                // Parse và tạo khách hàng
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    try
                    {
                        var row = dataTable.Rows[i];
                        var customerResult = await ParseAndCreateCustomerAsync(row);

                        if (customerResult.Success)
                        {
                            result.SuccessCount++;
                            if (customerResult.Data > 0)
                            {
                                result.CreatedCustomerIds.Add(customerResult.Data);
                            }
                        }
                        else
                        {
                            result.ErrorCount++;
                            result.Errors.Add($"Dòng {i + 2}: {customerResult.Message}");
                        }
                    }
                    catch (Exception ex)
                    {
                        result.ErrorCount++;
                        result.Errors.Add($"Dòng {i + 2}: Lỗi xử lý - {ex.Message}");
                    }
                }

                return Result<ImportResult>.Ok(result,
                    $"Import hoàn tất: {result.SuccessCount} thành công, {result.ErrorCount} lỗi");
            }
            catch (Exception ex)
            {
                return HandleException<ImportResult>(ex, "import khách hàng từ file");
            }
        }

        public async Task<Result<bool>> ValidateImportFileAsync(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return Result<bool>.Fail("File không tồn tại");
                }

                var extension = Path.GetExtension(filePath).ToLower();
                if (extension != ".csv" && extension != ".xlsx" && extension != ".xls")
                {
                    return Result<bool>.Fail("Định dạng file không được hỗ trợ");
                }

                DataTable dataTable;
                if (extension == ".csv")
                {
                    dataTable = ReadCsvFile(filePath);
                }
                else
                {
                    dataTable = ReadExcelFile(filePath);
                }

                if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    return Result<bool>.Fail("File không có dữ liệu");
                }

                return Result<bool>.Ok(true, "File hợp lệ");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "validate file");
            }
        }

        #region Private Methods

        private DataTable ReadExcelFile(string filePath)
        {
            try
            {
                using (var workbook = new XLWorkbook(filePath))
                {
                    var worksheet = workbook.Worksheets.First();
                    var dataTable = new DataTable();

                    // Đọc header
                    var firstRow = worksheet.FirstRowUsed();
                    if (firstRow == null) return null;

                    foreach (var cell in firstRow.CellsUsed())
                    {
                        dataTable.Columns.Add(cell.GetString() ?? $"Column{cell.Address.ColumnNumber}");
                    }

                    // Đọc dữ liệu
                    var rows = worksheet.RowsUsed().Skip(1);
                    foreach (var row in rows)
                    {
                        var dataRow = dataTable.NewRow();
                        int colIndex = 0;
                        foreach (var cell in row.CellsUsed())
                        {
                            if (colIndex < dataTable.Columns.Count)
                            {
                                dataRow[colIndex] = cell.GetString() ?? "";
                                colIndex++;
                            }
                        }
                        dataTable.Rows.Add(dataRow);
                    }

                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi đọc file Excel: {ex.Message}");
                throw;
            }
        }

        private DataTable ReadCsvFile(string filePath)
        {
            try
            {
                var dataTable = new DataTable();
                var lines = File.ReadAllLines(filePath, System.Text.Encoding.UTF8);

                if (lines.Length == 0) return null;

                // Đọc header
                var headers = lines[0].Split(',');
                foreach (var header in headers)
                {
                    dataTable.Columns.Add(header.Trim());
                }

                // Đọc dữ liệu
                for (int i = 1; i < lines.Length; i++)
                {
                    var values = lines[i].Split(',');
                    var row = dataTable.NewRow();
                    for (int j = 0; j < Math.Min(values.Length, dataTable.Columns.Count); j++)
                    {
                        row[j] = values[j].Trim();
                    }
                    dataTable.Rows.Add(row);
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi đọc file CSV: {ex.Message}");
                throw;
            }
        }

        private async Task<Result<int>> ParseAndCreateOrderAsync(DataRow row, OrderSource source)
        {
            try
            {
                // Mapping các cột phổ biến (có thể tùy chỉnh theo format file)
                var customerName = GetValue(row, "Tên khách hàng", "CustomerName", "Name", "Họ tên");
                var phone = GetValue(row, "Số điện thoại", "Phone", "PhoneNumber", "Điện thoại");
                var address = GetValue(row, "Địa chỉ", "Address", "Địa chỉ giao hàng");
                var productName = GetValue(row, "Sản phẩm", "Product", "ProductName", "Tên sản phẩm");
                var quantityStr = GetValue(row, "Số lượng", "Quantity", "SL");
                var unitPriceStr = GetValue(row, "Đơn giá", "UnitPrice", "Price", "Giá");
                var note = GetValue(row, "Ghi chú", "Note", "Notes", "Note");

                if (string.IsNullOrWhiteSpace(customerName))
                {
                    return Result<int>.Fail("Thiếu tên khách hàng");
                }

                if (string.IsNullOrWhiteSpace(phone))
                {
                    return Result<int>.Fail("Thiếu số điện thoại");
                }

                // Tìm hoặc tạo khách hàng
                var customer = await _unitOfWork.Customers.GetByPhoneAsync(phone);

                if (customer == null)
                {
                    // Tạo khách hàng mới
                    var newCustomer = new Customer
                    {
                        Name = customerName,
                        Phone = phone,
                        CreatedDate = DateTime.Now
                    };
                    var createCustomerResult = await _customerService.CreateCustomerAsync(newCustomer);
                    if (createCustomerResult.Success && createCustomerResult.Data > 0)
                    {
                        var getCustomerResult = await _customerService.GetCustomerByIdAsync(createCustomerResult.Data);
                        customer = getCustomerResult.Data;
                    }
                }

                if (customer == null)
                {
                    return Result<int>.Fail("Không thể tạo hoặc tìm khách hàng");
                }

                // Parse số lượng và đơn giá
                if (!decimal.TryParse(quantityStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal quantity))
                {
                    quantity = 1;
                }

                if (!decimal.TryParse(unitPriceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal unitPrice))
                {
                    unitPrice = 0;
                }

                // Tìm sản phẩm
                var productsResult = await _productService.GetAllProductsAsync();
                var productNameLower = productName?.ToLowerInvariant() ?? "";
                var product = productsResult.Data?.FirstOrDefault(p => 
                    (p.Name?.ToLowerInvariant() ?? "").Contains(productNameLower) ||
                    productNameLower.Contains((p.Name?.ToLowerInvariant() ?? "")));

                if (product == null)
                {
                    return Result<int>.Fail($"Không tìm thấy sản phẩm: {productName}");
                }

                // Tạo đơn hàng
                var order = new Order
                {
                    CustomerId = customer.CustomerId,
                    Source = source,
                    Status = OrderStatus.DRAFT,
                    PaymentStatus = PaymentStatus.UNPAID,
                    PaymentMethod = PaymentMethod.CASH,
                    Address = address ?? "", // Address có thể null
                    Note = note,
                    TotalAmount = quantity * unitPrice,
                    LastUpdated = DateTime.Now
                };

                var orderDetails = new List<OrderDetail>
                {
                    new OrderDetail
                    {
                        ProductId = product.ProductId,
                        Quantity = quantity,
                        UnitPrice = unitPrice
                    }
                };

                var createOrderResult = await _orderService.CreateOrderAsync(order, orderDetails);
                return createOrderResult;
            }
            catch (Exception ex)
            {
                return Result<int>.Fail($"Lỗi parse đơn hàng: {ex.Message}");
            }
        }

        private async Task<Result<int>> ParseAndCreateCustomerAsync(DataRow row)
        {
            try
            {
                var name = GetValue(row, "Tên khách hàng", "CustomerName", "Name", "Họ tên");
                var phone = GetValue(row, "Số điện thoại", "Phone", "PhoneNumber", "Điện thoại");
                var address = GetValue(row, "Địa chỉ", "Address");
                var email = GetValue(row, "Email", "E-mail");

                if (string.IsNullOrWhiteSpace(name))
                {
                    return Result<int>.Fail("Thiếu tên khách hàng");
                }

                if (string.IsNullOrWhiteSpace(phone))
                {
                    return Result<int>.Fail("Thiếu số điện thoại");
                }

                // Kiểm tra khách hàng đã tồn tại
                var existingCustomer = await _unitOfWork.Customers.GetByPhoneAsync(phone);
                if (existingCustomer != null)
                {
                    return Result<int>.Ok(existingCustomer.CustomerId, "Khách hàng đã tồn tại");
                }

                var customer = new Customer
                {
                    Name = name,
                    Phone = phone,
                    // Customer entity không có Address field trong V0
                    CreatedDate = DateTime.Now
                };

                var createResult = await _customerService.CreateCustomerAsync(customer);
                return createResult;
            }
            catch (Exception ex)
            {
                return Result<int>.Fail($"Lỗi parse khách hàng: {ex.Message}");
            }
        }

        private string GetValue(DataRow row, params string[] columnNames)
        {
            foreach (var columnName in columnNames)
            {
                if (row.Table.Columns.Contains(columnName))
                {
                    var value = row[columnName]?.ToString();
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        return value.Trim();
                    }
                }
            }
            return string.Empty;
        }

        #endregion
    }
}

