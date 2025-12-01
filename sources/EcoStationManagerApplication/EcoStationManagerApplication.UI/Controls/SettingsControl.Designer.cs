using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    partial class SettingsControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.headerPanel = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.settingsSection = new System.Windows.Forms.FlowLayoutPanel();
            this.businessCard = new System.Windows.Forms.Panel();
            this.businessTable = new System.Windows.Forms.TableLayoutPanel();
            this.lblBusinessName = new System.Windows.Forms.Label();
            this.txtBusinessName = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.btnSaveBusinessInfo = new System.Windows.Forms.Button();
            this.notificationCard = new System.Windows.Forms.Panel();
            this.notificationFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSaveNotificationSettings = new System.Windows.Forms.Button();
            this.inventoryCard = new System.Windows.Forms.Panel();
            this.lblStockAlert = new System.Windows.Forms.Label();
            this.numStockAlertLevel = new System.Windows.Forms.NumericUpDown();
            this.lblDefaultUnit = new System.Windows.Forms.Label();
            this.cmbDefaultUnit = new System.Windows.Forms.ComboBox();
            this.btnSaveInventorySettings = new System.Windows.Forms.Button();
            this.packagingCard = new System.Windows.Forms.Panel();
            this.lblMaxReuse = new System.Windows.Forms.Label();
            this.numMaxReuse = new System.Windows.Forms.NumericUpDown();
            this.lblCleaningCycle = new System.Windows.Forms.Label();
            this.numCleaningCycle = new System.Windows.Forms.NumericUpDown();
            this.lblTargetRecovery = new System.Windows.Forms.Label();
            this.numTargetRecoveryRate = new System.Windows.Forms.NumericUpDown();
            this.percentLabel = new System.Windows.Forms.Label();
            this.btnSavePackagingSettings = new System.Windows.Forms.Button();
            this.chkNewOrderNotification = new System.Windows.Forms.CheckBox();
            this.chkLowStockAlert = new System.Windows.Forms.CheckBox();
            this.chkPackagingReturn = new System.Windows.Forms.CheckBox();
            this.chkDailyReport = new System.Windows.Forms.CheckBox();
            this.systemInfoPanel = new System.Windows.Forms.Panel();
            this.systemTable = new System.Windows.Forms.TableLayoutPanel();
            this.lblVersion = new System.Windows.Forms.Label();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.lblInstallDate = new System.Windows.Forms.Label();
            this.txtInstallDate = new System.Windows.Forms.TextBox();
            this.lblDataSize = new System.Windows.Forms.Label();
            this.txtDataSize = new System.Windows.Forms.TextBox();
            this.buttonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnExportSystemLog = new System.Windows.Forms.Button();
            this.btnResetSettings = new System.Windows.Forms.Button();
            this.systemTitleLabel = new System.Windows.Forms.Label();
            this.headerPanel.SuspendLayout();
            this.settingsSection.SuspendLayout();
            this.businessCard.SuspendLayout();
            this.businessTable.SuspendLayout();
            this.notificationCard.SuspendLayout();
            this.inventoryCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numStockAlertLevel)).BeginInit();
            this.packagingCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxReuse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCleaningCycle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTargetRecoveryRate)).BeginInit();
            this.systemInfoPanel.SuspendLayout();
            this.systemTable.SuspendLayout();
            this.buttonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerPanel
            // 
            this.headerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.headerPanel.Controls.Add(this.titleLabel);
            this.headerPanel.Location = new System.Drawing.Point(17, 17);
            this.headerPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(772, 52);
            this.headerPanel.TabIndex = 0;
            // 
            // titleLabel
            // 
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.titleLabel.Location = new System.Drawing.Point(0, 0);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(172, 52);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Cài đặt hệ thống";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // settingsSection
            // 
            this.settingsSection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsSection.AutoScroll = true;
            this.settingsSection.Controls.Add(this.businessCard);
            this.settingsSection.Controls.Add(this.notificationCard);
            this.settingsSection.Controls.Add(this.inventoryCard);
            this.settingsSection.Controls.Add(this.packagingCard);
            this.settingsSection.Location = new System.Drawing.Point(17, 78);
            this.settingsSection.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.settingsSection.Name = "settingsSection";
            this.settingsSection.Size = new System.Drawing.Size(772, 520);
            this.settingsSection.TabIndex = 1;
            // 
            // businessCard
            // 
            this.businessCard.BackColor = System.Drawing.Color.White;
            this.businessCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.businessCard.Controls.Add(this.businessTable);
            this.businessCard.Controls.Add(this.btnSaveBusinessInfo);
            this.businessCard.Location = new System.Drawing.Point(8, 9);
            this.businessCard.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.businessCard.Name = "businessCard";
            this.businessCard.Padding = new System.Windows.Forms.Padding(13, 13, 13, 13);
            this.businessCard.Size = new System.Drawing.Size(369, 277);
            this.businessCard.TabIndex = 0;
            // 
            // businessTable
            // 
            this.businessTable.ColumnCount = 1;
            this.businessTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.businessTable.Controls.Add(this.lblBusinessName, 0, 1);
            this.businessTable.Controls.Add(this.txtBusinessName, 0, 2);
            this.businessTable.Controls.Add(this.lblAddress, 0, 3);
            this.businessTable.Controls.Add(this.txtAddress, 0, 4);
            this.businessTable.Controls.Add(this.lblPhone, 0, 5);
            this.businessTable.Controls.Add(this.txtPhone, 0, 6);
            this.businessTable.Controls.Add(this.lblEmail, 0, 7);
            this.businessTable.Controls.Add(this.txtEmail, 0, 8);
            this.businessTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.businessTable.Location = new System.Drawing.Point(13, 13);
            this.businessTable.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.businessTable.Name = "businessTable";
            this.businessTable.RowCount = 9;
            this.businessTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.businessTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.businessTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.businessTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.businessTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.businessTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.businessTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.businessTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.businessTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.businessTable.Size = new System.Drawing.Size(341, 219);
            this.businessTable.TabIndex = 0;
            // 
            // lblBusinessName
            // 
            this.lblBusinessName.AutoSize = true;
            this.lblBusinessName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBusinessName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblBusinessName.Location = new System.Drawing.Point(2, 26);
            this.lblBusinessName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBusinessName.Name = "lblBusinessName";
            this.lblBusinessName.Size = new System.Drawing.Size(337, 22);
            this.lblBusinessName.TabIndex = 0;
            this.lblBusinessName.Text = "Tên doanh nghiệp";
            this.lblBusinessName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBusinessName
            // 
            this.txtBusinessName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBusinessName.Location = new System.Drawing.Point(2, 50);
            this.txtBusinessName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtBusinessName.Name = "txtBusinessName";
            this.txtBusinessName.Size = new System.Drawing.Size(337, 20);
            this.txtBusinessName.TabIndex = 1;
            this.txtBusinessName.Text = "EcoStation Refill";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAddress.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblAddress.Location = new System.Drawing.Point(2, 74);
            this.lblAddress.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(337, 22);
            this.lblAddress.TabIndex = 2;
            this.lblAddress.Text = "Địa chỉ";
            this.lblAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtAddress
            // 
            this.txtAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAddress.Location = new System.Drawing.Point(2, 98);
            this.txtAddress.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(337, 20);
            this.txtAddress.TabIndex = 3;
            this.txtAddress.Text = "123 Đường ABC, Quận 1, TP.HCM";
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPhone.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPhone.Location = new System.Drawing.Point(2, 122);
            this.lblPhone.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(337, 22);
            this.lblPhone.TabIndex = 4;
            this.lblPhone.Text = "Số điện thoại";
            this.lblPhone.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPhone
            // 
            this.txtPhone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPhone.Location = new System.Drawing.Point(2, 146);
            this.txtPhone.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(337, 20);
            this.txtPhone.TabIndex = 5;
            this.txtPhone.Text = "0909123456";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblEmail.Location = new System.Drawing.Point(2, 170);
            this.lblEmail.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(337, 22);
            this.lblEmail.TabIndex = 6;
            this.lblEmail.Text = "Email";
            this.lblEmail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtEmail
            // 
            this.txtEmail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEmail.Location = new System.Drawing.Point(2, 194);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(337, 20);
            this.txtEmail.TabIndex = 7;
            this.txtEmail.Text = "contact@ecostation.vn";
            // 
            // btnSaveBusinessInfo
            // 
            this.btnSaveBusinessInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnSaveBusinessInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSaveBusinessInfo.FlatAppearance.BorderSize = 0;
            this.btnSaveBusinessInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveBusinessInfo.ForeColor = System.Drawing.Color.White;
            this.btnSaveBusinessInfo.Location = new System.Drawing.Point(13, 232);
            this.btnSaveBusinessInfo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSaveBusinessInfo.Name = "btnSaveBusinessInfo";
            this.btnSaveBusinessInfo.Size = new System.Drawing.Size(341, 30);
            this.btnSaveBusinessInfo.TabIndex = 1;
            this.btnSaveBusinessInfo.Text = "Lưu thông tin";
            this.btnSaveBusinessInfo.UseVisualStyleBackColor = false;
            this.btnSaveBusinessInfo.Click += new System.EventHandler(this.btnSaveBusinessInfo_Click);
            // 
            // notificationCard
            // 
            this.notificationCard.BackColor = System.Drawing.Color.White;
            this.notificationCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.notificationCard.Controls.Add(this.notificationFlowPanel);
            this.notificationCard.Controls.Add(this.btnSaveNotificationSettings);
            this.notificationCard.Location = new System.Drawing.Point(393, 9);
            this.notificationCard.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.notificationCard.Name = "notificationCard";
            this.notificationCard.Padding = new System.Windows.Forms.Padding(13, 13, 13, 13);
            this.notificationCard.Size = new System.Drawing.Size(369, 277);
            this.notificationCard.TabIndex = 1;
            // 
            // notificationFlowPanel
            // 
            this.notificationFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.notificationFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.notificationFlowPanel.Location = new System.Drawing.Point(13, 13);
            this.notificationFlowPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.notificationFlowPanel.Name = "notificationFlowPanel";
            this.notificationFlowPanel.Size = new System.Drawing.Size(341, 219);
            this.notificationFlowPanel.TabIndex = 0;
            // 
            // btnSaveNotificationSettings
            // 
            this.btnSaveNotificationSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnSaveNotificationSettings.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSaveNotificationSettings.FlatAppearance.BorderSize = 0;
            this.btnSaveNotificationSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveNotificationSettings.ForeColor = System.Drawing.Color.White;
            this.btnSaveNotificationSettings.Location = new System.Drawing.Point(13, 232);
            this.btnSaveNotificationSettings.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSaveNotificationSettings.Name = "btnSaveNotificationSettings";
            this.btnSaveNotificationSettings.Size = new System.Drawing.Size(341, 30);
            this.btnSaveNotificationSettings.TabIndex = 1;
            this.btnSaveNotificationSettings.Text = "Lưu cài đặt";
            this.btnSaveNotificationSettings.UseVisualStyleBackColor = false;
            this.btnSaveNotificationSettings.Click += new System.EventHandler(this.btnSaveNotificationSettings_Click);
            // 
            // inventoryCard
            // 
            this.inventoryCard.BackColor = System.Drawing.Color.White;
            this.inventoryCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.inventoryCard.Controls.Add(this.lblStockAlert);
            this.inventoryCard.Controls.Add(this.numStockAlertLevel);
            this.inventoryCard.Controls.Add(this.lblDefaultUnit);
            this.inventoryCard.Controls.Add(this.cmbDefaultUnit);
            this.inventoryCard.Controls.Add(this.btnSaveInventorySettings);
            this.inventoryCard.Location = new System.Drawing.Point(8, 304);
            this.inventoryCard.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.inventoryCard.Name = "inventoryCard";
            this.inventoryCard.Padding = new System.Windows.Forms.Padding(13, 13, 13, 13);
            this.inventoryCard.Size = new System.Drawing.Size(369, 217);
            this.inventoryCard.TabIndex = 2;
            // 
            // lblStockAlert
            // 
            this.lblStockAlert.AutoSize = true;
            this.lblStockAlert.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblStockAlert.Location = new System.Drawing.Point(13, 35);
            this.lblStockAlert.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStockAlert.Name = "lblStockAlert";
            this.lblStockAlert.Size = new System.Drawing.Size(158, 19);
            this.lblStockAlert.TabIndex = 0;
            this.lblStockAlert.Text = "Mức cảnh báo tồn kho";
            // 
            // numStockAlertLevel
            // 
            this.numStockAlertLevel.Location = new System.Drawing.Point(13, 56);
            this.numStockAlertLevel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numStockAlertLevel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numStockAlertLevel.Name = "numStockAlertLevel";
            this.numStockAlertLevel.Size = new System.Drawing.Size(86, 20);
            this.numStockAlertLevel.TabIndex = 1;
            this.numStockAlertLevel.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // lblDefaultUnit
            // 
            this.lblDefaultUnit.AutoSize = true;
            this.lblDefaultUnit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDefaultUnit.Location = new System.Drawing.Point(112, 35);
            this.lblDefaultUnit.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDefaultUnit.Name = "lblDefaultUnit";
            this.lblDefaultUnit.Size = new System.Drawing.Size(146, 19);
            this.lblDefaultUnit.TabIndex = 2;
            this.lblDefaultUnit.Text = "Đơn vị tính mặc định";
            // 
            // cmbDefaultUnit
            // 
            this.cmbDefaultUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDefaultUnit.FormattingEnabled = true;
            this.cmbDefaultUnit.Items.AddRange(new object[] {
            "Lít (L)",
            "Mililit (ml)",
            "Kilogram (kg)",
            "Gram (g)"});
            this.cmbDefaultUnit.Location = new System.Drawing.Point(112, 56);
            this.cmbDefaultUnit.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbDefaultUnit.Name = "cmbDefaultUnit";
            this.cmbDefaultUnit.Size = new System.Drawing.Size(129, 21);
            this.cmbDefaultUnit.TabIndex = 3;
            // 
            // btnSaveInventorySettings
            // 
            this.btnSaveInventorySettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveInventorySettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnSaveInventorySettings.FlatAppearance.BorderSize = 0;
            this.btnSaveInventorySettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveInventorySettings.ForeColor = System.Drawing.Color.White;
            this.btnSaveInventorySettings.Location = new System.Drawing.Point(253, 173);
            this.btnSaveInventorySettings.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSaveInventorySettings.Name = "btnSaveInventorySettings";
            this.btnSaveInventorySettings.Size = new System.Drawing.Size(103, 30);
            this.btnSaveInventorySettings.TabIndex = 4;
            this.btnSaveInventorySettings.Text = "Lưu cài đặt";
            this.btnSaveInventorySettings.UseVisualStyleBackColor = false;
            this.btnSaveInventorySettings.Click += new System.EventHandler(this.btnSaveInventorySettings_Click);
            // 
            // packagingCard
            // 
            this.packagingCard.BackColor = System.Drawing.Color.White;
            this.packagingCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.packagingCard.Controls.Add(this.lblMaxReuse);
            this.packagingCard.Controls.Add(this.numMaxReuse);
            this.packagingCard.Controls.Add(this.lblCleaningCycle);
            this.packagingCard.Controls.Add(this.numCleaningCycle);
            this.packagingCard.Controls.Add(this.lblTargetRecovery);
            this.packagingCard.Controls.Add(this.numTargetRecoveryRate);
            this.packagingCard.Controls.Add(this.percentLabel);
            this.packagingCard.Controls.Add(this.btnSavePackagingSettings);
            this.packagingCard.Location = new System.Drawing.Point(393, 304);
            this.packagingCard.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.packagingCard.Name = "packagingCard";
            this.packagingCard.Padding = new System.Windows.Forms.Padding(13, 13, 13, 13);
            this.packagingCard.Size = new System.Drawing.Size(369, 217);
            this.packagingCard.TabIndex = 3;
            // 
            // lblMaxReuse
            // 
            this.lblMaxReuse.AutoSize = true;
            this.lblMaxReuse.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMaxReuse.Location = new System.Drawing.Point(13, 35);
            this.lblMaxReuse.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMaxReuse.Name = "lblMaxReuse";
            this.lblMaxReuse.Size = new System.Drawing.Size(171, 19);
            this.lblMaxReuse.TabIndex = 0;
            this.lblMaxReuse.Text = "Số lần tái sử dụng tối đa";
            // 
            // numMaxReuse
            // 
            this.numMaxReuse.Location = new System.Drawing.Point(13, 56);
            this.numMaxReuse.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numMaxReuse.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxReuse.Name = "numMaxReuse";
            this.numMaxReuse.Size = new System.Drawing.Size(86, 20);
            this.numMaxReuse.TabIndex = 1;
            this.numMaxReuse.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // lblCleaningCycle
            // 
            this.lblCleaningCycle.AutoSize = true;
            this.lblCleaningCycle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCleaningCycle.Location = new System.Drawing.Point(112, 35);
            this.lblCleaningCycle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCleaningCycle.Name = "lblCleaningCycle";
            this.lblCleaningCycle.Size = new System.Drawing.Size(151, 19);
            this.lblCleaningCycle.TabIndex = 2;
            this.lblCleaningCycle.Text = "Chu kỳ vệ sinh (ngày)";
            // 
            // numCleaningCycle
            // 
            this.numCleaningCycle.Location = new System.Drawing.Point(112, 56);
            this.numCleaningCycle.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numCleaningCycle.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCleaningCycle.Name = "numCleaningCycle";
            this.numCleaningCycle.Size = new System.Drawing.Size(86, 20);
            this.numCleaningCycle.TabIndex = 3;
            this.numCleaningCycle.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            // 
            // lblTargetRecovery
            // 
            this.lblTargetRecovery.AutoSize = true;
            this.lblTargetRecovery.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTargetRecovery.Location = new System.Drawing.Point(13, 87);
            this.lblTargetRecovery.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTargetRecovery.Name = "lblTargetRecovery";
            this.lblTargetRecovery.Size = new System.Drawing.Size(152, 19);
            this.lblTargetRecovery.TabIndex = 4;
            this.lblTargetRecovery.Text = "Tỷ lệ thu hồi mục tiêu";
            // 
            // numTargetRecoveryRate
            // 
            this.numTargetRecoveryRate.Location = new System.Drawing.Point(13, 108);
            this.numTargetRecoveryRate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numTargetRecoveryRate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTargetRecoveryRate.Name = "numTargetRecoveryRate";
            this.numTargetRecoveryRate.Size = new System.Drawing.Size(86, 20);
            this.numTargetRecoveryRate.TabIndex = 5;
            this.numTargetRecoveryRate.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // percentLabel
            // 
            this.percentLabel.AutoSize = true;
            this.percentLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.percentLabel.Location = new System.Drawing.Point(103, 111);
            this.percentLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.percentLabel.Name = "percentLabel";
            this.percentLabel.Size = new System.Drawing.Size(20, 19);
            this.percentLabel.TabIndex = 6;
            this.percentLabel.Text = "%";
            // 
            // btnSavePackagingSettings
            // 
            this.btnSavePackagingSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSavePackagingSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnSavePackagingSettings.FlatAppearance.BorderSize = 0;
            this.btnSavePackagingSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSavePackagingSettings.ForeColor = System.Drawing.Color.White;
            this.btnSavePackagingSettings.Location = new System.Drawing.Point(253, 173);
            this.btnSavePackagingSettings.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSavePackagingSettings.Name = "btnSavePackagingSettings";
            this.btnSavePackagingSettings.Size = new System.Drawing.Size(103, 30);
            this.btnSavePackagingSettings.TabIndex = 7;
            this.btnSavePackagingSettings.Text = "Lưu cài đặt";
            this.btnSavePackagingSettings.UseVisualStyleBackColor = false;
            this.btnSavePackagingSettings.Click += new System.EventHandler(this.btnSavePackagingSettings_Click);
            // 
            // chkNewOrderNotification
            // 
            this.chkNewOrderNotification.AutoSize = true;
            this.chkNewOrderNotification.Checked = true;
            this.chkNewOrderNotification.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNewOrderNotification.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkNewOrderNotification.Location = new System.Drawing.Point(3, 40);
            this.chkNewOrderNotification.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.chkNewOrderNotification.Name = "chkNewOrderNotification";
            this.chkNewOrderNotification.Size = new System.Drawing.Size(159, 19);
            this.chkNewOrderNotification.TabIndex = 0;
            this.chkNewOrderNotification.Text = "Thông báo đơn hàng mới";
            this.chkNewOrderNotification.UseVisualStyleBackColor = true;
            // 
            // chkLowStockAlert
            // 
            this.chkLowStockAlert.AutoSize = true;
            this.chkLowStockAlert.Checked = true;
            this.chkLowStockAlert.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLowStockAlert.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkLowStockAlert.Location = new System.Drawing.Point(3, 65);
            this.chkLowStockAlert.Name = "chkLowStockAlert";
            this.chkLowStockAlert.Size = new System.Drawing.Size(141, 19);
            this.chkLowStockAlert.TabIndex = 1;
            this.chkLowStockAlert.Text = "Cảnh báo tồn kho thấp";
            this.chkLowStockAlert.UseVisualStyleBackColor = true;
            // 
            // chkPackagingReturn
            // 
            this.chkPackagingReturn.AutoSize = true;
            this.chkPackagingReturn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkPackagingReturn.Location = new System.Drawing.Point(3, 90);
            this.chkPackagingReturn.Name = "chkPackagingReturn";
            this.chkPackagingReturn.Size = new System.Drawing.Size(161, 19);
            this.chkPackagingReturn.TabIndex = 2;
            this.chkPackagingReturn.Text = "Thông báo thu hồi bao bì";
            this.chkPackagingReturn.UseVisualStyleBackColor = true;
            // 
            // chkDailyReport
            // 
            this.chkDailyReport.AutoSize = true;
            this.chkDailyReport.Checked = true;
            this.chkDailyReport.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDailyReport.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkDailyReport.Location = new System.Drawing.Point(3, 115);
            this.chkDailyReport.Name = "chkDailyReport";
            this.chkDailyReport.Size = new System.Drawing.Size(121, 19);
            this.chkDailyReport.TabIndex = 3;
            this.chkDailyReport.Text = "Báo cáo hàng ngày";
            this.chkDailyReport.UseVisualStyleBackColor = true;
            // 
            // systemInfoPanel
            // 
            this.systemInfoPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.systemInfoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.systemInfoPanel.Controls.Add(this.systemTable);
            this.systemInfoPanel.Controls.Add(this.buttonPanel);
            this.systemInfoPanel.Controls.Add(this.systemTitleLabel);
            this.systemInfoPanel.Location = new System.Drawing.Point(17, 607);
            this.systemInfoPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.systemInfoPanel.Name = "systemInfoPanel";
            this.systemInfoPanel.Padding = new System.Windows.Forms.Padding(13, 13, 13, 13);
            this.systemInfoPanel.Size = new System.Drawing.Size(772, 173);
            this.systemInfoPanel.TabIndex = 2;
            // 
            // systemTable
            // 
            this.systemTable.ColumnCount = 3;
            this.systemTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.systemTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.systemTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.systemTable.Controls.Add(this.lblVersion, 0, 0);
            this.systemTable.Controls.Add(this.txtVersion, 0, 1);
            this.systemTable.Controls.Add(this.lblInstallDate, 1, 0);
            this.systemTable.Controls.Add(this.txtInstallDate, 1, 1);
            this.systemTable.Controls.Add(this.lblDataSize, 2, 0);
            this.systemTable.Controls.Add(this.txtDataSize, 2, 1);
            this.systemTable.Dock = System.Windows.Forms.DockStyle.Top;
            this.systemTable.Location = new System.Drawing.Point(13, 39);
            this.systemTable.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.systemTable.Name = "systemTable";
            this.systemTable.RowCount = 2;
            this.systemTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.systemTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.systemTable.Size = new System.Drawing.Size(744, 48);
            this.systemTable.TabIndex = 2;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblVersion.Location = new System.Drawing.Point(2, 0);
            this.lblVersion.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(293, 22);
            this.lblVersion.TabIndex = 0;
            this.lblVersion.Text = "Phiên bản";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtVersion
            // 
            this.txtVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtVersion.Location = new System.Drawing.Point(2, 24);
            this.txtVersion.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.ReadOnly = true;
            this.txtVersion.Size = new System.Drawing.Size(293, 20);
            this.txtVersion.TabIndex = 1;
            this.txtVersion.Text = "EcoStation Manager - Ver.0 (Offline)";
            // 
            // lblInstallDate
            // 
            this.lblInstallDate.AutoSize = true;
            this.lblInstallDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInstallDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblInstallDate.Location = new System.Drawing.Point(299, 0);
            this.lblInstallDate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblInstallDate.Name = "lblInstallDate";
            this.lblInstallDate.Size = new System.Drawing.Size(219, 22);
            this.lblInstallDate.TabIndex = 2;
            this.lblInstallDate.Text = "Ngày cài đặt";
            this.lblInstallDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtInstallDate
            // 
            this.txtInstallDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInstallDate.Location = new System.Drawing.Point(299, 24);
            this.txtInstallDate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtInstallDate.Name = "txtInstallDate";
            this.txtInstallDate.ReadOnly = true;
            this.txtInstallDate.Size = new System.Drawing.Size(219, 20);
            this.txtInstallDate.TabIndex = 3;
            this.txtInstallDate.Text = "15/03/2025";
            // 
            // lblDataSize
            // 
            this.lblDataSize.AutoSize = true;
            this.lblDataSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDataSize.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDataSize.Location = new System.Drawing.Point(522, 0);
            this.lblDataSize.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDataSize.Name = "lblDataSize";
            this.lblDataSize.Size = new System.Drawing.Size(220, 22);
            this.lblDataSize.TabIndex = 4;
            this.lblDataSize.Text = "Dung lượng dữ liệu";
            this.lblDataSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDataSize
            // 
            this.txtDataSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDataSize.Location = new System.Drawing.Point(522, 24);
            this.txtDataSize.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtDataSize.Name = "txtDataSize";
            this.txtDataSize.ReadOnly = true;
            this.txtDataSize.Size = new System.Drawing.Size(220, 20);
            this.txtDataSize.TabIndex = 5;
            this.txtDataSize.Text = "45.2 MB";
            // 
            // buttonPanel
            // 
            this.buttonPanel.Controls.Add(this.btnExportSystemLog);
            this.buttonPanel.Controls.Add(this.btnResetSettings);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonPanel.Location = new System.Drawing.Point(13, 123);
            this.buttonPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(744, 35);
            this.buttonPanel.TabIndex = 1;
            // 
            // btnExportSystemLog
            // 
            this.btnExportSystemLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnExportSystemLog.FlatAppearance.BorderSize = 0;
            this.btnExportSystemLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportSystemLog.Location = new System.Drawing.Point(2, 2);
            this.btnExportSystemLog.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnExportSystemLog.Name = "btnExportSystemLog";
            this.btnExportSystemLog.Size = new System.Drawing.Size(112, 30);
            this.btnExportSystemLog.TabIndex = 0;
            this.btnExportSystemLog.Text = "Xuất log hệ thống";
            this.btnExportSystemLog.UseVisualStyleBackColor = false;
            this.btnExportSystemLog.Click += new System.EventHandler(this.btnExportSystemLog_Click);
            // 
            // btnResetSettings
            // 
            this.btnResetSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.btnResetSettings.FlatAppearance.BorderSize = 0;
            this.btnResetSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetSettings.ForeColor = System.Drawing.Color.White;
            this.btnResetSettings.Location = new System.Drawing.Point(118, 2);
            this.btnResetSettings.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnResetSettings.Name = "btnResetSettings";
            this.btnResetSettings.Size = new System.Drawing.Size(112, 30);
            this.btnResetSettings.TabIndex = 1;
            this.btnResetSettings.Text = "Đặt lại cài đặt";
            this.btnResetSettings.UseVisualStyleBackColor = false;
            this.btnResetSettings.Click += new System.EventHandler(this.btnResetSettings_Click);
            // 
            // systemTitleLabel
            // 
            this.systemTitleLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.systemTitleLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.systemTitleLabel.Location = new System.Drawing.Point(13, 13);
            this.systemTitleLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.systemTitleLabel.Name = "systemTitleLabel";
            this.systemTitleLabel.Size = new System.Drawing.Size(744, 26);
            this.systemTitleLabel.TabIndex = 0;
            this.systemTitleLabel.Text = "Thông tin Hệ thống";
            this.systemTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.systemInfoPanel);
            this.Controls.Add(this.settingsSection);
            this.Controls.Add(this.headerPanel);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "SettingsControl";
            this.Size = new System.Drawing.Size(806, 797);
            this.headerPanel.ResumeLayout(false);
            this.settingsSection.ResumeLayout(false);
            this.businessCard.ResumeLayout(false);
            this.businessTable.ResumeLayout(false);
            this.businessTable.PerformLayout();
            this.notificationCard.ResumeLayout(false);
            this.inventoryCard.ResumeLayout(false);
            this.inventoryCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numStockAlertLevel)).EndInit();
            this.packagingCard.ResumeLayout(false);
            this.packagingCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxReuse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCleaningCycle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTargetRecoveryRate)).EndInit();
            this.systemInfoPanel.ResumeLayout(false);
            this.systemTable.ResumeLayout(false);
            this.systemTable.PerformLayout();
            this.buttonPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel headerPanel;
        private Label titleLabel;
        private FlowLayoutPanel settingsSection;
        private Panel businessCard;
        private TableLayoutPanel businessTable;
        private Label lblBusinessName;
        private TextBox txtBusinessName;
        private Label lblAddress;
        private TextBox txtAddress;
        private Label lblPhone;
        private TextBox txtPhone;
        private Label lblEmail;
        private TextBox txtEmail;
        private Button btnSaveBusinessInfo;
        private Panel notificationCard;
        private FlowLayoutPanel notificationFlowPanel;
        private CheckBox chkNewOrderNotification;
        private CheckBox chkLowStockAlert;
        private CheckBox chkPackagingReturn;
        private CheckBox chkDailyReport;
        private Button btnSaveNotificationSettings;
        private Panel inventoryCard;
        private Label lblStockAlert;
        private NumericUpDown numStockAlertLevel;
        private Label lblDefaultUnit;
        private ComboBox cmbDefaultUnit;
        private Button btnSaveInventorySettings;
        private Panel packagingCard;
        private Label lblMaxReuse;
        private NumericUpDown numMaxReuse;
        private Label lblCleaningCycle;
        private NumericUpDown numCleaningCycle;
        private Label lblTargetRecovery;
        private NumericUpDown numTargetRecoveryRate;
        private Label percentLabel;
        private Button btnSavePackagingSettings;
        private Panel systemInfoPanel;
        private TableLayoutPanel systemTable;
        private Label lblVersion;
        private TextBox txtVersion;
        private Label lblInstallDate;
        private TextBox txtInstallDate;
        private Label lblDataSize;
        private TextBox txtDataSize;
        private FlowLayoutPanel buttonPanel;
        private Button btnExportSystemLog;
        private Button btnResetSettings;
        private Label systemTitleLabel;
    }
}