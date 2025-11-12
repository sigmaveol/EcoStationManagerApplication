using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    partial class DashboardControl
    {
        private System.ComponentModel.IContainer components = null;

        private Panel alertPanel;
        private FlowLayoutPanel statsPanel;
        private Panel recentOrdersPanel;
        private Button btnViewAllOrders;
        private DataGridView dgvRecentOrders;
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
            this.borderPanel = new System.Windows.Forms.Panel();
            this.alertLabel = new System.Windows.Forms.Label();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.btnViewAllOrders = new System.Windows.Forms.Button();
            this.borderPanelOrders = new System.Windows.Forms.Panel();
            this.alertPanel = new System.Windows.Forms.Panel();
            this.statsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.recentOrdersPanel = new System.Windows.Forms.Panel();
            this.dgvRecentOrders = new System.Windows.Forms.DataGridView();
            this.ordersTable = new System.Windows.Forms.TableLayoutPanel();
            this.headerPanel.SuspendLayout();
            this.alertPanel.SuspendLayout();
            this.recentOrdersPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentOrders)).BeginInit();
            this.SuspendLayout();
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
            // alertLabel
            // 
            this.alertLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alertLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.alertLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            this.alertLabel.Location = new System.Drawing.Point(19, 0);
            this.alertLabel.Name = "alertLabel";
            this.alertLabel.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.alertLabel.Size = new System.Drawing.Size(881, 60);
            this.alertLabel.TabIndex = 0;
            this.alertLabel.Text = "Chế độ Offline: Hệ thống đang hoạt động nội bộ. Kết nối internet để đồng bộ dữ li" +
    "ệu với các công cụ online.";
            this.alertLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // headerPanel
            // 
            this.headerPanel.Controls.Add(this.titleLabel);
            this.headerPanel.Controls.Add(this.btnViewAllOrders);
            this.headerPanel.Controls.Add(this.borderPanelOrders);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(15, 15);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(870, 40);
            this.headerPanel.TabIndex = 1;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.titleLabel.Location = new System.Drawing.Point(0, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(203, 30);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Đơn hàng gần đây";
            // 
            // btnViewAllOrders
            // 
            this.btnViewAllOrders.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnViewAllOrders.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnViewAllOrders.FlatAppearance.BorderSize = 0;
            this.btnViewAllOrders.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewAllOrders.Location = new System.Drawing.Point(770, 0);
            this.btnViewAllOrders.Name = "btnViewAllOrders";
            this.btnViewAllOrders.Size = new System.Drawing.Size(100, 39);
            this.btnViewAllOrders.TabIndex = 1;
            this.btnViewAllOrders.Text = "Xem tất cả";
            this.btnViewAllOrders.UseVisualStyleBackColor = false;
            // 
            // borderPanelOrders
            // 
            this.borderPanelOrders.BackColor = System.Drawing.Color.LightGray;
            this.borderPanelOrders.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.borderPanelOrders.Location = new System.Drawing.Point(0, 39);
            this.borderPanelOrders.Name = "borderPanelOrders";
            this.borderPanelOrders.Size = new System.Drawing.Size(870, 1);
            this.borderPanelOrders.TabIndex = 2;
            // 
            // alertPanel
            // 
            this.alertPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.alertPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(242)))), ((int)(((byte)(253)))));
            this.alertPanel.Controls.Add(this.alertLabel);
            this.alertPanel.Controls.Add(this.borderPanel);
            this.alertPanel.Location = new System.Drawing.Point(20, 20);
            this.alertPanel.Name = "alertPanel";
            this.alertPanel.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.alertPanel.Size = new System.Drawing.Size(900, 60);
            this.alertPanel.TabIndex = 0;
            // 
            // statsPanel
            // 
            this.statsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statsPanel.Location = new System.Drawing.Point(20, 100);
            this.statsPanel.Name = "statsPanel";
            this.statsPanel.Size = new System.Drawing.Size(900, 150);
            this.statsPanel.TabIndex = 1;
            // 
            // recentOrdersPanel
            // 
            this.recentOrdersPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.recentOrdersPanel.BackColor = System.Drawing.Color.White;
            this.recentOrdersPanel.Controls.Add(this.dgvRecentOrders);
            this.recentOrdersPanel.Controls.Add(this.headerPanel);
            this.recentOrdersPanel.Location = new System.Drawing.Point(20, 270);
            this.recentOrdersPanel.Name = "recentOrdersPanel";
            this.recentOrdersPanel.Padding = new System.Windows.Forms.Padding(15);
            this.recentOrdersPanel.Size = new System.Drawing.Size(900, 300);
            this.recentOrdersPanel.TabIndex = 2;
            // 
            // dgvRecentOrders
            // 
            this.dgvRecentOrders.ColumnHeadersHeight = 29;
            this.dgvRecentOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRecentOrders.Location = new System.Drawing.Point(15, 55);
            this.dgvRecentOrders.Name = "dgvRecentOrders";
            this.dgvRecentOrders.RowHeadersWidth = 51;
            this.dgvRecentOrders.Size = new System.Drawing.Size(870, 230);
            this.dgvRecentOrders.TabIndex = 0;
            // 
            // ordersTable
            // 
            this.ordersTable.Location = new System.Drawing.Point(0, 0);
            this.ordersTable.Name = "ordersTable";
            this.ordersTable.Size = new System.Drawing.Size(200, 100);
            this.ordersTable.TabIndex = 0;
            // 
            // DashboardControl
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.alertPanel);
            this.Controls.Add(this.statsPanel);
            this.Controls.Add(this.recentOrdersPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DashboardControl";
            this.Size = new System.Drawing.Size(940, 640);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.alertPanel.ResumeLayout(false);
            this.recentOrdersPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentOrders)).EndInit();
            this.ResumeLayout(false);

        }

        private Panel borderPanel;
        private Label alertLabel;
        private Panel headerPanel;
        private Label titleLabel;
        private Panel borderPanelOrders;
    }
}