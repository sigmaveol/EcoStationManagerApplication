using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.UI.Common;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class DashboardControl : UserControl, IRefreshableControl
    {
        public event EventHandler ViewAllOrdersClicked;

        public DashboardControl()
        {
            InitializeComponent();
        }

        public void RefreshData()
        {
            _ = RefreshDataAsync();
        }

        private async void DashboardControl_Load(object sender, EventArgs e)
        {
            await InitializeComponentCustom();

        }

        private async Task InitializeComponentCustom()
        {
            PopulateStatsPanel();
            InitializeCharts();
            await LoadDashboardData();
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
                new { Label = "Bao bì đang được sử dụng", Value = "0", Desc = "Chai/lọ", IsEco = false },
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
                    LoadChartsData()
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
                var todayOrdersResult = await AppServices.OrderService.GetTodayOrdersAsync();
                int todayOrderCount = todayOrdersResult.Success ? todayOrdersResult.Data.Count() : 0;
                UpdateStatCard("Đơn hàng hôm nay", todayOrderCount.ToString(), "Đơn hàng mới");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi lấy đơn hàng hôm nay: {ex.Message}");
                UpdateStatCard("Đơn hàng hôm nay", "0", "Lỗi tải dữ liệu");
            }

            // Doanh thu tháng từ ngày 1 tới hiện tại
            try
            {
                var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var endDate = DateTime.Now;
                var totalRevenueResult = await AppServices.OrderService.GetTotalRevenueAsync(startDate, endDate);
                decimal monthlyRevenue = totalRevenueResult.Success ? totalRevenueResult.Data : 0;
                UpdateStatCard("Doanh thu tháng", $"{monthlyRevenue:N0}", "VND");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi tính doanh thu tháng: {ex.Message}");
                UpdateStatCard("Doanh thu tháng", "0", "Lỗi tải dữ liệu");
            }

            // Sản phẩm tồn kho thấp
            try
            {
                var lowStockResult = await AppServices.InventoryService.GetLowStockItemsAsync();
                int lowStockCount = lowStockResult.Success ? lowStockResult.Data.Count() : 0;
                UpdateStatCard("Tồn kho thấp", lowStockCount.ToString(), "Sản phẩm cần nhập");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi lấy tồn kho thấp: {ex.Message}");
                UpdateStatCard("Tồn kho thấp", "0", "Lỗi tải dữ liệu");
            }

            // Bao bì đang được sử dụng
            try
            {
                var packagingResult = await AppServices.PackagingInventoryService.GetAllAsync();
                int packagingInUse = (packagingResult != null && packagingResult.Success)
                    ? packagingResult.Data.Sum(p => p.QtyInUse)
                    : 0;
                UpdateStatCard("Bao bì đang được sử dụng", packagingInUse.ToString(), "Chai/lọ lưu hành");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi tính bao bì: {ex.Message}");
                UpdateStatCard("Bao bì đang được sử dụng", "0", "Lỗi tải dữ liệu");
            }

            // Đơn chờ xử lý
            try
            {
                var pendingOrdersResult = await AppServices.OrderService.GetPendingOrdersAsync();
                int pendingOrdersCount = pendingOrdersResult.Success ? pendingOrdersResult.Data.Count() : 0;
                UpdateStatCard("Đơn chờ xử lý", pendingOrdersCount.ToString(), "Cần xử lý");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi lấy đơn chờ xử lý: {ex.Message}");
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

        private async Task LoadChartsData()
        {
            try
            {
                var endDate = DateTime.Today;
                var startDate = endDate.AddDays(-6);

                var criteria = new EcoStationManagerApplication.Models.DTOs.OrderSearchCriteria
                {
                    FromDate = startDate,
                    ToDate = endDate
                };

                var ordersResult = await AppServices.OrderService.GetPagedOrdersAsync(1, 1000, criteria);
                var orders = ordersResult.Success ? ordersResult.Data.Orders.ToList() : new System.Collections.Generic.List<EcoStationManagerApplication.Models.Entities.Order>();

                if (chartOrderSource != null)
                {
                    chartOrderSource.Series.Clear();
                    chartOrderSource.ChartAreas.Clear();
                    chartOrderSource.Legends.Clear();

                    var area = new ChartArea("PieArea");
                    chartOrderSource.ChartAreas.Add(area);
                    var legend = new Legend("Legend");
                    legend.Docking = Docking.Right;
                    chartOrderSource.Legends.Add(legend);
                    var series = new Series("Sources")
                    {
                        ChartType = SeriesChartType.Pie,
                        ChartArea = "PieArea",
                        Legend = "Legend"
                    };
                    series.IsValueShownAsLabel = true;

                    var total = Math.Max(orders.Count, 1);
                    var sourceGroups = orders.GroupBy(o => o.Source)
                        .ToDictionary(g => g.Key, g => g.Count());

                    foreach (var src in new[] { OrderSource.EXCEL, OrderSource.GOOGLEFORM, OrderSource.MANUAL, OrderSource.EMAIL })
                    {
                        var count = sourceGroups.ContainsKey(src) ? sourceGroups[src] : 0;
                        var pct = (double)count / total;
                        var dp = new DataPoint
                        {
                            AxisLabel = GetOrderSourceDisplay(src),
                            YValues = new[] { (double)count }
                        };
                        dp.Label = string.Format("{0:P0}", pct);
                        dp.LegendText = GetOrderSourceDisplay(src);
                        series.Points.Add(dp);
                    }

                    chartOrderSource.Series.Add(series);
                }

                if (chartRevenue7Days != null)
                {
                    chartRevenue7Days.Series.Clear();
                    chartRevenue7Days.ChartAreas.Clear();
                    chartRevenue7Days.Legends.Clear();

                    var area = new ChartArea("BarArea");
                    area.AxisX.Interval = 1;
                    area.AxisX.MajorGrid.Enabled = false;
                    area.AxisY.MajorGrid.LineColor = Color.Gainsboro;
                    area.AxisY.LabelStyle.Format = "#,0";
                    chartRevenue7Days.ChartAreas.Add(area);
                    var legend = new Legend("Legend");
                    chartRevenue7Days.Legends.Add(legend);
                    var series = new Series("Revenue")
                    {
                        ChartType = SeriesChartType.Column,
                        ChartArea = "BarArea",
                        Legend = "Legend"
                    };
                    series.IsValueShownAsLabel = true;

                    for (int i = 6; i >= 0; i--)
                    {
                        var day = endDate.AddDays(-i);
                        var dayOrders = orders.Where(o => o.LastUpdated.Date == day.Date && o.Status == OrderStatus.COMPLETED);
                        var revenue = dayOrders.Sum(o => Math.Max(o.TotalAmount - o.DiscountedAmount, 0));
                        var point = new DataPoint
                        {
                            AxisLabel = day.ToString("dd/MM"),
                            YValues = new[] { (double)revenue }
                        };
                        series.Points.Add(point);
                    }

                    chartRevenue7Days.Series.Add(series);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi tải dữ liệu biểu đồ: {ex.Message}");
            }
        }

        private string GetOrderSourceDisplay(OrderSource source)
        {
            switch (source)
            {
                case OrderSource.GOOGLEFORM:
                    return "Google Form";
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

        private void InitializeCharts()
        {
            if (chartOrderSource != null)
            {
                chartOrderSource.Palette = ChartColorPalette.BrightPastel;
            }

            if (chartRevenue7Days != null)
            {
                chartRevenue7Days.Palette = ChartColorPalette.SeaGreen;
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
            lblValue.Tag = "Value";

            var lblDesc = new Label();
            lblDesc.Text = desc;
            lblDesc.Font = new Font("Segoe UI", 9);
            lblDesc.ForeColor = Color.Gray;
            lblDesc.AutoSize = true;
            lblDesc.Location = new Point(15, 85);
            lblDesc.Tag = "Desc";

            card.Controls.Add(lblLabel);
            card.Controls.Add(lblValue);
            card.Controls.Add(lblDesc);

            return card;
        }

        

        // Method để refresh dữ liệu từ bên ngoài
        public async Task RefreshDataAsync()
        {
            await LoadDashboardData();
        }
    }
}
