using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.UI.Common;
using EcoStationManagerApplication.UI.Forms;
using EcoStationManagerApplication.Common.Exporters;
using EcoStationManagerApplication.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace EcoStationManagerApplication.UI.Controls
{
    public partial class OrdersControl : UserControl, IRefreshableControl
    {
        private string _currentFilter = "all";
        
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

            if (btnAddOrder != null)
                btnAddOrder.Click += btnAddOrder_Click;

            if (dgvOrders != null)
            {
                dgvOrders.CellContentClick += dgvOrders_CellContentClick;
                dgvOrders.CellFormatting += dgvOrders_CellFormatting;
            }
        }

        // Đổ các nút Tab vào FlowLayoutPanel
        private void PopulateTabPanel()
        {
            if (tabPanel == null) return;

            // Tạo mảng các tab với tag tương ứng
            var tabs = new[]
            {
            new { Text = "Tất cả", Tag = "all" },
            new { Text = "Đơn Online", Tag = "online" },
            new { Text = "Đơn Offline", Tag = "offline" },
            new { Text = "Mới", Tag = "new" },
            new { Text = "Chuẩn bị", Tag = "ready" },
            new { Text = "Đang giao", Tag = "shipping" },
            new { Text = "Hoàn thành", Tag = "completed" },
            new { Text = "Thu hồi bao bì", Tag = "return" }
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
                new { Name = "Product", Header = "Sản phẩm" },
                new { Name = "Type", Header = "Loại" },
                new { Name = "Quantity", Header = "Số lượng" },
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

            // Tự co giãn cột
            dgvOrders.Columns["Product"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvOrders.Columns["Customer"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        // Thêm dữ liệu 
        private async Task LoadOrdersAsync(string filterTag = "all")
        {
            try
            {
                ShowLoading(true);
                dgvOrders.Rows.Clear();

                // Lấy danh sách đơn hàng
                var ordersResult = await AppServices.OrderService.GetProcessingOrdersAsync();

                if (!ordersResult.Success || !ordersResult.Data.Any())
                {
                    dgvOrders.Rows.Add("", "Không có đơn hàng nào", "", "", "", "", "");
                    return;
                }

                var orders = ordersResult.Data;

                // Lọc đơn hàng dựa trên filterTag
                if (filterTag != "all")
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
                            case "return":
                                // Bỏ qua lọc cho thu hồi bao bì, hoặc có thể xử lý sau
                                return false; // Tạm thời không hiển thị gì
                            default:
                                return true;
                        }
                    }).ToList();
                }

                orders = orders.Take(50).ToList(); // Giới hạn hiển thị

                // Populate DataGridView
                foreach (var order in orders)
                {
                    string customerName = "Khách lẻ";
                    if (order.CustomerId.HasValue)
                    {
                        var customerResult = await AppServices.CustomerService.GetCustomerByIdAsync(order.CustomerId.Value);
                        if (customerResult.Success && customerResult.Data != null)
                        {
                            customerName = customerResult.Data.Name;
                        }
                    }

                    // Lấy thông tin sản phẩm
                    var orderDetailsResult = await AppServices.OrderService.GetOrderWithDetailsAsync(order.OrderId);
                    string productInfo = "Đa dạng sản phẩm";

                    if (orderDetailsResult.Success && orderDetailsResult.Data?.OrderDetails?.Any() == true)
                    {
                        var firstProduct = orderDetailsResult.Data.OrderDetails.First();
                        var productResult = await AppServices.ProductService.GetProductByIdAsync(firstProduct.ProductId);
                        if (productResult.Success)
                        {
                            var product = productResult.Data;
                            productInfo = $"{product.Name} ({firstProduct.Quantity} {product.Unit})";
                        }
                    }

                    var row = dgvOrders.Rows[dgvOrders.Rows.Add(
                        order.OrderCode ?? $"ORD-{order.OrderId:D5}",
                        customerName,
                        productInfo,
                        GetOrderSourceDisplay(order.Source),
                        order.TotalAmount.ToString("N0") ?? "0",
                        GetOrderStatusDisplay(order.Status),
                        order.LastUpdated.ToString("dd/MM/yyyy HH:mm")
                    )];
                    // Lưu OrderId vào Tag của row để sử dụng khi mở form chi tiết
                    row.Tag = order.OrderId;
                }
            }
            catch (Exception ex)
            {
                dgvOrders.Rows.Add("", "Lỗi tải dữ liệu", "", "", "", "", "");
                Console.WriteLine($"Lỗi tải đơn hàng: {ex.Message}");
            }
            finally
            {
                ShowLoading(false);
            }
        }

        // Helper methods giữ nguyên
        private string GetOrderSourceDisplay(OrderSource source)
        {
            switch (source)
            {
                case OrderSource.GOOGLEFORM:
                case OrderSource.EMAIL:
                    return "Email";
                case OrderSource.MANUAL:
                    return "Offline";
                case OrderSource.EXCEL:
                    return "Excel";
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
                string orderCode = row.Cells["OrderCode"].Value?.ToString() ?? "";
                MessageBox.Show($"Cập nhật trạng thái cho đơn hàng: {orderCode}", "Cập nhật");
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

        }

        private void searchControl1_Load(object sender, EventArgs e)
        {

        }

        private void headerPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void OrdersControl_Load(object sender, EventArgs e)
        {

        }
    }
}