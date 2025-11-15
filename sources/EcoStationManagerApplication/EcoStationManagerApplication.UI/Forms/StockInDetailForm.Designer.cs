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
            this.lblTitle = new Label();
            this.lblReferenceNumberLabel = new Label();
            this.lblReferenceNumber = new Label();
            this.lblProductNameLabel = new Label();
            this.lblProductName = new Label();
            this.lblBatchNoLabel = new Label();
            this.lblBatchNo = new Label();
            this.lblSupplierNameLabel = new Label();
            this.lblSupplierName = new Label();
            this.lblQuantityLabel = new Label();
            this.lblQuantity = new Label();
            this.lblUnitPriceLabel = new Label();
            this.lblUnitPrice = new Label();
            this.lblTotalValueLabel = new Label();
            this.lblTotalValue = new Label();
            this.lblCreatedDateLabel = new Label();
            this.lblCreatedDate = new Label();
            this.lblCreatedByLabel = new Label();
            this.lblCreatedBy = new Label();
            this.lblExpiryDateLabel = new Label();
            this.lblExpiryDate = new Label();
            this.lblNotesLabel = new Label();
            this.txtNotes = new TextBox();
            this.btnClose = new Button();
            this.dgvProducts = new DataGridView();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTitle.Location = new Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(240, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Chi tiết phiếu nhập kho";

            // lblReferenceNumberLabel
            this.lblReferenceNumberLabel.AutoSize = true;
            this.lblReferenceNumberLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblReferenceNumberLabel.Location = new Point(20, 70);
            this.lblReferenceNumberLabel.Name = "lblReferenceNumberLabel";
            this.lblReferenceNumberLabel.Size = new Size(85, 20);
            this.lblReferenceNumberLabel.TabIndex = 1;
            this.lblReferenceNumberLabel.Text = "Mã phiếu:";

            // lblReferenceNumber
            this.lblReferenceNumber.AutoSize = true;
            this.lblReferenceNumber.Location = new Point(120, 70);
            this.lblReferenceNumber.Name = "lblReferenceNumber";
            this.lblReferenceNumber.Size = new Size(50, 20);
            this.lblReferenceNumber.TabIndex = 2;

            // lblProductNameLabel
            this.lblProductNameLabel.AutoSize = true;
            this.lblProductNameLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblProductNameLabel.Location = new Point(20, 100);
            this.lblProductNameLabel.Name = "lblProductNameLabel";
            this.lblProductNameLabel.Size = new Size(88, 20);
            this.lblProductNameLabel.TabIndex = 3;
            this.lblProductNameLabel.Text = "Sản phẩm:";

            // lblProductName
            this.lblProductName.AutoSize = true;
            this.lblProductName.Location = new Point(120, 100);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new Size(50, 20);
            this.lblProductName.TabIndex = 4;

            // lblBatchNoLabel
            this.lblBatchNoLabel.AutoSize = true;
            this.lblBatchNoLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblBatchNoLabel.Location = new Point(20, 130);
            this.lblBatchNoLabel.Name = "lblBatchNoLabel";
            this.lblBatchNoLabel.Size = new Size(78, 20);
            this.lblBatchNoLabel.TabIndex = 5;
            this.lblBatchNoLabel.Text = "Lô hàng:";

            // lblBatchNo
            this.lblBatchNo.AutoSize = true;
            this.lblBatchNo.Location = new Point(120, 130);
            this.lblBatchNo.Name = "lblBatchNo";
            this.lblBatchNo.Size = new Size(50, 20);
            this.lblBatchNo.TabIndex = 6;

            // lblSupplierNameLabel
            this.lblSupplierNameLabel.AutoSize = true;
            this.lblSupplierNameLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblSupplierNameLabel.Location = new Point(20, 160);
            this.lblSupplierNameLabel.Name = "lblSupplierNameLabel";
            this.lblSupplierNameLabel.Size = new Size(120, 20);
            this.lblSupplierNameLabel.TabIndex = 7;
            this.lblSupplierNameLabel.Text = "Nhà cung cấp:";

            // lblSupplierName
            this.lblSupplierName.AutoSize = true;
            this.lblSupplierName.Location = new Point(150, 160);
            this.lblSupplierName.Name = "lblSupplierName";
            this.lblSupplierName.Size = new Size(50, 20);
            this.lblSupplierName.TabIndex = 8;

            // lblQuantityLabel
            this.lblQuantityLabel.AutoSize = true;
            this.lblQuantityLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblQuantityLabel.Location = new Point(20, 190);
            this.lblQuantityLabel.Name = "lblQuantityLabel";
            this.lblQuantityLabel.Size = new Size(85, 20);
            this.lblQuantityLabel.TabIndex = 9;
            this.lblQuantityLabel.Text = "Số lượng:";

            // lblQuantity
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new Point(120, 190);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new Size(50, 20);
            this.lblQuantity.TabIndex = 10;

            // lblUnitPriceLabel
            this.lblUnitPriceLabel.AutoSize = true;
            this.lblUnitPriceLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblUnitPriceLabel.Location = new Point(20, 220);
            this.lblUnitPriceLabel.Name = "lblUnitPriceLabel";
            this.lblUnitPriceLabel.Size = new Size(70, 20);
            this.lblUnitPriceLabel.TabIndex = 11;
            this.lblUnitPriceLabel.Text = "Đơn giá:";

            // lblUnitPrice
            this.lblUnitPrice.AutoSize = true;
            this.lblUnitPrice.Location = new Point(120, 220);
            this.lblUnitPrice.Name = "lblUnitPrice";
            this.lblUnitPrice.Size = new Size(50, 20);
            this.lblUnitPrice.TabIndex = 12;

            // lblTotalValueLabel
            this.lblTotalValueLabel.AutoSize = true;
            this.lblTotalValueLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblTotalValueLabel.Location = new Point(20, 250);
            this.lblTotalValueLabel.Name = "lblTotalValueLabel";
            this.lblTotalValueLabel.Size = new Size(85, 20);
            this.lblTotalValueLabel.TabIndex = 13;
            this.lblTotalValueLabel.Text = "Tổng tiền:";

            // lblTotalValue
            this.lblTotalValue.AutoSize = true;
            this.lblTotalValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblTotalValue.ForeColor = Color.FromArgb(31, 107, 59);
            this.lblTotalValue.Location = new Point(120, 250);
            this.lblTotalValue.Name = "lblTotalValue";
            this.lblTotalValue.Size = new Size(50, 20);
            this.lblTotalValue.TabIndex = 14;

            // lblCreatedDateLabel
            this.lblCreatedDateLabel.AutoSize = true;
            this.lblCreatedDateLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblCreatedDateLabel.Location = new Point(20, 280);
            this.lblCreatedDateLabel.Name = "lblCreatedDateLabel";
            this.lblCreatedDateLabel.Size = new Size(85, 20);
            this.lblCreatedDateLabel.TabIndex = 15;
            this.lblCreatedDateLabel.Text = "Ngày tạo:";

            // lblCreatedDate
            this.lblCreatedDate.AutoSize = true;
            this.lblCreatedDate.Location = new Point(120, 280);
            this.lblCreatedDate.Name = "lblCreatedDate";
            this.lblCreatedDate.Size = new Size(50, 20);
            this.lblCreatedDate.TabIndex = 16;

            // lblCreatedByLabel
            this.lblCreatedByLabel.AutoSize = true;
            this.lblCreatedByLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblCreatedByLabel.Location = new Point(20, 310);
            this.lblCreatedByLabel.Name = "lblCreatedByLabel";
            this.lblCreatedByLabel.Size = new Size(95, 20);
            this.lblCreatedByLabel.TabIndex = 17;
            this.lblCreatedByLabel.Text = "Người tạo:";

            // lblCreatedBy
            this.lblCreatedBy.AutoSize = true;
            this.lblCreatedBy.Location = new Point(120, 310);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new Size(50, 20);
            this.lblCreatedBy.TabIndex = 18;

            // lblExpiryDateLabel
            this.lblExpiryDateLabel.AutoSize = true;
            this.lblExpiryDateLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblExpiryDateLabel.Location = new Point(300, 190);
            this.lblExpiryDateLabel.Name = "lblExpiryDateLabel";
            this.lblExpiryDateLabel.Size = new Size(105, 20);
            this.lblExpiryDateLabel.TabIndex = 19;
            this.lblExpiryDateLabel.Text = "Hạn sử dụng:";

            // lblExpiryDate
            this.lblExpiryDate.AutoSize = true;
            this.lblExpiryDate.Location = new Point(410, 190);
            this.lblExpiryDate.Name = "lblExpiryDate";
            this.lblExpiryDate.Size = new Size(50, 20);
            this.lblExpiryDate.TabIndex = 20;

            // lblNotesLabel
            this.lblNotesLabel.AutoSize = true;
            this.lblNotesLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblNotesLabel.Location = new Point(20, 340);
            this.lblNotesLabel.Name = "lblNotesLabel";
            this.lblNotesLabel.Size = new Size(70, 20);
            this.lblNotesLabel.TabIndex = 21;
            this.lblNotesLabel.Text = "Ghi chú:";

            // txtNotes
            this.txtNotes.Location = new Point(20, 365);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ReadOnly = true;
            this.txtNotes.ScrollBars = ScrollBars.Vertical;
            this.txtNotes.Size = new Size(550, 60);
            this.txtNotes.TabIndex = 22;

            // btnClose
            this.btnClose.Location = new Point(495, 440);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(75, 30);
            this.btnClose.TabIndex = 23;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);

            // dgvProducts
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AllowUserToDeleteRows = false;
            this.dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProducts.Location = new Point(20, 380);
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.RowHeadersWidth = 51;
            this.dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.Size = new Size(850, 200);
            this.dgvProducts.TabIndex = 24;
            this.dgvProducts.Visible = false;

            // StockInDetailForm
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(900, 600);
            this.Controls.Add(this.dgvProducts);
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
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StockInDetailForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Chi tiết phiếu nhập kho";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

