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
    /// <summary>
    /// Report Control for displaying various business reports
    /// </summary>
    public partial class ReportControl : UserControl
    {
        private readonly IReportService _reportService;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private ReportType _currentReportType = ReportType.REVENUE;
        private bool _isLoadingReport = false;

        public ReportControl(IReportService reportService, IOrderService orderService, IOrderDetailService orderDetailService)
        {
            _reportService = reportService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            InitializeComponent();
            InitializeEvents();
            SetDefaultDates();
        }

        private void InitializeEvents()
        {
            cmbTimeRange.SelectedIndexChanged += cmbTimeRange_SelectedIndexChanged;
            btnGenerateReport.Click += btnGenerateReport_Click;
            btnExportPDF.Click += btnExportPDF_Click;
            btnExportExcel.Click += btnExportExcel_Click;
        }

        private void SetDefaultDates()
        {
            dtpFromDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpToDate.Value = DateTime.Now;
            cmbTimeRange.SelectedIndex = 2; // Tháng này
        }

        private void cmbTimeRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            var today = DateTime.Today;
            panelCustomDateRange.Visible = cmbTimeRange.SelectedIndex == 4; // Custom range

            switch (cmbTimeRange.SelectedIndex)
            {
                case 0: // Hôm nay
                    dtpFromDate.Value = today;
                    dtpToDate.Value = today;
                    break;
                case 1: // 7 ngày qua
                    dtpFromDate.Value = today.AddDays(-6);
                    dtpToDate.Value = today;
                    break;
                case 2: // Tháng này
                    dtpFromDate.Value = new DateTime(today.Year, today.Month, 1);
                    dtpToDate.Value = today;
                    break;
                case 3: // Tháng trước
                    var firstDayLastMonth = new DateTime(today.Year, today.Month, 1).AddMonths(-1);
                    dtpFromDate.Value = firstDayLastMonth;
                    dtpToDate.Value = firstDayLastMonth.AddMonths(1).AddDays(-1);
                    break;
            }
        }

        private async void btnGenerateReport_Click(object sender, EventArgs e)
        {
            if (dtpFromDate.Value > dtpToDate.Value)
            {
                MessageBox.Show("Ngày bắt đầu không thể lớn hơn ngày kết thúc", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            await LoadReport();
        }

        private async Task LoadReport()
        {
            // Tránh reload đồng thời nhiều lần
            if (_isLoadingReport) return;
            
            try
            {
                _isLoadingReport = true;
                var fromDate = dtpFromDate.Value.Date;
                var toDate = dtpToDate.Value.Date.AddDays(1).AddSeconds(-1);

                switch (_currentReportType)
                {
                    case ReportType.REVENUE:
                        await LoadRevenueReport(fromDate, toDate);
                        break;
                    case ReportType.CUSTOMER_REFILL:
                        await LoadCustomerRefillReport(fromDate, toDate);
                        break;
                    case ReportType.PACKAGING_RECOVERY:
                        await LoadPackagingRecoveryReport(fromDate, toDate);
                        break;
                    case ReportType.PLASTIC_REDUCTION:
                        await LoadPlasticReductionReport(fromDate, toDate);
                        break;
                    case ReportType.PAYMENT_METHOD:
                        await LoadPaymentMethodReport(fromDate, toDate);
                        break;
                    case ReportType.BEST_SELLING:
                        await LoadBestSellingReport(fromDate, toDate);
                        break;
                }

                // Refresh UI để đảm bảo các thay đổi được hiển thị
                this.Refresh();
            }
            finally
            {
                _isLoadingReport = false;
            }
        }

        // UI Helper Methods
        private void ShowLoadingMessage(string message)
        {
            ClearReportContent();
            var label = new Label
            {
                Text = message,
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = Color.FromArgb(85, 85, 85),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = false
            };
            panelReportContent.Controls.Add(label);
        }
        private void ShowPlaceholderMessage(string message)
        {
            ClearReportContent();
            flowPanelKPICards.Visible = false;
            dataGridViewReport.Visible = false;
            panelChart.Visible = false;

            var label = new Label
            {
                Text = message,
                Font = new Font("Segoe UI", 14, FontStyle.Regular),
                ForeColor = Color.FromArgb(85, 85, 85),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = false
            };
            panelReportContent.Controls.Add(label);
        }

        protected void RemovePlaceholder()
        {
            foreach (Control control in panelReportContent.Controls.OfType<Label>().ToList())
            {
                panelReportContent.Controls.Remove(control);
                control.Dispose();
            }
        }

        protected void ClearReportContent()
        {
            flowPanelKPICards.Controls.Clear();
            dataGridViewReport.DataSource = null;
            dataGridViewReport.Columns.Clear();
            panelChart.Controls.Clear();

            foreach (Control control in panelReportContent.Controls.OfType<Label>().ToList())
            {
                panelReportContent.Controls.Remove(control);
                control.Dispose();
            }
        }

        protected Panel CreateKPICard(string title, string value, string icon, Color color)
        {
            var card = new Panel
            {
                Size = new Size(200, 100),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(15)
            };

            // Add shadow effect
            card.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(30, 0, 0, 0), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                }
            };

            var lblIcon = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = color,
                Location = new Point(15, 15),
                AutoSize = true
            };

            var lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(50, 12),
                AutoSize = true
            };

            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.FromArgb(85, 85, 85),
                Location = new Point(15, 55),
                AutoSize = true
            };

            card.Controls.Add(lblIcon);
            card.Controls.Add(lblValue);
            card.Controls.Add(lblTitle);

            return card;
        }

        protected string FormatCurrency(decimal amount)
        {
            return amount.ToString("C0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
        }

        // Event Handlers
        private void btnToggleReportType_Click(object sender, EventArgs e)
        {
            var clickedButton = sender as Guna.UI2.WinForms.Guna2Button;
            if (clickedButton == null) return;

            // Reset all buttons to default style
            foreach (Guna.UI2.WinForms.Guna2Button btn in flowPanelReportTypes.Controls)
            {
                btn.FillColor = Color.FromArgb(200, 200, 200);
                btn.ForeColor = Color.FromArgb(51, 51, 51);
            }

            // Set active button style
            clickedButton.FillColor = Color.FromArgb(31, 107, 59);
            clickedButton.ForeColor = Color.White;

            // Update current report type
            _currentReportType = GetReportTypeFromButton(clickedButton);

            // Update UI labels based on report type
            UpdateReportTitle();

            // Clear nội dung báo cáo cũ khi chuyển đổi report type
            ClearReportContent();
            ShowPlaceholderMessage("Vui lòng chọn khoảng thời gian và nhấn 'Tạo báo cáo' để xem dữ liệu.");
        }

        private ReportType GetReportTypeFromButton(Guna.UI2.WinForms.Guna2Button button)
        {
            if (button == btnToggleRevenue) return ReportType.REVENUE;
            if (button == btnToggleCustomerRefill) return ReportType.CUSTOMER_REFILL;
            if (button == btnTogglePackagingRecovery) return ReportType.PACKAGING_RECOVERY;
            if (button == btnTogglePlasticReduction) return ReportType.PLASTIC_REDUCTION;
            if (button == btnTogglePaymentMethod) return ReportType.PAYMENT_METHOD;
            if (button == btnToggleBestSelling) return ReportType.BEST_SELLING;
            return ReportType.REVENUE;
        }

        private void UpdateReportTitle()
        {
            var titles = new Dictionary<ReportType, (string Title, string Description)>
            {
                [ReportType.REVENUE] = ("Báo cáo Doanh thu", "Phân tích doanh thu và tăng trưởng theo thời gian"),
                [ReportType.CUSTOMER_REFILL] = ("Báo cáo Tần suất KH quay lại", "Theo dõi tần suất refill và khách hàng trung thành"),
                [ReportType.PACKAGING_RECOVERY] = ("Báo cáo Thu hồi bao bì", "Tỷ lệ thu hồi và tái sử dụng bao bì"),
                [ReportType.PLASTIC_REDUCTION] = ("Báo cáo Giảm phát thải nhựa", "Lượng nhựa tiết kiệm được từ refill"),
                [ReportType.PAYMENT_METHOD] = ("Báo cáo Phương thức thanh toán", "Phân tích xu hướng thanh toán của khách hàng"),
                [ReportType.BEST_SELLING] = ("Báo cáo Mặt hàng bán chạy", "Top sản phẩm và dịch vụ được ưa chuộng")
            };

            if (titles.ContainsKey(_currentReportType))
            {
                var (title, description) = titles[_currentReportType];
                lblReportTitle.Text = title;
                lblReportDescription.Text = description;
            }
        }

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem có dữ liệu để xuất không
                if (dataGridViewReport.DataSource == null || !dataGridViewReport.Visible)
                {
                    MessageBox.Show("Không có dữ liệu để xuất. Vui lòng tạo báo cáo trước.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Lấy DataTable từ DataSource
                var dataTable = dataGridViewReport.DataSource as DataTable;
                if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Hiển thị SaveFileDialog
                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "PDF files (*.pdf)|*.pdf";
                    saveDialog.FileName = $"BaoCao_{GetReportTypeName(_currentReportType)}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                    saveDialog.Title = "Xuất báo cáo ra PDF";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        var fromDate = dtpFromDate.Value.Date;
                        var toDate = dtpToDate.Value.Date;
                        var reportTitle = GetReportTitleForExport();

                        // Xuất PDF sử dụng FastReportHelper
                        FastReportHelper.ExportToPdf(dataTable, saveDialog.FileName, reportTitle, fromDate, toDate);

                        MessageBox.Show($"Đã xuất PDF thành công!\nFile: {saveDialog.FileName}", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
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
                // Kiểm tra xem có dữ liệu để xuất không
                if (dataGridViewReport.DataSource == null || !dataGridViewReport.Visible)
                {
                    MessageBox.Show("Không có dữ liệu để xuất. Vui lòng tạo báo cáo trước.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Lấy DataTable từ DataSource
                var dataTable = dataGridViewReport.DataSource as DataTable;
                if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Hiển thị SaveFileDialog
                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                    saveDialog.FileName = $"BaoCao_{GetReportTypeName(_currentReportType)}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    saveDialog.Title = "Xuất báo cáo ra Excel";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        var fromDate = dtpFromDate.Value.Date;
                        var toDate = dtpToDate.Value.Date;
                        var reportTitle = GetReportTitleForExport();

                        // Xuất Excel sử dụng FastReportHelper
                        FastReportHelper.ExportToExcel(dataTable, saveDialog.FileName, reportTitle, fromDate, toDate);

                        MessageBox.Show($"Đã xuất Excel thành công!\nFile: {saveDialog.FileName}", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất Excel: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetReportTitleForExport()
        {
            switch (_currentReportType)
            {
                case ReportType.REVENUE:
                    return "Báo cáo Doanh thu";
                case ReportType.CUSTOMER_REFILL:
                    return "Báo cáo Tần suất Khách hàng Quay lại";
                case ReportType.PACKAGING_RECOVERY:
                    return "Báo cáo Thu hồi Bao bì";
                case ReportType.PLASTIC_REDUCTION:
                    return "Báo cáo Giảm phát thải Nhựa";
                case ReportType.PAYMENT_METHOD:
                    return "Báo cáo Phương thức Thanh toán";
                case ReportType.BEST_SELLING:
                    return "Báo cáo Mặt hàng Bán chạy";
                default:
                    return "Báo cáo";
            }
        }

        private string GetReportTypeName(ReportType reportType)
        {
            switch (reportType)
            {
                case ReportType.REVENUE:
                    return "DoanhThu";
                case ReportType.CUSTOMER_REFILL:
                    return "KhachHangQuayLai";
                case ReportType.PACKAGING_RECOVERY:
                    return "ThuHoiBaoBi";
                case ReportType.PLASTIC_REDUCTION:
                    return "GiamPhatThaiNhua";
                case ReportType.PAYMENT_METHOD:
                    return "PhuongThucThanhToan";
                case ReportType.BEST_SELLING:
                    return "MatHangBanChay";
                default:
                    return "BaoCao";
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Set revenue as default active
            btnToggleRevenue.PerformClick();
            
            // Hiển thị placeholder message
            ClearReportContent();
            ShowPlaceholderMessage("Vui lòng chọn khoảng thời gian và nhấn 'Tạo báo cáo' để xem dữ liệu.");
        }

        // Placeholder methods - được implement trong các file partial
        // Các method này được định nghĩa trong:
        // - ReportControl.Revenue.cs
        // - ReportControl.CustomerRefill.cs
        // - ReportControl.PackagingRecovery.cs
        // - ReportControl.PlasticReduction.cs
        // - ReportControl.PaymentMethod.cs
        // - ReportControl.BestSelling.cs
    }

    public enum ReportType
    {
        REVENUE,
        CUSTOMER_REFILL,
        PACKAGING_RECOVERY,
        PLASTIC_REDUCTION,
        PAYMENT_METHOD,
        BEST_SELLING
    }
}
