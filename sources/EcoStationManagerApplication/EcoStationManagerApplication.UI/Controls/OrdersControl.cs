using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.Models.Results;
using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.UI.Common;
using EcoStationManagerApplication.UI.Forms;
using EcoStationManagerApplication.Common.Exporters;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace EcoStationManagerApplication.UI.Controls
{
    public partial class OrdersControl : UserControl, IRefreshableControl
    {
        private string _currentFilter = "all";
        private bool _isLoading = false;
        private System.Threading.Timer _searchTimer;
        private string _lastSearchText = "";
        private DateTime? _dateFilterStart = null;
        private DateTime? _dateFilterEnd = null;
        private string _currentDateFilter = null; // "today", "week", "month", "range", null

        // Exporters từ tầng Common
        private readonly IExcelExporter _excelExporter;
        private readonly IPdfExporter _pdfExporter;

        public OrdersControl()
        {
            InitializeComponent();

            // Khởi tạo exporters
            _excelExporter = new ExcelExporter();
            _pdfExporter = new PdfExporter();

            SetupDataGridStyle(dgvOrders);

            PopulateTabPanel();
            InitializeDataGridColumns();
            _ = LoadOrdersAsync();
            InitializeEvents();
        }

        public void RefreshData()
        {
            _ = LoadOrdersAsync(_currentFilter);
        }

        // Gán tất cả sự kiện ở đây
        private void InitializeEvents()
        {
            if (btnExportPDF != null)
                btnExportPDF.Click += btnExportPDF_Click;

            if (btnExportExcel != null)
                btnExportExcel.Click += btnExportExcel_Click;

            if (btnImportExcel != null)
                btnImportExcel.Click += btnImportExcel_Click;

            if (btnAddOrder != null)
                btnAddOrder.Click += btnAddOrder_Click;

            if (dgvOrders != null)
            {
                //dgvOrders.CellContentClick += dgvOrders_CellContentClick;
                //dgvOrders.CellFormatting += dgvOrders_CellFormatting;
            }

            if (txtSearch != null)
            {
                txtSearch.TextChanged += txtOrderSearch_TextChanged;
            }

            // Date filter events
            if (btnFilterToday != null)
                btnFilterToday.Click += btnFilterToday_Click;
            if (btnFilterThisWeek != null)
                btnFilterThisWeek.Click += btnFilterThisWeek_Click;
            if (btnFilterThisMonth != null)
                btnFilterThisMonth.Click += btnFilterThisMonth_Click;
            if (btnFilterDateRange != null)
                btnFilterDateRange.Click += btnFilterDateRange_Click;
            if (dtpStartDate != null)
                dtpStartDate.ValueChanged += dtpStartDate_ValueChanged;
            if (dtpEndDate != null)
                dtpEndDate.ValueChanged += dtpEndDate_ValueChanged;
        }

        // Đổ các nút Tab vào FlowLayoutPanel
        private void PopulateTabPanel()
        {
            if (tabPanel == null) return;

            // Tạo mảng các tab với tag tương ứng
            var tabs = new[]
            {
            new { Text = "Tất cả", Tag = "all" },
            new { Text = "Xử lí", Tag = "processing" },
            new { Text = "Đơn online", Tag = "online" },
            new { Text = "Đơn offline", Tag = "offline" },
            new { Text = "Mới", Tag = "new" },
            new { Text = "Chuẩn bị", Tag = "ready" },
            new { Text = "Đang giao", Tag = "shipping" },
            new { Text = "Hoàn thành", Tag = "completed" }
        };

            foreach (var tab in tabs)
            {
                var tabButton = new Button();
                tabButton.Text = tab.Text;
                tabButton.Tag = tab.Tag; // Gán tag
                tabButton.Size = new Size(100, 34);
                tabButton.Margin = new Padding(3);
                tabButton.FlatStyle = FlatStyle.Flat;
                tabButton.FlatAppearance.BorderSize = 0;
                tabButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

                // Nút đầu tiên (Tất cả) được chọn
                if (tab.Tag.ToString() == "all")
                {
                    tabButton.BackColor = Color.FromArgb(46, 125, 50);
                    tabButton.ForeColor = Color.White;
                }
                else
                {
                    tabButton.BackColor = Color.White;
                    tabButton.ForeColor = Color.Black;
                }

                tabButton.Click += contentTab_Click;
                tabPanel.Controls.Add(tabButton);
            }
        }

        // Xử lý khi nhấn vào Tab
        private async void contentTab_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                // Reset tất cả các nút
                foreach (Control control in tabPanel.Controls)
                {
                    if (control is Button btn)
                    {
                        btn.BackColor = Color.White;
                        btn.ForeColor = Color.Black;
                    }
                }
                // Highlight nút được chọn
                button.BackColor = Color.FromArgb(46, 125, 50);
                button.ForeColor = Color.White;

                // Lấy tag và gọi LoadOrdersAsync với tag đó
                string filterTag = button.Tag?.ToString() ?? "all";
                _currentFilter = filterTag; // Lưu filter hiện tại
                await LoadOrdersAsync(filterTag);
            }
        }

        // Thêm cột vào DataGridView
        private void InitializeDataGridColumns()
        {
            if (dgvOrders == null) return;

            var columns = new[]
            {
                new { Name = "OrderCode", Header = "Mã đơn" },
                new { Name = "Customer", Header = "Khách hàng" },
                new { Name = "Phone", Header = "SĐT" },
                new { Name = "Source", Header = "Nguồn" },
                new { Name = "TotalAmount", Header = "Tổng tiền" },
                new { Name = "Status", Header = "Trạng thái" },
                new { Name = "CreatedDate", Header = "Ngày tạo" },
            };

            foreach (var col in columns)
            {
                dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = col.Name,
                    HeaderText = col.Header,
                    ReadOnly = true,
                    DefaultCellStyle = (col.Name == "TotalAmount" || col.Name == "Quantity")
                        ? new DataGridViewCellStyle { Format = "N0" }
                        : null
                });
            }

            dgvOrders.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colDetail",
                HeaderText = "Chi tiết",
                Text = "Chi tiết",
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgvOrders.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colUpdate",
                HeaderText = "Cập nhật",
                Text = "Cập nhật",
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgvOrders.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colDelete",
                HeaderText = "Xóa",
                Text = "Xóa",
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(244, 67, 54),
                    SelectionForeColor = Color.White,
                    SelectionBackColor = Color.FromArgb(198, 40, 40)
                }
            });

            // Tự co giãn cột
            dgvOrders.Columns["Customer"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        // Thêm dữ liệu 
        private async Task LoadOrdersAsync(string filterTag = "all")
        {
            // Tránh load nhiều lần cùng lúc
            if (_isLoading)
                return;

            try
            {
                _isLoading = true;
                ShowLoading(true);
                dgvOrders.Rows.Clear();

                // Lấy danh sách đơn hàng
                Result<IEnumerable<OrderDTO>> ordersResult;

                if (filterTag == "processing")
                {
                    // Lấy đơn hàng đang xử lý
                    ordersResult = await AppServices.OrderService.GetProcessingOrdersAsync();
                }
                else
                {
                    // Lấy tất cả đơn hàng cho các tab khác
                    ordersResult = await AppServices.OrderService.GetAllAsync();
                }

                if (!ordersResult.Success || !ordersResult.Data.Any())
                {
                    dgvOrders.Rows.Add("", "Không có đơn hàng nào", "", "", "", "", "", "");
                    return;
                }

                var orders = ordersResult.Data.ToList();

                // Lọc đơn hàng dựa trên filterTag
                if (filterTag != "all" && filterTag != "processing")
                {
                    orders = orders.Where(order =>
                    {
                        switch (filterTag)
                        {
                            case "online":
                                // Lọc đơn online: các nguồn EXCEL, EMAIL, GOOGLEFORM
                                return order.Source == OrderSource.EXCEL ||
                                       order.Source == OrderSource.EMAIL ||
                                       order.Source == OrderSource.GOOGLEFORM;
                            case "offline":
                                // Lọc đơn offline: nguồn MANUAL
                                return order.Source == OrderSource.MANUAL;
                            case "new":
                                return order.Status == OrderStatus.CONFIRMED;
                            case "ready":
                                return order.Status == OrderStatus.READY;
                            case "shipping":
                                return order.Status == OrderStatus.SHIPPED;
                            case "completed":
                                return order.Status == OrderStatus.COMPLETED;
                            default:
                                return true;
                        }
                    }).ToList();
                }

                // Áp dụng bộ lọc ngày nếu có
                if (_dateFilterStart.HasValue || _dateFilterEnd.HasValue)
                {
                    orders = orders.Where(order =>
                    {
                        var orderDate = order.LastUpdated.Date;
                        bool matches = true;

                        if (_dateFilterStart.HasValue)
                            matches = matches && orderDate >= _dateFilterStart.Value.Date;
                        if (_dateFilterEnd.HasValue)
                            matches = matches && orderDate <= _dateFilterEnd.Value.Date;

                        return matches;
                    }).ToList();
                }

                orders = orders.Take(50).ToList(); // Giới hạn hiển thị

                // Populate DataGridView
                foreach (var order in orders)
                {
                    string customerName = "Khách lẻ";
                    string customerPhone = "";

                    if (order.CustomerId.HasValue)
                    {
                        var customerResult = await AppServices.CustomerService.GetCustomerByIdAsync(order.CustomerId.Value);
                        if (customerResult.Success && customerResult.Data != null)
                        {
                            customerName = customerResult.Data.Name ?? "Khách lẻ";
                            customerPhone = customerResult.Data.Phone ?? "";
                        }
                    }

                    // Tính tổng tiền sau khi trừ discount (TotalAmount đã được tính = sum - discount)
                    // Nhưng để hiển thị đúng, ta cần kiểm tra xem TotalAmount đã trừ discount chưa
                    // Dựa vào code, TotalAmount trong OrderDTO đã là giá trị sau khi trừ discount
                    var displayAmount = order.TotalAmount;
                    if (displayAmount < 0) displayAmount = 0;
                    
                    var row = dgvOrders.Rows[dgvOrders.Rows.Add(
                        order.OrderCode ?? $"ORD-{order.OrderId:D5}",
                        customerName,
                        customerPhone,
                        GetOrderSourceDisplay(order.Source),
                        displayAmount.ToString("N0"),
                        GetOrderStatusDisplay(order.Status),
                        order.LastUpdated.ToString("dd/MM/yyyy HH:mm")
                    )];
                    // Lưu OrderId vào Tag của row để sử dụng khi mở form chi tiết
                    row.Tag = order.OrderId;
                }
            }
            catch (Exception ex)
            {
                dgvOrders.Rows.Add("", "Lỗi tải dữ liệu", "", "", "", "", "", "");
                Console.WriteLine($"Lỗi tải đơn hàng: {ex.Message}");
            }
            finally
            {
                _isLoading = false;
                ShowLoading(false);
            }
        }

        // Helper methods giữ nguyên
        private string GetOrderSourceDisplay(OrderSource source)
        {
            switch (source)
            {
                case OrderSource.MANUAL:
                    return "Thủ công";
                case OrderSource.GOOGLEFORM:
                    return "Google Form";
                case OrderSource.EXCEL:
                    return "Excel";
                case OrderSource.EMAIL:
                    return "Email";
                default:
                    return "Khác";
            }
        }

        private string GetOrderStatusDisplay(OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.DRAFT:
                    return "Nháp";
                case OrderStatus.CONFIRMED:
                    return "Mới";
                case OrderStatus.PROCESSING:
                    return "Đang xử lý";
                case OrderStatus.READY:
                    return "Chuẩn bị";
                case OrderStatus.SHIPPED:
                    return "Đang giao";
                case OrderStatus.COMPLETED:
                    return "Hoàn thành";
                case OrderStatus.CANCELLED:
                    return "Đã hủy";
                default:
                    return "Không xác định";
            }
        }

        private void ShowLoading(bool show)
        {
            Cursor = show ? Cursors.WaitCursor : Cursors.Default;
        }

        // --- HÀM XỬ LÝ SỰ KIỆN ---

        private async void btnExportPDF_Click(object sender, EventArgs e)
        {
            try
            {
                var saveDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    FileName = $"DanhSachDonHang_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
                    Title = "Xuất danh sách đơn hàng ra PDF"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ShowLoading(true);

                    // Tầng BLL: Lấy dữ liệu đã được chuẩn bị
                    var exportData = await AppServices.ExportService.GetOrdersForExportAsync(_currentFilter);

                    if (exportData == null || !exportData.Any())
                    {
                        MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // Tầng Common: Export ra PDF
                    var headers = new Dictionary<string, string>
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

                    try
                    {
                        _pdfExporter.ExportToPdf(exportData, saveDialog.FileName, "DANH SÁCH ĐƠN HÀNG", headers);

                        ShowLoading(false);
                        MessageBox.Show($"Đã xuất PDF thành công!\nFile: {saveDialog.FileName}",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception exportEx)
                    {
                        ShowLoading(false);
                        MessageBox.Show($"Lỗi trong quá trình xuất PDF: {exportEx.Message}\n\nChi tiết: {exportEx.StackTrace}",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowLoading(false);
                MessageBox.Show($"Lỗi khi xuất PDF: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                var saveDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    FileName = $"DanhSachDonHang_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                    Title = "Xuất danh sách đơn hàng ra Excel"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ShowLoading(true);

                    // Tầng BLL: Lấy dữ liệu đã được chuẩn bị
                    var exportData = await AppServices.ExportService.GetOrdersForExportAsync(_currentFilter);

                    if (exportData == null || !exportData.Any())
                    {
                        MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // Tầng Common: Export ra Excel
                    var headers = new Dictionary<string, string>
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

                    _excelExporter.ExportToExcel(exportData, saveDialog.FileName, "Danh sách đơn hàng", headers);

                    ShowLoading(false);
                    MessageBox.Show($"Đã xuất Excel thành công!\nFile: {saveDialog.FileName}",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                ShowLoading(false);
                MessageBox.Show($"Lỗi khi xuất Excel: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnImportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                using (var openDialog = new OpenFileDialog())
                {
                    openDialog.Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls|All files (*.*)|*.*";
                    openDialog.Title = "Chọn file Excel để import đơn hàng";
                    openDialog.Multiselect = false;

                    if (openDialog.ShowDialog() == DialogResult.OK)
                    {
                        ShowLoading(true);
                        btnImportExcel.Enabled = false;

                        try
                        {
                            // Gọi service import
                            var result = await AppServices.ImportService.ImportOrdersFromExcelTemplateAsync(openDialog.FileName);

                            ShowLoading(false);
                            btnImportExcel.Enabled = true;

                            if (result.Success && result.Data != null)
                            {
                                var importResult = result.Data;
                                string message = $"Import hoàn tất!\n\n" +
                                    $"✓ Thành công: {importResult.SuccessCount} đơn hàng\n" +
                                    $"✗ Lỗi: {importResult.ErrorCount}";

                                if (importResult.CreatedCustomerIds.Count > 0)
                                {
                                    message += $"\n✓ Đã tạo mới: {importResult.CreatedCustomerIds.Count} khách hàng";
                                }

                                if (importResult.Errors.Count > 0)
                                {
                                    message += "\n\nChi tiết lỗi:\n" + string.Join("\n", importResult.Errors.Take(10));
                                    if (importResult.Errors.Count > 10)
                                    {
                                        message += $"\n... và {importResult.Errors.Count - 10} lỗi khác";
                                    }
                                }

                                MessageBox.Show(message, "Kết quả Import",
                                    MessageBoxButtons.OK,
                                    importResult.ErrorCount > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

                                // Refresh danh sách đơn hàng
                                if (importResult.SuccessCount > 0)
                                {
                                    await LoadOrdersAsync(_currentFilter);
                                }
                            }
                            else
                            {
                                MessageBox.Show($"Lỗi khi import: {result.Message}", "Lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            ShowLoading(false);
                            btnImportExcel.Enabled = true;
                            MessageBox.Show($"Lỗi khi xử lý file: {ex.Message}", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowLoading(false);
                btnImportExcel.Enabled = true;
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnAddOrder_Click(object sender, EventArgs e)
        {
            using (var addOrderForm = new AddOrderForm())
            {
                if (addOrderForm.ShowDialog() == DialogResult.OK)
                {
                    // Đơn hàng đã được tạo thành công, refresh danh sách
                    await LoadOrdersAsync(_currentFilter);
                }
            }
        }


        // Xử lý khi nhấn nút trên DataGridView
        private void dgvOrders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string colName = dgvOrders.Columns[e.ColumnIndex].Name;
            var row = dgvOrders.Rows[e.RowIndex];

            if (colName == "colDetail")
            {
                // Lấy OrderId từ Tag của row
                if (row.Tag is int orderId)
                {
                    OpenOrderDetailForm(orderId);
                }
                else
                {
                    MessageBox.Show("Không thể lấy thông tin đơn hàng!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (colName == "colUpdate")
            {
                // Lấy OrderId từ Tag của row
                if (row.Tag is int orderId)
                {
                    OpenUpdateOrderForm(orderId);
                }
                else
                {
                    MessageBox.Show("Không thể lấy thông tin đơn hàng!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (colName == "colDelete")
            {
                // Lấy OrderId từ Tag của row
                if (row.Tag is int orderId)
                {
                    DeleteOrder(orderId);
                }
                else
                {
                    MessageBox.Show("Không thể lấy thông tin đơn hàng!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void OpenOrderDetailForm(int orderId)
        {
            try
            {
                // Tìm MainForm để sử dụng FormHelper
                Form mainForm = this.FindForm();
                while (mainForm != null && !(mainForm is MainForm))
                {
                    mainForm = mainForm.ParentForm ?? mainForm.Owner;
                }

                // Mở OrderDetailForm
                using (var orderDetailForm = new OrderDetailForm(orderId))
                {
                    DialogResult result;
                    if (mainForm != null)
                    {
                        result = FormHelper.ShowModalWithDim(mainForm, orderDetailForm);
                    }
                    else
                    {
                        result = orderDetailForm.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở chi tiết đơn hàng: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void OpenUpdateOrderForm(int orderId)
        {
            try
            {
                // Tìm MainForm để sử dụng FormHelper
                Form mainForm = this.FindForm();
                while (mainForm != null && !(mainForm is MainForm))
                {
                    mainForm = mainForm.ParentForm ?? mainForm.Owner;
                }

                // Mở UpdateOrderForm
                using (var updateOrderForm = new UpdateOrderForm(orderId))
                {
                    DialogResult result;
                    if (mainForm != null)
                    {
                        result = FormHelper.ShowModalWithDim(mainForm, updateOrderForm);
                    }
                    else
                    {
                        result = updateOrderForm.ShowDialog();
                    }

                    // Refresh danh sách nếu có thay đổi
                    if (result == DialogResult.OK)
                    {
                        await LoadOrdersAsync(_currentFilter);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form cập nhật đơn hàng: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void DeleteOrder(int orderId)
        {
            try
            {
                // Xác nhận xóa
                var confirmResult = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa đơn hàng #{orderId}?\n\n" +
                    "Hành động này không thể hoàn tác!",
                    "Xác nhận xóa đơn hàng",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (confirmResult != DialogResult.Yes)
                {
                    return;
                }

                ShowLoading(true);

                // Gọi service để xóa đơn hàng
                var result = await AppServices.OrderService.DeleteOrderAsync(orderId);

                ShowLoading(false);

                if (result.Success)
                {
                    MessageBox.Show(result.Message ?? "Đã xóa đơn hàng thành công", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh danh sách đơn hàng
                    await LoadOrdersAsync(_currentFilter);
                }
                else
                {
                    MessageBox.Show(result.Message ?? "Không thể xóa đơn hàng", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                ShowLoading(false);
                MessageBox.Show($"Lỗi khi xóa đơn hàng: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- HÀM HELPER (Hàm phụ trợ) ---

        // Hàm tô màu cho các ô
        private void dgvOrders_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            string colName = dgvOrders.Columns[e.ColumnIndex].Name;
            if (colName == "Status" || colName == "Type")
            {
                if (e.Value != null)
                {
                    string status = e.Value.ToString();
                    e.CellStyle.BackColor = GetBadgeColor(status);
                    e.CellStyle.ForeColor = Color.Black;
                    e.CellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        private Color GetBadgeColor(string status)
        {
            switch (status)
            {
                case "Online": return Color.FromArgb(187, 222, 251);
                case "Offline": return Color.FromArgb(224, 224, 224);
                case "Đang giao": return Color.FromArgb(200, 230, 201);
                case "Chuẩn bị": return Color.FromArgb(255, 249, 196);
                case "Mới": return Color.FromArgb(209, 196, 233); // Thêm màu
                case "Hoàn thành": return Color.FromArgb(232, 234, 237); // Thêm màu
                default: return Color.LightGray;
            }
        }

        // Hàm áp dụng style chung cho DataGridView
        private void SetupDataGridStyle(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(240, 240, 240);
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.RowHeadersVisible = false;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersHeight = 40;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 245, 255);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.RowTemplate.Height = 35;
        }

        private void dgvOrders_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtOrderSearch_TextChanged(object sender, EventArgs e)
        {
            // Debounce: Chờ 500ms sau khi người dùng ngừng gõ
            var searchText = txtSearch.Text?.Trim() ?? "";

            // Nếu text không thay đổi, không làm gì
            if (searchText == _lastSearchText)
                return;

            _lastSearchText = searchText;

            // Hủy timer cũ nếu có
            _searchTimer?.Dispose();

            // Nếu ô tìm kiếm trống, load lại danh sách với filter hiện tại
            if (string.IsNullOrEmpty(searchText))
            {
                _searchTimer = new System.Threading.Timer(async _ =>
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(async () => await LoadOrdersAsync(_currentFilter)));
                    }
                    else
                    {
                        await LoadOrdersAsync(_currentFilter);
                    }
                }, null, 300, Timeout.Infinite);
                return;
            }

            // Tìm kiếm theo tên và số điện thoại sau 500ms
            _searchTimer = new System.Threading.Timer(async _ =>
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(async () => await SearchOrdersAsync(searchText)));
                }
                else
                {
                    await SearchOrdersAsync(searchText);
                }
            }, null, 500, Timeout.Infinite);
        }

        private async Task SearchOrdersAsync(string searchText)
        {
            // Tránh search nhiều lần cùng lúc
            if (_isLoading)
                return;

            try
            {
                _isLoading = true;
                ShowLoading(true);
                dgvOrders.Rows.Clear();

                // Lấy danh sách đơn hàng
                Result<IEnumerable<OrderDTO>> ordersResult;

                if (_currentFilter == "processing")
                {
                    ordersResult = await AppServices.OrderService.GetProcessingOrdersAsync();
                }
                else
                {
                    // Lấy tất cả đơn hàng cho các tab khác
                    ordersResult = await AppServices.OrderService.GetAllAsync();
                }

                if (!ordersResult.Success || !ordersResult.Data.Any())
                {
                    dgvOrders.Rows.Add("", "Không có đơn hàng nào", "", "", "", "", "", "");
                    return;
                }

                var allOrders = ordersResult.Data.ToList();

                // Lọc theo filterTag trước
                if (_currentFilter != "all" && _currentFilter != "processing")
                {
                    allOrders = allOrders.Where(order =>
                    {
                        switch (_currentFilter)
                        {
                            case "online":
                                return order.Source == OrderSource.EXCEL ||
                                       order.Source == OrderSource.EMAIL ||
                                       order.Source == OrderSource.GOOGLEFORM;
                            case "offline":
                                return order.Source == OrderSource.MANUAL;
                            case "new":
                                return order.Status == OrderStatus.CONFIRMED;
                            case "ready":
                                return order.Status == OrderStatus.READY;
                            case "shipping":
                                return order.Status == OrderStatus.SHIPPED;
                            case "completed":
                                return order.Status == OrderStatus.COMPLETED;
                            default:
                                return true;
                        }
                    }).ToList();
                }

                // Áp dụng bộ lọc ngày nếu có
                if (_dateFilterStart.HasValue || _dateFilterEnd.HasValue)
                {
                    allOrders = allOrders.Where(order =>
                    {
                        var orderDate = order.LastUpdated.Date;
                        bool matches = true;

                        if (_dateFilterStart.HasValue)
                            matches = matches && orderDate >= _dateFilterStart.Value.Date;
                        if (_dateFilterEnd.HasValue)
                            matches = matches && orderDate <= _dateFilterEnd.Value.Date;

                        return matches;
                    }).ToList();
                }

                // Tìm kiếm theo tên và số điện thoại
                var searchLower = searchText.ToLower();
                var filteredOrders = new List<OrderDTO>();

                foreach (var order in allOrders)
                {
                    bool matches = false;

                    // Tìm kiếm theo tên khách hàng
                    if (order.CustomerId.HasValue)
                    {
                        var customerResult = await AppServices.CustomerService.GetCustomerByIdAsync(order.CustomerId.Value);
                        if (customerResult.Success && customerResult.Data != null)
                        {
                            var customer = customerResult.Data;
                            // Tìm theo tên
                            if (!string.IsNullOrEmpty(customer.Name) &&
                                customer.Name.ToLower().Contains(searchLower))
                            {
                                matches = true;
                            }
                            // Tìm theo số điện thoại
                            if (!string.IsNullOrEmpty(customer.Phone) &&
                                customer.Phone.Contains(searchText))
                            {
                                matches = true;
                            }
                        }
                    }

                    if (matches)
                    {
                        filteredOrders.Add(order);
                    }
                }

                if (!filteredOrders.Any())
                {
                    dgvOrders.Rows.Add("", "Không tìm thấy đơn hàng nào", "", "", "", "", "", "");
                    return;
                }

                // Populate DataGridView
                foreach (var order in filteredOrders.Take(50))
                {
                    string customerName = "Khách lẻ";
                    string customerPhone = "";

                    if (order.CustomerId.HasValue)
                    {
                        var customerResult = await AppServices.CustomerService.GetCustomerByIdAsync(order.CustomerId.Value);
                        if (customerResult.Success && customerResult.Data != null)
                        {
                            customerName = customerResult.Data.Name ?? "Khách lẻ";
                            customerPhone = customerResult.Data.Phone ?? "";
                        }
                    }

                    // Tính tổng tiền sau khi trừ discount
                    var displayAmount = order.TotalAmount;
                    if (displayAmount < 0) displayAmount = 0;
                    
                    var row = dgvOrders.Rows[dgvOrders.Rows.Add(
                        order.OrderCode ?? $"ORD-{order.OrderId:D5}",
                        customerName,
                        customerPhone,
                        GetOrderSourceDisplay(order.Source),
                        displayAmount.ToString("N0"),
                        GetOrderStatusDisplay(order.Status),
                        order.LastUpdated.ToString("dd/MM/yyyy HH:mm")
                    )];
                    row.Tag = order.OrderId;
                }
            }
            catch (Exception ex)
            {
                dgvOrders.Rows.Add("", "Lỗi tìm kiếm", "", "", "", "", "", "");
                Console.WriteLine($"Lỗi tìm kiếm đơn hàng: {ex.Message}");
            }
            finally
            {
                _isLoading = false;
                ShowLoading(false);
            }
        }

        private void headerPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void OrdersControl_Load(object sender, EventArgs e)
        {
            // Không cần load lại vì đã load trong constructor
        }

        // Date filter event handlers
        private async void btnFilterToday_Click(object sender, EventArgs e)
        {
            ResetDateFilterButtons();
            btnFilterToday.BackColor = Color.FromArgb(46, 125, 50);
            btnFilterToday.ForeColor = Color.White;

            var today = DateTime.Now.Date;
            _dateFilterStart = today;
            _dateFilterEnd = today;
            _currentDateFilter = "today";

            HideDateRangePickers();
            await LoadOrdersAsync(_currentFilter);
        }

        private async void btnFilterThisWeek_Click(object sender, EventArgs e)
        {
            ResetDateFilterButtons();
            btnFilterThisWeek.BackColor = Color.FromArgb(46, 125, 50);
            btnFilterThisWeek.ForeColor = Color.White;

            var today = DateTime.Now.Date;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            _dateFilterStart = startOfWeek;
            _dateFilterEnd = today;
            _currentDateFilter = "week";

            HideDateRangePickers();
            await LoadOrdersAsync(_currentFilter);
        }

        private async void btnFilterThisMonth_Click(object sender, EventArgs e)
        {
            ResetDateFilterButtons();
            btnFilterThisMonth.BackColor = Color.FromArgb(46, 125, 50);
            btnFilterThisMonth.ForeColor = Color.White;

            var today = DateTime.Now.Date;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            _dateFilterStart = startOfMonth;
            _dateFilterEnd = today;
            _currentDateFilter = "month";

            HideDateRangePickers();
            await LoadOrdersAsync(_currentFilter);
        }

        private void btnFilterDateRange_Click(object sender, EventArgs e)
        {
            ResetDateFilterButtons();
            btnFilterDateRange.BackColor = Color.FromArgb(46, 125, 50);
            btnFilterDateRange.ForeColor = Color.White;

            _currentDateFilter = "range";
            ShowDateRangePickers();
        }

        private async void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            if (_currentDateFilter == "range")
            {
                _dateFilterStart = dtpStartDate.Value.Date;
                if (_dateFilterEnd.HasValue && _dateFilterStart.Value > _dateFilterEnd.Value)
                {
                    _dateFilterEnd = _dateFilterStart.Value;
                    dtpEndDate.Value = _dateFilterEnd.Value;
                }
                await LoadOrdersAsync(_currentFilter);
            }
        }

        private async void dtpEndDate_ValueChanged(object sender, EventArgs e)
        {
            if (_currentDateFilter == "range")
            {
                _dateFilterEnd = dtpEndDate.Value.Date;
                if (_dateFilterStart.HasValue && _dateFilterEnd.Value < _dateFilterStart.Value)
                {
                    _dateFilterStart = _dateFilterEnd.Value;
                    dtpStartDate.Value = _dateFilterStart.Value;
                }
                await LoadOrdersAsync(_currentFilter);
            }
        }

        private void ResetDateFilterButtons()
        {
            btnFilterToday.BackColor = Color.White;
            btnFilterToday.ForeColor = Color.Black;
            btnFilterThisWeek.BackColor = Color.White;
            btnFilterThisWeek.ForeColor = Color.Black;
            btnFilterThisMonth.BackColor = Color.White;
            btnFilterThisMonth.ForeColor = Color.Black;
            btnFilterDateRange.BackColor = Color.White;
            btnFilterDateRange.ForeColor = Color.Black;
        }

        private void ShowDateRangePickers()
        {
            dtpStartDate.Visible = true;
            dtpEndDate.Visible = true;
            lblDateRange.Visible = true;
            lblDateRange.Text = "Từ:";

            if (!_dateFilterStart.HasValue)
            {
                dtpStartDate.Value = DateTime.Now.Date.AddDays(-7);
                _dateFilterStart = dtpStartDate.Value.Date;
            }
            else
            {
                dtpStartDate.Value = _dateFilterStart.Value;
            }

            if (!_dateFilterEnd.HasValue)
            {
                dtpEndDate.Value = DateTime.Now.Date;
                _dateFilterEnd = dtpEndDate.Value.Date;
            }
            else
            {
                dtpEndDate.Value = _dateFilterEnd.Value;
            }

            _ = LoadOrdersAsync(_currentFilter);
        }

        private void HideDateRangePickers()
        {
            dtpStartDate.Visible = false;
            dtpEndDate.Visible = false;
            lblDateRange.Visible = false;
        }
    }
}