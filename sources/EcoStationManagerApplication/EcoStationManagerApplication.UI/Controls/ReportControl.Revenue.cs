using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    /// <summary>
    /// Partial class for Revenue Report functionality
    /// </summary>
    public partial class ReportControl
    {
        private async Task LoadRevenueReport(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var revenueResult = await _reportService.GetRevenueReportAsync(fromDate, toDate, "day");
                
                if (!revenueResult.Success)
                {
                    ShowPlaceholderMessage($"Không thể tải dữ liệu: {revenueResult.Message ?? "Lỗi không xác định"}");
                    return;
                }

                if (revenueResult.Data == null)
                {
                    ShowPlaceholderMessage("Không có dữ liệu để hiển thị");
                    return;
                }

                var reportData = revenueResult.Data;

                var ordersResult = await _orderService.GetPagedOrdersAsync(
                    1, 10000, 
                    new Models.DTOs.OrderSearchCriteria 
                    { 
                        FromDate = fromDate, 
                        ToDate = toDate,
                        Status = OrderStatus.COMPLETED 
                    });

                List<Models.Entities.Order> orders = new List<Models.Entities.Order>();
                if (ordersResult.Success && ordersResult.Data.Orders != null)
                {
                    orders = ordersResult.Data.Orders.ToList();
                }

                RemovePlaceholder();
                ClearReportContent();
                flowPanelKPICards.Visible = true;
                dataGridViewReport.Visible = true;
                panelChart.Visible = true;

                CreateRevenueKPICards(reportData, orders);
                CreateRevenueDataTable(reportData.DataPoints, orders);
                CreateChartPlaceholder();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải báo cáo doanh thu: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ShowPlaceholderMessage($"Có lỗi xảy ra: {ex.Message}");
            }
        }

        private void CreateRevenueKPICards(RevenueReportDTO reportData, List<Models.Entities.Order> orders)
        {
            flowPanelKPICards.Controls.Clear();

            int totalCustomers = orders.Select(o => o.CustomerId).Where(id => id.HasValue).Distinct().Count();
            decimal avgRevenuePerCustomer = totalCustomers > 0 
                ? reportData.TotalRevenue / totalCustomers 
                : 0;

            int googleFormOrders = orders.Count(o => o.Source == OrderSource.GOOGLEFORM);
            int excelOrders = orders.Count(o => o.Source == OrderSource.EXCEL);
            int emailOrders = orders.Count(o => o.Source == OrderSource.EMAIL);
            int manualOrders = orders.Count(o => o.Source == OrderSource.MANUAL);

            var kpiData = new[]
            {
                new { Label = "Tổng giao dịch", Value = reportData.TotalOrders.ToString("N0"), Icon = "📊" },
                new { Label = "Tổng doanh thu", Value = ReportControlHelpers.FormatCurrency(reportData.TotalRevenue), Icon = "💰" },
                new { Label = "Số khách hàng", Value = totalCustomers.ToString("N0"), Icon = "👥" },
                new { Label = "TB mỗi giao dịch", Value = ReportControlHelpers.FormatCurrency(reportData.AverageOrderValue), Icon = "📈" },
                new { Label = "TB mỗi khách", Value = ReportControlHelpers.FormatCurrency(avgRevenuePerCustomer), Icon = "🎯" }
            };

            foreach (var kpi in kpiData)
            {
                var card = ReportControlHelpers.CreateKPICard(kpi.Label, kpi.Value, kpi.Icon);
                card.Margin = new Padding(10, 5, 10, 5);
                card.Size = new Size(200, 100);
                flowPanelKPICards.Controls.Add(card);
            }

            if (googleFormOrders > 0)
            {
                var card = ReportControlHelpers.CreateKPICard("Google Form", googleFormOrders.ToString("N0"), "📝");
                card.Margin = new Padding(10, 5, 10, 5);
                card.Size = new Size(150, 100);
                flowPanelKPICards.Controls.Add(card);
            }

            if (excelOrders > 0)
            {
                var card = ReportControlHelpers.CreateKPICard("Excel", excelOrders.ToString("N0"), "📗");
                card.Margin = new Padding(10, 5, 10, 5);
                card.Size = new Size(150, 100);
                flowPanelKPICards.Controls.Add(card);
            }

            if (emailOrders > 0)
            {
                var card = ReportControlHelpers.CreateKPICard("Email", emailOrders.ToString("N0"), "📧");
                card.Margin = new Padding(10, 5, 10, 5);
                card.Size = new Size(150, 100);
                flowPanelKPICards.Controls.Add(card);
            }

            if (manualOrders > 0)
            {
                var card = ReportControlHelpers.CreateKPICard("Thủ công", manualOrders.ToString("N0"), "✋");
                card.Margin = new Padding(10, 5, 10, 5);
                card.Size = new Size(150, 100);
                flowPanelKPICards.Controls.Add(card);
            }
        }

        private void CreateRevenueDataTable(List<RevenueDataPoint> dataPoints, List<Models.Entities.Order> orders)
        {
            dataGridViewReport.DataSource = null;
            dataGridViewReport.Columns.Clear();

            if (dataPoints == null || dataPoints.Count == 0)
                return;

            var ordersByDate = orders
                .Where(o => o.LastUpdated >= dataPoints.Min(d => d.Date).Date && 
                           o.LastUpdated <= dataPoints.Max(d => d.Date).AddDays(1))
                .GroupBy(o => o.LastUpdated.Date)
                .ToDictionary(g => g.Key, g => g.ToList());

            var dataTable = new DataTable();
            dataTable.TableName = "Báo cáo Doanh thu";
            dataTable.Columns.Add("Ngày", typeof(string));
            dataTable.Columns.Add("Số đơn", typeof(int));
            dataTable.Columns.Add("Doanh thu", typeof(string));
            dataTable.Columns.Add("Thanh toán", typeof(string));
            dataTable.Columns.Add("Phương thức TT", typeof(string));
            dataTable.Columns.Add("Nguồn", typeof(string));
            dataTable.Columns.Add("Ghi chú", typeof(string));

            foreach (var point in dataPoints.OrderBy(x => x.Date))
            {
                var dateKey = new DateTime(point.Date.Year, point.Date.Month, point.Date.Day);
                var dayOrders = ordersByDate.ContainsKey(dateKey) ? ordersByDate[dateKey] : new List<Models.Entities.Order>();

                Console.WriteLine("POINT DATE = " + point.Date);

                foreach (var o in orders)
                    Console.WriteLine("ORDER DATE = " + o.LastUpdated.Date);

                int paidCount = dayOrders.Count(o => o.PaymentStatus == PaymentStatus.PAID);
                int unpaidCount = dayOrders.Count(o => o.PaymentStatus == PaymentStatus.UNPAID);
                string paymentInfo = $"Đã TT: {paidCount} | Chưa TT: {unpaidCount}";

                int cashCount = dayOrders.Count(o => o.PaymentMethod == PaymentMethod.CASH);
                int transferCount = dayOrders.Count(o => o.PaymentMethod == PaymentMethod.TRANSFER);
                string paymentMethodInfo = $"Tiền mặt: {cashCount} | Chuyển khoản: {transferCount}";

                var sourceGroups = dayOrders.GroupBy(o => o.Source)
                    .Select(g => new { Source = ReportControlHelpers.GetOrderSourceName(g.Key), Count = g.Count() })
                    .ToList();
                string sourceInfo = string.Join(", ", sourceGroups.Select(s => $"{s.Source}: {s.Count}"));

                dataTable.Rows.Add(
                    point.Date.ToString("dd/MM/yyyy"),
                    point.OrderCount,
                    ReportControlHelpers.FormatCurrency(point.Revenue),
                    paymentInfo,
                    paymentMethodInfo,
                    sourceInfo,
                    point.Period
                );
            }

            dataGridViewReport.DataSource = dataTable;
            dataGridViewReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewReport.ColumnHeadersVisible = true;
            dataGridViewReport.EnableHeadersVisualStyles = false;
        }
    }
}

