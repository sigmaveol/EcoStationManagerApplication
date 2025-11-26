using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    partial class PackagingTransactionDetailForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle;
        private Label lblReferenceNumberLabel;
        private Label lblReferenceNumber;
        private Label lblCustomerNameLabel;
        private Label lblCustomerName;
        private Label lblCustomerPhoneLabel;
        private Label lblCustomerPhone;
        private Label lblPackagingNameLabel;
        private Label lblPackagingName;
        private Label lblBarcodeLabel;
        private Label lblBarcode;
        private Label lblTransactionTypeLabel;
        private Label lblTransactionType;
        private Label lblOwnershipTypeLabel;
        private Label lblOwnershipType;
        private Label lblQuantityLabel;
        private Label lblQuantity;
        private Label lblDepositPriceLabel;
        private Label lblDepositPrice;
        private Label lblRefundAmountLabel;
        private Label lblRefundAmount;
        private Label lblCreatedDateLabel;
        private Label lblCreatedDate;
        private Label lblCreatedByLabel;
        private Label lblCreatedBy;
        private Label lblNotesLabel;
        private TextBox txtNotes;
        private Button btnClose;
        private Button btnExportExcel;

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
            this.lblCustomerNameLabel = new System.Windows.Forms.Label();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.lblCustomerPhoneLabel = new System.Windows.Forms.Label();
            this.lblCustomerPhone = new System.Windows.Forms.Label();
            this.lblPackagingNameLabel = new System.Windows.Forms.Label();
            this.lblPackagingName = new System.Windows.Forms.Label();
            this.lblBarcodeLabel = new System.Windows.Forms.Label();
            this.lblBarcode = new System.Windows.Forms.Label();
            this.lblTransactionTypeLabel = new System.Windows.Forms.Label();
            this.lblTransactionType = new System.Windows.Forms.Label();
            this.lblOwnershipTypeLabel = new System.Windows.Forms.Label();
            this.lblOwnershipType = new System.Windows.Forms.Label();
            this.lblQuantityLabel = new System.Windows.Forms.Label();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.lblDepositPriceLabel = new System.Windows.Forms.Label();
            this.lblDepositPrice = new System.Windows.Forms.Label();
            this.lblRefundAmountLabel = new System.Windows.Forms.Label();
            this.lblRefundAmount = new System.Windows.Forms.Label();
            this.lblCreatedDateLabel = new System.Windows.Forms.Label();
            this.lblCreatedDate = new System.Windows.Forms.Label();
            this.lblCreatedByLabel = new System.Windows.Forms.Label();
            this.lblCreatedBy = new System.Windows.Forms.Label();
            this.lblNotesLabel = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 16);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(286, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Chi tiết giao dịch bao bì";
            // 
            // lblReferenceNumberLabel
            // 
            this.lblReferenceNumberLabel.AutoSize = true;
            this.lblReferenceNumberLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblReferenceNumberLabel.Location = new System.Drawing.Point(20, 56);
            this.lblReferenceNumberLabel.Name = "lblReferenceNumberLabel";
            this.lblReferenceNumberLabel.Size = new System.Drawing.Size(102, 20);
            this.lblReferenceNumberLabel.TabIndex = 1;
            this.lblReferenceNumberLabel.Text = "Mã giao dịch:";
            // 
            // lblReferenceNumber
            // 
            this.lblReferenceNumber.AutoSize = true;
            this.lblReferenceNumber.Location = new System.Drawing.Point(150, 56);
            this.lblReferenceNumber.Name = "lblReferenceNumber";
            this.lblReferenceNumber.Size = new System.Drawing.Size(0, 16);
            this.lblReferenceNumber.TabIndex = 2;
            // 
            // lblCustomerNameLabel
            // 
            this.lblCustomerNameLabel.AutoSize = true;
            this.lblCustomerNameLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCustomerNameLabel.Location = new System.Drawing.Point(20, 80);
            this.lblCustomerNameLabel.Name = "lblCustomerNameLabel";
            this.lblCustomerNameLabel.Size = new System.Drawing.Size(122, 20);
            this.lblCustomerNameLabel.TabIndex = 3;
            this.lblCustomerNameLabel.Text = "Tên khách hàng:";
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.AutoSize = true;
            this.lblCustomerName.Location = new System.Drawing.Point(150, 80);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(0, 16);
            this.lblCustomerName.TabIndex = 4;
            // 
            // lblCustomerPhoneLabel
            // 
            this.lblCustomerPhoneLabel.AutoSize = true;
            this.lblCustomerPhoneLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCustomerPhoneLabel.Location = new System.Drawing.Point(20, 104);
            this.lblCustomerPhoneLabel.Name = "lblCustomerPhoneLabel";
            this.lblCustomerPhoneLabel.Size = new System.Drawing.Size(104, 20);
            this.lblCustomerPhoneLabel.TabIndex = 5;
            this.lblCustomerPhoneLabel.Text = "Số điện thoại:";
            // 
            // lblCustomerPhone
            // 
            this.lblCustomerPhone.AutoSize = true;
            this.lblCustomerPhone.Location = new System.Drawing.Point(150, 104);
            this.lblCustomerPhone.Name = "lblCustomerPhone";
            this.lblCustomerPhone.Size = new System.Drawing.Size(0, 16);
            this.lblCustomerPhone.TabIndex = 6;
            // 
            // lblPackagingNameLabel
            // 
            this.lblPackagingNameLabel.AutoSize = true;
            this.lblPackagingNameLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPackagingNameLabel.Location = new System.Drawing.Point(20, 128);
            this.lblPackagingNameLabel.Name = "lblPackagingNameLabel";
            this.lblPackagingNameLabel.Size = new System.Drawing.Size(85, 20);
            this.lblPackagingNameLabel.TabIndex = 7;
            this.lblPackagingNameLabel.Text = "Tên bao bì:";
            // 
            // lblPackagingName
            // 
            this.lblPackagingName.AutoSize = true;
            this.lblPackagingName.Location = new System.Drawing.Point(150, 128);
            this.lblPackagingName.Name = "lblPackagingName";
            this.lblPackagingName.Size = new System.Drawing.Size(0, 16);
            this.lblPackagingName.TabIndex = 8;
            // 
            // lblBarcodeLabel
            // 
            this.lblBarcodeLabel.AutoSize = true;
            this.lblBarcodeLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblBarcodeLabel.Location = new System.Drawing.Point(20, 152);
            this.lblBarcodeLabel.Name = "lblBarcodeLabel";
            this.lblBarcodeLabel.Size = new System.Drawing.Size(70, 20);
            this.lblBarcodeLabel.TabIndex = 9;
            this.lblBarcodeLabel.Text = "Barcode:";
            // 
            // lblBarcode
            // 
            this.lblBarcode.AutoSize = true;
            this.lblBarcode.Location = new System.Drawing.Point(150, 152);
            this.lblBarcode.Name = "lblBarcode";
            this.lblBarcode.Size = new System.Drawing.Size(0, 16);
            this.lblBarcode.TabIndex = 10;
            // 
            // lblTransactionTypeLabel
            // 
            this.lblTransactionTypeLabel.AutoSize = true;
            this.lblTransactionTypeLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTransactionTypeLabel.Location = new System.Drawing.Point(20, 176);
            this.lblTransactionTypeLabel.Name = "lblTransactionTypeLabel";
            this.lblTransactionTypeLabel.Size = new System.Drawing.Size(109, 20);
            this.lblTransactionTypeLabel.TabIndex = 11;
            this.lblTransactionTypeLabel.Text = "Loại giao dịch:";
            // 
            // lblTransactionType
            // 
            this.lblTransactionType.AutoSize = true;
            this.lblTransactionType.Location = new System.Drawing.Point(150, 176);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(0, 16);
            this.lblTransactionType.TabIndex = 12;
            // 
            // lblOwnershipTypeLabel
            // 
            this.lblOwnershipTypeLabel.AutoSize = true;
            this.lblOwnershipTypeLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblOwnershipTypeLabel.Location = new System.Drawing.Point(20, 200);
            this.lblOwnershipTypeLabel.Name = "lblOwnershipTypeLabel";
            this.lblOwnershipTypeLabel.Size = new System.Drawing.Size(82, 20);
            this.lblOwnershipTypeLabel.TabIndex = 13;
            this.lblOwnershipTypeLabel.Text = "Hình thức:";
            // 
            // lblOwnershipType
            // 
            this.lblOwnershipType.AutoSize = true;
            this.lblOwnershipType.Location = new System.Drawing.Point(150, 200);
            this.lblOwnershipType.Name = "lblOwnershipType";
            this.lblOwnershipType.Size = new System.Drawing.Size(0, 16);
            this.lblOwnershipType.TabIndex = 14;
            // 
            // lblQuantityLabel
            // 
            this.lblQuantityLabel.AutoSize = true;
            this.lblQuantityLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblQuantityLabel.Location = new System.Drawing.Point(19, 229);
            this.lblQuantityLabel.Name = "lblQuantityLabel";
            this.lblQuantityLabel.Size = new System.Drawing.Size(75, 20);
            this.lblQuantityLabel.TabIndex = 15;
            this.lblQuantityLabel.Text = "Số lượng:";
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new System.Drawing.Point(117, 230);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(0, 16);
            this.lblQuantity.TabIndex = 16;
            // 
            // lblDepositPriceLabel
            // 
            this.lblDepositPriceLabel.AutoSize = true;
            this.lblDepositPriceLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDepositPriceLabel.Location = new System.Drawing.Point(19, 254);
            this.lblDepositPriceLabel.Name = "lblDepositPriceLabel";
            this.lblDepositPriceLabel.Size = new System.Drawing.Size(70, 20);
            this.lblDepositPriceLabel.TabIndex = 17;
            this.lblDepositPriceLabel.Text = "Tiền cọc:";
            // 
            // lblDepositPrice
            // 
            this.lblDepositPrice.AutoSize = true;
            this.lblDepositPrice.Location = new System.Drawing.Point(117, 254);
            this.lblDepositPrice.Name = "lblDepositPrice";
            this.lblDepositPrice.Size = new System.Drawing.Size(0, 16);
            this.lblDepositPrice.TabIndex = 18;
            // 
            // lblRefundAmountLabel
            // 
            this.lblRefundAmountLabel.AutoSize = true;
            this.lblRefundAmountLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblRefundAmountLabel.Location = new System.Drawing.Point(19, 278);
            this.lblRefundAmountLabel.Name = "lblRefundAmountLabel";
            this.lblRefundAmountLabel.Size = new System.Drawing.Size(82, 20);
            this.lblRefundAmountLabel.TabIndex = 19;
            this.lblRefundAmountLabel.Text = "Tiền hoàn:";
            // 
            // lblRefundAmount
            // 
            this.lblRefundAmount.AutoSize = true;
            this.lblRefundAmount.Location = new System.Drawing.Point(117, 278);
            this.lblRefundAmount.Name = "lblRefundAmount";
            this.lblRefundAmount.Size = new System.Drawing.Size(0, 16);
            this.lblRefundAmount.TabIndex = 20;
            // 
            // lblCreatedDateLabel
            // 
            this.lblCreatedDateLabel.AutoSize = true;
            this.lblCreatedDateLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCreatedDateLabel.Location = new System.Drawing.Point(19, 302);
            this.lblCreatedDateLabel.Name = "lblCreatedDateLabel";
            this.lblCreatedDateLabel.Size = new System.Drawing.Size(77, 20);
            this.lblCreatedDateLabel.TabIndex = 21;
            this.lblCreatedDateLabel.Text = "Ngày tạo:";
            // 
            // lblCreatedDate
            // 
            this.lblCreatedDate.AutoSize = true;
            this.lblCreatedDate.Location = new System.Drawing.Point(117, 302);
            this.lblCreatedDate.Name = "lblCreatedDate";
            this.lblCreatedDate.Size = new System.Drawing.Size(0, 16);
            this.lblCreatedDate.TabIndex = 22;
            // 
            // lblCreatedByLabel
            // 
            this.lblCreatedByLabel.AutoSize = true;
            this.lblCreatedByLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCreatedByLabel.Location = new System.Drawing.Point(19, 330);
            this.lblCreatedByLabel.Name = "lblCreatedByLabel";
            this.lblCreatedByLabel.Size = new System.Drawing.Size(84, 20);
            this.lblCreatedByLabel.TabIndex = 23;
            this.lblCreatedByLabel.Text = "Người tạo:";
            // 
            // lblCreatedBy
            // 
            this.lblCreatedBy.AutoSize = true;
            this.lblCreatedBy.Location = new System.Drawing.Point(117, 326);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new System.Drawing.Size(0, 16);
            this.lblCreatedBy.TabIndex = 24;
            // 
            // lblNotesLabel
            // 
            this.lblNotesLabel.AutoSize = true;
            this.lblNotesLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblNotesLabel.Location = new System.Drawing.Point(20, 367);
            this.lblNotesLabel.Name = "lblNotesLabel";
            this.lblNotesLabel.Size = new System.Drawing.Size(66, 20);
            this.lblNotesLabel.TabIndex = 25;
            this.lblNotesLabel.Text = "Ghi chú:";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(24, 399);
            this.txtNotes.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ReadOnly = true;
            this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNotes.Size = new System.Drawing.Size(560, 90);
            this.txtNotes.TabIndex = 26;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(457, 506);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(123, 40);
            this.btnClose.TabIndex = 27;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Location = new System.Drawing.Point(316, 506);
            this.btnExportExcel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(123, 40);
            this.btnExportExcel.TabIndex = 28;
            this.btnExportExcel.Text = "Xuất Excel";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // PackagingTransactionDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 578);
            this.Controls.Add(this.btnExportExcel);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.lblNotesLabel);
            this.Controls.Add(this.lblCreatedBy);
            this.Controls.Add(this.lblCreatedByLabel);
            this.Controls.Add(this.lblCreatedDate);
            this.Controls.Add(this.lblCreatedDateLabel);
            this.Controls.Add(this.lblRefundAmount);
            this.Controls.Add(this.lblRefundAmountLabel);
            this.Controls.Add(this.lblDepositPrice);
            this.Controls.Add(this.lblDepositPriceLabel);
            this.Controls.Add(this.lblQuantity);
            this.Controls.Add(this.lblQuantityLabel);
            this.Controls.Add(this.lblOwnershipType);
            this.Controls.Add(this.lblOwnershipTypeLabel);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.lblTransactionTypeLabel);
            this.Controls.Add(this.lblBarcode);
            this.Controls.Add(this.lblBarcodeLabel);
            this.Controls.Add(this.lblPackagingName);
            this.Controls.Add(this.lblPackagingNameLabel);
            this.Controls.Add(this.lblCustomerPhone);
            this.Controls.Add(this.lblCustomerPhoneLabel);
            this.Controls.Add(this.lblCustomerName);
            this.Controls.Add(this.lblCustomerNameLabel);
            this.Controls.Add(this.lblReferenceNumber);
            this.Controls.Add(this.lblReferenceNumberLabel);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PackagingTransactionDetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chi tiết giao dịch bao bì";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}