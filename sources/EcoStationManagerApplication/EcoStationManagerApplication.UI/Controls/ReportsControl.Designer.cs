using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    partial class ReportsControl
    {
        private System.ComponentModel.IContainer components = null;

        // Khai báo các control sẽ được truy cập từ file .cs
        private Panel headerPanel;
        private Panel filterPanel;
        private Panel salesReportPanel;
        private Panel ecoImpactPanel;
        private Button btnExportReportPDF;
        private Button btnExportReportExcel;
        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;
        private ComboBox cmbReportType;
        private Button btnGenerateReport;

        // Các control "khung" nội bộ
        private FlowLayoutPanel statsReportFlowPanel;
        private TableLayoutPanel metricsEcoTable;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        // Designer sẽ chạy code này để vẽ giao diện
        private void InitializeComponent()
        {
            this.titleLabelHeader = new System.Windows.Forms.Label();
            this.flowPanelFilter = new System.Windows.Forms.FlowLayoutPanel();
            this.pFrom = new System.Windows.Forms.Panel();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.pTo = new System.Windows.Forms.Panel();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.pReportType = new System.Windows.Forms.Panel();
            this.lblReportType = new System.Windows.Forms.Label();
            this.cmbReportType = new System.Windows.Forms.ComboBox();
            this.btnGenerateReport = new System.Windows.Forms.Button();
            this.titleLabelSales = new System.Windows.Forms.Label();
            this.chartPanel = new System.Windows.Forms.Panel();
            this.chartLabel = new System.Windows.Forms.Label();
            this.titleLabelEco = new System.Windows.Forms.Label();
            this.infoPanelEco = new System.Windows.Forms.Panel();
            this.infoLabelEco = new System.Windows.Forms.Label();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.btnExportReportPDF = new System.Windows.Forms.Button();
            this.btnExportReportExcel = new System.Windows.Forms.Button();
            this.filterPanel = new System.Windows.Forms.Panel();
            this.salesReportPanel = new System.Windows.Forms.Panel();
            this.statsReportFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ecoImpactPanel = new System.Windows.Forms.Panel();
            this.metricsEcoTable = new System.Windows.Forms.TableLayoutPanel();
            this.flowPanelFilter.SuspendLayout();
            this.pFrom.SuspendLayout();
            this.pTo.SuspendLayout();
            this.pReportType.SuspendLayout();
            this.chartPanel.SuspendLayout();
            this.infoPanelEco.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.filterPanel.SuspendLayout();
            this.salesReportPanel.SuspendLayout();
            this.ecoImpactPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleLabelHeader
            // 
            this.titleLabelHeader.AutoSize = true;
            this.titleLabelHeader.Dock = System.Windows.Forms.DockStyle.Left;
            this.titleLabelHeader.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.titleLabelHeader.Location = new System.Drawing.Point(0, 0);
            this.titleLabelHeader.Name = "titleLabelHeader";
            this.titleLabelHeader.Size = new System.Drawing.Size(251, 37);
            this.titleLabelHeader.TabIndex = 0;
            this.titleLabelHeader.Text = "Báo cáo & Phân tích";
            // 
            // flowPanelFilter
            // 
            this.flowPanelFilter.Controls.Add(this.pFrom);
            this.flowPanelFilter.Controls.Add(this.pTo);
            this.flowPanelFilter.Controls.Add(this.pReportType);
            this.flowPanelFilter.Controls.Add(this.btnGenerateReport);
            this.flowPanelFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPanelFilter.Location = new System.Drawing.Point(15, 15);
            this.flowPanelFilter.Name = "flowPanelFilter";
            this.flowPanelFilter.Size = new System.Drawing.Size(870, 50);
            this.flowPanelFilter.TabIndex = 0;
            this.flowPanelFilter.WrapContents = false;
            // 
            // pFrom
            // 
            this.pFrom.Controls.Add(this.lblFrom);
            this.pFrom.Controls.Add(this.dtpFrom);
            this.pFrom.Location = new System.Drawing.Point(3, 3);
            this.pFrom.Name = "pFrom";
            this.pFrom.Size = new System.Drawing.Size(160, 60);
            this.pFrom.TabIndex = 0;
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblFrom.Location = new System.Drawing.Point(0, 0);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(66, 20);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "Từ ngày";
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(0, 25);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(150, 27);
            this.dtpFrom.TabIndex = 1;
            // 
            // pTo
            // 
            this.pTo.Controls.Add(this.lblTo);
            this.pTo.Controls.Add(this.dtpTo);
            this.pTo.Location = new System.Drawing.Point(169, 3);
            this.pTo.Name = "pTo";
            this.pTo.Size = new System.Drawing.Size(160, 60);
            this.pTo.TabIndex = 1;
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTo.Location = new System.Drawing.Point(0, 0);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(75, 20);
            this.lblTo.TabIndex = 0;
            this.lblTo.Text = "Đến ngày";
            // 
            // dtpTo
            // 
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(0, 25);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(150, 27);
            this.dtpTo.TabIndex = 1;
            // 
            // pReportType
            // 
            this.pReportType.Controls.Add(this.lblReportType);
            this.pReportType.Controls.Add(this.cmbReportType);
            this.pReportType.Location = new System.Drawing.Point(335, 3);
            this.pReportType.Name = "pReportType";
            this.pReportType.Size = new System.Drawing.Size(160, 60);
            this.pReportType.TabIndex = 2;
            // 
            // lblReportType
            // 
            this.lblReportType.AutoSize = true;
            this.lblReportType.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblReportType.Location = new System.Drawing.Point(0, 0);
            this.lblReportType.Name = "lblReportType";
            this.lblReportType.Size = new System.Drawing.Size(96, 20);
            this.lblReportType.TabIndex = 0;
            this.lblReportType.Text = "Loại báo cáo";
            // 
            // cmbReportType
            // 
            this.cmbReportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReportType.Items.AddRange(new object[] {
            "Doanh thu",
            "Đơn hàng",
            "Khách hàng",
            "Bao bì",
            "Tác động môi trường"});
            this.cmbReportType.Location = new System.Drawing.Point(0, 25);
            this.cmbReportType.Name = "cmbReportType";
            this.cmbReportType.Size = new System.Drawing.Size(150, 28);
            this.cmbReportType.TabIndex = 1;
            // 
            // btnGenerateReport
            // 
            this.btnGenerateReport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnGenerateReport.FlatAppearance.BorderSize = 0;
            this.btnGenerateReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerateReport.ForeColor = System.Drawing.Color.White;
            this.btnGenerateReport.Location = new System.Drawing.Point(508, 25);
            this.btnGenerateReport.Margin = new System.Windows.Forms.Padding(10, 25, 0, 0);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new System.Drawing.Size(100, 35);
            this.btnGenerateReport.TabIndex = 3;
            this.btnGenerateReport.Text = "Tạo báo cáo";
            this.btnGenerateReport.UseVisualStyleBackColor = false;
            // 
            // titleLabelSales
            // 
            this.titleLabelSales.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleLabelSales.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.titleLabelSales.Location = new System.Drawing.Point(15, 15);
            this.titleLabelSales.Name = "titleLabelSales";
            this.titleLabelSales.Size = new System.Drawing.Size(870, 30);
            this.titleLabelSales.TabIndex = 0;
            this.titleLabelSales.Text = "Báo cáo Doanh thu";
            // 
            // chartPanel
            // 
            this.chartPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chartPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
            this.chartPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chartPanel.Controls.Add(this.chartLabel);
            this.chartPanel.Location = new System.Drawing.Point(15, 45);
            this.chartPanel.Name = "chartPanel";
            this.chartPanel.Size = new System.Drawing.Size(870, 150);
            this.chartPanel.TabIndex = 1;
            // 
            // chartLabel
            // 
            this.chartLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chartLabel.Location = new System.Drawing.Point(0, 0);
            this.chartLabel.Name = "chartLabel";
            this.chartLabel.Size = new System.Drawing.Size(868, 148);
            this.chartLabel.TabIndex = 0;
            this.chartLabel.Text = "Biểu đồ doanh thu theo ngày/tuần/tháng (Nơi đây sẽ là Chart Control)";
            this.chartLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // titleLabelEco
            // 
            this.titleLabelEco.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleLabelEco.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.titleLabelEco.Location = new System.Drawing.Point(15, 15);
            this.titleLabelEco.Name = "titleLabelEco";
            this.titleLabelEco.Size = new System.Drawing.Size(870, 30);
            this.titleLabelEco.TabIndex = 2;
            this.titleLabelEco.Text = "Báo cáo Tác động Môi trường";
            // 
            // infoPanelEco
            // 
            this.infoPanelEco.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(230)))), ((int)(((byte)(201)))));
            this.infoPanelEco.Controls.Add(this.infoLabelEco);
            this.infoPanelEco.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.infoPanelEco.Location = new System.Drawing.Point(15, 145);
            this.infoPanelEco.Name = "infoPanelEco";
            this.infoPanelEco.Padding = new System.Windows.Forms.Padding(10);
            this.infoPanelEco.Size = new System.Drawing.Size(870, 40);
            this.infoPanelEco.TabIndex = 1;
            // 
            // infoLabelEco
            // 
            this.infoLabelEco.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoLabelEco.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.infoLabelEco.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(94)))), ((int)(((byte)(32)))));
            this.infoLabelEco.Location = new System.Drawing.Point(10, 10);
            this.infoLabelEco.Name = "infoLabelEco";
            this.infoLabelEco.Size = new System.Drawing.Size(850, 20);
            this.infoLabelEco.TabIndex = 0;
            this.infoLabelEco.Text = "Ước tính: Mỗi chai nhựa 500ml tiết kiệm được khoảng 0.25kg nhựa và 1.44kg CO2 so " +
    "với sản xuất chai mới.";
            this.infoLabelEco.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // headerPanel
            // 
            this.headerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.headerPanel.Controls.Add(this.titleLabelHeader);
            this.headerPanel.Controls.Add(this.btnExportReportPDF);
            this.headerPanel.Controls.Add(this.btnExportReportExcel);
            this.headerPanel.Location = new System.Drawing.Point(20, 20);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(900, 60);
            this.headerPanel.TabIndex = 0;
            // 
            // btnExportReportPDF
            // 
            this.btnExportReportPDF.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnExportReportPDF.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExportReportPDF.FlatAppearance.BorderSize = 0;
            this.btnExportReportPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportReportPDF.Location = new System.Drawing.Point(700, 0);
            this.btnExportReportPDF.Name = "btnExportReportPDF";
            this.btnExportReportPDF.Size = new System.Drawing.Size(100, 60);
            this.btnExportReportPDF.TabIndex = 1;
            this.btnExportReportPDF.Text = "Xuất PDF";
            this.btnExportReportPDF.UseVisualStyleBackColor = false;
            // 
            // btnExportReportExcel
            // 
            this.btnExportReportExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnExportReportExcel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExportReportExcel.FlatAppearance.BorderSize = 0;
            this.btnExportReportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportReportExcel.Location = new System.Drawing.Point(800, 0);
            this.btnExportReportExcel.Name = "btnExportReportExcel";
            this.btnExportReportExcel.Size = new System.Drawing.Size(100, 60);
            this.btnExportReportExcel.TabIndex = 2;
            this.btnExportReportExcel.Text = "Xuất Excel";
            this.btnExportReportExcel.UseVisualStyleBackColor = false;
            // 
            // filterPanel
            // 
            this.filterPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filterPanel.BackColor = System.Drawing.Color.White;
            this.filterPanel.Controls.Add(this.flowPanelFilter);
            this.filterPanel.Location = new System.Drawing.Point(20, 90);
            this.filterPanel.Name = "filterPanel";
            this.filterPanel.Padding = new System.Windows.Forms.Padding(15);
            this.filterPanel.Size = new System.Drawing.Size(900, 80);
            this.filterPanel.TabIndex = 1;
            // 
            // salesReportPanel
            // 
            this.salesReportPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.salesReportPanel.BackColor = System.Drawing.Color.White;
            this.salesReportPanel.Controls.Add(this.titleLabelSales);
            this.salesReportPanel.Controls.Add(this.chartPanel);
            this.salesReportPanel.Controls.Add(this.statsReportFlowPanel);
            this.salesReportPanel.Location = new System.Drawing.Point(20, 190);
            this.salesReportPanel.Name = "salesReportPanel";
            this.salesReportPanel.Padding = new System.Windows.Forms.Padding(15);
            this.salesReportPanel.Size = new System.Drawing.Size(900, 300);
            this.salesReportPanel.TabIndex = 2;
            // 
            // statsReportFlowPanel
            // 
            this.statsReportFlowPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statsReportFlowPanel.Location = new System.Drawing.Point(15, 205);
            this.statsReportFlowPanel.Name = "statsReportFlowPanel";
            this.statsReportFlowPanel.Size = new System.Drawing.Size(870, 90);
            this.statsReportFlowPanel.TabIndex = 2;
            // 
            // ecoImpactPanel
            // 
            this.ecoImpactPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ecoImpactPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.ecoImpactPanel.Controls.Add(this.metricsEcoTable);
            this.ecoImpactPanel.Controls.Add(this.infoPanelEco);
            this.ecoImpactPanel.Controls.Add(this.titleLabelEco);
            this.ecoImpactPanel.Location = new System.Drawing.Point(20, 510);
            this.ecoImpactPanel.Name = "ecoImpactPanel";
            this.ecoImpactPanel.Padding = new System.Windows.Forms.Padding(15);
            this.ecoImpactPanel.Size = new System.Drawing.Size(900, 200);
            this.ecoImpactPanel.TabIndex = 3;
            // 
            // metricsEcoTable
            // 
            this.metricsEcoTable.ColumnCount = 2;
            this.metricsEcoTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.metricsEcoTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.metricsEcoTable.Dock = System.Windows.Forms.DockStyle.Top;
            this.metricsEcoTable.Location = new System.Drawing.Point(15, 45);
            this.metricsEcoTable.Name = "metricsEcoTable";
            this.metricsEcoTable.RowCount = 3;
            this.metricsEcoTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.metricsEcoTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.metricsEcoTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.metricsEcoTable.Size = new System.Drawing.Size(870, 100);
            this.metricsEcoTable.TabIndex = 0;
            // 
            // ReportsControl
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.headerPanel);
            this.Controls.Add(this.filterPanel);
            this.Controls.Add(this.salesReportPanel);
            this.Controls.Add(this.ecoImpactPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ReportsControl";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(940, 740);
            this.flowPanelFilter.ResumeLayout(false);
            this.pFrom.ResumeLayout(false);
            this.pFrom.PerformLayout();
            this.pTo.ResumeLayout(false);
            this.pTo.PerformLayout();
            this.pReportType.ResumeLayout(false);
            this.pReportType.PerformLayout();
            this.chartPanel.ResumeLayout(false);
            this.infoPanelEco.ResumeLayout(false);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.filterPanel.ResumeLayout(false);
            this.salesReportPanel.ResumeLayout(false);
            this.ecoImpactPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private Label titleLabelHeader;
        private FlowLayoutPanel flowPanelFilter;
        private Panel pFrom;
        private Label lblFrom;
        private Panel pTo;
        private Label lblTo;
        private Panel pReportType;
        private Label lblReportType;
        private Label titleLabelSales;
        private Panel chartPanel;
        private Label chartLabel;
        private Label titleLabelEco;
        private Panel infoPanelEco;
        private Label infoLabelEco;

        // HÀM HELPER CreateReportStatCard ĐÃ BỊ XÓA (đã chuyển sang file .cs)
    }
}