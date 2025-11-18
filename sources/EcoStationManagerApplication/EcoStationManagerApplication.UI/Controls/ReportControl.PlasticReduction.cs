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
    /// Partial class for the plastic reduction / environmental impact report.
    /// </summary>
    public partial class ReportControl
    {
        protected async Task LoadPlasticReductionReport(DateTime fromDate, DateTime toDate)
        {
            try
            {
                ShowLoadingMessage("Đang tải báo cáo giảm phát thải nhựa...");

                var reportResult = await _reportService.GetEnvironmentalImpactReportAsync(fromDate, toDate);
                if (!reportResult.Success || reportResult.Data == null)
                {
                    ShowPlaceholderMessage($"Không thể tải dữ liệu: {reportResult.Message ?? "Lỗi không xác định"}");
                    return;
                }

                var report = reportResult.Data;
                var points = report.DataPoints ?? new List<EnvironmentalImpactDataPoint>();
                if (!points.Any())
                {
                    ShowPlaceholderMessage("Không có giao dịch thu hồi bao bì nào trong khoảng thời gian này.");
                    return;
                }

                RemovePlaceholder();
                ClearReportContent();

                flowPanelKPICards.Visible = true;
                dataGridViewReport.Visible = true;
                panelChart.Visible = true;

                BuildPlasticReductionKpis(report);
                BuildPlasticReductionTable(points);
                BuildPlasticReductionChart(points);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải báo cáo giảm phát thải: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ShowPlaceholderMessage($"Có lỗi xảy ra: {ex.Message}");
            }
        }

        private void BuildPlasticReductionKpis(EnvironmentalImpactReportDTO report)
        {
            flowPanelKPICards.Controls.Clear();

            var avgPlastic = report.TotalRefills > 0
                ? report.PlasticSavedKg / report.TotalRefills
                : 0;

            var avgCO2 = report.TotalRefills > 0
                ? report.CO2SavedKg / report.TotalRefills
                : 0;

            var cards = new[]
            {
                new { Title = "Lần refill", Value = report.TotalRefills.ToString("N0"), Icon = "🔁", Color = Color.FromArgb(46, 204, 113) },
                new { Title = "Nhựa tiết kiệm (kg)", Value = $"{report.PlasticSavedKg:N2} kg", Icon = "🌱", Color = Color.FromArgb(56, 142, 60) },
                new { Title = "Nhựa tiết kiệm (tấn)", Value = $"{report.PlasticSavedTons:N3} tấn", Icon = "🏭", Color = Color.FromArgb(0, 150, 136) },
                new { Title = "CO₂ giảm thải (kg)", Value = $"{report.CO2SavedKg:N2} kg", Icon = "🌍", Color = Color.FromArgb(30, 136, 229) },
                new { Title = "Nhựa/refill", Value = $"{avgPlastic:N3} kg", Icon = "⚖️", Color = Color.FromArgb(255, 152, 0) },
                new { Title = "CO₂/refill", Value = $"{avgCO2:N3} kg", Icon = "💨", Color = Color.FromArgb(171, 71, 188) }
            };

            foreach (var cardInfo in cards)
            {
                var card = CreateKPICard(cardInfo.Title, cardInfo.Value, cardInfo.Icon, cardInfo.Color);
                card.Margin = new Padding(10, 5, 10, 5);
                card.Size = new Size(220, 100);
                flowPanelKPICards.Controls.Add(card);
            }
        }

        private void BuildPlasticReductionTable(IEnumerable<EnvironmentalImpactDataPoint> dataPoints)
        {
            var table = new DataTable();
            table.Columns.Add("STT", typeof(int));
            table.Columns.Add("Ngày", typeof(string));
            table.Columns.Add("Lần refill", typeof(string));
            table.Columns.Add("Nhựa tiết kiệm (kg)", typeof(string));
            table.Columns.Add("CO₂ tiết kiệm (kg)", typeof(string));

            int stt = 1;
            foreach (var point in dataPoints.OrderBy(p => p.Date))
            {
                table.Rows.Add(
                    stt++,
                    point.Date.ToString("dd/MM/yyyy"),
                    point.Refills.ToString("N0"),
                    $"{point.PlasticSavedKg:N2}",
                    $"{point.CO2SavedKg:N2}");
            }

            dataGridViewReport.DataSource = table;
            dataGridViewReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewReport.BringToFront();
        }

        private void BuildPlasticReductionChart(IEnumerable<EnvironmentalImpactDataPoint> dataPoints)
        {
            panelChart.Controls.Clear();
            panelChart.Padding = new Padding(20);

            var orderedPoints = dataPoints
                .OrderBy(p => p.Date)
                .ToList();

            if (orderedPoints.Count > 10)
            {
                orderedPoints = orderedPoints
                    .Skip(Math.Max(0, orderedPoints.Count - 10))
                    .ToList();
            }

            if (!orderedPoints.Any())
            {
                panelChart.Controls.Add(new Label
                {
                    Text = "Không có dữ liệu để hiển thị biểu đồ",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 10, FontStyle.Italic)
                });
                return;
            }

            var maxPlastic = orderedPoints.Max(p => p.PlasticSavedKg);
            if (maxPlastic <= 0)
            {
                panelChart.Controls.Add(new Label
                {
                    Text = "Nhựa tiết kiệm ở mức 0 kg trong giai đoạn này",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 10, FontStyle.Italic)
                });
                return;
            }

            var title = new Label
            {
                Text = "NHỰA TIẾT KIỆM THEO NGÀY (10 ngày gần nhất)",
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 107, 59),
                TextAlign = ContentAlignment.MiddleLeft
            };

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

                var lblDate = new Label
                {
                    Text = point.Date.ToString("dd/MM"),
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    AutoSize = false,
                    Width = 80
                };

                var percent = maxPlastic == 0 ? 0 : (int)Math.Round((point.PlasticSavedKg / maxPlastic) * 100);
                percent = Math.Min(Math.Max(percent, 0), 100);

                var bar = new ProgressBar
                {
                    Height = 18,
                    Width = 360,
                    Maximum = 100,
                    Value = percent
                };

                var lblValue = new Label
                {
                    Text = $"{point.PlasticSavedKg:N2} kg nhựa | {point.CO2SavedKg:N2} kg CO₂",
                    AutoSize = false,
                    Width = 240,
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.FromArgb(85, 85, 85)
                };

                lblDate.Location = new Point(0, 5);
                bar.Location = new Point(90, 5);
                lblValue.Location = new Point(460, 5);

                row.Controls.Add(lblDate);
                row.Controls.Add(bar);
                row.Controls.Add(lblValue);
                stack.Controls.Add(row);
            }

            panelChart.Controls.Add(stack);
            panelChart.Controls.Add(title);
        }
    }
}

