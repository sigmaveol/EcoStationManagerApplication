using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class ReportsControl : UserControl
    {
        private Timer refreshTimer;
        private DateTime lastRefreshTime;

        public ReportsControl()
        {
            InitializeComponent();
            InitializeControls();
        }

        private void InitializeControls()
        {
            // Initialize refresh timer
            refreshTimer = new Timer();
            refreshTimer.Interval = 30000; // 30 seconds
            refreshTimer.Tick += RefreshTimer_Tick;
            refreshTimer.Start();

            lastRefreshTime = DateTime.Now;
            UpdateLastRefreshTime();

            // Set up report cards
            SetupReportCards();
        }

        private void SetupReportCards()
        {
            // Report cards data
            var reportCards = new[]
            {
                new {
                    Title = "Báo cáo doanh thu",
                    Description = "Doanh thu theo ngày/tuần/tháng, chi tiết theo trạm",
                    Icon = "💰",
                    Color = Color.FromArgb(34, 139, 34),
                    HasData = true
                },
                new {
                    Title = "Báo cáo tồn kho",
                    Description = "Tồn kho theo trạm/sản phẩm, cảnh báo mức tồn",
                    Icon = "📦",
                    Color = Color.FromArgb(30, 107, 59),
                    HasData = true
                },
                new {
                    Title = "Báo cáo hao hụt",
                    Description = "Hao hụt & hư hỏng theo sản phẩm và trạm",
                    Icon = "📉",
                    Color = Color.FromArgb(220, 53, 69),
                    HasData = true
                },
                new {
                    Title = "Lịch vệ sinh",
                    Description = "Lịch vệ sinh bồn chứa & bao bì theo trạm",
                    Icon = "🧹",
                    Color = Color.FromArgb(13, 110, 253),
                    HasData = false
                },
                new {
                    Title = "Quản lý bao bì",
                    Description = "Báo cáo phát hành & thu hồi bao bì",
                    Icon = "📄",
                    Color = Color.FromArgb(111, 66, 193),
                    HasData = false
                },
                new {
                    Title = "Báo cáo hiệu suất",
                    Description = "Hiệu suất nhân viên & hoạt động trạm",
                    Icon = "📊",
                    Color = Color.FromArgb(253, 126, 20),
                    HasData = false
                }
            };

            // Clear existing controls
            flowLayoutPanelReports.Controls.Clear();

            // Create report cards
            foreach (var card in reportCards)
            {
                var reportCard = CreateReportCard(card.Title, card.Description, card.Icon, card.Color, card.HasData);
                flowLayoutPanelReports.Controls.Add(reportCard);
            }
        }

        private Guna2Panel CreateReportCard(string title, string description, string icon, Color color, bool hasData)
        {
            var panel = new Guna2Panel
            {
                Size = new Size(350, 120),
                BorderRadius = 15,
                BorderColor = Color.LightGray,
                BorderThickness = 1,
                FillColor = Color.White,
                Margin = new Padding(10),
                Cursor = Cursors.Hand
            };

            // Icon label
            var lblIcon = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 20),
                Location = new Point(20, 20),
                Size = new Size(40, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Title label
            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(70, 20),
                Size = new Size(250, 25),
                ForeColor = color
            };

            // Description label
            var lblDescription = new Label
            {
                Text = description,
                Font = new Font("Segoe UI", 9),
                Location = new Point(70, 45),
                Size = new Size(250, 40),
                ForeColor = Color.Gray
            };

            // Status label
            var lblStatus = new Label
            {
                Text = hasData ? "🟢 Có dữ liệu" : "🟡 Đang phát triển",
                Font = new Font("Segoe UI", 8),
                Location = new Point(70, 85),
                Size = new Size(120, 20),
                ForeColor = hasData ? Color.Green : Color.Orange
            };

            // View button
            var btnView = new Guna2Button
            {
                Text = "Xem báo cáo",
                Size = new Size(100, 30),
                Location = new Point(220, 80),
                FillColor = color,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                BorderRadius = 10,
                Animated = true
            };

            btnView.Click += (sender, e) =>
            {
                if (hasData)
                {
                    ShowReportPreview(title);
                }
                else
                {
                    MessageBox.Show($"Báo cáo '{title}' đang được phát triển và sẽ sớm có sẵn.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            // Add hover effects
            panel.MouseEnter += (sender, e) =>
            {
                panel.ShadowDecoration.Enabled = true;
                panel.ShadowDecoration.Color = Color.FromArgb(150, 150, 150);
                panel.ShadowDecoration.BorderRadius = 15;
                panel.ShadowDecoration.Depth = 10;
                panel.FillColor = Color.FromArgb(250, 250, 250);
            };

            panel.MouseLeave += (sender, e) =>
            {
                panel.ShadowDecoration.Enabled = false;
                panel.FillColor = Color.White;
            };

            // Add controls to panel
            panel.Controls.AddRange(new Control[] { lblIcon, lblTitle, lblDescription, lblStatus, btnView });

            return panel;
        }

        private void ShowReportPreview(string reportTitle)
        {
            // In a real application, this would open the actual report
            var previewForm = new Form
            {
                Text = $"Xem trước: {reportTitle}",
                Size = new Size(800, 600),
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = Color.White
            };

            var contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                AutoScroll = true
            };

            var titleLabel = new Label
            {
                Text = reportTitle,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 20),
                Size = new Size(400, 30),
                ForeColor = Color.FromArgb(30, 107, 59)
            };

            var descriptionLabel = new Label
            {
                Text = GetReportDescription(reportTitle),
                Font = new Font("Segoe UI", 10),
                Location = new Point(20, 60),
                Size = new Size(600, 60),
                ForeColor = Color.Gray
            };

            var chartPanel = new Panel
            {
                Location = new Point(20, 140),
                Size = new Size(700, 300),
                BackColor = Color.FromArgb(245, 245, 245),
                BorderStyle = BorderStyle.FixedSingle
            };

            var chartLabel = new Label
            {
                Text = "📊 Biểu đồ báo cáo sẽ hiển thị tại đây",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(250, 130),
                Size = new Size(300, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Gray
            };

            var closeButton = new Guna2Button
            {
                Text = "Đóng",
                Size = new Size(100, 35),
                Location = new Point(620, 470),
                FillColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                Animated = true
            };
            closeButton.Click += (s, e) => previewForm.Close();

            var exportButton = new Guna2Button
            {
                Text = "Xuất Excel",
                Size = new Size(100, 35),
                Location = new Point(500, 470),
                FillColor = Color.FromArgb(30, 107, 59),
                ForeColor = Color.White,
                Animated = true
            };
            exportButton.Click += (s, e) => MessageBox.Show("Đã xuất báo cáo ra Excel!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            chartPanel.Controls.Add(chartLabel);
            contentPanel.Controls.AddRange(new Control[] { titleLabel, descriptionLabel, chartPanel, closeButton, exportButton });
            previewForm.Controls.Add(contentPanel);

            previewForm.ShowDialog();
        }

        private string GetReportDescription(string reportTitle)
        {
            switch (reportTitle)
            {
                case "Báo cáo doanh thu":
                    return "Phân tích doanh thu chi tiết theo thời gian thực, so sánh với cùng kỳ và dự báo xu hướng.";
                case "Báo cáo tồn kho":
                    return "Theo dõi mức tồn kho thực tế, dự báo nhu cầu và cảnh báo mức tồn kho tối ưu.";
                case "Báo cáo hao hụt":
                    return "Phân tích nguyên nhân hao hụt, tỷ lệ hư hỏng và đề xuất biện pháp giảm thiểu.";
                case "Lịch vệ sinh":
                    return "Lịch trình vệ sinh định kỳ, lịch sử bảo trì và cảnh báo lịch sắp tới.";
                case "Quản lý bao bì":
                    return "Theo dõi vòng đời bao bì từ phát hành đến thu hồi và tái sử dụng.";
                case "Báo cáo hiệu suất":
                    return "Đánh giá hiệu suất làm việc của nhân viên và hiệu quả hoạt động các trạm.";
                default:
                    return "Báo cáo chi tiết với đầy đủ thông tin phân tích và thống kê.";
            }
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            lastRefreshTime = DateTime.Now;
            UpdateLastRefreshTime();
        }

        private void UpdateLastRefreshTime()
        {
            lblLastUpdate.Text = $"Cập nhật lần cuối: {lastRefreshTime:HH:mm:ss}";
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Simulate data refresh
            lastRefreshTime = DateTime.Now;
            UpdateLastRefreshTime();

            // Show refresh animation
            btnRefresh.Text = "🔄 Đang làm mới...";
            btnRefresh.Enabled = false;

            var timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += (s, g) =>
            {
                btnRefresh.Text = "🔄 Làm mới";
                btnRefresh.Enabled = true;
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();

            MessageBox.Show("Đã làm mới dữ liệu báo cáo!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        refreshTimer?.Stop();
        //        refreshTimer?.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}