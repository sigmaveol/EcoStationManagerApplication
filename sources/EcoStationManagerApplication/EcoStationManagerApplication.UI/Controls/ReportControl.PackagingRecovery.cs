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
    /// Partial class for Packaging Recovery report logic
    /// </summary>
    public partial class ReportControl
    {
        protected async Task LoadPackagingRecoveryReport(DateTime fromDate, DateTime toDate)
        {
            try
            {
                ShowLoadingMessage("Đang tải báo cáo tỷ lệ thu hồi bao bì...");

                var reportResult = await _reportService.GetPackagingRecoveryReportAsync(fromDate, toDate);
                if (!reportResult.Success || reportResult.Data == null)
                {
                    ShowPlaceholderMessage($"Không thể tải dữ liệu: {reportResult.Message ?? "Lỗi không xác định"}");
                    return;
                }

                var reportData = reportResult.Data;
                var details = reportData.PackagingData ?? new List<PackagingRecoveryData>();
                if (!details.Any())
                {
                    ShowPlaceholderMessage("Không có dữ liệu thu hồi bao bì trong khoảng thời gian này.");
                    return;
                }

                RemovePlaceholder();
                ClearReportContent();
                flowPanelKPICards.Visible = true;
                panelChart.Visible = true;
                dataGridViewReport.Visible = true;

                BuildPackagingRecoveryKpis(reportData);
                BuildPackagingRecoveryTable(details);
                BuildPackagingRecoveryChart(details);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải báo cáo thu hồi bao bì: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ShowPlaceholderMessage($"Có lỗi xảy ra: {ex.Message}");
            }
        }

        private void BuildPackagingRecoveryKpis(PackagingRecoveryReportDTO report)
        {
            flowPanelKPICards.Controls.Clear();

            var best = report.PackagingData.OrderByDescending(p => p.RecoveryRate).FirstOrDefault();
            var worst = report.PackagingData.OrderBy(p => p.RecoveryRate).FirstOrDefault();

            var kpiCards = new[]
            {
                new { Title = "Bao bì phát hành", Value = report.TotalIssued.ToString("N0"), Icon = "📦", Color = Color.FromArgb(33, 150, 243) },
                new { Title = "Bao bì thu hồi", Value = report.TotalReturned.ToString("N0"), Icon = "♻️", Color = Color.FromArgb(76, 175, 80) },
                new { Title = "Tỷ lệ thu hồi", Value = $"{report.RecoveryRate:0.00}%", Icon = "📈", Color = Color.FromArgb(255, 152, 0) },
                new { Title = "Hiệu suất cao nhất", Value = best != null ? $"{best.PackagingName} ({best.RecoveryRate:0.00}%)" : "N/A", Icon = "🏆", Color = Color.FromArgb(156, 39, 176) },
                new { Title = "Hiệu suất thấp nhất", Value = worst != null ? $"{worst.PackagingName} ({worst.RecoveryRate:0.00}%)" : "N/A", Icon = "⚠️", Color = Color.FromArgb(244, 67, 54) }
            };

            foreach (var kpi in kpiCards)
            {
                var card = CreateKPICard(kpi.Title, kpi.Value, kpi.Icon, kpi.Color);
                card.Margin = new Padding(10, 5, 10, 5);
                card.Size = new Size(220, 100);
                flowPanelKPICards.Controls.Add(card);
            }
        }

        private void BuildPackagingRecoveryTable(List<PackagingRecoveryData> data)
        {
            var table = new DataTable();
            table.Columns.Add("STT", typeof(int));
            table.Columns.Add("Mã bao bì", typeof(string));
            table.Columns.Add("Tên bao bì", typeof(string));
            table.Columns.Add("Phát hành", typeof(string));
            table.Columns.Add("Thu hồi", typeof(string));
            table.Columns.Add("Tỷ lệ thu hồi", typeof(string));

            int stt = 1;
            foreach (var item in data.OrderByDescending(d => d.RecoveryRate))
            {
                table.Rows.Add(
                    stt++,
                    $"PKG-{item.PackagingId:D4}",
                    item.PackagingName,
                    item.Issued.ToString("N0"),
                    item.Returned.ToString("N0"),
                    $"{item.RecoveryRate:0.00}%");
            }

            dataGridViewReport.DataSource = table;
            dataGridViewReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewReport.BringToFront();
        }

        private void BuildPackagingRecoveryChart(List<PackagingRecoveryData> data)
        {
            panelChart.Controls.Clear();
            panelChart.Padding = new Padding(20);

            var topItems = data.OrderByDescending(d => d.RecoveryRate).Take(6).ToList();
            if (!topItems.Any())
            {
                panelChart.Controls.Add(new Label
                {
                    Text = "Không có dữ liệu để vẽ biểu đồ",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 10, FontStyle.Italic)
                });
                return;
            }

            var title = new Label
            {
                Text = "TOP BAO BÌ THU HỒI TỐT NHẤT",
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

            foreach (var item in topItems)
            {
                var row = new Panel
                {
                    Width = panelChart.Width - 80,
                    Height = 55,
                    Margin = new Padding(0, 5, 0, 5)
                };

                var nameLabel = new Label
                {
                    Text = item.PackagingName,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    AutoSize = false,
                    Width = 220
                };

                var progress = new ProgressBar
                {
                    Height = 18,
                    Width = 360,
                    Maximum = 100,
                    Value = ClampPercent(item.RecoveryRate)
                };

                var valueLabel = new Label
                {
                    Text = $"{item.RecoveryRate:0.00}% (Thu hồi {item.Returned:N0}/{item.Issued:N0})",
                    AutoSize = false,
                    Width = 220,
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.FromArgb(85, 85, 85)
                };

                nameLabel.Location = new Point(0, 5);
                progress.Location = new Point(230, 5);
                valueLabel.Location = new Point(600, 5);

                row.Controls.Add(nameLabel);
                row.Controls.Add(progress);
                row.Controls.Add(valueLabel);
                stack.Controls.Add(row);
            }

            panelChart.Controls.Add(stack);
            panelChart.Controls.Add(title);
        }

        private int ClampPercent(double value) =>
            (int)Math.Max(0, Math.Min(100, Math.Round(value, MidpointRounding.AwayFromZero)));
    }
}

