using System;
using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class ReportsControl : UserControl
    {
        public ReportsControl()
        {
            InitializeComponent();

            PopulateReportStats();
            PopulateEcoImpactStats();

            InitializeEvents();

            InitializeDates();
        }

        // Gán tất cả sự kiện ở đây
        private void InitializeEvents()
        {
            if (btnExportReportPDF != null)
                btnExportReportPDF.Click += btnExportReportPDF_Click;

            if (btnExportReportExcel != null)
                btnExportReportExcel.Click += btnExportReportExcel_Click;

            if (btnGenerateReport != null)
                btnGenerateReport.Click += btnGenerateReport_Click;
        }

        // --- HÀM LOGIC & DỮ LIỆU ---

        private void InitializeDates()
        {
            dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpTo.Value = DateTime.Now;
        }

        // Đổ dữ liệu cho các thẻ thống kê Doanh thu
        private void PopulateReportStats()
        {
            if (statsReportFlowPanel == null) return;

            var statsData = new[]
            {
                new { Label = "Doanh thu ngày", Value = "850K" },
                new { Label = "Doanh thu tuần", Value = "5.2M" },
                new { Label = "Doanh thu tháng", Value = "18.5M" }
            };

            foreach (var stat in statsData)
            {
                var statCard = CreateReportStatCard(stat.Label, stat.Value);
                statCard.Margin = new Padding(10);
                statCard.Size = new Size(250, 70);
                statsReportFlowPanel.Controls.Add(statCard);
            }
        }

        // Đổ dữ liệu cho bảng Tác động môi trường
        private void PopulateEcoImpactStats()
        {
            if (metricsEcoTable == null) return;

            var metrics = new[]
            {
                new { Label = "Tổng số chai tái sử dụng", Value = "1,250" },
                new { Label = "Lượng nhựa tiết kiệm (ước tính)", Value = "312.5 kg" },
                new { Label = "CO2 giảm phát thải", Value = "1.8 tấn" }
            };

            foreach (var metric in metrics)
            {
                var lblLabel = new Label();
                lblLabel.Text = metric.Label;
                lblLabel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                lblLabel.Dock = DockStyle.Fill;
                lblLabel.TextAlign = ContentAlignment.MiddleLeft;

                var lblValue = new Label();
                lblValue.Text = metric.Value;
                lblValue.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                lblValue.ForeColor = Color.FromArgb(46, 125, 50);
                lblValue.Dock = DockStyle.Fill;
                lblValue.TextAlign = ContentAlignment.MiddleRight;

                metricsEcoTable.Controls.Add(lblLabel);
                metricsEcoTable.Controls.Add(lblValue);
            }
        }

        // --- HÀM XỬ LÝ SỰ KIỆN ---

        private void btnExportReportPDF_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Xuất báo cáo PDF", "Xuất PDF");
        }

        private void btnExportReportExcel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Xuất báo cáo Excel", "Xuất Excel");
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            string reportType = cmbReportType.SelectedItem?.ToString() ?? "Doanh thu";
            MessageBox.Show($"Tạo báo cáo {reportType} từ {dtpFrom.Value:dd/MM/yyyy} đến {dtpTo.Value:dd/MM/yyyy}", "Tạo báo cáo");
        }

        // --- HÀM HELPER (Hàm phụ trợ) ---

        private Panel CreateReportStatCard(string label, string value)
        {
            var card = new Panel();
            card.Size = new Size(250, 70);
            card.BackColor = Color.White;
            card.BorderStyle = BorderStyle.FixedSingle; // Giữ viền cho thẻ stat
            card.Padding = new Padding(10);

            var lblLabel = new Label();
            lblLabel.Text = label;
            lblLabel.Font = new Font("Segoe UI", 9);
            lblLabel.ForeColor = Color.Gray;
            lblLabel.AutoSize = true;
            lblLabel.Location = new Point(10, 10);

            var lblValue = new Label();
            lblValue.Text = value;
            lblValue.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblValue.ForeColor = Color.FromArgb(46, 125, 50);
            lblValue.AutoSize = true;
            lblValue.Location = new Point(10, 30);

            card.Controls.Add(lblLabel);
            card.Controls.Add(lblValue);

            return card;
        }
    }
}