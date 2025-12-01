using Guna.UI2.WinForms;

namespace EcoStationManagerApplication.UI.Controls
{
    partial class CustomersControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            this.guna2PanelContent = new Guna.UI2.WinForms.Guna2Panel();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.dataGridViewCustomers = new Guna.UI2.WinForms.Guna2DataGridView();
            this.panelTransactions = new System.Windows.Forms.Panel();
            this.dgvPackagingTransactions = new Guna.UI2.WinForms.Guna2DataGridView();
            this.panelTransactionHeader = new System.Windows.Forms.Panel();
            this.lblTransactionCount = new System.Windows.Forms.Label();
            this.lblSelectedCustomer = new System.Windows.Forms.Label();
            this.panelFilter = new System.Windows.Forms.Panel();
            this.dtpToDate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.cmbCustomerRank = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpFromDate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExportPDF = new Guna.UI2.WinForms.Guna2Button();
            this.btnExportExcel = new Guna.UI2.WinForms.Guna2Button();
            this.btnRefresh = new Guna.UI2.WinForms.Guna2Button();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAddCustomer = new Guna.UI2.WinForms.Guna2Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.guna2PanelHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2PanelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCustomers)).BeginInit();
            this.panelTransactions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPackagingTransactions)).BeginInit();
            this.panelTransactionHeader.SuspendLayout();
            this.panelFilter.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.guna2PanelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2PanelContent
            // 
            this.guna2PanelContent.AutoScroll = true;
            this.guna2PanelContent.BackColor = System.Drawing.Color.White;
            this.guna2PanelContent.Controls.Add(this.splitContainer);
            this.guna2PanelContent.Controls.Add(this.panelFilter);
            this.guna2PanelContent.Controls.Add(this.panelSearch);
            this.guna2PanelContent.Controls.Add(this.guna2PanelHeader);
            this.guna2PanelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2PanelContent.Location = new System.Drawing.Point(0, 0);
            this.guna2PanelContent.Name = "guna2PanelContent";
            this.guna2PanelContent.Padding = new System.Windows.Forms.Padding(20);
            this.guna2PanelContent.Size = new System.Drawing.Size(1200, 872);
            this.guna2PanelContent.TabIndex = 0;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer.Location = new System.Drawing.Point(20, 220);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.dataGridViewCustomers);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.panelTransactions);
            this.splitContainer.Size = new System.Drawing.Size(1160, 652);
            this.splitContainer.SplitterDistance = 326;
            this.splitContainer.TabIndex = 4;
            // 
            // dataGridViewCustomers
            // 
            this.dataGridViewCustomers.AllowUserToAddRows = false;
            this.dataGridViewCustomers.AllowUserToDeleteRows = false;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.White;
            this.dataGridViewCustomers.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle13;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(234)))), ((int)(((byte)(237)))));
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            dataGridViewCellStyle14.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewCustomers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.dataGridViewCustomers.ColumnHeadersHeight = 40;
            this.dataGridViewCustomers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            dataGridViewCellStyle15.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCustomers.DefaultCellStyle = dataGridViewCellStyle15;
            this.dataGridViewCustomers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewCustomers.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            this.dataGridViewCustomers.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewCustomers.Margin = new System.Windows.Forms.Padding(5, 15, 5, 15);
            this.dataGridViewCustomers.Name = "dataGridViewCustomers";
            this.dataGridViewCustomers.ReadOnly = true;
            this.dataGridViewCustomers.RowHeadersVisible = false;
            this.dataGridViewCustomers.RowHeadersWidth = 51;
            this.dataGridViewCustomers.Size = new System.Drawing.Size(1160, 326);
            this.dataGridViewCustomers.TabIndex = 2;
            this.dataGridViewCustomers.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.LightGrid;
            this.dataGridViewCustomers.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewCustomers.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dataGridViewCustomers.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dataGridViewCustomers.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dataGridViewCustomers.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dataGridViewCustomers.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewCustomers.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            this.dataGridViewCustomers.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(234)))), ((int)(((byte)(237)))));
            this.dataGridViewCustomers.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridViewCustomers.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.dataGridViewCustomers.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            this.dataGridViewCustomers.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dataGridViewCustomers.ThemeStyle.HeaderStyle.Height = 40;
            this.dataGridViewCustomers.ThemeStyle.ReadOnly = true;
            this.dataGridViewCustomers.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewCustomers.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridViewCustomers.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.dataGridViewCustomers.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.Black;
            this.dataGridViewCustomers.ThemeStyle.RowsStyle.Height = 22;
            this.dataGridViewCustomers.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            this.dataGridViewCustomers.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridViewCustomers.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewCustomers_CellClick);
            // 
            // panelTransactions
            // 
            this.panelTransactions.Controls.Add(this.dgvPackagingTransactions);
            this.panelTransactions.Controls.Add(this.panelTransactionHeader);
            this.panelTransactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTransactions.Location = new System.Drawing.Point(0, 0);
            this.panelTransactions.Name = "panelTransactions";
            this.panelTransactions.Size = new System.Drawing.Size(1160, 322);
            this.panelTransactions.TabIndex = 0;
            // 
            // dgvPackagingTransactions
            // 
            this.dgvPackagingTransactions.AllowUserToAddRows = false;
            this.dgvPackagingTransactions.AllowUserToDeleteRows = false;
            dataGridViewCellStyle16.BackColor = System.Drawing.Color.White;
            this.dgvPackagingTransactions.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle16;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(234)))), ((int)(((byte)(237)))));
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            dataGridViewCellStyle17.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPackagingTransactions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle17;
            this.dgvPackagingTransactions.ColumnHeadersHeight = 40;
            this.dgvPackagingTransactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            dataGridViewCellStyle18.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPackagingTransactions.DefaultCellStyle = dataGridViewCellStyle18;
            this.dgvPackagingTransactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPackagingTransactions.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            this.dgvPackagingTransactions.Location = new System.Drawing.Point(0, 40);
            this.dgvPackagingTransactions.Name = "dgvPackagingTransactions";
            this.dgvPackagingTransactions.ReadOnly = true;
            this.dgvPackagingTransactions.RowHeadersVisible = false;
            this.dgvPackagingTransactions.RowHeadersWidth = 51;
            this.dgvPackagingTransactions.RowTemplate.Height = 30;
            this.dgvPackagingTransactions.Size = new System.Drawing.Size(1160, 282);
            this.dgvPackagingTransactions.TabIndex = 1;
            this.dgvPackagingTransactions.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.LightGrid;
            this.dgvPackagingTransactions.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvPackagingTransactions.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvPackagingTransactions.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvPackagingTransactions.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvPackagingTransactions.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvPackagingTransactions.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvPackagingTransactions.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            this.dgvPackagingTransactions.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(234)))), ((int)(((byte)(237)))));
            this.dgvPackagingTransactions.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvPackagingTransactions.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.dgvPackagingTransactions.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvPackagingTransactions.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvPackagingTransactions.ThemeStyle.HeaderStyle.Height = 40;
            this.dgvPackagingTransactions.ThemeStyle.ReadOnly = true;
            this.dgvPackagingTransactions.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvPackagingTransactions.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvPackagingTransactions.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.dgvPackagingTransactions.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvPackagingTransactions.ThemeStyle.RowsStyle.Height = 30;
            this.dgvPackagingTransactions.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            this.dgvPackagingTransactions.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.Black;
            // 
            // panelTransactionHeader
            // 
            this.panelTransactionHeader.Controls.Add(this.lblTransactionCount);
            this.panelTransactionHeader.Controls.Add(this.lblSelectedCustomer);
            this.panelTransactionHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTransactionHeader.Location = new System.Drawing.Point(0, 0);
            this.panelTransactionHeader.Name = "panelTransactionHeader";
            this.panelTransactionHeader.Size = new System.Drawing.Size(1160, 40);
            this.panelTransactionHeader.TabIndex = 0;
            // 
            // lblTransactionCount
            // 
            this.lblTransactionCount.AutoSize = true;
            this.lblTransactionCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionCount.ForeColor = System.Drawing.Color.Gray;
            this.lblTransactionCount.Location = new System.Drawing.Point(400, 12);
            this.lblTransactionCount.Name = "lblTransactionCount";
            this.lblTransactionCount.Size = new System.Drawing.Size(241, 20);
            this.lblTransactionCount.TabIndex = 1;
            this.lblTransactionCount.Text = "Chọn khách hàng để xem giao dịch";
            // 
            // lblSelectedCustomer
            // 
            this.lblSelectedCustomer.AutoSize = true;
            this.lblSelectedCustomer.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedCustomer.Location = new System.Drawing.Point(3, 10);
            this.lblSelectedCustomer.Name = "lblSelectedCustomer";
            this.lblSelectedCustomer.Size = new System.Drawing.Size(150, 25);
            this.lblSelectedCustomer.TabIndex = 0;
            this.lblSelectedCustomer.Text = "Giao dịch bao bì";
            // 
            // panelFilter
            // 
            this.panelFilter.BackColor = System.Drawing.Color.White;
            this.panelFilter.Controls.Add(this.dtpToDate);
            this.panelFilter.Controls.Add(this.cmbCustomerRank);
            this.panelFilter.Controls.Add(this.label2);
            this.panelFilter.Controls.Add(this.dtpFromDate);
            this.panelFilter.Controls.Add(this.label1);
            this.panelFilter.Controls.Add(this.btnExportPDF);
            this.panelFilter.Controls.Add(this.btnExportExcel);
            this.panelFilter.Controls.Add(this.btnRefresh);
            this.panelFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilter.Location = new System.Drawing.Point(20, 157);
            this.panelFilter.Name = "panelFilter";
            this.panelFilter.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.panelFilter.Size = new System.Drawing.Size(1160, 63);
            this.panelFilter.TabIndex = 3;
            // 
            // dtpToDate
            // 
            this.dtpToDate.BorderRadius = 5;
            this.dtpToDate.Checked = true;
            this.dtpToDate.CustomFormat = "dd/MM/yyyy";
            this.dtpToDate.FillColor = System.Drawing.Color.White;
            this.dtpToDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new System.Drawing.Point(275, 15);
            this.dtpToDate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpToDate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(127, 36);
            this.dtpToDate.TabIndex = 6;
            this.dtpToDate.Value = new System.DateTime(2025, 11, 26, 0, 0, 0, 0);
            this.dtpToDate.ValueChanged += new System.EventHandler(this.dtpToDate_ValueChanged);
            // 
            // cmbCustomerRank
            // 
            this.cmbCustomerRank.BackColor = System.Drawing.Color.Transparent;
            this.cmbCustomerRank.BorderRadius = 5;
            this.cmbCustomerRank.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCustomerRank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCustomerRank.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbCustomerRank.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbCustomerRank.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbCustomerRank.ForeColor = System.Drawing.Color.Black;
            this.cmbCustomerRank.ItemHeight = 30;
            this.cmbCustomerRank.Location = new System.Drawing.Point(430, 15);
            this.cmbCustomerRank.Name = "cmbCustomerRank";
            this.cmbCustomerRank.Size = new System.Drawing.Size(150, 36);
            this.cmbCustomerRank.TabIndex = 8;
            this.cmbCustomerRank.SelectedIndexChanged += new System.EventHandler(this.cmbCustomerRank_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(244, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "đến";
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.BorderRadius = 5;
            this.dtpFromDate.Checked = true;
            this.dtpFromDate.CustomFormat = "dd/MM/yyyy";
            this.dtpFromDate.FillColor = System.Drawing.Color.White;
            this.dtpFromDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new System.Drawing.Point(115, 15);
            this.dtpFromDate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpFromDate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(127, 36);
            this.dtpFromDate.TabIndex = 4;
            this.dtpFromDate.Value = new System.DateTime(2025, 11, 26, 0, 0, 0, 0);
            this.dtpFromDate.ValueChanged += new System.EventHandler(this.dtpFromDate_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(24, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Thời gian: Từ";
            // 
            // btnExportPDF
            // 
            this.btnExportPDF.BorderRadius = 5;
            this.btnExportPDF.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnExportPDF.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnExportPDF.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnExportPDF.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnExportPDF.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnExportPDF.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnExportPDF.ForeColor = System.Drawing.Color.White;
            this.btnExportPDF.Location = new System.Drawing.Point(1030, 15);
            this.btnExportPDF.Name = "btnExportPDF";
            this.btnExportPDF.Size = new System.Drawing.Size(120, 36);
            this.btnExportPDF.TabIndex = 2;
            this.btnExportPDF.Text = "Xuất PDF";
            this.btnExportPDF.Click += new System.EventHandler(this.btnExportPDF_Click);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.BorderRadius = 5;
            this.btnExportExcel.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnExportExcel.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnExportExcel.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnExportExcel.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnExportExcel.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnExportExcel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnExportExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportExcel.Location = new System.Drawing.Point(900, 15);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(120, 36);
            this.btnExportExcel.TabIndex = 1;
            this.btnExportExcel.Text = "Xuất Excel";
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BorderRadius = 5;
            this.btnRefresh.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnRefresh.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnRefresh.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnRefresh.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnRefresh.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(162)))), ((int)(((byte)(184)))));
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(770, 15);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(120, 36);
            this.btnRefresh.TabIndex = 0;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // panelSearch
            // 
            this.panelSearch.BackColor = System.Drawing.Color.White;
            this.panelSearch.Controls.Add(this.txtSearch);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(20, 95);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.panelSearch.Size = new System.Drawing.Size(1160, 62);
            this.panelSearch.TabIndex = 1;
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
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtSearch.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtSearch.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSearch.Location = new System.Drawing.Point(0, 10);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.txtSearch.PlaceholderText = "Tìm kiếm khách hàng...";
            this.txtSearch.SelectedText = "";
            this.txtSearch.Size = new System.Drawing.Size(300, 42);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(410, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "⦿";
            // 
            // btnAddCustomer
            // 
            this.btnAddCustomer.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAddCustomer.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnAddCustomer.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnAddCustomer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnAddCustomer.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.btnAddCustomer.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAddCustomer.ForeColor = System.Drawing.Color.White;
            this.btnAddCustomer.Location = new System.Drawing.Point(993, 20);
            this.btnAddCustomer.Name = "btnAddCustomer";
            this.btnAddCustomer.Size = new System.Drawing.Size(157, 45);
            this.btnAddCustomer.TabIndex = 2;
            this.btnAddCustomer.Text = "Thêm khách hàng";
            this.btnAddCustomer.Click += new System.EventHandler(this.btnAddCustomer_Click);
            this.btnAddCustomer.MouseEnter += new System.EventHandler(this.btnAddCustomer_MouseEnter);
            this.btnAddCustomer.MouseLeave += new System.EventHandler(this.btnAddCustomer_MouseLeave);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(267, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Quản lý khách hàng";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDescription.ForeColor = System.Drawing.Color.Gray;
            this.lblDescription.Location = new System.Drawing.Point(22, 55);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(409, 20);
            this.lblDescription.TabIndex = 1;
            this.lblDescription.Text = "Quản lý thông tin khách hàng, điểm tích lũy, hạng thành viên.";
            // 
            // guna2PanelHeader
            // 
            this.guna2PanelHeader.AutoSize = true;
            this.guna2PanelHeader.BackColor = System.Drawing.Color.White;
            this.guna2PanelHeader.Controls.Add(this.lblDescription);
            this.guna2PanelHeader.Controls.Add(this.lblTitle);
            this.guna2PanelHeader.Controls.Add(this.btnAddCustomer);
            this.guna2PanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2PanelHeader.Location = new System.Drawing.Point(20, 20);
            this.guna2PanelHeader.Name = "guna2PanelHeader";
            this.guna2PanelHeader.Size = new System.Drawing.Size(1160, 75);
            this.guna2PanelHeader.TabIndex = 0;
            // 
            // CustomersControl
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.guna2PanelContent);
            this.Name = "CustomersControl";
            this.Size = new System.Drawing.Size(1200, 872);
            this.Load += new System.EventHandler(this.CustomersControl_Load);
            this.guna2PanelContent.ResumeLayout(false);
            this.guna2PanelContent.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCustomers)).EndInit();
            this.panelTransactions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPackagingTransactions)).EndInit();
            this.panelTransactionHeader.ResumeLayout(false);
            this.panelTransactionHeader.PerformLayout();
            this.panelFilter.ResumeLayout(false);
            this.panelFilter.PerformLayout();
            this.panelSearch.ResumeLayout(false);
            this.guna2PanelHeader.ResumeLayout(false);
            this.guna2PanelHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2PanelContent;
        private Guna.UI2.WinForms.Guna2DataGridView dataGridViewCustomers;
        private System.Windows.Forms.Panel panelSearch;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private System.Windows.Forms.Panel panelFilter;
        private Guna.UI2.WinForms.Guna2ComboBox cmbCustomerRank;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2Button btnExportPDF;
        private Guna.UI2.WinForms.Guna2Button btnExportExcel;
        private Guna.UI2.WinForms.Guna2Button btnRefresh;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Panel panelTransactions;
        private Guna.UI2.WinForms.Guna2DataGridView dgvPackagingTransactions;
        private System.Windows.Forms.Panel panelTransactionHeader;
        private System.Windows.Forms.Label lblTransactionCount;
        private System.Windows.Forms.Label lblSelectedCustomer;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpToDate;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpFromDate;
        private System.Windows.Forms.Label label1;
        private Guna2Panel guna2PanelHeader;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblTitle;
        private Guna2Button btnAddCustomer;
    }
}