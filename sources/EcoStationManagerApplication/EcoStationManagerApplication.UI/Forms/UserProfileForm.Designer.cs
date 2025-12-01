using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace EcoStationManagerApplication.UI.Forms
{
    partial class UserProfileForm
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

        private void InitializeComponent()
        {
            this.panelMain = new Guna2Panel();
            this.lblTitle = new Label();
            this.panelInfo = new Guna2Panel();
            this.lblUsername = new Label();
            this.txtUsername = new Guna2TextBox();
            this.lblFullname = new Label();
            this.txtFullname = new Guna2TextBox();
            this.lblRole = new Label();
            this.txtRole = new Guna2TextBox();
            this.lblRoleDesc = new Label();
            this.txtRoleDesc = new Guna2TextBox();
            this.lblStatus = new Label();
            this.txtStatus = new Guna2TextBox();
            this.lblCreated = new Label();
            this.txtCreated = new Guna2TextBox();
            this.btnClose = new Guna2Button();
            this.pictureAvatar = new Guna2CirclePictureBox();
            this.panelMain.SuspendLayout();
            this.panelInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureAvatar)).BeginInit();
            this.SuspendLayout();

            this.panelMain.Dock = DockStyle.Fill;
            this.panelMain.Padding = new Padding(20);
            this.panelMain.Controls.Add(this.btnClose);
            this.panelMain.Controls.Add(this.panelInfo);
            this.panelMain.Controls.Add(this.pictureAvatar);
            this.panelMain.Controls.Add(this.lblTitle);

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Text = "Thông tin người dùng";

            this.pictureAvatar.Location = new System.Drawing.Point(24, 60);
            this.pictureAvatar.Size = new System.Drawing.Size(72, 72);
            this.pictureAvatar.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.pictureAvatar.SizeMode = PictureBoxSizeMode.Zoom;

            this.panelInfo.Location = new System.Drawing.Point(112, 60);
            this.panelInfo.Size = new System.Drawing.Size(560, 260);
            this.panelInfo.Padding = new Padding(10);

            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblUsername.Location = new System.Drawing.Point(10, 10);
            this.lblUsername.Text = "Tên đăng nhập";

            this.txtUsername.Location = new System.Drawing.Point(10, 34);
            this.txtUsername.Size = new System.Drawing.Size(250, 36);
            this.txtUsername.ReadOnly = true;

            this.lblFullname.AutoSize = true;
            this.lblFullname.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblFullname.Location = new System.Drawing.Point(300, 10);
            this.lblFullname.Text = "Họ và tên";

            this.txtFullname.Location = new System.Drawing.Point(300, 34);
            this.txtFullname.Size = new System.Drawing.Size(250, 36);
            this.txtFullname.ReadOnly = true;

            this.lblRole.AutoSize = true;
            this.lblRole.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblRole.Location = new System.Drawing.Point(10, 85);
            this.lblRole.Text = "Vai trò";

            this.txtRole.Location = new System.Drawing.Point(10, 109);
            this.txtRole.Size = new System.Drawing.Size(250, 36);
            this.txtRole.ReadOnly = true;

            this.lblRoleDesc.AutoSize = true;
            this.lblRoleDesc.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblRoleDesc.Location = new System.Drawing.Point(300, 85);
            this.lblRoleDesc.Text = "Mô tả vai trò";

            this.txtRoleDesc.Location = new System.Drawing.Point(300, 109);
            this.txtRoleDesc.Size = new System.Drawing.Size(250, 36);
            this.txtRoleDesc.ReadOnly = true;

            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblStatus.Location = new System.Drawing.Point(10, 160);
            this.lblStatus.Text = "Trạng thái";

            this.txtStatus.Location = new System.Drawing.Point(10, 184);
            this.txtStatus.Size = new System.Drawing.Size(250, 36);
            this.txtStatus.ReadOnly = true;

            this.lblCreated.AutoSize = true;
            this.lblCreated.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCreated.Location = new System.Drawing.Point(300, 160);
            this.lblCreated.Text = "Ngày tạo";

            this.txtCreated.Location = new System.Drawing.Point(300, 184);
            this.txtCreated.Size = new System.Drawing.Size(250, 36);
            this.txtCreated.ReadOnly = true;

            this.btnClose.Text = "Đóng";
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.FillColor = System.Drawing.Color.FromArgb(46, 125, 50);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Size = new System.Drawing.Size(90, 36);
            this.btnClose.Location = new System.Drawing.Point(582, 330);
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            this.panelInfo.Controls.Add(this.lblUsername);
            this.panelInfo.Controls.Add(this.txtUsername);
            this.panelInfo.Controls.Add(this.lblFullname);
            this.panelInfo.Controls.Add(this.txtFullname);
            this.panelInfo.Controls.Add(this.lblRole);
            this.panelInfo.Controls.Add(this.txtRole);
            this.panelInfo.Controls.Add(this.lblRoleDesc);
            this.panelInfo.Controls.Add(this.txtRoleDesc);
            this.panelInfo.Controls.Add(this.lblStatus);
            this.panelInfo.Controls.Add(this.txtStatus);
            this.panelInfo.Controls.Add(this.lblCreated);
            this.panelInfo.Controls.Add(this.txtCreated);

            this.ClientSize = new System.Drawing.Size(700, 380);
            this.Controls.Add(this.panelMain);
            this.Text = "Thông tin người dùng";

            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureAvatar)).EndInit();
            this.ResumeLayout(false);
        }

        private Guna2Panel panelMain;
        private Label lblTitle;
        private Guna2CirclePictureBox pictureAvatar;
        private Guna2Panel panelInfo;
        private Label lblUsername;
        private Guna2TextBox txtUsername;
        private Label lblFullname;
        private Guna2TextBox txtFullname;
        private Label lblRole;
        private Guna2TextBox txtRole;
        private Label lblRoleDesc;
        private Guna2TextBox txtRoleDesc;
        private Label lblStatus;
        private Guna2TextBox txtStatus;
        private Label lblCreated;
        private Guna2TextBox txtCreated;
        private Guna2Button btnClose;
    }
}