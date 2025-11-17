using System;
using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            InitializeComponent();
            InitializeControl();
        }

        private void InitializeControl()
        {
            // Thêm các control vào notificationFlowPanel
            var notificationTitleLabel = new Label
            {
                Text = "🔔 Cài đặt Thông báo",
                Font = new Font("Segoe UI Emoji", 12, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10)
            };
            notificationFlowPanel.Controls.Add(notificationTitleLabel);
            notificationFlowPanel.Controls.Add(chkNewOrderNotification);
            notificationFlowPanel.Controls.Add(chkLowStockAlert);
            notificationFlowPanel.Controls.Add(chkPackagingReturn);
            notificationFlowPanel.Controls.Add(chkDailyReport);

            // Thêm title cho business card
            var businessTitleLabel = new Label
            {
                Text = "🏢 Thông tin Doanh nghiệp",
                Font = new Font("Segoe UI Emoji", 12, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleLeft
            };
            businessTable.Controls.Add(businessTitleLabel, 0, 0);

            // Thêm title cho inventory card
            var inventoryTitleLabel = new Label
            {
                Text = "📦 Cài đặt Kho hàng",
                Font = new Font("Segoe UI Emoji", 12, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleLeft
            };
            inventoryCard.Controls.Add(inventoryTitleLabel);

            // Thêm title cho packaging card
            var packagingTitleLabel = new Label
            {
                Text = "♻️ Cài đặt Bao bì",
                Font = new Font("Segoe UI Emoji", 12, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleLeft
            };
            packagingCard.Controls.Add(packagingTitleLabel);
        }

        private void btnSaveBusinessInfo_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Đã lưu thông tin doanh nghiệp", "Thành công");
        }

        private void btnSaveNotificationSettings_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Đã lưu cài đặt thông báo", "Thành công");
        }

        private void btnSaveInventorySettings_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Đã lưu cài đặt kho hàng", "Thành công");
        }

        private void btnSavePackagingSettings_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Đã lưu cài đặt bao bì", "Thành công");
        }

        private void btnExportSystemLog_Click(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Text files (*.txt)|*.txt";
            saveDialog.FileName = $"EcoStation_SystemLog_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show($"Đã xuất log hệ thống thành công!\nFile: {saveDialog.FileName}", "Xuất log");
            }
        }

        private void btnResetSettings_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn đặt lại tất cả cài đặt về mặc định?",
                "Xác nhận đặt lại",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                MessageBox.Show("Đã đặt lại cài đặt về mặc định", "Thành công");
            }
        }
    }
}