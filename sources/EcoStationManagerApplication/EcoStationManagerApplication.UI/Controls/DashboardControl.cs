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
            InitializeCharts();
            await LoadDashboardData();
        }

        

        // Đổ dữ liệu tạm thời cho các thẻ thống kê
        private void PopulateStatsPanel()
        {
            if (statsPanel == null) return;
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
                .FirstOrDefault(c => c.Tag?.ToString() == label) as CardControl;

            if (statCard != null)
            {
                statCard.Value = value;
                statCard.SubInfo = description;
            }
        }

        private async Task LoadChartsData()
        {
            try
            {
                var endDate = DateTime.Today;
                var startDate = endDate.AddDays(-6);

                var criteria = new Models.DTOs.OrderSearchCriteria
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

        
        

        // Method để refresh dữ liệu từ bên ngoài
        public async Task RefreshDataAsync()
        {
            await LoadDashboardData();
        }
    }
}
