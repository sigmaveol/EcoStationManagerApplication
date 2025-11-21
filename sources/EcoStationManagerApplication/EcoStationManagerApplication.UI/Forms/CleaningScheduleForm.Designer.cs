using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    partial class CleaningScheduleForm
    {
        private System.ComponentModel.IContainer components = null;

        private ComboBox cmbCleaningType;
        private DateTimePicker dtpCleaningDate;
        private DateTimePicker dtpCleaningTime;
        private ComboBox cbCleanedBy;
        private ComboBox cmbStatus;
        private TextBox txtNotes;
        private Button btnSave;
        private Button btnCancel;
        private Label lblCleaningType;
        private Label lblCleaningDate;
        private Label lblCleaningTime;
        private Label lblCleanedBy;
        private Label lblStatus;
        private Label lblNotes;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.cmbCleaningType = new System.Windows.Forms.ComboBox();
            this.dtpCleaningDate = new System.Windows.Forms.DateTimePicker();
            this.dtpCleaningTime = new System.Windows.Forms.DateTimePicker();
            this.cbCleanedBy = new System.Windows.Forms.ComboBox();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblCleaningType = new System.Windows.Forms.Label();
            this.lblCleaningDate = new System.Windows.Forms.Label();
            this.lblCleaningTime = new System.Windows.Forms.Label();
            this.lblCleanedBy = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblNotes = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbCleaningType
            // 
            this.cmbCleaningType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCleaningType.Items.AddRange(new object[] {
            "TANK",
            "PACKAGING"});
            this.cmbCleaningType.Location = new System.Drawing.Point(120, 18);
            this.cmbCleaningType.Name = "cmbCleaningType";
            this.cmbCleaningType.Size = new System.Drawing.Size(220, 24);
            this.cmbCleaningType.TabIndex = 0;
            // 
            // dtpCleaningDate
            // 
            this.dtpCleaningDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpCleaningDate.Location = new System.Drawing.Point(120, 58);
            this.dtpCleaningDate.Name = "dtpCleaningDate";
            this.dtpCleaningDate.Size = new System.Drawing.Size(200, 22);
            this.dtpCleaningDate.TabIndex = 1;
            // 
            // dtpCleaningTime
            // 
            this.dtpCleaningTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpCleaningTime.Location = new System.Drawing.Point(120, 98);
            this.dtpCleaningTime.Name = "dtpCleaningTime";
            this.dtpCleaningTime.ShowUpDown = true;
            this.dtpCleaningTime.Size = new System.Drawing.Size(200, 22);
            this.dtpCleaningTime.TabIndex = 2;
            // 
            // cbCleanedBy
            // 
            this.cbCleanedBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCleanedBy.Location = new System.Drawing.Point(120, 138);
            this.cbCleanedBy.Name = "cbCleanedBy";
            this.cbCleanedBy.Size = new System.Drawing.Size(220, 24);
            this.cbCleanedBy.TabIndex = 3;
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Location = new System.Drawing.Point(120, 178);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(220, 24);
            this.cmbStatus.TabIndex = 4;
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(120, 218);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(220, 60);
            this.txtNotes.TabIndex = 5;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(120, 300);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Lưu";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(220, 300);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblCleaningType
            // 
            this.lblCleaningType.Location = new System.Drawing.Point(20, 20);
            this.lblCleaningType.Name = "lblCleaningType";
            this.lblCleaningType.Size = new System.Drawing.Size(100, 23);
            this.lblCleaningType.TabIndex = 8;
            this.lblCleaningType.Text = "Loại vệ sinh:";
            // 
            // lblCleaningDate
            // 
            this.lblCleaningDate.Location = new System.Drawing.Point(20, 60);
            this.lblCleaningDate.Name = "lblCleaningDate";
            this.lblCleaningDate.Size = new System.Drawing.Size(100, 23);
            this.lblCleaningDate.TabIndex = 9;
            this.lblCleaningDate.Text = "Ngày vệ sinh:";
            // 
            // lblCleaningTime
            // 
            this.lblCleaningTime.Location = new System.Drawing.Point(20, 100);
            this.lblCleaningTime.Name = "lblCleaningTime";
            this.lblCleaningTime.Size = new System.Drawing.Size(100, 23);
            this.lblCleaningTime.TabIndex = 10;
            this.lblCleaningTime.Text = "Giờ vệ sinh:";
            // 
            // lblCleanedBy
            // 
            this.lblCleanedBy.Location = new System.Drawing.Point(20, 140);
            this.lblCleanedBy.Name = "lblCleanedBy";
            this.lblCleanedBy.Size = new System.Drawing.Size(100, 23);
            this.lblCleanedBy.TabIndex = 11;
            this.lblCleanedBy.Text = "Người phụ trách:";
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(20, 180);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(100, 23);
            this.lblStatus.TabIndex = 12;
            this.lblStatus.Text = "Trạng thái:";
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(20, 220);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(100, 23);
            this.lblNotes.TabIndex = 14;
            this.lblNotes.Text = "Ghi chú:";
            // 
            // CleaningScheduleForm
            // 
            this.ClientSize = new System.Drawing.Size(380, 360);
            this.Controls.Add(this.cmbCleaningType);
            this.Controls.Add(this.dtpCleaningDate);
            this.Controls.Add(this.dtpCleaningTime);
            this.Controls.Add(this.cbCleanedBy);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblCleaningType);
            this.Controls.Add(this.lblCleaningDate);
            this.Controls.Add(this.lblCleaningTime);
            this.Controls.Add(this.lblCleanedBy);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblNotes);
            this.Name = "CleaningScheduleForm";
            this.Text = "Thêm lịch vệ sinh";
            this.Load += new System.EventHandler(this.CleaningScheduleForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}


