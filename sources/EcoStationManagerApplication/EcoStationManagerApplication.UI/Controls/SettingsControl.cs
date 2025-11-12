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
            CreateHeaderPanel();
            CreateSettingsSection();
            CreateSystemInfoPanel();
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

        private void CreateHeaderPanel()
        {
            headerPanel = new Panel();
            headerPanel.Size = new Size(900, 60);
            headerPanel.Location = new Point(20, 20);
            headerPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            var titleLabel = new Label();
            titleLabel.Text = "Cài đặt hệ thống";
            titleLabel.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            titleLabel.AutoSize = true;
            titleLabel.Location = new Point(0, 15);
            titleLabel.Dock = DockStyle.Left;

            headerPanel.Controls.Add(titleLabel);
            this.Controls.Add(headerPanel);
        }

        private void CreateSettingsSection()
        {
            settingsSection = new FlowLayoutPanel();
            settingsSection.Size = new Size(900, 600);
            settingsSection.Location = new Point(20, 90);
            settingsSection.FlowDirection = FlowDirection.LeftToRight;
            settingsSection.WrapContents = true;
            settingsSection.AutoScroll = true;
            settingsSection.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            var businessCard = CreateBusinessInfoCard();
            businessCard.Size = new Size(430, 320);
            businessCard.Margin = new Padding(10);

            var notificationCard = CreateNotificationSettingsCard();
            notificationCard.Size = new Size(430, 320);
            notificationCard.Margin = new Padding(10);

            var inventoryCard = CreateInventorySettingsCard();
            inventoryCard.Size = new Size(430, 250);
            inventoryCard.Margin = new Padding(10);

            var packagingCard = CreatePackagingSettingsCard();
            packagingCard.Size = new Size(430, 250);
            packagingCard.Margin = new Padding(10);

            settingsSection.Controls.Add(businessCard);
            settingsSection.Controls.Add(notificationCard);
            settingsSection.Controls.Add(inventoryCard);
            settingsSection.Controls.Add(packagingCard);

            this.Controls.Add(settingsSection);
        }

        private Panel CreateBusinessInfoCard()
        {
            var card = new Panel();
            card.BackColor = Color.White;
            card.BorderStyle = BorderStyle.FixedSingle;
            card.Padding = new Padding(15);

            var titleLabel = new Label();
            titleLabel.Text = "🏢 Thông tin Doanh nghiệp";
            titleLabel.Font = new Font("Segoe UI Emoji", 12, FontStyle.Bold);
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Height = 30;

            var table = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 9 };
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30f));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 25f));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30f));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 25f));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30f));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 25f));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30f));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 25f));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30f));

            table.Controls.Add(titleLabel, 0, 0);

            table.Controls.Add(new Label { Text = "Tên doanh nghiệp", Font = new Font("Segoe UI", 9, FontStyle.Bold), AutoSize = true, Dock = DockStyle.Fill }, 0, 1);
            txtBusinessName = new TextBox { Dock = DockStyle.Fill, Text = "EcoStation Refill" };
            table.Controls.Add(txtBusinessName, 0, 2);

            table.Controls.Add(new Label { Text = "Địa chỉ", Font = new Font("Segoe UI", 9, FontStyle.Bold), AutoSize = true, Dock = DockStyle.Fill }, 0, 3);
            txtAddress = new TextBox { Dock = DockStyle.Fill, Text = "123 Đường ABC, Quận 1, TP.HCM" };
            table.Controls.Add(txtAddress, 0, 4);

            table.Controls.Add(new Label { Text = "Số điện thoại", Font = new Font("Segoe UI", 9, FontStyle.Bold), AutoSize = true, Dock = DockStyle.Fill }, 0, 5);
            txtPhone = new TextBox { Dock = DockStyle.Fill, Text = "0909123456" };
            table.Controls.Add(txtPhone, 0, 6);

            table.Controls.Add(new Label { Text = "Email", Font = new Font("Segoe UI", 9, FontStyle.Bold), AutoSize = true, Dock = DockStyle.Fill }, 0, 7);
            txtEmail = new TextBox { Dock = DockStyle.Fill, Text = "contact@ecostation.vn" };
            table.Controls.Add(txtEmail, 0, 8);

            btnSaveBusinessInfo = new Button();
            btnSaveBusinessInfo.Text = "Lưu thông tin";
            btnSaveBusinessInfo.Size = new Size(120, 35);
            btnSaveBusinessInfo.BackColor = Color.FromArgb(46, 125, 50);
            btnSaveBusinessInfo.ForeColor = Color.White;
            btnSaveBusinessInfo.FlatStyle = FlatStyle.Flat;
            btnSaveBusinessInfo.FlatAppearance.BorderSize = 0;
            btnSaveBusinessInfo.Click += btnSaveBusinessInfo_Click;
            btnSaveBusinessInfo.Dock = DockStyle.Bottom;

            card.Controls.Add(table);
            card.Controls.Add(btnSaveBusinessInfo);

            return card;
        }

        private Panel CreateNotificationSettingsCard()
        {
            var card = new Panel();
            card.BackColor = Color.White;
            card.BorderStyle = BorderStyle.FixedSingle;
            card.Padding = new Padding(15);

            var titleLabel = new Label();
            titleLabel.Text = "🔔 Cài đặt Thông báo";
            titleLabel.Font = new Font("Segoe UI Emoji", 12, FontStyle.Bold);
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Height = 30;

            var flowPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown };
            flowPanel.Controls.Add(titleLabel);

            chkNewOrderNotification = new CheckBox { Text = "Thông báo đơn hàng mới", Checked = true, Font = new Font("Segoe UI", 9), AutoSize = true, Margin = new Padding(0, 10, 0, 0) };
            chkLowStockAlert = new CheckBox { Text = "Cảnh báo tồn kho thấp", Checked = true, Font = new Font("Segoe UI", 9), AutoSize = true };
            chkPackagingReturn = new CheckBox { Text = "Thông báo thu hồi bao bì", Checked = false, Font = new Font("Segoe UI", 9), AutoSize = true };
            chkDailyReport = new CheckBox { Text = "Báo cáo hàng ngày", Checked = true, Font = new Font("Segoe UI", 9), AutoSize = true };

            flowPanel.Controls.Add(chkNewOrderNotification);
            flowPanel.Controls.Add(chkLowStockAlert);
            flowPanel.Controls.Add(chkPackagingReturn);
            flowPanel.Controls.Add(chkDailyReport);

            btnSaveNotificationSettings = new Button();
            btnSaveNotificationSettings.Text = "Lưu cài đặt";
            btnSaveNotificationSettings.Size = new Size(120, 35);
            btnSaveNotificationSettings.BackColor = Color.FromArgb(46, 125, 50);
            btnSaveNotificationSettings.ForeColor = Color.White;
            btnSaveNotificationSettings.FlatStyle = FlatStyle.Flat;
            btnSaveNotificationSettings.FlatAppearance.BorderSize = 0;
            btnSaveNotificationSettings.Click += btnSaveNotificationSettings_Click;
            btnSaveNotificationSettings.Dock = DockStyle.Bottom;

            card.Controls.Add(flowPanel);
            card.Controls.Add(btnSaveNotificationSettings);

            return card;
        }

        private Panel CreateInventorySettingsCard()
        {
            var card = new Panel();
            card.BackColor = Color.White;
            card.BorderStyle = BorderStyle.FixedSingle;
            card.Padding = new Padding(15);

            var titleLabel = new Label();
            titleLabel.Text = "📦 Cài đặt Kho hàng";
            titleLabel.Font = new Font("Segoe UI Emoji", 12, FontStyle.Bold);
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Height = 30;

            var lblStockAlert = new Label { Text = "Mức cảnh báo tồn kho", Font = new Font("Segoe UI", 9, FontStyle.Bold), AutoSize = true, Location = new Point(15, 40) };
            numStockAlertLevel = new NumericUpDown { Size = new Size(100, 25), Location = new Point(15, 65), Minimum = 1, Value = 10 };

            var lblDefaultUnit = new Label { Text = "Đơn vị tính mặc định", Font = new Font("Segoe UI", 9, FontStyle.Bold), AutoSize = true, Location = new Point(130, 40) };
            cmbDefaultUnit = new ComboBox { Size = new Size(150, 25), Location = new Point(130, 65), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbDefaultUnit.Items.AddRange(new object[] { "Lít (L)", "Mililit (ml)", "Kilogram (kg)", "Gram (g)" });
            cmbDefaultUnit.SelectedIndex = 0;

            btnSaveInventorySettings = new Button();
            btnSaveInventorySettings.Text = "Lưu cài đặt";
            btnSaveInventorySettings.Size = new Size(120, 35);
            btnSaveInventorySettings.BackColor = Color.FromArgb(46, 125, 50);
            btnSaveInventorySettings.ForeColor = Color.White;
            btnSaveInventorySettings.FlatStyle = FlatStyle.Flat;
            btnSaveInventorySettings.FlatAppearance.BorderSize = 0;
            btnSaveInventorySettings.Click += btnSaveInventorySettings_Click;
            btnSaveInventorySettings.Location = new Point(295, 200);
            btnSaveInventorySettings.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            card.Controls.Add(titleLabel);
            card.Controls.Add(lblStockAlert);
            card.Controls.Add(numStockAlertLevel);
            card.Controls.Add(lblDefaultUnit);
            card.Controls.Add(cmbDefaultUnit);
            card.Controls.Add(btnSaveInventorySettings);

            return card;
        }

        private Panel CreatePackagingSettingsCard()
        {
            var card = new Panel();
            card.BackColor = Color.White;
            card.BorderStyle = BorderStyle.FixedSingle;
            card.Padding = new Padding(15);

            var titleLabel = new Label();
            titleLabel.Text = "♻️ Cài đặt Bao bì";
            titleLabel.Font = new Font("Segoe UI Emoji", 12, FontStyle.Bold);
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Height = 30;

            var lblMaxReuse = new Label { Text = "Số lần tái sử dụng tối đa", Font = new Font("Segoe UI", 9, FontStyle.Bold), AutoSize = true, Location = new Point(15, 40) };
            numMaxReuse = new NumericUpDown { Size = new Size(100, 25), Location = new Point(15, 65), Minimum = 1, Value = 15 };

            var lblCleaningCycle = new Label { Text = "Chu kỳ vệ sinh (ngày)", Font = new Font("Segoe UI", 9, FontStyle.Bold), AutoSize = true, Location = new Point(130, 40) };
            numCleaningCycle = new NumericUpDown { Size = new Size(100, 25), Location = new Point(130, 65), Minimum = 1, Value = 7 };

            var lblTargetRecovery = new Label { Text = "Tỷ lệ thu hồi mục tiêu", Font = new Font("Segoe UI", 9, FontStyle.Bold), AutoSize = true, Location = new Point(15, 100) };
            numTargetRecoveryRate = new NumericUpDown { Size = new Size(100, 25), Location = new Point(15, 125), Minimum = 1, Maximum = 100, Value = 80 };
            var percentLabel = new Label { Text = "%", Font = new Font("Segoe UI", 9), AutoSize = true, Location = new Point(120, 128) };

            btnSavePackagingSettings = new Button();
            btnSavePackagingSettings.Text = "Lưu cài đặt";
            btnSavePackagingSettings.Size = new Size(120, 35);
            btnSavePackagingSettings.BackColor = Color.FromArgb(46, 125, 50);
            btnSavePackagingSettings.ForeColor = Color.White;
            btnSavePackagingSettings.FlatStyle = FlatStyle.Flat;
            btnSavePackagingSettings.FlatAppearance.BorderSize = 0;
            btnSavePackagingSettings.Click += btnSavePackagingSettings_Click;
            btnSavePackagingSettings.Location = new Point(295, 200);
            btnSavePackagingSettings.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            card.Controls.Add(titleLabel);
            card.Controls.Add(lblMaxReuse);
            card.Controls.Add(numMaxReuse);
            card.Controls.Add(lblCleaningCycle);
            card.Controls.Add(numCleaningCycle);
            card.Controls.Add(lblTargetRecovery);
            card.Controls.Add(numTargetRecoveryRate);
            card.Controls.Add(percentLabel);
            card.Controls.Add(btnSavePackagingSettings);

            return card;
        }

        private void CreateSystemInfoPanel()
        {
            systemInfoPanel = new Panel();
            systemInfoPanel.Size = new Size(900, 200);
            systemInfoPanel.Location = new Point(20, 700);
            systemInfoPanel.BorderStyle = BorderStyle.FixedSingle;
            systemInfoPanel.Padding = new Padding(15);
            systemInfoPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            var titleLabel = new Label();
            titleLabel.Text = "Thông tin Hệ thống";
            titleLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Height = 30;

            var table = new TableLayoutPanel { Dock = DockStyle.Top, Height = 60, ColumnCount = 3, RowCount = 2 };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 25f));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30f));

            table.Controls.Add(new Label { Text = "Phiên bản", Font = new Font("Segoe UI", 9, FontStyle.Bold), AutoSize = true, Dock = DockStyle.Fill }, 0, 0);
            var txtVersion = new TextBox { Dock = DockStyle.Fill, Text = "EcoStation Manager - Ver.0 (Offline)", ReadOnly = true };
            table.Controls.Add(txtVersion, 0, 1);

            table.Controls.Add(new Label { Text = "Ngày cài đặt", Font = new Font("Segoe UI", 9, FontStyle.Bold), AutoSize = true, Dock = DockStyle.Fill }, 1, 0);
            var txtInstallDate = new TextBox { Dock = DockStyle.Fill, Text = "15/03/2025", ReadOnly = true };
            table.Controls.Add(txtInstallDate, 1, 1);

            table.Controls.Add(new Label { Text = "Dung lượng dữ liệu", Font = new Font("Segoe UI", 9, FontStyle.Bold), AutoSize = true, Dock = DockStyle.Fill }, 2, 0);
            var txtDataSize = new TextBox { Dock = DockStyle.Fill, Text = "45.2 MB", ReadOnly = true };
            table.Controls.Add(txtDataSize, 2, 1);

            var buttonPanel = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 40, FlowDirection = FlowDirection.LeftToRight };
            btnExportSystemLog = new Button();
            btnExportSystemLog.Text = "Xuất log hệ thống";
            btnExportSystemLog.Size = new Size(130, 35);
            btnExportSystemLog.BackColor = Color.FromArgb(224, 224, 224);
            btnExportSystemLog.FlatStyle = FlatStyle.Flat;
            btnExportSystemLog.FlatAppearance.BorderSize = 0;
            btnExportSystemLog.Click += btnExportSystemLog_Click;

            btnResetSettings = new Button();
            btnResetSettings.Text = "Đặt lại cài đặt";
            btnResetSettings.Size = new Size(130, 35);
            btnResetSettings.BackColor = Color.FromArgb(255, 152, 0);
            btnResetSettings.ForeColor = Color.White;
            btnResetSettings.FlatStyle = FlatStyle.Flat;
            btnResetSettings.FlatAppearance.BorderSize = 0;
            btnResetSettings.Click += btnResetSettings_Click;

            buttonPanel.Controls.Add(btnExportSystemLog);
            buttonPanel.Controls.Add(btnResetSettings);

            systemInfoPanel.Controls.Add(table);
            systemInfoPanel.Controls.Add(buttonPanel);
            systemInfoPanel.Controls.Add(titleLabel);

            this.Controls.Add(systemInfoPanel);
        }
    }
}