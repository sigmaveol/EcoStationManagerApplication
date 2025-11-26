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
    /// Partial class for best selling / product performance report.
    /// </summary>
    public partial class ReportControl
    {
        protected async Task LoadBestSellingReport(DateTime fromDate, DateTime toDate)
        {
            try
            {
                ShowLoadingMessage("Đang tải báo cáo mặt hàng bán chạy...");

                var topProductsResult = await _orderDetailService.GetTopSellingProductsAsync(10, fromDate, toDate);
                if (!topProductsResult.Success || topProductsResult.Data == null)
                {
                    ShowPlaceholderMessage($"Không thể tải dữ liệu: {topProductsResult.Message ?? "Lỗi không xác định"}");
                    return;
                }

                var products = topProductsResult.Data
                    .OrderByDescending(p => p.TotalQuantity)
                    .ToList();

                if (!products.Any())
                {
                    ShowPlaceholderMessage("Không có sản phẩm nào được bán trong giai đoạn này.");
                    return;
                }

                RemovePlaceholder();
                ClearReportContent();

                flowPanelKPICards.Visible = true;
                dataGridViewReport.Visible = true;
                panelChart.Visible = true;

                BuildBestSellingKpis(products);
                BuildBestSellingTable(products);
                BuildBestSellingChart(products);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải báo cáo bán chạy: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ShowPlaceholderMessage($"Có lỗi xảy ra: {ex.Message}");
            }
        }

        private void BuildBestSellingKpis(IList<ProductSales> products)
        {
            flowPanelKPICards.SuspendLayout();
            flowPanelKPICards.Controls.Clear();

            var totalQuantity = products.Sum(p => p.TotalQuantity);
            var totalRevenue = products.Sum(p => p.TotalRevenue);
            var topProduct = products.First();

            var kpis = new[]
            {
                new { Title = "Sản phẩm dẫn đầu", Value = topProduct.ProductName, Icon = "🏅", Color = Color.FromArgb(255, 152, 0) },
                new { Title = "SL bán ra (top 10)", Value = totalQuantity.ToString("N0"), Icon = "📦", Color = Color.FromArgb(33, 150, 243) },
                new { Title = "Doanh thu top 10", Value = FormatCurrency(totalRevenue), Icon = "💵", Color = Color.FromArgb(46, 204, 113) },
                new { Title = "Doanh thu/SP dẫn đầu", Value = FormatCurrency(topProduct.TotalRevenue), Icon = "💡", Color = Color.FromArgb(156, 39, 176) },
                new { Title = "SL bình quân/SP", Value = (products.Average(p => p.TotalQuantity)).ToString("N1"), Icon = "⚖️", Color = Color.FromArgb(0, 150, 136) }
            };

            foreach (var info in kpis)
            {
                var card = CreateKPICard(info.Title, info.Value, info.Icon, info.Color);
                card.Margin = new Padding(10, 5, 10, 5);
                card.Size = new Size(230, 100);
                flowPanelKPICards.Controls.Add(card);
            }
            flowPanelKPICards.ResumeLayout(true);
        }

        private void BuildBestSellingTable(IList<ProductSales> products)
        {
            dataGridViewReport.SuspendLayout();
            var totalQuantity = products.Sum(p => p.TotalQuantity);

            var table = new DataTable();
            table.Columns.Add("STT", typeof(int));
            table.Columns.Add("SKU", typeof(string));
            table.Columns.Add("Sản phẩm", typeof(string));
            table.Columns.Add("Số lượng", typeof(string));
            table.Columns.Add("Doanh thu", typeof(string));
            table.Columns.Add("Giá TB", typeof(string));
            table.Columns.Add("Tỉ trọng SL", typeof(string));

            int stt = 1;
            foreach (var product in products)
            {
                var avgPrice = product.TotalQuantity > 0
                    ? product.TotalRevenue / product.TotalQuantity
                    : 0;

                var share = totalQuantity > 0
                    ? (double)(product.TotalQuantity / totalQuantity) * 100
                    : 0;

                table.Rows.Add(
                    stt++,
                    product.Sku,
                    product.ProductName,
                    product.TotalQuantity.ToString("N0"),
                    FormatCurrency(product.TotalRevenue),
                    FormatCurrency(avgPrice),
                    $"{share:0.0}%");
            }

            dataGridViewReport.DataSource = table;
            dataGridViewReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewReport.BringToFront();
            dataGridViewReport.ResumeLayout(true);
        }

        private void BuildBestSellingChart(IList<ProductSales> products)
        {
            panelChart.SuspendLayout();
            panelChart.Controls.Clear();
            panelChart.Padding = new Padding(20);

            var topForChart = products.Take(8).ToList();
            var maxQuantity = topForChart.Max(p => p.TotalQuantity);
            if (maxQuantity <= 0)
            {
                panelChart.Controls.Add(new Label
                {
                    Text = "Không có số lượng bán ra để hiển thị biểu đồ",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 10, FontStyle.Italic)
                });
                return;
            }

            var title = new Label
            {
                Text = "TOP SẢN PHẨM THEO SỐ LƯỢNG BÁN",
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

            foreach (var product in topForChart)
            {
                var percent = maxQuantity > 0
                    ? (int)Math.Round((product.TotalQuantity / maxQuantity) * 100, MidpointRounding.AwayFromZero)
                    : 0;
                percent = Math.Min(Math.Max(percent, 0), 100);

                var row = new Panel
                {
                    Width = panelChart.Width - 80,
                    Height = 55,
                    Margin = new Padding(0, 5, 0, 5)
                };

                var nameLabel = new Label
                {
                    Text = product.ProductName,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    AutoSize = false,
                    Width = 250
                };

                var bar = new ProgressBar
                {
                    Height = 18,
                    Width = 320,
                    Maximum = 100,
                    Value = percent
                };

                var valueLabel = new Label
                {
                    Text = $"{product.TotalQuantity:N0} sp | {FormatCurrency(product.TotalRevenue)}",
                    AutoSize = false,
                    Width = 260,
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.FromArgb(85, 85, 85)
                };

                nameLabel.Location = new Point(0, 5);
                bar.Location = new Point(260, 5);
                valueLabel.Location = new Point(600, 5);

                row.Controls.Add(nameLabel);
                row.Controls.Add(bar);
                row.Controls.Add(valueLabel);
                stack.Controls.Add(row);
            }

            panelChart.Controls.Add(stack);
            panelChart.Controls.Add(title);
            panelChart.ResumeLayout(true);
        }
    }
}

