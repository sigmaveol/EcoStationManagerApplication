using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    partial class OrderDetailForm
    {
        private System.ComponentModel.IContainer components = null;
        
        // Title and close button
        private Label lblTitle;
        private Button btnCloseX;
        
        // General order information
        private Panel pnlGeneralInfo;
        private Label lblOrderCodeLabel;
        private Label lblOrderCode;
        private Label lblStatusLabel;
        private Label lblStatus;
        private Label lblSourceLabel;
        private Label lblSource;
        private Label lblCreatedDateLabel;
        private Label lblCreatedDate;
        
        // Customer information
        private Panel pnlCustomerInfo;
        private Label lblCustomerNameLabel;
        private Label lblCustomerName;
        private Label lblPhoneLabel;
        private Label lblPhone;
        private Label lblAddressLabel;
        private Label lblAddress;
        private Label lblPaymentMethodLabel;
        private Label lblPaymentMethod;
        
        // Product details
        private Label lblProductDetailsTitle;
        private DataGridView dgvProducts;
        
        // Summary section
        private Panel pnlSummary;
        private Label lblSubtotalLabel;
        private Label lblSubtotal;
        private Label lblDiscountLabel;
        private Label lblDiscount;
        private Label lblTotalLabel;
        private Label lblTotal;
        
        // Notes section
        private Label lblNotesLabel;
        private TextBox txtNotes;
        
        // Close button
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
            this.btnCloseX = new Button();
            this.pnlGeneralInfo = new Panel();
            this.lblOrderCodeLabel = new Label();
            this.lblOrderCode = new Label();
            this.lblStatusLabel = new Label();
            this.lblStatus = new Label();
            this.lblSourceLabel = new Label();
            this.lblSource = new Label();
            this.lblCreatedDateLabel = new Label();
            this.lblCreatedDate = new Label();
            this.pnlCustomerInfo = new Panel();
            this.lblCustomerNameLabel = new Label();
            this.lblCustomerName = new Label();
            this.lblPhoneLabel = new Label();
            this.lblPhone = new Label();
            this.lblAddressLabel = new Label();
            this.lblAddress = new Label();
            this.lblPaymentMethodLabel = new Label();
            this.lblPaymentMethod = new Label();
            this.lblProductDetailsTitle = new Label();
            this.dgvProducts = new DataGridView();
            this.pnlSummary = new Panel();
            this.lblSubtotalLabel = new Label();
            this.lblSubtotal = new Label();
            this.lblDiscountLabel = new Label();
            this.lblDiscount = new Label();
            this.lblTotalLabel = new Label();
            this.lblTotal = new Label();
            this.lblNotesLabel = new Label();
            this.txtNotes = new TextBox();
            this.btnClose = new Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.pnlGeneralInfo.SuspendLayout();
            this.pnlCustomerInfo.SuspendLayout();
            this.pnlSummary.SuspendLayout();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblTitle.Location = new Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(200, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Chi tiết đơn hàng";

            // btnCloseX
            this.btnCloseX.BackColor = Color.Transparent;
            this.btnCloseX.FlatAppearance.BorderSize = 0;
            this.btnCloseX.FlatStyle = FlatStyle.Flat;
            this.btnCloseX.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.btnCloseX.ForeColor = Color.Gray;
            this.btnCloseX.Location = new Point(750, 15);
            this.btnCloseX.Name = "btnCloseX";
            this.btnCloseX.Size = new Size(35, 35);
            this.btnCloseX.TabIndex = 1;
            this.btnCloseX.Text = "×";
            this.btnCloseX.UseVisualStyleBackColor = false;
            this.btnCloseX.Click += new System.EventHandler(this.btnCloseX_Click);

            // pnlGeneralInfo
            this.pnlGeneralInfo.BorderStyle = BorderStyle.FixedSingle;
            this.pnlGeneralInfo.Controls.Add(this.lblCreatedDate);
            this.pnlGeneralInfo.Controls.Add(this.lblCreatedDateLabel);
            this.pnlGeneralInfo.Controls.Add(this.lblSource);
            this.pnlGeneralInfo.Controls.Add(this.lblSourceLabel);
            this.pnlGeneralInfo.Controls.Add(this.lblStatus);
            this.pnlGeneralInfo.Controls.Add(this.lblStatusLabel);
            this.pnlGeneralInfo.Controls.Add(this.lblOrderCode);
            this.pnlGeneralInfo.Controls.Add(this.lblOrderCodeLabel);
            this.pnlGeneralInfo.Location = new Point(20, 70);
            this.pnlGeneralInfo.Name = "pnlGeneralInfo";
            this.pnlGeneralInfo.Size = new Size(760, 100);
            this.pnlGeneralInfo.TabIndex = 2;

            // lblOrderCodeLabel
            this.lblOrderCodeLabel.AutoSize = true;
            this.lblOrderCodeLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblOrderCodeLabel.Location = new Point(15, 15);
            this.lblOrderCodeLabel.Name = "lblOrderCodeLabel";
            this.lblOrderCodeLabel.Size = new Size(100, 20);
            this.lblOrderCodeLabel.TabIndex = 0;
            this.lblOrderCodeLabel.Text = "Mã đơn hàng:";

            // lblOrderCode
            this.lblOrderCode.AutoSize = true;
            this.lblOrderCode.Location = new Point(125, 15);
            this.lblOrderCode.Name = "lblOrderCode";
            this.lblOrderCode.Size = new Size(50, 20);
            this.lblOrderCode.TabIndex = 1;

            // lblStatusLabel
            this.lblStatusLabel.AutoSize = true;
            this.lblStatusLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblStatusLabel.Location = new Point(15, 45);
            this.lblStatusLabel.Name = "lblStatusLabel";
            this.lblStatusLabel.Size = new Size(85, 20);
            this.lblStatusLabel.TabIndex = 2;
            this.lblStatusLabel.Text = "Trạng thái:";

            // lblStatus
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new Point(125, 45);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(50, 20);
            this.lblStatus.TabIndex = 3;

            // lblSourceLabel
            this.lblSourceLabel.AutoSize = true;
            this.lblSourceLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblSourceLabel.Location = new Point(400, 15);
            this.lblSourceLabel.Name = "lblSourceLabel";
            this.lblSourceLabel.Size = new Size(85, 20);
            this.lblSourceLabel.TabIndex = 4;
            this.lblSourceLabel.Text = "Nguồn đơn:";

            // lblSource
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new Point(500, 15);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new Size(50, 20);
            this.lblSource.TabIndex = 5;

            // lblCreatedDateLabel
            this.lblCreatedDateLabel.AutoSize = true;
            this.lblCreatedDateLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblCreatedDateLabel.Location = new Point(400, 45);
            this.lblCreatedDateLabel.Name = "lblCreatedDateLabel";
            this.lblCreatedDateLabel.Size = new Size(75, 20);
            this.lblCreatedDateLabel.TabIndex = 6;
            this.lblCreatedDateLabel.Text = "Ngày tạo:";

            // lblCreatedDate
            this.lblCreatedDate.AutoSize = true;
            this.lblCreatedDate.Location = new Point(500, 45);
            this.lblCreatedDate.Name = "lblCreatedDate";
            this.lblCreatedDate.Size = new Size(50, 20);
            this.lblCreatedDate.TabIndex = 7;

            // pnlCustomerInfo
            this.pnlCustomerInfo.BorderStyle = BorderStyle.FixedSingle;
            this.pnlCustomerInfo.Controls.Add(this.lblPaymentMethod);
            this.pnlCustomerInfo.Controls.Add(this.lblPaymentMethodLabel);
            this.pnlCustomerInfo.Controls.Add(this.lblAddress);
            this.pnlCustomerInfo.Controls.Add(this.lblAddressLabel);
            this.pnlCustomerInfo.Controls.Add(this.lblPhone);
            this.pnlCustomerInfo.Controls.Add(this.lblPhoneLabel);
            this.pnlCustomerInfo.Controls.Add(this.lblCustomerName);
            this.pnlCustomerInfo.Controls.Add(this.lblCustomerNameLabel);
            this.pnlCustomerInfo.Location = new Point(20, 180);
            this.pnlCustomerInfo.Name = "pnlCustomerInfo";
            this.pnlCustomerInfo.Size = new Size(760, 120);
            this.pnlCustomerInfo.TabIndex = 3;

            // lblCustomerNameLabel
            this.lblCustomerNameLabel.AutoSize = true;
            this.lblCustomerNameLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblCustomerNameLabel.Location = new Point(15, 15);
            this.lblCustomerNameLabel.Name = "lblCustomerNameLabel";
            this.lblCustomerNameLabel.Size = new Size(130, 20);
            this.lblCustomerNameLabel.TabIndex = 0;
            this.lblCustomerNameLabel.Text = "Tên khách hàng:";

            // lblCustomerName
            this.lblCustomerName.AutoSize = true;
            this.lblCustomerName.Location = new Point(155, 15);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new Size(50, 20);
            this.lblCustomerName.TabIndex = 1;

            // lblPhoneLabel
            this.lblPhoneLabel.AutoSize = true;
            this.lblPhoneLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblPhoneLabel.Location = new Point(15, 45);
            this.lblPhoneLabel.Name = "lblPhoneLabel";
            this.lblPhoneLabel.Size = new Size(120, 20);
            this.lblPhoneLabel.TabIndex = 2;
            this.lblPhoneLabel.Text = "Số điện thoại:";

            // lblPhone
            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new Point(155, 45);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new Size(50, 20);
            this.lblPhone.TabIndex = 3;

            // lblAddressLabel
            this.lblAddressLabel.AutoSize = true;
            this.lblAddressLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblAddressLabel.Location = new Point(15, 75);
            this.lblAddressLabel.Name = "lblAddressLabel";
            this.lblAddressLabel.Size = new Size(130, 20);
            this.lblAddressLabel.TabIndex = 4;
            this.lblAddressLabel.Text = "Địa chỉ giao hàng:";

            // lblAddress
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new Point(155, 75);
            this.lblAddress.MaximumSize = new Size(580, 0);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new Size(50, 20);
            this.lblAddress.TabIndex = 5;

            // lblPaymentMethodLabel
            this.lblPaymentMethodLabel.AutoSize = true;
            this.lblPaymentMethodLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblPaymentMethodLabel.Location = new Point(400, 15);
            this.lblPaymentMethodLabel.Name = "lblPaymentMethodLabel";
            this.lblPaymentMethodLabel.Size = new Size(165, 20);
            this.lblPaymentMethodLabel.TabIndex = 6;
            this.lblPaymentMethodLabel.Text = "Phương thức thanh toán:";

            // lblPaymentMethod
            this.lblPaymentMethod.AutoSize = true;
            this.lblPaymentMethod.Location = new Point(575, 15);
            this.lblPaymentMethod.Name = "lblPaymentMethod";
            this.lblPaymentMethod.Size = new Size(50, 20);
            this.lblPaymentMethod.TabIndex = 7;

            // lblProductDetailsTitle
            this.lblProductDetailsTitle.AutoSize = true;
            this.lblProductDetailsTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblProductDetailsTitle.Location = new Point(20, 320);
            this.lblProductDetailsTitle.Name = "lblProductDetailsTitle";
            this.lblProductDetailsTitle.Size = new Size(150, 23);
            this.lblProductDetailsTitle.TabIndex = 4;
            this.lblProductDetailsTitle.Text = "Chi tiết sản phẩm";

            // dgvProducts
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AllowUserToDeleteRows = false;
            this.dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProducts.Location = new Point(20, 350);
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.RowHeadersWidth = 51;
            this.dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.Size = new Size(760, 200);
            this.dgvProducts.TabIndex = 5;

            // pnlSummary
            this.pnlSummary.BorderStyle = BorderStyle.FixedSingle;
            this.pnlSummary.Controls.Add(this.lblTotal);
            this.pnlSummary.Controls.Add(this.lblTotalLabel);
            this.pnlSummary.Controls.Add(this.lblDiscount);
            this.pnlSummary.Controls.Add(this.lblDiscountLabel);
            this.pnlSummary.Controls.Add(this.lblSubtotal);
            this.pnlSummary.Controls.Add(this.lblSubtotalLabel);
            this.pnlSummary.Location = new Point(500, 560);
            this.pnlSummary.Name = "pnlSummary";
            this.pnlSummary.Size = new Size(280, 120);
            this.pnlSummary.TabIndex = 6;

            // lblSubtotalLabel
            this.lblSubtotalLabel.AutoSize = true;
            this.lblSubtotalLabel.Location = new Point(15, 15);
            this.lblSubtotalLabel.Name = "lblSubtotalLabel";
            this.lblSubtotalLabel.Size = new Size(110, 20);
            this.lblSubtotalLabel.TabIndex = 0;
            this.lblSubtotalLabel.Text = "Tổng tiền hàng:";

            // lblSubtotal
            this.lblSubtotal.AutoSize = true;
            this.lblSubtotal.Location = new Point(180, 15);
            this.lblSubtotal.Name = "lblSubtotal";
            this.lblSubtotal.Size = new Size(50, 20);
            this.lblSubtotal.TabIndex = 1;

            // lblDiscountLabel
            this.lblDiscountLabel.AutoSize = true;
            this.lblDiscountLabel.Location = new Point(15, 50);
            this.lblDiscountLabel.Name = "lblDiscountLabel";
            this.lblDiscountLabel.Size = new Size(80, 20);
            this.lblDiscountLabel.TabIndex = 2;
            this.lblDiscountLabel.Text = "Giảm giá:";

            // lblDiscount
            this.lblDiscount.AutoSize = true;
            this.lblDiscount.ForeColor = Color.Red;
            this.lblDiscount.Location = new Point(180, 50);
            this.lblDiscount.Name = "lblDiscount";
            this.lblDiscount.Size = new Size(50, 20);
            this.lblDiscount.TabIndex = 3;

            // lblTotalLabel
            this.lblTotalLabel.AutoSize = true;
            this.lblTotalLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblTotalLabel.Location = new Point(15, 85);
            this.lblTotalLabel.Name = "lblTotalLabel";
            this.lblTotalLabel.Size = new Size(95, 20);
            this.lblTotalLabel.TabIndex = 4;
            this.lblTotalLabel.Text = "Thành tiền:";

            // lblTotal
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblTotal.Location = new Point(180, 85);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new Size(50, 23);
            this.lblTotal.TabIndex = 5;

            // lblNotesLabel
            this.lblNotesLabel.AutoSize = true;
            this.lblNotesLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblNotesLabel.Location = new Point(20, 560);
            this.lblNotesLabel.Name = "lblNotesLabel";
            this.lblNotesLabel.Size = new Size(70, 20);
            this.lblNotesLabel.TabIndex = 7;
            this.lblNotesLabel.Text = "Ghi chú:";

            // txtNotes
            this.txtNotes.Location = new Point(20, 585);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ReadOnly = true;
            this.txtNotes.ScrollBars = ScrollBars.Vertical;
            this.txtNotes.Size = new Size(460, 60);
            this.txtNotes.TabIndex = 8;

            // btnClose
            this.btnClose.Location = new Point(705, 640);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(75, 35);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            // OrderDetailForm
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 700);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.lblNotesLabel);
            this.Controls.Add(this.pnlSummary);
            this.Controls.Add(this.dgvProducts);
            this.Controls.Add(this.lblProductDetailsTitle);
            this.Controls.Add(this.pnlCustomerInfo);
            this.Controls.Add(this.pnlGeneralInfo);
            this.Controls.Add(this.btnCloseX);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrderDetailForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Chi tiết đơn hàng";
            this.pnlGeneralInfo.ResumeLayout(false);
            this.pnlGeneralInfo.PerformLayout();
            this.pnlCustomerInfo.ResumeLayout(false);
            this.pnlCustomerInfo.PerformLayout();
            this.pnlSummary.ResumeLayout(false);
            this.pnlSummary.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
