using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    partial class InventoryControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelMain = new Guna.UI2.WinForms.Guna2Panel();
            this.panelContent = new Guna.UI2.WinForms.Guna2Panel();
            this.dataGridViewPackaging = new Guna.UI2.WinForms.Guna2DataGridView();
            this.dataGridViewProducts = new Guna.UI2.WinForms.Guna2DataGridView();
            this.panelToolbar = new Guna.UI2.WinForms.Guna2Panel();
            this.btnRefresh = new Guna.UI2.WinForms.Guna2Button();
            this.btnExportPDF = new Guna.UI2.WinForms.Guna2Button();
            this.btnStockOut = new Guna.UI2.WinForms.Guna2Button();
            this.btnStockIn = new Guna.UI2.WinForms.Guna2Button();
            this.panelFilter = new Guna.UI2.WinForms.Guna2Panel();
            this.cmbStatusFilter = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblStatusFilter = new System.Windows.Forms.Label();
            this.cmbCategoryFilter = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblCategoryFilter = new System.Windows.Forms.Label();
            this.txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.panelToggle = new Guna.UI2.WinForms.Guna2Panel();
            this.btnPackaging = new Guna.UI2.WinForms.Guna2Button();
            this.btnProducts = new Guna.UI2.WinForms.Guna2Button();
            this.panelStatsCards = new System.Windows.Forms.FlowLayoutPanel();
            this.cardExpired = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.cardLowStock = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.cardTotalQty = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.cardBatches = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.guna2PanelHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelMain.SuspendLayout();
            this.panelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPackaging)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProducts)).BeginInit();
            this.panelToolbar.SuspendLayout();
            this.panelFilter.SuspendLayout();
            this.panelToggle.SuspendLayout();
            this.panelStatsCards.SuspendLayout();
            this.guna2PanelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.AutoScroll = true;
            this.panelMain.BackColor = System.Drawing.Color.White;
            this.panelMain.Controls.Add(this.panelContent);
            this.panelMain.Controls.Add(this.panelToolbar);
            this.panelMain.Controls.Add(this.panelFilter);
            this.panelMain.Controls.Add(this.panelToggle);
            this.panelMain.Controls.Add(this.panelStatsCards);
            this.panelMain.Controls.Add(this.guna2PanelHeader);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(20);
            this.panelMain.Size = new System.Drawing.Size(1200, 700);
            this.panelMain.TabIndex = 0;
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.White;
            this.panelContent.Controls.Add(this.dataGridViewPackaging);
            this.panelContent.Controls.Add(this.dataGridViewProducts);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelContent.Location = new System.Drawing.Point(20, 427);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(1160, 270);
            this.panelContent.TabIndex = 4;
            // 
            // dataGridViewPackaging
            // 
            this.dataGridViewPackaging.AllowUserToAddRows = false;
            this.dataGridViewPackaging.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dataGridViewPackaging.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewPackaging.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewPackaging.ColumnHeadersHeight = 40;
            this.dataGridViewPackaging.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(248)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewPackaging.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewPackaging.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewPackaging.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dataGridViewPackaging.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewPackaging.Name = "dataGridViewPackaging";
            this.dataGridViewPackaging.ReadOnly = true;
            this.dataGridViewPackaging.RowHeadersVisible = false;
            this.dataGridViewPackaging.RowHeadersWidth = 51;
            this.dataGridViewPackaging.Size = new System.Drawing.Size(1160, 270);
            this.dataGridViewPackaging.TabIndex = 1;
            this.dataGridViewPackaging.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewPackaging.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dataGridViewPackaging.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dataGridViewPackaging.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dataGridViewPackaging.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dataGridViewPackaging.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewPackaging.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dataGridViewPackaging.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.dataGridViewPackaging.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridViewPackaging.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.dataGridViewPackaging.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dataGridViewPackaging.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dataGridViewPackaging.ThemeStyle.HeaderStyle.Height = 40;
            this.dataGridViewPackaging.ThemeStyle.ReadOnly = true;
            this.dataGridViewPackaging.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewPackaging.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridViewPackaging.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.dataGridViewPackaging.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.dataGridViewPackaging.ThemeStyle.RowsStyle.Height = 22;
            this.dataGridViewPackaging.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(248)))), ((int)(((byte)(243)))));
            this.dataGridViewPackaging.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.dataGridViewPackaging.Visible = false;
            this.dataGridViewPackaging.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewPackaging_CellClick);
            // 
            // dataGridViewProducts
            // 
            this.dataGridViewProducts.AllowUserToAddRows = false;
            this.dataGridViewProducts.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.dataGridViewProducts.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewProducts.ColumnHeadersHeight = 40;
            this.dataGridViewProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(248)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewProducts.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewProducts.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dataGridViewProducts.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewProducts.Name = "dataGridViewProducts";
            this.dataGridViewProducts.ReadOnly = true;
            this.dataGridViewProducts.RowHeadersVisible = false;
            this.dataGridViewProducts.RowHeadersWidth = 51;
            this.dataGridViewProducts.Size = new System.Drawing.Size(1160, 270);
            this.dataGridViewProducts.TabIndex = 0;
            this.dataGridViewProducts.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewProducts.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dataGridViewProducts.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dataGridViewProducts.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dataGridViewProducts.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dataGridViewProducts.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewProducts.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dataGridViewProducts.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.dataGridViewProducts.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridViewProducts.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.dataGridViewProducts.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dataGridViewProducts.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dataGridViewProducts.ThemeStyle.HeaderStyle.Height = 40;
            this.dataGridViewProducts.ThemeStyle.ReadOnly = true;
            this.dataGridViewProducts.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewProducts.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridViewProducts.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.dataGridViewProducts.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.dataGridViewProducts.ThemeStyle.RowsStyle.Height = 22;
            this.dataGridViewProducts.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(248)))), ((int)(((byte)(243)))));
            this.dataGridViewProducts.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.dataGridViewProducts.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewProducts_CellClick);
            // 
            // panelToolbar
            // 
            this.panelToolbar.BackColor = System.Drawing.Color.White;
            this.panelToolbar.Controls.Add(this.btnRefresh);
            this.panelToolbar.Controls.Add(this.btnExportPDF);
            this.panelToolbar.Controls.Add(this.btnStockOut);
            this.panelToolbar.Controls.Add(this.btnStockIn);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolbar.Location = new System.Drawing.Point(20, 365);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.panelToolbar.Size = new System.Drawing.Size(1160, 62);
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
            this.btnRefresh.Location = new System.Drawing.Point(450, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(120, 42);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnExportPDF
            // 
            this.btnExportPDF.BorderRadius = 5;
            this.btnExportPDF.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnExportPDF.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnExportPDF.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnExportPDF.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnExportPDF.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(24)))), ((int)(((byte)(61)))));
            this.btnExportPDF.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnExportPDF.ForeColor = System.Drawing.Color.White;
            this.btnExportPDF.Location = new System.Drawing.Point(300, 10);
            this.btnExportPDF.Name = "btnExportPDF";
            this.btnExportPDF.Size = new System.Drawing.Size(140, 42);
            this.btnExportPDF.TabIndex = 2;
            this.btnExportPDF.Text = "Xuất PDF";
            this.btnExportPDF.Click += new System.EventHandler(this.btnExportPDF_Click);
            // 
            // btnStockOut
            // 
            this.btnStockOut.BorderRadius = 5;
            this.btnStockOut.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnStockOut.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnStockOut.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnStockOut.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnStockOut.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(115)))), ((int)(((byte)(22)))));
            this.btnStockOut.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnStockOut.ForeColor = System.Drawing.Color.White;
            this.btnStockOut.Location = new System.Drawing.Point(150, 10);
            this.btnStockOut.Name = "btnStockOut";
            this.btnStockOut.Size = new System.Drawing.Size(140, 42);
            this.btnStockOut.TabIndex = 1;
            this.btnStockOut.Text = "Xuất kho";
            this.btnStockOut.Click += new System.EventHandler(this.btnStockOut_Click);
            // 
            // btnStockIn
            // 
            this.btnStockIn.BorderRadius = 5;
            this.btnStockIn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnStockIn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnStockIn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnStockIn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnStockIn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.btnStockIn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnStockIn.ForeColor = System.Drawing.Color.White;
            this.btnStockIn.Location = new System.Drawing.Point(0, 10);
            this.btnStockIn.Name = "btnStockIn";
            this.btnStockIn.Size = new System.Drawing.Size(140, 42);
            this.btnStockIn.TabIndex = 0;
            this.btnStockIn.Text = "Nhập kho";
            this.btnStockIn.Click += new System.EventHandler(this.btnStockIn_Click);
            // 
            // panelFilter
            // 
            this.panelFilter.BackColor = System.Drawing.Color.Transparent;
            this.panelFilter.Controls.Add(this.cmbStatusFilter);
            this.panelFilter.Controls.Add(this.lblStatusFilter);
            this.panelFilter.Controls.Add(this.cmbCategoryFilter);
            this.panelFilter.Controls.Add(this.lblCategoryFilter);
            this.panelFilter.Controls.Add(this.txtSearch);
            this.panelFilter.Controls.Add(this.lblSearch);
            this.panelFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilter.Location = new System.Drawing.Point(20, 305);
            this.panelFilter.Name = "panelFilter";
            this.panelFilter.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.panelFilter.Size = new System.Drawing.Size(1160, 60);
            this.panelFilter.TabIndex = 5;
            // 
            // cmbStatusFilter
            // 
            this.cmbStatusFilter.BackColor = System.Drawing.Color.Transparent;
            this.cmbStatusFilter.BorderRadius = 5;
            this.cmbStatusFilter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbStatusFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatusFilter.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbStatusFilter.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbStatusFilter.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbStatusFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbStatusFilter.ItemHeight = 30;
            this.cmbStatusFilter.Location = new System.Drawing.Point(590, 25);
            this.cmbStatusFilter.Name = "cmbStatusFilter";
            this.cmbStatusFilter.Size = new System.Drawing.Size(200, 36);
            this.cmbStatusFilter.TabIndex = 4;
            this.cmbStatusFilter.SelectedIndexChanged += new System.EventHandler(this.cmbStatusFilter_SelectedIndexChanged);
            // 
            // lblStatusFilter
            // 
            this.lblStatusFilter.AutoSize = true;
            this.lblStatusFilter.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatusFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblStatusFilter.Location = new System.Drawing.Point(590, 5);
            this.lblStatusFilter.Name = "lblStatusFilter";
            this.lblStatusFilter.Size = new System.Drawing.Size(84, 20);
            this.lblStatusFilter.TabIndex = 5;
            this.lblStatusFilter.Text = "Trạng thái:";
            // 
            // cmbCategoryFilter
            // 
            this.cmbCategoryFilter.BackColor = System.Drawing.Color.Transparent;
            this.cmbCategoryFilter.BorderRadius = 5;
            this.cmbCategoryFilter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCategoryFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategoryFilter.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbCategoryFilter.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbCategoryFilter.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbCategoryFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbCategoryFilter.ItemHeight = 30;
            this.cmbCategoryFilter.Location = new System.Drawing.Point(320, 25);
            this.cmbCategoryFilter.Name = "cmbCategoryFilter";
            this.cmbCategoryFilter.Size = new System.Drawing.Size(250, 36);
            this.cmbCategoryFilter.TabIndex = 2;
            this.cmbCategoryFilter.SelectedIndexChanged += new System.EventHandler(this.cmbCategoryFilter_SelectedIndexChanged);
            // 
            // lblCategoryFilter
            // 
            this.lblCategoryFilter.AutoSize = true;
            this.lblCategoryFilter.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCategoryFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblCategoryFilter.Location = new System.Drawing.Point(320, 5);
            this.lblCategoryFilter.Name = "lblCategoryFilter";
            this.lblCategoryFilter.Size = new System.Drawing.Size(96, 20);
            this.lblCategoryFilter.TabIndex = 3;
            this.lblCategoryFilter.Text = "Nhóm hàng:";
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
            this.txtSearch.Location = new System.Drawing.Point(0, 25);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceholderText = "Tìm kiếm theo tên sản phẩm...";
            this.txtSearch.SelectedText = "";
            this.txtSearch.Size = new System.Drawing.Size(300, 30);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblSearch.Location = new System.Drawing.Point(0, 5);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(78, 20);
            this.lblSearch.TabIndex = 1;
            this.lblSearch.Text = "Tìm kiếm:";
            // 
            // panelToggle
            // 
            this.panelToggle.BackColor = System.Drawing.Color.White;
            this.panelToggle.Controls.Add(this.btnPackaging);
            this.panelToggle.Controls.Add(this.btnProducts);
            this.panelToggle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToggle.Location = new System.Drawing.Point(20, 265);
            this.panelToggle.Name = "panelToggle";
            this.panelToggle.Padding = new System.Windows.Forms.Padding(10, 15, 10, 15);
            this.panelToggle.Size = new System.Drawing.Size(1160, 40);
            this.panelToggle.TabIndex = 1;
            // 
            // btnPackaging
            // 
            this.btnPackaging.BorderRadius = 5;
            this.btnPackaging.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnPackaging.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnPackaging.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnPackaging.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnPackaging.FillColor = System.Drawing.Color.White;
            this.btnPackaging.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnPackaging.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnPackaging.Location = new System.Drawing.Point(150, 0);
            this.btnPackaging.Name = "btnPackaging";
            this.btnPackaging.Size = new System.Drawing.Size(150, 40);
            this.btnPackaging.TabIndex = 1;
            this.btnPackaging.Text = "Bao bì";
            this.btnPackaging.Click += new System.EventHandler(this.btnPackaging_Click);
            // 
            // btnProducts
            // 
            this.btnProducts.BorderRadius = 5;
            this.btnProducts.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnProducts.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnProducts.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnProducts.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnProducts.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.btnProducts.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnProducts.ForeColor = System.Drawing.Color.White;
            this.btnProducts.Location = new System.Drawing.Point(0, 0);
            this.btnProducts.Name = "btnProducts";
            this.btnProducts.Size = new System.Drawing.Size(150, 40);
            this.btnProducts.TabIndex = 0;
            this.btnProducts.Text = "Sản phẩm";
            this.btnProducts.Click += new System.EventHandler(this.btnProducts_Click);
            // 
            // panelStatsCards
            // 
            this.panelStatsCards.AutoSize = true;
            this.panelStatsCards.BackColor = System.Drawing.Color.Transparent;
            this.panelStatsCards.Controls.Add(this.cardExpired);
            this.panelStatsCards.Controls.Add(this.cardLowStock);
            this.panelStatsCards.Controls.Add(this.cardTotalQty);
            this.panelStatsCards.Controls.Add(this.cardBatches);
            this.panelStatsCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelStatsCards.Location = new System.Drawing.Point(20, 95);
            this.panelStatsCards.Name = "panelStatsCards";
            this.panelStatsCards.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.panelStatsCards.Size = new System.Drawing.Size(1160, 170);
            this.panelStatsCards.TabIndex = 2;
            // 
            // cardExpired
            // 
            this.cardExpired.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.cardExpired.BackColor = System.Drawing.Color.Transparent;
            this.cardExpired.CardColor = System.Drawing.Color.White;
            this.cardExpired.Change = null;
            this.cardExpired.ChangeColor = System.Drawing.Color.Green;
            this.cardExpired.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardExpired.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardExpired.Icon = null;
            this.cardExpired.Location = new System.Drawing.Point(10, 5);
            this.cardExpired.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.cardExpired.MaximumSize = new System.Drawing.Size(260, 150);
            this.cardExpired.Name = "cardExpired";
            this.cardExpired.Padding = new System.Windows.Forms.Padding(10);
            this.cardExpired.Size = new System.Drawing.Size(260, 150);
            this.cardExpired.SubInfo = "Đang tải...";
            this.cardExpired.SubInfoColor = System.Drawing.Color.Gray;
            this.cardExpired.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardExpired.TabIndex = 3;
            this.cardExpired.Tag = "ProductExpired";
            this.cardExpired.Title = "Hết hạn";
            this.cardExpired.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.cardExpired.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardExpired.Value = "0";
            this.cardExpired.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.cardExpired.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            // 
            // cardLowStock
            // 
            this.cardLowStock.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.cardLowStock.BackColor = System.Drawing.Color.Transparent;
            this.cardLowStock.CardColor = System.Drawing.Color.White;
            this.cardLowStock.Change = null;
            this.cardLowStock.ChangeColor = System.Drawing.Color.Green;
            this.cardLowStock.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardLowStock.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardLowStock.Icon = null;
            this.cardLowStock.Location = new System.Drawing.Point(290, 5);
            this.cardLowStock.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.cardLowStock.MaximumSize = new System.Drawing.Size(260, 150);
            this.cardLowStock.Name = "cardLowStock";
            this.cardLowStock.Padding = new System.Windows.Forms.Padding(10);
            this.cardLowStock.Size = new System.Drawing.Size(260, 150);
            this.cardLowStock.SubInfo = "Đang tải...";
            this.cardLowStock.SubInfoColor = System.Drawing.Color.Gray;
            this.cardLowStock.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardLowStock.TabIndex = 2;
            this.cardLowStock.Tag = "ProductLowStock";
            this.cardLowStock.Title = "Sắp hết";
            this.cardLowStock.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.cardLowStock.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardLowStock.Value = "0";
            this.cardLowStock.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.cardLowStock.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            // 
            // cardTotalQty
            // 
            this.cardTotalQty.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.cardTotalQty.BackColor = System.Drawing.Color.Transparent;
            this.cardTotalQty.CardColor = System.Drawing.Color.White;
            this.cardTotalQty.Change = null;
            this.cardTotalQty.ChangeColor = System.Drawing.Color.Green;
            this.cardTotalQty.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardTotalQty.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardTotalQty.Icon = null;
            this.cardTotalQty.Location = new System.Drawing.Point(570, 5);
            this.cardTotalQty.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.cardTotalQty.MaximumSize = new System.Drawing.Size(260, 150);
            this.cardTotalQty.Name = "cardTotalQty";
            this.cardTotalQty.Padding = new System.Windows.Forms.Padding(10);
            this.cardTotalQty.Size = new System.Drawing.Size(260, 150);
            this.cardTotalQty.SubInfo = "Đang tải...";
            this.cardTotalQty.SubInfoColor = System.Drawing.Color.Gray;
            this.cardTotalQty.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardTotalQty.TabIndex = 1;
            this.cardTotalQty.Tag = "ProductTotalQty";
            this.cardTotalQty.Title = "Tổng số lượng";
            this.cardTotalQty.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.cardTotalQty.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardTotalQty.Value = "0";
            this.cardTotalQty.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.cardTotalQty.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            // 
            // cardBatches
            // 
            this.cardBatches.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.cardBatches.BackColor = System.Drawing.Color.Transparent;
            this.cardBatches.CardColor = System.Drawing.Color.White;
            this.cardBatches.Change = null;
            this.cardBatches.ChangeColor = System.Drawing.Color.Green;
            this.cardBatches.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardBatches.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardBatches.Icon = null;
            this.cardBatches.Location = new System.Drawing.Point(850, 5);
            this.cardBatches.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.cardBatches.MaximumSize = new System.Drawing.Size(260, 150);
            this.cardBatches.Name = "cardBatches";
            this.cardBatches.Padding = new System.Windows.Forms.Padding(10);
            this.cardBatches.Size = new System.Drawing.Size(260, 150);
            this.cardBatches.SubInfo = "Đang tải...";
            this.cardBatches.SubInfoColor = System.Drawing.Color.Gray;
            this.cardBatches.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardBatches.TabIndex = 0;
            this.cardBatches.Tag = "ProductBatches";
            this.cardBatches.Title = "Tổng số lô";
            this.cardBatches.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.cardBatches.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardBatches.Value = "0";
            this.cardBatches.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.cardBatches.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
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
            this.guna2PanelHeader.Size = new System.Drawing.Size(1160, 75);
            this.guna2PanelHeader.TabIndex = 0;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDescription.ForeColor = System.Drawing.Color.Gray;
            this.lblDescription.Location = new System.Drawing.Point(0, 55);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(463, 20);
            this.lblDescription.TabIndex = 1;
            this.lblDescription.Text = "Quản lý tồn kho sản phẩm và bao bì, theo dõi số lượng, hạn sử dụng.";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(0, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(220, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Quản lý tồn kho";
            // 
            // InventoryControl
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.panelMain);
            this.Name = "InventoryControl";
            this.Size = new System.Drawing.Size(1200, 700);
            this.Load += new System.EventHandler(this.InventoryControl_Load);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.panelContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPackaging)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProducts)).EndInit();
            this.panelToolbar.ResumeLayout(false);
            this.panelFilter.ResumeLayout(false);
            this.panelFilter.PerformLayout();
            this.panelToggle.ResumeLayout(false);
            this.panelStatsCards.ResumeLayout(false);
            this.guna2PanelHeader.ResumeLayout(false);
            this.guna2PanelHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna2Panel panelMain;
        private Guna2Panel guna2PanelHeader;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblTitle;
        private Guna2Panel panelToggle;
        private Guna2Button btnProducts;
        private Guna2Button btnPackaging;
        private Guna2Panel panelToolbar;
        private Guna2Button btnStockIn;
        private Guna2Button btnStockOut;
        private Guna2Button btnExportPDF;
        private Guna2Button btnRefresh;
        private Guna2Panel panelContent;
        private Guna2DataGridView dataGridViewProducts;
        private Guna2DataGridView dataGridViewPackaging;
        private System.Windows.Forms.FlowLayoutPanel panelStatsCards;
        private EcoStationManagerApplication.UI.Controls.CardControl cardBatches;
        private EcoStationManagerApplication.UI.Controls.CardControl cardTotalQty;
        private EcoStationManagerApplication.UI.Controls.CardControl cardLowStock;
        private EcoStationManagerApplication.UI.Controls.CardControl cardExpired;
        private Guna2Panel panelFilter;
        private Guna2TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;
        private Guna2ComboBox cmbCategoryFilter;
        private System.Windows.Forms.Label lblCategoryFilter;
        private Guna2ComboBox cmbStatusFilter;
        private System.Windows.Forms.Label lblStatusFilter;
    }
}
