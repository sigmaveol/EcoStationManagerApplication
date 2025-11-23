using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    partial class StockInDetailForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle;
        private Label lblReferenceNumberLabel;
        private Label lblReferenceNumber;
        private Label lblProductNameLabel;
        private Label lblProductName;
        private Label lblBatchNoLabel;
        private Label lblBatchNo;
        private Label lblSupplierNameLabel;
        private Label lblSupplierName;
        private Label lblQuantityLabel;
        private Label lblQuantity;
        private Label lblUnitPriceLabel;
        private Label lblUnitPrice;
        private Label lblTotalValueLabel;
        private Label lblTotalValue;
        private Label lblCreatedDateLabel;
        private Label lblCreatedDate;
        private Label lblCreatedByLabel;
        private Label lblCreatedBy;
        private Label lblExpiryDateLabel;
        private Label lblExpiryDate;
        private Label lblNotesLabel;
        private TextBox txtNotes;
        private Button btnClose;
        private Button btnExportExcel;
        private DataGridView dgvProducts;

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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblReferenceNumberLabel = new System.Windows.Forms.Label();
            this.lblReferenceNumber = new System.Windows.Forms.Label();
            this.lblProductNameLabel = new System.Windows.Forms.Label();
            this.lblProductName = new System.Windows.Forms.Label();
            this.lblBatchNoLabel = new System.Windows.Forms.Label();
            this.lblBatchNo = new System.Windows.Forms.Label();
            this.lblSupplierNameLabel = new System.Windows.Forms.Label();
            this.lblSupplierName = new System.Windows.Forms.Label();
            this.lblQuantityLabel = new System.Windows.Forms.Label();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.lblUnitPriceLabel = new System.Windows.Forms.Label();
            this.lblUnitPrice = new System.Windows.Forms.Label();
            this.lblTotalValueLabel = new System.Windows.Forms.Label();
            this.lblTotalValue = new System.Windows.Forms.Label();
            this.lblCreatedDateLabel = new System.Windows.Forms.Label();
            this.lblCreatedDate = new System.Windows.Forms.Label();
            this.lblCreatedByLabel = new System.Windows.Forms.Label();
            this.lblCreatedBy = new System.Windows.Forms.Label();
            this.lblExpiryDateLabel = new System.Windows.Forms.Label();
            this.lblExpiryDate = new System.Windows.Forms.Label();
            this.lblNotesLabel = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 16);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(279, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Chi tiết phiếu nhập kho";
            // 
            // lblReferenceNumberLabel
            // 
            this.lblReferenceNumberLabel.AutoSize = true;
            this.lblReferenceNumberLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblReferenceNumberLabel.Location = new System.Drawing.Point(20, 56);
            this.lblReferenceNumberLabel.Name = "lblReferenceNumberLabel";
            this.lblReferenceNumberLabel.Size = new System.Drawing.Size(78, 20);
            this.lblReferenceNumberLabel.TabIndex = 1;
            this.lblReferenceNumberLabel.Text = "Mã phiếu:";
            // 
            // lblReferenceNumber
            // 
            this.lblReferenceNumber.AutoSize = true;
            this.lblReferenceNumber.Location = new System.Drawing.Point(120, 56);
            this.lblReferenceNumber.Name = "lblReferenceNumber";
            this.lblReferenceNumber.Size = new System.Drawing.Size(0, 16);
            this.lblReferenceNumber.TabIndex = 2;
            // 
            // lblProductNameLabel
            // 
            this.lblProductNameLabel.AutoSize = true;
            this.lblProductNameLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblProductNameLabel.Location = new System.Drawing.Point(20, 80);
            this.lblProductNameLabel.Name = "lblProductNameLabel";
            this.lblProductNameLabel.Size = new System.Drawing.Size(82, 20);
            this.lblProductNameLabel.TabIndex = 3;
            this.lblProductNameLabel.Text = "Sản phẩm:";
            // 
            // lblProductName
            // 
            this.lblProductName.AutoSize = true;
            this.lblProductName.Location = new System.Drawing.Point(120, 80);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(0, 16);
            this.lblProductName.TabIndex = 4;
            // 
            // lblBatchNoLabel
            // 
            this.lblBatchNoLabel.AutoSize = true;
            this.lblBatchNoLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblBatchNoLabel.Location = new System.Drawing.Point(20, 104);
            this.lblBatchNoLabel.Name = "lblBatchNoLabel";
            this.lblBatchNoLabel.Size = new System.Drawing.Size(69, 20);
            this.lblBatchNoLabel.TabIndex = 5;
            this.lblBatchNoLabel.Text = "Lô hàng:";
            // 
            // lblBatchNo
            // 
            this.lblBatchNo.AutoSize = true;
            this.lblBatchNo.Location = new System.Drawing.Point(120, 104);
            this.lblBatchNo.Name = "lblBatchNo";
            this.lblBatchNo.Size = new System.Drawing.Size(0, 16);
            this.lblBatchNo.TabIndex = 6;
            // 
            // lblSupplierNameLabel
            // 
            this.lblSupplierNameLabel.AutoSize = true;
            this.lblSupplierNameLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSupplierNameLabel.Location = new System.Drawing.Point(20, 128);
            this.lblSupplierNameLabel.Name = "lblSupplierNameLabel";
            this.lblSupplierNameLabel.Size = new System.Drawing.Size(108, 20);
            this.lblSupplierNameLabel.TabIndex = 7;
            this.lblSupplierNameLabel.Text = "Nhà cung cấp:";
            // 
            // lblSupplierName
            // 
            this.lblSupplierName.AutoSize = true;
            this.lblSupplierName.Location = new System.Drawing.Point(150, 128);
            this.lblSupplierName.Name = "lblSupplierName";
            this.lblSupplierName.Size = new System.Drawing.Size(0, 16);
            this.lblSupplierName.TabIndex = 8;
            // 
            // lblQuantityLabel
            // 
            this.lblQuantityLabel.AutoSize = true;
            this.lblQuantityLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblQuantityLabel.Location = new System.Drawing.Point(20, 152);
            this.lblQuantityLabel.Name = "lblQuantityLabel";
            this.lblQuantityLabel.Size = new System.Drawing.Size(75, 20);
            this.lblQuantityLabel.TabIndex = 9;
            this.lblQuantityLabel.Text = "Số lượng:";
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new System.Drawing.Point(120, 152);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(0, 16);
            this.lblQuantity.TabIndex = 10;
            // 
            // lblUnitPriceLabel
            // 
            this.lblUnitPriceLabel.AutoSize = true;
            this.lblUnitPriceLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblUnitPriceLabel.Location = new System.Drawing.Point(20, 176);
            this.lblUnitPriceLabel.Name = "lblUnitPriceLabel";
            this.lblUnitPriceLabel.Size = new System.Drawing.Size(67, 20);
            this.lblUnitPriceLabel.TabIndex = 11;
            this.lblUnitPriceLabel.Text = "Đơn giá:";
            // 
            // lblUnitPrice
            // 
            this.lblUnitPrice.AutoSize = true;
            this.lblUnitPrice.Location = new System.Drawing.Point(120, 176);
            this.lblUnitPrice.Name = "lblUnitPrice";
            this.lblUnitPrice.Size = new System.Drawing.Size(0, 16);
            this.lblUnitPrice.TabIndex = 12;
            // 
            // lblTotalValueLabel
            // 
            this.lblTotalValueLabel.AutoSize = true;
            this.lblTotalValueLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotalValueLabel.Location = new System.Drawing.Point(20, 200);
            this.lblTotalValueLabel.Name = "lblTotalValueLabel";
            this.lblTotalValueLabel.Size = new System.Drawing.Size(80, 20);
            this.lblTotalValueLabel.TabIndex = 13;
            this.lblTotalValueLabel.Text = "Tổng tiền:";
            // 
            // lblTotalValue
            // 
            this.lblTotalValue.AutoSize = true;
            this.lblTotalValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotalValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.lblTotalValue.Location = new System.Drawing.Point(120, 200);
            this.lblTotalValue.Name = "lblTotalValue";
            this.lblTotalValue.Size = new System.Drawing.Size(0, 20);
            this.lblTotalValue.TabIndex = 14;
            // 
            // lblCreatedDateLabel
            // 
            this.lblCreatedDateLabel.AutoSize = true;
            this.lblCreatedDateLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCreatedDateLabel.Location = new System.Drawing.Point(20, 224);
            this.lblCreatedDateLabel.Name = "lblCreatedDateLabel";
            this.lblCreatedDateLabel.Size = new System.Drawing.Size(77, 20);
            this.lblCreatedDateLabel.TabIndex = 15;
            this.lblCreatedDateLabel.Text = "Ngày tạo:";
            // 
            // lblCreatedDate
            // 
            this.lblCreatedDate.AutoSize = true;
            this.lblCreatedDate.Location = new System.Drawing.Point(120, 224);
            this.lblCreatedDate.Name = "lblCreatedDate";
            this.lblCreatedDate.Size = new System.Drawing.Size(0, 16);
            this.lblCreatedDate.TabIndex = 16;
            // 
            // lblCreatedByLabel
            // 
            this.lblCreatedByLabel.AutoSize = true;
            this.lblCreatedByLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCreatedByLabel.Location = new System.Drawing.Point(20, 248);
            this.lblCreatedByLabel.Name = "lblCreatedByLabel";
            this.lblCreatedByLabel.Size = new System.Drawing.Size(84, 20);
            this.lblCreatedByLabel.TabIndex = 17;
            this.lblCreatedByLabel.Text = "Người tạo:";
            // 
            // lblCreatedBy
            // 
            this.lblCreatedBy.AutoSize = true;
            this.lblCreatedBy.Location = new System.Drawing.Point(120, 248);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new System.Drawing.Size(0, 16);
            this.lblCreatedBy.TabIndex = 18;
            // 
            // lblExpiryDateLabel
            // 
            this.lblExpiryDateLabel.AutoSize = true;
            this.lblExpiryDateLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblExpiryDateLabel.Location = new System.Drawing.Point(392, 80);
            this.lblExpiryDateLabel.Name = "lblExpiryDateLabel";
            this.lblExpiryDateLabel.Size = new System.Drawing.Size(102, 20);
            this.lblExpiryDateLabel.TabIndex = 19;
            this.lblExpiryDateLabel.Text = "Hạn sử dụng:";
            // 
            // lblExpiryDate
            // 
            this.lblExpiryDate.AutoSize = true;
            this.lblExpiryDate.Location = new System.Drawing.Point(410, 152);
            this.lblExpiryDate.Name = "lblExpiryDate";
            this.lblExpiryDate.Size = new System.Drawing.Size(0, 16);
            this.lblExpiryDate.TabIndex = 20;
            // 
            // lblNotesLabel
            // 
            this.lblNotesLabel.AutoSize = true;
            this.lblNotesLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblNotesLabel.Location = new System.Drawing.Point(392, 132);
            this.lblNotesLabel.Name = "lblNotesLabel";
            this.lblNotesLabel.Size = new System.Drawing.Size(66, 20);
            this.lblNotesLabel.TabIndex = 21;
            this.lblNotesLabel.Text = "Ghi chú:";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(396, 167);
            this.txtNotes.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ReadOnly = true;
            this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNotes.Size = new System.Drawing.Size(440, 73);
            this.txtNotes.TabIndex = 22;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(821, 263);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(123, 46);
            this.btnClose.TabIndex = 23;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.btnExportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportExcel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnExportExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportExcel.Location = new System.Drawing.Point(654, 263);
            this.btnExportExcel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(122, 46);
            this.btnExportExcel.TabIndex = 25;
            this.btnExportExcel.Text = "Xuất Excel";
            this.btnExportExcel.UseVisualStyleBackColor = false;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // dgvProducts
            // 
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AllowUserToDeleteRows = false;
            this.dgvProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProducts.Location = new System.Drawing.Point(20, 329);
            this.dgvProducts.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.RowHeadersWidth = 51;
            this.dgvProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.Size = new System.Drawing.Size(965, 272);
            this.dgvProducts.TabIndex = 24;
            this.dgvProducts.Visible = false;
            // 
            // StockInDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 612);
            this.Controls.Add(this.dgvProducts);
            this.Controls.Add(this.btnExportExcel);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.lblNotesLabel);
            this.Controls.Add(this.lblExpiryDate);
            this.Controls.Add(this.lblExpiryDateLabel);
            this.Controls.Add(this.lblCreatedBy);
            this.Controls.Add(this.lblCreatedByLabel);
            this.Controls.Add(this.lblCreatedDate);
            this.Controls.Add(this.lblCreatedDateLabel);
            this.Controls.Add(this.lblTotalValue);
            this.Controls.Add(this.lblTotalValueLabel);
            this.Controls.Add(this.lblUnitPrice);
            this.Controls.Add(this.lblUnitPriceLabel);
            this.Controls.Add(this.lblQuantity);
            this.Controls.Add(this.lblQuantityLabel);
            this.Controls.Add(this.lblSupplierName);
            this.Controls.Add(this.lblSupplierNameLabel);
            this.Controls.Add(this.lblBatchNo);
            this.Controls.Add(this.lblBatchNoLabel);
            this.Controls.Add(this.lblProductName);
            this.Controls.Add(this.lblProductNameLabel);
            this.Controls.Add(this.lblReferenceNumber);
            this.Controls.Add(this.lblReferenceNumberLabel);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StockInDetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chi tiết phiếu nhập kho";
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

