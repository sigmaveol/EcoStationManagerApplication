using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.DTOs;
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
    public partial class OrdersControl : UserControl
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
                //dgvOrders.CellContentClick += dgvOrders_CellContentClick;
                //dgvOrders.CellFormatting += dgvOrders_CellFormatting;
            }
        }

        // Đổ các nút Tab vào FlowLayoutPanel
        private void PopulateTabPanel()
        {
            if (tabPanel == null) return;

            // Tạo mảng các tab với tag tương ứng
            var tabs = new[]
            {
            new { Text = "Cần xử lí", Tag = "all" },
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
                await LoadOrdersAsync(filterTag, txtSearch?.Text?.Trim());
            }
        }

        // Thêm cột vào DataGridView
        private void InitializeDataGridColumns()
        {
            if (dgvOrders == null) return;

            dgvOrders.Columns.Clear();
            dgvOrders.AutoGenerateColumns = false;

            // 1. Cột OrderId (QUAN TRỌNG: Phải để Visible = false)
            // Cột này dùng để lấy ID khi nhấn nút Chi tiết/Cập nhật
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OrderId",
                Visible = false
            });

            // 2. Các cột hiển thị (Thứ tự 1)
            var columns = new[]
            {
                new { Name = "OrderCode", Header = "Mã đơn" },
                new { Name = "Customer", Header = "Khách hàng" },
                new { Name = "Source", Header = "Nguồn" },
                new { Name = "TotalAmount", Header = "Tổng tiền" },
                new { Name = "PaymentStatus", Header = "Thanh toán" },
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
                    // Định dạng tiền tệ cho cột Tổng tiền
                    DefaultCellStyle = (col.Name == "TotalAmount")
                        ? new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
                        : null
                });
            }

            // 3. Các nút thao tác
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
            dgvOrders.Columns["Customer"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        // Thêm dữ liệu 
        private async Task LoadOrdersAsync(string filterTag = "all", string keyword = null)
        {
            try
            {
                ShowLoading(true);
                dgvOrders.Rows.Clear();

                IEnumerable<OrderDTO> orders = Enumerable.Empty<OrderDTO>();

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    var searchResult = await AppServices.OrderService.SearchOrdersAsync(keyword);
                    if (!searchResult.Success)
                    {
                        dgvOrders.Rows.Add(null, "", searchResult.Message ?? "Không tìm thấy đơn hàng", "", "", "", "", "", "");
                        return;
                    }
                    var searchOrders = searchResult.Data ?? Enumerable.Empty<Order>();
                    orders = searchOrders.Select(o => new OrderDTO
                    {
                        OrderId = o.OrderId,
                        OrderCode = o.OrderCode,
                        CustomerId = o.CustomerId,
                        Source = o.Source,
                        TotalAmount = o.TotalAmount,
                        Status = o.Status,
                        PaymentStatus = o.PaymentStatus,
                        LastUpdated = o.LastUpdated
                    });
                }
                else
                {
                    // Nếu filter là "completed", cần lấy tất cả đơn hàng vì GetProcessingOrdersAsync không bao gồm COMPLETED
                    if (filterTag == "completed")
                    {
                        var allOrdersResult = await AppServices.OrderService.GetAllAsync();
                        if (!allOrdersResult.Success)
                        {
                            dgvOrders.Rows.Add(null, "", allOrdersResult.Message ?? "Lỗi tải đơn hàng", "", "", "", "", "", "");
                            return;
                        }
                        orders = allOrdersResult.Data ?? Enumerable.Empty<OrderDTO>();
                        // Lọc chỉ lấy đơn đã hoàn thành
                        orders = orders.Where(order => order.Status == OrderStatus.COMPLETED).ToList();
                    }
                    else
                    {
                        var ordersResult = await AppServices.OrderService.GetProcessingOrdersAsync();
                        if (!ordersResult.Success)
                        {
                            dgvOrders.Rows.Add(null, "", ordersResult.Message ?? "Lỗi tải đơn hàng", "", "", "", "", "", "");
                            return;
                        }

                        orders = ordersResult.Data ?? Enumerable.Empty<OrderDTO>();

                        if (filterTag != "all")
                        {
                            orders = orders.Where(order =>
                            {
                                switch (filterTag)
                                {
                                    case "online":
                                        // Đơn online: EXCEL, EMAIL, GOOGLEFORM
                                        return order.Source == OrderSource.EXCEL ||
                                               order.Source == OrderSource.EMAIL ||
                                               order.Source == OrderSource.GOOGLEFORM;
                                    case "offline":
                                        // Đơn offline: MANUAL
                                        return order.Source == OrderSource.MANUAL;
                                    case "new":
                                        return order.Status == OrderStatus.CONFIRMED;
                                    case "ready":
                                        return order.Status == OrderStatus.READY;
                                    case "shipping":
                                        return order.Status == OrderStatus.SHIPPED;
                                    case "return":
                                        return false;
                                    default:
                                        return true;
                                }
                            }).ToList();
                        }
                    }
                }

                var orderList = (orders ?? Enumerable.Empty<OrderDTO>()).Take(50).ToList();

                if (!orderList.Any())
                {
                    dgvOrders.Rows.Add(null, "", "Không có đơn hàng phù hợp", "", "", "", "", "", "");
                    return;
                }

                foreach (var order in orderList)
                {
                    // 1. Khai báo biến customerName (BẮT BUỘC CÓ DÒNG NÀY)
                    string customerName = "Khách lẻ";

                    // 2. Lấy tên khách hàng từ Service (nếu có CustomerId)
                    if (order.CustomerId.HasValue)
                    {
                        var customerResult = await AppServices.CustomerService.GetCustomerByIdAsync(order.CustomerId.Value);
                        if (customerResult.Success && customerResult.Data != null)
                        {
                            customerName = customerResult.Data.Name;
                        }
                    }

                    // 3. Lấy trạng thái thanh toán
                    string paymentStatusText = GetPaymentStatusDisplay(order.PaymentStatus);
                    
                    // 4. Lấy nguồn đơn hàng
                    string sourceText = GetOrderSourceDisplay(order.Source);

                    // 5. Thêm vào DataGridView (Thứ tự khớp 100% với InitializeDataGridColumns)

                    dgvOrders.Rows.Add(
                        order.OrderId,                                   // [0] OrderId
                        order.OrderCode ?? $"ORD-{order.OrderId:D5}",    // [1] Mã đơn
                        customerName,                                    // [2] Khách hàng
                        sourceText,                                      // [3] Nguồn
                        order.TotalAmount,                               // [4] Tổng tiền
                        paymentStatusText,                               // [5] Thanh toán
                        GetOrderStatusDisplay(order.Status),             // [6] Trạng thái
                        order.LastUpdated.ToString("dd/MM/yyyy HH:mm")   // [7] Ngày tạo
                    );
                }
            }
            catch (Exception ex)
            {
                dgvOrders.Rows.Add(null, "", $"Lỗi tải dữ liệu: {ex.Message}", "", "", "", "", "", "");
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

        private string GetPaymentStatusDisplay(PaymentStatus status)
        {
            switch (status)
            {
                case PaymentStatus.UNPAID:
                    return "Chưa thanh toán";
                case PaymentStatus.PAID:
                    return "Đã thanh toán";
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

                    _pdfExporter.ExportToPdf(exportData, saveDialog.FileName, "DANH SÁCH ĐƠN HÀNG", headers);
                    
                    ShowLoading(false);
                    MessageBox.Show($"Đã xuất PDF thành công!\nFile: {saveDialog.FileName}", 
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    await LoadOrdersAsync(_currentFilter, txtSearch?.Text?.Trim());
                }
            }
        }


        // Xử lý khi nhấn nút trên DataGridView
        private async void dgvOrders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string colName = dgvOrders.Columns[e.ColumnIndex].Name;
            var cellValue = dgvOrders.Rows[e.RowIndex].Cells["OrderId"].Value;
            if (cellValue == null || !int.TryParse(cellValue.ToString(), out int orderId))
                return;

            if (colName == "colDetail")
            {
                using (var detailForm = new OrderDetailForm(orderId))
                {
                    FormHelper.ShowModalWithDim(FindForm(), detailForm);
                }
            }
            else if (colName == "colUpdate")
            {
                using (var updateForm = new UpdateOrderForm(orderId))
                {
                    if (FormHelper.ShowModalWithDim(FindForm(), updateForm) == DialogResult.OK)
                    {
                        await LoadOrdersAsync(_currentFilter, txtSearch?.Text?.Trim());
                    }
                }
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

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await LoadOrdersAsync(_currentFilter, txtSearch?.Text?.Trim());
        }

        private async void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                await LoadOrdersAsync(_currentFilter, txtSearch?.Text?.Trim());
            }
        }

        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                await LoadOrdersAsync(_currentFilter);
            }
        }

        private void tabPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void headerPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void OrdersControl_Load(object sender, EventArgs e)
        {

        }

        private void searchPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}