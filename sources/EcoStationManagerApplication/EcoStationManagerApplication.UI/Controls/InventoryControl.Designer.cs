namespace EcoStationManagerApplication.UI.Controls
{
    partial class InventoryControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.guna2PanelContent = new Guna.UI2.WinForms.Guna2Panel();
            this.dataGridViewInventory = new Guna.UI2.WinForms.Guna2DataGridView();
            this.panelSearchFilter = new System.Windows.Forms.Panel();
            this.comboBoxStation = new Guna.UI2.WinForms.Guna2ComboBox();
            this.txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.flowLayoutPanelStats = new System.Windows.Forms.FlowLayoutPanel();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTotalStock = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.lblReservedStock = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.guna2Panel3 = new Guna.UI2.WinForms.Guna2Panel();
            this.lblAvailableStock = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.guna2Panel4 = new Guna.UI2.WinForms.Guna2Panel();
            this.lblLowStockCount = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.guna2PanelHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.guna2PanelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInventory)).BeginInit();
            this.panelSearchFilter.SuspendLayout();
            this.flowLayoutPanelStats.SuspendLayout();
            this.guna2Panel1.SuspendLayout();
            this.guna2Panel2.SuspendLayout();
            this.guna2Panel3.SuspendLayout();
            this.guna2Panel4.SuspendLayout();
            this.guna2PanelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2PanelContent
            // 
            this.guna2PanelContent.AutoScroll = true;
            this.guna2PanelContent.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.guna2PanelContent.BackColor = System.Drawing.Color.White;
            this.guna2PanelContent.Controls.Add(this.dataGridViewInventory);
            this.guna2PanelContent.Controls.Add(this.panelSearchFilter);
            this.guna2PanelContent.Controls.Add(this.flowLayoutPanelStats);
            this.guna2PanelContent.Controls.Add(this.guna2PanelHeader);
            this.guna2PanelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2PanelContent.Location = new System.Drawing.Point(0, 0);
            this.guna2PanelContent.Name = "guna2PanelContent";
            this.guna2PanelContent.Padding = new System.Windows.Forms.Padding(20);
            this.guna2PanelContent.Size = new System.Drawing.Size(1200, 700);
            this.guna2PanelContent.TabIndex = 0;
            // 
            // dataGridViewInventory
            // 
            this.dataGridViewInventory.AllowUserToAddRows = false;
            this.dataGridViewInventory.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dataGridViewInventory.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewInventory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewInventory.ColumnHeadersHeight = 40;
            this.dataGridViewInventory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewInventory.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewInventory.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridViewInventory.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dataGridViewInventory.Location = new System.Drawing.Point(20, 267);
            this.dataGridViewInventory.Name = "dataGridViewInventory";
            this.dataGridViewInventory.ReadOnly = true;
            this.dataGridViewInventory.RowHeadersVisible = false;
            this.dataGridViewInventory.RowHeadersWidth = 51;
            this.dataGridViewInventory.Size = new System.Drawing.Size(1139, 435);
            this.dataGridViewInventory.TabIndex = 3;
            this.dataGridViewInventory.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewInventory.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dataGridViewInventory.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dataGridViewInventory.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dataGridViewInventory.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dataGridViewInventory.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewInventory.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dataGridViewInventory.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dataGridViewInventory.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridViewInventory.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.dataGridViewInventory.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dataGridViewInventory.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dataGridViewInventory.ThemeStyle.HeaderStyle.Height = 40;
            this.dataGridViewInventory.ThemeStyle.ReadOnly = true;
            this.dataGridViewInventory.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewInventory.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridViewInventory.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.dataGridViewInventory.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dataGridViewInventory.ThemeStyle.RowsStyle.Height = 22;
            this.dataGridViewInventory.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dataGridViewInventory.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // panelSearchFilter
            // 
            this.panelSearchFilter.AutoSize = true;
            this.panelSearchFilter.BackColor = System.Drawing.Color.White;
            this.panelSearchFilter.Controls.Add(this.comboBoxStation);
            this.panelSearchFilter.Controls.Add(this.txtSearch);
            this.panelSearchFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearchFilter.Location = new System.Drawing.Point(20, 208);
            this.panelSearchFilter.Name = "panelSearchFilter";
            this.panelSearchFilter.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.panelSearchFilter.Size = new System.Drawing.Size(1139, 59);
            this.panelSearchFilter.TabIndex = 2;
            // 
            // comboBoxStation
            // 
            this.comboBoxStation.BackColor = System.Drawing.Color.Transparent;
            this.comboBoxStation.BorderRadius = 5;
            this.comboBoxStation.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxStation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStation.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.comboBoxStation.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.comboBoxStation.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.comboBoxStation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.comboBoxStation.ItemHeight = 30;
            this.comboBoxStation.Location = new System.Drawing.Point(320, 10);
            this.comboBoxStation.Name = "comboBoxStation";
            this.comboBoxStation.Size = new System.Drawing.Size(200, 36);
            this.comboBoxStation.TabIndex = 1;
            this.comboBoxStation.SelectedIndexChanged += new System.EventHandler(this.comboBoxStation_SelectedIndexChanged);
            // 
            // txtSearch
            // 
            this.txtSearch.BorderRadius = 5;
            this.txtSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSearch.DefaultText = "";
            this.txtSearch.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtSearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtSearch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSearch.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSearch.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtSearch.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSearch.Location = new System.Drawing.Point(0, 10);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceholderText = "Tìm kiếm sản phẩm...";
            this.txtSearch.SelectedText = "";
            this.txtSearch.Size = new System.Drawing.Size(300, 35);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // flowLayoutPanelStats
            // 
            this.flowLayoutPanelStats.AutoSize = true;
            this.flowLayoutPanelStats.Controls.Add(this.guna2Panel1);
            this.flowLayoutPanelStats.Controls.Add(this.guna2Panel2);
            this.flowLayoutPanelStats.Controls.Add(this.guna2Panel3);
            this.flowLayoutPanelStats.Controls.Add(this.guna2Panel4);
            this.flowLayoutPanelStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanelStats.Location = new System.Drawing.Point(20, 95);
            this.flowLayoutPanelStats.Name = "flowLayoutPanelStats";
            this.flowLayoutPanelStats.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.flowLayoutPanelStats.Size = new System.Drawing.Size(1139, 113);
            this.flowLayoutPanelStats.TabIndex = 1;
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.White;
            this.guna2Panel1.BorderColor = System.Drawing.Color.LightGray;
            this.guna2Panel1.BorderRadius = 10;
            this.guna2Panel1.BorderThickness = 1;
            this.guna2Panel1.Controls.Add(this.lblTotalStock);
            this.guna2Panel1.Controls.Add(this.label1);
            this.guna2Panel1.Location = new System.Drawing.Point(0, 23);
            this.guna2Panel1.Margin = new System.Windows.Forms.Padding(0, 13, 10, 10);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(270, 70);
            this.guna2Panel1.TabIndex = 0;
            // 
            // lblTotalStock
            // 
            this.lblTotalStock.AutoSize = true;
            this.lblTotalStock.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTotalStock.Location = new System.Drawing.Point(15, 35);
            this.lblTotalStock.Name = "lblTotalStock";
            this.lblTotalStock.Size = new System.Drawing.Size(33, 37);
            this.lblTotalStock.TabIndex = 1;
            this.lblTotalStock.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tổng tồn kho";
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.BackColor = System.Drawing.Color.White;
            this.guna2Panel2.BorderColor = System.Drawing.Color.LightGray;
            this.guna2Panel2.BorderRadius = 10;
            this.guna2Panel2.BorderThickness = 1;
            this.guna2Panel2.Controls.Add(this.lblReservedStock);
            this.guna2Panel2.Controls.Add(this.label3);
            this.guna2Panel2.Location = new System.Drawing.Point(280, 23);
            this.guna2Panel2.Margin = new System.Windows.Forms.Padding(0, 13, 10, 10);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.Size = new System.Drawing.Size(270, 70);
            this.guna2Panel2.TabIndex = 1;
            // 
            // lblReservedStock
            // 
            this.lblReservedStock.AutoSize = true;
            this.lblReservedStock.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblReservedStock.Location = new System.Drawing.Point(15, 35);
            this.lblReservedStock.Name = "lblReservedStock";
            this.lblReservedStock.Size = new System.Drawing.Size(33, 37);
            this.lblReservedStock.TabIndex = 1;
            this.lblReservedStock.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label3.ForeColor = System.Drawing.Color.Gray;
            this.label3.Location = new System.Drawing.Point(15, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Đã đặt trước";
            // 
            // guna2Panel3
            // 
            this.guna2Panel3.BackColor = System.Drawing.Color.White;
            this.guna2Panel3.BorderColor = System.Drawing.Color.LightGray;
            this.guna2Panel3.BorderRadius = 10;
            this.guna2Panel3.BorderThickness = 1;
            this.guna2Panel3.Controls.Add(this.lblAvailableStock);
            this.guna2Panel3.Controls.Add(this.label5);
            this.guna2Panel3.Location = new System.Drawing.Point(560, 23);
            this.guna2Panel3.Margin = new System.Windows.Forms.Padding(0, 13, 10, 10);
            this.guna2Panel3.Name = "guna2Panel3";
            this.guna2Panel3.Size = new System.Drawing.Size(270, 70);
            this.guna2Panel3.TabIndex = 2;
            // 
            // lblAvailableStock
            // 
            this.lblAvailableStock.AutoSize = true;
            this.lblAvailableStock.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblAvailableStock.ForeColor = System.Drawing.Color.Green;
            this.lblAvailableStock.Location = new System.Drawing.Point(15, 35);
            this.lblAvailableStock.Name = "lblAvailableStock";
            this.lblAvailableStock.Size = new System.Drawing.Size(33, 37);
            this.lblAvailableStock.TabIndex = 1;
            this.lblAvailableStock.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label5.ForeColor = System.Drawing.Color.Gray;
            this.label5.Location = new System.Drawing.Point(15, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 20);
            this.label5.TabIndex = 0;
            this.label5.Text = "Có thể bán";
            // 
            // guna2Panel4
            // 
            this.guna2Panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(229)))));
            this.guna2Panel4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(150)))));
            this.guna2Panel4.BorderRadius = 10;
            this.guna2Panel4.BorderThickness = 1;
            this.guna2Panel4.Controls.Add(this.lblLowStockCount);
            this.guna2Panel4.Controls.Add(this.label7);
            this.guna2Panel4.Location = new System.Drawing.Point(840, 23);
            this.guna2Panel4.Margin = new System.Windows.Forms.Padding(0, 13, 10, 10);
            this.guna2Panel4.Name = "guna2Panel4";
            this.guna2Panel4.Size = new System.Drawing.Size(270, 70);
            this.guna2Panel4.TabIndex = 3;
            // 
            // lblLowStockCount
            // 
            this.lblLowStockCount.AutoSize = true;
            this.lblLowStockCount.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblLowStockCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblLowStockCount.Location = new System.Drawing.Point(15, 35);
            this.lblLowStockCount.Name = "lblLowStockCount";
            this.lblLowStockCount.Size = new System.Drawing.Size(33, 37);
            this.lblLowStockCount.TabIndex = 1;
            this.lblLowStockCount.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.label7.Location = new System.Drawing.Point(15, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(109, 20);
            this.label7.TabIndex = 0;
            this.label7.Text = "Cần nhập thêm";
            // 
            // guna2PanelHeader
            // 
            this.guna2PanelHeader.AutoSize = true;
            this.guna2PanelHeader.BackColor = System.Drawing.Color.White;
            this.guna2PanelHeader.Controls.Add(this.lblDescription);
            this.guna2PanelHeader.Controls.Add(this.lblTitle);
            this.guna2PanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2PanelHeader.Location = new System.Drawing.Point(20, 20);
            this.guna2PanelHeader.Name = "guna2PanelHeader";
            this.guna2PanelHeader.Size = new System.Drawing.Size(1139, 75);
            this.guna2PanelHeader.TabIndex = 0;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDescription.ForeColor = System.Drawing.Color.Gray;
            this.lblDescription.Location = new System.Drawing.Point(22, 55);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(474, 20);
            this.lblDescription.TabIndex = 1;
            this.lblDescription.Text = "Inventories tổng, StationInventories cho từng trạm, cảnh báo min/max.";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(121, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Tồn kho";
            // 
            // InventoryControl
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.guna2PanelContent);
            this.Name = "InventoryControl";
            this.Size = new System.Drawing.Size(1200, 700);
            this.guna2PanelContent.ResumeLayout(false);
            this.guna2PanelContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInventory)).EndInit();
            this.panelSearchFilter.ResumeLayout(false);
            this.flowLayoutPanelStats.ResumeLayout(false);
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            this.guna2Panel2.ResumeLayout(false);
            this.guna2Panel2.PerformLayout();
            this.guna2Panel3.ResumeLayout(false);
            this.guna2Panel3.PerformLayout();
            this.guna2Panel4.ResumeLayout(false);
            this.guna2Panel4.PerformLayout();
            this.guna2PanelHeader.ResumeLayout(false);
            this.guna2PanelHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2PanelContent;
        private Guna.UI2.WinForms.Guna2DataGridView dataGridViewInventory;
        private System.Windows.Forms.Panel panelSearchFilter;
        private Guna.UI2.WinForms.Guna2ComboBox comboBoxStation;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelStats;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private System.Windows.Forms.Label lblTotalStock;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private System.Windows.Forms.Label lblReservedStock;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel3;
        private System.Windows.Forms.Label lblAvailableStock;
        private System.Windows.Forms.Label label5;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel4;
        private System.Windows.Forms.Label lblLowStockCount;
        private System.Windows.Forms.Label label7;
        private Guna.UI2.WinForms.Guna2Panel guna2PanelHeader;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblTitle;
    }
}