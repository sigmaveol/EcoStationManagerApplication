using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    partial class DashboardControl
    {
        private System.ComponentModel.IContainer components = null;
        private TableLayoutPanel ordersTable; 

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
            this.ordersTable = new System.Windows.Forms.TableLayoutPanel();
            this.panelContent = new System.Windows.Forms.Panel();
            this.recentOrdersPanel = new System.Windows.Forms.Panel();
            this.dgvRecentOrders = new System.Windows.Forms.DataGridView();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.btnViewAllOrders = new System.Windows.Forms.Button();
            this.borderPanelOrders = new System.Windows.Forms.Panel();
            this.statsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.alertPanel = new System.Windows.Forms.Panel();
            this.alertLabel = new System.Windows.Forms.Label();
            this.borderPanel = new System.Windows.Forms.Panel();
            this.panelContent.SuspendLayout();
            this.recentOrdersPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentOrders)).BeginInit();
            this.headerPanel.SuspendLayout();
            this.alertPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ordersTable
            // 
            this.ordersTable.Location = new System.Drawing.Point(0, 0);
            this.ordersTable.Name = "ordersTable";
            this.ordersTable.Size = new System.Drawing.Size(200, 100);
            this.ordersTable.TabIndex = 0;
            // 
            // panelContent
            // 
            this.panelContent.AutoScroll = true;
            this.panelContent.Controls.Add(this.recentOrdersPanel);
            this.panelContent.Controls.Add(this.statsPanel);
            this.panelContent.Controls.Add(this.alertPanel);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 0);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(15, 20, 20, 15);
            this.panelContent.Size = new System.Drawing.Size(940, 640);
            this.panelContent.TabIndex = 3;
            // 
            // recentOrdersPanel
            // 
            this.recentOrdersPanel.BackColor = System.Drawing.Color.White;
            this.recentOrdersPanel.Controls.Add(this.dgvRecentOrders);
            this.recentOrdersPanel.Controls.Add(this.headerPanel);
            this.recentOrdersPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.recentOrdersPanel.Location = new System.Drawing.Point(15, 110);
            this.recentOrdersPanel.MinimumSize = new System.Drawing.Size(200, 200);
            this.recentOrdersPanel.Name = "recentOrdersPanel";
            this.recentOrdersPanel.Padding = new System.Windows.Forms.Padding(15);
            this.recentOrdersPanel.Size = new System.Drawing.Size(905, 515);
            this.recentOrdersPanel.TabIndex = 2;
            // 
            // dgvRecentOrders
            // 
            this.dgvRecentOrders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRecentOrders.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvRecentOrders.ColumnHeadersHeight = 29;
            this.dgvRecentOrders.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvRecentOrders.Location = new System.Drawing.Point(15, 55);
            this.dgvRecentOrders.Name = "dgvRecentOrders";
            this.dgvRecentOrders.ReadOnly = true;
            this.dgvRecentOrders.RowHeadersWidth = 51;
            this.dgvRecentOrders.Size = new System.Drawing.Size(875, 445);
            this.dgvRecentOrders.TabIndex = 0;
            // 
            // headerPanel
            // 
            this.headerPanel.Controls.Add(this.titleLabel);
            this.headerPanel.Controls.Add(this.btnViewAllOrders);
            this.headerPanel.Controls.Add(this.borderPanelOrders);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(15, 15);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(875, 40);
            this.headerPanel.TabIndex = 1;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.titleLabel.Location = new System.Drawing.Point(0, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(224, 32);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Đơn hàng gần đây";
            // 
            // btnViewAllOrders
            // 
            this.btnViewAllOrders.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnViewAllOrders.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnViewAllOrders.FlatAppearance.BorderSize = 0;
            this.btnViewAllOrders.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewAllOrders.Location = new System.Drawing.Point(775, 0);
            this.btnViewAllOrders.Name = "btnViewAllOrders";
            this.btnViewAllOrders.Size = new System.Drawing.Size(100, 39);
            this.btnViewAllOrders.TabIndex = 1;
            this.btnViewAllOrders.Text = "Xem tất cả";
            this.btnViewAllOrders.UseVisualStyleBackColor = false;
            this.btnViewAllOrders.Click += new System.EventHandler(this.btnViewAllOrders_Click);
            // 
            // borderPanelOrders
            // 
            this.borderPanelOrders.BackColor = System.Drawing.Color.LightGray;
            this.borderPanelOrders.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.borderPanelOrders.Location = new System.Drawing.Point(0, 39);
            this.borderPanelOrders.Name = "borderPanelOrders";
            this.borderPanelOrders.Size = new System.Drawing.Size(875, 1);
            this.borderPanelOrders.TabIndex = 2;
            // 
            // statsPanel
            // 
            this.statsPanel.AutoSize = true;
            this.statsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.statsPanel.Location = new System.Drawing.Point(15, 80);
            this.statsPanel.Name = "statsPanel";
            this.statsPanel.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.statsPanel.Size = new System.Drawing.Size(905, 30);
            this.statsPanel.TabIndex = 1;
            // 
            // alertPanel
            // 
            this.alertPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(242)))), ((int)(((byte)(253)))));
            this.alertPanel.Controls.Add(this.alertLabel);
            this.alertPanel.Controls.Add(this.borderPanel);
            this.alertPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.alertPanel.Location = new System.Drawing.Point(15, 20);
            this.alertPanel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 70);
            this.alertPanel.Name = "alertPanel";
            this.alertPanel.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.alertPanel.Size = new System.Drawing.Size(905, 60);
            this.alertPanel.TabIndex = 0;
            // 
            // alertLabel
            // 
            this.alertLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alertLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.alertLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            this.alertLabel.Location = new System.Drawing.Point(19, 0);
            this.alertLabel.Name = "alertLabel";
            this.alertLabel.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.alertLabel.Size = new System.Drawing.Size(886, 60);
            this.alertLabel.TabIndex = 0;
            this.alertLabel.Text = "Chế độ Offline: Hệ thống đang hoạt động nội bộ. Kết nối internet để đồng bộ dữ li" +
    "ệu với các công cụ online.";
            this.alertLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // borderPanel
            // 
            this.borderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            this.borderPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.borderPanel.Location = new System.Drawing.Point(15, 0);
            this.borderPanel.Name = "borderPanel";
            this.borderPanel.Size = new System.Drawing.Size(4, 60);
            this.borderPanel.TabIndex = 1;
            // 
            // DashboardControl
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.panelContent);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DashboardControl";
            this.Size = new System.Drawing.Size(940, 640);
            this.Load += new System.EventHandler(this.DashboardControl_Load);
            this.panelContent.ResumeLayout(false);
            this.panelContent.PerformLayout();
            this.recentOrdersPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentOrders)).EndInit();
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.alertPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        private Panel panelContent;
        private Panel recentOrdersPanel;
        private DataGridView dgvRecentOrders;
        private Panel headerPanel;
        private Label titleLabel;
        private Button btnViewAllOrders;
        private Panel borderPanelOrders;
        private FlowLayoutPanel statsPanel;
        private Panel alertPanel;
        private Label alertLabel;
        private Panel borderPanel;
    }
}