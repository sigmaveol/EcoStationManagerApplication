using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    partial class StaffControl
    {
        private System.ComponentModel.IContainer components = null;

        private Guna2Panel headerPanel;
        private Guna2Panel assignmentPanel;
        private Guna2Panel kpiPanel;
        private Guna2Panel cleaningSchedulePanel;
        private Guna2Panel dashboardPanel;
        private DataGridView dgvAssignments;
        private DataGridView dgvKPI;
        
        // Delivery Assignment Toolbar
        private Guna2Panel deliveryToolbar;
        private Guna2Button btnAssignDelivery;
        private Guna2Button btnUpdateDeliveryStatus;
        private Guna2Button btnExportDeliveryExcel;
        private Guna2Button btnExportDeliveryPdf;
        private Guna2TextBox txtDeliverySearch;
        private Guna2ComboBox cmbDeliveryStatusFilter;
        private Guna2DateTimePicker dtpDeliveryDateFilter;
        
        // WorkShift Toolbar
        private Guna2Panel workShiftToolbar;
        private Guna2Button btnAddWorkShift;
        private Guna2Button btnEditWorkShift;
        private Guna2Button btnDeleteWorkShift;
        private Guna2Button btnExportWorkShiftExcel;
        private Guna2Button btnExportWorkShiftPdf;
        private Guna2TextBox txtWorkShiftSearch;
        private Guna2ComboBox cmbWorkShiftRoleFilter;
        private Guna2DateTimePicker dtpWorkShiftDateFilter;
        
        // Dashboard KPI Labels
        private Label lblTodayShifts;
        private Label lblDeliveredOrders;
        private Label lblOverdueOrders;
        private Label lblTotalCOD;

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
            this.titleLabelAssign = new System.Windows.Forms.Label();
            this.titleLabelKPI = new System.Windows.Forms.Label();
            this.titleLabelCleaningSchedule = new System.Windows.Forms.Label();
            this.headerPanel = new Guna.UI2.WinForms.Guna2Panel();
            this.assignmentPanel = new Guna.UI2.WinForms.Guna2Panel();
            this.dgvAssignments = new System.Windows.Forms.DataGridView();
            this.deliveryToolbar = new Guna.UI2.WinForms.Guna2Panel();
            this.btnAssignDelivery = new Guna.UI2.WinForms.Guna2Button();
            this.btnUpdateDeliveryStatus = new Guna.UI2.WinForms.Guna2Button();
            this.btnExportDeliveryExcel = new Guna.UI2.WinForms.Guna2Button();
            this.btnExportDeliveryPdf = new Guna.UI2.WinForms.Guna2Button();
            this.txtDeliverySearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbDeliveryStatusFilter = new Guna.UI2.WinForms.Guna2ComboBox();
            this.dtpDeliveryDateFilter = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.kpiPanel = new Guna.UI2.WinForms.Guna2Panel();
            this.dgvKPI = new System.Windows.Forms.DataGridView();
            this.workShiftToolbar = new Guna.UI2.WinForms.Guna2Panel();
            this.btnAddWorkShift = new Guna.UI2.WinForms.Guna2Button();
            this.btnEditWorkShift = new Guna.UI2.WinForms.Guna2Button();
            this.btnDeleteWorkShift = new Guna.UI2.WinForms.Guna2Button();
            this.btnExportWorkShiftExcel = new Guna.UI2.WinForms.Guna2Button();
            this.btnExportWorkShiftPdf = new Guna.UI2.WinForms.Guna2Button();
            this.txtWorkShiftSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbWorkShiftRoleFilter = new Guna.UI2.WinForms.Guna2ComboBox();
            this.dtpWorkShiftDateFilter = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.cleaningSchedulePanel = new Guna.UI2.WinForms.Guna2Panel();
            this.calendarControl = new EcoStationManagerApplication.UI.Controls.CalendarControl();
            this.dashboardPanel = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTodayShifts = new System.Windows.Forms.Label();
            this.lblDeliveredOrders = new System.Windows.Forms.Label();
            this.lblOverdueOrders = new System.Windows.Forms.Label();
            this.lblTotalCOD = new System.Windows.Forms.Label();
            this.headerPanel.SuspendLayout();
            this.assignmentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssignments)).BeginInit();
            this.deliveryToolbar.SuspendLayout();
            this.kpiPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKPI)).BeginInit();
            this.workShiftToolbar.SuspendLayout();
            this.cleaningSchedulePanel.SuspendLayout();
            this.dashboardPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleLabelHeader
            // 
            this.titleLabelHeader.AutoSize = true;
            this.titleLabelHeader.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.titleLabelHeader.Location = new System.Drawing.Point(3, 0);
            this.titleLabelHeader.Name = "titleLabelHeader";
            this.titleLabelHeader.Size = new System.Drawing.Size(248, 37);
            this.titleLabelHeader.TabIndex = 0;
            this.titleLabelHeader.Text = "Nhân sự & Giao vận";
            // 
            // titleLabelAssign
            // 
            this.titleLabelAssign.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleLabelAssign.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.titleLabelAssign.Location = new System.Drawing.Point(15, 15);
            this.titleLabelAssign.Name = "titleLabelAssign";
            this.titleLabelAssign.Size = new System.Drawing.Size(807, 30);
            this.titleLabelAssign.TabIndex = 1;
            this.titleLabelAssign.Text = "Phân công nhân viên";
            // 
            // titleLabelKPI
            // 
            this.titleLabelKPI.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleLabelKPI.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.titleLabelKPI.Location = new System.Drawing.Point(15, 15);
            this.titleLabelKPI.Name = "titleLabelKPI";
            this.titleLabelKPI.Size = new System.Drawing.Size(807, 30);
            this.titleLabelKPI.TabIndex = 1;
            this.titleLabelKPI.Text = "Quản lý ca làm & KPI";
            // 
            // titleLabelCleaningSchedule
            // 
            this.titleLabelCleaningSchedule.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleLabelCleaningSchedule.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.titleLabelCleaningSchedule.Location = new System.Drawing.Point(15, 15);
            this.titleLabelCleaningSchedule.Name = "titleLabelCleaningSchedule";
            this.titleLabelCleaningSchedule.Size = new System.Drawing.Size(807, 30);
            this.titleLabelCleaningSchedule.TabIndex = 1;
            this.titleLabelCleaningSchedule.Text = "Lịch vệ sinh";
            // 
            // headerPanel
            // 
            this.headerPanel.Controls.Add(this.titleLabelHeader);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(10, 668);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(837, 60);
            this.headerPanel.TabIndex = 0;
            // 
            // assignmentPanel
            // 
            this.assignmentPanel.BackColor = System.Drawing.Color.White;
            this.assignmentPanel.Controls.Add(this.dgvAssignments);
            this.assignmentPanel.Controls.Add(this.deliveryToolbar);
            this.assignmentPanel.Controls.Add(this.titleLabelAssign);
            this.assignmentPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.assignmentPanel.FillColor = System.Drawing.Color.White;
            this.assignmentPanel.Location = new System.Drawing.Point(10, 728);
            this.assignmentPanel.Name = "assignmentPanel";
            this.assignmentPanel.Padding = new System.Windows.Forms.Padding(15);
            this.assignmentPanel.Size = new System.Drawing.Size(837, 320);
            this.assignmentPanel.TabIndex = 1;
            // 
            // dgvAssignments
            // 
            this.dgvAssignments.ColumnHeadersHeight = 29;
            this.dgvAssignments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAssignments.Location = new System.Drawing.Point(15, 95);
            this.dgvAssignments.Name = "dgvAssignments";
            this.dgvAssignments.RowHeadersWidth = 51;
            this.dgvAssignments.Size = new System.Drawing.Size(807, 210);
            this.dgvAssignments.TabIndex = 0;
            // 
            // deliveryToolbar
            // 
            this.deliveryToolbar.Controls.Add(this.btnAssignDelivery);
            this.deliveryToolbar.Controls.Add(this.btnUpdateDeliveryStatus);
            this.deliveryToolbar.Controls.Add(this.btnExportDeliveryExcel);
            this.deliveryToolbar.Controls.Add(this.btnExportDeliveryPdf);
            this.deliveryToolbar.Controls.Add(this.txtDeliverySearch);
            this.deliveryToolbar.Controls.Add(this.cmbDeliveryStatusFilter);
            this.deliveryToolbar.Controls.Add(this.dtpDeliveryDateFilter);
            this.deliveryToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.deliveryToolbar.Location = new System.Drawing.Point(15, 45);
            this.deliveryToolbar.Name = "deliveryToolbar";
            this.deliveryToolbar.Size = new System.Drawing.Size(807, 50);
            this.deliveryToolbar.TabIndex = 2;
            // 
            // btnAssignDelivery
            // 
            this.btnAssignDelivery.BorderRadius = 5;
            this.btnAssignDelivery.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAssignDelivery.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnAssignDelivery.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnAssignDelivery.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnAssignDelivery.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnAssignDelivery.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAssignDelivery.ForeColor = System.Drawing.Color.White;
            this.btnAssignDelivery.Location = new System.Drawing.Point(3, 8);
            this.btnAssignDelivery.Name = "btnAssignDelivery";
            this.btnAssignDelivery.Size = new System.Drawing.Size(102, 37);
            this.btnAssignDelivery.TabIndex = 0;
            this.btnAssignDelivery.Text = "Phân công";
            // 
            // btnUpdateDeliveryStatus
            // 
            this.btnUpdateDeliveryStatus.BorderRadius = 5;
            this.btnUpdateDeliveryStatus.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnUpdateDeliveryStatus.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnUpdateDeliveryStatus.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnUpdateDeliveryStatus.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnUpdateDeliveryStatus.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnUpdateDeliveryStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnUpdateDeliveryStatus.ForeColor = System.Drawing.Color.White;
            this.btnUpdateDeliveryStatus.Location = new System.Drawing.Point(112, 8);
            this.btnUpdateDeliveryStatus.Name = "btnUpdateDeliveryStatus";
            this.btnUpdateDeliveryStatus.Size = new System.Drawing.Size(148, 37);
            this.btnUpdateDeliveryStatus.TabIndex = 1;
            this.btnUpdateDeliveryStatus.Text = "Cập nhật trạng thái";
            // 
            // btnExportDeliveryExcel
            // 
            this.btnExportDeliveryExcel.BorderRadius = 5;
            this.btnExportDeliveryExcel.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnExportDeliveryExcel.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnExportDeliveryExcel.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnExportDeliveryExcel.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnExportDeliveryExcel.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnExportDeliveryExcel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnExportDeliveryExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportDeliveryExcel.Location = new System.Drawing.Point(663, 8);
            this.btnExportDeliveryExcel.Name = "btnExportDeliveryExcel";
            this.btnExportDeliveryExcel.Size = new System.Drawing.Size(80, 37);
            this.btnExportDeliveryExcel.TabIndex = 2;
            this.btnExportDeliveryExcel.Text = "Excel";
            // 
            // btnExportDeliveryPdf
            // 
            this.btnExportDeliveryPdf.BorderRadius = 5;
            this.btnExportDeliveryPdf.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnExportDeliveryPdf.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnExportDeliveryPdf.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnExportDeliveryPdf.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnExportDeliveryPdf.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.btnExportDeliveryPdf.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnExportDeliveryPdf.ForeColor = System.Drawing.Color.White;
            this.btnExportDeliveryPdf.Location = new System.Drawing.Point(753, 8);
            this.btnExportDeliveryPdf.Name = "btnExportDeliveryPdf";
            this.btnExportDeliveryPdf.Size = new System.Drawing.Size(80, 37);
            this.btnExportDeliveryPdf.TabIndex = 3;
            this.btnExportDeliveryPdf.Text = "PDF";
            // 
            // txtDeliverySearch
            // 
            this.txtDeliverySearch.BorderRadius = 5;
            this.txtDeliverySearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtDeliverySearch.DefaultText = "";
            this.txtDeliverySearch.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtDeliverySearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtDeliverySearch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtDeliverySearch.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtDeliverySearch.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtDeliverySearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtDeliverySearch.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtDeliverySearch.Location = new System.Drawing.Point(266, 12);
            this.txtDeliverySearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDeliverySearch.Name = "txtDeliverySearch";
            this.txtDeliverySearch.PlaceholderText = "Tìm kiếm...";
            this.txtDeliverySearch.SelectedText = "";
            this.txtDeliverySearch.Size = new System.Drawing.Size(150, 27);
            this.txtDeliverySearch.TabIndex = 4;
            // 
            // cmbDeliveryStatusFilter
            // 
            this.cmbDeliveryStatusFilter.BackColor = System.Drawing.Color.Transparent;
            this.cmbDeliveryStatusFilter.BorderRadius = 5;
            this.cmbDeliveryStatusFilter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbDeliveryStatusFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDeliveryStatusFilter.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbDeliveryStatusFilter.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbDeliveryStatusFilter.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbDeliveryStatusFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbDeliveryStatusFilter.ItemHeight = 30;
            this.cmbDeliveryStatusFilter.Items.AddRange(new object[] {
            "Tất cả",
            "Chờ giao",
            "Đang giao",
            "Đã giao",
            "Thất bại"});
            this.cmbDeliveryStatusFilter.Location = new System.Drawing.Point(426, 12);
            this.cmbDeliveryStatusFilter.Name = "cmbDeliveryStatusFilter";
            this.cmbDeliveryStatusFilter.Size = new System.Drawing.Size(100, 36);
            this.cmbDeliveryStatusFilter.TabIndex = 5;
            // 
            // dtpDeliveryDateFilter
            // 
            this.dtpDeliveryDateFilter.BorderRadius = 5;
            this.dtpDeliveryDateFilter.Checked = true;
            this.dtpDeliveryDateFilter.FillColor = System.Drawing.Color.White;
            this.dtpDeliveryDateFilter.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpDeliveryDateFilter.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDeliveryDateFilter.Location = new System.Drawing.Point(540, 13);
            this.dtpDeliveryDateFilter.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpDeliveryDateFilter.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpDeliveryDateFilter.Name = "dtpDeliveryDateFilter";
            this.dtpDeliveryDateFilter.Size = new System.Drawing.Size(107, 27);
            this.dtpDeliveryDateFilter.TabIndex = 6;
            this.dtpDeliveryDateFilter.Value = new System.DateTime(2024, 1, 1, 0, 0, 0, 0);
            // 
            // kpiPanel
            // 
            this.kpiPanel.BackColor = System.Drawing.Color.White;
            this.kpiPanel.Controls.Add(this.dgvKPI);
            this.kpiPanel.Controls.Add(this.workShiftToolbar);
            this.kpiPanel.Controls.Add(this.titleLabelKPI);
            this.kpiPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.kpiPanel.FillColor = System.Drawing.Color.White;
            this.kpiPanel.Location = new System.Drawing.Point(10, 1048);
            this.kpiPanel.Name = "kpiPanel";
            this.kpiPanel.Padding = new System.Windows.Forms.Padding(15);
            this.kpiPanel.Size = new System.Drawing.Size(837, 550);
            this.kpiPanel.TabIndex = 2;
            // 
            // dgvKPI
            // 
            this.dgvKPI.ColumnHeadersHeight = 29;
            this.dgvKPI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvKPI.Location = new System.Drawing.Point(15, 95);
            this.dgvKPI.Name = "dgvKPI";
            this.dgvKPI.RowHeadersWidth = 51;
            this.dgvKPI.Size = new System.Drawing.Size(807, 440);
            this.dgvKPI.TabIndex = 0;
            // 
            // workShiftToolbar
            // 
            this.workShiftToolbar.Controls.Add(this.btnAddWorkShift);
            this.workShiftToolbar.Controls.Add(this.btnEditWorkShift);
            this.workShiftToolbar.Controls.Add(this.btnDeleteWorkShift);
            this.workShiftToolbar.Controls.Add(this.btnExportWorkShiftExcel);
            this.workShiftToolbar.Controls.Add(this.btnExportWorkShiftPdf);
            this.workShiftToolbar.Controls.Add(this.txtWorkShiftSearch);
            this.workShiftToolbar.Controls.Add(this.cmbWorkShiftRoleFilter);
            this.workShiftToolbar.Controls.Add(this.dtpWorkShiftDateFilter);
            this.workShiftToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.workShiftToolbar.Location = new System.Drawing.Point(15, 45);
            this.workShiftToolbar.Name = "workShiftToolbar";
            this.workShiftToolbar.Size = new System.Drawing.Size(807, 50);
            this.workShiftToolbar.TabIndex = 2;
            // 
            // btnAddWorkShift
            // 
            this.btnAddWorkShift.BorderRadius = 5;
            this.btnAddWorkShift.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAddWorkShift.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnAddWorkShift.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnAddWorkShift.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnAddWorkShift.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnAddWorkShift.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAddWorkShift.ForeColor = System.Drawing.Color.White;
            this.btnAddWorkShift.Location = new System.Drawing.Point(0, 10);
            this.btnAddWorkShift.Name = "btnAddWorkShift";
            this.btnAddWorkShift.Size = new System.Drawing.Size(80, 30);
            this.btnAddWorkShift.TabIndex = 0;
            this.btnAddWorkShift.Text = "Thêm ca";
            // 
            // btnEditWorkShift
            // 
            this.btnEditWorkShift.BorderRadius = 5;
            this.btnEditWorkShift.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnEditWorkShift.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnEditWorkShift.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnEditWorkShift.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnEditWorkShift.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnEditWorkShift.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnEditWorkShift.ForeColor = System.Drawing.Color.White;
            this.btnEditWorkShift.Location = new System.Drawing.Point(90, 10);
            this.btnEditWorkShift.Name = "btnEditWorkShift";
            this.btnEditWorkShift.Size = new System.Drawing.Size(80, 30);
            this.btnEditWorkShift.TabIndex = 1;
            this.btnEditWorkShift.Text = "Sửa ca";
            // 
            // btnDeleteWorkShift
            // 
            this.btnDeleteWorkShift.BorderRadius = 5;
            this.btnDeleteWorkShift.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnDeleteWorkShift.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnDeleteWorkShift.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnDeleteWorkShift.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnDeleteWorkShift.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.btnDeleteWorkShift.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnDeleteWorkShift.ForeColor = System.Drawing.Color.White;
            this.btnDeleteWorkShift.Location = new System.Drawing.Point(180, 10);
            this.btnDeleteWorkShift.Name = "btnDeleteWorkShift";
            this.btnDeleteWorkShift.Size = new System.Drawing.Size(80, 30);
            this.btnDeleteWorkShift.TabIndex = 2;
            this.btnDeleteWorkShift.Text = "Xóa ca";
            // 
            // btnExportWorkShiftExcel
            // 
            this.btnExportWorkShiftExcel.BorderRadius = 5;
            this.btnExportWorkShiftExcel.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnExportWorkShiftExcel.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnExportWorkShiftExcel.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnExportWorkShiftExcel.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnExportWorkShiftExcel.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnExportWorkShiftExcel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnExportWorkShiftExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportWorkShiftExcel.Location = new System.Drawing.Point(661, 11);
            this.btnExportWorkShiftExcel.Name = "btnExportWorkShiftExcel";
            this.btnExportWorkShiftExcel.Size = new System.Drawing.Size(80, 30);
            this.btnExportWorkShiftExcel.TabIndex = 3;
            this.btnExportWorkShiftExcel.Text = "Excel";
            // 
            // btnExportWorkShiftPdf
            // 
            this.btnExportWorkShiftPdf.BorderRadius = 5;
            this.btnExportWorkShiftPdf.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnExportWorkShiftPdf.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnExportWorkShiftPdf.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnExportWorkShiftPdf.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnExportWorkShiftPdf.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.btnExportWorkShiftPdf.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnExportWorkShiftPdf.ForeColor = System.Drawing.Color.White;
            this.btnExportWorkShiftPdf.Location = new System.Drawing.Point(752, 10);
            this.btnExportWorkShiftPdf.Name = "btnExportWorkShiftPdf";
            this.btnExportWorkShiftPdf.Size = new System.Drawing.Size(80, 30);
            this.btnExportWorkShiftPdf.TabIndex = 4;
            this.btnExportWorkShiftPdf.Text = "PDF";
            // 
            // txtWorkShiftSearch
            // 
            this.txtWorkShiftSearch.BorderRadius = 5;
            this.txtWorkShiftSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtWorkShiftSearch.DefaultText = "";
            this.txtWorkShiftSearch.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtWorkShiftSearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtWorkShiftSearch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtWorkShiftSearch.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtWorkShiftSearch.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtWorkShiftSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtWorkShiftSearch.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtWorkShiftSearch.Location = new System.Drawing.Point(270, 12);
            this.txtWorkShiftSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtWorkShiftSearch.Name = "txtWorkShiftSearch";
            this.txtWorkShiftSearch.PlaceholderText = "Tìm kiếm...";
            this.txtWorkShiftSearch.SelectedText = "";
            this.txtWorkShiftSearch.Size = new System.Drawing.Size(150, 27);
            this.txtWorkShiftSearch.TabIndex = 5;
            // 
            // cmbWorkShiftRoleFilter
            // 
            this.cmbWorkShiftRoleFilter.BackColor = System.Drawing.Color.Transparent;
            this.cmbWorkShiftRoleFilter.BorderRadius = 5;
            this.cmbWorkShiftRoleFilter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbWorkShiftRoleFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWorkShiftRoleFilter.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbWorkShiftRoleFilter.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbWorkShiftRoleFilter.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbWorkShiftRoleFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbWorkShiftRoleFilter.ItemHeight = 30;
            this.cmbWorkShiftRoleFilter.Items.AddRange(new object[] {
            "Tất cả",
            "Quản trị viên",
            "Quản lý trạm",
            "Nhân viên",
            "Tài xế"});
            this.cmbWorkShiftRoleFilter.Location = new System.Drawing.Point(430, 12);
            this.cmbWorkShiftRoleFilter.Name = "cmbWorkShiftRoleFilter";
            this.cmbWorkShiftRoleFilter.Size = new System.Drawing.Size(100, 36);
            this.cmbWorkShiftRoleFilter.TabIndex = 6;
            // 
            // dtpWorkShiftDateFilter
            // 
            this.dtpWorkShiftDateFilter.BorderRadius = 5;
            this.dtpWorkShiftDateFilter.Checked = true;
            this.dtpWorkShiftDateFilter.FillColor = System.Drawing.Color.White;
            this.dtpWorkShiftDateFilter.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpWorkShiftDateFilter.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpWorkShiftDateFilter.Location = new System.Drawing.Point(540, 12);
            this.dtpWorkShiftDateFilter.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpWorkShiftDateFilter.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpWorkShiftDateFilter.Name = "dtpWorkShiftDateFilter";
            this.dtpWorkShiftDateFilter.Size = new System.Drawing.Size(80, 27);
            this.dtpWorkShiftDateFilter.TabIndex = 7;
            this.dtpWorkShiftDateFilter.Value = new System.DateTime(2024, 1, 1, 0, 0, 0, 0);
            // 
            // cleaningSchedulePanel
            // 
            this.cleaningSchedulePanel.BackColor = System.Drawing.Color.White;
            this.cleaningSchedulePanel.Controls.Add(this.calendarControl);
            this.cleaningSchedulePanel.Controls.Add(this.titleLabelCleaningSchedule);
            this.cleaningSchedulePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.cleaningSchedulePanel.FillColor = System.Drawing.Color.White;
            this.cleaningSchedulePanel.Location = new System.Drawing.Point(10, 10);
            this.cleaningSchedulePanel.Name = "cleaningSchedulePanel";
            this.cleaningSchedulePanel.Padding = new System.Windows.Forms.Padding(15);
            this.cleaningSchedulePanel.Size = new System.Drawing.Size(837, 658);
            this.cleaningSchedulePanel.TabIndex = 4;
            // 
            // calendarControl
            // 
            this.calendarControl.BackColor = System.Drawing.Color.White;
            this.calendarControl.CurrentDate = new System.DateTime(2025, 11, 20, 0, 0, 0, 0);
            this.calendarControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.calendarControl.HeaderBackground = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.calendarControl.Location = new System.Drawing.Point(15, 45);
            this.calendarControl.Margin = new System.Windows.Forms.Padding(4);
            this.calendarControl.Name = "calendarControl";
            this.calendarControl.OtherMonthColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.calendarControl.PrimaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.calendarControl.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(245)))), ((int)(((byte)(220)))));
            this.calendarControl.SelectedDate = new System.DateTime(2025, 11, 20, 0, 0, 0, 0);
            this.calendarControl.Size = new System.Drawing.Size(807, 598);
            this.calendarControl.TabIndex = 2;
            this.calendarControl.TodayColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(235)))), ((int)(((byte)(156)))));
            this.calendarControl.WeekendColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            // 
            // dashboardPanel
            // 
            this.dashboardPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dashboardPanel.BackColor = System.Drawing.Color.White;
            this.dashboardPanel.Controls.Add(this.lblTodayShifts);
            this.dashboardPanel.Controls.Add(this.lblDeliveredOrders);
            this.dashboardPanel.Controls.Add(this.lblOverdueOrders);
            this.dashboardPanel.Controls.Add(this.lblTotalCOD);
            this.dashboardPanel.FillColor = System.Drawing.Color.White;
            this.dashboardPanel.Location = new System.Drawing.Point(20, 770);
            this.dashboardPanel.Name = "dashboardPanel";
            this.dashboardPanel.Size = new System.Drawing.Size(817, 100);
            this.dashboardPanel.TabIndex = 3;
            // 
            // lblTodayShifts
            // 
            this.lblTodayShifts.AutoSize = true;
            this.lblTodayShifts.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTodayShifts.Location = new System.Drawing.Point(20, 20);
            this.lblTodayShifts.Name = "lblTodayShifts";
            this.lblTodayShifts.Size = new System.Drawing.Size(181, 23);
            this.lblTodayShifts.TabIndex = 0;
            this.lblTodayShifts.Text = "Số ca làm hôm nay: 0";
            // 
            // lblDeliveredOrders
            // 
            this.lblDeliveredOrders.AutoSize = true;
            this.lblDeliveredOrders.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDeliveredOrders.Location = new System.Drawing.Point(200, 20);
            this.lblDeliveredOrders.Name = "lblDeliveredOrders";
            this.lblDeliveredOrders.Size = new System.Drawing.Size(152, 23);
            this.lblDeliveredOrders.TabIndex = 1;
            this.lblDeliveredOrders.Text = "Số đơn đã giao: 0";
            // 
            // lblOverdueOrders
            // 
            this.lblOverdueOrders.AutoSize = true;
            this.lblOverdueOrders.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblOverdueOrders.ForeColor = System.Drawing.Color.Red;
            this.lblOverdueOrders.Location = new System.Drawing.Point(430, 20);
            this.lblOverdueOrders.Name = "lblOverdueOrders";
            this.lblOverdueOrders.Size = new System.Drawing.Size(115, 23);
            this.lblOverdueOrders.TabIndex = 2;
            this.lblOverdueOrders.Text = "Số đơn trễ: 0";
            // 
            // lblTotalCOD
            // 
            this.lblTotalCOD.AutoSize = true;
            this.lblTotalCOD.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalCOD.ForeColor = System.Drawing.Color.Green;
            this.lblTotalCOD.Location = new System.Drawing.Point(610, 20);
            this.lblTotalCOD.Name = "lblTotalCOD";
            this.lblTotalCOD.Size = new System.Drawing.Size(109, 23);
            this.lblTotalCOD.TabIndex = 3;
            this.lblTotalCOD.Text = "COD: 0 VNĐ";
            // 
            // StaffControl
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.kpiPanel);
            this.Controls.Add(this.assignmentPanel);
            this.Controls.Add(this.headerPanel);
            this.Controls.Add(this.cleaningSchedulePanel);
            this.Controls.Add(this.dashboardPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "StaffControl";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(857, 815);
            this.Load += new System.EventHandler(this.StaffControl_Load);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.assignmentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssignments)).EndInit();
            this.deliveryToolbar.ResumeLayout(false);
            this.kpiPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKPI)).EndInit();
            this.workShiftToolbar.ResumeLayout(false);
            this.cleaningSchedulePanel.ResumeLayout(false);
            this.dashboardPanel.ResumeLayout(false);
            this.dashboardPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        private Label titleLabelHeader;
        private Label titleLabelAssign;
        private Label titleLabelKPI;
        private Label titleLabelCleaningSchedule;
        private CalendarControl calendarControl;
    }
}