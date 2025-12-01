using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
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
    /// Partial class for payment method insight report
    /// </summary>
    public partial class ReportControl
    {
        protected async Task LoadPaymentMethodReport(DateTime fromDate, DateTime toDate)
        {
            try
            {
                ShowLoadingMessage("Đang phân tích phương thức thanh toán...");

                var criteria = new OrderSearchCriteria
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    Status = OrderStatus.COMPLETED
                };

                var orderResult = await _orderService.GetPagedOrdersAsync(1, 5000, criteria);
                if (!orderResult.Success || orderResult.Data.Orders == null)
                {
                    ShowPlaceholderMessage($"Không thể tải dữ liệu: {orderResult.Message ?? "Lỗi không xác định"}");
                    return;
                }

                var orders = orderResult.Data.Orders.ToList();
                if (!orders.Any())
                {
                    ShowPlaceholderMessage("Không có đơn hàng hoàn tất trong giai đoạn này.");
                    return;
                }

                RemovePlaceholder();
                ClearReportContent();

                flowPanelKPICards.Visible = true;
                dataGridViewReport.Visible = true;
                panelChart.Visible = true;

                var stats = BuildPaymentMethodStats(orders);

                BuildPaymentMethodKpis(stats, orders);
                BuildPaymentMethodTable(stats);
                BuildPaymentMethodChart(stats);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải báo cáo thanh toán: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ShowPlaceholderMessage($"Có lỗi xảy ra: {ex.Message}");
            }
        }

        private List<PaymentMethodStat> BuildPaymentMethodStats(List<Order> orders)
        {
            decimal NetAmount(Order order)
            {
                var net = order.TotalAmount - order.DiscountedAmount;
                return net < 0 ? order.TotalAmount : net;
            }

            return orders
                .GroupBy(o => o.PaymentMethod)
                .Select(g => new PaymentMethodStat
                {
                    Method = g.Key,
                    OrderCount = g.Count(),
                    Revenue = g.Sum(NetAmount),
                    PaidOrders = g.Count(o => o.PaymentStatus == PaymentStatus.PAID)
                })
                .OrderByDescending(s => s.OrderCount)
                .ToList();
        }

        private void BuildPaymentMethodKpis(List<PaymentMethodStat> stats, List<Order> orders)
        {
            flowPanelKPICards.SuspendLayout();
            flowPanelKPICards.Controls.Clear();

            var totalOrders = orders.Count;
            var totalRevenue = stats.Sum(s => s.Revenue);
            var paidOrders = orders.Count(o => o.PaymentStatus == PaymentStatus.PAID);
            var topMethod = stats.FirstOrDefault();

            var cashShare = stats.FirstOrDefault(s => s.Method == PaymentMethod.CASH)?.OrderCount ?? 0;
            double cashRatio = totalOrders > 0 ? (double)cashShare / totalOrders * 100 : 0;

            var kpis = new[]
            {
                new { Title = "Tổng đơn hoàn tất", Value = totalOrders.ToString("N0"), Icon = "🧾", Color = Color.FromArgb(33, 150, 243) },
                new { Title = "Doanh thu thuần", Value = FormatCurrency(totalRevenue), Icon = "💰", Color = Color.FromArgb(46, 204, 113) },
                new { Title = "Đơn đã thanh toán", Value = $"{paidOrders:N0} ({(totalOrders > 0 ? (double)paidOrders / totalOrders * 100 : 0):0.0}%)", Icon = "✅", Color = Color.FromArgb(0, 150, 136) },
                new { Title = "Top phương thức", Value = topMethod != null ? $"{GetPaymentMethodDisplayName(topMethod.Method)} ({topMethod.OrderCount:N0} đơn)" : "N/A", Icon = "🏆", Color = Color.FromArgb(255, 152, 0) },
                new { Title = "Tỉ trọng tiền mặt", Value = $"{cashRatio:0.0}%", Icon = "💵", Color = Color.FromArgb(156, 39, 176) }
            };

            foreach (var cardInfo in kpis)
            {
                var card = CreateKPICard(cardInfo.Title, cardInfo.Value, cardInfo.Icon, cardInfo.Color);
                card.Margin = new Padding(10, 5, 10, 5);
                card.Size = new Size(230, 100);
                flowPanelKPICards.Controls.Add(card);
            }
            flowPanelKPICards.ResumeLayout(true);
        }

        private void BuildPaymentMethodTable(List<PaymentMethodStat> stats)
        {
            dataGridViewReport.SuspendLayout();
            var totalOrders = stats.Sum(s => s.OrderCount);
            var totalRevenue = stats.Sum(s => s.Revenue);

            var table = new DataTable();
            table.Columns.Add("STT", typeof(int));
            table.Columns.Add("Phương thức", typeof(string));
            table.Columns.Add("Số đơn", typeof(string));
            table.Columns.Add("Tỉ lệ đơn", typeof(string));
            table.Columns.Add("Doanh thu", typeof(string));
            table.Columns.Add("Tỉ trọng doanh thu", typeof(string));
            table.Columns.Add("Giá trị TB/đơn", typeof(string));

            int stt = 1;
            foreach (var stat in stats)
            {
                double orderShare = totalOrders > 0 ? (double)stat.OrderCount / totalOrders * 100 : 0;
                double revenueShare = totalRevenue > 0 ? (double)(stat.Revenue / totalRevenue) * 100 : 0;
                var avgOrder = stat.OrderCount > 0 ? stat.Revenue / stat.OrderCount : 0;

                table.Rows.Add(
                    stt++,
                    GetPaymentMethodDisplayName(stat.Method),
                    stat.OrderCount.ToString("N0"),
                    $"{orderShare:0.0}%",
                    FormatCurrency(stat.Revenue),
                    $"{revenueShare:0.0}%",
                    FormatCurrency(avgOrder));
            }

            dataGridViewReport.DataSource = table;
            dataGridViewReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewReport.BringToFront();
            dataGridViewReport.ResumeLayout(true);
        }

        private void BuildPaymentMethodChart(List<PaymentMethodStat> stats)
        {
            panelChart.SuspendLayout();
            panelChart.Controls.Clear();
            panelChart.Padding = new Padding(20);

            var totalRevenue = stats.Sum(s => s.Revenue);
            if (totalRevenue <= 0)
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

            var title = new Label
            {
                Text = "TỈ TRỌNG DOANH THU THEO PHƯƠNG THỨC THANH TOÁN",
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

            foreach (var stat in stats)
            {
                var share = totalRevenue > 0 ? (double)(stat.Revenue / totalRevenue) * 100 : 0;
                share = Math.Min(Math.Max(share, 0), 100);

                var row = new Panel
                {
                    Width = panelChart.Width - 80,
                    Height = 55,
                    Margin = new Padding(0, 5, 0, 5)
                };

                var nameLabel = new Label
                {
                    Text = GetPaymentMethodDisplayName(stat.Method),
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    AutoSize = false,
                    Width = 150
                };

                var bar = new ProgressBar
                {
                    Height = 18,
                    Width = 360,
                    Maximum = 100,
                    Value = (int)Math.Round(share)
                };

                var valueLabel = new Label
                {
                    Text = $"{share:0.0}% ({FormatCurrency(stat.Revenue)})",
                    AutoSize = false,
                    Width = 260,
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.FromArgb(85, 85, 85)
                };

                nameLabel.Location = new Point(0, 5);
                bar.Location = new Point(160, 5);
                valueLabel.Location = new Point(530, 5);

                row.Controls.Add(nameLabel);
                row.Controls.Add(bar);
                row.Controls.Add(valueLabel);
                stack.Controls.Add(row);
            }

            panelChart.Controls.Add(stack);
            panelChart.Controls.Add(title);
            panelChart.ResumeLayout(true);
        }

        private string GetPaymentMethodDisplayName(PaymentMethod method)
        {
            switch (method)
            {
                case PaymentMethod.CASH:
                    return "Tiền mặt";
                case PaymentMethod.TRANSFER:
                    return "Chuyển khoản";
                default:
                    return method.ToString();
            }
        }

        private class PaymentMethodStat
        {
            public PaymentMethod Method { get; set; }
            public int OrderCount { get; set; }
            public decimal Revenue { get; set; }
            public int PaidOrders { get; set; }
        }
    }
}

