using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    partial class StockInManagementControl
    {
        private System.ComponentModel.IContainer components = null;
        private Guna2Panel panelMain;
        private Guna2Panel guna2PanelHeader;
        private Label lblTitle;
        private Label lblDescription;
        private FlowLayoutPanel panelStatsCards;
        private CardControl cardTotalStockIns;
        private CardControl cardTotalQuantity;
        private CardControl cardTotalValue;
        private CardControl cardQualityPass;
        private Guna2Panel panelFilter;
        private Guna2TextBox txtSearch;
        private Label lblSearch;
        private Guna2ComboBox cmbSourceFilter;
        private Label lblSourceFilter;
        private DateTimePicker dtpFromDate;
        private Label lblFromDate;
        private DateTimePicker dtpToDate;
        private Label lblToDate;
        private Guna2Panel panelToolbar;
        private Guna2Button btnExportExcel;
        private Guna2Button btnCreateStockIn;
        private Guna2Button btnRefresh;
        private Guna2DataGridView dgvStockIn;

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
            this.dgvStockIn = new Guna.UI2.WinForms.Guna2DataGridView();
            this.panelToolbar = new Guna.UI2.WinForms.Guna2Panel();
            this.btnRefresh = new Guna.UI2.WinForms.Guna2Button();
            this.btnExportExcel = new Guna.UI2.WinForms.Guna2Button();
            this.btnCreateStockIn = new Guna.UI2.WinForms.Guna2Button();
            this.panelFilter = new Guna.UI2.WinForms.Guna2Panel();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.lblToDate = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.cmbSourceFilter = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblSourceFilter = new System.Windows.Forms.Label();
            this.txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.panelStatsCards = new System.Windows.Forms.FlowLayoutPanel();
            this.cardQualityPass = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.cardTotalValue = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.cardTotalQuantity = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.cardTotalStockIns = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.guna2PanelHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockIn)).BeginInit();
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
            this.panelMain.Controls.Add(this.dgvStockIn);
            this.panelMain.Controls.Add(this.panelToolbar);
            this.panelMain.Controls.Add(this.panelFilter);
            this.panelMain.Controls.Add(this.panelStatsCards);
            this.panelMain.Controls.Add(this.guna2PanelHeader);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(20, 16, 20, 16);
            this.panelMain.Size = new System.Drawing.Size(1200, 765);
            this.panelMain.TabIndex = 0;
            // 
            // dgvStockIn
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvStockIn.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvStockIn.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvStockIn.ColumnHeadersHeight = 40;
            this.dgvStockIn.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(248)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvStockIn.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvStockIn.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvStockIn.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvStockIn.Location = new System.Drawing.Point(20, 413);
            this.dgvStockIn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvStockIn.Name = "dgvStockIn";
            this.dgvStockIn.ReadOnly = true;
            this.dgvStockIn.RowHeadersVisible = false;
            this.dgvStockIn.RowHeadersWidth = 51;
            this.dgvStockIn.Size = new System.Drawing.Size(1139, 350);
            this.dgvStockIn.TabIndex = 4;
            this.dgvStockIn.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvStockIn.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvStockIn.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvStockIn.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvStockIn.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvStockIn.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvStockIn.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvStockIn.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.dgvStockIn.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvStockIn.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.dgvStockIn.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvStockIn.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvStockIn.ThemeStyle.HeaderStyle.Height = 40;
            this.dgvStockIn.ThemeStyle.ReadOnly = true;
            this.dgvStockIn.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvStockIn.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvStockIn.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.dgvStockIn.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.dgvStockIn.ThemeStyle.RowsStyle.Height = 22;
            this.dgvStockIn.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(248)))), ((int)(((byte)(243)))));
            this.dgvStockIn.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.dgvStockIn.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockIn_CellContentClick);
            // 
            // panelToolbar
            // 
            this.panelToolbar.BackColor = System.Drawing.Color.White;
            this.panelToolbar.Controls.Add(this.btnRefresh);
            this.panelToolbar.Controls.Add(this.btnExportExcel);
            this.panelToolbar.Controls.Add(this.btnCreateStockIn);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolbar.Location = new System.Drawing.Point(20, 352);
            this.panelToolbar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.panelToolbar.Size = new System.Drawing.Size(1139, 61);
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
            this.btnRefresh.Location = new System.Drawing.Point(335, 8);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(132, 45);
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
            this.btnExportExcel.Location = new System.Drawing.Point(174, 8);
            this.btnExportExcel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(144, 45);
            this.btnExportExcel.TabIndex = 1;
            this.btnExportExcel.Text = "Xuất Excel";
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // btnCreateStockIn
            // 
            this.btnCreateStockIn.BorderRadius = 5;
            this.btnCreateStockIn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCreateStockIn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCreateStockIn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCreateStockIn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCreateStockIn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.btnCreateStockIn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnCreateStockIn.ForeColor = System.Drawing.Color.White;
            this.btnCreateStockIn.Location = new System.Drawing.Point(0, 8);
            this.btnCreateStockIn.Margin = new System.Windows.Forms.Padding(3, 2, 15, 2);
            this.btnCreateStockIn.Name = "btnCreateStockIn";
            this.btnCreateStockIn.Size = new System.Drawing.Size(160, 45);
            this.btnCreateStockIn.TabIndex = 0;
            this.btnCreateStockIn.Text = "Tạo phiếu nhập";
            this.btnCreateStockIn.Click += new System.EventHandler(this.btnCreateStockIn_Click);
            // 
            // panelFilter
            // 
            this.panelFilter.BackColor = System.Drawing.Color.Transparent;
            this.panelFilter.Controls.Add(this.dtpToDate);
            this.panelFilter.Controls.Add(this.lblToDate);
            this.panelFilter.Controls.Add(this.dtpFromDate);
            this.panelFilter.Controls.Add(this.lblFromDate);
            this.panelFilter.Controls.Add(this.cmbSourceFilter);
            this.panelFilter.Controls.Add(this.lblSourceFilter);
            this.panelFilter.Controls.Add(this.txtSearch);
            this.panelFilter.Controls.Add(this.lblSearch);
            this.panelFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilter.Location = new System.Drawing.Point(20, 274);
            this.panelFilter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelFilter.Name = "panelFilter";
            this.panelFilter.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.panelFilter.Size = new System.Drawing.Size(1139, 78);
            this.panelFilter.TabIndex = 2;
            // 
            // dtpToDate
            // 
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpToDate.Location = new System.Drawing.Point(778, 42);
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
            this.lblToDate.Location = new System.Drawing.Point(774, 20);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(79, 20);
            this.lblToDate.TabIndex = 5;
            this.lblToDate.Text = "Đến ngày:";
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFromDate.Location = new System.Drawing.Point(589, 42);
            this.dtpFromDate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(164, 22);
            this.dtpFromDate.TabIndex = 4;
            this.dtpFromDate.ValueChanged += new System.EventHandler(this.dtpFromDate_ValueChanged);
            // 
            // lblFromDate
            // 
            this.lblFromDate.AutoSize = true;
            this.lblFromDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblFromDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblFromDate.Location = new System.Drawing.Point(585, 20);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(70, 20);
            this.lblFromDate.TabIndex = 3;
            this.lblFromDate.Text = "Từ ngày:";
            // 
            // cmbSourceFilter
            // 
            this.cmbSourceFilter.BackColor = System.Drawing.Color.Transparent;
            this.cmbSourceFilter.BorderRadius = 5;
            this.cmbSourceFilter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSourceFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSourceFilter.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbSourceFilter.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbSourceFilter.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbSourceFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbSourceFilter.ItemHeight = 30;
            this.cmbSourceFilter.Location = new System.Drawing.Point(324, 28);
            this.cmbSourceFilter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbSourceFilter.Name = "cmbSourceFilter";
            this.cmbSourceFilter.Size = new System.Drawing.Size(250, 36);
            this.cmbSourceFilter.TabIndex = 2;
            this.cmbSourceFilter.SelectedIndexChanged += new System.EventHandler(this.cmbSourceFilter_SelectedIndexChanged);
            // 
            // lblSourceFilter
            // 
            this.lblSourceFilter.AutoSize = true;
            this.lblSourceFilter.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSourceFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblSourceFilter.Location = new System.Drawing.Point(320, 4);
            this.lblSourceFilter.Name = "lblSourceFilter";
            this.lblSourceFilter.Size = new System.Drawing.Size(100, 20);
            this.lblSourceFilter.TabIndex = 3;
            this.lblSourceFilter.Text = "Nguồn nhập:";
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
            this.txtSearch.Location = new System.Drawing.Point(4, 28);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceholderText = "Tìm kiếm theo lô hàng, sản phẩm...";
            this.txtSearch.SelectedText = "";
            this.txtSearch.Size = new System.Drawing.Size(300, 44);
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
            this.panelStatsCards.Controls.Add(this.cardQualityPass);
            this.panelStatsCards.Controls.Add(this.cardTotalValue);
            this.panelStatsCards.Controls.Add(this.cardTotalQuantity);
            this.panelStatsCards.Controls.Add(this.cardTotalStockIns);
            this.panelStatsCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelStatsCards.Location = new System.Drawing.Point(20, 108);
            this.panelStatsCards.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelStatsCards.Name = "panelStatsCards";
            this.panelStatsCards.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.panelStatsCards.Size = new System.Drawing.Size(1139, 166);
            this.panelStatsCards.TabIndex = 1;
            // 
            // cardQualityPass
            // 
            this.cardQualityPass.BackColor = System.Drawing.Color.Transparent;
            this.cardQualityPass.CardColor = System.Drawing.Color.White;
            this.cardQualityPass.Change = null;
            this.cardQualityPass.ChangeColor = System.Drawing.Color.Green;
            this.cardQualityPass.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold);
            this.cardQualityPass.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardQualityPass.Icon = null;
            this.cardQualityPass.Location = new System.Drawing.Point(10, 4);
            this.cardQualityPass.Margin = new System.Windows.Forms.Padding(10, 4, 10, 4);
            this.cardQualityPass.MaximumSize = new System.Drawing.Size(260, 150);
            this.cardQualityPass.Name = "cardQualityPass";
            this.cardQualityPass.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.cardQualityPass.Size = new System.Drawing.Size(260, 150);
            this.cardQualityPass.SubInfo = "Đang tải...";
            this.cardQualityPass.SubInfoColor = System.Drawing.Color.Gray;
            this.cardQualityPass.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardQualityPass.TabIndex = 3;
            this.cardQualityPass.Tag = "QualityPass";
            this.cardQualityPass.Title = "Phiếu mới";
            this.cardQualityPass.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.cardQualityPass.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.cardQualityPass.Value = "0";
            this.cardQualityPass.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.cardQualityPass.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            // 
            // cardTotalValue
            // 
            this.cardTotalValue.BackColor = System.Drawing.Color.Transparent;
            this.cardTotalValue.CardColor = System.Drawing.Color.White;
            this.cardTotalValue.Change = null;
            this.cardTotalValue.ChangeColor = System.Drawing.Color.Green;
            this.cardTotalValue.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold);
            this.cardTotalValue.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardTotalValue.Icon = null;
            this.cardTotalValue.Location = new System.Drawing.Point(290, 4);
            this.cardTotalValue.Margin = new System.Windows.Forms.Padding(10, 4, 10, 4);
            this.cardTotalValue.MaximumSize = new System.Drawing.Size(260, 150);
            this.cardTotalValue.Name = "cardTotalValue";
            this.cardTotalValue.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.cardTotalValue.Size = new System.Drawing.Size(260, 150);
            this.cardTotalValue.SubInfo = "Đang tải...";
            this.cardTotalValue.SubInfoColor = System.Drawing.Color.Gray;
            this.cardTotalValue.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardTotalValue.TabIndex = 2;
            this.cardTotalValue.Tag = "TotalValue";
            this.cardTotalValue.Title = "Tổng giá trị";
            this.cardTotalValue.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.cardTotalValue.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.cardTotalValue.Value = "0";
            this.cardTotalValue.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.cardTotalValue.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            // 
            // cardTotalQuantity
            // 
            this.cardTotalQuantity.BackColor = System.Drawing.Color.Transparent;
            this.cardTotalQuantity.CardColor = System.Drawing.Color.White;
            this.cardTotalQuantity.Change = null;
            this.cardTotalQuantity.ChangeColor = System.Drawing.Color.Green;
            this.cardTotalQuantity.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold);
            this.cardTotalQuantity.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardTotalQuantity.Icon = null;
            this.cardTotalQuantity.Location = new System.Drawing.Point(570, 4);
            this.cardTotalQuantity.Margin = new System.Windows.Forms.Padding(10, 4, 10, 4);
            this.cardTotalQuantity.MaximumSize = new System.Drawing.Size(260, 150);
            this.cardTotalQuantity.Name = "cardTotalQuantity";
            this.cardTotalQuantity.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.cardTotalQuantity.Size = new System.Drawing.Size(260, 150);
            this.cardTotalQuantity.SubInfo = "Đang tải...";
            this.cardTotalQuantity.SubInfoColor = System.Drawing.Color.Gray;
            this.cardTotalQuantity.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardTotalQuantity.TabIndex = 1;
            this.cardTotalQuantity.Tag = "TotalQuantity";
            this.cardTotalQuantity.Title = "Tổng số lượng";
            this.cardTotalQuantity.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.cardTotalQuantity.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.cardTotalQuantity.Value = "0";
            this.cardTotalQuantity.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.cardTotalQuantity.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            // 
            // cardTotalStockIns
            // 
            this.cardTotalStockIns.BackColor = System.Drawing.Color.Transparent;
            this.cardTotalStockIns.CardColor = System.Drawing.Color.White;
            this.cardTotalStockIns.Change = null;
            this.cardTotalStockIns.ChangeColor = System.Drawing.Color.Green;
            this.cardTotalStockIns.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold);
            this.cardTotalStockIns.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardTotalStockIns.Icon = null;
            this.cardTotalStockIns.Location = new System.Drawing.Point(850, 4);
            this.cardTotalStockIns.Margin = new System.Windows.Forms.Padding(10, 4, 10, 4);
            this.cardTotalStockIns.MaximumSize = new System.Drawing.Size(260, 150);
            this.cardTotalStockIns.Name = "cardTotalStockIns";
            this.cardTotalStockIns.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.cardTotalStockIns.Size = new System.Drawing.Size(260, 150);
            this.cardTotalStockIns.SubInfo = "Đang tải...";
            this.cardTotalStockIns.SubInfoColor = System.Drawing.Color.Gray;
            this.cardTotalStockIns.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardTotalStockIns.TabIndex = 0;
            this.cardTotalStockIns.Tag = "TotalStockIns";
            this.cardTotalStockIns.Title = "Tổng phiếu nhập";
            this.cardTotalStockIns.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.cardTotalStockIns.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.cardTotalStockIns.Value = "0";
            this.cardTotalStockIns.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.cardTotalStockIns.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            // 
            // guna2PanelHeader
            // 
            this.guna2PanelHeader.AutoScroll = true;
            this.guna2PanelHeader.AutoSize = true;
            this.guna2PanelHeader.BackColor = System.Drawing.Color.White;
            this.guna2PanelHeader.Controls.Add(this.lblDescription);
            this.guna2PanelHeader.Controls.Add(this.lblTitle);
            this.guna2PanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2PanelHeader.Location = new System.Drawing.Point(20, 16);
            this.guna2PanelHeader.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.guna2PanelHeader.Name = "guna2PanelHeader";
            this.guna2PanelHeader.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.guna2PanelHeader.Size = new System.Drawing.Size(1139, 92);
            this.guna2PanelHeader.TabIndex = 0;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDescription.ForeColor = System.Drawing.Color.Gray;
            this.lblDescription.Location = new System.Drawing.Point(18, 42);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(317, 20);
            this.lblDescription.TabIndex = 1;
            this.lblDescription.Text = "Theo dõi và quản lý tất cả phiếu nhập vào kho.";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(15, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(245, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Quản lý Nhập kho";
            // 
            // StockInManagementControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMain);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "StockInManagementControl";
            this.Size = new System.Drawing.Size(1200, 765);
            this.Load += new System.EventHandler(this.StockInManagementControl_Load);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockIn)).EndInit();
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

