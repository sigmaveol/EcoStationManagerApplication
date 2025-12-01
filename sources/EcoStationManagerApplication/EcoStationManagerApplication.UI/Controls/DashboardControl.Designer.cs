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
            this.panelChartsRight = new System.Windows.Forms.Panel();
            this.chartRevenue7Days = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblRevenueTitle = new System.Windows.Forms.Label();
            this.panelChartsLeft = new System.Windows.Forms.Panel();
            this.chartOrderSource = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblSourceTitle = new System.Windows.Forms.Label();
            this.statsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.cardTodayOrders = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.cardMonthlyRevenue = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.cardLowStock = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.cardPackagingInUse = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.cardPendingOrders = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.alertPanel = new System.Windows.Forms.Panel();
            this.alertLabel = new System.Windows.Forms.Label();
            this.borderPanel = new System.Windows.Forms.Panel();
            this.panelContent.SuspendLayout();
            this.recentOrdersPanel.SuspendLayout();
            this.panelChartsRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartRevenue7Days)).BeginInit();
            this.panelChartsLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartOrderSource)).BeginInit();
            this.statsPanel.SuspendLayout();
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
            this.recentOrdersPanel.Controls.Add(this.panelChartsRight);
            this.recentOrdersPanel.Controls.Add(this.panelChartsLeft);
            this.recentOrdersPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.recentOrdersPanel.Location = new System.Drawing.Point(15, 426);
            this.recentOrdersPanel.MinimumSize = new System.Drawing.Size(200, 200);
            this.recentOrdersPanel.Name = "recentOrdersPanel";
            this.recentOrdersPanel.Padding = new System.Windows.Forms.Padding(15);
            this.recentOrdersPanel.Size = new System.Drawing.Size(884, 512);
            this.recentOrdersPanel.TabIndex = 2;
            // 
            // panelChartsRight
            // 
            this.panelChartsRight.BackColor = System.Drawing.Color.White;
            this.panelChartsRight.Controls.Add(this.chartRevenue7Days);
            this.panelChartsRight.Controls.Add(this.lblRevenueTitle);
            this.panelChartsRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChartsRight.Location = new System.Drawing.Point(445, 15);
            this.panelChartsRight.Name = "panelChartsRight";
            this.panelChartsRight.Padding = new System.Windows.Forms.Padding(10);
            this.panelChartsRight.Size = new System.Drawing.Size(424, 482);
            this.panelChartsRight.TabIndex = 4;
            // 
            // chartRevenue7Days
            // 
            this.chartRevenue7Days.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartRevenue7Days.Location = new System.Drawing.Point(10, 38);
            this.chartRevenue7Days.Name = "chartRevenue7Days";
            this.chartRevenue7Days.Size = new System.Drawing.Size(404, 434);
            this.chartRevenue7Days.TabIndex = 1;
            // 
            // lblRevenueTitle
            // 
            this.lblRevenueTitle.AutoSize = true;
            this.lblRevenueTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRevenueTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblRevenueTitle.Location = new System.Drawing.Point(10, 10);
            this.lblRevenueTitle.Name = "lblRevenueTitle";
            this.lblRevenueTitle.Size = new System.Drawing.Size(310, 28);
            this.lblRevenueTitle.TabIndex = 6;
            this.lblRevenueTitle.Text = "Doanh thu 7 ngày (biểu đồ cột)";
            // 
            // panelChartsLeft
            // 
            this.panelChartsLeft.BackColor = System.Drawing.Color.White;
            this.panelChartsLeft.Controls.Add(this.chartOrderSource);
            this.panelChartsLeft.Controls.Add(this.lblSourceTitle);
            this.panelChartsLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelChartsLeft.Location = new System.Drawing.Point(15, 15);
            this.panelChartsLeft.Name = "panelChartsLeft";
            this.panelChartsLeft.Padding = new System.Windows.Forms.Padding(10);
            this.panelChartsLeft.Size = new System.Drawing.Size(430, 482);
            this.panelChartsLeft.TabIndex = 3;
            // 
            // chartOrderSource
            // 
            this.chartOrderSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartOrderSource.Location = new System.Drawing.Point(10, 38);
            this.chartOrderSource.Name = "chartOrderSource";
            this.chartOrderSource.Size = new System.Drawing.Size(410, 434);
            this.chartOrderSource.TabIndex = 0;
            // 
            // lblSourceTitle
            // 
            this.lblSourceTitle.AutoSize = true;
            this.lblSourceTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSourceTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSourceTitle.Location = new System.Drawing.Point(10, 10);
            this.lblSourceTitle.Name = "lblSourceTitle";
            this.lblSourceTitle.Size = new System.Drawing.Size(252, 28);
            this.lblSourceTitle.TabIndex = 5;
            this.lblSourceTitle.Text = "Tỷ lệ nguồn đơn (7 ngày)";
            // 
            // statsPanel
            // 
            this.statsPanel.AutoSize = true;
            this.statsPanel.Controls.Add(this.cardTodayOrders);
            this.statsPanel.Controls.Add(this.cardMonthlyRevenue);
            this.statsPanel.Controls.Add(this.cardLowStock);
            this.statsPanel.Controls.Add(this.cardPackagingInUse);
            this.statsPanel.Controls.Add(this.cardPendingOrders);
            this.statsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.statsPanel.Location = new System.Drawing.Point(15, 80);
            this.statsPanel.Name = "statsPanel";
            this.statsPanel.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.statsPanel.Size = new System.Drawing.Size(884, 346);
            this.statsPanel.TabIndex = 1;
            // 
            // cardTodayOrders
            // 
            this.cardTodayOrders.AutoSize = true;
            this.cardTodayOrders.BackColor = System.Drawing.Color.Transparent;
            this.cardTodayOrders.CardColor = System.Drawing.Color.White;
            this.cardTodayOrders.Change = null;
            this.cardTodayOrders.ChangeColor = System.Drawing.Color.Green;
            this.cardTodayOrders.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardTodayOrders.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardTodayOrders.Icon = null;
            this.cardTodayOrders.Location = new System.Drawing.Point(6, 34);
            this.cardTodayOrders.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.cardTodayOrders.MaximumSize = new System.Drawing.Size(260, 150);
            this.cardTodayOrders.Name = "cardTodayOrders";
            this.cardTodayOrders.Padding = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.cardTodayOrders.Size = new System.Drawing.Size(204, 150);
            this.cardTodayOrders.SubInfo = "Đang tải...";
            this.cardTodayOrders.SubInfoColor = System.Drawing.Color.Gray;
            this.cardTodayOrders.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardTodayOrders.TabIndex = 0;
            this.cardTodayOrders.Tag = "Đơn hàng hôm nay";
            this.cardTodayOrders.Title = "Đơn hàng hôm nay";
            this.cardTodayOrders.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.cardTodayOrders.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.cardTodayOrders.Value = "0";
            this.cardTodayOrders.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.cardTodayOrders.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            // 
            // cardMonthlyRevenue
            // 
            this.cardMonthlyRevenue.AutoSize = true;
            this.cardMonthlyRevenue.BackColor = System.Drawing.Color.Transparent;
            this.cardMonthlyRevenue.CardColor = System.Drawing.Color.White;
            this.cardMonthlyRevenue.Change = null;
            this.cardMonthlyRevenue.ChangeColor = System.Drawing.Color.Green;
            this.cardMonthlyRevenue.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardMonthlyRevenue.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardMonthlyRevenue.Icon = null;
            this.cardMonthlyRevenue.Location = new System.Drawing.Point(222, 34);
            this.cardMonthlyRevenue.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.cardMonthlyRevenue.MaximumSize = new System.Drawing.Size(260, 150);
            this.cardMonthlyRevenue.Name = "cardMonthlyRevenue";
            this.cardMonthlyRevenue.Padding = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.cardMonthlyRevenue.Size = new System.Drawing.Size(185, 150);
            this.cardMonthlyRevenue.SubInfo = "VND";
            this.cardMonthlyRevenue.SubInfoColor = System.Drawing.Color.Gray;
            this.cardMonthlyRevenue.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardMonthlyRevenue.TabIndex = 1;
            this.cardMonthlyRevenue.Tag = "Doanh thu tháng";
            this.cardMonthlyRevenue.Title = "Doanh thu tháng";
            this.cardMonthlyRevenue.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.cardMonthlyRevenue.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.cardMonthlyRevenue.Value = "0";
            this.cardMonthlyRevenue.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.cardMonthlyRevenue.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            // 
            // cardLowStock
            // 
            this.cardLowStock.AutoSize = true;
            this.cardLowStock.BackColor = System.Drawing.Color.Transparent;
            this.cardLowStock.CardColor = System.Drawing.Color.White;
            this.cardLowStock.Change = null;
            this.cardLowStock.ChangeColor = System.Drawing.Color.Green;
            this.cardLowStock.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardLowStock.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardLowStock.Icon = null;
            this.cardLowStock.Location = new System.Drawing.Point(419, 34);
            this.cardLowStock.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.cardLowStock.MaximumSize = new System.Drawing.Size(260, 150);
            this.cardLowStock.Name = "cardLowStock";
            this.cardLowStock.Padding = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.cardLowStock.Size = new System.Drawing.Size(152, 150);
            this.cardLowStock.SubInfo = "Sản phẩm";
            this.cardLowStock.SubInfoColor = System.Drawing.Color.Gray;
            this.cardLowStock.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardLowStock.TabIndex = 2;
            this.cardLowStock.Tag = "Tồn kho thấp";
            this.cardLowStock.Title = "Tồn kho thấp";
            this.cardLowStock.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.cardLowStock.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.cardLowStock.Value = "0";
            this.cardLowStock.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.cardLowStock.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            // 
            // cardPackagingInUse
            // 
            this.cardPackagingInUse.AutoSize = true;
            this.cardPackagingInUse.BackColor = System.Drawing.Color.Transparent;
            this.cardPackagingInUse.CardColor = System.Drawing.Color.White;
            this.cardPackagingInUse.Change = null;
            this.cardPackagingInUse.ChangeColor = System.Drawing.Color.Green;
            this.cardPackagingInUse.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardPackagingInUse.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardPackagingInUse.Icon = null;
            this.cardPackagingInUse.Location = new System.Drawing.Point(583, 34);
            this.cardPackagingInUse.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.cardPackagingInUse.MaximumSize = new System.Drawing.Size(260, 150);
            this.cardPackagingInUse.Name = "cardPackagingInUse";
            this.cardPackagingInUse.Padding = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.cardPackagingInUse.Size = new System.Drawing.Size(260, 150);
            this.cardPackagingInUse.SubInfo = "Chai/lọ";
            this.cardPackagingInUse.SubInfoColor = System.Drawing.Color.Gray;
            this.cardPackagingInUse.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardPackagingInUse.TabIndex = 3;
            this.cardPackagingInUse.Tag = "Bao bì đang được sử dụng";
            this.cardPackagingInUse.Title = "Bao bì đang được sử dụng";
            this.cardPackagingInUse.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.cardPackagingInUse.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.cardPackagingInUse.Value = "0";
            this.cardPackagingInUse.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.cardPackagingInUse.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            // 
            // cardPendingOrders
            // 
            this.cardPendingOrders.AutoSize = true;
            this.cardPendingOrders.BackColor = System.Drawing.Color.Transparent;
            this.cardPendingOrders.CardColor = System.Drawing.Color.White;
            this.cardPendingOrders.Change = null;
            this.cardPendingOrders.ChangeColor = System.Drawing.Color.Green;
            this.cardPendingOrders.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardPendingOrders.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardPendingOrders.Icon = null;
            this.cardPendingOrders.Location = new System.Drawing.Point(6, 192);
            this.cardPendingOrders.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.cardPendingOrders.MaximumSize = new System.Drawing.Size(260, 150);
            this.cardPendingOrders.Name = "cardPendingOrders";
            this.cardPendingOrders.Padding = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.cardPendingOrders.Size = new System.Drawing.Size(160, 150);
            this.cardPendingOrders.SubInfo = "Cần xử lý";
            this.cardPendingOrders.SubInfoColor = System.Drawing.Color.Gray;
            this.cardPendingOrders.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardPendingOrders.TabIndex = 4;
            this.cardPendingOrders.Tag = "Đơn chờ xử lý";
            this.cardPendingOrders.Title = "Đơn chờ xử lý";
            this.cardPendingOrders.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.cardPendingOrders.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.cardPendingOrders.Value = "0";
            this.cardPendingOrders.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.cardPendingOrders.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
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
            this.alertPanel.Size = new System.Drawing.Size(884, 60);
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
            this.alertLabel.Size = new System.Drawing.Size(865, 60);
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
            this.panelChartsRight.ResumeLayout(false);
            this.panelChartsRight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartRevenue7Days)).EndInit();
            this.panelChartsLeft.ResumeLayout(false);
            this.panelChartsLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartOrderSource)).EndInit();
            this.statsPanel.ResumeLayout(false);
            this.statsPanel.PerformLayout();
            this.alertPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        private Panel panelContent;
        private Panel recentOrdersPanel;
        private FlowLayoutPanel statsPanel;
        private EcoStationManagerApplication.UI.Controls.CardControl cardTodayOrders;
        private EcoStationManagerApplication.UI.Controls.CardControl cardMonthlyRevenue;
        private EcoStationManagerApplication.UI.Controls.CardControl cardLowStock;
        private EcoStationManagerApplication.UI.Controls.CardControl cardPackagingInUse;
        private EcoStationManagerApplication.UI.Controls.CardControl cardPendingOrders;
        private Panel alertPanel;
        private Label alertLabel;
        private Panel borderPanel;
        private Panel panelChartsLeft;
        private Panel panelChartsRight;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartOrderSource;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRevenue7Days;
        private Label lblSourceTitle;
        private Label lblRevenueTitle;
    }
}
