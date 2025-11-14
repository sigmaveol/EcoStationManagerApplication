namespace EcoStationManagerApplication.UI
{
    partial class LoginForm
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
            this.panelLeft = new Guna.UI2.WinForms.Guna2GradientPanel();
            this.labelSlogan = new System.Windows.Forms.Label();
            this.pictureBoxLogo = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.panelRight = new Guna.UI2.WinForms.Guna2Panel();
            this.labelError = new System.Windows.Forms.Label();
            this.btnExit = new Guna.UI2.WinForms.Guna2Button();
            this.btnLogin = new Guna.UI2.WinForms.Guna2Button();
            this.checkBoxRemember = new Guna.UI2.WinForms.Guna2CheckBox();
            this.txtPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtUsername = new Guna.UI2.WinForms.Guna2TextBox();
            this.labelSubtitle = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.btnTogglePassword = new Guna.UI2.WinForms.Guna2ImageButton();
            this.panelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.panelRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.labelSlogan);
            this.panelLeft.Controls.Add(this.pictureBoxLogo);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.FillColor = System.Drawing.Color.FromArgb(31, 107, 59);
            this.panelLeft.FillColor2 = System.Drawing.Color.FromArgb(241, 248, 243);
            this.panelLeft.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(400, 600);
            this.panelLeft.TabIndex = 0;
            // 
            // labelSlogan
            // 
            this.labelSlogan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSlogan.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSlogan.ForeColor = System.Drawing.Color.White;
            this.labelSlogan.Location = new System.Drawing.Point(30, 500);
            this.labelSlogan.Name = "labelSlogan";
            this.labelSlogan.Size = new System.Drawing.Size(340, 50);
            this.labelSlogan.TabIndex = 1;
            this.labelSlogan.Text = "EcoStation Manager – Quản lý toàn diện mô hình Refill";
            this.labelSlogan.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBoxLogo.Image = global::EcoStationManagerApplication.UI.Properties.Resources.logo_pm;
            this.pictureBoxLogo.ImageRotate = 0F;
            this.pictureBoxLogo.Location = new System.Drawing.Point(100, 80);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.pictureBoxLogo.Size = new System.Drawing.Size(200, 200);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxLogo.TabIndex = 0;
            this.pictureBoxLogo.TabStop = false;
            // 
            // panelRight
            // 
            this.panelRight.BackColor = System.Drawing.Color.White;
            this.panelRight.Controls.Add(this.labelError);
            this.panelRight.Controls.Add(this.btnExit);
            this.panelRight.Controls.Add(this.btnLogin);
            this.panelRight.Controls.Add(this.checkBoxShowPassword);
            this.panelRight.Controls.Add(this.checkBoxRemember);
            this.panelRight.Controls.Add(this.txtPassword);
            this.panelRight.Controls.Add(this.txtUsername);
            this.panelRight.Controls.Add(this.labelSubtitle);
            this.panelRight.Controls.Add(this.labelTitle);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(400, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Padding = new System.Windows.Forms.Padding(60);
            this.panelRight.ShadowDecoration.Enabled = true;
            this.panelRight.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.panelRight.Size = new System.Drawing.Size(600, 600);
            this.panelRight.TabIndex = 1;
            // 
            // labelError
            // 
            this.labelError.AutoSize = true;
            this.labelError.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelError.ForeColor = System.Drawing.Color.FromArgb(212, 24, 61);
            this.labelError.Location = new System.Drawing.Point(60, 420);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(0, 23);
            this.labelError.TabIndex = 8;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BorderRadius = 10;
            this.btnExit.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnExit.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnExit.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
            this.btnExit.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
            this.btnExit.FillColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(360, 520);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(120, 40);
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "Thoát";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLogin.BorderRadius = 10;
            this.btnLogin.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnLogin.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnLogin.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
            this.btnLogin.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
            this.btnLogin.FillColor = System.Drawing.Color.FromArgb(31, 107, 59);
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(60, 520);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(200, 40);
            this.btnLogin.TabIndex = 6;
            this.btnLogin.Text = "Đăng nhập";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // checkBoxShowPassword
            // 
            this.checkBoxShowPassword.AutoSize = true;
            this.checkBoxShowPassword.CheckedState.BorderColor = System.Drawing.Color.FromArgb(31, 107, 59);
            this.checkBoxShowPassword.CheckedState.BorderRadius = 2;
            this.checkBoxShowPassword.CheckedState.BorderThickness = 0;
            this.checkBoxShowPassword.CheckedState.FillColor = System.Drawing.Color.FromArgb(31, 107, 59);
            this.checkBoxShowPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.checkBoxShowPassword.Location = new System.Drawing.Point(60, 360);
            this.checkBoxShowPassword.Name = "checkBoxShowPassword";
            this.checkBoxShowPassword.Size = new System.Drawing.Size(170, 27);
            this.checkBoxShowPassword.TabIndex = 5;
            this.checkBoxShowPassword.Text = "Hiển thị mật khẩu";
            this.checkBoxShowPassword.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(125, 137, 149);
            this.checkBoxShowPassword.UncheckedState.BorderRadius = 2;
            this.checkBoxShowPassword.UncheckedState.BorderThickness = 1;
            this.checkBoxShowPassword.UncheckedState.FillColor = System.Drawing.Color.White;
            this.checkBoxShowPassword.CheckedChanged += new System.EventHandler(this.checkBoxShowPassword_CheckedChanged);
            // 
            // checkBoxRemember
            // 
            this.checkBoxRemember.AutoSize = true;
            this.checkBoxRemember.CheckedState.BorderColor = System.Drawing.Color.FromArgb(31, 107, 59);
            this.checkBoxRemember.CheckedState.BorderRadius = 2;
            this.checkBoxRemember.CheckedState.BorderThickness = 0;
            this.checkBoxRemember.CheckedState.FillColor = System.Drawing.Color.FromArgb(31, 107, 59);
            this.checkBoxRemember.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.checkBoxRemember.Location = new System.Drawing.Point(250, 360);
            this.checkBoxRemember.Name = "checkBoxRemember";
            this.checkBoxRemember.Size = new System.Drawing.Size(180, 27);
            this.checkBoxRemember.TabIndex = 6;
            this.checkBoxRemember.Text = "Ghi nhớ đăng nhập";
            this.checkBoxRemember.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(125, 137, 149);
            this.checkBoxRemember.UncheckedState.BorderRadius = 2;
            this.checkBoxRemember.UncheckedState.BorderThickness = 1;
            this.checkBoxRemember.UncheckedState.FillColor = System.Drawing.Color.White;
            // 
            // txtPassword
            // 
            this.txtPassword.BorderRadius = 10;
            this.txtPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPassword.DefaultText = "";
            this.txtPassword.DisabledState.BorderColor = System.Drawing.Color.FromArgb(208, 208, 208);
            this.txtPassword.DisabledState.FillColor = System.Drawing.Color.FromArgb(226, 226, 226);
            this.txtPassword.DisabledState.ForeColor = System.Drawing.Color.FromArgb(138, 138, 138);
            this.txtPassword.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(138, 138, 138);
            this.txtPassword.FocusedState.BorderColor = System.Drawing.Color.FromArgb(31, 107, 59);
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtPassword.ForeColor = System.Drawing.Color.Black;
            this.txtPassword.HoverState.BorderColor = System.Drawing.Color.FromArgb(31, 107, 59);
            this.txtPassword.Location = new System.Drawing.Point(60, 300);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '●';
            this.txtPassword.PlaceholderText = "Mật khẩu";
            this.txtPassword.SelectedText = "";
            this.txtPassword.Size = new System.Drawing.Size(420, 50);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);
            // 
            // txtUsername
            // 
            this.txtUsername.BorderRadius = 10;
            this.txtUsername.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtUsername.DefaultText = "";
            this.txtUsername.DisabledState.BorderColor = System.Drawing.Color.FromArgb(208, 208, 208);
            this.txtUsername.DisabledState.FillColor = System.Drawing.Color.FromArgb(226, 226, 226);
            this.txtUsername.DisabledState.ForeColor = System.Drawing.Color.FromArgb(138, 138, 138);
            this.txtUsername.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(138, 138, 138);
            this.txtUsername.FocusedState.BorderColor = System.Drawing.Color.FromArgb(31, 107, 59);
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtUsername.ForeColor = System.Drawing.Color.Black;
            this.txtUsername.HoverState.BorderColor = System.Drawing.Color.FromArgb(31, 107, 59);
            this.txtUsername.Location = new System.Drawing.Point(60, 220);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.PlaceholderText = "Tên đăng nhập";
            this.txtUsername.SelectedText = "";
            this.txtUsername.Size = new System.Drawing.Size(420, 50);
            this.txtUsername.TabIndex = 3;
            this.txtUsername.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUsername_KeyDown);
            // 
            // labelSubtitle
            // 
            this.labelSubtitle.AutoSize = true;
            this.labelSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSubtitle.ForeColor = System.Drawing.Color.FromArgb(85, 85, 85);
            this.labelSubtitle.Location = new System.Drawing.Point(60, 100);
            this.labelSubtitle.Name = "labelSubtitle";
            this.labelSubtitle.Size = new System.Drawing.Size(360, 23);
            this.labelSubtitle.TabIndex = 2;
            this.labelSubtitle.Text = "Vui lòng nhập thông tin tài khoản để tiếp tục.";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            this.labelTitle.Location = new System.Drawing.Point(60, 50);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(300, 46);
            this.labelTitle.TabIndex = 1;
            this.labelTitle.Text = "Đăng nhập hệ thống";
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelLeft);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng nhập - EcoStation Manager";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.panelLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.panelRight.ResumeLayout(false);
            this.panelRight.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2GradientPanel panelLeft;
        private Guna.UI2.WinForms.Guna2CirclePictureBox pictureBoxLogo;
        private System.Windows.Forms.Label labelSlogan;
        private Guna.UI2.WinForms.Guna2Panel panelRight;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelSubtitle;
        private Guna.UI2.WinForms.Guna2TextBox txtUsername;
        private Guna.UI2.WinForms.Guna2TextBox txtPassword;
        private Guna.UI2.WinForms.Guna2CheckBox checkBoxRemember;
        private Guna.UI2.WinForms.Guna2Button btnLogin;
        private Guna.UI2.WinForms.Guna2Button btnExit;
        private System.Windows.Forms.Label labelError;
        private Guna.UI2.WinForms.Guna2CheckBox checkBoxShowPassword;
    }
}
