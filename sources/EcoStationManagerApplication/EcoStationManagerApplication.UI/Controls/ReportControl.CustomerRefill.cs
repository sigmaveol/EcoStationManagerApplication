using EcoStationManagerApplication.Core.Interfaces;
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
    /// Partial class for Customer Refill Report functionality
    /// </summary>
    public partial class ReportControl
    {
        private async Task LoadCustomerRefillReport(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var reportResult = await _reportService.GetCustomerReturnReportAsync(fromDate, toDate);

                if (!reportResult.Success)
                {
                    ShowPlaceholderMessage($"Không thể tải dữ liệu: {reportResult.Message ?? "Lỗi không xác định"}");
                    return;
                }

                if (reportResult.Data == null || reportResult.Data.CustomerData == null || reportResult.Data.CustomerData.Count == 0)
                {
                    ShowPlaceholderMessage("Không có dữ liệu để hiển thị");
                    return;
                }

                var customerData = reportResult.Data.CustomerData;

                var ordersResult = await _orderService.GetPagedOrdersAsync(
                    1, 10000,
                    new Models.DTOs.OrderSearchCriteria
                    {
                        FromDate = fromDate,
                        ToDate = toDate,
                        Status = OrderStatus.COMPLETED
                    });

                Dictionary<int, decimal> customerTotalValues = new Dictionary<int, decimal>();
                if (ordersResult.Success && ordersResult.Data.Orders != null)
                {
                    var orders = ordersResult.Data.Orders.ToList();
                    customerTotalValues = orders
                        .Where(o => o.CustomerId.HasValue)
                        .GroupBy(o => o.CustomerId.Value)
                        .ToDictionary(g => g.Key, g => g.Sum(o => o.TotalAmount - o.DiscountedAmount));
                }

                RemovePlaceholder();
                ClearReportContent();
                flowPanelKPICards.Visible = true;
                dataGridViewReport.Visible = true;
                panelChart.Visible = true;

                CreateCustomerRefillKPICards(customerData, customerTotalValues);
                CreateCustomerRefillDataTable(customerData, customerTotalValues);
                CreateChartPlaceholder();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải báo cáo: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ShowPlaceholderMessage($"Có lỗi xảy ra: {ex.Message}");
            }
        }

        private void CreateCustomerRefillKPICards(List<CustomerReturnData> customerData, Dictionary<int, decimal> customerTotalValues)
        {
            flowPanelKPICards.Controls.Clear();

            int totalReturningCustomers = customerData.Count(c => c.ReturnCount >= 2);
            int firstTimeRefillCustomers = customerData.Count(c => c.ReturnCount == 1);

            var mostFrequentCustomer = customerData
                .OrderByDescending(c => c.ReturnCount)
                .ThenByDescending(c => c.TotalOrders)
                .FirstOrDefault();

            string mostFrequentCustomerName = mostFrequentCustomer != null
                ? mostFrequentCustomer.CustomerName
                : "N/A";
            int mostFrequentRefillCount = mostFrequentCustomer?.ReturnCount ?? 0;

            var kpiData = new[]
            {
                new { Label = "KH quay lại >= 2 lần", Value = totalReturningCustomers.ToString("N0"), Icon = "🔄" },
                new { Label = "KH refill lần đầu", Value = firstTimeRefillCustomers.ToString("N0"), Icon = "🆕" },
                new { Label = "KH refill nhiều nhất", Value = mostFrequentCustomerName, Icon = "⭐" },
                new { Label = "Số lần refill (cao nhất)", Value = mostFrequentRefillCount.ToString("N0"), Icon = "📊" },
                new { Label = "Tổng số KH quay lại", Value = customerData.Count.ToString("N0"), Icon = "👥" }
            };

            foreach (var kpi in kpiData)
            {
                var card = ReportControlHelpers.CreateKPICard(kpi.Label, kpi.Value, kpi.Icon);
                card.Margin = new Padding(10, 5, 10, 5);
                card.Size = new Size(200, 100);
                flowPanelKPICards.Controls.Add(card);
            }
        }

        private void CreateCustomerRefillDataTable(List<CustomerReturnData> customerData, Dictionary<int, decimal> customerTotalValues)
        {
            dataGridViewReport.DataSource = null;
            dataGridViewReport.Columns.Clear();

            if (customerData == null || customerData.Count == 0)
                return;

            var dataTable = new DataTable();
            dataTable.TableName = "Báo cáo Tần suất khách hàng quay lại";
            dataTable.Columns.Add("Mã KH", typeof(string));
            dataTable.Columns.Add("Tên KH", typeof(string));
            dataTable.Columns.Add("Lần refill", typeof(int));
            dataTable.Columns.Add("Ngày gần nhất", typeof(string));
            dataTable.Columns.Add("Tổng giá trị", typeof(string));

            foreach (var customer in customerData.OrderByDescending(c => c.ReturnCount).ThenByDescending(c => c.TotalOrders))
            {
                decimal totalValue = customerTotalValues.ContainsKey(customer.CustomerId)
                    ? customerTotalValues[customer.CustomerId]
                    : 0;

                dataTable.Rows.Add(
                    $"KH-{customer.CustomerId:D5}",
                    customer.CustomerName,
                    customer.ReturnCount,
                    customer.LastOrderDate != DateTime.MinValue ? customer.LastOrderDate.ToString("dd/MM/yyyy") : "N/A",
                    ReportControlHelpers.FormatCurrency(totalValue)
                );
            }

            dataGridViewReport.DataSource = dataTable;
            dataGridViewReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewReport.ColumnHeadersVisible = true;
            dataGridViewReport.EnableHeadersVisualStyles = false;
        }
    }
}

