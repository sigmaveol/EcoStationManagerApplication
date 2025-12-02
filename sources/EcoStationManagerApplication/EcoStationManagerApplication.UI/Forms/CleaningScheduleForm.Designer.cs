using System.Windows.Forms;
using Guna.UI2.WinForms;
using System.Drawing;
using EcoStationManagerApplication.UI.Common;

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
            this.panelMain = new Guna.UI2.WinForms.Guna2Panel();
            this.lblCleaningType = new System.Windows.Forms.Label();
            this.cmbCleaningType = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblCleaningDate = new System.Windows.Forms.Label();
            this.dtpCleaningDate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.lblCleaningTime = new System.Windows.Forms.Label();
            this.cmbCleaningTime = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblCleanedBy = new System.Windows.Forms.Label();
            this.cbCleanedBy = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbStatus = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtNotes = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnSave = new Guna.UI2.WinForms.Guna2Button();
            this.btnCancel = new Guna.UI2.WinForms.Guna2Button();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.White;
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
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(20);
            this.panelMain.Size = new System.Drawing.Size(540, 420);
            this.panelMain.TabIndex = 0;
            // 
            // lblCleaningType
            // 
            this.lblCleaningType.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblCleaningType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblCleaningType.Location = new System.Drawing.Point(7, 21);
            this.lblCleaningType.Name = "lblCleaningType";
            this.lblCleaningType.Size = new System.Drawing.Size(129, 23);
            this.lblCleaningType.TabIndex = 8;
            this.lblCleaningType.Text = "Loại vệ sinh:";
            // 
            // cmbCleaningType
            // 
            this.cmbCleaningType.BackColor = System.Drawing.Color.Transparent;
            this.cmbCleaningType.BorderRadius = 8;
            this.cmbCleaningType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCleaningType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCleaningType.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.cmbCleaningType.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.cmbCleaningType.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbCleaningType.ForeColor = System.Drawing.Color.Black;
            this.cmbCleaningType.ItemHeight = 30;
            this.cmbCleaningType.Location = new System.Drawing.Point(174, 18);
            this.cmbCleaningType.Name = "cmbCleaningType";
            this.cmbCleaningType.Size = new System.Drawing.Size(320, 36);
            this.cmbCleaningType.TabIndex = 0;
            // 
            // lblCleaningDate
            // 
            this.lblCleaningDate.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblCleaningDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblCleaningDate.Location = new System.Drawing.Point(10, 74);
            this.lblCleaningDate.Name = "lblCleaningDate";
            this.lblCleaningDate.Size = new System.Drawing.Size(124, 23);
            this.lblCleaningDate.TabIndex = 9;
            this.lblCleaningDate.Text = "Ngày vệ sinh:";
            // 
            // dtpCleaningDate
            // 
            this.dtpCleaningDate.BorderRadius = 8;
            this.dtpCleaningDate.Checked = true;
            this.dtpCleaningDate.FillColor = System.Drawing.Color.White;
            this.dtpCleaningDate.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.dtpCleaningDate.ForeColor = System.Drawing.Color.Black;
            this.dtpCleaningDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpCleaningDate.Location = new System.Drawing.Point(174, 66);
            this.dtpCleaningDate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpCleaningDate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpCleaningDate.Name = "dtpCleaningDate";
            this.dtpCleaningDate.Size = new System.Drawing.Size(320, 36);
            this.dtpCleaningDate.TabIndex = 1;
            this.dtpCleaningDate.Value = new System.DateTime(2025, 12, 2, 9, 4, 43, 339);
            // 
            // lblCleaningTime
            // 
            this.lblCleaningTime.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblCleaningTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblCleaningTime.Location = new System.Drawing.Point(17, 123);
            this.lblCleaningTime.Name = "lblCleaningTime";
            this.lblCleaningTime.Size = new System.Drawing.Size(116, 27);
            this.lblCleaningTime.TabIndex = 10;
            this.lblCleaningTime.Text = "Giờ vệ sinh:";
            // 
            // cmbCleaningTime
            // 
            this.cmbCleaningTime.BackColor = System.Drawing.Color.Transparent;
            this.cmbCleaningTime.BorderRadius = 8;
            this.cmbCleaningTime.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCleaningTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCleaningTime.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.cmbCleaningTime.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.cmbCleaningTime.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbCleaningTime.ForeColor = System.Drawing.Color.Black;
            this.cmbCleaningTime.ItemHeight = 30;
            this.cmbCleaningTime.Location = new System.Drawing.Point(174, 114);
            this.cmbCleaningTime.Name = "cmbCleaningTime";
            this.cmbCleaningTime.Size = new System.Drawing.Size(320, 36);
            this.cmbCleaningTime.TabIndex = 2;
            // 
            // lblCleanedBy
            // 
            this.lblCleanedBy.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblCleanedBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblCleanedBy.Location = new System.Drawing.Point(12, 170);
            this.lblCleanedBy.Name = "lblCleanedBy";
            this.lblCleanedBy.Size = new System.Drawing.Size(166, 28);
            this.lblCleanedBy.TabIndex = 11;
            this.lblCleanedBy.Text = "Người phụ trách:";
            // 
            // cbCleanedBy
            // 
            this.cbCleanedBy.BackColor = System.Drawing.Color.Transparent;
            this.cbCleanedBy.BorderRadius = 8;
            this.cbCleanedBy.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbCleanedBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCleanedBy.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.cbCleanedBy.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.cbCleanedBy.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cbCleanedBy.ForeColor = System.Drawing.Color.Black;
            this.cbCleanedBy.ItemHeight = 30;
            this.cbCleanedBy.Location = new System.Drawing.Point(173, 162);
            this.cbCleanedBy.Name = "cbCleanedBy";
            this.cbCleanedBy.Size = new System.Drawing.Size(321, 36);
            this.cbCleanedBy.TabIndex = 3;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblStatus.Location = new System.Drawing.Point(20, 218);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(100, 23);
            this.lblStatus.TabIndex = 12;
            this.lblStatus.Text = "Trạng thái:";
            // 
            // cmbStatus
            // 
            this.cmbStatus.BackColor = System.Drawing.Color.Transparent;
            this.cmbStatus.BorderRadius = 8;
            this.cmbStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.cmbStatus.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.cmbStatus.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbStatus.ForeColor = System.Drawing.Color.Black;
            this.cmbStatus.ItemHeight = 30;
            this.cmbStatus.Location = new System.Drawing.Point(174, 210);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(320, 36);
            this.cmbStatus.TabIndex = 4;
            // 
            // lblNotes
            // 
            this.lblNotes.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblNotes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblNotes.Location = new System.Drawing.Point(36, 258);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(100, 23);
            this.lblNotes.TabIndex = 14;
            this.lblNotes.Text = "Ghi chú:";
            // 
            // txtNotes
            // 
            this.txtNotes.BorderRadius = 8;
            this.txtNotes.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNotes.DefaultText = "";
            this.txtNotes.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtNotes.ForeColor = System.Drawing.Color.Black;
            this.txtNotes.Location = new System.Drawing.Point(174, 258);
            this.txtNotes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.PlaceholderText = "";
            this.txtNotes.SelectedText = "";
            this.txtNotes.Size = new System.Drawing.Size(320, 80);
            this.txtNotes.TabIndex = 5;
            // 
            // btnSave
            // 
            this.btnSave.BorderRadius = 8;
            this.btnSave.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(120)))), ((int)(((byte)(60)))));
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(258, 372);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(103, 36);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Lưu";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BorderRadius = 8;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(385, 372);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 36);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // CleaningScheduleForm
            // 
            this.AcceptButton = this.btnSave;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(540, 420);
            this.Controls.Add(this.panelMain);
            this.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "CleaningScheduleForm";
            this.Text = "Thêm lịch vệ sinh";
            this.Load += new System.EventHandler(this.CleaningScheduleForm_Load);
            this.panelMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
