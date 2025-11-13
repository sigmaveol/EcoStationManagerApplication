namespace EcoStationManagerApplication.UI.Forms
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.guna2PanelRight = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2PanelLayout = new Guna.UI2.WinForms.Guna2Panel();
            this.contentControl = new EcoStationManagerApplication.UI.Controls.ContentControl();
            this.footerControl = new EcoStationManagerApplication.UI.Controls.FooterControl();
            this.headerControl = new EcoStationManagerApplication.UI.Controls.HeaderControl();
            this.sidebarControl = new EcoStationManagerApplication.UI.Controls.SidebarControl();
            this.guna2PanelRight.SuspendLayout();
            this.guna2PanelLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2PanelRight
            // 
            this.guna2PanelRight.Controls.Add(this.contentControl);
            this.guna2PanelRight.Controls.Add(this.footerControl);
            this.guna2PanelRight.Controls.Add(this.headerControl);
            this.guna2PanelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2PanelRight.Location = new System.Drawing.Point(302, 0);
            this.guna2PanelRight.Name = "guna2PanelRight";
            this.guna2PanelRight.Size = new System.Drawing.Size(1180, 803);
            this.guna2PanelRight.TabIndex = 1;
            // 
            // guna2PanelLayout
            // 
            this.guna2PanelLayout.Controls.Add(this.guna2PanelRight);
            this.guna2PanelLayout.Controls.Add(this.sidebarControl);
            this.guna2PanelLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2PanelLayout.Location = new System.Drawing.Point(0, 0);
            this.guna2PanelLayout.Name = "guna2PanelLayout";
            this.guna2PanelLayout.Size = new System.Drawing.Size(1482, 803);
            this.guna2PanelLayout.TabIndex = 3;
            // 
            // contentControl
            // 
            this.contentControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentControl.Location = new System.Drawing.Point(0, 70);
            this.contentControl.Name = "contentControl";
            this.contentControl.Size = new System.Drawing.Size(1180, 660);
            this.contentControl.TabIndex = 2;
            // 
            // footerControl
            // 
            this.footerControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footerControl.Location = new System.Drawing.Point(0, 730);
            this.footerControl.Name = "footerControl";
            this.footerControl.Size = new System.Drawing.Size(1180, 73);
            this.footerControl.TabIndex = 1;
            // 
            // headerControl
            // 
            this.headerControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerControl.Location = new System.Drawing.Point(0, 0);
            this.headerControl.Name = "headerControl";
            this.headerControl.Size = new System.Drawing.Size(1180, 70);
            this.headerControl.TabIndex = 0;
            // 
            // sidebarControl
            // 
            this.sidebarControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebarControl.Location = new System.Drawing.Point(0, 0);
            this.sidebarControl.Name = "sidebarControl";
            this.sidebarControl.Size = new System.Drawing.Size(302, 803);
            this.sidebarControl.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1482, 803);
            this.Controls.Add(this.guna2PanelLayout);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Ecostation Manager";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.guna2PanelRight.ResumeLayout(false);
            this.guna2PanelLayout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.SidebarControl sidebarControl;
        private Guna.UI2.WinForms.Guna2Panel guna2PanelRight;
        private Controls.ContentControl contentControl;
        private Controls.FooterControl footerControl;
        private Controls.HeaderControl headerControl;
        private Guna.UI2.WinForms.Guna2Panel guna2PanelLayout;
    }
}