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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.guna2PanelRight = new Guna.UI2.WinForms.Guna2Panel();
            this.footerControl1 = new EcoStationManagerApplication.UI.Controls.FooterControl();
            this.contentControl1 = new EcoStationManagerApplication.UI.Controls.ContentControl();
            this.headerControl1 = new EcoStationManagerApplication.UI.Controls.HeaderControl();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.sidebarControl = new EcoStationManagerApplication.UI.Controls.SidebarControl();
            this.sidebarControl1 = new EcoStationManagerApplication.UI.Controls.SidebarControl();
            this.guna2PanelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // guna2PanelRight
            // 
            this.guna2PanelRight.Controls.Add(this.footerControl1);
            this.guna2PanelRight.Controls.Add(this.contentControl1);
            this.guna2PanelRight.Controls.Add(this.headerControl1);
            this.guna2PanelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2PanelRight.Location = new System.Drawing.Point(0, 0);
            this.guna2PanelRight.Name = "guna2PanelRight";
            this.guna2PanelRight.Size = new System.Drawing.Size(1482, 803);
            this.guna2PanelRight.TabIndex = 1;
            // 
            // footerControl1
            // 
            this.footerControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footerControl1.Location = new System.Drawing.Point(0, 711);
            this.footerControl1.Name = "footerControl1";
            this.footerControl1.Size = new System.Drawing.Size(1482, 92);
            this.footerControl1.TabIndex = 2;
            // 
            // contentControl1
            // 
            this.contentControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentControl1.Location = new System.Drawing.Point(0, 83);
            this.contentControl1.Name = "contentControl1";
            this.contentControl1.Size = new System.Drawing.Size(1482, 720);
            this.contentControl1.TabIndex = 1;
            // 
            // headerControl1
            // 
            this.headerControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerControl1.Location = new System.Drawing.Point(0, 0);
            this.headerControl1.Name = "headerControl1";
            this.headerControl1.Size = new System.Drawing.Size(1482, 83);
            this.headerControl1.TabIndex = 0;
            // 
            // sidebarControl
            // 
            this.sidebarControl.AutoSize = true;
            this.sidebarControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebarControl.Location = new System.Drawing.Point(0, 0);
            this.sidebarControl.Name = "sidebarControl";
            this.sidebarControl.Size = new System.Drawing.Size(0, 803);
            this.sidebarControl.TabIndex = 0;
            // 
            // sidebarControl1
            // 
            this.sidebarControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebarControl1.Location = new System.Drawing.Point(0, 0);
            this.sidebarControl1.Name = "sidebarControl1";
            this.sidebarControl1.Size = new System.Drawing.Size(285, 803);
            this.sidebarControl1.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1482, 803);
            this.Controls.Add(this.sidebarControl1);
            this.Controls.Add(this.guna2PanelRight);
            this.Controls.Add(this.sidebarControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Ecostation Manager";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.guna2PanelRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource bindingSource1;
        private Controls.SidebarControl sidebarControl;
        private Guna.UI2.WinForms.Guna2Panel guna2PanelRight;
        private Controls.HeaderControl headerControl1;
        private Controls.FooterControl footerControl1;
        private Controls.ContentControl contentControl1;
        private Controls.SidebarControl sidebarControl1;
    }
}