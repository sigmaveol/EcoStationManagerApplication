using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    partial class MultiProductStockOutForm
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
            this.panelMain = new Guna.UI2.WinForms.Guna2Panel();
            this.panelSummary = new Guna.UI2.WinForms.Guna2Panel();
            this.lblSummaryWarning = new System.Windows.Forms.Label();
            this.lblTotalQuantity = new System.Windows.Forms.Label();
            this.lblTotalRows = new System.Windows.Forms.Label();
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            this.btnAddRow = new Guna.UI2.WinForms.Guna2Button();
            this.labelError = new System.Windows.Forms.Label();
            this.btnCancel = new Guna.UI2.WinForms.Guna2Button();
            this.btnSave = new Guna.UI2.WinForms.Guna2Button();
            this.txtNotes = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.dtpStockOutDate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.lblStockOutDate = new System.Windows.Forms.Label();
            this.cmbPurpose = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblPurpose = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelMain.SuspendLayout();
            this.panelSummary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.panelSummary);
            this.panelMain.Controls.Add(this.dgvProducts);
            this.panelMain.Controls.Add(this.btnAddRow);
            this.panelMain.Controls.Add(this.labelError);
            this.panelMain.Controls.Add(this.btnCancel);
            this.panelMain.Controls.Add(this.btnSave);
            this.panelMain.Controls.Add(this.txtNotes);
            this.panelMain.Controls.Add(this.lblNotes);
            this.panelMain.Controls.Add(this.dtpStockOutDate);
            this.panelMain.Controls.Add(this.lblStockOutDate);
            this.panelMain.Controls.Add(this.cmbPurpose);
            this.panelMain.Controls.Add(this.lblPurpose);
            this.panelMain.Controls.Add(this.lblTitle);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(30);
            this.panelMain.Size = new System.Drawing.Size(1000, 750);
            this.panelMain.TabIndex = 0;
            // 
            // panelSummary
            // 
            this.panelSummary.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.panelSummary.BorderRadius = 8;
            this.panelSummary.BorderThickness = 1;
            this.panelSummary.Controls.Add(this.lblSummaryWarning);
            this.panelSummary.Controls.Add(this.lblTotalQuantity);
            this.panelSummary.Controls.Add(this.lblTotalRows);
            this.panelSummary.Location = new System.Drawing.Point(30, 580);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Padding = new System.Windows.Forms.Padding(15);
            this.panelSummary.Size = new System.Drawing.Size(940, 60);
            this.panelSummary.TabIndex = 20;
            // 
            // lblSummaryWarning
            // 
            this.lblSummaryWarning.AutoSize = true;
            this.lblSummaryWarning.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSummaryWarning.ForeColor = System.Drawing.Color.Green;
            this.lblSummaryWarning.Location = new System.Drawing.Point(650, 18);
            this.lblSummaryWarning.Name = "lblSummaryWarning";
            this.lblSummaryWarning.Size = new System.Drawing.Size(180, 23);
            this.lblSummaryWarning.TabIndex = 2;
            this.lblSummaryWarning.Text = "✓ Tất cả dòng hợp lệ";
            // 
            // lblTotalQuantity
            // 
            this.lblTotalQuantity.AutoSize = true;
            this.lblTotalQuantity.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalQuantity.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblTotalQuantity.Location = new System.Drawing.Point(350, 18);
            this.lblTotalQuantity.Name = "lblTotalQuantity";
            this.lblTotalQuantity.Size = new System.Drawing.Size(146, 23);
            this.lblTotalQuantity.TabIndex = 1;
            this.lblTotalQuantity.Text = "Tổng số lượng: 0";
            // 
            // lblTotalRows
            // 
            this.lblTotalRows.AutoSize = true;
            this.lblTotalRows.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalRows.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblTotalRows.Location = new System.Drawing.Point(15, 18);
            this.lblTotalRows.Name = "lblTotalRows";
            this.lblTotalRows.Size = new System.Drawing.Size(140, 23);
            this.lblTotalRows.TabIndex = 0;
            this.lblTotalRows.Text = "Tổng số dòng: 0";
            // 
            // dgvProducts
            // 
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AllowUserToDeleteRows = false;
            this.dgvProducts.BackgroundColor = System.Drawing.Color.White;
            this.dgvProducts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProducts.Location = new System.Drawing.Point(30, 262);
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.RowHeadersWidth = 51;
            this.dgvProducts.RowTemplate.Height = 30;
            this.dgvProducts.Size = new System.Drawing.Size(940, 308);
            this.dgvProducts.TabIndex = 19;
            // 
            // btnAddRow
            // 
            this.btnAddRow.BorderRadius = 8;
            this.btnAddRow.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAddRow.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnAddRow.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnAddRow.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnAddRow.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.btnAddRow.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnAddRow.ForeColor = System.Drawing.Color.White;
            this.btnAddRow.Location = new System.Drawing.Point(30, 210);
            this.btnAddRow.Name = "btnAddRow";
            this.btnAddRow.Size = new System.Drawing.Size(155, 46);
            this.btnAddRow.TabIndex = 18;
            this.btnAddRow.Text = "+ Thêm dòng";
            this.btnAddRow.Click += new System.EventHandler(this.btnAddRow_Click);
            // 
            // labelError
            // 
            this.labelError.AutoSize = true;
            this.labelError.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(24)))), ((int)(((byte)(61)))));
            this.labelError.Location = new System.Drawing.Point(30, 650);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(0, 20);
            this.labelError.TabIndex = 17;
            // 
            // btnCancel
            // 
            this.btnCancel.BorderRadius = 8;
            this.btnCancel.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCancel.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCancel.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCancel.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCancel.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(870, 700);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 40);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.BorderRadius = 8;
            this.btnSave.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSave.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSave.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(760, 700);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 40);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "Lưu phiếu";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtNotes
            // 
            this.txtNotes.BorderRadius = 8;
            this.txtNotes.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNotes.DefaultText = "";
            this.txtNotes.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtNotes.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtNotes.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtNotes.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtNotes.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.txtNotes.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtNotes.ForeColor = System.Drawing.Color.Black;
            this.txtNotes.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.txtNotes.Location = new System.Drawing.Point(30, 150);
            this.txtNotes.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.PlaceholderText = "Ghi chú (tùy chọn)";
            this.txtNotes.SelectedText = "";
            this.txtNotes.Size = new System.Drawing.Size(940, 50);
            this.txtNotes.TabIndex = 14;
            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblNotes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblNotes.Location = new System.Drawing.Point(30, 125);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(75, 23);
            this.lblNotes.TabIndex = 13;
            this.lblNotes.Text = "Ghi chú:";
            // 
            // dtpStockOutDate
            // 
            this.dtpStockOutDate.BorderRadius = 8;
            this.dtpStockOutDate.Checked = true;
            this.dtpStockOutDate.FillColor = System.Drawing.Color.White;
            this.dtpStockOutDate.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpStockOutDate.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.dtpStockOutDate.Location = new System.Drawing.Point(330, 80);
            this.dtpStockOutDate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpStockOutDate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpStockOutDate.Name = "dtpStockOutDate";
            this.dtpStockOutDate.Size = new System.Drawing.Size(300, 40);
            this.dtpStockOutDate.TabIndex = 8;
            this.dtpStockOutDate.Value = new System.DateTime(2024, 1, 1, 0, 0, 0, 0);
            // 
            // lblStockOutDate
            // 
            this.lblStockOutDate.AutoSize = true;
            this.lblStockOutDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblStockOutDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblStockOutDate.Location = new System.Drawing.Point(330, 55);
            this.lblStockOutDate.Name = "lblStockOutDate";
            this.lblStockOutDate.Size = new System.Drawing.Size(97, 23);
            this.lblStockOutDate.TabIndex = 7;
            this.lblStockOutDate.Text = "Ngày xuất:";
            // 
            // cmbPurpose
            // 
            this.cmbPurpose.BackColor = System.Drawing.Color.Transparent;
            this.cmbPurpose.BorderRadius = 8;
            this.cmbPurpose.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbPurpose.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPurpose.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.cmbPurpose.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.cmbPurpose.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbPurpose.ForeColor = System.Drawing.Color.Black;
            this.cmbPurpose.ItemHeight = 30;
            this.cmbPurpose.Location = new System.Drawing.Point(30, 80);
            this.cmbPurpose.Name = "cmbPurpose";
            this.cmbPurpose.Size = new System.Drawing.Size(280, 36);
            this.cmbPurpose.TabIndex = 6;
            // 
            // lblPurpose
            // 
            this.lblPurpose.AutoSize = true;
            this.lblPurpose.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblPurpose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblPurpose.Location = new System.Drawing.Point(30, 55);
            this.lblPurpose.Name = "lblPurpose";
            this.lblPurpose.Size = new System.Drawing.Size(128, 23);
            this.lblPurpose.TabIndex = 5;
            this.lblPurpose.Text = "Mục đích xuất:";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblTitle.Location = new System.Drawing.Point(30, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(145, 41);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Xuất kho";
            // 
            // MultiProductStockOutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 750);
            this.Controls.Add(this.panelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MultiProductStockOutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Xuất kho nhiều sản phẩm";
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.panelSummary.ResumeLayout(false);
            this.panelSummary.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna2Panel panelMain;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblPurpose;
        private Guna2ComboBox cmbPurpose;
        private System.Windows.Forms.Label lblStockOutDate;
        private Guna2DateTimePicker dtpStockOutDate;
        private System.Windows.Forms.Label lblNotes;
        private Guna2TextBox txtNotes;
        private Guna2Button btnSave;
        private Guna2Button btnCancel;
        private System.Windows.Forms.Label labelError;
        private Guna2Button btnAddRow;
        private System.Windows.Forms.DataGridView dgvProducts;
        private Guna2Panel panelSummary;
        private System.Windows.Forms.Label lblTotalRows;
        private System.Windows.Forms.Label lblTotalQuantity;
        private System.Windows.Forms.Label lblSummaryWarning;
    }
}

