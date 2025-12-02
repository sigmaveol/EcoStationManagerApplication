using System.Windows.Forms;
using System.Drawing;
using EcoStationManagerApplication.UI.Common;

namespace EcoStationManagerApplication.UI.Forms
{
    partial class WorkShiftForm
    {
        private System.ComponentModel.IContainer components = null;

        private ComboBox cbStaff;
        private DateTimePicker dtpShiftDate;
        private DateTimePicker dtpStart;
        private DateTimePicker dtpEnd;
        private NumericUpDown numKpi;
        private TextBox txtNotes;
        private Button btnSave;
        private Button btnCancel;
        private Label lblStaff;
        private Label lblShiftDate;
        private Label lblStart;
        private Label lblEnd;
        private Label lblKpi;
        private Label lblNotes;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.cbStaff = new System.Windows.Forms.ComboBox();
            this.dtpShiftDate = new System.Windows.Forms.DateTimePicker();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.numKpi = new System.Windows.Forms.NumericUpDown();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblStaff = new System.Windows.Forms.Label();
            this.lblShiftDate = new System.Windows.Forms.Label();
            this.lblStart = new System.Windows.Forms.Label();
            this.lblEnd = new System.Windows.Forms.Label();
            this.lblKpi = new System.Windows.Forms.Label();
            this.lblNotes = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numKpi)).BeginInit();
            this.SuspendLayout();
            // 
            // cbStaff
            // 
            this.cbStaff.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbStaff.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cbStaff.Location = new System.Drawing.Point(120, 18);
            this.cbStaff.Name = "cbStaff";
            this.cbStaff.Size = new System.Drawing.Size(277, 33);
            this.cbStaff.TabIndex = 0;
            // 
            // dtpShiftDate
            // 
            this.dtpShiftDate.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.dtpShiftDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpShiftDate.Location = new System.Drawing.Point(120, 58);
            this.dtpShiftDate.Name = "dtpShiftDate";
            this.dtpShiftDate.Size = new System.Drawing.Size(277, 32);
            this.dtpShiftDate.TabIndex = 1;
            // 
            // dtpStart
            // 
            this.dtpStart.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpStart.Location = new System.Drawing.Point(120, 98);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.ShowUpDown = true;
            this.dtpStart.Size = new System.Drawing.Size(277, 32);
            this.dtpStart.TabIndex = 2;
            // 
            // dtpEnd
            // 
            this.dtpEnd.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpEnd.Location = new System.Drawing.Point(120, 138);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.ShowUpDown = true;
            this.dtpEnd.Size = new System.Drawing.Size(277, 32);
            this.dtpEnd.TabIndex = 3;
            // 
            // numKpi
            // 
            this.numKpi.DecimalPlaces = 2;
            this.numKpi.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.numKpi.Location = new System.Drawing.Point(120, 178);
            this.numKpi.Name = "numKpi";
            this.numKpi.Size = new System.Drawing.Size(277, 32);
            this.numKpi.TabIndex = 4;
            // 
            // txtNotes
            // 
            this.txtNotes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNotes.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtNotes.Location = new System.Drawing.Point(120, 224);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(277, 60);
            this.txtNotes.TabIndex = 5;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(120)))), ((int)(((byte)(60)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(175, 316);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(87, 42);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnCancel.Location = new System.Drawing.Point(299, 316);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 43);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblStaff
            // 
            this.lblStaff.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblStaff.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblStaff.Location = new System.Drawing.Point(25, 20);
            this.lblStaff.Name = "lblStaff";
            this.lblStaff.Size = new System.Drawing.Size(100, 23);
            this.lblStaff.TabIndex = 8;
            this.lblStaff.Text = "Nhân viên:";
            // 
            // lblShiftDate
            // 
            this.lblShiftDate.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblShiftDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblShiftDate.Location = new System.Drawing.Point(20, 62);
            this.lblShiftDate.Name = "lblShiftDate";
            this.lblShiftDate.Size = new System.Drawing.Size(100, 23);
            this.lblShiftDate.TabIndex = 9;
            this.lblShiftDate.Text = "Ngày ca:";
            // 
            // lblStart
            // 
            this.lblStart.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblStart.Location = new System.Drawing.Point(20, 102);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(100, 23);
            this.lblStart.TabIndex = 10;
            this.lblStart.Text = "Giờ bắt đầu:";
            // 
            // lblEnd
            // 
            this.lblEnd.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblEnd.Location = new System.Drawing.Point(20, 143);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(100, 23);
            this.lblEnd.TabIndex = 11;
            this.lblEnd.Text = "Giờ kết thúc:";
            // 
            // lblKpi
            // 
            this.lblKpi.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblKpi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblKpi.Location = new System.Drawing.Point(20, 183);
            this.lblKpi.Name = "lblKpi";
            this.lblKpi.Size = new System.Drawing.Size(100, 23);
            this.lblKpi.TabIndex = 12;
            this.lblKpi.Text = "KPI (%):";
            // 
            // lblNotes
            // 
            this.lblNotes.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblNotes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblNotes.Location = new System.Drawing.Point(20, 223);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(100, 23);
            this.lblNotes.TabIndex = 13;
            this.lblNotes.Text = "Ghi chú:";
            // 
            // WorkShiftForm
            // 
            this.AcceptButton = this.btnSave;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(437, 380);
            this.Controls.Add(this.cbStaff);
            this.Controls.Add(this.dtpShiftDate);
            this.Controls.Add(this.dtpStart);
            this.Controls.Add(this.dtpEnd);
            this.Controls.Add(this.numKpi);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblStaff);
            this.Controls.Add(this.lblShiftDate);
            this.Controls.Add(this.lblStart);
            this.Controls.Add(this.lblEnd);
            this.Controls.Add(this.lblKpi);
            this.Controls.Add(this.lblNotes);
            this.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "WorkShiftForm";
            this.Text = "Thêm ca làm việc";
            this.Load += new System.EventHandler(this.WorkShiftForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numKpi)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
