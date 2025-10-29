namespace EcoStationManagerApplication.UI.Controls
{
    partial class ReportsControl
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
            this.guna2PanelContent = new Guna.UI2.WinForms.Guna2Panel();
            this.flowLayoutPanelReports = new System.Windows.Forms.FlowLayoutPanel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblLastUpdate = new System.Windows.Forms.Label();
            this.btnRefresh = new Guna.UI2.WinForms.Guna2Button();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.guna2PanelContent.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2PanelContent
            // 
            this.guna2PanelContent.AutoScroll = true;
            this.guna2PanelContent.BackColor = System.Drawing.Color.White;
            this.guna2PanelContent.Controls.Add(this.flowLayoutPanelReports);
            this.guna2PanelContent.Controls.Add(this.panelHeader);
            this.guna2PanelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2PanelContent.Location = new System.Drawing.Point(0, 0);
            this.guna2PanelContent.Name = "guna2PanelContent";
            this.guna2PanelContent.Padding = new System.Windows.Forms.Padding(20);
            this.guna2PanelContent.Size = new System.Drawing.Size(1200, 700);
            this.guna2PanelContent.TabIndex = 0;
            // 
            // flowLayoutPanelReports
            // 
            this.flowLayoutPanelReports.AutoScroll = true;
            this.flowLayoutPanelReports.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanelReports.Location = new System.Drawing.Point(20, 115);
            this.flowLayoutPanelReports.Name = "flowLayoutPanelReports";
            this.flowLayoutPanelReports.Padding = new System.Windows.Forms.Padding(10);
            this.flowLayoutPanelReports.Size = new System.Drawing.Size(1160, 565);
            this.flowLayoutPanelReports.TabIndex = 1;
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.White;
            this.panelHeader.Controls.Add(this.lblLastUpdate);
            this.panelHeader.Controls.Add(this.btnRefresh);
            this.panelHeader.Controls.Add(this.lblDescription);
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(20, 20);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1160, 95);
            this.panelHeader.TabIndex = 0;
            // 
            // lblLastUpdate
            // 
            this.lblLastUpdate.AutoSize = true;
            this.lblLastUpdate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblLastUpdate.ForeColor = System.Drawing.Color.Gray;
            this.lblLastUpdate.Location = new System.Drawing.Point(22, 70);
            this.lblLastUpdate.Name = "lblLastUpdate";
            this.lblLastUpdate.Size = new System.Drawing.Size(139, 20);
            this.lblLastUpdate.TabIndex = 3;
            this.lblLastUpdate.Text = "Cập nhật lần cuối: ...";
            // 
            // btnRefresh
            // 
            this.btnRefresh.BorderRadius = 10;
            this.btnRefresh.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnRefresh.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnRefresh.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnRefresh.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnRefresh.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(1030, 30);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(120, 35);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "🔄 Làm mới";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDescription.ForeColor = System.Drawing.Color.Gray;
            this.lblDescription.Location = new System.Drawing.Point(22, 50);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(386, 20);
            this.lblDescription.TabIndex = 1;
            this.lblDescription.Text = "Các báo cáo tổng quan và chi tiết về hoạt động hệ thống";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(118, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Báo cáo";
            // 
            // ReportsControl
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.guna2PanelContent);
            this.Name = "ReportsControl";
            this.Size = new System.Drawing.Size(1200, 700);
            this.guna2PanelContent.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2PanelContent;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelReports;
        private Guna.UI2.WinForms.Guna2Button btnRefresh;
        private System.Windows.Forms.Label lblLastUpdate;
    }
}