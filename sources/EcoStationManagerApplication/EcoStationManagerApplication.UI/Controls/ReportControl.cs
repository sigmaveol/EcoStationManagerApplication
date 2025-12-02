using EcoStationManagerApplication.Common.Exporters;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.UI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;

namespace EcoStationManagerApplication.UI.Controls
{
    /// <summary>
    /// Report Control for displaying various business reports
    /// </summary>
    public partial class ReportControl : UserControl, IRefreshableControl
    {
        private readonly IReportService _reportService;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private ReportType _currentReportType = ReportType.REVENUE;
        private bool _isLoadingReport = false;
        private DateTime _lastFromDate;
        private DateTime _lastToDate;
        private RevenueReportDTO _lastRevenueReport;
        private List<RevenueDataPoint> _lastRevenuePoints;
        private List<CustomerReturnData> _lastCustomerReturnData;
        private Dictionary<int, decimal> _lastCustomerTotals;
        private PackagingRecoveryReportDTO _lastPackagingReport;
        private List<PackagingRecoveryData> _lastPackagingData;
        private List<EnvironmentalImpactDataPoint> _lastPlasticDataPoints;
        private List<PaymentMethodStat> _lastPaymentStats;
        private List<Order> _lastPaymentOrders;
        private IList<ProductSales> _lastBestSellingProducts;

        public enum ReportType
        {
            REVENUE,
            CUSTOMER_REFILL,
            PACKAGING_RECOVERY,
            PLASTIC_REDUCTION,
            PAYMENT_METHOD,
            BEST_SELLING
        }

        public ReportControl()
        {
            _reportService = AppServices.ReportService;
            _orderService = AppServices.OrderService;
            _orderDetailService = AppServices.OrderDetailService;
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            UpdateStyles();
            EnableDoubleBuffering(this);
            InitializeLoadingOverlay();
            SetDefaultDates();
        }

        public void RefreshData()
        {
            _ = LoadReport();
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
                ShowLoading(true);
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
            }
            finally
            {
                _isLoadingReport = false;
                ShowLoading(false);
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

            if (_placeholderLabel == null)
            {
                _placeholderLabel = new Label
                {
                    Font = new Font("Segoe UI", 14, FontStyle.Regular),
                    ForeColor = Color.FromArgb(85, 85, 85),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    AutoSize = false
                };
                panelReportContent.Controls.Add(_placeholderLabel);
            }
            _placeholderLabel.Text = message;
            _placeholderLabel.Visible = true;
        }

        protected void RemovePlaceholder()
        {
            if (_placeholderLabel != null)
            {
                _placeholderLabel.Visible = false;
            }
        }

        protected void ClearReportContent()
        {
            panelReportContent.SuspendLayout();
            flowPanelKPICards.SuspendLayout();
            panelChart.SuspendLayout();
            dataGridViewReport.DataSource = null;
            dataGridViewReport.Columns.Clear();
            panelChart.Controls.Clear();

            panelChart.ResumeLayout(true);
            flowPanelKPICards.ResumeLayout(true);
            panelReportContent.ResumeLayout(true);
        }



        private void UpdatePrebuiltKpisCards((string Title, string Value, Color Color)[] kpis)
        {
            var cards = flowPanelKPICards.Controls.OfType<CardControl>().ToList();
            int count = Math.Min(kpis.Length, cards.Count);
            for (int i = 0; i < count; i++)
            {
                var meta = kpis[i];
                var card = cards[i];
                card.Title = meta.Title;
                card.Value = meta.Value;
                card.ValueColor = meta.Color;
                card.Visible = true;
            }
            for (int i = count; i < cards.Count; i++)
            {
                cards[i].Visible = false;
            }
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

            // Update current report type
            _currentReportType = GetReportTypeFromButton(clickedButton);

            // Update UI labels based on report type
            UpdateReportTitle();

            RemovePlaceholder();
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
                        var reportTitle = GetReportTitleForExport();
                        var charts = CaptureCurrentCharts();
                        var pdfExporter = new PdfExporter();
                        if (charts != null && charts.Count > 0)
                        {
                            pdfExporter.ExportToPdf(dataTable, saveDialog.FileName, reportTitle, null, charts);
                        }
                        else
                        {
                            pdfExporter.ExportToPdf(dataTable, saveDialog.FileName, reportTitle);
                        }

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
                        var reportTitle = GetReportTitleForExport();
                        var charts = CaptureCurrentCharts();
                        var excelExporter = new ExcelExporter();
                        if (charts != null && charts.Count > 0)
                        {
                            excelExporter.ExportToExcel(dataTable, saveDialog.FileName, GetReportTypeName(_currentReportType), null, reportTitle, charts);
                        }
                        else
                        {
                            excelExporter.ExportToExcel(dataTable, saveDialog.FileName, GetReportTypeName(_currentReportType), null, reportTitle);
                        }

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

        private IList<byte[]> CaptureCurrentCharts()
        {
            var list = new List<byte[]>();
            foreach (Control ctrl in panelChart.Controls)
            {
                if (ctrl is Chart chart)
                {
                    using (var ms = new MemoryStream())
                    {
                        chart.SaveImage(ms, ChartImageFormat.Png);
                        list.Add(ms.ToArray());
                    }
                }
            }
            return list;
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

            // Mặc định hiển thị placeholder và chờ người dùng tạo báo cáo
            ClearReportContent();
            ShowPlaceholderMessage("Vui lòng chọn khoảng thời gian và nhấn 'Tạo báo cáo' để xem dữ liệu.");

            panelHeader.ShadowDecoration.Enabled = false;
            panelFilters.ShadowDecoration.Enabled = false;
            panelReportContent.ShadowDecoration.Enabled = false;
            BeginInvoke(new Action(() =>
            {
                panelHeader.ShadowDecoration.Enabled = true;
                panelFilters.ShadowDecoration.Enabled = true;
                panelReportContent.ShadowDecoration.Enabled = true;
            }));
        }

        private void EnableDoubleBuffering(Control control)
        {
            if (control == null) return;
            var prop = typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            prop?.SetValue(control, true, null);
            foreach (Control child in control.Controls)
            {
                EnableDoubleBuffering(child);
            }
        }

        private Panel _loadingOverlay;
        private Label _loadingLabel;
        private Label _placeholderLabel;

        private void InitializeLoadingOverlay()
        {
            _loadingOverlay = new Panel();
            _loadingOverlay.Dock = DockStyle.Fill;
            _loadingOverlay.BackColor = Color.FromArgb(60, 255, 255, 255);
            _loadingOverlay.Visible = false;

            _loadingLabel = new Label();
            _loadingLabel.Text = "Đang tải báo cáo...";
            _loadingLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            _loadingLabel.ForeColor = Color.FromArgb(31, 107, 59);
            _loadingLabel.AutoSize = true;
            _loadingLabel.Location = new Point((panelReportContent.Width - _loadingLabel.Width) / 2, (panelReportContent.Height - _loadingLabel.Height) / 2);
            _loadingLabel.Anchor = AnchorStyles.None;

            _loadingOverlay.Controls.Add(_loadingLabel);
            panelReportContent.Controls.Add(_loadingOverlay);
            panelReportContent.ControlAdded += (s, e2) => { EnableDoubleBuffering(_loadingOverlay); };
            panelReportContent.Resize += (s, e3) =>
            {
                _loadingLabel.Left = (panelReportContent.Width - _loadingLabel.Width) / 2;
                _loadingLabel.Top = (panelReportContent.Height - _loadingLabel.Height) / 2;
            };
        }

        private void ShowLoading(bool show)
        {
            if (_loadingOverlay == null) return;
            _loadingOverlay.Visible = show;
            Cursor = show ? Cursors.WaitCursor : Cursors.Default;
        }

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
                _lastFromDate = fromDate;
                _lastToDate = toDate;
                _lastRevenueReport = report;
                _lastRevenuePoints = report.DataPoints.ToList();
                RemovePlaceholder();
                ClearReportContent();
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
            var kpis = new (string Title, string Value, Color Color)[]
            {
                ("Tổng doanh thu", FormatCurrency(report.TotalRevenue), Color.FromArgb(41, 128, 185)),
                ("Tổng số đơn", report.TotalOrders.ToString("N0"), Color.FromArgb(52, 152, 219)),
                ("Giá trị TB / đơn", FormatCurrency(report.AverageOrderValue), Color.FromArgb(155, 89, 182)),
                ("Doanh thu ngày", FormatCurrency(report.DailyRevenue), Color.FromArgb(230, 126, 34)),
                ("Doanh thu tuần", FormatCurrency(report.WeeklyRevenue), Color.FromArgb(26, 188, 156)),
                ("Doanh thu tháng", FormatCurrency(report.MonthlyRevenue), Color.FromArgb(241, 196, 15))
            };
            UpdatePrebuiltKpisCards(kpis);
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
            if (!orderedPoints.Any() || orderedPoints.Max(p => p.Revenue) <= 0)
            {
                panelChart.Controls.Add(new Label
                {
                    Text = "Không có doanh thu để hiển thị biểu đồ",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 10, FontStyle.Italic)
                });
                panelChart.ResumeLayout(true);
                return;
            }
            var title = new Label
            {
                Text = "DOANH THU THEO NGÀY",
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 107, 59)
            };
            var chart = new Chart { Dock = DockStyle.Fill, BackColor = Color.White };
            var area = new ChartArea("main");
            area.AxisX.Interval = 1;
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.LabelStyle.Format = "#,##0";
            area.AxisY.MajorGrid.LineColor = Color.Gainsboro;
            chart.ChartAreas.Add(area);
            var series = new Series("Doanh thu")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                Color = Color.FromArgb(31, 107, 59)
            };
            foreach (var p in orderedPoints)
            {
                series.Points.AddXY(p.Period, (double)p.Revenue);
            }
            chart.Series.Add(series);
            panelChart.Controls.Add(chart);
            panelChart.Controls.Add(title);
            panelChart.ResumeLayout(true);
        }

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
                _lastFromDate = fromDate;
                _lastToDate = toDate;
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
                    _lastPaymentOrders = orders;
                    customerTotals = orders
                        .Where(o => o.CustomerId.HasValue)
                        .GroupBy(o => o.CustomerId.Value)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Sum(o => Math.Max(0, o.TotalAmount - o.DiscountedAmount)));
                }
                _lastCustomerReturnData = customerData.ToList();
                _lastCustomerTotals = customerTotals;
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
            flowPanelKPICards.SuspendLayout();
            int returningCustomers = customerData.Count(c => c.ReturnCount >= 2);
            int firstTimer = customerData.Count(c => c.ReturnCount == 1);
            int loyalCustomers = customerData.Count(c => c.ReturnCount >= 5);
            decimal totalRevenue = customerTotals.Values.Sum();
            var bestCustomer = customerData
                .OrderByDescending(c => c.ReturnCount)
                .ThenByDescending(c => c.TotalOrders)
                .FirstOrDefault();
            var cards = new (string Title, string Value, Color Color)[]
            {
                ("KH quay lại ≥ 2 lần", returningCustomers.ToString("N0"), Color.FromArgb(41, 128, 185)),
                ("KH refill lần đầu", firstTimer.ToString("N0"), Color.FromArgb(39, 174, 96)),
                ("KH trung thành (≥5)", loyalCustomers.ToString("N0"), Color.FromArgb(243, 156, 18)),
                ("KH refill nhiều nhất", bestCustomer?.CustomerName ?? "N/A", Color.FromArgb(155, 89, 182)),
                ("Số lần refill cao nhất", (bestCustomer?.ReturnCount ?? 0).ToString("N0"), Color.FromArgb(231, 76, 60)),
                ("Tổng doanh thu", FormatCurrency(totalRevenue), Color.FromArgb(52, 152, 219))
            };
            UpdatePrebuiltKpisCards(cards);
            flowPanelKPICards.ResumeLayout(true);
        }

        private void CreateCustomerRefillDataTable(List<CustomerReturnData> customerData, Dictionary<int, decimal> customerTotals)
        {
            dataGridViewReport.SuspendLayout();
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
            dataGridViewReport.ResumeLayout(true);
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
            panelChart.SuspendLayout();
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
                panelChart.ResumeLayout(true);
                return;
            }
            var title = new Label
            {
                Text = "PHÂN BỐ SỐ LẦN REFILL",
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 107, 59)
            };
            var chart = new Chart { Dock = DockStyle.Fill, BackColor = Color.White };
            var area = new ChartArea("main");
            area.AxisX.Interval = 1;
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.LineColor = Color.Gainsboro;
            chart.ChartAreas.Add(area);
            var series = new Series("Khách hàng") { ChartType = SeriesChartType.Column, Color = Color.FromArgb(33, 150, 243) };
            foreach (var g in grouped)
            {
                series.Points.AddXY($"{g.ReturnCount} lần", g.CustomerCount);
            }
            chart.Series.Add(series);
            panelChart.Controls.Add(chart);
            panelChart.Controls.Add(title);
            panelChart.ResumeLayout(true);
        }

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
                _lastFromDate = fromDate;
                _lastToDate = toDate;
                _lastPackagingReport = reportData;
                _lastPackagingData = details.ToList();
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
            flowPanelKPICards.SuspendLayout();
            var best = report.PackagingData.OrderByDescending(p => p.RecoveryRate).FirstOrDefault();
            var worst = report.PackagingData.OrderBy(p => p.RecoveryRate).FirstOrDefault();
            var kpiCards = new (string Title, string Value, Color Color)[]
            {
                ("Bao bì phát hành", report.TotalIssued.ToString("N0"), Color.FromArgb(33, 150, 243)),
                ("Bao bì thu hồi", report.TotalReturned.ToString("N0"), Color.FromArgb(76, 175, 80)),
                ("Tỷ lệ thu hồi", $"{report.RecoveryRate:0.00}%", Color.FromArgb(255, 152, 0)),
                ("Hiệu suất cao nhất", best != null ? $"{best.PackagingName} ({best.RecoveryRate:0.00}%)" : "N/A", Color.FromArgb(156, 39, 176)),
                ("Hiệu suất thấp nhất", worst != null ? $"{worst.PackagingName} ({worst.RecoveryRate:0.00}%)" : "N/A", Color.FromArgb(244, 67, 54))
            };
            UpdatePrebuiltKpisCards(kpiCards);
            flowPanelKPICards.ResumeLayout(true);
        }

        private void BuildPackagingRecoveryTable(List<PackagingRecoveryData> data)
        {
            dataGridViewReport.SuspendLayout();
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
            dataGridViewReport.ResumeLayout(true);
        }

        private void BuildPackagingRecoveryChart(List<PackagingRecoveryData> data)
        {
            panelChart.SuspendLayout();
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
                panelChart.ResumeLayout(true);
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
            var chart = new Chart { Dock = DockStyle.Fill, BackColor = Color.White };
            var area = new ChartArea("main");
            area.AxisX.Interval = 1;
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.Maximum = 100;
            area.AxisY.LabelStyle.Format = "0%";
            area.AxisY.MajorGrid.LineColor = Color.Gainsboro;
            chart.ChartAreas.Add(area);
            var series = new Series("Tỷ lệ thu hồi") { ChartType = SeriesChartType.Bar, Color = Color.FromArgb(76, 175, 80) };
            foreach (var item in topItems)
            {
                series.Points.AddXY(item.PackagingName, Math.Min(Math.Max(item.RecoveryRate, 0), 100));
            }
            chart.Series.Add(series);
            panelChart.Controls.Add(chart);
            panelChart.Controls.Add(title);
            panelChart.ResumeLayout(true);
        }

        private int ClampPercent(double value) =>
            (int)Math.Max(0, Math.Min(100, Math.Round(value, MidpointRounding.AwayFromZero)));

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
                _lastFromDate = fromDate;
                _lastToDate = toDate;
                _lastPlasticDataPoints = points.ToList();
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
            flowPanelKPICards.SuspendLayout();
            var avgPlastic = report.TotalRefills > 0
                ? report.PlasticSavedKg / report.TotalRefills
                : 0;
            var avgCO2 = report.TotalRefills > 0
                ? report.CO2SavedKg / report.TotalRefills
                : 0;
            var cards = new (string Title, string Value, Color Color)[]
            {
                ("Lần refill", report.TotalRefills.ToString("N0"), Color.FromArgb(46, 204, 113)),
                ("Nhựa tiết kiệm (kg)", $"{report.PlasticSavedKg:N2} kg", Color.FromArgb(56, 142, 60)),
                ("Nhựa tiết kiệm (tấn)", $"{report.PlasticSavedTons:N3} tấn", Color.FromArgb(0, 150, 136)),
                ("CO₂ giảm thải (kg)", $"{report.CO2SavedKg:N2} kg", Color.FromArgb(30, 136, 229)),
                ("Nhựa/refill", $"{avgPlastic:N3} kg", Color.FromArgb(255, 152, 0)),
                ("CO₂/refill", $"{avgCO2:N3} kg", Color.FromArgb(171, 71, 188))
            };
            UpdatePrebuiltKpisCards(cards);
            flowPanelKPICards.ResumeLayout(true);
        }

        private void BuildPlasticReductionTable(IEnumerable<EnvironmentalImpactDataPoint> dataPoints)
        {
            dataGridViewReport.SuspendLayout();
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
            dataGridViewReport.ResumeLayout(true);
        }

        private void BuildPlasticReductionChart(IEnumerable<EnvironmentalImpactDataPoint> dataPoints)
        {
            panelChart.SuspendLayout();
            panelChart.Controls.Clear();
            panelChart.Padding = new Padding(20);
            var orderedPoints = dataPoints.OrderBy(p => p.Date).ToList();
            if (orderedPoints.Count > 10)
            {
                orderedPoints = orderedPoints.Skip(Math.Max(0, orderedPoints.Count - 10)).ToList();
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
                panelChart.ResumeLayout(true);
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
            var chart = new Chart { Dock = DockStyle.Fill, BackColor = Color.White };
            var area = new ChartArea("main");
            area.AxisX.Interval = 1;
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.LineColor = Color.Gainsboro;
            chart.ChartAreas.Add(area);
            var series = new Series("Nhựa tiết kiệm") { ChartType = SeriesChartType.Line, BorderWidth = 3, Color = Color.FromArgb(56, 142, 60) };
            foreach (var p in orderedPoints)
            {
                series.Points.AddXY(p.Date.ToString("dd/MM"), p.PlasticSavedKg);
            }
            chart.Series.Add(series);
            panelChart.Controls.Add(chart);
            panelChart.Controls.Add(title);
            panelChart.ResumeLayout(true);
        }

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
                _lastFromDate = fromDate;
                _lastToDate = toDate;
                _lastPaymentOrders = orders;
                var stats = BuildPaymentMethodStats(orders);
                _lastPaymentStats = stats;
                RemovePlaceholder();
                ClearReportContent();
                flowPanelKPICards.Visible = true;
                dataGridViewReport.Visible = true;
                panelChart.Visible = true;
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
            var totalOrders = orders.Count;
            var totalRevenue = stats.Sum(s => s.Revenue);
            var paidOrders = orders.Count(o => o.PaymentStatus == PaymentStatus.PAID);
            var topMethod = stats.FirstOrDefault();
            var cashShare = stats.FirstOrDefault(s => s.Method == PaymentMethod.CASH)?.OrderCount ?? 0;
            double cashRatio = totalOrders > 0 ? (double)cashShare / totalOrders * 100 : 0;
            var kpis = new (string Title, string Value, Color Color)[]
            {
                ("Tổng đơn hoàn tất", totalOrders.ToString("N0"), Color.FromArgb(33, 150, 243)),
                ("Doanh thu thuần", FormatCurrency(totalRevenue), Color.FromArgb(46, 204, 113)),
                ("Đơn đã thanh toán", $"{paidOrders:N0} ({(totalOrders > 0 ? (double)paidOrders / totalOrders * 100 : 0):0.0}%)", Color.FromArgb(0, 150, 136)),
                ("Top phương thức", topMethod != null ? $"{GetPaymentMethodDisplayName(topMethod.Method)} ({topMethod.OrderCount:N0} đơn)" : "N/A", Color.FromArgb(255, 152, 0)),
                ("Tỉ trọng tiền mặt", $"{cashRatio:0.0}%", Color.FromArgb(156, 39, 176))
            };
            UpdatePrebuiltKpisCards(kpis);
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
                panelChart.ResumeLayout(true);
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
            var chart = new Chart { Dock = DockStyle.Fill, BackColor = Color.White };
            var area = new ChartArea("main");
            area.Area3DStyle.Enable3D = true;
            chart.ChartAreas.Add(area);
            var series = new Series("Doanh thu") { ChartType = SeriesChartType.Pie };
            foreach (var s in stats)
            {
                var p = series.Points.Add((double)s.Revenue);
                p.AxisLabel = GetPaymentMethodDisplayName(s.Method);
                p.Label = $"{GetPaymentMethodDisplayName(s.Method)}";
            }
            chart.Series.Add(series);
            panelChart.Controls.Add(chart);
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
                _lastFromDate = fromDate;
                _lastToDate = toDate;
                _lastBestSellingProducts = products;
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
            var totalQuantity = products.Sum(p => p.TotalQuantity);
            var totalRevenue = products.Sum(p => p.TotalRevenue);
            var topProduct = products.First();
            var kpis = new (string Title, string Value, Color Color)[]
            {
                ("Sản phẩm dẫn đầu", topProduct.ProductName, Color.FromArgb(255, 152, 0)),
                ("SL bán ra (top 10)", totalQuantity.ToString("N0"), Color.FromArgb(33, 150, 243)),
                ("Doanh thu top 10", FormatCurrency(totalRevenue), Color.FromArgb(46, 204, 113)),
                ("Doanh thu/SP dẫn đầu", FormatCurrency(topProduct.TotalRevenue), Color.FromArgb(156, 39, 176)),
                ("SL bình quân/SP", (products.Average(p => p.TotalQuantity)).ToString("N1"), Color.FromArgb(0, 150, 136))
            };
            UpdatePrebuiltKpisCards(kpis);
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
            if (!topForChart.Any())
            {
                panelChart.Controls.Add(new Label
                {
                    Text = "Không có số lượng bán ra để hiển thị biểu đồ",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 10, FontStyle.Italic)
                });
                panelChart.ResumeLayout(true);
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
            var chart = new Chart { Dock = DockStyle.Fill, BackColor = Color.White };
            var area = new ChartArea("main");
            area.AxisX.Interval = 1;
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.LineColor = Color.Gainsboro;
            chart.ChartAreas.Add(area);
            var series = new Series("Số lượng") { ChartType = SeriesChartType.Column, Color = Color.FromArgb(33, 150, 243) };
            foreach (var p in topForChart)
            {
                series.Points.AddXY(p.ProductName, p.TotalQuantity);
            }
            chart.Series.Add(series);
            panelChart.Controls.Add(chart);
            panelChart.Controls.Add(title);
            panelChart.ResumeLayout(true);
        }
    }
}
