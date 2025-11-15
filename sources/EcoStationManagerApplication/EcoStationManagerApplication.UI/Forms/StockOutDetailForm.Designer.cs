using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    partial class StockOutDetailForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle;
        private Label lblReferenceNumberLabel;
        private Label lblReferenceNumber;
        private Label lblProductNameLabel;
        private Label lblProductName;
        private Label lblBatchNoLabel;
        private Label lblBatchNo;
        private Label lblQuantityLabel;
        private Label lblQuantity;
        private Label lblPurposeLabel;
        private Label lblPurpose;
        private Label lblOrderCodeLabel;
        private Label lblOrderCode;
        private Label lblCreatedDateLabel;
        private Label lblCreatedDate;
        private Label lblCreatedByLabel;
        private Label lblCreatedBy;
        private Label lblNotesLabel;
        private TextBox txtNotes;
        private Button btnClose;

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
            this.lblQuantityLabel = new Label();
            this.lblQuantity = new Label();
            this.lblPurposeLabel = new Label();
            this.lblPurpose = new Label();
            this.lblOrderCodeLabel = new Label();
            this.lblOrderCode = new Label();
            this.lblCreatedDateLabel = new Label();
            this.lblCreatedDate = new Label();
            this.lblCreatedByLabel = new Label();
            this.lblCreatedBy = new Label();
            this.lblNotesLabel = new Label();
            this.txtNotes = new TextBox();
            this.btnClose = new Button();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTitle.Location = new Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(240, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Chi tiết phiếu xuất kho";

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

            // lblQuantityLabel
            this.lblQuantityLabel.AutoSize = true;
            this.lblQuantityLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblQuantityLabel.Location = new Point(20, 160);
            this.lblQuantityLabel.Name = "lblQuantityLabel";
            this.lblQuantityLabel.Size = new Size(85, 20);
            this.lblQuantityLabel.TabIndex = 7;
            this.lblQuantityLabel.Text = "Số lượng:";

            // lblQuantity
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new Point(120, 160);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new Size(50, 20);
            this.lblQuantity.TabIndex = 8;

            // lblPurposeLabel
            this.lblPurposeLabel.AutoSize = true;
            this.lblPurposeLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblPurposeLabel.Location = new Point(20, 190);
            this.lblPurposeLabel.Name = "lblPurposeLabel";
            this.lblPurposeLabel.Size = new Size(85, 20);
            this.lblPurposeLabel.TabIndex = 9;
            this.lblPurposeLabel.Text = "Mục đích:";

            // lblPurpose
            this.lblPurpose.AutoSize = true;
            this.lblPurpose.Location = new Point(120, 190);
            this.lblPurpose.Name = "lblPurpose";
            this.lblPurpose.Size = new Size(50, 20);
            this.lblPurpose.TabIndex = 10;

            // lblOrderCodeLabel
            this.lblOrderCodeLabel.AutoSize = true;
            this.lblOrderCodeLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblOrderCodeLabel.Location = new Point(20, 220);
            this.lblOrderCodeLabel.Name = "lblOrderCodeLabel";
            this.lblOrderCodeLabel.Size = new Size(85, 20);
            this.lblOrderCodeLabel.TabIndex = 11;
            this.lblOrderCodeLabel.Text = "Đơn hàng:";

            // lblOrderCode
            this.lblOrderCode.AutoSize = true;
            this.lblOrderCode.Location = new Point(120, 220);
            this.lblOrderCode.Name = "lblOrderCode";
            this.lblOrderCode.Size = new Size(50, 20);
            this.lblOrderCode.TabIndex = 12;

            // lblCreatedDateLabel
            this.lblCreatedDateLabel.AutoSize = true;
            this.lblCreatedDateLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblCreatedDateLabel.Location = new Point(20, 250);
            this.lblCreatedDateLabel.Name = "lblCreatedDateLabel";
            this.lblCreatedDateLabel.Size = new Size(85, 20);
            this.lblCreatedDateLabel.TabIndex = 13;
            this.lblCreatedDateLabel.Text = "Ngày xuất:";

            // lblCreatedDate
            this.lblCreatedDate.AutoSize = true;
            this.lblCreatedDate.Location = new Point(120, 250);
            this.lblCreatedDate.Name = "lblCreatedDate";
            this.lblCreatedDate.Size = new Size(50, 20);
            this.lblCreatedDate.TabIndex = 14;

            // lblCreatedByLabel
            this.lblCreatedByLabel.AutoSize = true;
            this.lblCreatedByLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblCreatedByLabel.Location = new Point(20, 280);
            this.lblCreatedByLabel.Name = "lblCreatedByLabel";
            this.lblCreatedByLabel.Size = new Size(95, 20);
            this.lblCreatedByLabel.TabIndex = 15;
            this.lblCreatedByLabel.Text = "Người tạo:";

            // lblCreatedBy
            this.lblCreatedBy.AutoSize = true;
            this.lblCreatedBy.Location = new Point(120, 280);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new Size(50, 20);
            this.lblCreatedBy.TabIndex = 16;

            // lblNotesLabel
            this.lblNotesLabel.AutoSize = true;
            this.lblNotesLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblNotesLabel.Location = new Point(20, 310);
            this.lblNotesLabel.Name = "lblNotesLabel";
            this.lblNotesLabel.Size = new Size(70, 20);
            this.lblNotesLabel.TabIndex = 17;
            this.lblNotesLabel.Text = "Ghi chú:";

            // txtNotes
            this.txtNotes.Location = new Point(20, 335);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ReadOnly = true;
            this.txtNotes.ScrollBars = ScrollBars.Vertical;
            this.txtNotes.Size = new Size(550, 60);
            this.txtNotes.TabIndex = 18;

            // btnClose
            this.btnClose.Location = new Point(495, 410);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(75, 30);
            this.btnClose.TabIndex = 19;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            // StockOutDetailForm
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(600, 500);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.lblNotesLabel);
            this.Controls.Add(this.lblCreatedBy);
            this.Controls.Add(this.lblCreatedByLabel);
            this.Controls.Add(this.lblCreatedDate);
            this.Controls.Add(this.lblCreatedDateLabel);
            this.Controls.Add(this.lblOrderCode);
            this.Controls.Add(this.lblOrderCodeLabel);
            this.Controls.Add(this.lblPurpose);
            this.Controls.Add(this.lblPurposeLabel);
            this.Controls.Add(this.lblQuantity);
            this.Controls.Add(this.lblQuantityLabel);
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
            this.Name = "StockOutDetailForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Chi tiết phiếu xuất kho";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

