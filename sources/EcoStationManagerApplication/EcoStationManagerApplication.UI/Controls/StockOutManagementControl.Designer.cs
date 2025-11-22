using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    partial class StockOutManagementControl
    {
        private System.ComponentModel.IContainer components = null;
        private Guna2Panel panelMain;
        private Guna2Panel guna2PanelHeader;
        private Label lblTitle;
        private Label lblDescription;
        private FlowLayoutPanel panelStatsCards;
        private CardControl cardTotalStockOuts;
        private CardControl cardSale;
        private CardControl cardTransfer;
        private CardControl cardWaste;
        private CardControl cardTotalQuantity;
        private Guna2Panel panelFilter;
        private Guna2TextBox txtSearch;
        private Label lblSearch;
        private Guna2ComboBox cmbPurposeFilter;
        private Label lblPurposeFilter;
        private DateTimePicker dtpFromDate;
        private Label lblFromDate;
        private DateTimePicker dtpToDate;
        private Label lblToDate;
        private Guna2Panel panelToolbar;
        private Guna2Button btnExportExcel;
        private Guna2Button btnCreateStockOut;
        private Guna2Button btnRefresh;
        private Guna2DataGridView dgvStockOut;

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelMain = new Guna.UI2.WinForms.Guna2Panel();
            this.dgvStockOut = new Guna.UI2.WinForms.Guna2DataGridView();
            this.panelToolbar = new Guna.UI2.WinForms.Guna2Panel();
            this.btnRefresh = new Guna.UI2.WinForms.Guna2Button();
            this.btnExportExcel = new Guna.UI2.WinForms.Guna2Button();
            this.btnCreateStockOut = new Guna.UI2.WinForms.Guna2Button();
            this.panelFilter = new Guna.UI2.WinForms.Guna2Panel();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.lblToDate = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.cmbPurposeFilter = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblPurposeFilter = new System.Windows.Forms.Label();
            this.txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.panelStatsCards = new System.Windows.Forms.FlowLayoutPanel();
            this.cardTotalQuantity = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.cardWaste = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.cardTransfer = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.cardSale = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.cardTotalStockOuts = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.guna2PanelHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockOut)).BeginInit();
            this.panelToolbar.SuspendLayout();
            this.panelFilter.SuspendLayout();
            this.panelStatsCards.SuspendLayout();
            this.guna2PanelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.AutoScroll = true;
            this.panelMain.BackColor = System.Drawing.Color.White;
            this.panelMain.Controls.Add(this.dgvStockOut);
            this.panelMain.Controls.Add(this.panelToolbar);
            this.panelMain.Controls.Add(this.panelFilter);
            this.panelMain.Controls.Add(this.panelStatsCards);
            this.panelMain.Controls.Add(this.guna2PanelHeader);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(20, 16, 20, 16);
            this.panelMain.Size = new System.Drawing.Size(1200, 821);
            this.panelMain.TabIndex = 0;
            // 
            // dgvStockOut
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvStockOut.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvStockOut.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvStockOut.ColumnHeadersHeight = 40;
            this.dgvStockOut.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(248)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvStockOut.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvStockOut.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvStockOut.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvStockOut.Location = new System.Drawing.Point(20, 523);
            this.dgvStockOut.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvStockOut.Name = "dgvStockOut";
            this.dgvStockOut.ReadOnly = true;
            this.dgvStockOut.RowHeadersVisible = false;
            this.dgvStockOut.RowHeadersWidth = 51;
            this.dgvStockOut.Size = new System.Drawing.Size(1160, 296);
            this.dgvStockOut.TabIndex = 4;
            this.dgvStockOut.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvStockOut.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvStockOut.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvStockOut.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvStockOut.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvStockOut.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvStockOut.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvStockOut.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.dgvStockOut.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvStockOut.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.dgvStockOut.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvStockOut.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvStockOut.ThemeStyle.HeaderStyle.Height = 40;
            this.dgvStockOut.ThemeStyle.ReadOnly = true;
            this.dgvStockOut.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvStockOut.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvStockOut.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.dgvStockOut.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.dgvStockOut.ThemeStyle.RowsStyle.Height = 22;
            this.dgvStockOut.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(248)))), ((int)(((byte)(243)))));
            this.dgvStockOut.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.dgvStockOut.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockOut_CellContentClick);
            // 
            // panelToolbar
            // 
            this.panelToolbar.BackColor = System.Drawing.Color.White;
            this.panelToolbar.Controls.Add(this.btnRefresh);
            this.panelToolbar.Controls.Add(this.btnExportExcel);
            this.panelToolbar.Controls.Add(this.btnCreateStockOut);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolbar.Location = new System.Drawing.Point(20, 467);
            this.panelToolbar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.panelToolbar.Size = new System.Drawing.Size(1160, 56);
            this.panelToolbar.TabIndex = 3;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BorderRadius = 5;
            this.btnRefresh.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnRefresh.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnRefresh.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnRefresh.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnRefresh.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(308, 8);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(120, 44);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.BorderRadius = 5;
            this.btnExportExcel.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnExportExcel.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnExportExcel.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnExportExcel.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnExportExcel.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(24)))), ((int)(((byte)(61)))));
            this.btnExportExcel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnExportExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportExcel.Location = new System.Drawing.Point(158, 8);
            this.btnExportExcel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(140, 44);
            this.btnExportExcel.TabIndex = 1;
            this.btnExportExcel.Text = "Xuất Excel";
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // btnCreateStockOut
            // 
            this.btnCreateStockOut.BorderRadius = 5;
            this.btnCreateStockOut.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCreateStockOut.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCreateStockOut.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCreateStockOut.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCreateStockOut.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.btnCreateStockOut.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnCreateStockOut.ForeColor = System.Drawing.Color.White;
            this.btnCreateStockOut.Location = new System.Drawing.Point(7, 8);
            this.btnCreateStockOut.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCreateStockOut.Name = "btnCreateStockOut";
            this.btnCreateStockOut.Size = new System.Drawing.Size(140, 44);
            this.btnCreateStockOut.TabIndex = 0;
            this.btnCreateStockOut.Text = "Tạo phiếu xuất";
            this.btnCreateStockOut.Click += new System.EventHandler(this.btnCreateStockOut_Click);
            // 
            // panelFilter
            // 
            this.panelFilter.BackColor = System.Drawing.Color.Transparent;
            this.panelFilter.Controls.Add(this.dtpToDate);
            this.panelFilter.Controls.Add(this.lblToDate);
            this.panelFilter.Controls.Add(this.dtpFromDate);
            this.panelFilter.Controls.Add(this.lblFromDate);
            this.panelFilter.Controls.Add(this.cmbPurposeFilter);
            this.panelFilter.Controls.Add(this.lblPurposeFilter);
            this.panelFilter.Controls.Add(this.txtSearch);
            this.panelFilter.Controls.Add(this.lblSearch);
            this.panelFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilter.Location = new System.Drawing.Point(20, 402);
            this.panelFilter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelFilter.Name = "panelFilter";
            this.panelFilter.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.panelFilter.Size = new System.Drawing.Size(1160, 65);
            this.panelFilter.TabIndex = 2;
            // 
            // dtpToDate
            // 
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpToDate.Location = new System.Drawing.Point(757, 26);
            this.dtpToDate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(150, 22);
            this.dtpToDate.TabIndex = 6;
            this.dtpToDate.ValueChanged += new System.EventHandler(this.dtpToDate_ValueChanged);
            // 
            // lblToDate
            // 
            this.lblToDate.AutoSize = true;
            this.lblToDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblToDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblToDate.Location = new System.Drawing.Point(753, 4);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(79, 20);
            this.lblToDate.TabIndex = 5;
            this.lblToDate.Text = "Đến ngày:";
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFromDate.Location = new System.Drawing.Point(580, 26);
            this.dtpFromDate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(150, 22);
            this.dtpFromDate.TabIndex = 4;
            this.dtpFromDate.ValueChanged += new System.EventHandler(this.dtpFromDate_ValueChanged);
            // 
            // lblFromDate
            // 
            this.lblFromDate.AutoSize = true;
            this.lblFromDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblFromDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblFromDate.Location = new System.Drawing.Point(576, 4);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(70, 20);
            this.lblFromDate.TabIndex = 3;
            this.lblFromDate.Text = "Từ ngày:";
            // 
            // cmbPurposeFilter
            // 
            this.cmbPurposeFilter.BackColor = System.Drawing.Color.Transparent;
            this.cmbPurposeFilter.BorderRadius = 5;
            this.cmbPurposeFilter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbPurposeFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPurposeFilter.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbPurposeFilter.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbPurposeFilter.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbPurposeFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbPurposeFilter.ItemHeight = 30;
            this.cmbPurposeFilter.Location = new System.Drawing.Point(320, 25);
            this.cmbPurposeFilter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbPurposeFilter.Name = "cmbPurposeFilter";
            this.cmbPurposeFilter.Size = new System.Drawing.Size(250, 36);
            this.cmbPurposeFilter.TabIndex = 2;
            this.cmbPurposeFilter.SelectedIndexChanged += new System.EventHandler(this.cmbPurposeFilter_SelectedIndexChanged);
            // 
            // lblPurposeFilter
            // 
            this.lblPurposeFilter.AutoSize = true;
            this.lblPurposeFilter.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPurposeFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblPurposeFilter.Location = new System.Drawing.Point(320, 4);
            this.lblPurposeFilter.Name = "lblPurposeFilter";
            this.lblPurposeFilter.Size = new System.Drawing.Size(76, 20);
            this.lblPurposeFilter.TabIndex = 3;
            this.lblPurposeFilter.Text = "Mục đích:";
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
            this.txtSearch.Location = new System.Drawing.Point(4, 25);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceholderText = "Tìm kiếm theo lô hàng, sản phẩm...";
            this.txtSearch.SelectedText = "";
            this.txtSearch.Size = new System.Drawing.Size(300, 40);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblSearch.Location = new System.Drawing.Point(0, 4);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(78, 20);
            this.lblSearch.TabIndex = 1;
            this.lblSearch.Text = "Tìm kiếm:";
            // 
            // panelStatsCards
            // 
            this.panelStatsCards.AutoSize = true;
            this.panelStatsCards.BackColor = System.Drawing.Color.Transparent;
            this.panelStatsCards.Controls.Add(this.cardTotalQuantity);
            this.panelStatsCards.Controls.Add(this.cardWaste);
            this.panelStatsCards.Controls.Add(this.cardTransfer);
            this.panelStatsCards.Controls.Add(this.cardSale);
            this.panelStatsCards.Controls.Add(this.cardTotalStockOuts);
            this.panelStatsCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelStatsCards.Location = new System.Drawing.Point(20, 78);
            this.panelStatsCards.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelStatsCards.Name = "panelStatsCards";
            this.panelStatsCards.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.panelStatsCards.Size = new System.Drawing.Size(1160, 324);
            this.panelStatsCards.TabIndex = 1;
            // 
            // cardTotalQuantity
            // 
            this.cardTotalQuantity.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.cardTotalQuantity.BackColor = System.Drawing.Color.Transparent;
            this.cardTotalQuantity.CardColor = System.Drawing.Color.White;
            this.cardTotalQuantity.Change = null;
            this.cardTotalQuantity.ChangeColor = System.Drawing.Color.Green;
            this.cardTotalQuantity.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold);
            this.cardTotalQuantity.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardTotalQuantity.Icon = null;
            this.cardTotalQuantity.Location = new System.Drawing.Point(10, 4);
            this.cardTotalQuantity.Margin = new System.Windows.Forms.Padding(10, 4, 10, 4);
            this.cardTotalQuantity.MaximumSize = new System.Drawing.Size(260, 150);
            this.cardTotalQuantity.Name = "cardTotalQuantity";
            this.cardTotalQuantity.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.cardTotalQuantity.Size = new System.Drawing.Size(260, 150);
            this.cardTotalQuantity.SubInfo = "Đang tải...";
            this.cardTotalQuantity.SubInfoColor = System.Drawing.Color.Gray;
            this.cardTotalQuantity.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardTotalQuantity.TabIndex = 4;
            this.cardTotalQuantity.Tag = "TotalQuantity";
            this.cardTotalQuantity.Title = "Tổng số lượng";
            this.cardTotalQuantity.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.cardTotalQuantity.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.cardTotalQuantity.Value = "0";
            this.cardTotalQuantity.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.cardTotalQuantity.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            // 
            // cardWaste
            // 
            this.cardWaste.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.cardWaste.BackColor = System.Drawing.Color.Transparent;
            this.cardWaste.CardColor = System.Drawing.Color.White;
            this.cardWaste.Change = null;
            this.cardWaste.ChangeColor = System.Drawing.Color.Green;
            this.cardWaste.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold);
            this.cardWaste.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardWaste.Icon = null;
            this.cardWaste.Location = new System.Drawing.Point(290, 4);
            this.cardWaste.Margin = new System.Windows.Forms.Padding(10, 4, 10, 4);
            this.cardWaste.MaximumSize = new System.Drawing.Size(260, 150);
            this.cardWaste.Name = "cardWaste";
            this.cardWaste.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.cardWaste.Size = new System.Drawing.Size(260, 150);
            this.cardWaste.SubInfo = "Đang tải...";
            this.cardWaste.SubInfoColor = System.Drawing.Color.Gray;
            this.cardWaste.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardWaste.TabIndex = 3;
            this.cardWaste.Tag = "Waste";
            this.cardWaste.Title = "Hao hụt";
            this.cardWaste.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.cardWaste.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.cardWaste.Value = "0";
            this.cardWaste.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.cardWaste.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            // 
            // cardTransfer
            // 
            this.cardTransfer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.cardTransfer.BackColor = System.Drawing.Color.Transparent;
            this.cardTransfer.CardColor = System.Drawing.Color.White;
            this.cardTransfer.Change = null;
            this.cardTransfer.ChangeColor = System.Drawing.Color.Green;
            this.cardTransfer.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold);
            this.cardTransfer.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardTransfer.Icon = null;
            this.cardTransfer.Location = new System.Drawing.Point(570, 4);
            this.cardTransfer.Margin = new System.Windows.Forms.Padding(10, 4, 10, 4);
            this.cardTransfer.MaximumSize = new System.Drawing.Size(260, 150);
            this.cardTransfer.Name = "cardTransfer";
            this.cardTransfer.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.cardTransfer.Size = new System.Drawing.Size(260, 150);
            this.cardTransfer.SubInfo = "Đang tải...";
            this.cardTransfer.SubInfoColor = System.Drawing.Color.Gray;
            this.cardTransfer.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardTransfer.TabIndex = 2;
            this.cardTransfer.Tag = "Transfer";
            this.cardTransfer.Title = "Chuyển kho";
            this.cardTransfer.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.cardTransfer.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.cardTransfer.Value = "0";
            this.cardTransfer.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.cardTransfer.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            // 
            // cardSale
            // 
            this.cardSale.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.cardSale.BackColor = System.Drawing.Color.Transparent;
            this.cardSale.CardColor = System.Drawing.Color.White;
            this.cardSale.Change = null;
            this.cardSale.ChangeColor = System.Drawing.Color.Green;
            this.cardSale.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold);
            this.cardSale.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardSale.Icon = null;
            this.cardSale.Location = new System.Drawing.Point(850, 4);
            this.cardSale.Margin = new System.Windows.Forms.Padding(10, 4, 10, 4);
            this.cardSale.MaximumSize = new System.Drawing.Size(260, 150);
            this.cardSale.Name = "cardSale";
            this.cardSale.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.cardSale.Size = new System.Drawing.Size(260, 150);
            this.cardSale.SubInfo = "Đang tải...";
            this.cardSale.SubInfoColor = System.Drawing.Color.Gray;
            this.cardSale.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardSale.TabIndex = 1;
            this.cardSale.Tag = "Sale";
            this.cardSale.Title = "Bán hàng";
            this.cardSale.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.cardSale.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.cardSale.Value = "0";
            this.cardSale.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.cardSale.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            // 
            // cardTotalStockOuts
            // 
            this.cardTotalStockOuts.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.cardTotalStockOuts.BackColor = System.Drawing.Color.Transparent;
            this.cardTotalStockOuts.CardColor = System.Drawing.Color.White;
            this.cardTotalStockOuts.Change = null;
            this.cardTotalStockOuts.ChangeColor = System.Drawing.Color.Green;
            this.cardTotalStockOuts.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold);
            this.cardTotalStockOuts.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardTotalStockOuts.Icon = null;
            this.cardTotalStockOuts.Location = new System.Drawing.Point(10, 162);
            this.cardTotalStockOuts.Margin = new System.Windows.Forms.Padding(10, 4, 10, 4);
            this.cardTotalStockOuts.MaximumSize = new System.Drawing.Size(260, 150);
            this.cardTotalStockOuts.Name = "cardTotalStockOuts";
            this.cardTotalStockOuts.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.cardTotalStockOuts.Size = new System.Drawing.Size(260, 150);
            this.cardTotalStockOuts.SubInfo = "Đang tải...";
            this.cardTotalStockOuts.SubInfoColor = System.Drawing.Color.Gray;
            this.cardTotalStockOuts.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardTotalStockOuts.TabIndex = 0;
            this.cardTotalStockOuts.Tag = "TotalStockOuts";
            this.cardTotalStockOuts.Title = "Tổng phiếu xuất";
            this.cardTotalStockOuts.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.cardTotalStockOuts.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.cardTotalStockOuts.Value = "0";
            this.cardTotalStockOuts.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.cardTotalStockOuts.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            // 
            // guna2PanelHeader
            // 
            this.guna2PanelHeader.AutoSize = true;
            this.guna2PanelHeader.BackColor = System.Drawing.Color.White;
            this.guna2PanelHeader.Controls.Add(this.lblDescription);
            this.guna2PanelHeader.Controls.Add(this.lblTitle);
            this.guna2PanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2PanelHeader.Location = new System.Drawing.Point(20, 16);
            this.guna2PanelHeader.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.guna2PanelHeader.Name = "guna2PanelHeader";
            this.guna2PanelHeader.Size = new System.Drawing.Size(1160, 62);
            this.guna2PanelHeader.TabIndex = 0;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDescription.ForeColor = System.Drawing.Color.Gray;
            this.lblDescription.Location = new System.Drawing.Point(15, 42);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(446, 20);
            this.lblDescription.TabIndex = 1;
            this.lblDescription.Text = "Theo dõi và xử lý phiếu xuất cho bán hàng, chuyển kho và hao hụt.";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(236, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Quản lý Xuất kho";
            // 
            // StockOutManagementControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMain);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "StockOutManagementControl";
            this.Size = new System.Drawing.Size(1200, 821);
            this.Load += new System.EventHandler(this.StockOutManagementControl_Load);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockOut)).EndInit();
            this.panelToolbar.ResumeLayout(false);
            this.panelFilter.ResumeLayout(false);
            this.panelFilter.PerformLayout();
            this.panelStatsCards.ResumeLayout(false);
            this.guna2PanelHeader.ResumeLayout(false);
            this.guna2PanelHeader.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}

