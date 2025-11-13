using EcoStationManagerApplication.UI.Common;
using Guna.UI2.WinForms;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    partial class SidebarControl
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
            this.guna2PanelSidebar = new Guna.UI2.WinForms.Guna2GradientPanel();
            this.guna2PanelMenuSidebar = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2PanelUserSidebar = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2PanelHighLightUser = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2PanelAvatar = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2CirclePictureBoxAvatar = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.labelUsername = new System.Windows.Forms.Label();
            this.guna2PanelHeaderSidebar = new Guna.UI2.WinForms.Guna2Panel();
            this.labelAppName = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2ButtonMenuTogger = new Guna.UI2.WinForms.Guna2Button();
            this.guna2CirclePictureBoxLogoSidebar = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.guna2PanelSidebar.SuspendLayout();
            this.guna2PanelUserSidebar.SuspendLayout();
            this.guna2PanelHighLightUser.SuspendLayout();
            this.guna2PanelAvatar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBoxAvatar)).BeginInit();
            this.guna2PanelHeaderSidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBoxLogoSidebar)).BeginInit();
            this.SuspendLayout();
            // 
            // guna2PanelSidebar
            // 
            this.guna2PanelSidebar.Controls.Add(this.guna2PanelMenuSidebar);
            this.guna2PanelSidebar.Controls.Add(this.guna2PanelUserSidebar);
            this.guna2PanelSidebar.Controls.Add(this.guna2PanelHeaderSidebar);
            this.guna2PanelSidebar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2PanelSidebar.FillColor = AppColors.Primary;
            this.guna2PanelSidebar.Location = new System.Drawing.Point(0, 0);
            this.guna2PanelSidebar.Name = "guna2PanelSidebar";
            this.guna2PanelSidebar.Size = new System.Drawing.Size(303, 803);
            this.guna2PanelSidebar.TabIndex = 5;
            // 
            // guna2PanelMenuSidebar
            // 
            this.guna2PanelMenuSidebar.AutoScroll = true;
            this.guna2PanelMenuSidebar.BackColor = AppColors.Primary;
            this.guna2PanelMenuSidebar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2PanelMenuSidebar.Location = new System.Drawing.Point(0, 70);
            this.guna2PanelMenuSidebar.Name = "guna2PanelMenuSidebar";
            this.guna2PanelMenuSidebar.Size = new System.Drawing.Size(303, 659);
            this.guna2PanelMenuSidebar.TabIndex = 2;
            // 
            // guna2PanelUserSidebar
            // 
            this.guna2PanelUserSidebar.BackColor = AppColors.Primary;
            this.guna2PanelUserSidebar.Controls.Add(this.guna2PanelHighLightUser);
            this.guna2PanelUserSidebar.CustomBorderColor = System.Drawing.Color.White;
            this.guna2PanelUserSidebar.CustomBorderThickness = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.guna2PanelUserSidebar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.guna2PanelUserSidebar.Location = new System.Drawing.Point(0, 729);
            this.guna2PanelUserSidebar.Name = "guna2PanelUserSidebar";
            this.guna2PanelUserSidebar.Padding = new System.Windows.Forms.Padding(5);
            this.guna2PanelUserSidebar.Size = new System.Drawing.Size(303, 74);
            this.guna2PanelUserSidebar.TabIndex = 1;
            // 
            // guna2PanelHighLightUser
            // 
            this.guna2PanelHighLightUser.BorderRadius = 15;
            this.guna2PanelHighLightUser.Controls.Add(this.guna2PanelAvatar);
            this.guna2PanelHighLightUser.Controls.Add(this.labelUsername);
            this.guna2PanelHighLightUser.Cursor = System.Windows.Forms.Cursors.Hand;
            this.guna2PanelHighLightUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2PanelHighLightUser.Location = new System.Drawing.Point(5, 5);
            this.guna2PanelHighLightUser.Name = "guna2PanelHighLightUser";
            this.guna2PanelHighLightUser.Size = new System.Drawing.Size(293, 64);
            this.guna2PanelHighLightUser.TabIndex = 0;
            // 
            // guna2PanelAvatar
            // 
            this.guna2PanelAvatar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.guna2PanelAvatar.Controls.Add(this.guna2CirclePictureBoxAvatar);
            this.guna2PanelAvatar.Location = new System.Drawing.Point(6, 11);
            this.guna2PanelAvatar.Name = "guna2PanelAvatar";
            this.guna2PanelAvatar.Size = new System.Drawing.Size(50, 43);
            this.guna2PanelAvatar.TabIndex = 0;
            // 
            // guna2CirclePictureBoxAvatar
            // 
            this.guna2CirclePictureBoxAvatar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.guna2CirclePictureBoxAvatar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2CirclePictureBoxAvatar.Image = global::EcoStationManagerApplication.UI.Properties.Resources.logo_pm;
            this.guna2CirclePictureBoxAvatar.ImageRotate = 0F;
            this.guna2CirclePictureBoxAvatar.Location = new System.Drawing.Point(0, 0);
            this.guna2CirclePictureBoxAvatar.Name = "guna2CirclePictureBoxAvatar";
            this.guna2CirclePictureBoxAvatar.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.guna2CirclePictureBoxAvatar.Size = new System.Drawing.Size(50, 43);
            this.guna2CirclePictureBoxAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.guna2CirclePictureBoxAvatar.TabIndex = 1;
            this.guna2CirclePictureBoxAvatar.TabStop = false;
            // 
            // labelUsername
            // 
            this.labelUsername.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelUsername.AutoEllipsis = true;
            this.labelUsername.Font = new System.Drawing.Font("Segoe UI Black", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUsername.ForeColor = System.Drawing.Color.Transparent;
            this.labelUsername.Location = new System.Drawing.Point(71, 14);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(199, 28);
            this.labelUsername.TabIndex = 2;
            this.labelUsername.Text = "Hoàng Sinh Hùng Hùng Hùng Hùng";
            this.labelUsername.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // guna2PanelHeaderSidebar
            // 
            this.guna2PanelHeaderSidebar.BackColor = AppColors.Primary;
            this.guna2PanelHeaderSidebar.Controls.Add(this.labelAppName);
            this.guna2PanelHeaderSidebar.Controls.Add(this.guna2ButtonMenuTogger);
            this.guna2PanelHeaderSidebar.Controls.Add(this.guna2CirclePictureBoxLogoSidebar);
            this.guna2PanelHeaderSidebar.CustomBorderColor = System.Drawing.Color.White;
            this.guna2PanelHeaderSidebar.CustomBorderThickness = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.guna2PanelHeaderSidebar.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2PanelHeaderSidebar.Location = new System.Drawing.Point(0, 0);
            this.guna2PanelHeaderSidebar.Name = "guna2PanelHeaderSidebar";
            this.guna2PanelHeaderSidebar.Size = new System.Drawing.Size(303, 70);
            this.guna2PanelHeaderSidebar.TabIndex = 0;
            // 
            // labelAppName
            // 
            this.labelAppName.BackColor = System.Drawing.Color.Transparent;
            this.labelAppName.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAppName.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.labelAppName.Location = new System.Drawing.Point(72, 19);
            this.labelAppName.Name = "labelAppName";
            this.labelAppName.Size = new System.Drawing.Size(118, 33);
            this.labelAppName.TabIndex = 0;
            this.labelAppName.Text = "EcoStation";
            this.labelAppName.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // guna2ButtonMenuTogger
            // 
            this.guna2ButtonMenuTogger.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ButtonMenuTogger.BackColor = AppColors.Primary;
            this.guna2ButtonMenuTogger.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.guna2ButtonMenuTogger.BorderColor = System.Drawing.Color.Transparent;
            this.guna2ButtonMenuTogger.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2ButtonMenuTogger.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2ButtonMenuTogger.FillColor = this.guna2PanelHeaderSidebar.FillColor;
            this.guna2ButtonMenuTogger.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2ButtonMenuTogger.ForeColor = System.Drawing.Color.White;
            this.guna2ButtonMenuTogger.HoverState.FillColor = AppColors.PrimaryHover;
            this.guna2ButtonMenuTogger.Image = global::EcoStationManagerApplication.UI.Properties.Resources.menu;
            this.guna2ButtonMenuTogger.ImageSize = new System.Drawing.Size(30, 30);
            this.guna2ButtonMenuTogger.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.guna2ButtonMenuTogger.Location = new System.Drawing.Point(249, 12);
            this.guna2ButtonMenuTogger.Name = "guna2ButtonMenuTogger";
            this.guna2ButtonMenuTogger.Size = new System.Drawing.Size(40, 40);
            this.guna2ButtonMenuTogger.TabIndex = 3;
            this.guna2ButtonMenuTogger.Click += new System.EventHandler(this.guna2ButtonMenuTogger_Click);
            // 
            // guna2CirclePictureBoxLogoSidebar
            // 
            this.guna2CirclePictureBoxLogoSidebar.Image = global::EcoStationManagerApplication.UI.Properties.Resources.logo_pm;
            this.guna2CirclePictureBoxLogoSidebar.ImageRotate = 0F;
            this.guna2CirclePictureBoxLogoSidebar.Location = new System.Drawing.Point(6, 8);
            this.guna2CirclePictureBoxLogoSidebar.Name = "guna2CirclePictureBoxLogoSidebar";
            this.guna2CirclePictureBoxLogoSidebar.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.guna2CirclePictureBoxLogoSidebar.Size = new System.Drawing.Size(50, 50);
            this.guna2CirclePictureBoxLogoSidebar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.guna2CirclePictureBoxLogoSidebar.TabIndex = 1;
            this.guna2CirclePictureBoxLogoSidebar.TabStop = false;
            // 
            // SidebarControl
            // 
            this.Controls.Add(this.guna2PanelSidebar);
            this.Name = "SidebarControl";
            this.Size = new System.Drawing.Size(303, 803);
            this.Load += new System.EventHandler(this.SidebarControl_Load);
            this.guna2PanelSidebar.ResumeLayout(false);
            this.guna2PanelUserSidebar.ResumeLayout(false);
            this.guna2PanelHighLightUser.ResumeLayout(false);
            this.guna2PanelAvatar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBoxAvatar)).EndInit();
            this.guna2PanelHeaderSidebar.ResumeLayout(false);
            this.guna2PanelHeaderSidebar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBoxLogoSidebar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna2GradientPanel guna2PanelSidebar;
        private Guna2Panel guna2PanelMenuSidebar;
        private Guna2Panel guna2PanelUserSidebar;
        private Guna2Panel guna2PanelHighLightUser;
        private Label labelUsername;
        private Guna2CirclePictureBox guna2CirclePictureBoxAvatar;
        private Guna2Panel guna2PanelHeaderSidebar;
        private Guna2CirclePictureBox guna2CirclePictureBoxLogoSidebar;
        private Guna2Panel guna2PanelAvatar;
        private Guna2HtmlLabel labelAppName;
        private Guna2Button guna2ButtonMenuTogger;
    }
}
