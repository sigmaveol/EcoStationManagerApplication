using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    partial class InventoryControl
    {
        private System.ComponentModel.IContainer components = null;

        private Panel headerPanel;
        private Panel alertPanel;
        private Label lblAlertCount;
        private Panel productsPanel;
        private Panel historyPanel;
        private Button btnAddInventory;
        private TextBox txtInventorySearch;
        private DataGridView dgvProducts;
        private DataGridView dgvHistory;


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
            this.flowPanelAlert = new System.Windows.Forms.FlowLayoutPanel();
            this.alertLabel = new System.Windows.Forms.Label();
            this.lblAlertCount = new System.Windows.Forms.Label();
            this.alertLabel2 = new System.Windows.Forms.Label();
            this.headerPanelProducts = new System.Windows.Forms.Panel();
            this.titleLabelProducts = new System.Windows.Forms.Label();
            this.txtInventorySearch = new System.Windows.Forms.TextBox();
            this.titleLabelHistory = new System.Windows.Forms.Label();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.btnAddInventory = new System.Windows.Forms.Button();
            this.alertPanel = new System.Windows.Forms.Panel();
            this.productsPanel = new System.Windows.Forms.Panel();
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            this.historyPanel = new System.Windows.Forms.Panel();
            this.dgvHistory = new System.Windows.Forms.DataGridView();
            this.flowPanelAlert.SuspendLayout();
            this.headerPanelProducts.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.alertPanel.SuspendLayout();
            this.productsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.historyPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // titleLabelHeader
            // 
            this.titleLabelHeader.AutoSize = true;
            this.titleLabelHeader.Dock = System.Windows.Forms.DockStyle.Left;
            this.titleLabelHeader.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.titleLabelHeader.Location = new System.Drawing.Point(0, 0);
            this.titleLabelHeader.Name = "titleLabelHeader";
            this.titleLabelHeader.Size = new System.Drawing.Size(324, 37);
            this.titleLabelHeader.TabIndex = 0;
            this.titleLabelHeader.Text = "Quản lý Kho & Trạm Refill";
            // 
            // flowPanelAlert
            // 
            this.flowPanelAlert.Controls.Add(this.alertLabel);
            this.flowPanelAlert.Controls.Add(this.lblAlertCount);
            this.flowPanelAlert.Controls.Add(this.alertLabel2);
            this.flowPanelAlert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPanelAlert.Location = new System.Drawing.Point(15, 0);
            this.flowPanelAlert.Name = "flowPanelAlert";
            this.flowPanelAlert.Size = new System.Drawing.Size(870, 50);
            this.flowPanelAlert.TabIndex = 0;
            this.flowPanelAlert.WrapContents = false;
            // 
            // alertLabel
            // 
            this.alertLabel.AutoSize = true;
            this.alertLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.alertLabel.Location = new System.Drawing.Point(0, 15);
            this.alertLabel.Margin = new System.Windows.Forms.Padding(0, 15, 0, 0);
            this.alertLabel.Name = "alertLabel";
            this.alertLabel.Size = new System.Drawing.Size(119, 23);
            this.alertLabel.TabIndex = 0;
            this.alertLabel.Text = "Cảnh báo: Có ";
            // 
            // lblAlertCount
            // 
            this.lblAlertCount.AutoSize = true;
            this.lblAlertCount.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblAlertCount.ForeColor = System.Drawing.Color.Red;
            this.lblAlertCount.Location = new System.Drawing.Point(119, 15);
            this.lblAlertCount.Margin = new System.Windows.Forms.Padding(0, 15, 0, 0);
            this.lblAlertCount.Name = "lblAlertCount";
            this.lblAlertCount.Size = new System.Drawing.Size(20, 23);
            this.lblAlertCount.TabIndex = 1;
            this.lblAlertCount.Text = "0";
            // 
            // alertLabel2
            // 
            this.alertLabel2.AutoSize = true;
            this.alertLabel2.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.alertLabel2.Location = new System.Drawing.Point(139, 15);
            this.alertLabel2.Margin = new System.Windows.Forms.Padding(0, 15, 0, 0);
            this.alertLabel2.Name = "alertLabel2";
            this.alertLabel2.Size = new System.Drawing.Size(199, 23);
            this.alertLabel2.TabIndex = 2;
            this.alertLabel2.Text = " sản phẩm sắp hết hàng.";
            // 
            // headerPanelProducts
            // 
            this.headerPanelProducts.Controls.Add(this.titleLabelProducts);
            this.headerPanelProducts.Controls.Add(this.txtInventorySearch);
            this.headerPanelProducts.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanelProducts.Location = new System.Drawing.Point(15, 15);
            this.headerPanelProducts.Name = "headerPanelProducts";
            this.headerPanelProducts.Size = new System.Drawing.Size(870, 40);
            this.headerPanelProducts.TabIndex = 0;
            // 
            // titleLabelProducts
            // 
            this.titleLabelProducts.AutoSize = true;
            this.titleLabelProducts.Dock = System.Windows.Forms.DockStyle.Left;
            this.titleLabelProducts.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.titleLabelProducts.Location = new System.Drawing.Point(0, 0);
            this.titleLabelProducts.Name = "titleLabelProducts";
            this.titleLabelProducts.Size = new System.Drawing.Size(207, 28);
            this.titleLabelProducts.TabIndex = 0;
            this.titleLabelProducts.Text = "Danh sách sản phẩm";
            // 
            // txtInventorySearch
            // 
            this.txtInventorySearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.txtInventorySearch.Location = new System.Drawing.Point(620, 0);
            this.txtInventorySearch.Name = "txtInventorySearch";
            this.txtInventorySearch.Size = new System.Drawing.Size(250, 27);
            this.txtInventorySearch.TabIndex = 1;
            // 
            // titleLabelHistory
            // 
            this.titleLabelHistory.AutoSize = true;
            this.titleLabelHistory.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleLabelHistory.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.titleLabelHistory.Location = new System.Drawing.Point(15, 15);
            this.titleLabelHistory.Name = "titleLabelHistory";
            this.titleLabelHistory.Size = new System.Drawing.Size(220, 28);
            this.titleLabelHistory.TabIndex = 0;
            this.titleLabelHistory.Text = "Lịch sử nhập xuất kho";
            // 
            // headerPanel
            // 
            this.headerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.headerPanel.Controls.Add(this.titleLabelHeader);
            this.headerPanel.Controls.Add(this.btnAddInventory);
            this.headerPanel.Location = new System.Drawing.Point(20, 20);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(900, 60);
            this.headerPanel.TabIndex = 0;
            // 
            // btnAddInventory
            // 
            this.btnAddInventory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnAddInventory.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAddInventory.FlatAppearance.BorderSize = 0;
            this.btnAddInventory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddInventory.ForeColor = System.Drawing.Color.White;
            this.btnAddInventory.Location = new System.Drawing.Point(770, 0);
            this.btnAddInventory.Name = "btnAddInventory";
            this.btnAddInventory.Size = new System.Drawing.Size(130, 60);
            this.btnAddInventory.TabIndex = 1;
            this.btnAddInventory.Text = "+ Thêm sản phẩm";
            this.btnAddInventory.UseVisualStyleBackColor = false;
            // 
            // alertPanel
            // 
            this.alertPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.alertPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(225)))));
            this.alertPanel.Controls.Add(this.flowPanelAlert);
            this.alertPanel.Location = new System.Drawing.Point(20, 90);
            this.alertPanel.Name = "alertPanel";
            this.alertPanel.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.alertPanel.Size = new System.Drawing.Size(900, 50);
            this.alertPanel.TabIndex = 1;
            // 
            // productsPanel
            // 
            this.productsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.productsPanel.BackColor = System.Drawing.Color.White;
            this.productsPanel.Controls.Add(this.headerPanelProducts);
            this.productsPanel.Controls.Add(this.dgvProducts);
            this.productsPanel.Location = new System.Drawing.Point(20, 160);
            this.productsPanel.Name = "productsPanel";
            this.productsPanel.Padding = new System.Windows.Forms.Padding(15);
            this.productsPanel.Size = new System.Drawing.Size(900, 250);
            this.productsPanel.TabIndex = 2;
            // 
            // dgvProducts
            // 
            this.dgvProducts.ColumnHeadersHeight = 29;
            this.dgvProducts.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvProducts.Location = new System.Drawing.Point(15, 55);
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.RowHeadersWidth = 51;
            this.dgvProducts.Size = new System.Drawing.Size(870, 180);
            this.dgvProducts.TabIndex = 1;
            // 
            // historyPanel
            // 
            this.historyPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.historyPanel.BackColor = System.Drawing.Color.White;
            this.historyPanel.Controls.Add(this.titleLabelHistory);
            this.historyPanel.Controls.Add(this.dgvHistory);
            this.historyPanel.Location = new System.Drawing.Point(20, 430);
            this.historyPanel.Name = "historyPanel";
            this.historyPanel.Padding = new System.Windows.Forms.Padding(15);
            this.historyPanel.Size = new System.Drawing.Size(900, 200);
            this.historyPanel.TabIndex = 3;
            // 
            // dgvHistory
            // 
            this.dgvHistory.ColumnHeadersHeight = 29;
            this.dgvHistory.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvHistory.Location = new System.Drawing.Point(15, 35);
            this.dgvHistory.Name = "dgvHistory";
            this.dgvHistory.RowHeadersWidth = 51;
            this.dgvHistory.Size = new System.Drawing.Size(870, 150);
            this.dgvHistory.TabIndex = 1;
            // 
            // InventoryControl
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.headerPanel);
            this.Controls.Add(this.alertPanel);
            this.Controls.Add(this.productsPanel);
            this.Controls.Add(this.historyPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "InventoryControl";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(940, 640);
            this.flowPanelAlert.ResumeLayout(false);
            this.flowPanelAlert.PerformLayout();
            this.headerPanelProducts.ResumeLayout(false);
            this.headerPanelProducts.PerformLayout();
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.alertPanel.ResumeLayout(false);
            this.productsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.historyPanel.ResumeLayout(false);
            this.historyPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).EndInit();
            this.ResumeLayout(false);

        }

        private Label titleLabelHeader;
        private FlowLayoutPanel flowPanelAlert;
        private Label alertLabel;
        private Label alertLabel2;
        private Panel headerPanelProducts;
        private Label titleLabelProducts;
        private Label titleLabelHistory;

        // HÀM HELPER SetupDataGridStyle ĐÃ BỊ XÓA KHỎI FILE NÀY
    }
}