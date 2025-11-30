using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace EcoStationManagerApplication.UI.Forms
{
    partial class CleaningScheduleForm
    {
        private System.ComponentModel.IContainer components = null;

        private Guna2ComboBox cmbCleaningType;
        private Guna2DateTimePicker dtpCleaningDate;
        private Guna2ComboBox cmbCleaningTime;
        private Guna2ComboBox cbCleanedBy;
        private Guna2ComboBox cmbStatus;
        private Guna2TextBox txtNotes;
        private Guna2Button btnSave;
        private Guna2Button btnCancel;
        private Guna2Panel panelMain;
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
            this.panelMain = new Guna2Panel();
            this.cmbCleaningType = new Guna2ComboBox();
            this.dtpCleaningDate = new Guna2DateTimePicker();
            this.cmbCleaningTime = new Guna2ComboBox();
            this.cbCleanedBy = new Guna2ComboBox();
            this.cmbStatus = new Guna2ComboBox();
            this.txtNotes = new Guna2TextBox();
            this.btnSave = new Guna2Button();
            this.btnCancel = new Guna2Button();
            this.lblCleaningType = new System.Windows.Forms.Label();
            this.lblCleaningDate = new System.Windows.Forms.Label();
            this.lblCleaningTime = new System.Windows.Forms.Label();
            this.lblCleanedBy = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblNotes = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Dock = DockStyle.Fill;
            this.panelMain.Padding = new Padding(20);
            this.panelMain.BackColor = System.Drawing.Color.White;
            // 
            // cmbCleaningType
            // 
            this.cmbCleaningType.BackColor = System.Drawing.Color.Transparent;
            this.cmbCleaningType.BorderRadius = 8;
            this.cmbCleaningType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCleaningType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCleaningType.FocusedColor = System.Drawing.Color.FromArgb(31, 107, 59);
            this.cmbCleaningType.FocusedState.BorderColor = System.Drawing.Color.FromArgb(31, 107, 59);
            this.cmbCleaningType.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbCleaningType.ForeColor = System.Drawing.Color.Black;
            this.cmbCleaningType.ItemHeight = 30;
            this.cmbCleaningType.Location = new System.Drawing.Point(140, 18);
            this.cmbCleaningType.Name = "cmbCleaningType";
            this.cmbCleaningType.Size = new System.Drawing.Size(220, 36);
            this.cmbCleaningType.TabIndex = 0;
            // 
            // dtpCleaningDate
            // 
            this.dtpCleaningDate.BorderRadius = 8;
            this.dtpCleaningDate.Checked = true;
            this.dtpCleaningDate.FillColor = System.Drawing.Color.White;
            this.dtpCleaningDate.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpCleaningDate.ForeColor = System.Drawing.Color.Black;
            this.dtpCleaningDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpCleaningDate.Location = new System.Drawing.Point(140, 66);
            this.dtpCleaningDate.Name = "dtpCleaningDate";
            this.dtpCleaningDate.Size = new System.Drawing.Size(220, 36);
            this.dtpCleaningDate.TabIndex = 1;
            // 
            // cmbCleaningTime
            // 
            this.cmbCleaningTime.BackColor = System.Drawing.Color.Transparent;
            this.cmbCleaningTime.BorderRadius = 8;
            this.cmbCleaningTime.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCleaningTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCleaningTime.FocusedColor = System.Drawing.Color.FromArgb(31, 107, 59);
            this.cmbCleaningTime.FocusedState.BorderColor = System.Drawing.Color.FromArgb(31, 107, 59);
            this.cmbCleaningTime.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbCleaningTime.ForeColor = System.Drawing.Color.Black;
            this.cmbCleaningTime.ItemHeight = 30;
            this.cmbCleaningTime.Location = new System.Drawing.Point(140, 114);
            this.cmbCleaningTime.Name = "cmbCleaningTime";
            this.cmbCleaningTime.Size = new System.Drawing.Size(220, 36);
            this.cmbCleaningTime.TabIndex = 2;
            // 
            // cbCleanedBy
            // 
            this.cbCleanedBy.BackColor = System.Drawing.Color.Transparent;
            this.cbCleanedBy.BorderRadius = 8;
            this.cbCleanedBy.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbCleanedBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCleanedBy.FocusedColor = System.Drawing.Color.FromArgb(31, 107, 59);
            this.cbCleanedBy.FocusedState.BorderColor = System.Drawing.Color.FromArgb(31, 107, 59);
            this.cbCleanedBy.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbCleanedBy.ForeColor = System.Drawing.Color.Black;
            this.cbCleanedBy.ItemHeight = 30;
            this.cbCleanedBy.Location = new System.Drawing.Point(140, 162);
            this.cbCleanedBy.Name = "cbCleanedBy";
            this.cbCleanedBy.Size = new System.Drawing.Size(260, 36);
            this.cbCleanedBy.TabIndex = 3;
            // 
            // cmbStatus
            // 
            this.cmbStatus.BackColor = System.Drawing.Color.Transparent;
            this.cmbStatus.BorderRadius = 8;
            this.cmbStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FocusedColor = System.Drawing.Color.FromArgb(31, 107, 59);
            this.cmbStatus.FocusedState.BorderColor = System.Drawing.Color.FromArgb(31, 107, 59);
            this.cmbStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbStatus.ForeColor = System.Drawing.Color.Black;
            this.cmbStatus.ItemHeight = 30;
            this.cmbStatus.Location = new System.Drawing.Point(140, 210);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(220, 36);
            this.cmbStatus.TabIndex = 4;
            // 
            // txtNotes
            // 
            this.txtNotes.BorderRadius = 8;
            this.txtNotes.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtNotes.ForeColor = System.Drawing.Color.Black;
            this.txtNotes.Location = new System.Drawing.Point(140, 258);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(320, 80);
            this.txtNotes.TabIndex = 5;
            // 
            // btnSave
            // 
            this.btnSave.BorderRadius = 8;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnSave.Text = "Lưu";
            this.btnSave.FillColor = System.Drawing.Color.FromArgb(31, 107, 59);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(290, 360);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 36);
            this.btnSave.TabIndex = 6;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BorderRadius = 8;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnCancel.Text = "Hủy";
            this.btnCancel.FillColor = System.Drawing.Color.FromArgb(125, 137, 149);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(385, 360);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 36);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblCleaningType
            // 
            this.lblCleaningType.Location = new System.Drawing.Point(20, 24);
            this.lblCleaningType.Name = "lblCleaningType";
            this.lblCleaningType.Size = new System.Drawing.Size(100, 23);
            this.lblCleaningType.TabIndex = 8;
            this.lblCleaningType.Text = "Loại vệ sinh:";
            // 
            // lblCleaningDate
            // 
            this.lblCleaningDate.Location = new System.Drawing.Point(20, 74);
            this.lblCleaningDate.Name = "lblCleaningDate";
            this.lblCleaningDate.Size = new System.Drawing.Size(100, 23);
            this.lblCleaningDate.TabIndex = 9;
            this.lblCleaningDate.Text = "Ngày vệ sinh:";
            // 
            // lblCleaningTime
            // 
            this.lblCleaningTime.Location = new System.Drawing.Point(20, 122);
            this.lblCleaningTime.Name = "lblCleaningTime";
            this.lblCleaningTime.Size = new System.Drawing.Size(100, 23);
            this.lblCleaningTime.TabIndex = 10;
            this.lblCleaningTime.Text = "Giờ vệ sinh:";
            // 
            // lblCleanedBy
            // 
            this.lblCleanedBy.Location = new System.Drawing.Point(20, 170);
            this.lblCleanedBy.Name = "lblCleanedBy";
            this.lblCleanedBy.Size = new System.Drawing.Size(100, 23);
            this.lblCleanedBy.TabIndex = 11;
            this.lblCleanedBy.Text = "Người phụ trách:";
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(20, 218);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(100, 23);
            this.lblStatus.TabIndex = 12;
            this.lblStatus.Text = "Trạng thái:";
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(20, 262);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(100, 23);
            this.lblNotes.TabIndex = 14;
            this.lblNotes.Text = "Ghi chú:";
            // 
            // CleaningScheduleForm
            // 
            this.ClientSize = new System.Drawing.Size(520, 420);
            this.panelMain.Controls.Add(this.lblCleaningType);
            this.panelMain.Controls.Add(this.cmbCleaningType);
            this.panelMain.Controls.Add(this.lblCleaningDate);
            this.panelMain.Controls.Add(this.dtpCleaningDate);
            this.panelMain.Controls.Add(this.lblCleaningTime);
            this.panelMain.Controls.Add(this.cmbCleaningTime);
            this.panelMain.Controls.Add(this.lblCleanedBy);
            this.panelMain.Controls.Add(this.cbCleanedBy);
            this.panelMain.Controls.Add(this.lblStatus);
            this.panelMain.Controls.Add(this.cmbStatus);
            this.panelMain.Controls.Add(this.lblNotes);
            this.panelMain.Controls.Add(this.txtNotes);
            this.panelMain.Controls.Add(this.btnSave);
            this.panelMain.Controls.Add(this.btnCancel);
            this.Controls.Add(this.panelMain);
            this.Name = "CleaningScheduleForm";
            this.Text = "Thêm lịch vệ sinh";
            this.Load += new System.EventHandler(this.CleaningScheduleForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

