using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    partial class IntegrationsControl
    {
        private System.ComponentModel.IContainer components = null;

        private Panel headerPanel;
        private Panel alertPanel;
        private FlowLayoutPanel integrationsPanel;

        private Button btnUpdateGoogleForms;
        private Button btnConnectGoogleSheets;

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
            this.titleLabelHeader = new System.Windows.Forms.Label();
            this.alertLabel = new System.Windows.Forms.Label();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.alertPanel = new System.Windows.Forms.Panel();
            this.integrationsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.headerPanel.SuspendLayout();
            this.alertPanel.SuspendLayout();
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
            this.alertLabel.Size = new System.Drawing.Size(870, 30);
            this.alertLabel.TabIndex = 0;
            this.alertLabel.Text = "Lưu ý: Các tích hợp này yêu cầu kết nối internet để hoạt động. Dữ liệu sẽ được đồ" +
    "ng bộ về hệ thống offline.";
            this.alertLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // headerPanel
            // 
            this.headerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.headerPanel.Controls.Add(this.titleLabelHeader);
            this.headerPanel.Location = new System.Drawing.Point(20, 20);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(900, 60);
            this.headerPanel.TabIndex = 0;
            // 
            // alertPanel
            // 
            this.alertPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.alertPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(242)))), ((int)(((byte)(253)))));
            this.alertPanel.Controls.Add(this.alertLabel);
            this.alertPanel.Location = new System.Drawing.Point(20, 90);
            this.alertPanel.Name = "alertPanel";
            this.alertPanel.Padding = new System.Windows.Forms.Padding(15);
            this.alertPanel.Size = new System.Drawing.Size(900, 60);
            this.alertPanel.TabIndex = 1;
            // 
            // integrationsPanel
            // 
            this.integrationsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.integrationsPanel.Location = new System.Drawing.Point(20, 160);
            this.integrationsPanel.Name = "integrationsPanel";
            this.integrationsPanel.Size = new System.Drawing.Size(900, 450);
            this.integrationsPanel.TabIndex = 2;
            // 
            // IntegrationsControl
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.headerPanel);
            this.Controls.Add(this.alertPanel);
            this.Controls.Add(this.integrationsPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "IntegrationsControl";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(940, 640);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.alertPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private Label titleLabelHeader;
        private Label alertLabel;
    }
}