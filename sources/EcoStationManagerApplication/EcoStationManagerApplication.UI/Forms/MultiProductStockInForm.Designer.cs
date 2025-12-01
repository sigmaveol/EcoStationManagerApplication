using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    partial class MultiProductStockInForm
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
            this.lblTotalValue = new System.Windows.Forms.Label();
            this.lblTotalQuantity = new System.Windows.Forms.Label();
            this.lblTotalRows = new System.Windows.Forms.Label();
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            this.btnAddRow = new Guna.UI2.WinForms.Guna2Button();
            this.labelError = new System.Windows.Forms.Label();
            this.btnCancel = new Guna.UI2.WinForms.Guna2Button();
            this.btnSave = new Guna.UI2.WinForms.Guna2Button();
            this.txtNotes = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtBatchNo = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblBatchNo = new System.Windows.Forms.Label();
            this.dtpStockInDate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.lblStockInDate = new System.Windows.Forms.Label();
            this.cmbSupplier = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblSupplier = new System.Windows.Forms.Label();
            this.cmbSource = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblSource = new System.Windows.Forms.Label();
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
            this.panelMain.Controls.Add(this.txtBatchNo);
            this.panelMain.Controls.Add(this.lblBatchNo);
            this.panelMain.Controls.Add(this.dtpStockInDate);
            this.panelMain.Controls.Add(this.lblStockInDate);
            this.panelMain.Controls.Add(this.cmbSupplier);
            this.panelMain.Controls.Add(this.lblSupplier);
            this.panelMain.Controls.Add(this.cmbSource);
            this.panelMain.Controls.Add(this.lblSource);
            this.panelMain.Controls.Add(this.lblTitle);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(30);
            this.panelMain.Size = new System.Drawing.Size(1137, 750);
            this.panelMain.TabIndex = 0;
            // 
            // panelSummary
            // 
            this.panelSummary.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.panelSummary.BorderRadius = 8;
            this.panelSummary.BorderThickness = 1;
            this.panelSummary.Controls.Add(this.lblTotalValue);
            this.panelSummary.Controls.Add(this.lblTotalQuantity);
            this.panelSummary.Controls.Add(this.lblTotalRows);
            this.panelSummary.Location = new System.Drawing.Point(68, 578);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Padding = new System.Windows.Forms.Padding(15);
            this.panelSummary.Size = new System.Drawing.Size(940, 60);
            this.panelSummary.TabIndex = 20;
            // 
            // lblTotalValue
            // 
            this.lblTotalValue.AutoSize = true;
            this.lblTotalValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.lblTotalValue.Location = new System.Drawing.Point(650, 18);
            this.lblTotalValue.Name = "lblTotalValue";
            this.lblTotalValue.Size = new System.Drawing.Size(167, 23);
            this.lblTotalValue.TabIndex = 2;
            this.lblTotalValue.Text = "Tổng giá trị: 0 VNĐ";
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
            this.dgvProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProducts.BackgroundColor = System.Drawing.Color.White;
            this.dgvProducts.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProducts.Location = new System.Drawing.Point(30, 262);
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.RowHeadersWidth = 51;
            this.dgvProducts.RowTemplate.Height = 30;
            this.dgvProducts.Size = new System.Drawing.Size(1095, 288);
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
            this.btnAddRow.Location = new System.Drawing.Point(31, 217);
            this.btnAddRow.Name = "btnAddRow";
            this.btnAddRow.Size = new System.Drawing.Size(219, 39);
            this.btnAddRow.TabIndex = 18;
            this.btnAddRow.Text = "+ Thêm sản phẩm";
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
            this.btnCancel.Location = new System.Drawing.Point(983, 677);
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
            this.btnSave.Location = new System.Drawing.Point(786, 677);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(147, 40);
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
            this.txtNotes.Location = new System.Drawing.Point(464, 167);
            this.txtNotes.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.PlaceholderText = "Ghi chú (tùy chọn)";
            this.txtNotes.SelectedText = "";
            this.txtNotes.Size = new System.Drawing.Size(640, 69);
            this.txtNotes.TabIndex = 14;
            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblNotes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblNotes.Location = new System.Drawing.Point(460, 140);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(75, 23);
            this.lblNotes.TabIndex = 13;
            this.lblNotes.Text = "Ghi chú:";
            // 
            // txtBatchNo
            // 
            this.txtBatchNo.BorderRadius = 8;
            this.txtBatchNo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtBatchNo.DefaultText = "";
            this.txtBatchNo.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtBatchNo.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtBatchNo.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtBatchNo.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtBatchNo.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.txtBatchNo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtBatchNo.ForeColor = System.Drawing.Color.Black;
            this.txtBatchNo.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.txtBatchNo.Location = new System.Drawing.Point(446, 76);
            this.txtBatchNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBatchNo.MaxLength = 100;
            this.txtBatchNo.Name = "txtBatchNo";
            this.txtBatchNo.PlaceholderText = "Mã lô (tùy chọn)";
            this.txtBatchNo.SelectedText = "";
            this.txtBatchNo.Size = new System.Drawing.Size(300, 50);
            this.txtBatchNo.TabIndex = 15;
            // 
            // lblBatchNo
            // 
            this.lblBatchNo.AutoSize = true;
            this.lblBatchNo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblBatchNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblBatchNo.Location = new System.Drawing.Point(442, 49);
            this.lblBatchNo.Name = "lblBatchNo";
            this.lblBatchNo.Size = new System.Drawing.Size(60, 23);
            this.lblBatchNo.TabIndex = 16;
            this.lblBatchNo.Text = "Mã lô:";
            // 
            // dtpStockInDate
            // 
            this.dtpStockInDate.BorderRadius = 8;
            this.dtpStockInDate.Checked = true;
            this.dtpStockInDate.FillColor = System.Drawing.Color.White;
            this.dtpStockInDate.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpStockInDate.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.dtpStockInDate.Location = new System.Drawing.Point(784, 76);
            this.dtpStockInDate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpStockInDate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpStockInDate.Name = "dtpStockInDate";
            this.dtpStockInDate.Size = new System.Drawing.Size(320, 54);
            this.dtpStockInDate.TabIndex = 12;
            this.dtpStockInDate.Value = new System.DateTime(2024, 1, 1, 0, 0, 0, 0);
            // 
            // lblStockInDate
            // 
            this.lblStockInDate.AutoSize = true;
            this.lblStockInDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblStockInDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblStockInDate.Location = new System.Drawing.Point(780, 50);
            this.lblStockInDate.Name = "lblStockInDate";
            this.lblStockInDate.Size = new System.Drawing.Size(102, 23);
            this.lblStockInDate.TabIndex = 11;
            this.lblStockInDate.Text = "Ngày nhập:";
            // 
            // cmbSupplier
            // 
            this.cmbSupplier.BackColor = System.Drawing.Color.Transparent;
            this.cmbSupplier.BorderRadius = 8;
            this.cmbSupplier.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSupplier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSupplier.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.cmbSupplier.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.cmbSupplier.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbSupplier.ForeColor = System.Drawing.Color.Black;
            this.cmbSupplier.ItemHeight = 30;
            this.cmbSupplier.Location = new System.Drawing.Point(47, 167);
            this.cmbSupplier.Name = "cmbSupplier";
            this.cmbSupplier.Size = new System.Drawing.Size(280, 36);
            this.cmbSupplier.TabIndex = 10;
            // 
            // lblSupplier
            // 
            this.lblSupplier.AutoSize = true;
            this.lblSupplier.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSupplier.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblSupplier.Location = new System.Drawing.Point(44, 141);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(124, 23);
            this.lblSupplier.TabIndex = 9;
            this.lblSupplier.Text = "Nhà cung cấp:";
            // 
            // cmbSource
            // 
            this.cmbSource.BackColor = System.Drawing.Color.Transparent;
            this.cmbSource.BorderRadius = 8;
            this.cmbSource.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSource.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.cmbSource.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.cmbSource.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbSource.ForeColor = System.Drawing.Color.Black;
            this.cmbSource.ItemHeight = 30;
            this.cmbSource.Location = new System.Drawing.Point(47, 94);
            this.cmbSource.Name = "cmbSource";
            this.cmbSource.Size = new System.Drawing.Size(296, 36);
            this.cmbSource.TabIndex = 8;
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSource.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblSource.Location = new System.Drawing.Point(43, 68);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(114, 23);
            this.lblSource.TabIndex = 7;
            this.lblSource.Text = "Nguồn nhập:";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(156, 41);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Nhập kho";
            // 
            // MultiProductStockInForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1137, 750);
            this.Controls.Add(this.panelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MultiProductStockInForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Nhập kho nhiều sản phẩm";
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
        private System.Windows.Forms.Label lblSource;
        private Guna2ComboBox cmbSource;
        private System.Windows.Forms.Label lblSupplier;
        private Guna2ComboBox cmbSupplier;
        private System.Windows.Forms.Label lblStockInDate;
        private Guna2DateTimePicker dtpStockInDate;
        private System.Windows.Forms.Label lblNotes;
        private Guna2TextBox txtNotes;
        private Guna2TextBox txtBatchNo;
        private System.Windows.Forms.Label lblBatchNo;
        private Guna2Button btnSave;
        private Guna2Button btnCancel;
        private System.Windows.Forms.Label labelError;
        private Guna2Button btnAddRow;
        private System.Windows.Forms.DataGridView dgvProducts;
        private Guna2Panel panelSummary;
        private System.Windows.Forms.Label lblTotalRows;
        private System.Windows.Forms.Label lblTotalQuantity;
        private System.Windows.Forms.Label lblTotalValue;
    }
}

