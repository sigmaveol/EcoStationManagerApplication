using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    partial class ReportControl
    {
        private System.ComponentModel.IContainer components = null;

        // Header Panel
        private Guna.UI2.WinForms.Guna2Panel panelHeader;
        private Label lblReportTitle;
        private Label lblReportDescription;
        private Guna.UI2.WinForms.Guna2Button btnExportPDF;
        private Guna.UI2.WinForms.Guna2Button btnExportExcel;
        private FlowLayoutPanel flowPanelReportTypes;
        private Guna.UI2.WinForms.Guna2Button btnToggleRevenue;
        private Guna.UI2.WinForms.Guna2Button btnToggleCustomerRefill;
        private Guna.UI2.WinForms.Guna2Button btnTogglePackagingRecovery;
        private Guna.UI2.WinForms.Guna2Button btnTogglePlasticReduction;
        private Guna.UI2.WinForms.Guna2Button btnTogglePaymentMethod;
        private Guna.UI2.WinForms.Guna2Button btnToggleBestSelling;

        // Filter Panel - Time Range 
        private Guna.UI2.WinForms.Guna2Panel panelFilters;
        private Label lblTimeRange;
        private Guna.UI2.WinForms.Guna2ComboBox cmbTimeRange;
        private Panel panelCustomDateRange;
        private Label lblFromDate;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpFromDate;
        private Label lblToDate;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpToDate;
        private Guna.UI2.WinForms.Guna2Button btnGenerateReport;

        // Report Content Panel
        private Guna.UI2.WinForms.Guna2Panel panelReportContent;
        private FlowLayoutPanel flowPanelKPICards;
        private Panel panelChart;
        private Guna.UI2.WinForms.Guna2DataGridView dataGridViewReport;
        private Guna.UI2.WinForms.Guna2Panel panelContent;
        private CardControl cardKpi1;
        private CardControl cardKpi2;
        private CardControl cardKpi3;
        private CardControl cardKpi4;
        private CardControl cardKpi5;
        private CardControl cardKpi6;

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
            this.panelHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.lblReportTitle = new System.Windows.Forms.Label();
            this.lblReportDescription = new System.Windows.Forms.Label();
            this.btnExportPDF = new Guna.UI2.WinForms.Guna2Button();
            this.btnExportExcel = new Guna.UI2.WinForms.Guna2Button();
            this.flowPanelReportTypes = new System.Windows.Forms.FlowLayoutPanel();
            this.btnToggleRevenue = new Guna.UI2.WinForms.Guna2Button();
            this.btnToggleCustomerRefill = new Guna.UI2.WinForms.Guna2Button();
            this.btnTogglePackagingRecovery = new Guna.UI2.WinForms.Guna2Button();
            this.btnTogglePlasticReduction = new Guna.UI2.WinForms.Guna2Button();
            this.btnTogglePaymentMethod = new Guna.UI2.WinForms.Guna2Button();
            this.btnToggleBestSelling = new Guna.UI2.WinForms.Guna2Button();
            this.panelFilters = new Guna.UI2.WinForms.Guna2Panel();
            this.btnGenerateReport = new Guna.UI2.WinForms.Guna2Button();
            this.panelCustomDateRange = new System.Windows.Forms.Panel();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.dtpFromDate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.lblToDate = new System.Windows.Forms.Label();
            this.dtpToDate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.cmbTimeRange = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblTimeRange = new System.Windows.Forms.Label();
            this.panelReportContent = new Guna.UI2.WinForms.Guna2Panel();
            this.dataGridViewReport = new Guna.UI2.WinForms.Guna2DataGridView();
            this.panelChart = new System.Windows.Forms.Panel();
            this.flowPanelKPICards = new System.Windows.Forms.FlowLayoutPanel();
            this.panelContent = new Guna.UI2.WinForms.Guna2Panel();
            this.cardKpi1 = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.cardKpi2 = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.cardKpi3 = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.cardKpi4 = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.cardKpi5 = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.cardKpi6 = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.panelHeader.SuspendLayout();
            this.flowPanelReportTypes.SuspendLayout();
            this.panelFilters.SuspendLayout();
            this.panelCustomDateRange.SuspendLayout();
            this.panelReportContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReport)).BeginInit();
            this.flowPanelKPICards.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.Transparent;
            this.panelHeader.Controls.Add(this.lblReportTitle);
            this.panelHeader.Controls.Add(this.lblReportDescription);
            this.panelHeader.Controls.Add(this.btnExportPDF);
            this.panelHeader.Controls.Add(this.btnExportExcel);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.FillColor = System.Drawing.Color.White;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(20);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(20);
            this.panelHeader.ShadowDecoration.BorderRadius = 8;
            this.panelHeader.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.panelHeader.ShadowDecoration.Depth = 5;
            this.panelHeader.ShadowDecoration.Enabled = true;
            this.panelHeader.Size = new System.Drawing.Size(1179, 82);
            this.panelHeader.TabIndex = 0;
            // 
            // lblReportTitle
            // 
            this.lblReportTitle.AutoSize = true;
            this.lblReportTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblReportTitle.Location = new System.Drawing.Point(3, 0);
            this.lblReportTitle.Name = "lblReportTitle";
            this.lblReportTitle.Size = new System.Drawing.Size(261, 41);
            this.lblReportTitle.TabIndex = 0;
            this.lblReportTitle.Text = "Báo cáo thống kê";
            // 
            // lblReportDescription
            // 
            this.lblReportDescription.AutoSize = true;
            this.lblReportDescription.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.lblReportDescription.Location = new System.Drawing.Point(15, 47);
            this.lblReportDescription.Name = "lblReportDescription";
            this.lblReportDescription.Size = new System.Drawing.Size(449, 25);
            this.lblReportDescription.TabIndex = 1;
            this.lblReportDescription.Text = "Phân tích và thống kê dữ liệu hoạt động kinh doanh";
            // 
            // btnExportPDF
            // 
            this.btnExportPDF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportPDF.BorderRadius = 6;
            this.btnExportPDF.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnExportPDF.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnExportPDF.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnExportPDF.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnExportPDF.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.btnExportPDF.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportPDF.ForeColor = System.Drawing.Color.White;
            this.btnExportPDF.ImageSize = new System.Drawing.Size(16, 16);
            this.btnExportPDF.Location = new System.Drawing.Point(914, 20);
            this.btnExportPDF.Name = "btnExportPDF";
            this.btnExportPDF.Size = new System.Drawing.Size(116, 47);
            this.btnExportPDF.TabIndex = 2;
            this.btnExportPDF.Text = "Xuất PDF";
            this.btnExportPDF.Click += new System.EventHandler(this.btnExportPDF_Click);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportExcel.BorderRadius = 6;
            this.btnExportExcel.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnExportExcel.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnExportExcel.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnExportExcel.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnExportExcel.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.btnExportExcel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportExcel.ImageSize = new System.Drawing.Size(16, 16);
            this.btnExportExcel.Location = new System.Drawing.Point(1060, 20);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(116, 47);
            this.btnExportExcel.TabIndex = 3;
            this.btnExportExcel.Text = "Xuất Excel";
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // flowPanelReportTypes
            // 
            this.flowPanelReportTypes.AutoSize = true;
            this.flowPanelReportTypes.Controls.Add(this.btnToggleRevenue);
            this.flowPanelReportTypes.Controls.Add(this.btnToggleCustomerRefill);
            this.flowPanelReportTypes.Controls.Add(this.btnTogglePackagingRecovery);
            this.flowPanelReportTypes.Controls.Add(this.btnTogglePlasticReduction);
            this.flowPanelReportTypes.Controls.Add(this.btnTogglePaymentMethod);
            this.flowPanelReportTypes.Controls.Add(this.btnToggleBestSelling);
            this.flowPanelReportTypes.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowPanelReportTypes.Location = new System.Drawing.Point(0, 82);
            this.flowPanelReportTypes.Name = "flowPanelReportTypes";
            this.flowPanelReportTypes.Padding = new System.Windows.Forms.Padding(20);
            this.flowPanelReportTypes.Size = new System.Drawing.Size(1179, 122);
            this.flowPanelReportTypes.TabIndex = 0;
            // 
            // btnToggleRevenue
            // 
            this.btnToggleRevenue.BorderRadius = 6;
            this.btnToggleRevenue.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnToggleRevenue.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnToggleRevenue.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnToggleRevenue.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnToggleRevenue.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.btnToggleRevenue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToggleRevenue.ForeColor = System.Drawing.Color.White;
            this.btnToggleRevenue.Location = new System.Drawing.Point(23, 23);
            this.btnToggleRevenue.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.btnToggleRevenue.Name = "btnToggleRevenue";
            this.btnToggleRevenue.Size = new System.Drawing.Size(150, 35);
            this.btnToggleRevenue.TabIndex = 0;
            this.btnToggleRevenue.Text = "Doanh thu";
            this.btnToggleRevenue.Click += new System.EventHandler(this.btnToggleReportType_Click);
            // 
            // btnToggleCustomerRefill
            // 
            this.btnToggleCustomerRefill.BorderRadius = 6;
            this.btnToggleCustomerRefill.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnToggleCustomerRefill.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnToggleCustomerRefill.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnToggleCustomerRefill.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnToggleCustomerRefill.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnToggleCustomerRefill.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToggleCustomerRefill.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnToggleCustomerRefill.Location = new System.Drawing.Point(191, 23);
            this.btnToggleCustomerRefill.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.btnToggleCustomerRefill.Name = "btnToggleCustomerRefill";
            this.btnToggleCustomerRefill.Size = new System.Drawing.Size(180, 35);
            this.btnToggleCustomerRefill.TabIndex = 1;
            this.btnToggleCustomerRefill.Text = "Tần suất KH quay lại";
            this.btnToggleCustomerRefill.Click += new System.EventHandler(this.btnToggleReportType_Click);
            // 
            // btnTogglePackagingRecovery
            // 
            this.btnTogglePackagingRecovery.BorderRadius = 6;
            this.btnTogglePackagingRecovery.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnTogglePackagingRecovery.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnTogglePackagingRecovery.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnTogglePackagingRecovery.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnTogglePackagingRecovery.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnTogglePackagingRecovery.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTogglePackagingRecovery.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnTogglePackagingRecovery.Location = new System.Drawing.Point(389, 23);
            this.btnTogglePackagingRecovery.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.btnTogglePackagingRecovery.Name = "btnTogglePackagingRecovery";
            this.btnTogglePackagingRecovery.Size = new System.Drawing.Size(160, 35);
            this.btnTogglePackagingRecovery.TabIndex = 2;
            this.btnTogglePackagingRecovery.Text = "Tỷ lệ thu hồi bao bì";
            this.btnTogglePackagingRecovery.Click += new System.EventHandler(this.btnToggleReportType_Click);
            // 
            // btnTogglePlasticReduction
            // 
            this.btnTogglePlasticReduction.BorderRadius = 6;
            this.btnTogglePlasticReduction.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnTogglePlasticReduction.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnTogglePlasticReduction.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnTogglePlasticReduction.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnTogglePlasticReduction.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnTogglePlasticReduction.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTogglePlasticReduction.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnTogglePlasticReduction.Location = new System.Drawing.Point(567, 23);
            this.btnTogglePlasticReduction.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.btnTogglePlasticReduction.Name = "btnTogglePlasticReduction";
            this.btnTogglePlasticReduction.Size = new System.Drawing.Size(212, 35);
            this.btnTogglePlasticReduction.TabIndex = 3;
            this.btnTogglePlasticReduction.Text = "Lượng nhựa giảm phát thải";
            this.btnTogglePlasticReduction.Click += new System.EventHandler(this.btnToggleReportType_Click);
            // 
            // btnTogglePaymentMethod
            // 
            this.btnTogglePaymentMethod.BorderRadius = 6;
            this.btnTogglePaymentMethod.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnTogglePaymentMethod.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnTogglePaymentMethod.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnTogglePaymentMethod.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnTogglePaymentMethod.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnTogglePaymentMethod.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTogglePaymentMethod.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnTogglePaymentMethod.Location = new System.Drawing.Point(797, 23);
            this.btnTogglePaymentMethod.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.btnTogglePaymentMethod.Name = "btnTogglePaymentMethod";
            this.btnTogglePaymentMethod.Size = new System.Drawing.Size(214, 35);
            this.btnTogglePaymentMethod.TabIndex = 4;
            this.btnTogglePaymentMethod.Text = "Phương thức thanh toán";
            this.btnTogglePaymentMethod.Click += new System.EventHandler(this.btnToggleReportType_Click);
            // 
            // btnToggleBestSelling
            // 
            this.btnToggleBestSelling.BorderRadius = 6;
            this.btnToggleBestSelling.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnToggleBestSelling.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnToggleBestSelling.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnToggleBestSelling.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnToggleBestSelling.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnToggleBestSelling.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToggleBestSelling.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnToggleBestSelling.Location = new System.Drawing.Point(23, 64);
            this.btnToggleBestSelling.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.btnToggleBestSelling.Name = "btnToggleBestSelling";
            this.btnToggleBestSelling.Size = new System.Drawing.Size(181, 35);
            this.btnToggleBestSelling.TabIndex = 5;
            this.btnToggleBestSelling.Text = "Mặt hàng bán chạy";
            this.btnToggleBestSelling.Click += new System.EventHandler(this.btnToggleReportType_Click);
            // 
            // panelFilters
            // 
            this.panelFilters.BackColor = System.Drawing.Color.Transparent;
            this.panelFilters.Controls.Add(this.btnGenerateReport);
            this.panelFilters.Controls.Add(this.panelCustomDateRange);
            this.panelFilters.Controls.Add(this.cmbTimeRange);
            this.panelFilters.Controls.Add(this.lblTimeRange);
            this.panelFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilters.FillColor = System.Drawing.Color.White;
            this.panelFilters.Location = new System.Drawing.Point(0, 204);
            this.panelFilters.Margin = new System.Windows.Forms.Padding(20, 10, 20, 20);
            this.panelFilters.Name = "panelFilters";
            this.panelFilters.Padding = new System.Windows.Forms.Padding(20);
            this.panelFilters.ShadowDecoration.BorderRadius = 8;
            this.panelFilters.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.panelFilters.ShadowDecoration.Depth = 5;
            this.panelFilters.ShadowDecoration.Enabled = true;
            this.panelFilters.Size = new System.Drawing.Size(1179, 100);
            this.panelFilters.TabIndex = 2;
            // 
            // btnGenerateReport
            // 
            this.btnGenerateReport.BorderRadius = 6;
            this.btnGenerateReport.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnGenerateReport.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnGenerateReport.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnGenerateReport.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnGenerateReport.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnGenerateReport.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.btnGenerateReport.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateReport.ForeColor = System.Drawing.Color.White;
            this.btnGenerateReport.Location = new System.Drawing.Point(366, 20);
            this.btnGenerateReport.MaximumSize = new System.Drawing.Size(150, 40);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new System.Drawing.Size(150, 40);
            this.btnGenerateReport.TabIndex = 3;
            this.btnGenerateReport.Text = "Tạo báo cáo";
            this.btnGenerateReport.Click += new System.EventHandler(this.btnGenerateReport_Click);
            // 
            // panelCustomDateRange
            // 
            this.panelCustomDateRange.Controls.Add(this.lblFromDate);
            this.panelCustomDateRange.Controls.Add(this.dtpFromDate);
            this.panelCustomDateRange.Controls.Add(this.lblToDate);
            this.panelCustomDateRange.Controls.Add(this.dtpToDate);
            this.panelCustomDateRange.Location = new System.Drawing.Point(547, 13);
            this.panelCustomDateRange.Name = "panelCustomDateRange";
            this.panelCustomDateRange.Size = new System.Drawing.Size(600, 71);
            this.panelCustomDateRange.TabIndex = 2;
            this.panelCustomDateRange.Visible = false;
            // 
            // lblFromDate
            // 
            this.lblFromDate.AutoSize = true;
            this.lblFromDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromDate.Location = new System.Drawing.Point(0, 10);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(65, 20);
            this.lblFromDate.TabIndex = 0;
            this.lblFromDate.Text = "Từ ngày:";
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.BorderRadius = 6;
            this.dtpFromDate.Checked = true;
            this.dtpFromDate.FillColor = System.Drawing.Color.White;
            this.dtpFromDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFromDate.Location = new System.Drawing.Point(0, 30);
            this.dtpFromDate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpFromDate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(250, 35);
            this.dtpFromDate.TabIndex = 1;
            this.dtpFromDate.Value = new System.DateTime(2024, 1, 1, 0, 0, 0, 0);
            // 
            // lblToDate
            // 
            this.lblToDate.AutoSize = true;
            this.lblToDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToDate.Location = new System.Drawing.Point(270, 10);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(75, 20);
            this.lblToDate.TabIndex = 2;
            this.lblToDate.Text = "Đến ngày:";
            // 
            // dtpToDate
            // 
            this.dtpToDate.BorderRadius = 6;
            this.dtpToDate.Checked = true;
            this.dtpToDate.FillColor = System.Drawing.Color.White;
            this.dtpToDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpToDate.Location = new System.Drawing.Point(270, 30);
            this.dtpToDate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpToDate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(250, 35);
            this.dtpToDate.TabIndex = 3;
            this.dtpToDate.Value = new System.DateTime(2024, 12, 31, 0, 0, 0, 0);
            // 
            // cmbTimeRange
            // 
            this.cmbTimeRange.BackColor = System.Drawing.Color.Transparent;
            this.cmbTimeRange.BorderRadius = 6;
            this.cmbTimeRange.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmbTimeRange.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbTimeRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTimeRange.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbTimeRange.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbTimeRange.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbTimeRange.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbTimeRange.ItemHeight = 30;
            this.cmbTimeRange.Items.AddRange(new object[] {
            "Hôm nay",
            "7 ngày qua",
            "Tháng này",
            "Tháng trước",
            "Khoảng thời gian tùy chỉnh"});
            this.cmbTimeRange.Location = new System.Drawing.Point(166, 20);
            this.cmbTimeRange.Name = "cmbTimeRange";
            this.cmbTimeRange.Size = new System.Drawing.Size(200, 36);
            this.cmbTimeRange.TabIndex = 1;
            this.cmbTimeRange.SelectedIndexChanged += new System.EventHandler(this.cmbTimeRange_SelectedIndexChanged);
            // 
            // lblTimeRange
            // 
            this.lblTimeRange.AutoSize = true;
            this.lblTimeRange.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTimeRange.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeRange.Location = new System.Drawing.Point(20, 20);
            this.lblTimeRange.Name = "lblTimeRange";
            this.lblTimeRange.Size = new System.Drawing.Size(146, 23);
            this.lblTimeRange.TabIndex = 0;
            this.lblTimeRange.Text = "Khoảng thời gian:";
            // 
            // panelReportContent
            // 
            this.panelReportContent.BackColor = System.Drawing.Color.Transparent;
            this.panelReportContent.Controls.Add(this.dataGridViewReport);
            this.panelReportContent.Controls.Add(this.panelChart);
            this.panelReportContent.Controls.Add(this.flowPanelKPICards);
            this.panelReportContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelReportContent.FillColor = System.Drawing.Color.White;
            this.panelReportContent.Location = new System.Drawing.Point(0, 304);
            this.panelReportContent.Margin = new System.Windows.Forms.Padding(20);
            this.panelReportContent.Name = "panelReportContent";
            this.panelReportContent.Padding = new System.Windows.Forms.Padding(20);
            this.panelReportContent.ShadowDecoration.BorderRadius = 8;
            this.panelReportContent.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.panelReportContent.ShadowDecoration.Depth = 5;
            this.panelReportContent.ShadowDecoration.Enabled = true;
            this.panelReportContent.Size = new System.Drawing.Size(1179, 757);
            this.panelReportContent.TabIndex = 3;
            // 
            // dataGridViewReport
            // 
            this.dataGridViewReport.AllowUserToAddRows = false;
            this.dataGridViewReport.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dataGridViewReport.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewReport.ColumnHeadersHeight = 35;
            this.dataGridViewReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewReport.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewReport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dataGridViewReport.Location = new System.Drawing.Point(20, 486);
            this.dataGridViewReport.Name = "dataGridViewReport";
            this.dataGridViewReport.ReadOnly = true;
            this.dataGridViewReport.RowHeadersVisible = false;
            this.dataGridViewReport.RowHeadersWidth = 51;
            this.dataGridViewReport.RowTemplate.Height = 25;
            this.dataGridViewReport.Size = new System.Drawing.Size(1139, 251);
            this.dataGridViewReport.TabIndex = 1;
            this.dataGridViewReport.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewReport.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dataGridViewReport.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dataGridViewReport.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dataGridViewReport.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dataGridViewReport.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewReport.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dataGridViewReport.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.dataGridViewReport.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridViewReport.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.dataGridViewReport.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dataGridViewReport.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dataGridViewReport.ThemeStyle.HeaderStyle.Height = 35;
            this.dataGridViewReport.ThemeStyle.ReadOnly = true;
            this.dataGridViewReport.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewReport.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridViewReport.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dataGridViewReport.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.dataGridViewReport.ThemeStyle.RowsStyle.Height = 25;
            this.dataGridViewReport.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dataGridViewReport.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // panelChart
            // 
            this.panelChart.BackColor = System.Drawing.Color.White;
            this.panelChart.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelChart.Location = new System.Drawing.Point(20, 203);
            this.panelChart.Name = "panelChart";
            this.panelChart.Size = new System.Drawing.Size(1139, 283);
            this.panelChart.TabIndex = 2;
            // 
            // flowPanelKPICards
            // 
            this.flowPanelKPICards.AutoSize = true;
            this.flowPanelKPICards.Controls.Add(this.cardKpi1);
            this.flowPanelKPICards.Controls.Add(this.cardKpi2);
            this.flowPanelKPICards.Controls.Add(this.cardKpi3);
            this.flowPanelKPICards.Controls.Add(this.cardKpi4);
            this.flowPanelKPICards.Controls.Add(this.cardKpi5);
            this.flowPanelKPICards.Controls.Add(this.cardKpi6);
            this.flowPanelKPICards.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowPanelKPICards.Location = new System.Drawing.Point(20, 20);
            this.flowPanelKPICards.Name = "flowPanelKPICards";
            this.flowPanelKPICards.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.flowPanelKPICards.Size = new System.Drawing.Size(1139, 183);
            this.flowPanelKPICards.TabIndex = 0;
            // 
            // panelContent
            // 
            this.panelContent.AutoScroll = true;
            this.panelContent.BackColor = System.Drawing.Color.White;
            this.panelContent.Controls.Add(this.panelReportContent);
            this.panelContent.Controls.Add(this.panelFilters);
            this.panelContent.Controls.Add(this.flowPanelReportTypes);
            this.panelContent.Controls.Add(this.panelHeader);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.FillColor = System.Drawing.Color.White;
            this.panelContent.Location = new System.Drawing.Point(0, 0);
            this.panelContent.Name = "panelContent";
            this.panelContent.ShadowDecoration.BorderRadius = 8;
            this.panelContent.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.panelContent.ShadowDecoration.Depth = 2;
            this.panelContent.ShadowDecoration.Enabled = true;
            this.panelContent.Size = new System.Drawing.Size(1200, 1020);
            this.panelContent.TabIndex = 5;
            // 
            // cardKpi1
            // 
            this.cardKpi1.AutoScroll = true;
            this.cardKpi1.AutoSize = true;
            this.cardKpi1.BackColor = System.Drawing.Color.Transparent;
            this.cardKpi1.CardColor = System.Drawing.Color.White;
            this.cardKpi1.Change = null;
            this.cardKpi1.ChangeColor = System.Drawing.Color.Green;
            this.cardKpi1.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardKpi1.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardKpi1.Icon = null;
            this.cardKpi1.Location = new System.Drawing.Point(10, 5);
            this.cardKpi1.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.cardKpi1.MaximumSize = new System.Drawing.Size(400, 250);
            this.cardKpi1.Name = "cardKpi1";
            this.cardKpi1.Padding = new System.Windows.Forms.Padding(10);
            this.cardKpi1.Size = new System.Drawing.Size(57, 163);
            this.cardKpi1.SubInfo = null;
            this.cardKpi1.SubInfoColor = System.Drawing.Color.Gray;
            this.cardKpi1.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardKpi1.TabIndex = 0;
            this.cardKpi1.Title = "";
            this.cardKpi1.TitleColor = System.Drawing.Color.Gray;
            this.cardKpi1.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardKpi1.Value = null;
            this.cardKpi1.ValueColor = System.Drawing.Color.Black;
            this.cardKpi1.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            // 
            // cardKpi2
            // 
            this.cardKpi2.AutoScroll = true;
            this.cardKpi2.AutoSize = true;
            this.cardKpi2.BackColor = System.Drawing.Color.Transparent;
            this.cardKpi2.CardColor = System.Drawing.Color.White;
            this.cardKpi2.Change = null;
            this.cardKpi2.ChangeColor = System.Drawing.Color.Green;
            this.cardKpi2.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardKpi2.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardKpi2.Icon = null;
            this.cardKpi2.Location = new System.Drawing.Point(87, 5);
            this.cardKpi2.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.cardKpi2.MaximumSize = new System.Drawing.Size(400, 250);
            this.cardKpi2.Name = "cardKpi2";
            this.cardKpi2.Padding = new System.Windows.Forms.Padding(10);
            this.cardKpi2.Size = new System.Drawing.Size(57, 163);
            this.cardKpi2.SubInfo = null;
            this.cardKpi2.SubInfoColor = System.Drawing.Color.Gray;
            this.cardKpi2.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardKpi2.TabIndex = 1;
            this.cardKpi2.Title = "";
            this.cardKpi2.TitleColor = System.Drawing.Color.Gray;
            this.cardKpi2.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardKpi2.Value = null;
            this.cardKpi2.ValueColor = System.Drawing.Color.Black;
            this.cardKpi2.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            // 
            // cardKpi3
            // 
            this.cardKpi3.AutoScroll = true;
            this.cardKpi3.AutoSize = true;
            this.cardKpi3.BackColor = System.Drawing.Color.Transparent;
            this.cardKpi3.CardColor = System.Drawing.Color.White;
            this.cardKpi3.Change = null;
            this.cardKpi3.ChangeColor = System.Drawing.Color.Green;
            this.cardKpi3.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardKpi3.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardKpi3.Icon = null;
            this.cardKpi3.Location = new System.Drawing.Point(164, 5);
            this.cardKpi3.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.cardKpi3.MaximumSize = new System.Drawing.Size(400, 250);
            this.cardKpi3.Name = "cardKpi3";
            this.cardKpi3.Padding = new System.Windows.Forms.Padding(10);
            this.cardKpi3.Size = new System.Drawing.Size(57, 163);
            this.cardKpi3.SubInfo = null;
            this.cardKpi3.SubInfoColor = System.Drawing.Color.Gray;
            this.cardKpi3.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardKpi3.TabIndex = 2;
            this.cardKpi3.Title = "";
            this.cardKpi3.TitleColor = System.Drawing.Color.Gray;
            this.cardKpi3.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardKpi3.Value = null;
            this.cardKpi3.ValueColor = System.Drawing.Color.Black;
            this.cardKpi3.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            // 
            // cardKpi4
            // 
            this.cardKpi4.AutoScroll = true;
            this.cardKpi4.AutoSize = true;
            this.cardKpi4.BackColor = System.Drawing.Color.Transparent;
            this.cardKpi4.CardColor = System.Drawing.Color.White;
            this.cardKpi4.Change = null;
            this.cardKpi4.ChangeColor = System.Drawing.Color.Green;
            this.cardKpi4.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardKpi4.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardKpi4.Icon = null;
            this.cardKpi4.Location = new System.Drawing.Point(241, 5);
            this.cardKpi4.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.cardKpi4.MaximumSize = new System.Drawing.Size(400, 250);
            this.cardKpi4.Name = "cardKpi4";
            this.cardKpi4.Padding = new System.Windows.Forms.Padding(10);
            this.cardKpi4.Size = new System.Drawing.Size(57, 163);
            this.cardKpi4.SubInfo = null;
            this.cardKpi4.SubInfoColor = System.Drawing.Color.Gray;
            this.cardKpi4.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardKpi4.TabIndex = 3;
            this.cardKpi4.Title = "";
            this.cardKpi4.TitleColor = System.Drawing.Color.Gray;
            this.cardKpi4.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardKpi4.Value = null;
            this.cardKpi4.ValueColor = System.Drawing.Color.Black;
            this.cardKpi4.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            // 
            // cardKpi5
            // 
            this.cardKpi5.AutoScroll = true;
            this.cardKpi5.AutoSize = true;
            this.cardKpi5.BackColor = System.Drawing.Color.Transparent;
            this.cardKpi5.CardColor = System.Drawing.Color.White;
            this.cardKpi5.Change = null;
            this.cardKpi5.ChangeColor = System.Drawing.Color.Green;
            this.cardKpi5.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardKpi5.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardKpi5.Icon = null;
            this.cardKpi5.Location = new System.Drawing.Point(318, 5);
            this.cardKpi5.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.cardKpi5.MaximumSize = new System.Drawing.Size(400, 250);
            this.cardKpi5.Name = "cardKpi5";
            this.cardKpi5.Padding = new System.Windows.Forms.Padding(10);
            this.cardKpi5.Size = new System.Drawing.Size(57, 163);
            this.cardKpi5.SubInfo = null;
            this.cardKpi5.SubInfoColor = System.Drawing.Color.Gray;
            this.cardKpi5.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardKpi5.TabIndex = 4;
            this.cardKpi5.Title = "";
            this.cardKpi5.TitleColor = System.Drawing.Color.Gray;
            this.cardKpi5.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardKpi5.Value = null;
            this.cardKpi5.ValueColor = System.Drawing.Color.Black;
            this.cardKpi5.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            // 
            // cardKpi6
            // 
            this.cardKpi6.AutoScroll = true;
            this.cardKpi6.AutoSize = true;
            this.cardKpi6.BackColor = System.Drawing.Color.Transparent;
            this.cardKpi6.CardColor = System.Drawing.Color.Transparent;
            this.cardKpi6.Change = null;
            this.cardKpi6.ChangeColor = System.Drawing.Color.Green;
            this.cardKpi6.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardKpi6.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardKpi6.Icon = null;
            this.cardKpi6.Location = new System.Drawing.Point(395, 5);
            this.cardKpi6.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.cardKpi6.MaximumSize = new System.Drawing.Size(400, 250);
            this.cardKpi6.Name = "cardKpi6";
            this.cardKpi6.Padding = new System.Windows.Forms.Padding(10);
            this.cardKpi6.Size = new System.Drawing.Size(57, 163);
            this.cardKpi6.SubInfo = null;
            this.cardKpi6.SubInfoColor = System.Drawing.Color.Gray;
            this.cardKpi6.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardKpi6.TabIndex = 5;
            this.cardKpi6.Title = "";
            this.cardKpi6.TitleColor = System.Drawing.Color.Gray;
            this.cardKpi6.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardKpi6.Value = null;
            this.cardKpi6.ValueColor = System.Drawing.Color.Black;
            this.cardKpi6.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            // 
            // ReportControl
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.panelContent);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ReportControl";
            this.Size = new System.Drawing.Size(1200, 1020);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.flowPanelReportTypes.ResumeLayout(false);
            this.panelFilters.ResumeLayout(false);
            this.panelFilters.PerformLayout();
            this.panelCustomDateRange.ResumeLayout(false);
            this.panelCustomDateRange.PerformLayout();
            this.panelReportContent.ResumeLayout(false);
            this.panelReportContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReport)).EndInit();
            this.flowPanelKPICards.ResumeLayout(false);
            this.flowPanelKPICards.PerformLayout();
            this.panelContent.ResumeLayout(false);
            this.panelContent.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
