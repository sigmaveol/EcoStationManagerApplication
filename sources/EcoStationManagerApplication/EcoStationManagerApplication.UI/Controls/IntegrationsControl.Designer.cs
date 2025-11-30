using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    partial class IntegrationsControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.titleLabelHeader = new System.Windows.Forms.Label();
            this.alertLabel = new System.Windows.Forms.Label();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.alertPanel = new System.Windows.Forms.Panel();
            this.integrationsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.googleFormsCard = new System.Windows.Forms.Panel();
            this.googleFormsTitle = new System.Windows.Forms.Label();
            this.googleFormsDesc = new System.Windows.Forms.Label();
            this.googleFormsUrlLabel = new System.Windows.Forms.Label();
            this.googleFormsUrlTextBox = new System.Windows.Forms.TextBox();
            this.googleFormsStatusLabel = new System.Windows.Forms.Label();
            this.googleFormsStatusValue = new System.Windows.Forms.Label();
            this.btnUpdateGoogleForms = new System.Windows.Forms.Button();
            this.googleSheetsCard = new System.Windows.Forms.Panel();
            this.googleSheetsTitle = new System.Windows.Forms.Label();
            this.googleSheetsDesc = new System.Windows.Forms.Label();
            this.googleSheetsUrlLabel = new System.Windows.Forms.Label();
            this.txtGoogleSheetsUrl = new System.Windows.Forms.TextBox();
            this.googleSheetsApiKeyLabel = new System.Windows.Forms.Label();
            this.txtGoogleSheetsApiKey = new System.Windows.Forms.TextBox();
            this.googleSheetsStatusLabel = new System.Windows.Forms.Label();
            this.statusLabelGoogleSheets = new System.Windows.Forms.Label();
            this.btnTestGoogleSheets = new System.Windows.Forms.Button();
            this.btnConnectGoogleSheets = new System.Windows.Forms.Button();
            this.gmailCard = new System.Windows.Forms.Panel();
            this.gmailTitle = new System.Windows.Forms.Label();
            this.gmailDesc = new System.Windows.Forms.Label();
            this.gmailServerLabel = new System.Windows.Forms.Label();
            this.txtGmailServer = new System.Windows.Forms.TextBox();
            this.gmailEmailLabel = new System.Windows.Forms.Label();
            this.txtGmailEmail = new System.Windows.Forms.TextBox();
            this.gmailPasswordLabel = new System.Windows.Forms.Label();
            this.txtGmailPassword = new System.Windows.Forms.TextBox();
            this.gmailStatusLabel = new System.Windows.Forms.Label();
            this.statusLabelGmail = new System.Windows.Forms.Label();
            this.btnTestGmail = new System.Windows.Forms.Button();
            this.btnConnectGmail = new System.Windows.Forms.Button();
            this.googleSheetsIntervalLabel = new System.Windows.Forms.Label();
            this.cmbGoogleSheetsInterval = new System.Windows.Forms.ComboBox();
            this.googleSheetsLastSyncLabel = new System.Windows.Forms.Label();
            this.lblLastSyncGoogleSheets = new System.Windows.Forms.Label();
            this.googleSheetsTimer = new System.Windows.Forms.Timer(this.components);
            this.headerPanel.SuspendLayout();
            this.alertPanel.SuspendLayout();
            this.integrationsPanel.SuspendLayout();
            this.googleFormsCard.SuspendLayout();
            this.googleSheetsCard.SuspendLayout();
            this.gmailCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleLabelHeader
            // 
            this.titleLabelHeader.AutoSize = true;
            this.titleLabelHeader.Dock = System.Windows.Forms.DockStyle.Left;
            this.titleLabelHeader.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.titleLabelHeader.Location = new System.Drawing.Point(0, 0);
            this.titleLabelHeader.Name = "titleLabelHeader";
            this.titleLabelHeader.Size = new System.Drawing.Size(327, 37);
            this.titleLabelHeader.TabIndex = 0;
            this.titleLabelHeader.Text = "Tích hợp Công cụ Online";
            // 
            // alertLabel
            // 
            this.alertLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alertLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.alertLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            this.alertLabel.Location = new System.Drawing.Point(15, 15);
            this.alertLabel.Name = "alertLabel";
            this.alertLabel.Size = new System.Drawing.Size(890, 30);
            this.alertLabel.TabIndex = 0;
            this.alertLabel.Text = "Lưu ý: Các tích hợp này yêu cầu kết nối internet để hoạt động. Dữ liệu sẽ được đồ" +
    "ng bộ về hệ thống offline.";
            this.alertLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // headerPanel
            // 
            this.headerPanel.Controls.Add(this.titleLabelHeader);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(10, 10);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(920, 60);
            this.headerPanel.TabIndex = 0;
            // 
            // alertPanel
            // 
            this.alertPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(242)))), ((int)(((byte)(253)))));
            this.alertPanel.Controls.Add(this.alertLabel);
            this.alertPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.alertPanel.Location = new System.Drawing.Point(10, 70);
            this.alertPanel.Name = "alertPanel";
            this.alertPanel.Padding = new System.Windows.Forms.Padding(15);
            this.alertPanel.Size = new System.Drawing.Size(920, 60);
            this.alertPanel.TabIndex = 1;
            // 
            // integrationsPanel
            // 
            this.integrationsPanel.AutoSize = true;
            this.integrationsPanel.Controls.Add(this.googleFormsCard);
            this.integrationsPanel.Controls.Add(this.googleSheetsCard);
            this.integrationsPanel.Controls.Add(this.gmailCard);
            this.integrationsPanel.Controls.Add(this.googleSheetsIntervalLabel);
            this.integrationsPanel.Controls.Add(this.cmbGoogleSheetsInterval);
            this.integrationsPanel.Controls.Add(this.googleSheetsLastSyncLabel);
            this.integrationsPanel.Controls.Add(this.lblLastSyncGoogleSheets);
            this.integrationsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.integrationsPanel.Location = new System.Drawing.Point(10, 130);
            this.integrationsPanel.Name = "integrationsPanel";
            this.integrationsPanel.Size = new System.Drawing.Size(920, 689);
            this.integrationsPanel.TabIndex = 2;
            // 
            // googleFormsCard
            // 
            this.googleFormsCard.BackColor = System.Drawing.Color.White;
            this.googleFormsCard.Controls.Add(this.googleFormsTitle);
            this.googleFormsCard.Controls.Add(this.googleFormsDesc);
            this.googleFormsCard.Controls.Add(this.googleFormsUrlLabel);
            this.googleFormsCard.Controls.Add(this.googleFormsUrlTextBox);
            this.googleFormsCard.Controls.Add(this.googleFormsStatusLabel);
            this.googleFormsCard.Controls.Add(this.googleFormsStatusValue);
            this.googleFormsCard.Controls.Add(this.btnUpdateGoogleForms);
            this.googleFormsCard.Location = new System.Drawing.Point(10, 10);
            this.googleFormsCard.Margin = new System.Windows.Forms.Padding(10);
            this.googleFormsCard.Name = "googleFormsCard";
            this.googleFormsCard.Padding = new System.Windows.Forms.Padding(15);
            this.googleFormsCard.Size = new System.Drawing.Size(430, 300);
            this.googleFormsCard.TabIndex = 0;
            // 
            // googleFormsTitle
            // 
            this.googleFormsTitle.AutoSize = true;
            this.googleFormsTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.googleFormsTitle.Location = new System.Drawing.Point(13, 11);
            this.googleFormsTitle.Name = "googleFormsTitle";
            this.googleFormsTitle.Size = new System.Drawing.Size(173, 32);
            this.googleFormsTitle.TabIndex = 0;
            this.googleFormsTitle.Text = "Google Forms";
            // 
            // googleFormsDesc
            // 
            this.googleFormsDesc.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.googleFormsDesc.Location = new System.Drawing.Point(15, 45);
            this.googleFormsDesc.Name = "googleFormsDesc";
            this.googleFormsDesc.Size = new System.Drawing.Size(380, 40);
            this.googleFormsDesc.TabIndex = 1;
            this.googleFormsDesc.Text = "Thu thập đơn hàng từ khách hàng qua Google Forms";
            // 
            // googleFormsUrlLabel
            // 
            this.googleFormsUrlLabel.AutoSize = true;
            this.googleFormsUrlLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.googleFormsUrlLabel.Location = new System.Drawing.Point(15, 95);
            this.googleFormsUrlLabel.Name = "googleFormsUrlLabel";
            this.googleFormsUrlLabel.Size = new System.Drawing.Size(140, 20);
            this.googleFormsUrlLabel.TabIndex = 2;
            this.googleFormsUrlLabel.Text = "Google Forms URL";
            // 
            // googleFormsUrlTextBox
            // 
            this.googleFormsUrlTextBox.Location = new System.Drawing.Point(15, 120);
            this.googleFormsUrlTextBox.Name = "googleFormsUrlTextBox";
            this.googleFormsUrlTextBox.Size = new System.Drawing.Size(380, 27);
            this.googleFormsUrlTextBox.TabIndex = 3;
            this.googleFormsUrlTextBox.Text = "https://forms.google.com/...";
            // 
            // googleFormsStatusLabel
            // 
            this.googleFormsStatusLabel.AutoSize = true;
            this.googleFormsStatusLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.googleFormsStatusLabel.Location = new System.Drawing.Point(15, 230);
            this.googleFormsStatusLabel.Name = "googleFormsStatusLabel";
            this.googleFormsStatusLabel.Size = new System.Drawing.Size(84, 20);
            this.googleFormsStatusLabel.TabIndex = 4;
            this.googleFormsStatusLabel.Text = "Trạng thái:";
            // 
            // googleFormsStatusValue
            // 
            this.googleFormsStatusValue.AutoSize = true;
            this.googleFormsStatusValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.googleFormsStatusValue.ForeColor = System.Drawing.Color.Green;
            this.googleFormsStatusValue.Location = new System.Drawing.Point(108, 230);
            this.googleFormsStatusValue.Name = "googleFormsStatusValue";
            this.googleFormsStatusValue.Size = new System.Drawing.Size(80, 20);
            this.googleFormsStatusValue.TabIndex = 5;
            this.googleFormsStatusValue.Text = "Đã kết nối";
            // 
            // btnUpdateGoogleForms
            // 
            this.btnUpdateGoogleForms.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnUpdateGoogleForms.FlatAppearance.BorderSize = 0;
            this.btnUpdateGoogleForms.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdateGoogleForms.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnUpdateGoogleForms.ForeColor = System.Drawing.Color.White;
            this.btnUpdateGoogleForms.Location = new System.Drawing.Point(275, 225);
            this.btnUpdateGoogleForms.Name = "btnUpdateGoogleForms";
            this.btnUpdateGoogleForms.Size = new System.Drawing.Size(120, 35);
            this.btnUpdateGoogleForms.TabIndex = 6;
            this.btnUpdateGoogleForms.Text = "Cập nhật";
            this.btnUpdateGoogleForms.UseVisualStyleBackColor = false;
            // 
            // googleSheetsCard
            // 
            this.googleSheetsCard.BackColor = System.Drawing.Color.White;
            this.googleSheetsCard.Controls.Add(this.googleSheetsTitle);
            this.googleSheetsCard.Controls.Add(this.googleSheetsDesc);
            this.googleSheetsCard.Controls.Add(this.googleSheetsUrlLabel);
            this.googleSheetsCard.Controls.Add(this.txtGoogleSheetsUrl);
            this.googleSheetsCard.Controls.Add(this.googleSheetsApiKeyLabel);
            this.googleSheetsCard.Controls.Add(this.txtGoogleSheetsApiKey);
            this.googleSheetsCard.Controls.Add(this.googleSheetsStatusLabel);
            this.googleSheetsCard.Controls.Add(this.statusLabelGoogleSheets);
            this.googleSheetsCard.Controls.Add(this.btnTestGoogleSheets);
            this.googleSheetsCard.Controls.Add(this.btnConnectGoogleSheets);
            this.googleSheetsCard.Location = new System.Drawing.Point(460, 10);
            this.googleSheetsCard.Margin = new System.Windows.Forms.Padding(10);
            this.googleSheetsCard.Name = "googleSheetsCard";
            this.googleSheetsCard.Padding = new System.Windows.Forms.Padding(15);
            this.googleSheetsCard.Size = new System.Drawing.Size(430, 300);
            this.googleSheetsCard.TabIndex = 1;
            this.googleSheetsCard.Paint += new System.Windows.Forms.PaintEventHandler(this.googleSheetsCard_Paint);
            // 
            // googleSheetsTitle
            // 
            this.googleSheetsTitle.AutoSize = true;
            this.googleSheetsTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.googleSheetsTitle.Location = new System.Drawing.Point(13, 12);
            this.googleSheetsTitle.Name = "googleSheetsTitle";
            this.googleSheetsTitle.Size = new System.Drawing.Size(176, 32);
            this.googleSheetsTitle.TabIndex = 0;
            this.googleSheetsTitle.Text = "Google Sheets";
            // 
            // googleSheetsDesc
            // 
            this.googleSheetsDesc.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.googleSheetsDesc.Location = new System.Drawing.Point(15, 45);
            this.googleSheetsDesc.Name = "googleSheetsDesc";
            this.googleSheetsDesc.Size = new System.Drawing.Size(380, 40);
            this.googleSheetsDesc.TabIndex = 1;
            this.googleSheetsDesc.Text = "Đồng bộ dữ liệu đặt hàng từ Google Sheets";
            // 
            // googleSheetsUrlLabel
            // 
            this.googleSheetsUrlLabel.AutoSize = true;
            this.googleSheetsUrlLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.googleSheetsUrlLabel.Location = new System.Drawing.Point(15, 95);
            this.googleSheetsUrlLabel.Name = "googleSheetsUrlLabel";
            this.googleSheetsUrlLabel.Size = new System.Drawing.Size(142, 20);
            this.googleSheetsUrlLabel.TabIndex = 2;
            this.googleSheetsUrlLabel.Text = "Google Sheets URL";
            // 
            // txtGoogleSheetsUrl
            // 
            this.txtGoogleSheetsUrl.Location = new System.Drawing.Point(15, 120);
            this.txtGoogleSheetsUrl.Name = "txtGoogleSheetsUrl";
            this.txtGoogleSheetsUrl.Size = new System.Drawing.Size(380, 27);
            this.txtGoogleSheetsUrl.TabIndex = 3;
            this.txtGoogleSheetsUrl.Text = "https://docs.google.com/spreadsheets/...";
            // 
            // googleSheetsApiKeyLabel
            // 
            this.googleSheetsApiKeyLabel.AutoSize = true;
            this.googleSheetsApiKeyLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.googleSheetsApiKeyLabel.Location = new System.Drawing.Point(15, 155);
            this.googleSheetsApiKeyLabel.Name = "googleSheetsApiKeyLabel";
            this.googleSheetsApiKeyLabel.Size = new System.Drawing.Size(129, 20);
            this.googleSheetsApiKeyLabel.TabIndex = 4;
            this.googleSheetsApiKeyLabel.Text = "API Key (Nếu có)";
            // 
            // txtGoogleSheetsApiKey
            // 
            this.txtGoogleSheetsApiKey.Location = new System.Drawing.Point(15, 180);
            this.txtGoogleSheetsApiKey.Name = "txtGoogleSheetsApiKey";
            this.txtGoogleSheetsApiKey.Size = new System.Drawing.Size(380, 27);
            this.txtGoogleSheetsApiKey.TabIndex = 5;
            this.txtGoogleSheetsApiKey.UseSystemPasswordChar = true;
            // 
            // googleSheetsStatusLabel
            // 
            this.googleSheetsStatusLabel.AutoSize = true;
            this.googleSheetsStatusLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.googleSheetsStatusLabel.Location = new System.Drawing.Point(15, 230);
            this.googleSheetsStatusLabel.Name = "googleSheetsStatusLabel";
            this.googleSheetsStatusLabel.Size = new System.Drawing.Size(84, 20);
            this.googleSheetsStatusLabel.TabIndex = 6;
            this.googleSheetsStatusLabel.Text = "Trạng thái:";
            // 
            // statusLabelGoogleSheets
            // 
            this.statusLabelGoogleSheets.AutoSize = true;
            this.statusLabelGoogleSheets.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.statusLabelGoogleSheets.ForeColor = System.Drawing.Color.Red;
            this.statusLabelGoogleSheets.Location = new System.Drawing.Point(11, 265);
            this.statusLabelGoogleSheets.Name = "statusLabelGoogleSheets";
            this.statusLabelGoogleSheets.Size = new System.Drawing.Size(97, 20);
            this.statusLabelGoogleSheets.TabIndex = 7;
            this.statusLabelGoogleSheets.Text = "Chưa kết nối";
            // 
            // btnTestGoogleSheets
            // 
            this.btnTestGoogleSheets.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.btnTestGoogleSheets.FlatAppearance.BorderSize = 0;
            this.btnTestGoogleSheets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestGoogleSheets.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnTestGoogleSheets.ForeColor = System.Drawing.Color.White;
            this.btnTestGoogleSheets.Location = new System.Drawing.Point(150, 225);
            this.btnTestGoogleSheets.Name = "btnTestGoogleSheets";
            this.btnTestGoogleSheets.Size = new System.Drawing.Size(120, 35);
            this.btnTestGoogleSheets.TabIndex = 8;
            this.btnTestGoogleSheets.Text = "Kiểm tra";
            this.btnTestGoogleSheets.UseVisualStyleBackColor = false;
            // 
            // btnConnectGoogleSheets
            // 
            this.btnConnectGoogleSheets.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnConnectGoogleSheets.FlatAppearance.BorderSize = 0;
            this.btnConnectGoogleSheets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnectGoogleSheets.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnConnectGoogleSheets.ForeColor = System.Drawing.Color.White;
            this.btnConnectGoogleSheets.Location = new System.Drawing.Point(275, 225);
            this.btnConnectGoogleSheets.Name = "btnConnectGoogleSheets";
            this.btnConnectGoogleSheets.Size = new System.Drawing.Size(120, 35);
            this.btnConnectGoogleSheets.TabIndex = 9;
            this.btnConnectGoogleSheets.Text = "Kết nối";
            this.btnConnectGoogleSheets.UseVisualStyleBackColor = false;
            // 
            // gmailCard
            // 
            this.gmailCard.BackColor = System.Drawing.Color.White;
            this.gmailCard.Controls.Add(this.gmailTitle);
            this.gmailCard.Controls.Add(this.gmailDesc);
            this.gmailCard.Controls.Add(this.gmailServerLabel);
            this.gmailCard.Controls.Add(this.txtGmailServer);
            this.gmailCard.Controls.Add(this.gmailEmailLabel);
            this.gmailCard.Controls.Add(this.txtGmailEmail);
            this.gmailCard.Controls.Add(this.gmailPasswordLabel);
            this.gmailCard.Controls.Add(this.txtGmailPassword);
            this.gmailCard.Controls.Add(this.gmailStatusLabel);
            this.gmailCard.Controls.Add(this.statusLabelGmail);
            this.gmailCard.Controls.Add(this.btnTestGmail);
            this.gmailCard.Controls.Add(this.btnConnectGmail);
            this.gmailCard.Location = new System.Drawing.Point(10, 330);
            this.gmailCard.Margin = new System.Windows.Forms.Padding(10);
            this.gmailCard.Name = "gmailCard";
            this.gmailCard.Padding = new System.Windows.Forms.Padding(15);
            this.gmailCard.Size = new System.Drawing.Size(430, 349);
            this.gmailCard.TabIndex = 2;
            // 
            // gmailTitle
            // 
            this.gmailTitle.AutoSize = true;
            this.gmailTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.gmailTitle.Location = new System.Drawing.Point(13, 12);
            this.gmailTitle.Name = "gmailTitle";
            this.gmailTitle.Size = new System.Drawing.Size(80, 32);
            this.gmailTitle.TabIndex = 0;
            this.gmailTitle.Text = "Gmail";
            // 
            // gmailDesc
            // 
            this.gmailDesc.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.gmailDesc.Location = new System.Drawing.Point(15, 45);
            this.gmailDesc.Name = "gmailDesc";
            this.gmailDesc.Size = new System.Drawing.Size(380, 40);
            this.gmailDesc.TabIndex = 1;
            this.gmailDesc.Text = "Nhận thông tin đơn hàng qua mail";
            // 
            // gmailServerLabel
            // 
            this.gmailServerLabel.AutoSize = true;
            this.gmailServerLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.gmailServerLabel.Location = new System.Drawing.Point(15, 95);
            this.gmailServerLabel.Name = "gmailServerLabel";
            this.gmailServerLabel.Size = new System.Drawing.Size(97, 20);
            this.gmailServerLabel.TabIndex = 2;
            this.gmailServerLabel.Text = "IMAP Server";
            // 
            // txtGmailServer
            // 
            this.txtGmailServer.Location = new System.Drawing.Point(15, 120);
            this.txtGmailServer.Name = "txtGmailServer";
            this.txtGmailServer.Size = new System.Drawing.Size(380, 27);
            this.txtGmailServer.TabIndex = 3;
            this.txtGmailServer.Text = "imap.gmail.com";
            // 
            // gmailEmailLabel
            // 
            this.gmailEmailLabel.AutoSize = true;
            this.gmailEmailLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.gmailEmailLabel.Location = new System.Drawing.Point(15, 155);
            this.gmailEmailLabel.Name = "gmailEmailLabel";
            this.gmailEmailLabel.Size = new System.Drawing.Size(47, 20);
            this.gmailEmailLabel.TabIndex = 4;
            this.gmailEmailLabel.Text = "Email";
            // 
            // txtGmailEmail
            // 
            this.txtGmailEmail.Location = new System.Drawing.Point(15, 180);
            this.txtGmailEmail.Name = "txtGmailEmail";
            this.txtGmailEmail.Size = new System.Drawing.Size(380, 27);
            this.txtGmailEmail.TabIndex = 5;
            // 
            // gmailPasswordLabel
            // 
            this.gmailPasswordLabel.AutoSize = true;
            this.gmailPasswordLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.gmailPasswordLabel.Location = new System.Drawing.Point(15, 210);
            this.gmailPasswordLabel.Name = "gmailPasswordLabel";
            this.gmailPasswordLabel.Size = new System.Drawing.Size(75, 20);
            this.gmailPasswordLabel.TabIndex = 6;
            this.gmailPasswordLabel.Text = "Mật khẩu";
            // 
            // txtGmailPassword
            // 
            this.txtGmailPassword.Location = new System.Drawing.Point(15, 234);
            this.txtGmailPassword.Name = "txtGmailPassword";
            this.txtGmailPassword.Size = new System.Drawing.Size(380, 27);
            this.txtGmailPassword.TabIndex = 7;
            this.txtGmailPassword.UseSystemPasswordChar = true;
            // 
            // gmailStatusLabel
            // 
            this.gmailStatusLabel.AutoSize = true;
            this.gmailStatusLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.gmailStatusLabel.Location = new System.Drawing.Point(15, 317);
            this.gmailStatusLabel.Name = "gmailStatusLabel";
            this.gmailStatusLabel.Size = new System.Drawing.Size(84, 20);
            this.gmailStatusLabel.TabIndex = 8;
            this.gmailStatusLabel.Text = "Trạng thái:";
            // 
            // statusLabelGmail
            // 
            this.statusLabelGmail.AutoSize = true;
            this.statusLabelGmail.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.statusLabelGmail.ForeColor = System.Drawing.Color.Red;
            this.statusLabelGmail.Location = new System.Drawing.Point(105, 317);
            this.statusLabelGmail.Name = "statusLabelGmail";
            this.statusLabelGmail.Size = new System.Drawing.Size(97, 20);
            this.statusLabelGmail.TabIndex = 9;
            this.statusLabelGmail.Text = "Chưa kết nối";
            // 
            // btnTestGmail
            // 
            this.btnTestGmail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.btnTestGmail.FlatAppearance.BorderSize = 0;
            this.btnTestGmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestGmail.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnTestGmail.ForeColor = System.Drawing.Color.White;
            this.btnTestGmail.Location = new System.Drawing.Point(150, 273);
            this.btnTestGmail.Name = "btnTestGmail";
            this.btnTestGmail.Size = new System.Drawing.Size(120, 35);
            this.btnTestGmail.TabIndex = 10;
            this.btnTestGmail.Text = "Kiểm tra";
            this.btnTestGmail.UseVisualStyleBackColor = false;
            // 
            // btnConnectGmail
            // 
            this.btnConnectGmail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnConnectGmail.FlatAppearance.BorderSize = 0;
            this.btnConnectGmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnectGmail.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnConnectGmail.ForeColor = System.Drawing.Color.White;
            this.btnConnectGmail.Location = new System.Drawing.Point(275, 273);
            this.btnConnectGmail.Name = "btnConnectGmail";
            this.btnConnectGmail.Size = new System.Drawing.Size(120, 35);
            this.btnConnectGmail.TabIndex = 11;
            this.btnConnectGmail.Text = "Kết nối";
            this.btnConnectGmail.UseVisualStyleBackColor = false;
            // 
            // googleSheetsIntervalLabel
            // 
            this.googleSheetsIntervalLabel.AutoSize = true;
            this.googleSheetsIntervalLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.googleSheetsIntervalLabel.Location = new System.Drawing.Point(453, 320);
            this.googleSheetsIntervalLabel.Name = "googleSheetsIntervalLabel";
            this.googleSheetsIntervalLabel.Size = new System.Drawing.Size(105, 20);
            this.googleSheetsIntervalLabel.TabIndex = 10;
            this.googleSheetsIntervalLabel.Text = "Chu kỳ (phút)";
            // 
            // cmbGoogleSheetsInterval
            // 
            this.cmbGoogleSheetsInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGoogleSheetsInterval.Items.AddRange(new object[] {
            "15",
            "30",
            "60",
            "120"});
            this.cmbGoogleSheetsInterval.Location = new System.Drawing.Point(564, 323);
            this.cmbGoogleSheetsInterval.Name = "cmbGoogleSheetsInterval";
            this.cmbGoogleSheetsInterval.Size = new System.Drawing.Size(80, 28);
            this.cmbGoogleSheetsInterval.TabIndex = 11;
            // 
            // googleSheetsLastSyncLabel
            // 
            this.googleSheetsLastSyncLabel.AutoSize = true;
            this.googleSheetsLastSyncLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.googleSheetsLastSyncLabel.Location = new System.Drawing.Point(650, 320);
            this.googleSheetsLastSyncLabel.Name = "googleSheetsLastSyncLabel";
            this.googleSheetsLastSyncLabel.Size = new System.Drawing.Size(166, 20);
            this.googleSheetsLastSyncLabel.TabIndex = 14;
            this.googleSheetsLastSyncLabel.Text = "Lần đồng bộ gần nhất:";
            // 
            // lblLastSyncGoogleSheets
            // 
            this.lblLastSyncGoogleSheets.AutoSize = true;
            this.lblLastSyncGoogleSheets.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblLastSyncGoogleSheets.Location = new System.Drawing.Point(822, 320);
            this.lblLastSyncGoogleSheets.Name = "lblLastSyncGoogleSheets";
            this.lblLastSyncGoogleSheets.Size = new System.Drawing.Size(63, 20);
            this.lblLastSyncGoogleSheets.TabIndex = 15;
            this.lblLastSyncGoogleSheets.Text = "Chưa có";
            // 
            // googleSheetsTimer
            // 
            this.googleSheetsTimer.Interval = 3600000;
            // 
            // IntegrationsControl
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.integrationsPanel);
            this.Controls.Add(this.alertPanel);
            this.Controls.Add(this.headerPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "IntegrationsControl";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(940, 829);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.alertPanel.ResumeLayout(false);
            this.integrationsPanel.ResumeLayout(false);
            this.integrationsPanel.PerformLayout();
            this.googleFormsCard.ResumeLayout(false);
            this.googleFormsCard.PerformLayout();
            this.googleSheetsCard.ResumeLayout(false);
            this.googleSheetsCard.PerformLayout();
            this.gmailCard.ResumeLayout(false);
            this.gmailCard.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        // Main containers
        private Label titleLabelHeader;
        private Label alertLabel;
        private Panel headerPanel;
        private Panel alertPanel;
        private FlowLayoutPanel integrationsPanel;

        // Google Forms Card
        private Panel googleFormsCard;
        private Label googleFormsTitle;
        private Label googleFormsDesc;
        private Label googleFormsUrlLabel;
        private TextBox googleFormsUrlTextBox;
        private Label googleFormsStatusLabel;
        private Label googleFormsStatusValue;
        private Button btnUpdateGoogleForms;

        // Google Sheets Card
        private Panel googleSheetsCard;
        private Label googleSheetsTitle;
        private Label googleSheetsDesc;
        private Label googleSheetsUrlLabel;
        private TextBox txtGoogleSheetsUrl;
        private Label googleSheetsApiKeyLabel;
        private TextBox txtGoogleSheetsApiKey;
        private Label googleSheetsStatusLabel;
        private Label statusLabelGoogleSheets;
        private Label googleSheetsIntervalLabel;
        private ComboBox cmbGoogleSheetsInterval;
        private Button btnTestGoogleSheets;
        private Button btnConnectGoogleSheets;

        // Timer
        private Timer googleSheetsTimer;
        private Label googleSheetsLastSyncLabel;
        private Label lblLastSyncGoogleSheets;

        // Gmail Card
        private Panel gmailCard;
        private Label gmailTitle;
        private Label gmailDesc;
        private Label gmailServerLabel;
        private TextBox txtGmailServer;
        private Label gmailEmailLabel;
        private TextBox txtGmailEmail;
        private Label gmailPasswordLabel;
        private TextBox txtGmailPassword;
        private Label gmailStatusLabel;
        private Label statusLabelGmail;
        private Button btnTestGmail;
        private Button btnConnectGmail;
    }
}