using EcoStationManagerApplication.Common.Exporters;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.UI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class BackupControl : UserControl
    {
        private readonly IExportService _exportService;
        private readonly IExcelExporter _excelExporter;
        private readonly IPdfExporter _pdfExporter;
        private readonly ICustomerService _customerService;
        private readonly IPackagingTransactionService _packagingTransactionService;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IInventoryService _inventoryService;
        private readonly IPackagingService _packagingService;
        private readonly IProductService _productService;
        private readonly IPackagingInventoryService _packagingInventoryService;
        private DateTime? _lastBackupTime;
        private bool _isProcessing;

        public BackupControl()
        {
            InitializeComponent();
            _exportService = AppServices.ExportService ?? throw new InvalidOperationException("Không thể khởi tạo dịch vụ export.");
            _excelExporter = new ExcelExporter();
            _pdfExporter = new PdfExporter();
            _customerService = AppServices.CustomerService ?? throw new InvalidOperationException("Không thể khởi tạo dịch vụ khách hàng.");
            _packagingTransactionService = AppServices.PackagingTransactionService ?? throw new InvalidOperationException("Không thể khởi tạo dịch vụ giao dịch bao bì.");
            _orderService = AppServices.OrderService ?? throw new InvalidOperationException("Không thể khởi tạo dịch vụ đơn hàng.");
            _orderDetailService = AppServices.OrderDetailService ?? throw new InvalidOperationException("Không thể khởi tạo dịch vụ chi tiết đơn hàng.");
            _inventoryService = AppServices.InventoryService ?? throw new InvalidOperationException("Không thể khởi tạo dịch vụ tồn kho.");
            _packagingService = AppServices.PackagingService ?? throw new InvalidOperationException("Không thể khởi tạo dịch vụ bao bì.");
            _productService = AppServices.ProductService ?? throw new InvalidOperationException("Không thể khởi tạo dịch vụ sản phẩm.");
            _packagingInventoryService = AppServices.PackagingInventoryService ?? throw new InvalidOperationException("Không thể khởi tạo dịch vụ tồn kho bao bì.");
        }

        private async void btnBackupExcel_Click(object sender, EventArgs e)
        {
            if (_isProcessing) return;

            try
            {
                using (var saveDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    FileName = $"EcoStation_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                    Title = "Chọn nơi lưu file sao lưu Excel"
                })
                {
                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        await RunWithProcessingStateAsync(() => BackupExcelDataAsync(saveDialog.FileName));
                        ShowSuccessMessage($"Đã sao lưu Excel thành công!\nFile: {saveDialog.FileName}");
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                ShowWarningMessage(ex.Message);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Lỗi khi sao lưu Excel: {ex.Message}");
            }
        }

        private async void btnBackupPDF_Click(object sender, EventArgs e)
        {
            if (_isProcessing) return;

            try
            {
                using (var saveDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    FileName = $"EcoStation_Report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
                    Title = "Chọn nơi lưu file sao lưu PDF"
                })
                {
                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        await RunWithProcessingStateAsync(() => BackupPdfDataAsync(saveDialog.FileName));
                        ShowSuccessMessage($"Đã sao lưu PDF thành công!\nFile: {saveDialog.FileName}");
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                ShowWarningMessage(ex.Message);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Lỗi khi sao lưu PDF: {ex.Message}");
            }
        }

        private async void btnBackupSQL_Click(object sender, EventArgs e)
        {
            if (_isProcessing) return;

            try
            {
                using (var saveDialog = new SaveFileDialog
                {
                    Filter = "SQL files (*.sql)|*.sql",
                    FileName = $"EcoStation_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.sql",
                    Title = "Chọn nơi lưu file sao lưu SQL"
                })
                {
                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        await RunWithProcessingStateAsync(() => BackupSqlDataAsync(saveDialog.FileName));
                        ShowSuccessMessage($"Đã sao lưu SQL thành công!\nFile: {saveDialog.FileName}");
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                ShowWarningMessage(ex.Message);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Lỗi khi sao lưu SQL: {ex.Message}");
            }
        }

        private async void btnRestore_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtRestoreFile?.Text))
                {
                    ShowWarningMessage("Vui lòng chọn một file để phục hồi trước.");
                    return;
                }

                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn phục hồi dữ liệu từ file:\n{txtRestoreFile.Text}?\n\nLưu ý: Thao tác này sẽ ghi đè dữ liệu hiện tại.",
                    "Xác nhận phục hồi",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    await RunWithProcessingStateAsync(() => RestoreDataAsync(txtRestoreFile.Text));
                    ShowSuccessMessage("Đã phục hồi dữ liệu thành công!");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Lỗi khi phục hồi dữ liệu: {ex.Message}");
            }
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            try
            {
                var openDialog = new OpenFileDialog();
                openDialog.Filter = "Backup files (*.xlsx;*.pdf;*.sql)|*.xlsx;*.pdf;*.sql|All files (*.*)|*.*";
                openDialog.Title = "Chọn file sao lưu để phục hồi";

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    txtRestoreFile.Text = openDialog.FileName;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Lỗi khi chọn file: {ex.Message}");
            }
        }

        #region Helper Methods
        private void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        #endregion

        #region Service Methods
        private async Task BackupExcelDataAsync(string filePath)
        {
            ValidateBackupPath(filePath);

            var orders = await GetOrderExportDataAsync();
            var ordersTable = BuildOrdersDataTable(orders);

            var customersResult = await _customerService.SearchCustomersAsync("");
            var customers = customersResult.Success && customersResult.Data != null ? customersResult.Data.ToList() : new List<EcoStationManagerApplication.Models.Entities.Customer>();
            var customersTable = BuildCustomersDataTable(customers);

            var transactions = new List<EcoStationManagerApplication.Models.Entities.PackagingTransaction>();
            foreach (var c in customers)
            {
                var tRes = await _packagingTransactionService.GetTransactionsByCustomerAsync(c.CustomerId);
                if (tRes.Success && tRes.Data != null)
                {
                    transactions.AddRange(tRes.Data);
                }
            }
            var transactionsTable = BuildCustomerTransactionsDataTable(transactions);

            var inventoryResult = await _inventoryService.GetAllAsync();
            var inventory = inventoryResult.Success && inventoryResult.Data != null ? inventoryResult.Data.ToList() : new List<EcoStationManagerApplication.Models.Entities.Inventory>();

            var packagingResult = await _packagingService.GetAllPackagingsAsync();
            var packaging = packagingResult.Success && packagingResult.Data != null ? packagingResult.Data.ToList() : new List<EcoStationManagerApplication.Models.Entities.Packaging>();
            var packagingTable = BuildPackagingDataTable(packaging);

            var productsResult = await _productService.GetAllProductsAsync();
            var products = productsResult.Success && productsResult.Data != null ? productsResult.Data.ToList() : new List<EcoStationManagerApplication.Models.Entities.Product>();
            var productsTable = BuildProductsDataTable(products);
            var inventoryTable = BuildProductInventoryDataTable(inventory, products);

            var ordersAllResult = await _orderService.GetAllAsync();
            var ordersAll = ordersAllResult.Success && ordersAllResult.Data != null ? ordersAllResult.Data.ToList() : new List<OrderDTO>();
            var orderDetailsTable = await BuildOrderDetailsDataTableAsync(ordersAll, products);

            var packagingInventoryResult = await _packagingInventoryService.GetAllAsync();
            var packagingInventory = packagingInventoryResult.Success && packagingInventoryResult.Data != null ? packagingInventoryResult.Data.ToList() : new List<EcoStationManagerApplication.Models.Entities.PackagingInventory>();
            var packagingInventoryTable = BuildPackagingInventoryDataTable(packagingInventory, packaging);

            var sheets = new Dictionary<string, System.Data.DataTable>
            {
                { "KhachHang", customersTable },
                { "GiaoDichKhachHang", transactionsTable },
                { "DonHang", ordersTable },
                { "ChiTietDonHang", orderDetailsTable },
                { "TonKhoSanPham", inventoryTable },
                { "BaoBi", packagingTable },
                { "TonKhoBaoBi", packagingInventoryTable },
                { "SanPham", productsTable }
            };

            var headersBySheet = new Dictionary<string, Dictionary<string, string>>
            {
                { "KhachHang", GetCustomerColumnHeaders() },
                { "GiaoDichKhachHang", GetCustomerTransactionHeaders() },
                { "DonHang", GetOrderColumnHeaders() },
                { "ChiTietDonHang", GetOrderDetailColumnHeaders() },
                { "TonKhoSanPham", GetInventoryColumnHeaders() },
                { "BaoBi", GetPackagingColumnHeaders() },
                { "TonKhoBaoBi", GetPackagingInventoryColumnHeaders() },
                { "SanPham", GetProductColumnHeaders() }
            };

            var titlesBySheet = new Dictionary<string, string>
            {
                { "KhachHang", $"DANH SÁCH KHÁCH HÀNG\nTổng số: {customers.Count}" },
                { "GiaoDichKhachHang", $"GIAO DỊCH KHÁCH HÀNG\nTổng số: {transactions.Count}" },
                { "DonHang", $"DANH SÁCH ĐƠN HÀNG\nTổng số: {orders.Count}" },
                { "ChiTietDonHang", $"CHI TIẾT ĐƠN HÀNG\nTổng số: {orderDetailsTable.Rows.Count}" },
                { "TonKhoSanPham", $"TỒN KHO SẢN PHẨM\nTổng số: {inventory.Count}" },
                { "BaoBi", $"DANH SÁCH BAO BÌ\nTổng số: {packaging.Count}" },
                { "TonKhoBaoBi", $"TỒN KHO BAO BÌ\nTổng số: {packagingInventory.Count}" },
                { "SanPham", $"DANH SÁCH SẢN PHẨM\nTổng số: {products.Count}" }
            };

            _excelExporter.ExportMultipleSheets(sheets, filePath, headersBySheet, titlesBySheet, null);
            UpdateLastBackupTime();
        }

        private async Task BackupPdfDataAsync(string filePath)
        {
            ValidateBackupPath(filePath);

            var orders = await GetOrderExportDataAsync();
            var ordersTable = BuildOrdersDataTable(orders);

            var customersResult = await _customerService.SearchCustomersAsync("");
            var customers = customersResult.Success && customersResult.Data != null ? customersResult.Data.ToList() : new List<EcoStationManagerApplication.Models.Entities.Customer>();
            var customersTable = BuildCustomersDataTable(customers);

            var transactions = new List<EcoStationManagerApplication.Models.Entities.PackagingTransaction>();
            foreach (var c in customers)
            {
                var tRes = await _packagingTransactionService.GetTransactionsByCustomerAsync(c.CustomerId);
                if (tRes.Success && tRes.Data != null)
                {
                    transactions.AddRange(tRes.Data);
                }
            }
            var transactionsTable = BuildCustomerTransactionsDataTable(transactions);

            var inventoryResult = await _inventoryService.GetAllAsync();
            var inventory = inventoryResult.Success && inventoryResult.Data != null ? inventoryResult.Data.ToList() : new List<EcoStationManagerApplication.Models.Entities.Inventory>();

            var packagingResult = await _packagingService.GetAllPackagingsAsync();
            var packaging = packagingResult.Success && packagingResult.Data != null ? packagingResult.Data.ToList() : new List<EcoStationManagerApplication.Models.Entities.Packaging>();
            var packagingTable = BuildPackagingDataTable(packaging);

            var productsResult = await _productService.GetAllProductsAsync();
            var products = productsResult.Success && productsResult.Data != null ? productsResult.Data.ToList() : new List<EcoStationManagerApplication.Models.Entities.Product>();
            var productsTable = BuildProductsDataTable(products);
            var inventoryTable = BuildProductInventoryDataTable(inventory, products);

            var ordersAllResult = await _orderService.GetAllAsync();
            var ordersAll = ordersAllResult.Success && ordersAllResult.Data != null ? ordersAllResult.Data.ToList() : new List<OrderDTO>();
            var orderDetailsTable = await BuildOrderDetailsDataTableAsync(ordersAll, products);

            var packagingInventoryResult = await _packagingInventoryService.GetAllAsync();
            var packagingInventory = packagingInventoryResult.Success && packagingInventoryResult.Data != null ? packagingInventoryResult.Data.ToList() : new List<EcoStationManagerApplication.Models.Entities.PackagingInventory>();
            var packagingInventoryTable = BuildPackagingInventoryDataTable(packagingInventory, packaging);

            var sections = new Dictionary<string, System.Data.DataTable>
            {
                { "KhachHang", customersTable },
                { "GiaoDichKhachHang", transactionsTable },
                { "DonHang", ordersTable },
                { "ChiTietDonHang", orderDetailsTable },
                { "TonKhoSanPham", inventoryTable },
                { "BaoBi", packagingTable },
                { "TonKhoBaoBi", packagingInventoryTable },
                { "SanPham", productsTable }
            };

            var headersBySection = new Dictionary<string, Dictionary<string, string>>
            {
                { "KhachHang", GetCustomerColumnHeaders() },
                { "GiaoDichKhachHang", GetCustomerTransactionHeaders() },
                { "DonHang", GetOrderColumnHeaders() },
                { "ChiTietDonHang", GetOrderDetailColumnHeaders() },
                { "TonKhoSanPham", GetInventoryColumnHeaders() },
                { "BaoBi", GetPackagingColumnHeaders() },
                { "TonKhoBaoBi", GetPackagingInventoryColumnHeaders() },
                { "SanPham", GetProductColumnHeaders() }
            };

            var titlesBySection = new Dictionary<string, string>
            {
                { "KhachHang", $"DANH SÁCH KHÁCH HÀNG\nTổng số: {customers.Count}" },
                { "GiaoDichKhachHang", $"GIAO DỊCH KHÁCH HÀNG\nTổng số: {transactions.Count}" },
                { "DonHang", $"DANH SÁCH ĐƠN HÀNG\nTổng số: {orders.Count}" },
                { "ChiTietDonHang", $"CHI TIẾT ĐƠN HÀNG\nTổng số: {orderDetailsTable.Rows.Count}" },
                { "TonKhoSanPham", $"TỒN KHO SẢN PHẨM\nTổng số: {inventory.Count}" },
                { "BaoBi", $"DANH SÁCH BAO BÌ\nTổng số: {packaging.Count}" },
                { "TonKhoBaoBi", $"TỒN KHO BAO BÌ\nTổng số: {packagingInventory.Count}" },
                { "SanPham", $"DANH SÁCH SẢN PHẨM\nTổng số: {products.Count}" }
            };

            var chartsBySection = new Dictionary<string, IList<byte[]>>
            {
                { "DonHang", GenerateBackupCharts(orders) }
            };

            _pdfExporter.ExportMultipleSections(sections, filePath, headersBySection, titlesBySection, chartsBySection);
            UpdateLastBackupTime();
        }

        private async Task BackupSqlDataAsync(string filePath)
        {
            ValidateBackupPath(filePath);
            var backupService = AppServices.DatabaseBackupService ?? throw new InvalidOperationException("Không thể khởi tạo dịch vụ backup SQL.");
            await backupService.DumpToSqlFileAsync(filePath);
            UpdateLastBackupTime();
        }

        

        private async Task RestoreDataAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                throw new FileNotFoundException("Không tìm thấy file sao lưu để phục hồi.", filePath);

            var ext = Path.GetExtension(filePath)?.ToLowerInvariant();
            if (ext == ".pdf")
            {
                throw new InvalidOperationException("Không hỗ trợ phục hồi từ file PDF. Vui lòng chọn file Excel/CSV.");
            }

            if (ext == ".sql")
            {
                var backupService = AppServices.DatabaseBackupService ?? throw new InvalidOperationException("Không thể khởi tạo dịch vụ backup.");
                await Task.Run(() => backupService.RestoreFromSqlFileAsync(filePath));
                return;
            }

            var importService = AppServices.ImportService;
            if (importService == null)
                throw new InvalidOperationException("Không thể khởi tạo dịch vụ import.");

            var validate = await importService.ValidateImportFileAsync(filePath);
            if (!validate.Success || !validate.Data)
            {
                var reason = string.IsNullOrEmpty(validate.Message) ? "File không hợp lệ để import" : validate.Message;
                throw new InvalidOperationException(reason);
            }

            if (ext == ".xlsx" || ext == ".xls")
            {
                var templateImport = await importService.ImportOrdersFromExcelTemplateAsync(filePath);
                if (templateImport.Success)
                {
                    ShowImportSummary(templateImport.Data, templateImport.Message);
                    return;
                }
            }

            var fallbackImport = await importService.ImportOrdersFromFileAsync(filePath, EcoStationManagerApplication.Models.Enums.OrderSource.EXCEL);
            if (!fallbackImport.Success)
            {
                var msg = string.IsNullOrEmpty(fallbackImport.Message) ? "Phục hồi thất bại" : fallbackImport.Message;
                throw new InvalidOperationException(msg);
            }

            ShowImportSummary(fallbackImport.Data, fallbackImport.Message);
        }

        private async Task<List<OrderExportDTO>> GetOrderExportDataAsync()
        {
            var data = await _exportService.GetOrdersForExportAsync(null, null, null);
            return data?.OrderByDescending(o => o.NgayTao).ToList() ?? new List<OrderExportDTO>();
        }

        private System.Data.DataTable BuildCustomersDataTable(List<EcoStationManagerApplication.Models.Entities.Customer> customers)
        {
            var table = new System.Data.DataTable();
            table.Columns.Add("CustomerId", typeof(int));
            table.Columns.Add("CustomerCode", typeof(string));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Phone", typeof(string));
            table.Columns.Add("TotalPoint", typeof(int));
            table.Columns.Add("Rank", typeof(string));
            table.Columns.Add("IsActive", typeof(string));
            table.Columns.Add("CreatedDate", typeof(DateTime));

            foreach (var c in customers)
            {
                table.Rows.Add(c.CustomerId, c.CustomerCode, c.Name, c.Phone, c.TotalPoint, c.Rank.ToString(), c.IsActive.ToString(), c.CreatedDate);
            }
            return table;
        }

        private System.Data.DataTable BuildCustomerTransactionsDataTable(List<EcoStationManagerApplication.Models.Entities.PackagingTransaction> transactions)
        {
            var table = new System.Data.DataTable();
            table.Columns.Add("TransactionId", typeof(int));
            table.Columns.Add("CustomerId", typeof(int));
            table.Columns.Add("CustomerName", typeof(string));
            table.Columns.Add("PackagingId", typeof(int));
            table.Columns.Add("PackagingName", typeof(string));
            table.Columns.Add("Type", typeof(string));
            table.Columns.Add("Quantity", typeof(int));
            table.Columns.Add("DepositPrice", typeof(decimal));
            table.Columns.Add("RefundAmount", typeof(decimal));
            table.Columns.Add("Notes", typeof(string));
            table.Columns.Add("CreatedDate", typeof(DateTime));

            foreach (var t in transactions.OrderByDescending(x => x.CreatedDate))
            {
                var customerName = t.Customer?.Name ?? "";
                var packagingName = t.Packaging?.Name ?? "";
                table.Rows.Add(t.TransactionId, t.CustomerId ?? 0, customerName, t.PackagingId, packagingName, t.Type.ToString(), t.Quantity, t.DepositPrice, t.RefundAmount, t.Notes, t.CreatedDate);
            }
            return table;
        }

        private System.Data.DataTable BuildProductInventoryDataTable(List<EcoStationManagerApplication.Models.Entities.Inventory> inventory, List<EcoStationManagerApplication.Models.Entities.Product> products)
        {
            var table = new System.Data.DataTable();
            table.Columns.Add("InventoryId", typeof(int));
            table.Columns.Add("ProductId", typeof(int));
            table.Columns.Add("ProductName", typeof(string));
            table.Columns.Add("BatchNo", typeof(string));
            table.Columns.Add("Quantity", typeof(decimal));
            table.Columns.Add("ExpiryDate", typeof(DateTime));
            table.Columns.Add("LastUpdated", typeof(DateTime));

            var productDict = products?.ToDictionary(p => p.ProductId, p => p) ?? new Dictionary<int, EcoStationManagerApplication.Models.Entities.Product>();
            foreach (var i in inventory.OrderByDescending(x => x.LastUpdated))
            {
                string productName = i.Product?.Name ?? i.ProductName;
                if (string.IsNullOrWhiteSpace(productName) && i.ProductId > 0)
                {
                    if (productDict.TryGetValue(i.ProductId, out var pr))
                        productName = pr?.Name ?? string.Empty;
                }
                table.Rows.Add(i.InventoryId, i.ProductId, productName, i.BatchNo, i.Quantity, i.ExpiryDate.HasValue ? i.ExpiryDate.Value : (object)DBNull.Value, i.LastUpdated);
            }
            return table;
        }

        private System.Data.DataTable BuildPackagingDataTable(List<EcoStationManagerApplication.Models.Entities.Packaging> packaging)
        {
            var table = new System.Data.DataTable();
            table.Columns.Add("PackagingId", typeof(int));
            table.Columns.Add("Barcode", typeof(string));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Type", typeof(string));
            table.Columns.Add("DepositPrice", typeof(decimal));

            foreach (var p in packaging.OrderBy(x => x.Name))
            {
                table.Rows.Add(p.PackagingId, p.Barcode, p.Name, p.Type, p.DepositPrice);
            }
            return table;
        }

        private System.Data.DataTable BuildProductsDataTable(List<EcoStationManagerApplication.Models.Entities.Product> products)
        {
            var table = new System.Data.DataTable();
            table.Columns.Add("ProductId", typeof(int));
            table.Columns.Add("Sku", typeof(string));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Unit", typeof(string));
            table.Columns.Add("Price", typeof(decimal));
            table.Columns.Add("MinStockLevel", typeof(decimal));
            table.Columns.Add("CategoryId", typeof(int));
            table.Columns.Add("IsActive", typeof(string));
            table.Columns.Add("CreatedDate", typeof(DateTime));

            foreach (var pr in products.OrderBy(x => x.Name))
            {
                table.Rows.Add(pr.ProductId, pr.Sku, pr.Name, pr.Unit, pr.Price, pr.MinStockLevel, pr.CategoryId ?? 0, pr.IsActive.ToString(), pr.CreatedDate);
            }
            return table;
        }

        private Dictionary<string, string> GetCustomerColumnHeaders() => new Dictionary<string, string>
        {
            { "CustomerId", "ID" },
            { "CustomerCode", "Mã KH" },
            { "Name", "Tên" },
            { "Phone", "Điện thoại" },
            { "TotalPoint", "Điểm" },
            { "Rank", "Hạng" },
            { "IsActive", "Trạng thái" },
            { "CreatedDate", "Ngày tạo" }
        };

        private Dictionary<string, string> GetCustomerTransactionHeaders() => new Dictionary<string, string>
        {
            { "TransactionId", "Mã GD" },
            { "CustomerId", "ID KH" },
            { "CustomerName", "Khách hàng" },
            { "PackagingId", "ID Bao bì" },
            { "PackagingName", "Bao bì" },
            { "Type", "Loại" },
            { "Quantity", "Số lượng" },
            { "DepositPrice", "Giá ký quỹ" },
            { "RefundAmount", "Hoàn tiền" },
            { "Notes", "Ghi chú" },
            { "CreatedDate", "Ngày" }
        };

        private Dictionary<string, string> GetInventoryColumnHeaders() => new Dictionary<string, string>
        {
            { "InventoryId", "ID" },
            { "ProductId", "ID SP" },
            { "ProductName", "Sản phẩm" },
            { "BatchNo", "Mã lô" },
            { "Quantity", "Số lượng" },
            { "ExpiryDate", "HSD" },
            { "LastUpdated", "Cập nhật" }
        };

        private Dictionary<string, string> GetPackagingColumnHeaders() => new Dictionary<string, string>
        {
            { "PackagingId", "ID" },
            { "Barcode", "Barcode" },
            { "Name", "Tên" },
            { "Type", "Loại" },
            { "DepositPrice", "Giá ký quỹ" }
        };

        private Dictionary<string, string> GetProductColumnHeaders() => new Dictionary<string, string>
        {
            { "ProductId", "ID" },
            { "Sku", "SKU" },
            { "Name", "Tên" },
            { "Unit", "Đơn vị" },
            { "Price", "Giá" },
            { "MinStockLevel", "Tồn tối thiểu" },
            { "CategoryId", "Danh mục" },
            { "IsActive", "Trạng thái" },
            { "CreatedDate", "Ngày tạo" }
        };

        private Dictionary<string, string> GetOrderColumnHeaders() => new Dictionary<string, string>
        {
            { "STT", "STT" },
            { "MaDon", "Mã đơn" },
            { "KhachHang", "Khách hàng" },
            { "Nguon", "Nguồn" },
            { "TrangThai", "Trạng thái" },
            { "TongTien", "Tổng tiền" },
            { "ThanhToan", "Thanh toán" },
            { "NgayTao", "Ngày tạo" }
        };

        private DataTable BuildOrdersDataTable(List<OrderExportDTO> orders)
        {
            var table = new DataTable();
            table.Columns.Add("STT", typeof(int));
            table.Columns.Add("MaDon", typeof(string));
            table.Columns.Add("KhachHang", typeof(string));
            table.Columns.Add("Nguon", typeof(string));
            table.Columns.Add("TrangThai", typeof(string));
            table.Columns.Add("TongTien", typeof(decimal));
            table.Columns.Add("ThanhToan", typeof(string));
            table.Columns.Add("NgayTao", typeof(string));

            int stt = 1;
            foreach (var o in orders.OrderByDescending(x => x.NgayTao))
            {
                table.Rows.Add(
                    stt++,
                    o.MaDon,
                    o.KhachHang,
                    o.Nguon,
                    o.TrangThai,
                    o.TongTien,
                    o.ThanhToan,
                    o.NgayTao.ToString("dd/MM/yyyy HH:mm")
                );
            }
            return table;
        }

        private IList<byte[]> GenerateBackupCharts(List<OrderExportDTO> orders)
        {
            var charts = new List<byte[]>();

            var byStatus = orders
                .GroupBy(x => string.IsNullOrWhiteSpace(x.TrangThai) ? "Khác" : x.TrangThai)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToList();

            charts.Add(RenderChartImage(seriesConfig: s =>
            {
                s.ChartType = SeriesChartType.Column;
                s.IsValueShownAsLabel = true;
                foreach (var item in byStatus)
                    s.Points.AddXY(item.Status, item.Count);
            }, title: "Số đơn theo trạng thái"));

            var byDayRevenue = orders
                .GroupBy(x => x.NgayTao.Date)
                .Select(g => new { Day = g.Key, Revenue = g.Sum(i => i.TongTien) })
                .OrderBy(x => x.Day)
                .ToList();

            charts.Add(RenderChartImage(seriesConfig: s =>
            {
                s.ChartType = SeriesChartType.Line;
                s.BorderWidth = 2;
                foreach (var item in byDayRevenue)
                    s.Points.AddXY(item.Day.ToString("dd/MM"), (double)item.Revenue);
            }, title: "Doanh thu theo ngày"));

            return charts;
        }

        private byte[] RenderChartImage(Action<Series> seriesConfig, string title)
        {
            var chart = new Chart();
            chart.Width = 800;
            chart.Height = 400;
            var area = new ChartArea("a");
            area.AxisX.Interval = 1;
            chart.ChartAreas.Add(area);
            var s = new Series("data");
            seriesConfig(s);
            chart.Series.Add(s);
            chart.Titles.Add(title);
            using (var bmp = new Bitmap(chart.Width, chart.Height))
            {
                chart.DrawToBitmap(bmp, new Rectangle(0, 0, chart.Width, chart.Height));
                using (var ms = new MemoryStream())
                {
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    return ms.ToArray();
                }
            }
        }

        private void ValidateBackupPath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Đường dẫn file sao lưu không hợp lệ.", nameof(filePath));

            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private void UpdateLastBackupTime()
        {
            _lastBackupTime = DateTime.Now;
        }

        private void ShowImportSummary(ImportResult result, string message)
        {
            var summary = $"{message}\n\nThành công: {result.SuccessCount}\nLỗi: {result.ErrorCount}";
            if (result.Errors != null && result.Errors.Count > 0)
            {
                var firstErrors = string.Join("\n", result.Errors.Take(5));
                summary += $"\n\nMột số lỗi:\n{firstErrors}";
            }
            ShowSuccessMessage(summary);
        }
        #endregion

        #region Public Methods
        public async Task PerformAutoBackupAsync()
        {
            try
            {
                string autoBackupDirectory = @"C:\EcoStationBackups";
                Directory.CreateDirectory(autoBackupDirectory);

                string autoBackupPath = Path.Combine(autoBackupDirectory, $"AutoBackup_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
                await BackupExcelDataAsync(autoBackupPath);
            }
            catch (Exception ex)
            {
            }
        }

        public string GetLastBackupInfo()
        {
            return _lastBackupTime.HasValue
                ? $"Last backup: {_lastBackupTime:dd/MM/yyyy HH:mm}"
                : "Chưa có bản sao lưu nào.";
        }

        public void SetRestoreFilePath(string filePath)
        {
            if (txtRestoreFile != null && !string.IsNullOrEmpty(filePath))
            {
                txtRestoreFile.Text = filePath;
            }
        }
        #endregion

        #region Private Helpers
        private async Task RunWithProcessingStateAsync(Func<Task> operation)
        {
            SetProcessingState(true);
            try
            {
                await operation();
            }
            finally
            {
                SetProcessingState(false);
            }
        }

        private void SetProcessingState(bool isProcessing)
        {
            _isProcessing = isProcessing;
            UseWaitCursor = isProcessing;

            if (btnBackupExcel != null) btnBackupExcel.Enabled = !isProcessing;
            if (btnBackupPDF != null) btnBackupPDF.Enabled = !isProcessing;
            if (btnRestore != null) btnRestore.Enabled = !isProcessing;
            if (browseButton != null) browseButton.Enabled = !isProcessing;
        }
        #endregion

        private async Task<System.Data.DataTable> BuildOrderDetailsDataTableAsync(List<OrderDTO> orders, List<EcoStationManagerApplication.Models.Entities.Product> products)
        {
            var table = new System.Data.DataTable();
            table.Columns.Add("OrderId", typeof(int));
            table.Columns.Add("OrderCode", typeof(string));
            table.Columns.Add("ProductId", typeof(int));
            table.Columns.Add("ProductName", typeof(string));
            table.Columns.Add("SKU", typeof(string));
            table.Columns.Add("Unit", typeof(string));
            table.Columns.Add("Quantity", typeof(decimal));
            table.Columns.Add("UnitPrice", typeof(decimal));
            table.Columns.Add("LineTotal", typeof(decimal));

            var productDict = products?.ToDictionary(p => p.ProductId) ?? new Dictionary<int, EcoStationManagerApplication.Models.Entities.Product>();

            foreach (var o in orders)
            {
                var detailsRes = await _orderDetailService.GetOrderDetailsByOrderAsync(o.OrderId);
                if (!detailsRes.Success || detailsRes.Data == null) continue;
                foreach (var d in detailsRes.Data)
                {
                    productDict.TryGetValue(d.ProductId, out var pr);
                    var name = pr?.Name ?? string.Empty;
                    var sku = pr?.Sku ?? string.Empty;
                    var unit = pr?.Unit ?? string.Empty;
                    var lineTotal = d.Quantity * d.UnitPrice;
                    table.Rows.Add(o.OrderId, o.OrderCode, d.ProductId, name, sku, unit, d.Quantity, d.UnitPrice, lineTotal);
                }
            }
            return table;
        }

        private System.Data.DataTable BuildPackagingInventoryDataTable(List<EcoStationManagerApplication.Models.Entities.PackagingInventory> inventories, List<EcoStationManagerApplication.Models.Entities.Packaging> packagings)
        {
            var table = new System.Data.DataTable();
            table.Columns.Add("PkInvId", typeof(int));
            table.Columns.Add("PackagingId", typeof(int));
            table.Columns.Add("PackagingName", typeof(string));
            table.Columns.Add("PackagingBarcode", typeof(string));
            table.Columns.Add("QtyNew", typeof(int));
            table.Columns.Add("QtyInUse", typeof(int));
            table.Columns.Add("QtyReturned", typeof(int));
            table.Columns.Add("QtyNeedCleaning", typeof(int));
            table.Columns.Add("QtyCleaned", typeof(int));
            table.Columns.Add("QtyDamaged", typeof(int));
            table.Columns.Add("LastUpdated", typeof(DateTime));

            var pDict = packagings?.ToDictionary(p => p.PackagingId) ?? new Dictionary<int, EcoStationManagerApplication.Models.Entities.Packaging>();
            foreach (var inv in inventories.OrderByDescending(x => x.LastUpdated))
            {
                pDict.TryGetValue(inv.PackagingId, out var p);
                table.Rows.Add(inv.PkInvId, inv.PackagingId, p?.Name ?? string.Empty, p?.Barcode ?? string.Empty,
                    inv.QtyNew, inv.QtyInUse, inv.QtyReturned, inv.QtyNeedCleaning, inv.QtyCleaned, inv.QtyDamaged, inv.LastUpdated);
            }
            return table;
        }

        private Dictionary<string, string> GetOrderDetailColumnHeaders() => new Dictionary<string, string>
        {
            { "OrderId", "ID Đơn" },
            { "OrderCode", "Mã đơn" },
            { "ProductId", "ID SP" },
            { "ProductName", "Sản phẩm" },
            { "SKU", "SKU" },
            { "Unit", "Đơn vị" },
            { "Quantity", "Số lượng" },
            { "UnitPrice", "Đơn giá" },
            { "LineTotal", "Thành tiền" }
        };

        private Dictionary<string, string> GetPackagingInventoryColumnHeaders() => new Dictionary<string, string>
        {
            { "PkInvId", "ID" },
            { "PackagingId", "ID Bao bì" },
            { "PackagingName", "Bao bì" },
            { "PackagingBarcode", "Barcode" },
            { "QtyNew", "Mới" },
            { "QtyInUse", "Đang dùng" },
            { "QtyReturned", "Đã thu hồi" },
            { "QtyNeedCleaning", "Cần vệ sinh" },
            { "QtyCleaned", "Đã vệ sinh" },
            { "QtyDamaged", "Hỏng" },
            { "LastUpdated", "Cập nhật" }
        };

    }
}
