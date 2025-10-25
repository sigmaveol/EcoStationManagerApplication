using System.Drawing;

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
            this.sidebarControl = new EcoStationManagerApplication.UI.Controls.SidebarControl();
            this.guna2PanelRight = new Guna.UI2.WinForms.Guna2Panel();
            this.contentControl = new EcoStationManagerApplication.UI.Controls.ContentControl();
            this.headerControl = new EcoStationManagerApplication.UI.Controls.HeaderControl();
            this.footerControl = new EcoStationManagerApplication.UI.Controls.FooterControl();
            this.guna2PanelRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // sidebarControl
            // 
            this.sidebarControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebarControl.Location = new System.Drawing.Point(0, 0);
            this.sidebarControl.Name = "sidebarControl";
            this.sidebarControl.Size = new System.Drawing.Size(303, 803);
            this.sidebarControl.TabIndex = 9;
            // 
            // guna2PanelRight
            // 
            this.guna2PanelRight.Controls.Add(this.footerControl);
            this.guna2PanelRight.Controls.Add(this.contentControl);
            this.guna2PanelRight.Controls.Add(this.headerControl);
            this.guna2PanelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2PanelRight.Location = new System.Drawing.Point(303, 0);
            this.guna2PanelRight.Name = "guna2PanelRight";
            this.guna2PanelRight.Size = new System.Drawing.Size(1179, 803);
            this.guna2PanelRight.TabIndex = 0;
            // 
            // contentControl
            // 
            this.contentControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentControl.Location = new System.Drawing.Point(0, 85);
            this.contentControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.contentControl.Name = "contentControl";
            this.contentControl.Size = new System.Drawing.Size(1179, 718);
            this.contentControl.TabIndex = 10;
            // 
            // headerControl
            // 
            this.headerControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerControl.Location = new System.Drawing.Point(0, 0);
            this.headerControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.headerControl.Name = "headerControl";
            this.headerControl.Size = new System.Drawing.Size(1179, 85);
            this.headerControl.TabIndex = 9;
            // 
            // footerControl
            // 
            this.footerControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footerControl.Location = new System.Drawing.Point(0, 711);
            this.footerControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.footerControl.Name = "footerControl";
            this.footerControl.Size = new System.Drawing.Size(1179, 92);
            this.footerControl.TabIndex = 11;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1482, 803);
            this.Controls.Add(this.guna2PanelRight);
            this.Controls.Add(this.sidebarControl);
            this.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.Text = "EcoStation Manager";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.guna2PanelRight.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Controls.SidebarControl sidebarControl;
        private Guna.UI2.WinForms.Guna2Panel guna2PanelRight;
        private Controls.HeaderControl headerControl;
        private Controls.ContentControl contentControl;
        private Controls.FooterControl footerControl;
    }
}