namespace Setup
{
    partial class Setup
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblInstallPath = new System.Windows.Forms.Label();
            this.txtInstallPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.grpDb = new System.Windows.Forms.GroupBox();
            this.lblServer = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.btnInstall = new System.Windows.Forms.Button();
            this.btnUninstall = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.grpDb.SuspendLayout();
            this.SuspendLayout();
            this.lblInstallPath.AutoSize = true;
            this.lblInstallPath.Location = new System.Drawing.Point(12, 15);
            this.lblInstallPath.Name = "lblInstallPath";
            this.lblInstallPath.Size = new System.Drawing.Size(88, 13);
            this.lblInstallPath.TabIndex = 0;
            this.lblInstallPath.Text = "Thư mục cài đặt";
            this.txtInstallPath.Location = new System.Drawing.Point(15, 31);
            this.txtInstallPath.Name = "txtInstallPath";
            this.txtInstallPath.Size = new System.Drawing.Size(520, 20);
            this.txtInstallPath.TabIndex = 1;
            this.btnBrowse.Location = new System.Drawing.Point(541, 29);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Chọn...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            this.grpDb.Controls.Add(this.lblServer);
            this.grpDb.Controls.Add(this.txtServer);
            this.grpDb.Controls.Add(this.lblDatabase);
            this.grpDb.Controls.Add(this.txtDatabase);
            this.grpDb.Controls.Add(this.lblUser);
            this.grpDb.Controls.Add(this.txtUser);
            this.grpDb.Controls.Add(this.lblPassword);
            this.grpDb.Controls.Add(this.txtPassword);
            this.grpDb.Controls.Add(this.lblPort);
            this.grpDb.Controls.Add(this.txtPort);
            this.grpDb.Location = new System.Drawing.Point(15, 67);
            this.grpDb.Name = "grpDb";
            this.grpDb.Size = new System.Drawing.Size(601, 145);
            this.grpDb.TabIndex = 3;
            this.grpDb.TabStop = false;
            this.grpDb.Text = "Cấu hình Database";
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(10, 25);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(38, 13);
            this.lblServer.TabIndex = 0;
            this.lblServer.Text = "Server";
            this.txtServer.Location = new System.Drawing.Point(90, 22);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(240, 20);
            this.txtServer.TabIndex = 1;
            this.lblDatabase.AutoSize = true;
            this.lblDatabase.Location = new System.Drawing.Point(10, 55);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(53, 13);
            this.lblDatabase.TabIndex = 2;
            this.lblDatabase.Text = "Database";
            this.txtDatabase.Location = new System.Drawing.Point(90, 52);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(240, 20);
            this.txtDatabase.TabIndex = 3;
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(10, 85);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(29, 13);
            this.lblUser.TabIndex = 4;
            this.lblUser.Text = "User";
            this.txtUser.Location = new System.Drawing.Point(90, 82);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(240, 20);
            this.txtUser.TabIndex = 5;
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(350, 25);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(52, 13);
            this.lblPassword.TabIndex = 6;
            this.lblPassword.Text = "Password";
            this.txtPassword.Location = new System.Drawing.Point(420, 22);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(165, 20);
            this.txtPassword.TabIndex = 7;
            this.txtPassword.UseSystemPasswordChar = true;
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(350, 55);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(26, 13);
            this.lblPort.TabIndex = 8;
            this.lblPort.Text = "Port";
            this.txtPort.Location = new System.Drawing.Point(420, 52);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(165, 20);
            this.txtPort.TabIndex = 9;
            this.btnInstall.Location = new System.Drawing.Point(15, 228);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(120, 30);
            this.btnInstall.TabIndex = 4;
            this.btnInstall.Text = "Cài đặt";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            this.btnUninstall.Location = new System.Drawing.Point(141, 228);
            this.btnUninstall.Name = "btnUninstall";
            this.btnUninstall.Size = new System.Drawing.Size(120, 30);
            this.btnUninstall.TabIndex = 5;
            this.btnUninstall.Text = "Gỡ cài đặt";
            this.btnUninstall.UseVisualStyleBackColor = true;
            this.btnUninstall.Click += new System.EventHandler(this.btnUninstall_Click);
            this.progressBar.Location = new System.Drawing.Point(15, 269);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(601, 18);
            this.progressBar.TabIndex = 6;
            this.txtLog.Location = new System.Drawing.Point(15, 293);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(601, 140);
            this.txtLog.TabIndex = 7;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 445);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnUninstall);
            this.Controls.Add(this.btnInstall);
            this.Controls.Add(this.grpDb);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtInstallPath);
            this.Controls.Add(this.lblInstallPath);
            this.Name = "Form1";
            this.Text = "EcoStation Setup";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grpDb.ResumeLayout(false);
            this.grpDb.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label lblInstallPath;
        private System.Windows.Forms.TextBox txtInstallPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.GroupBox grpDb;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.TextBox txtDatabase;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.Button btnUninstall;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox txtLog;
    }
}

