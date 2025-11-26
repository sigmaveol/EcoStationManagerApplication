using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    partial class BackupControl
    {
        private System.ComponentModel.IContainer components = null;
        private Panel headerPanel;
        private Panel alertPanel;
        private FlowLayoutPanel backupSection;
        private Button btnBackupExcel;
        private Button btnBackupPDF;
        private Button btnRestore;
        private TextBox txtRestoreFile;
        private Button browseButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.headerPanel = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.alertPanel = new System.Windows.Forms.Panel();
            this.alertLabel = new System.Windows.Forms.Label();
            this.backupSection = new System.Windows.Forms.FlowLayoutPanel();
            this.excelCard = new System.Windows.Forms.Panel();
            this.excelIconLabel = new System.Windows.Forms.Label();
            this.excelTitleLabel = new System.Windows.Forms.Label();
            this.excelDescLabel = new System.Windows.Forms.Label();
            this.btnBackupExcel = new System.Windows.Forms.Button();
            this.pdfCard = new System.Windows.Forms.Panel();
            this.pdfIconLabel = new System.Windows.Forms.Label();
            this.pdfTitleLabel = new System.Windows.Forms.Label();
            this.pdfDescLabel = new System.Windows.Forms.Label();
            this.btnBackupPDF = new System.Windows.Forms.Button();
            this.restoreCard = new System.Windows.Forms.Panel();
            this.restoreIconLabel = new System.Windows.Forms.Label();
            this.restoreTitleLabel = new System.Windows.Forms.Label();
            this.restoreDescLabel = new System.Windows.Forms.Label();
            this.filePanel = new System.Windows.Forms.Panel();
            this.txtRestoreFile = new System.Windows.Forms.TextBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.btnRestore = new System.Windows.Forms.Button();
            this.headerPanel.SuspendLayout();
            this.alertPanel.SuspendLayout();
            this.backupSection.SuspendLayout();
            this.excelCard.SuspendLayout();
            this.pdfCard.SuspendLayout();
            this.restoreCard.SuspendLayout();
            this.filePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerPanel
            // 
            this.headerPanel.Controls.Add(this.titleLabel);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(10, 10);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 20);
            this.headerPanel.Size = new System.Drawing.Size(920, 60);
            this.headerPanel.TabIndex = 0;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.titleLabel.Location = new System.Drawing.Point(0, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(260, 37);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Sao lưu && Phục hồi";
            // 
            // alertPanel
            // 
            this.alertPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(242)))), ((int)(((byte)(253)))));
            this.alertPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.alertPanel.Controls.Add(this.alertLabel);
            this.alertPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.alertPanel.Location = new System.Drawing.Point(10, 70);
            this.alertPanel.Name = "alertPanel";
            this.alertPanel.Padding = new System.Windows.Forms.Padding(15, 15, 15, 20);
            this.alertPanel.Size = new System.Drawing.Size(920, 60);
            this.alertPanel.TabIndex = 1;
            // 
            // alertLabel
            // 
            this.alertLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alertLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.alertLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            this.alertLabel.Location = new System.Drawing.Point(15, 15);
            this.alertLabel.Name = "alertLabel";
            this.alertLabel.Size = new System.Drawing.Size(888, 23);
            this.alertLabel.TabIndex = 0;
            this.alertLabel.Text = "Lưu ý: Thực hiện sao lưu định kỳ để tránh mất mát dữ liệu. Dữ liệu sẽ được xuất r" +
    "a file Excel hoặc PDF.";
            this.alertLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // backupSection
            // 
            this.backupSection.AutoSize = true;
            this.backupSection.Controls.Add(this.excelCard);
            this.backupSection.Controls.Add(this.pdfCard);
            this.backupSection.Controls.Add(this.restoreCard);
            this.backupSection.Dock = System.Windows.Forms.DockStyle.Top;
            this.backupSection.Location = new System.Drawing.Point(10, 130);
            this.backupSection.Name = "backupSection";
            this.backupSection.Padding = new System.Windows.Forms.Padding(0, 0, 0, 20);
            this.backupSection.Size = new System.Drawing.Size(920, 260);
            this.backupSection.TabIndex = 2;
            // 
            // excelCard
            // 
            this.excelCard.BackColor = System.Drawing.Color.White;
            this.excelCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.excelCard.Controls.Add(this.excelIconLabel);
            this.excelCard.Controls.Add(this.excelTitleLabel);
            this.excelCard.Controls.Add(this.excelDescLabel);
            this.excelCard.Controls.Add(this.btnBackupExcel);
            this.excelCard.Location = new System.Drawing.Point(10, 10);
            this.excelCard.Margin = new System.Windows.Forms.Padding(10);
            this.excelCard.Name = "excelCard";
            this.excelCard.Padding = new System.Windows.Forms.Padding(20);
            this.excelCard.Size = new System.Drawing.Size(280, 220);
            this.excelCard.TabIndex = 0;
            // 
            // excelIconLabel
            // 
            this.excelIconLabel.Font = new System.Drawing.Font("Segoe UI Emoji", 36F);
            this.excelIconLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(115)))), ((int)(((byte)(70)))));
            this.excelIconLabel.Location = new System.Drawing.Point(20, 20);
            this.excelIconLabel.Name = "excelIconLabel";
            this.excelIconLabel.Size = new System.Drawing.Size(240, 60);
            this.excelIconLabel.TabIndex = 0;
            this.excelIconLabel.Text = "📊";
            this.excelIconLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // excelTitleLabel
            // 
            this.excelTitleLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.excelTitleLabel.Location = new System.Drawing.Point(20, 90);
            this.excelTitleLabel.Name = "excelTitleLabel";
            this.excelTitleLabel.Size = new System.Drawing.Size(240, 30);
            this.excelTitleLabel.TabIndex = 1;
            this.excelTitleLabel.Text = "Sao lưu Excel";
            this.excelTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // excelDescLabel
            // 
            this.excelDescLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.excelDescLabel.Location = new System.Drawing.Point(20, 125);
            this.excelDescLabel.Name = "excelDescLabel";
            this.excelDescLabel.Size = new System.Drawing.Size(240, 30);
            this.excelDescLabel.TabIndex = 2;
            this.excelDescLabel.Text = "Xuất toàn bộ dữ liệu ra file Excel";
            this.excelDescLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnBackupExcel
            // 
            this.btnBackupExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(115)))), ((int)(((byte)(70)))));
            this.btnBackupExcel.FlatAppearance.BorderSize = 0;
            this.btnBackupExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackupExcel.ForeColor = System.Drawing.Color.White;
            this.btnBackupExcel.Location = new System.Drawing.Point(80, 165);
            this.btnBackupExcel.Name = "btnBackupExcel";
            this.btnBackupExcel.Size = new System.Drawing.Size(120, 35);
            this.btnBackupExcel.TabIndex = 3;
            this.btnBackupExcel.Text = "Sao lưu ngay";
            this.btnBackupExcel.UseVisualStyleBackColor = false;
            // 
            // pdfCard
            // 
            this.pdfCard.BackColor = System.Drawing.Color.White;
            this.pdfCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pdfCard.Controls.Add(this.pdfIconLabel);
            this.pdfCard.Controls.Add(this.pdfTitleLabel);
            this.pdfCard.Controls.Add(this.pdfDescLabel);
            this.pdfCard.Controls.Add(this.btnBackupPDF);
            this.pdfCard.Location = new System.Drawing.Point(310, 10);
            this.pdfCard.Margin = new System.Windows.Forms.Padding(10);
            this.pdfCard.Name = "pdfCard";
            this.pdfCard.Padding = new System.Windows.Forms.Padding(20);
            this.pdfCard.Size = new System.Drawing.Size(280, 220);
            this.pdfCard.TabIndex = 1;
            // 
            // pdfIconLabel
            // 
            this.pdfIconLabel.Font = new System.Drawing.Font("Segoe UI Emoji", 36F);
            this.pdfIconLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(15)))), ((int)(((byte)(2)))));
            this.pdfIconLabel.Location = new System.Drawing.Point(20, 20);
            this.pdfIconLabel.Name = "pdfIconLabel";
            this.pdfIconLabel.Size = new System.Drawing.Size(240, 60);
            this.pdfIconLabel.TabIndex = 0;
            this.pdfIconLabel.Text = "📄";
            this.pdfIconLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pdfTitleLabel
            // 
            this.pdfTitleLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.pdfTitleLabel.Location = new System.Drawing.Point(20, 90);
            this.pdfTitleLabel.Name = "pdfTitleLabel";
            this.pdfTitleLabel.Size = new System.Drawing.Size(240, 30);
            this.pdfTitleLabel.TabIndex = 1;
            this.pdfTitleLabel.Text = "Sao lưu PDF";
            this.pdfTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pdfDescLabel
            // 
            this.pdfDescLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pdfDescLabel.Location = new System.Drawing.Point(20, 125);
            this.pdfDescLabel.Name = "pdfDescLabel";
            this.pdfDescLabel.Size = new System.Drawing.Size(240, 30);
            this.pdfDescLabel.TabIndex = 2;
            this.pdfDescLabel.Text = "Xuất báo cáo tổng quan ra file PDF";
            this.pdfDescLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnBackupPDF
            // 
            this.btnBackupPDF.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(15)))), ((int)(((byte)(2)))));
            this.btnBackupPDF.FlatAppearance.BorderSize = 0;
            this.btnBackupPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackupPDF.ForeColor = System.Drawing.Color.White;
            this.btnBackupPDF.Location = new System.Drawing.Point(80, 165);
            this.btnBackupPDF.Name = "btnBackupPDF";
            this.btnBackupPDF.Size = new System.Drawing.Size(120, 35);
            this.btnBackupPDF.TabIndex = 3;
            this.btnBackupPDF.Text = "Sao lưu ngay";
            this.btnBackupPDF.UseVisualStyleBackColor = false;
            // 
            // restoreCard
            // 
            this.restoreCard.BackColor = System.Drawing.Color.White;
            this.restoreCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.restoreCard.Controls.Add(this.restoreIconLabel);
            this.restoreCard.Controls.Add(this.restoreTitleLabel);
            this.restoreCard.Controls.Add(this.restoreDescLabel);
            this.restoreCard.Controls.Add(this.filePanel);
            this.restoreCard.Controls.Add(this.btnRestore);
            this.restoreCard.Location = new System.Drawing.Point(610, 10);
            this.restoreCard.Margin = new System.Windows.Forms.Padding(10);
            this.restoreCard.Name = "restoreCard";
            this.restoreCard.Padding = new System.Windows.Forms.Padding(20);
            this.restoreCard.Size = new System.Drawing.Size(280, 220);
            this.restoreCard.TabIndex = 2;
            // 
            // restoreIconLabel
            // 
            this.restoreIconLabel.Font = new System.Drawing.Font("Segoe UI Emoji", 36F);
            this.restoreIconLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.restoreIconLabel.Location = new System.Drawing.Point(20, 20);
            this.restoreIconLabel.Name = "restoreIconLabel";
            this.restoreIconLabel.Size = new System.Drawing.Size(240, 60);
            this.restoreIconLabel.TabIndex = 0;
            this.restoreIconLabel.Text = "💾";
            this.restoreIconLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // restoreTitleLabel
            // 
            this.restoreTitleLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.restoreTitleLabel.Location = new System.Drawing.Point(20, 90);
            this.restoreTitleLabel.Name = "restoreTitleLabel";
            this.restoreTitleLabel.Size = new System.Drawing.Size(240, 30);
            this.restoreTitleLabel.TabIndex = 1;
            this.restoreTitleLabel.Text = "Phục hồi dữ liệu";
            this.restoreTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // restoreDescLabel
            // 
            this.restoreDescLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.restoreDescLabel.Location = new System.Drawing.Point(20, 125);
            this.restoreDescLabel.Name = "restoreDescLabel";
            this.restoreDescLabel.Size = new System.Drawing.Size(240, 20);
            this.restoreDescLabel.TabIndex = 2;
            this.restoreDescLabel.Text = "Khôi phục dữ liệu từ file sao lưu";
            this.restoreDescLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // filePanel
            // 
            this.filePanel.Controls.Add(this.txtRestoreFile);
            this.filePanel.Controls.Add(this.browseButton);
            this.filePanel.Location = new System.Drawing.Point(20, 150);
            this.filePanel.Name = "filePanel";
            this.filePanel.Size = new System.Drawing.Size(240, 30);
            this.filePanel.TabIndex = 3;
            // 
            // txtRestoreFile
            // 
            this.txtRestoreFile.Location = new System.Drawing.Point(0, 0);
            this.txtRestoreFile.Name = "txtRestoreFile";
            this.txtRestoreFile.ReadOnly = true;
            this.txtRestoreFile.Size = new System.Drawing.Size(195, 27);
            this.txtRestoreFile.TabIndex = 0;
            // 
            // browseButton
            // 
            this.browseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.browseButton.FlatAppearance.BorderSize = 0;
            this.browseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.browseButton.Location = new System.Drawing.Point(200, 0);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(40, 25);
            this.browseButton.TabIndex = 1;
            this.browseButton.Text = "...";
            this.browseButton.UseVisualStyleBackColor = false;
            // 
            // btnRestore
            // 
            this.btnRestore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnRestore.FlatAppearance.BorderSize = 0;
            this.btnRestore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestore.ForeColor = System.Drawing.Color.White;
            this.btnRestore.Location = new System.Drawing.Point(90, 185);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(100, 35);
            this.btnRestore.TabIndex = 4;
            this.btnRestore.Text = "Phục hồi";
            this.btnRestore.UseVisualStyleBackColor = false;
            // 
            // BackupControl
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.backupSection);
            this.Controls.Add(this.alertPanel);
            this.Controls.Add(this.headerPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "BackupControl";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(940, 640);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.alertPanel.ResumeLayout(false);
            this.backupSection.ResumeLayout(false);
            this.excelCard.ResumeLayout(false);
            this.pdfCard.ResumeLayout(false);
            this.restoreCard.ResumeLayout(false);
            this.filePanel.ResumeLayout(false);
            this.filePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Label titleLabel;
        private Label alertLabel;
        private Panel excelCard;
        private Label excelIconLabel;
        private Label excelTitleLabel;
        private Label excelDescLabel;
        private Panel pdfCard;
        private Label pdfIconLabel;
        private Label pdfTitleLabel;
        private Label pdfDescLabel;
        private Panel restoreCard;
        private Label restoreIconLabel;
        private Label restoreTitleLabel;
        private Label restoreDescLabel;
        private Panel filePanel;
    }
}