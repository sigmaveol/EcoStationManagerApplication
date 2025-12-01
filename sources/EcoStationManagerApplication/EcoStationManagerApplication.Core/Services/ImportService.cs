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

        public async Task<Result<ImportResult>> ImportOrdersFromExcelTemplateAsync(string filePath)
        {
            var result = new ImportResult();

            try
            {
                if (!File.Exists(filePath))
                {
                    return Result<ImportResult>.Fail("File không tồn tại");
                }

                var extension = Path.GetExtension(filePath).ToLower();
                if (extension != ".xlsx" && extension != ".xls")
                {
                    return Result<ImportResult>.Fail("Chỉ hỗ trợ file Excel (.xlsx, .xls)");
                }

                // Đọc file Excel bằng ClosedXML
                using (var workbook = new XLWorkbook(filePath))
                {
                    var worksheet = workbook.Worksheets.First();
                    var firstRow = worksheet.FirstRowUsed();
                    if (firstRow == null)
                    {
                        return Result<ImportResult>.Fail("File không có dữ liệu");
                    }

                    // Đọc header để tìm vị trí các cột
                    var headerRow = firstRow;
                    var columnMap = new Dictionary<string, int>();
                    foreach (var cell in headerRow.CellsUsed())
                    {
                        var headerName = cell.GetString()?.Trim().ToLowerInvariant() ?? "";
                        columnMap[headerName] = cell.Address.ColumnNumber;
                    }

                    // Tìm các cột cần thiết (case-insensitive)
                    int colOrderCode = FindColumn(columnMap, "ordercode", "mã đơn", "order_code");
                    int colCustomerName = FindColumn(columnMap, "customername", "tên khách hàng", "customer_name", "name");
                    int colPhone = FindColumn(columnMap, "phone", "số điện thoại", "phone", "sđt");
                    int colAddress = FindColumn(columnMap, "address", "địa chỉ", "diachi", "deliveryaddress", "shippingaddress");
                    int colProductName = FindColumn(columnMap, "productname", "tên sản phẩm", "product_name", "product");
                    int colQuantity = FindColumn(columnMap, "quantity", "số lượng", "sl");
                    int colUnitPrice = FindColumn(columnMap, "unitprice", "đơn giá", "unit_price", "price", "giá");
                    int colDiscount = FindColumn(columnMap, "discount", "giảm giá", "discount");
                    int colNote = FindColumn(columnMap, "note", "ghi chú", "notes");
                    int colCreatedDate = FindColumn(columnMap, "createddate", "ngày tạo", "created_date", "date");

                    if (colCustomerName == 0 || colPhone == 0 || colProductName == 0 || colQuantity == 0 || colUnitPrice == 0)
                    {
                        return Result<ImportResult>.Fail("File thiếu các cột bắt buộc: CustomerName, Phone, ProductName, Quantity, UnitPrice");
                    }

                    // Đọc dữ liệu và group theo OrderCode
                    var orderGroups = new Dictionary<string, List<ExcelOrderRow>>();
                    var rows = worksheet.RowsUsed().Skip(1); // Bỏ qua header

                    int rowNumber = 2; // Bắt đầu từ dòng 2 (sau header)
                    foreach (var row in rows)
                    {
                        try
                        {
                            var orderCode = colOrderCode > 0 ? row.Cell(colOrderCode).GetString()?.Trim() : "";
                            var customerName = row.Cell(colCustomerName).GetString()?.Trim() ?? "";
                            var phone = row.Cell(colPhone).GetString()?.Trim() ?? "";
                            var productName = row.Cell(colProductName).GetString()?.Trim() ?? "";
                            var address = colAddress > 0 ? row.Cell(colAddress).GetString()?.Trim() ?? "" : "";
                            if (!string.IsNullOrEmpty(address) && address.Length > 255)
                            {
                                address = address.Substring(0, 255);
                            }
                            
                            // Đọc số lượng và đơn giá - hỗ trợ cả số và text
                            decimal quantity = 0;
                            var quantityCell = row.Cell(colQuantity);
                            if (quantityCell.DataType == XLDataType.Number)
                            {
                                quantity = quantityCell.GetValue<decimal>();
                            }
                            else
                            {
                                var quantityStr = quantityCell.GetString()?.Trim() ?? "0";
                                if (!decimal.TryParse(quantityStr, NumberStyles.Any, CultureInfo.InvariantCulture, out quantity))
                                {
                                    quantity = 0;
                                }
                            }

                            decimal unitPrice = 0;
                            var unitPriceCell = row.Cell(colUnitPrice);
                            if (unitPriceCell.DataType == XLDataType.Number)
                            {
                                unitPrice = unitPriceCell.GetValue<decimal>();
                            }
                            else
                            {
                                var unitPriceStr = unitPriceCell.GetString()?.Trim() ?? "0";
                                if (!decimal.TryParse(unitPriceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out unitPrice))
                                {
                                    unitPrice = 0;
                                }
                            }

                            decimal discount = 0;
                            if (colDiscount > 0)
                            {
                                var discountCell = row.Cell(colDiscount);
                                if (discountCell.DataType == XLDataType.Number)
                                {
                                    discount = discountCell.GetValue<decimal>();
                                }
                                else
                                {
                                    var discountStr = discountCell.GetString()?.Trim() ?? "0";
                                    decimal.TryParse(discountStr, NumberStyles.Any, CultureInfo.InvariantCulture, out discount);
                                }
                            }

                            var note = colNote > 0 ? row.Cell(colNote).GetString()?.Trim() : "";
                            var createdDateStr = colCreatedDate > 0 ? row.Cell(colCreatedDate).GetString()?.Trim() : "";

                            // Validate dữ liệu
                            if (string.IsNullOrWhiteSpace(customerName))
                            {
                                result.ErrorCount++;
                                result.Errors.Add($"Dòng {rowNumber}: Thiếu tên khách hàng");
                                rowNumber++;
                                continue;
                            }

                            if (string.IsNullOrWhiteSpace(phone))
                            {
                                result.ErrorCount++;
                                result.Errors.Add($"Dòng {rowNumber}: Thiếu số điện thoại");
                                rowNumber++;
                                continue;
                            }

                            if (string.IsNullOrWhiteSpace(productName))
                            {
                                result.ErrorCount++;
                                result.Errors.Add($"Dòng {rowNumber}: Thiếu tên sản phẩm");
                                rowNumber++;
                                continue;
                            }

                            // Validate số lượng và đơn giá
                            if (quantity <= 0)
                            {
                                result.ErrorCount++;
                                result.Errors.Add($"Dòng {rowNumber}: Số lượng phải lớn hơn 0");
                                rowNumber++;
                                continue;
                            }

                            if (unitPrice < 0)
                            {
                                result.ErrorCount++;
                                result.Errors.Add($"Dòng {rowNumber}: Đơn giá không hợp lệ");
                                rowNumber++;
                                continue;
                            }

                            DateTime? createdDate = null;
                            if (colCreatedDate > 0 && !string.IsNullOrWhiteSpace(createdDateStr))
                            {
                                if (DateTime.TryParse(createdDateStr, out DateTime parsedDate))
                                {
                                    createdDate = parsedDate;
                                }
                            }

                            // Tạo key cho order (nếu không có OrderCode, tự tạo unique cho mỗi dòng)
                            if (string.IsNullOrWhiteSpace(orderCode))
                            {
                                orderCode = $"EXCEL_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid().ToString().Substring(0, 8)}";
                            }

                            if (!orderGroups.ContainsKey(orderCode))
                            {
                                orderGroups[orderCode] = new List<ExcelOrderRow>();
                            }

                            orderGroups[orderCode].Add(new ExcelOrderRow
                            {
                                OrderCode = orderCode,
                                CustomerName = customerName,
                                Phone = phone,
                                Address = address,
                                ProductName = productName,
                                Quantity = quantity,
                                UnitPrice = unitPrice,
                                Discount = discount,
                                Note = note,
                                CreatedDate = createdDate
                            });
                        }
                        catch (Exception ex)
                        {
                            result.ErrorCount++;
                            result.Errors.Add($"Dòng {rowNumber}: Lỗi đọc dữ liệu - {ex.Message}");
                        }

                        rowNumber++;
                    }

                    if (orderGroups.Count == 0)
                    {
                        return Result<ImportResult>.Fail("Không có dữ liệu hợp lệ để import");
                    }

                    // Xử lý từng đơn hàng
                    foreach (var orderGroup in orderGroups)
                    {
                        try
                        {
                            var orderRows = orderGroup.Value;
                            var firstRowData = orderRows.First();

                            // 1. Tìm hoặc tạo khách hàng
                            var customer = await _unitOfWork.Customers.GetByPhoneAsync(firstRowData.Phone);
                            if (customer == null)
                            {
                                // Tạo khách hàng mới với retry logic nếu CustomerCode trùng
                                Customer newCustomer = null;
                                int retryCount = 0;
                                const int maxRetries = 5;

                                while (retryCount < maxRetries && newCustomer == null)
                                {
                                    try
                                    {
                                        // Tạo CustomerCode ngắn gọn hơn (max 50 chars)
                                        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                                        var guid = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
                                        var customerCode = $"CUS{timestamp}{guid}";
                                        
                                        // Đảm bảo không vượt quá 50 ký tự
                                        if (customerCode.Length > 50)
                                        {
                                            customerCode = customerCode.Substring(0, 50);
                                        }

                                        // Đảm bảo Name không quá dài (max 255 chars)
                                        var customerName = firstRowData.CustomerName;
                                        if (customerName != null && customerName.Length > 255)
                                        {
                                            customerName = customerName.Substring(0, 255);
                                        }
                                        
                                        // Đảm bảo Phone không quá dài (max 50 chars)
                                        var phone = firstRowData.Phone;
                                        if (phone != null && phone.Length > 50)
                                        {
                                            phone = phone.Substring(0, 50);
                                        }

                                        newCustomer = new Customer
                                        {
                                            Name = customerName,
                                            Phone = phone,
                                            CustomerCode = customerCode,
                                            Rank = CustomerRank.MEMBER,
                                            IsActive = ActiveStatus.ACTIVE,
                                            CreatedDate = DateTime.Now
                                        };

                                        var createCustomerResult = await _customerService.CreateCustomerAsync(newCustomer);
                                        if (createCustomerResult.Success && createCustomerResult.Data > 0)
                                        {
                                            var getCustomerResult = await _customerService.GetCustomerByIdAsync(createCustomerResult.Data);
                                            customer = getCustomerResult.Data;
                                            result.CreatedCustomerIds.Add(createCustomerResult.Data);
                                            break; // Thành công, thoát khỏi retry loop
                                        }
                                        else
                                        {
                                            // Nếu lỗi do CustomerCode trùng, retry với code mới
                                            if (createCustomerResult.Message?.Contains("Mã khách hàng") == true || 
                                                createCustomerResult.Message?.Contains("đã tồn tại") == true)
                                            {
                                                retryCount++;
                                                newCustomer = null; // Reset để retry
                                                await Task.Delay(10); // Đợi một chút trước khi retry
                                                continue;
                                            }
                                            else
                                            {
                                                // Lỗi khác, không retry
                                                result.ErrorCount++;
                                                result.Errors.Add($"Đơn {orderGroup.Key}: Không thể tạo khách hàng - {createCustomerResult.Message}");
                                                break;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        retryCount++;
                                        if (retryCount >= maxRetries)
                                        {
                                            result.ErrorCount++;
                                            result.Errors.Add($"Đơn {orderGroup.Key}: Lỗi tạo khách hàng sau {maxRetries} lần thử - {ex.Message}");
                                        }
                                        else
                                        {
                                            await Task.Delay(10); // Đợi trước khi retry
                                        }
                                    }
                                }
                            }

                            if (customer == null)
                            {
                                result.ErrorCount++;
                                result.Errors.Add($"Đơn {orderGroup.Key}: Không thể tạo hoặc tìm khách hàng");
                                continue;
                            }

                            // 2. Tìm sản phẩm và tạo OrderDetails
                            var orderDetails = new List<OrderDetail>();
                            var allProductsResult = await _productService.GetAllProductsAsync();
                            if (!allProductsResult.Success || allProductsResult.Data == null)
                            {
                                result.ErrorCount++;
                                result.Errors.Add($"Đơn {orderGroup.Key}: Không thể lấy danh sách sản phẩm");
                                continue;
                            }

                            var allProducts = allProductsResult.Data.ToList();
                            bool hasError = false;

                            foreach (var rowData in orderRows)
                            {
                                // Tìm sản phẩm theo tên (case-insensitive, partial match)
                                var productNameLower = rowData.ProductName.ToLowerInvariant();
                                var product = allProducts.FirstOrDefault(p =>
                                    (p.Name?.ToLowerInvariant() ?? "").Equals(productNameLower) ||
                                    (p.Name?.ToLowerInvariant() ?? "").Contains(productNameLower) ||
                                    productNameLower.Contains((p.Name?.ToLowerInvariant() ?? "")));

                                if (product == null)
                                {
                                    result.ErrorCount++;
                                    result.Errors.Add($"Đơn {orderGroup.Key}: Không tìm thấy sản phẩm '{rowData.ProductName}'");
                                    hasError = true;
                                    continue;
                                }

                                orderDetails.Add(new OrderDetail
                                {
                                    ProductId = product.ProductId,
                                    Quantity = rowData.Quantity,
                                    UnitPrice = rowData.UnitPrice
                                });
                            }

                            if (hasError || orderDetails.Count == 0)
                            {
                                continue;
                            }

                            // 3. Kiểm tra OrderCode đã tồn tại chưa và đảm bảo độ dài hợp lệ
                            string finalOrderCode = orderGroup.Key;
                            
                            // Đảm bảo OrderCode không quá dài (max 50 chars theo schema)
                            if (!string.IsNullOrWhiteSpace(finalOrderCode) && finalOrderCode.Length > 50)
                            {
                                finalOrderCode = finalOrderCode.Substring(0, 50);
                            }
                            
                            // Nếu OrderCode trống hoặc null, tạo mới
                            if (string.IsNullOrWhiteSpace(finalOrderCode))
                            {
                                var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                                var guid = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
                                finalOrderCode = $"EXCEL{timestamp}{guid}";
                                if (finalOrderCode.Length > 50)
                                {
                                    finalOrderCode = finalOrderCode.Substring(0, 50);
                                }
                            }
                            
                            // Kiểm tra OrderCode đã tồn tại chưa
                            var existingOrder = await _unitOfWork.Orders.GetByOrderCodeAsync(orderGroup.Key);
                            
                            // Nếu OrderCode đã tồn tại, kiểm tra xem có trùng 100% không
                            if (existingOrder != null)
                            {
                                // Kiểm tra trùng 100%: cùng CustomerId, cùng số lượng sản phẩm
                                bool isDuplicate = false;
                                if (existingOrder.CustomerId == customer.CustomerId)
                                {
                                    // Lấy order details của đơn hàng đã tồn tại
                                    var existingOrderDetails = await _unitOfWork.OrderDetails.GetByOrderAsync(existingOrder.OrderId);
                                    if (existingOrderDetails != null && existingOrderDetails.Count() == orderDetails.Count)
                                    {
                                        // So sánh từng sản phẩm
                                        var existingDetailsList = existingOrderDetails.ToList();
                                        isDuplicate = true;
                                        foreach (var newDetail in orderDetails)
                                        {
                                            var matchingDetail = existingDetailsList.FirstOrDefault(ed => 
                                                ed.ProductId == newDetail.ProductId && 
                                                ed.Quantity == newDetail.Quantity && 
                                                ed.UnitPrice == newDetail.UnitPrice);
                                            if (matchingDetail == null)
                                            {
                                                isDuplicate = false;
                                                break;
                                            }
                                        }
                                    }
                                }
                                
                                if (isDuplicate)
                                {
                                    result.ErrorCount++;
                                    result.Errors.Add($"Đơn {orderGroup.Key}: Đơn hàng đã tồn tại (trùng 100%)");
                                    continue;
                                }
                                
                                // Nếu không trùng 100%, tạo OrderCode mới
                                int orderCodeRetryCount = 0;
                                const int maxOrderCodeRetries = 5;
                                
                                while (existingOrder != null && orderCodeRetryCount < maxOrderCodeRetries)
                                {
                                    var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                                    var guid = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
                                    finalOrderCode = $"EXCEL{timestamp}{guid}";
                                    if (finalOrderCode.Length > 50)
                                    {
                                        finalOrderCode = finalOrderCode.Substring(0, 50);
                                    }
                                    
                                    existingOrder = await _unitOfWork.Orders.GetByOrderCodeAsync(finalOrderCode);
                                    orderCodeRetryCount++;
                                    
                                    if (orderCodeRetryCount > 0)
                                    {
                                        await Task.Delay(10);
                                    }
                                }
                                
                                if (existingOrder != null)
                                {
                                    result.ErrorCount++;
                                    result.Errors.Add($"Đơn {orderGroup.Key}: Không thể tạo OrderCode unique sau {maxOrderCodeRetries} lần thử");
                                    continue;
                                }
                            }

                            // 4. Tạo Order
                            var order = new Order
                            {
                                OrderCode = finalOrderCode,
                                CustomerId = customer.CustomerId,
                                Source = OrderSource.EXCEL,
                                Status = OrderStatus.CONFIRMED,
                                PaymentStatus = PaymentStatus.UNPAID,
                                PaymentMethod = PaymentMethod.CASH,
                                Address = string.IsNullOrWhiteSpace(firstRowData.Address)
                                    ? null
                                    : (firstRowData.Address.Length > 255
                                        ? firstRowData.Address.Substring(0, 255)
                                        : firstRowData.Address),
                                Note = firstRowData.Note?.Length > 1000 ? firstRowData.Note.Substring(0, 1000) : firstRowData.Note,
                                DiscountedAmount = orderRows.Sum(r => r.Discount),
                                LastUpdated = firstRowData.CreatedDate ?? DateTime.Now
                            };

                            var createOrderResult = await _orderService.CreateOrderAsync(order, orderDetails);
                            if (createOrderResult.Success && createOrderResult.Data > 0)
                            {
                                result.SuccessCount++;
                                result.CreatedOrderIds.Add(createOrderResult.Data);
                            }
                            else
                            {
                                result.ErrorCount++;
                                var errorMsg = createOrderResult.Message ?? "Lỗi không xác định";
                                result.Errors.Add($"Đơn {orderGroup.Key}: {errorMsg}");
                                _logger.Error($"Import order failed - OrderCode: {orderGroup.Key}, Error: {errorMsg}");
                            }
                        }
                        catch (Exception ex)
                        {
                            result.ErrorCount++;
                            var errorMsg = $"Lỗi xử lý - {ex.Message}";
                            if (ex.InnerException != null)
                            {
                                errorMsg += $" | Inner: {ex.InnerException.Message}";
                            }
                            result.Errors.Add($"Đơn {orderGroup.Key}: {errorMsg}");
                            _logger.Error($"Import order exception - OrderCode: {orderGroup.Key}, Exception: {ex.Message}");
                            if (ex.InnerException != null)
                            {
                                _logger.Error($"Inner exception: {ex.InnerException.Message}");
                            }
                        }
                    }

                    return Result<ImportResult>.Ok(result,
                        $"Import hoàn tất: {result.SuccessCount} đơn thành công, {result.ErrorCount} lỗi");
                }
            }
            catch (Exception ex)
            {
                return HandleException<ImportResult>(ex, "import đơn hàng từ Excel template");
            }
        }

        private int FindColumn(Dictionary<string, int> columnMap, params string[] possibleNames)
        {
            foreach (var name in possibleNames)
            {
                var key = name.ToLowerInvariant();
                if (columnMap.ContainsKey(key))
                {
                    return columnMap[key];
                }
            }
            return 0;
        }

        private class ExcelOrderRow
        {
            public string OrderCode { get; set; }
            public string CustomerName { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
            public string ProductName { get; set; }
            public decimal Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal Discount { get; set; }
            public string Note { get; set; }
            public DateTime? CreatedDate { get; set; }
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

