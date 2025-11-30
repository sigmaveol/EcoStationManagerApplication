using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    partial class OrdersControl
    {
        private System.ComponentModel.IContainer components = null;

        private Panel headerPanel;
        private FlowLayoutPanel tabPanel; 
        private Panel contentPanel;
        private Button btnExportPDF;
        private Button btnExportExcel;
        private Button btnImportExcel;
        private Button btnAddOrder;
        private DataGridView dgvOrders;
        private Label lblColumnHeaders;
        private Panel dateFilterPanel;
        private Button btnFilterToday;
        private Button btnFilterThisWeek;
        private Button btnFilterThisMonth;
        private Button btnFilterDateRange;
        private DateTimePicker dtpStartDate;
        private DateTimePicker dtpEndDate;
        private Label lblDateRange;

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
            this.searchPanel = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.titleLabelContent = new System.Windows.Forms.Label();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.btnExportPDF = new System.Windows.Forms.Button();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnImportExcel = new System.Windows.Forms.Button();
            this.btnAddOrder = new System.Windows.Forms.Button();
            this.tabPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.dateFilterPanel = new System.Windows.Forms.Panel();
            this.lblDateRange = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.btnFilterDateRange = new System.Windows.Forms.Button();
            this.btnFilterThisMonth = new System.Windows.Forms.Button();
            this.btnFilterThisWeek = new System.Windows.Forms.Button();
            this.btnFilterToday = new System.Windows.Forms.Button();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.lblColumnHeaders = new System.Windows.Forms.Label();
            this.dgvOrders = new System.Windows.Forms.DataGridView();
            this.searchPanel.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.dateFilterPanel.SuspendLayout();
            this.contentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).BeginInit();
            this.SuspendLayout();
            // 
            // titleLabelHeader
            // 
            this.titleLabelHeader.AutoSize = true;
            this.titleLabelHeader.Dock = System.Windows.Forms.DockStyle.Left;
            this.titleLabelHeader.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.titleLabelHeader.Location = new System.Drawing.Point(0, 0);
            this.titleLabelHeader.Name = "titleLabelHeader";
            this.titleLabelHeader.Size = new System.Drawing.Size(245, 37);
            this.titleLabelHeader.TabIndex = 0;
            this.titleLabelHeader.Text = "Quản lý Đơn hàng";
            // 
            // searchPanel
            // 
            this.searchPanel.Controls.Add(this.btnSearch);
            this.searchPanel.Controls.Add(this.txtSearch);
            this.searchPanel.Controls.Add(this.titleLabelContent);
            this.searchPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.searchPanel.Location = new System.Drawing.Point(15, 15);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 15);
            this.searchPanel.Size = new System.Drawing.Size(870, 40);
            this.searchPanel.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(750, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(110, 27);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Tìm kiếm";
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Location = new System.Drawing.Point(460, 6);
            this.txtSearch.MaxLength = 100;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(280, 27);
            this.txtSearch.TabIndex = 1;
            // 
            // titleLabelContent
            // 
            this.titleLabelContent.AutoSize = true;
            this.titleLabelContent.Dock = System.Windows.Forms.DockStyle.Left;
            this.titleLabelContent.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.titleLabelContent.Location = new System.Drawing.Point(0, 0);
            this.titleLabelContent.Name = "titleLabelContent";
            this.titleLabelContent.Size = new System.Drawing.Size(207, 28);
            this.titleLabelContent.TabIndex = 0;
            this.titleLabelContent.Text = "Danh sách đơn hàng";
            // 
            // headerPanel
            // 
            this.headerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.headerPanel.Controls.Add(this.titleLabelHeader);
            this.headerPanel.Controls.Add(this.btnExportPDF);
            this.headerPanel.Controls.Add(this.btnExportExcel);
            this.headerPanel.Controls.Add(this.btnImportExcel);
            this.headerPanel.Controls.Add(this.btnAddOrder);
            this.headerPanel.Location = new System.Drawing.Point(20, 20);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(900, 60);
            this.headerPanel.TabIndex = 0;
            this.headerPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.headerPanel_Paint);
            // 
            // btnExportPDF
            // 
            this.btnExportPDF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportPDF.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnExportPDF.FlatAppearance.BorderSize = 0;
            this.btnExportPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportPDF.Location = new System.Drawing.Point(550, 12);
            this.btnExportPDF.Name = "btnExportPDF";
            this.btnExportPDF.Size = new System.Drawing.Size(100, 35);
            this.btnExportPDF.TabIndex = 1;
            this.btnExportPDF.Text = "Xuất PDF";
            this.btnExportPDF.UseVisualStyleBackColor = false;
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnExportExcel.FlatAppearance.BorderSize = 0;
            this.btnExportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportExcel.Location = new System.Drawing.Point(660, 12);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(100, 35);
            this.btnExportExcel.TabIndex = 2;
            this.btnExportExcel.Text = "Xuất Excel";
            this.btnExportExcel.UseVisualStyleBackColor = false;
            // 
            // btnImportExcel
            // 
            this.btnImportExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnImportExcel.FlatAppearance.BorderSize = 0;
            this.btnImportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportExcel.ForeColor = System.Drawing.Color.White;
            this.btnImportExcel.Location = new System.Drawing.Point(440, 12);
            this.btnImportExcel.Name = "btnImportExcel";
            this.btnImportExcel.Size = new System.Drawing.Size(100, 35);
            this.btnImportExcel.TabIndex = 4;
            this.btnImportExcel.Text = "Import Excel";
            this.btnImportExcel.UseVisualStyleBackColor = false;
            // 
            // btnAddOrder
            // 
            this.btnAddOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnAddOrder.FlatAppearance.BorderSize = 0;
            this.btnAddOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddOrder.ForeColor = System.Drawing.Color.White;
            this.btnAddOrder.Location = new System.Drawing.Point(770, 12);
            this.btnAddOrder.Name = "btnAddOrder";
            this.btnAddOrder.Size = new System.Drawing.Size(130, 35);
            this.btnAddOrder.TabIndex = 3;
            this.btnAddOrder.Text = "+ Thêm đơn hàng";
            this.btnAddOrder.UseVisualStyleBackColor = false;
            // 
            // tabPanel
            // 
            this.tabPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabPanel.BackColor = System.Drawing.Color.White;
            this.tabPanel.Location = new System.Drawing.Point(20, 90);
            this.tabPanel.Name = "tabPanel";
            this.tabPanel.Size = new System.Drawing.Size(900, 40);
            this.tabPanel.TabIndex = 1;
            this.tabPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.tabPanel_Paint);
            // 
            // dateFilterPanel
            // 
            this.dateFilterPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateFilterPanel.BackColor = System.Drawing.Color.White;
            this.dateFilterPanel.Controls.Add(this.lblDateRange);
            this.dateFilterPanel.Controls.Add(this.dtpEndDate);
            this.dateFilterPanel.Controls.Add(this.dtpStartDate);
            this.dateFilterPanel.Controls.Add(this.btnFilterDateRange);
            this.dateFilterPanel.Controls.Add(this.btnFilterThisMonth);
            this.dateFilterPanel.Controls.Add(this.btnFilterThisWeek);
            this.dateFilterPanel.Controls.Add(this.btnFilterToday);
            this.dateFilterPanel.Location = new System.Drawing.Point(20, 130);
            this.dateFilterPanel.Name = "dateFilterPanel";
            this.dateFilterPanel.Size = new System.Drawing.Size(900, 50);
            this.dateFilterPanel.TabIndex = 3;
            // 
            // lblDateRange
            // 
            this.lblDateRange.AutoSize = true;
            this.lblDateRange.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDateRange.Location = new System.Drawing.Point(450, 15);
            this.lblDateRange.Name = "lblDateRange";
            this.lblDateRange.Size = new System.Drawing.Size(0, 20);
            this.lblDateRange.TabIndex = 6;
            this.lblDateRange.Visible = false;
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(580, 12);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(120, 27);
            this.dtpEndDate.TabIndex = 5;
            this.dtpEndDate.Visible = false;
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new System.Drawing.Point(450, 12);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(120, 27);
            this.dtpStartDate.TabIndex = 4;
            this.dtpStartDate.Visible = false;
            // 
            // btnFilterDateRange
            // 
            this.btnFilterDateRange.BackColor = System.Drawing.Color.White;
            this.btnFilterDateRange.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnFilterDateRange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilterDateRange.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnFilterDateRange.Location = new System.Drawing.Point(340, 10);
            this.btnFilterDateRange.Name = "btnFilterDateRange";
            this.btnFilterDateRange.Size = new System.Drawing.Size(100, 30);
            this.btnFilterDateRange.TabIndex = 3;
            this.btnFilterDateRange.Text = "Khoảng ngày";
            this.btnFilterDateRange.UseVisualStyleBackColor = false;
            this.btnFilterDateRange.Click += new System.EventHandler(this.btnFilterDateRange_Click);
            // 
            // btnFilterThisMonth
            // 
            this.btnFilterThisMonth.BackColor = System.Drawing.Color.White;
            this.btnFilterThisMonth.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnFilterThisMonth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilterThisMonth.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnFilterThisMonth.Location = new System.Drawing.Point(230, 10);
            this.btnFilterThisMonth.Name = "btnFilterThisMonth";
            this.btnFilterThisMonth.Size = new System.Drawing.Size(100, 30);
            this.btnFilterThisMonth.TabIndex = 2;
            this.btnFilterThisMonth.Text = "Tháng này";
            this.btnFilterThisMonth.UseVisualStyleBackColor = false;
            this.btnFilterThisMonth.Click += new System.EventHandler(this.btnFilterThisMonth_Click);
            // 
            // btnFilterThisWeek
            // 
            this.btnFilterThisWeek.BackColor = System.Drawing.Color.White;
            this.btnFilterThisWeek.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnFilterThisWeek.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilterThisWeek.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnFilterThisWeek.Location = new System.Drawing.Point(120, 10);
            this.btnFilterThisWeek.Name = "btnFilterThisWeek";
            this.btnFilterThisWeek.Size = new System.Drawing.Size(100, 30);
            this.btnFilterThisWeek.TabIndex = 1;
            this.btnFilterThisWeek.Text = "Tuần này";
            this.btnFilterThisWeek.UseVisualStyleBackColor = false;
            this.btnFilterThisWeek.Click += new System.EventHandler(this.btnFilterThisWeek_Click);
            // 
            // btnFilterToday
            // 
            this.btnFilterToday.BackColor = System.Drawing.Color.White;
            this.btnFilterToday.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnFilterToday.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilterToday.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnFilterToday.Location = new System.Drawing.Point(10, 10);
            this.btnFilterToday.Name = "btnFilterToday";
            this.btnFilterToday.Size = new System.Drawing.Size(100, 30);
            this.btnFilterToday.TabIndex = 0;
            this.btnFilterToday.Text = "Hôm nay";
            this.btnFilterToday.UseVisualStyleBackColor = false;
            this.btnFilterToday.Click += new System.EventHandler(this.btnFilterToday_Click);
            // 
            // contentPanel
            // 
            this.contentPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.contentPanel.AutoSize = true;
            this.contentPanel.BackColor = System.Drawing.Color.White;
            this.contentPanel.Controls.Add(this.searchPanel);
            this.contentPanel.Controls.Add(this.lblColumnHeaders);
            this.contentPanel.Controls.Add(this.dgvOrders);
            this.contentPanel.Location = new System.Drawing.Point(20, 190);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Padding = new System.Windows.Forms.Padding(15);
            this.contentPanel.Size = new System.Drawing.Size(900, 480);
            this.contentPanel.TabIndex = 2;
            // 
            // lblColumnHeaders
            // 
            this.lblColumnHeaders.Location = new System.Drawing.Point(0, 0);
            this.lblColumnHeaders.Name = "lblColumnHeaders";
            this.lblColumnHeaders.Size = new System.Drawing.Size(100, 23);
            this.lblColumnHeaders.TabIndex = 1;
            // 
            // dgvOrders
            // 
            this.dgvOrders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvOrders.ColumnHeadersHeight = 29;
            this.dgvOrders.Location = new System.Drawing.Point(15, 80);
            this.dgvOrders.Name = "dgvOrders";
            this.dgvOrders.RowHeadersWidth = 51;
            this.dgvOrders.Size = new System.Drawing.Size(870, 385);
            this.dgvOrders.TabIndex = 1;
            this.dgvOrders.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOrders_CellContentClick);
            // 
            // OrdersControl
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.headerPanel);
            this.Controls.Add(this.tabPanel);
            this.Controls.Add(this.dateFilterPanel);
            this.Controls.Add(this.contentPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "OrdersControl";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(940, 640);
            this.Load += new System.EventHandler(this.OrdersControl_Load);
            this.searchPanel.ResumeLayout(false);
            this.searchPanel.PerformLayout();
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.dateFilterPanel.ResumeLayout(false);
            this.dateFilterPanel.PerformLayout();
            this.contentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Label titleLabelHeader;
        private Panel searchPanel;
        private Label titleLabelContent;
        private TextBox txtSearch;
        private Button btnSearch;
    }
}