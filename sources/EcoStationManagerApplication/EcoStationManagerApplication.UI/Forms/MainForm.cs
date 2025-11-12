using EcoStationManagerApplication.UI.Controls; // Đảm bảo namespace này CHÍNH XÁC
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class MainForm : Form
    {
        private bool sidebarCollapsed = false;
        private Dictionary<string, UserControl> pages = new Dictionary<string, UserControl>();

        public MainForm()
        {
            InitializeComponent();

            // ### SỬA LỖI: GỌI HÀM TẠO BUTTON Ở ĐÂY ###
            // (Sau khi các panel đã được InitializeComponent tạo ra)
            CreateSidebarButtons();

            this.Load += new System.EventHandler(this.MainForm_Load);
            ShowContent("btnDashboard");
            UpdateActiveButton(btnDashboard);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            lblDate.Left = this.ClientSize.Width - lblDate.Width - 20;
            lblStatus.Left = lblDate.Left - lblStatus.Width - 20;
        }

        // ### SỬA LỖI: HÀM TẠO BUTTON ĐƯỢC DI CHUYỂN RA NGOÀI FILE DESIGNER ###
        private void CreateSidebarButtons()
        {
            int yPos = 20;
            var buttonData = new[]
            {
                new { Name = "btnDashboard", Text = "Dashboard" },
                new { Name = "btnDataImport", Text = "Nhập dữ liệu" },
                new { Name = "btnOrders", Text = "Đơn hàng" },
                new { Name = "btnInventory", Text = "Kho & Trạm Refill" },
                new { Name = "btnPackaging", Text = "Bao bì & Tuần hoàn" },
                new { Name = "btnStaff", Text = "Nhân sự & Giao vận" },
                new { Name = "btnReports", Text = "Báo cáo & Phân tích" },
                new { Name = "btnIntegrations", Text = "Tích hợp Online" },
                new { Name = "btnBackup", Text = "Sao lưu & Phục hồi" },
                new { Name = "btnSettings", Text = "Cài đặt" }
            };

            foreach (var data in buttonData)
            {
                var button = new Button();
                button.Name = data.Name;
                button.Text = data.Text;
                button.Size = new Size(260, 45);
                button.Location = new Point(0, yPos);
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;
                button.BackColor = Color.Transparent;
                button.ForeColor = Color.White;
                button.Font = new Font("Segoe UI", 10);
                button.TextAlign = ContentAlignment.MiddleLeft;
                button.Padding = new Padding(20, 0, 0, 0);
                button.Tag = data.Text;
                button.Click += sidebarButton_Click;

                sidebarPanel.Controls.Add(button);
                yPos += 45;

                // Set tham chiếu đến các biến private
                switch (data.Name)
                {
                    case "btnDashboard": btnDashboard = button; break;
                    case "btnDataImport": btnDataImport = button; break;
                    case "btnOrders": btnOrders = button; break;
                    case "btnInventory": btnInventory = button; break;
                    case "btnPackaging": btnPackaging = button; break;
                    case "btnStaff": btnStaff = button; break;
                    case "btnReports": btnReports = button; break;
                    case "btnIntegrations": btnIntegrations = button; break;
                    case "btnBackup": btnBackup = button; break;
                    case "btnSettings": btnSettings = button; break;
                }
            }
        }

        private void ToggleSidebar()
        {
            sidebarCollapsed = !sidebarCollapsed;

            if (sidebarCollapsed)
            {
                sidebarPanel.Width = 70;
                foreach (Control control in sidebarPanel.Controls)
                {
                    if (control is Button btn && btn.Tag != null)
                    {
                        btn.Text = "";
                        btn.Width = 70;
                        btn.Padding = new Padding(0); // Căn icon ra giữa
                    }
                }
            }
            else
            {
                sidebarPanel.Width = 260;
                RestoreSidebarButtons();
            }
        }

        private void RestoreSidebarButtons()
        {
            var buttons = new[]
            {
                new { Button = btnDashboard, Text = "Dashboard" },
                new { Button = btnDataImport, Text = "Nhập dữ liệu" },
                new { Button = btnOrders, Text = "Đơn hàng" },
                new { Button = btnInventory, Text = "Kho & Trạm Refill" },
                new { Button = btnPackaging, Text = "Bao bì & Tuần hoàn" },
                new { Button = btnStaff, Text = "Nhân sự & Giao vận" },
                new { Button = btnReports, Text = "Báo cáo & Phân tích" },
                new { Button = btnIntegrations, Text = "Tích hợp Online" },
                new { Button = btnBackup, Text = "Sao lưu & Phục hồi" },
                new { Button = btnSettings, Text = "Cài đặt" }
            };

            foreach (var item in buttons)
            {
                if (item.Button != null)
                {
                    item.Button.Text = item.Text;
                    item.Button.Width = 260;
                    item.Button.Padding = new Padding(20, 0, 0, 0);
                }
            }
        }

        private void ShowContent(string pageName)
        {
            UserControl pageToShow = null;

            if (pages.ContainsKey(pageName))
            {
                pageToShow = pages[pageName];
            }
            else
            {
                try
                {
                    switch (pageName)
                    {
                        case "btnDashboard":
                            var dashboard = new DashboardControl();
                            dashboard.ViewAllOrdersClicked += Dashboard_ViewAllOrdersClicked;
                            pageToShow = dashboard; break;
                        case "btnDataImport":
                            pageToShow = new DataImportControl();
                            break;
                        case "btnOrders":
                            pageToShow = new OrdersControl();
                            break;
                        case "btnInventory":
                            pageToShow = new InventoryControl();
                            break;
                        case "btnPackaging":
                            pageToShow = new PackagingControl();
                            break;
                        case "btnStaff":
                            pageToShow = new StaffControl();
                            break;
                        case "btnReports":
                            pageToShow = new ReportsControl();
                            break;
                        case "btnIntegrations":
                            pageToShow = new IntegrationsControl();
                            break;
                        case "btnBackup":
                            pageToShow = new BackupControl();
                            break;
                        case "btnSettings":
                            pageToShow = new SettingsControl();
                            break;
                        default:
                            MessageBox.Show($"Không tìm thấy control: {pageName}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                    }

                    if (pageToShow != null)
                    {
                        pageToShow.Dock = DockStyle.Fill;
                        pages.Add(pageName, pageToShow);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tạo UserControl: {pageName}\n{ex.Message}", "Lỗi nghiêm trọng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (pageToShow != null)
            {
                mainContentPanel.Controls.Clear();
                mainContentPanel.Controls.Add(pageToShow);
                pageToShow.BringToFront();
            }
        }
        private void Dashboard_ViewAllOrdersClicked(object sender, EventArgs e)
        {
            // 1. Chuyển sang trang Orders
            ShowContent("btnOrders");

            // 2. Cập nhật nút "Đơn hàng" trên sidebar thành active
            UpdateActiveButton(btnOrders);
        }
        private void btnMenu_Click(object sender, EventArgs e)
        {
            ToggleSidebar();
        }

        private void sidebarButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                UpdateActiveButton(button);
                ShowContent(button.Name);
            }
        }

        private void UpdateActiveButton(Button activeButton)
        {
            var buttons = new[]
            {
                btnDashboard, btnDataImport, btnOrders, btnInventory,
                btnPackaging, btnStaff, btnReports, btnIntegrations,
                btnBackup, btnSettings
            };

            foreach (var btn in buttons)
            {
                if (btn != null)
                {
                    btn.BackColor = Color.Transparent;
                    btn.ForeColor = Color.White;
                    btn.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                }
            }

            if (activeButton != null)
            {
                activeButton.BackColor = Color.FromArgb(30, 255, 255, 255);
                activeButton.ForeColor = Color.White;
                activeButton.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            }
        }
    }
}