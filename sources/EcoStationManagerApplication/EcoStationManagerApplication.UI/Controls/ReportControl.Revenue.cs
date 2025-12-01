using EcoStationManagerApplication.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    /// <summary>
    /// Partial class handling the revenue report rendering.
    /// </summary>
    public partial class ReportControl
    {
        protected async Task LoadRevenueReport(DateTime fromDate, DateTime toDate)
        {
            try
            {
                ShowLoadingMessage("Đang tải dữ liệu báo cáo doanh thu...");

                var reportResult = await _reportService.GetRevenueReportAsync(fromDate, toDate, "day");
                if (!reportResult.Success || reportResult.Data == null)
                {
                    ShowPlaceholderMessage($"Không thể tải dữ liệu: {reportResult.Message ?? "Lỗi không xác định"}");
                    return;
                }

                var report = reportResult.Data;
                if (report.DataPoints == null || !report.DataPoints.Any())
                {
                    ShowPlaceholderMessage("Không có dữ liệu doanh thu trong giai đoạn này.");
                    return;
                }

                RemovePlaceholder();
                ClearReportContent();

                // Đảm bảo các panel được hiển thị
                flowPanelKPICards.Visible = true;
                dataGridViewReport.Visible = true;
                panelChart.Visible = true;

                BuildRevenueKpis(report);
                BuildRevenueTable(report.DataPoints);
                BuildRevenueChart(report.DataPoints);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải báo cáo doanh thu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ShowPlaceholderMessage($"Có lỗi xảy ra: {ex.Message}");
            }
        }

        private void BuildRevenueKpis(RevenueReportDTO report)
        {
            flowPanelKPICards.SuspendLayout();
            flowPanelKPICards.Controls.Clear();

            var kpis = new[]
            {
                new { Title = "Tổng doanh thu", Value = FormatCurrency(report.TotalRevenue), Icon = "💰", Color = Color.FromArgb(46, 204, 113) },
                new { Title = "Tổng số đơn", Value = report.TotalOrders.ToString("N0"), Icon = "🧾", Color = Color.FromArgb(52, 152, 219) },
                new { Title = "Giá trị TB / đơn", Value = FormatCurrency(report.AverageOrderValue), Icon = "⚖️", Color = Color.FromArgb(155, 89, 182) },
                new { Title = "Doanh thu ngày", Value = FormatCurrency(report.DailyRevenue), Icon = "📅", Color = Color.FromArgb(230, 126, 34) },
                new { Title = "Doanh thu tuần", Value = FormatCurrency(report.WeeklyRevenue), Icon = "📆", Color = Color.FromArgb(26, 188, 156) },
                new { Title = "Doanh thu tháng", Value = FormatCurrency(report.MonthlyRevenue), Icon = "📊", Color = Color.FromArgb(241, 196, 15) }
            };

            foreach (var card in kpis)
            {
                var control = CreateKPICard(card.Title, card.Value, card.Icon, card.Color);
                control.Margin = new Padding(10, 5, 10, 5);
                control.Size = new Size(220, 100);
                flowPanelKPICards.Controls.Add(control);
            }
            flowPanelKPICards.ResumeLayout(true);
        }

        private void BuildRevenueTable(IEnumerable<RevenueDataPoint> points)
        {
            var table = new DataTable();
            table.Columns.Add("STT", typeof(int));
            table.Columns.Add("Thời gian", typeof(string));
            table.Columns.Add("Doanh thu", typeof(string));
            table.Columns.Add("Số đơn", typeof(string));

            int index = 1;
            foreach (var point in points.OrderBy(p => p.Date))
            {
                table.Rows.Add(
                    index++,
                    point.Period,
                    FormatCurrency(point.Revenue),
                    point.OrderCount.ToString("N0"));
            }

            dataGridViewReport.DataSource = table;
            dataGridViewReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewReport.BringToFront();
        }

        private void BuildRevenueChart(IEnumerable<RevenueDataPoint> points)
        {
            panelChart.SuspendLayout();
            panelChart.Controls.Clear();
            panelChart.Padding = new Padding(20);

            var orderedPoints = points.OrderBy(p => p.Date).ToList();
            if (orderedPoints.Count > 12)
            {
                orderedPoints = orderedPoints.Skip(orderedPoints.Count - 12).ToList();
            }
            var maxRevenue = orderedPoints.Max(p => p.Revenue);

            if (maxRevenue <= 0)
            {
                panelChart.Controls.Add(new Label
                {
                    Text = "Không có doanh thu để hiển thị biểu đồ",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 10, FontStyle.Italic)
                });
                return;
            }

            var stack = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true
            };

            foreach (var point in orderedPoints)
            {
                var row = new Panel
                {
                    Width = panelChart.Width - 80,
                    Height = 55,
                    Margin = new Padding(0, 5, 0, 5)
                };

                var lblPeriod = new Label
                {
                    Text = point.Period,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    Width = 120,
                    Location = new Point(0, 5)
                };

                var progress = new ProgressBar
                {
                    Height = 18,
                    Width = 320,
                    Maximum = (int)Math.Max(1, Math.Round(maxRevenue)),
                    Value = (int)Math.Max(0, Math.Round(point.Revenue))
                };

                var lblValue = new Label
                {
                    Text = $"{FormatCurrency(point.Revenue)} ({point.OrderCount:N0} đơn)",
                    AutoSize = false,
                    Width = 260,
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.FromArgb(85, 85, 85),
                    Location = new Point(460, 5)
                };

                progress.Location = new Point(130, 5);

                row.Controls.Add(lblPeriod);
                row.Controls.Add(progress);
                row.Controls.Add(lblValue);
                stack.Controls.Add(row);
            }

            var title = new Label
            {
                Text = "DOANH THU THEO NGÀY",
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 107, 59)
            };

            panelChart.Controls.Add(stack);
            panelChart.Controls.Add(title);
            panelChart.ResumeLayout(true);
        }
    }
}

