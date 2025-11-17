using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.UI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class ReportControl : UserControl
    {
        private readonly IReportService _reportService;
        private readonly IOrderService _orderService;
        private string _currentReportType = "Revenue"; // Default report type

        public ReportControl()
        {
            _reportService = AppServices.ReportService;
            _orderService = AppServices.OrderService;
            InitializeComponent();
            InitializeControls();
        }

        private void InitializeControls()
        {
            // Set default toggle (Revenue) - already set in Designer
            SetToggleButtonActive(btnToggleRevenue);

            // Set default time range to "Tháng này"
            cmbTimeRange.SelectedIndex = 2;
            UpdateDateRange();

            // Set default dates
            var today = DateTime.Now;
            dtpFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpToDate.Value = today;

            // Add tooltips for export buttons
            var toolTip = new ToolTip();
            toolTip.SetToolTip(btnExportPDF, "Xuất báo cáo ra PDF");
            toolTip.SetToolTip(btnExportExcel, "Xuất báo cáo ra Excel");

            // Initialize report content
            InitializeReportContent();
        }

        private void InitializeReportContent()
        {
            // Show placeholder
            ShowPlaceholderMessage("Chọn loại báo cáo và nhấn 'Tạo báo cáo' để xem dữ liệu");
        }

        private void ShowPlaceholderMessage(string message)
        {
            // Remove loading indicator
            RemovePlaceholder();

            // Hide content panels
            flowPanelKPICards.Visible = false;
            dataGridViewReport.Visible = false;
            panelChart.Visible = false;

            // Show placeholder
            var lblPlaceholder = new Label
            {
                Text = message,
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = Color.FromArgb(85, 85, 85),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Name = "lblPlaceholder"
            };
            panelReportContent.Controls.Add(lblPlaceholder);
        }

        private void RemovePlaceholder()
        {
            var placeholder = panelReportContent.Controls.Find("lblPlaceholder", false).FirstOrDefault();
            if (placeholder != null)
            {
                panelReportContent.Controls.Remove(placeholder);
                placeholder.Dispose();
            }

            var loading = panelReportContent.Controls.Find("lblLoading", false).FirstOrDefault();
            if (loading != null)
            {
                panelReportContent.Controls.Remove(loading);
                loading.Dispose();
            }
        }

        private void btnToggleReportType_Click(object sender, EventArgs e)
        {
            if (sender is Guna.UI2.WinForms.Guna2Button clickedButton)
            {
                // Set all buttons to inactive state
                SetAllToggleButtonsInactive();

                // Set clicked button to active state
                SetToggleButtonActive(clickedButton);

                // Update current report type
                if (clickedButton == btnToggleRevenue)
                    _currentReportType = "Revenue";
                else if (clickedButton == btnToggleCustomerRefill)
                    _currentReportType = "CustomerRefill";
                else if (clickedButton == btnTogglePackagingRecovery)
                    _currentReportType = "PackagingRecovery";
                else if (clickedButton == btnTogglePlasticReduction)
                    _currentReportType = "PlasticReduction";
                else if (clickedButton == btnTogglePaymentMethod)
                    _currentReportType = "PaymentMethod";
                else if (clickedButton == btnToggleBestSelling)
                    _currentReportType = "BestSelling";

                // Clear current content
                ClearReportContent();
            }
        }

        private void SetToggleButtonActive(Guna.UI2.WinForms.Guna2Button button)
        {
            button.FillColor = Color.FromArgb(31, 107, 59); // EcoStation green
            button.ForeColor = Color.White;
        }

        private void SetToggleButtonInactive(Guna.UI2.WinForms.Guna2Button button)
        {
            button.FillColor = Color.FromArgb(200, 200, 200); // Light gray
            button.ForeColor = Color.FromArgb(51, 51, 51); // Dark text
        }

        private void SetAllToggleButtonsInactive()
        {
            SetToggleButtonInactive(btnToggleRevenue);
            SetToggleButtonInactive(btnToggleCustomerRefill);
            SetToggleButtonInactive(btnTogglePackagingRecovery);
            SetToggleButtonInactive(btnTogglePlasticReduction);
            SetToggleButtonInactive(btnTogglePaymentMethod);
            SetToggleButtonInactive(btnToggleBestSelling);
        }

        private void ClearReportContent()
        {
            flowPanelKPICards.Controls.Clear();
            dataGridViewReport.DataSource = null;
            dataGridViewReport.Columns.Clear();
            panelChart.Controls.Clear();
        }

        private void cmbTimeRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDateRange();
            // Show/hide custom date range panel
            panelCustomDateRange.Visible = (cmbTimeRange.SelectedIndex == 4); // "Khoảng thời gian tùy chỉnh"
        }

        private void UpdateDateRange()
        {
            var today = DateTime.Now;
            DateTime fromDate, toDate;

            switch (cmbTimeRange.SelectedIndex)
            {
                case 0: // Hôm nay
                    fromDate = today.Date;
                    toDate = today.Date.AddDays(1).AddSeconds(-1);
                    break;
                case 1: // 7 ngày qua
                    fromDate = today.Date.AddDays(-6);
                    toDate = today.Date.AddDays(1).AddSeconds(-1);
                    break;
                case 2: // Tháng này
                    fromDate = new DateTime(today.Year, today.Month, 1);
                    toDate = today.Date.AddDays(1).AddSeconds(-1);
                    break;
                case 3: // Tháng trước
                    var lastMonth = today.AddMonths(-1);
                    fromDate = new DateTime(lastMonth.Year, lastMonth.Month, 1);
                    toDate = new DateTime(today.Year, today.Month, 1).AddSeconds(-1);
                    break;
                case 4: // Khoảng thời gian tùy chỉnh
                    fromDate = dtpFromDate.Value.Date;
                    toDate = dtpToDate.Value.Date.AddDays(1).AddSeconds(-1);
                    break; // Don't update date pickers if custom range
                default:
                    fromDate = new DateTime(today.Year, today.Month, 1);
                    toDate = today.Date.AddDays(1).AddSeconds(-1);
                    break;
            }

            // Update date pickers
            dtpFromDate.Value = fromDate;
            dtpToDate.Value = toDate;
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                // Get date range
                DateTime fromDate = dtpFromDate.Value.Date;
                DateTime toDate = dtpToDate.Value.Date.AddDays(1).AddSeconds(-1);

                // Validate date range
                if (fromDate > toDate)
                {
                    MessageBox.Show("Ngày bắt đầu không thể lớn hơn ngày kết thúc!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Load report data based on current report type
                LoadReportData(fromDate, toDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo báo cáo: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void LoadReportData(DateTime fromDate, DateTime toDate)
        {
            try
            {
                // Show loading indicator
                ShowLoadingIndicator();

                // Load data based on report type
                switch (_currentReportType)
                {
                    case "Revenue":
                        await LoadRevenueReport(fromDate, toDate);
                        break;
                    case "CustomerRefill":
                        await LoadCustomerRefillReport(fromDate, toDate);
                        break;
                    case "PackagingRecovery":
                        await LoadPackagingRecoveryReport(fromDate, toDate);
                        break;
                    case "PlasticReduction":
                        await LoadPlasticReductionReport(fromDate, toDate);
                        break;
                    case "PaymentMethod":
                        await LoadPaymentMethodReport(fromDate, toDate);
                        break;
                    case "BestSelling":
                        await LoadBestSellingReport(fromDate, toDate);
                        break;
                    default:
                        ShowPlaceholderMessage("Loại báo cáo chưa được triển khai");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}\n\nChi tiết: {ex.StackTrace}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ShowPlaceholderMessage($"Có lỗi xảy ra khi tải dữ liệu: {ex.Message}");
            }
        }

        private void ShowLoadingIndicator()
        {
            // Remove placeholder if exists
            RemovePlaceholder();

            // Hide content panels
            flowPanelKPICards.Visible = false;
            dataGridViewReport.Visible = false;
            panelChart.Visible = false;

            // Show loading
            var lblLoading = new Label
            {
                Text = "Đang tải dữ liệu từ cơ sở dữ liệu...",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = Color.FromArgb(85, 85, 85),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Name = "lblLoading"
            };
            panelReportContent.Controls.Add(lblLoading);
        }

        private void CreateChartPlaceholder()
        {
            panelChart.Controls.Clear();
            var lblChart = new Label
            {
                Text = "\n(Biểu đồ sẽ được tích hợp sau)",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(85, 85, 85),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            panelChart.Controls.Add(lblChart);
        }

        private async Task LoadPackagingRecoveryReport(DateTime fromDate, DateTime toDate)
        {
            await Task.Delay(100);
            ShowPlaceholderMessage("Báo cáo Tỷ lệ thu hồi bao bì sẽ được triển khai sau");
        }

        private async Task LoadPlasticReductionReport(DateTime fromDate, DateTime toDate)
        {
            await Task.Delay(100);
            ShowPlaceholderMessage("Báo cáo Lượng nhựa giảm phát thải sẽ được triển khai sau");
        }

        private async Task LoadPaymentMethodReport(DateTime fromDate, DateTime toDate)
        {
            await Task.Delay(100);
            ShowPlaceholderMessage("Báo cáo Phương thức thanh toán sẽ được triển khai sau");
        }

        private async Task LoadBestSellingReport(DateTime fromDate, DateTime toDate)
        {
            await Task.Delay(100);
            ShowPlaceholderMessage("Báo cáo Mặt hàng bán chạy sẽ được triển khai sau");
        }

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            try
            {
                // TODO: Implement PDF export
                MessageBox.Show("Tính năng xuất PDF sẽ được triển khai trong các bước tiếp theo.",
                    "Xuất PDF", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất PDF: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                // TODO: Implement Excel export
                MessageBox.Show("Tính năng xuất Excel sẽ được triển khai trong các bước tiếp theo.",
                    "Xuất Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất Excel: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
