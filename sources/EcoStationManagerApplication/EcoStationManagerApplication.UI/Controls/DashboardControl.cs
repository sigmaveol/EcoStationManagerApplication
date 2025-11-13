using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.UI.Common;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class DashboardControl : UserControl
    {
        public event EventHandler ViewAllOrdersClicked;

        public DashboardControl()
        {
            InitializeComponent();
        }

        private async void DashboardControl_Load(object sender, EventArgs e)
        {
            await InitializeComponentCustom();

        }

        private async Task InitializeComponentCustom()
        {
            PopulateStatsPanel();
            InitializeDataGrid();


            btnViewAllOrders.Click += btnViewAllOrders_Click;
            await LoadDashboardData();
        }

        private void btnViewAllOrders_Click(object sender, EventArgs e)
        {
            ViewAllOrdersClicked?.Invoke(this, EventArgs.Empty);
        }

        // Đổ dữ liệu tạm thời cho các thẻ thống kê
        private void PopulateStatsPanel()
        {
            if (statsPanel == null) return;

            var statsData = new[]
            {
                new { Label = "Đơn hàng hôm nay", Value = "0", Desc = "Đang tải...", IsEco = false },
                new { Label = "Doanh thu tháng", Value = "0", Desc = "VND", IsEco = false },
                new { Label = "Tồn kho thấp", Value = "0", Desc = "Sản phẩm", IsEco = false },
                new { Label = "Bao bì đang sử dụng", Value = "0", Desc = "Chai/lọ", IsEco = false },
                new { Label = "Đơn chờ xử lý", Value = "0", Desc = "Cần xử lý", IsEco = false }
            };

            foreach (var stat in statsData)
            {
                var statCard = CreateStatCard(stat.Label, stat.Value, stat.Desc, stat.IsEco);
                statCard.Margin = new Padding(10);
                statCard.Size = new Size(170, 120);
                statCard.Tag = stat.Label; // Dùng để cập nhật sau này
                statsPanel.Controls.Add(statCard);
            }
        }

        private async Task LoadDashboardData()
        {
            try
            {
                ShowLoading(true);

                // Load dữ liệu song song
                await Task.WhenAll(
                    LoadStatistics(),
                    LoadRecentOrders()
                );
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "tải dữ liệu dashboard");
            }
            finally
            {
                ShowLoading(false);
            }
        }

        private async Task LoadStatistics()
        {
            try
            {
                // Tổng đơn hàng hôm nay
                var todayOrdersResult = await AppServices.OrderService.GetTodayOrdersAsync();
                var todayOrderCount = todayOrdersResult.Success ? todayOrdersResult.Data.Count() : 0;

                // Doanh thu tháng
                var monthlyRevenue = await CalculateMonthlyRevenue();

                // Sản phẩm tồn kho thấp
                var lowStockCount = await GetLowStockCount();

                // Bao bì cần thu hồi
                var packagingInUse = await CalculatePackagingInUse();

                // Đơn hàng chờ xử lý
                var pendingOrdersCount = await GetPendingOrdersCount();

                // Cập nhật UI
                UpdateStatCard("Đơn hàng hôm nay", todayOrderCount.ToString(), "Đơn hàng mới");
                UpdateStatCard("Doanh thu tháng", $"{monthlyRevenue:N0}", "VND");
                UpdateStatCard("Tồn kho thấp", lowStockCount.ToString(), "Sản phẩm cần nhập");
                UpdateStatCard("Bao bì đang sử dụng", packagingInUse.ToString(), "Chai/lọ lưu hành");
                UpdateStatCard("Đơn chờ xử lý", pendingOrdersCount.ToString(), "Cần xử lý");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi tải thống kê: {ex.Message}");
                // Fallback to default values
                UpdateStatCard("Đơn hàng hôm nay", "0", "Lỗi tải dữ liệu");
                UpdateStatCard("Doanh thu tháng", "0", "Lỗi tải dữ liệu");
                UpdateStatCard("Tồn kho thấp", "0", "Lỗi tải dữ liệu");
                UpdateStatCard("Bao bì đang sử dụng", "0", "Lỗi tải dữ liệu");
                UpdateStatCard("Đơn chờ xử lý", "0", "Lỗi tải dữ liệu");
            }
        }

        private void UpdateStatCard(string label, string value, string description)
        {
            if (statsPanel == null) return;

            var statCard = statsPanel.Controls
                .Cast<Control>()
                .FirstOrDefault(c => c.Tag?.ToString() == label);

            if (statCard != null)
            {
                var valueLabel = statCard.Controls
                    .OfType<Label>()
                    .FirstOrDefault(l => l.Tag?.ToString() == "Value");

                var descLabel = statCard.Controls
                    .OfType<Label>()
                    .FirstOrDefault(l => l.Tag?.ToString() == "Desc");

                if (valueLabel != null) valueLabel.Text = value;
                if (descLabel != null) descLabel.Text = description;
            }
        }

        private async Task LoadRecentOrders()
        {
            try
            {
                // Sử dụng GetTodayOrdersAsync để lấy đơn hàng gần đây
                var recentOrdersResult = await AppServices.OrderService.GetTodayOrdersAsync();

                if (recentOrdersResult.Success && dgvRecentOrders != null)
                {
                    dgvRecentOrders.Rows.Clear();

                    // Lấy 10 đơn hàng gần đây nhất
                    var recentOrders = recentOrdersResult.Data
                        .OrderByDescending(o => o.LastUpdated)
                        .Take(10)
                        .ToList();

                    if (!recentOrders.Any())
                    {
                        MessageBox.Show("Không có đơn hàng hôm nay");
                        return;
                    }

                    foreach (var order in recentOrders)
                    {
                        // Chỉ show các trường scalar, navigation property sẽ null
                        string msg = $"OrderId: {order.OrderId}\n" +
                                     $"OrderCode: {order.OrderCode}\n" +
                                     $"CustomerId: {order.CustomerId}\n" +
                                     $"UserId: {order.UserId}\n" +
                                     $"TotalAmount: {order.TotalAmount}\n" +
                                     $"DiscountedAmount: {order.DiscountedAmount}\n" +
                                     $"Status: {order.Status}\n" +
                                     $"PaymentStatus: {order.PaymentStatus}\n" +
                                     $"PaymentMethod: {order.PaymentMethod}\n" +
                                     $"Address: {order.Address}\n" +
                                     $"Note: {order.Note}\n" +
                                     $"LastUpdated: {order.LastUpdated}";

                        MessageBox.Show(msg, $"Đơn hàng {order.OrderId}");
                    }

                    foreach (var order in recentOrders)
                    {
                        // Lấy thông tin khách hàng
                        string customerName = "Khách lẻ";
                        if (order.CustomerId.HasValue)
                        {
                            var customerResult = await AppServices.CustomerService.GetCustomerByIdAsync(order.CustomerId.Value);
                            if (customerResult.Success && customerResult.Data != null)
                            {
                                MessageBox.Show("a");
                                customerName = customerResult.Data.Name;
                            }
                        }

                        // Lấy thông tin sản phẩm từ chi tiết đơn hàng
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

                        dgvRecentOrders.Rows.Add(
                            order.OrderCode,
                            customerName,
                            productInfo,
                            GetOrderSourceDisplay(order.Source),
                            GetOrderStatusDisplay(order.Status),
                            order.LastUpdated.ToString("dd/MM/yyyy HH:mm")
                        );
                    }

                    // Nếu không có đơn hàng nào, hiển thị thông báo
                    if (!recentOrders.Any())
                    {
                        dgvRecentOrders.Rows.Add("", "Không có đơn hàng nào hôm nay", "", "", "", "");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi tải đơn hàng gần đây: {ex.Message}");
            }
        }

        private async Task<decimal> CalculateMonthlyRevenue()
        {
            try
            {
                // Sử dụng GetOrderSummaryAsync để lấy doanh thu
                var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var summaryResult = await AppServices.OrderService.GetOrderSummaryAsync(startDate, endDate);
                return summaryResult.Success ? summaryResult.Data.TotalRevenue : 0;
            }
            catch
            {
                return 0;
            }
        }

        private async Task<int> GetLowStockCount()
        {
            try
            {
                // Giả định có service lấy số lượng sản phẩm tồn kho thấp
                // Trong thực tế, bạn cần triển khai IInventoryService.GetLowStockItemsAsync()
                var lowStockResult = await AppServices.InventoryService.GetLowStockItemsAsync();
                return lowStockResult.Success ? lowStockResult.Data.Count() : 0;
            }
            catch
            {
                return 0;
            }
        }

        private async Task<int> CalculatePackagingInUse()
        {
            try
            {
                // Giả định có service lấy thông tin bao bì
                var packagingResult = await AppServices.PackagingInventoryService.GetAllAsync();
                if (packagingResult != null && packagingResult.Success)
                {
                    return packagingResult.Data.Sum(p => p.QtyInUse);
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        private async Task<int> GetPendingOrdersCount()
        {
            try
            {
                var pendingOrdersResult = await AppServices.OrderService.GetPendingOrdersAsync();
                return pendingOrdersResult.Success ? pendingOrdersResult.Data.Count() : 0;
            }
            catch
            {
                return 0;
            }
        }

        private string GetOrderSourceDisplay(OrderSource source)
        {
            switch (source)
            {
                case OrderSource.GOOGLEFORM:
                    return "Online";
                case OrderSource.MANUAL:
                    return "Offline";
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
                    return "Đã xác nhận";
                case OrderStatus.PROCESSING:
                    return "Đang xử lý";
                case OrderStatus.READY:
                    return "Sẵn sàng";
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
            // Implement loading indicator if needed
            if (show)
            {
                Cursor = Cursors.WaitCursor;
            }
            else
            {
                Cursor = Cursors.Default;
            }
        }

        // Khởi tạo DataGridView
        private void InitializeDataGrid()
        {
            if (dgvRecentOrders == null) return;

            dgvRecentOrders.BackgroundColor = Color.White;
            dgvRecentOrders.BorderStyle = BorderStyle.None;
            dgvRecentOrders.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvRecentOrders.GridColor = Color.FromArgb(240, 240, 240);
            dgvRecentOrders.AllowUserToAddRows = false;
            dgvRecentOrders.AllowUserToDeleteRows = false;
            dgvRecentOrders.AllowUserToResizeRows = false;
            dgvRecentOrders.RowHeadersVisible = false;
            dgvRecentOrders.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvRecentOrders.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgvRecentOrders.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvRecentOrders.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvRecentOrders.EnableHeadersVisualStyles = false;
            dgvRecentOrders.ColumnHeadersHeight = 40;
            dgvRecentOrders.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgvRecentOrders.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 245, 255);
            dgvRecentOrders.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvRecentOrders.RowTemplate.Height = 35;

            // Clear existing columns
            dgvRecentOrders.Columns.Clear();

            dgvRecentOrders.Columns.Add("OrderCode", "Mã đơn");           // OrderCode
            dgvRecentOrders.Columns.Add("CustomerName", "Khách hàng");    // CustomerName
            dgvRecentOrders.Columns.Add("Products", "Sản phẩm");          // Nối từ OrderDetails.ProductName
            dgvRecentOrders.Columns.Add("Source", "Loại");                // Source
            dgvRecentOrders.Columns.Add("Status", "Trạng thái");          // Status
            dgvRecentOrders.Columns.Add("LastUpdated", "Thời gian");      // LastUpdated
            // Tự động co giãn cột
            dgvRecentOrders.Columns["Products"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvRecentOrders.Columns["CustomerName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvRecentOrders.CellFormatting += dgvRecentOrders_CellFormatting;
        }

        private void dgvRecentOrders_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string colName = dgvRecentOrders.Columns[e.ColumnIndex].Name;

            if (colName == "Source" || colName == "Status")
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

        private Panel CreateStatCard(string label, string value, string desc, bool isEco = false)
        {
            var card = new Panel();
            card.Size = new Size(170, 120);
            card.BackColor = isEco ? Color.FromArgb(232, 245, 233) : Color.White;
            card.BorderStyle = BorderStyle.None;
            card.Padding = new Padding(15);
            card.Margin = new Padding(5);

            var lblLabel = new Label();
            lblLabel.Text = label;
            lblLabel.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblLabel.ForeColor = Color.FromArgb(100, 100, 100);
            lblLabel.AutoSize = true;
            lblLabel.Location = new Point(15, 15);

            var lblValue = new Label();
            lblValue.Text = value;
            lblValue.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblValue.ForeColor = isEco ? Color.FromArgb(46, 125, 50) : Color.FromArgb(25, 118, 210);
            lblValue.AutoSize = true;
            lblValue.Location = new Point(15, 40);

            var lblDesc = new Label();
            lblDesc.Text = desc;
            lblDesc.Font = new Font("Segoe UI", 9);
            lblDesc.ForeColor = Color.Gray;
            lblDesc.AutoSize = true;
            lblDesc.Location = new Point(15, 85);

            card.Controls.Add(lblLabel);
            card.Controls.Add(lblValue);
            card.Controls.Add(lblDesc);

            return card;
        }

        private Color GetBadgeColor(string status)
        {
            switch (status)
            {
                case "Online": return Color.FromArgb(187, 222, 251);
                case "Offline": return Color.FromArgb(224, 224, 224);
                case "Đang giao": return Color.FromArgb(200, 230, 201);
                case "Chuẩn bị": return Color.FromArgb(255, 249, 196);
                case "Hoàn thành": return Color.FromArgb(200, 230, 201);
                case "Đã xác nhận": return Color.FromArgb(255, 249, 196);
                case "Đang xử lý": return Color.FromArgb(255, 243, 205);
                case "Sẵn sàng": return Color.FromArgb(209, 231, 221);
                case "Đã hủy": return Color.FromArgb(245, 198, 203);
                case "Nháp": return Color.FromArgb(222, 226, 230);
                default: return Color.LightGray;
            }
        }

        // Method để refresh dữ liệu từ bên ngoài
        public async Task RefreshDataAsync()
        {
            await LoadDashboardData();
        }
    }
}