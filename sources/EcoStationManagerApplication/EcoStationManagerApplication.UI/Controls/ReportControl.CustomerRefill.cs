using EcoStationManagerApplication.Models.DTOs;
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
    /// Partial class handling the customer refill / loyalty report.
    /// </summary>
    public partial class ReportControl
    {
        protected async Task LoadCustomerRefillReport(DateTime fromDate, DateTime toDate)
        {
            try
            {
                ShowLoadingMessage("Đang tải dữ liệu báo cáo tần suất khách hàng...");

                var reportResult = await _reportService.GetCustomerReturnReportAsync(fromDate, toDate);
                if (!reportResult.Success || reportResult.Data?.CustomerData == null)
                {
                    ShowPlaceholderMessage($"Không thể tải dữ liệu: {reportResult.Message ?? "Lỗi không xác định"}");
                    return;
                }

                var customerData = reportResult.Data.CustomerData;
                if (!customerData.Any())
                {
                    ShowPlaceholderMessage("Không có dữ liệu khách hàng quay lại trong giai đoạn này.");
                    return;
                }

                var orderCriteria = new OrderSearchCriteria
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    Status = OrderStatus.COMPLETED
                };

                var ordersResult = await _orderService.GetPagedOrdersAsync(1, 10000, orderCriteria);
                var customerTotals = new Dictionary<int, decimal>();

                if (ordersResult.Success && ordersResult.Data.Orders != null)
                {
                    var orders = ordersResult.Data.Orders.ToList();
                    customerTotals = orders
                        .Where(o => o.CustomerId.HasValue)
                        .GroupBy(o => o.CustomerId.Value)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Sum(o => Math.Max(0, o.TotalAmount - o.DiscountedAmount)));
                }

                RemovePlaceholder();
                ClearReportContent();
                flowPanelKPICards.Visible = true;
                dataGridViewReport.Visible = true;
                panelChart.Visible = true;

                CreateCustomerRefillKPICards(customerData.ToList(), customerTotals);
                CreateCustomerRefillDataTable(customerData.ToList(), customerTotals);
                CreateCustomerRefillChart(customerData.ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải báo cáo: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ShowPlaceholderMessage($"Có lỗi xảy ra: {ex.Message}");
            }
        }

        private void CreateCustomerRefillKPICards(List<CustomerReturnData> customerData, Dictionary<int, decimal> customerTotals)
        {
            flowPanelKPICards.Controls.Clear();

            int returningCustomers = customerData.Count(c => c.ReturnCount >= 2);
            int firstTimer = customerData.Count(c => c.ReturnCount == 1);
            int loyalCustomers = customerData.Count(c => c.ReturnCount >= 5);
            decimal totalRevenue = customerTotals.Values.Sum();

            var bestCustomer = customerData
                .OrderByDescending(c => c.ReturnCount)
                .ThenByDescending(c => c.TotalOrders)
                .FirstOrDefault();

            var cards = new[]
            {
                new { Label = "KH quay lại ≥ 2 lần", Value = returningCustomers.ToString("N0"), Icon = "🔁", Color = Color.FromArgb(41, 128, 185) },
                new { Label = "KH refill lần đầu", Value = firstTimer.ToString("N0"), Icon = "🆕", Color = Color.FromArgb(39, 174, 96) },
                new { Label = "KH trung thành (≥5)", Value = loyalCustomers.ToString("N0"), Icon = "⭐", Color = Color.FromArgb(243, 156, 18) },
                new { Label = "KH refill nhiều nhất", Value = bestCustomer?.CustomerName ?? "N/A", Icon = "👑", Color = Color.FromArgb(155, 89, 182) },
                new { Label = "Số lần refill cao nhất", Value = (bestCustomer?.ReturnCount ?? 0).ToString("N0"), Icon = "📊", Color = Color.FromArgb(231, 76, 60) },
                new { Label = "Tổng doanh thu", Value = FormatCurrency(totalRevenue), Icon = "💰", Color = Color.FromArgb(52, 152, 219) }
            };

            foreach (var card in cards)
            {
                var control = CreateKPICard(card.Label, card.Value, card.Icon, card.Color);
                control.Margin = new Padding(10, 5, 10, 5);
                control.Size = new Size(220, 100);
                flowPanelKPICards.Controls.Add(control);
            }
        }

        private void CreateCustomerRefillDataTable(List<CustomerReturnData> customerData, Dictionary<int, decimal> customerTotals)
        {
            var table = new DataTable();
            table.Columns.Add("STT", typeof(int));
            table.Columns.Add("Mã KH", typeof(string));
            table.Columns.Add("Tên KH", typeof(string));
            table.Columns.Add("Số điện thoại", typeof(string));
            table.Columns.Add("Lần refill", typeof(int));
            table.Columns.Add("Tổng đơn", typeof(int));
            table.Columns.Add("Ngày gần nhất", typeof(string));
            table.Columns.Add("Tổng giá trị", typeof(string));
            table.Columns.Add("Loại KH", typeof(string));

            int index = 1;
            foreach (var customer in customerData
                .OrderByDescending(c => c.ReturnCount)
                .ThenByDescending(c => c.TotalOrders))
            {
                decimal totalValue = customerTotals.TryGetValue(customer.CustomerId, out var sum)
                    ? sum
                    : 0;

                table.Rows.Add(
                    index++,
                    $"KH-{customer.CustomerId:D5}",
                    customer.CustomerName,
                    customer.Phone ?? "N/A",
                    customer.ReturnCount,
                    customer.TotalOrders,
                    customer.LastOrderDate != DateTime.MinValue ? customer.LastOrderDate.ToString("dd/MM/yyyy") : "N/A",
                    FormatCurrency(totalValue),
                    GetCustomerType(customer.ReturnCount));
            }

            dataGridViewReport.DataSource = table;
            dataGridViewReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewReport.EnableHeadersVisualStyles = false;
        }

        private string GetCustomerType(int refillCount)
        {
            if (refillCount == 1) return "Mới";
            if (refillCount >= 2 && refillCount <= 4) return "Thường xuyên";
            if (refillCount >= 5) return "Trung thành";
            return "Khác";
        }

        private void CreateCustomerRefillChart(List<CustomerReturnData> customerData)
        {
            panelChart.Controls.Clear();
            panelChart.Padding = new Padding(20);

            var grouped = customerData
                .GroupBy(c => c.ReturnCount)
                .OrderBy(g => g.Key)
                .Select(g => new { ReturnCount = g.Key, CustomerCount = g.Count() })
                .ToList();

            if (!grouped.Any())
            {
                panelChart.Controls.Add(new Label
                {
                    Text = "Không có dữ liệu biểu đồ",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                });
                return;
            }

            var container = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true
            };

            int maxCount = grouped.Max(g => g.CustomerCount);

            foreach (var group in grouped)
            {
                var row = new Panel
                {
                    Width = panelChart.Width - 80,
                    Height = 45,
                    Margin = new Padding(0, 5, 0, 5)
                };

                var lblRange = new Label
                {
                    Text = $"{group.ReturnCount} lần",
                    Location = new Point(0, 10),
                    Width = 80,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold)
                };

                var progress = new ProgressBar
                {
                    Location = new Point(90, 10),
                    Width = 300,
                    Height = 18,
                    Maximum = Math.Max(1, maxCount),
                    Value = group.CustomerCount
                };

                var percent = (double)group.CustomerCount / customerData.Count * 100;
                var lblPercent = new Label
                {
                    Text = $"{group.CustomerCount} KH ({percent:0.0}%)",
                    Location = new Point(400, 10),
                    Width = 160,
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.FromArgb(31, 107, 59)
                };

                row.Controls.AddRange(new Control[] { lblRange, progress, lblPercent });
                container.Controls.Add(row);
            }

            var title = new Label
            {
                Text = "PHÂN BỐ SỐ LẦN REFILL",
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 107, 59)
            };

            panelChart.Controls.Add(container);
            panelChart.Controls.Add(title);
        }

    }
}