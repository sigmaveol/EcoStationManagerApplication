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
            this.lblProductNameLabel = new System.Windows.Forms.Label();
            this.lblProductName = new System.Windows.Forms.Label();
            this.lblBatchNoLabel = new System.Windows.Forms.Label();
            this.lblBatchNo = new System.Windows.Forms.Label();
            this.lblQuantityLabel = new System.Windows.Forms.Label();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.lblPurposeLabel = new System.Windows.Forms.Label();
            this.lblPurpose = new System.Windows.Forms.Label();
            this.lblOrderCodeLabel = new System.Windows.Forms.Label();
            this.lblOrderCode = new System.Windows.Forms.Label();
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
            this.lblTitle.Size = new System.Drawing.Size(272, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Chi tiết phiếu xuất kho";
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
            // lblQuantityLabel
            // 
            this.lblQuantityLabel.AutoSize = true;
            this.lblQuantityLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblQuantityLabel.Location = new System.Drawing.Point(20, 128);
            this.lblQuantityLabel.Name = "lblQuantityLabel";
            this.lblQuantityLabel.Size = new System.Drawing.Size(75, 20);
            this.lblQuantityLabel.TabIndex = 7;
            this.lblQuantityLabel.Text = "Số lượng:";
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new System.Drawing.Point(120, 128);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(0, 16);
            this.lblQuantity.TabIndex = 8;
            // 
            // lblPurposeLabel
            // 
            this.lblPurposeLabel.AutoSize = true;
            this.lblPurposeLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPurposeLabel.Location = new System.Drawing.Point(20, 152);
            this.lblPurposeLabel.Name = "lblPurposeLabel";
            this.lblPurposeLabel.Size = new System.Drawing.Size(76, 20);
            this.lblPurposeLabel.TabIndex = 9;
            this.lblPurposeLabel.Text = "Mục đích:";
            // 
            // lblPurpose
            // 
            this.lblPurpose.AutoSize = true;
            this.lblPurpose.Location = new System.Drawing.Point(120, 152);
            this.lblPurpose.Name = "lblPurpose";
            this.lblPurpose.Size = new System.Drawing.Size(0, 16);
            this.lblPurpose.TabIndex = 10;
            // 
            // lblOrderCodeLabel
            // 
            this.lblOrderCodeLabel.AutoSize = true;
            this.lblOrderCodeLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblOrderCodeLabel.Location = new System.Drawing.Point(20, 176);
            this.lblOrderCodeLabel.Name = "lblOrderCodeLabel";
            this.lblOrderCodeLabel.Size = new System.Drawing.Size(81, 20);
            this.lblOrderCodeLabel.TabIndex = 11;
            this.lblOrderCodeLabel.Text = "Đơn hàng:";
            // 
            // lblOrderCode
            // 
            this.lblOrderCode.AutoSize = true;
            this.lblOrderCode.Location = new System.Drawing.Point(120, 176);
            this.lblOrderCode.Name = "lblOrderCode";
            this.lblOrderCode.Size = new System.Drawing.Size(0, 16);
            this.lblOrderCode.TabIndex = 12;
            // 
            // lblCreatedDateLabel
            // 
            this.lblCreatedDateLabel.AutoSize = true;
            this.lblCreatedDateLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCreatedDateLabel.Location = new System.Drawing.Point(20, 200);
            this.lblCreatedDateLabel.Name = "lblCreatedDateLabel";
            this.lblCreatedDateLabel.Size = new System.Drawing.Size(85, 20);
            this.lblCreatedDateLabel.TabIndex = 13;
            this.lblCreatedDateLabel.Text = "Ngày xuất:";
            // 
            // lblCreatedDate
            // 
            this.lblCreatedDate.AutoSize = true;
            this.lblCreatedDate.Location = new System.Drawing.Point(120, 200);
            this.lblCreatedDate.Name = "lblCreatedDate";
            this.lblCreatedDate.Size = new System.Drawing.Size(0, 16);
            this.lblCreatedDate.TabIndex = 14;
            // 
            // lblCreatedByLabel
            // 
            this.lblCreatedByLabel.AutoSize = true;
            this.lblCreatedByLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCreatedByLabel.Location = new System.Drawing.Point(20, 224);
            this.lblCreatedByLabel.Name = "lblCreatedByLabel";
            this.lblCreatedByLabel.Size = new System.Drawing.Size(84, 20);
            this.lblCreatedByLabel.TabIndex = 15;
            this.lblCreatedByLabel.Text = "Người tạo:";
            // 
            // lblCreatedBy
            // 
            this.lblCreatedBy.AutoSize = true;
            this.lblCreatedBy.Location = new System.Drawing.Point(120, 224);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new System.Drawing.Size(0, 16);
            this.lblCreatedBy.TabIndex = 16;
            // 
            // lblNotesLabel
            // 
            this.lblNotesLabel.AutoSize = true;
            this.lblNotesLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblNotesLabel.Location = new System.Drawing.Point(20, 248);
            this.lblNotesLabel.Name = "lblNotesLabel";
            this.lblNotesLabel.Size = new System.Drawing.Size(66, 20);
            this.lblNotesLabel.TabIndex = 17;
            this.lblNotesLabel.Text = "Ghi chú:";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(20, 270);
            this.txtNotes.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ReadOnly = true;
            this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNotes.Size = new System.Drawing.Size(550, 49);
            this.txtNotes.TabIndex = 18;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(486, 344);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(84, 45);
            this.btnClose.TabIndex = 19;
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
            this.btnExportExcel.Location = new System.Drawing.Point(345, 344);
            this.btnExportExcel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(108, 45);
            this.btnExportExcel.TabIndex = 20;
            this.btnExportExcel.Text = "Xuất Excel";
            this.btnExportExcel.UseVisualStyleBackColor = false;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // StockOutDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.Controls.Add(this.btnExportExcel);
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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StockOutDetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chi tiết phiếu xuất kho";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

