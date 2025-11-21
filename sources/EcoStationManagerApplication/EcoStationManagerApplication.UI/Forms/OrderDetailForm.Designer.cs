namespace EcoStationManagerApplication.UI.Forms
{
    partial class OrderDetailForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panelOrderInfo;
        private System.Windows.Forms.Panel panelCustomerInfo;
        private System.Windows.Forms.Panel panelProductDetails;
        private System.Windows.Forms.Panel panelSummary;
        private System.Windows.Forms.Label lblOrderCode;
        private System.Windows.Forms.Label lblOrderCodeValue;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblStatusValue;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.Label lblSourceValue;
        private System.Windows.Forms.Label lblCreatedDate;
        private System.Windows.Forms.Label lblCreatedDateValue;
        private System.Windows.Forms.Label lblCustomerName;
        private System.Windows.Forms.Label lblCustomerNameValue;
        private System.Windows.Forms.Label lblCustomerPhone;
        private System.Windows.Forms.Label lblCustomerPhoneValue;
        private System.Windows.Forms.Label lblDeliveryAddress;
        private System.Windows.Forms.Label lblDeliveryAddressValue;
        private System.Windows.Forms.Label lblPaymentMethod;
        private System.Windows.Forms.Label lblPaymentMethodValue;
        private System.Windows.Forms.Label lblProductDetailsTitle;
        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.Label lblTotalItems;
        private System.Windows.Forms.Label lblTotalItemsValue;
        private System.Windows.Forms.Label lblDiscount;
        private System.Windows.Forms.Label lblDiscountValue;
        private System.Windows.Forms.Label lblFinalTotal;
        private System.Windows.Forms.Label lblFinalTotalValue;
        private System.Windows.Forms.Label lblNoteTitle;
        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.Button btnCloseForm;
        private System.Windows.Forms.Panel panelSeparator1;
        private System.Windows.Forms.Panel panelSeparator2;
        private System.Windows.Forms.Panel panelSeparator3;

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
            this.btnClose = new System.Windows.Forms.Button();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.panelOrderInfo = new System.Windows.Forms.Panel();
            this.lblCreatedDateValue = new System.Windows.Forms.Label();
            this.lblCreatedDate = new System.Windows.Forms.Label();
            this.lblSourceValue = new System.Windows.Forms.Label();
            this.lblSource = new System.Windows.Forms.Label();
            this.lblStatusValue = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblOrderCodeValue = new System.Windows.Forms.Label();
            this.lblOrderCode = new System.Windows.Forms.Label();
            this.panelCustomerInfo = new System.Windows.Forms.Panel();
            this.lblPaymentMethodValue = new System.Windows.Forms.Label();
            this.lblPaymentMethod = new System.Windows.Forms.Label();
            this.lblDeliveryAddressValue = new System.Windows.Forms.Label();
            this.lblDeliveryAddress = new System.Windows.Forms.Label();
            this.lblCustomerPhoneValue = new System.Windows.Forms.Label();
            this.lblCustomerPhone = new System.Windows.Forms.Label();
            this.lblCustomerNameValue = new System.Windows.Forms.Label();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.panelProductDetails = new System.Windows.Forms.Panel();
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            this.lblProductDetailsTitle = new System.Windows.Forms.Label();
            this.panelSummary = new System.Windows.Forms.Panel();
            this.lblFinalTotalValue = new System.Windows.Forms.Label();
            this.lblFinalTotal = new System.Windows.Forms.Label();
            this.lblDiscountValue = new System.Windows.Forms.Label();
            this.lblDiscount = new System.Windows.Forms.Label();
            this.lblTotalItemsValue = new System.Windows.Forms.Label();
            this.lblTotalItems = new System.Windows.Forms.Label();
            this.lblNoteTitle = new System.Windows.Forms.Label();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.btnCloseForm = new System.Windows.Forms.Button();
            this.panelSeparator1 = new System.Windows.Forms.Panel();
            this.panelSeparator2 = new System.Windows.Forms.Panel();
            this.panelSeparator3 = new System.Windows.Forms.Panel();
            this.panelHeader.SuspendLayout();
            this.panelOrderInfo.SuspendLayout();
            this.panelCustomerInfo.SuspendLayout();
            this.panelProductDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.panelSummary.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(220, 35);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Chi tiết đơn hàng";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.Gray;
            this.btnClose.Location = new System.Drawing.Point(850, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(40, 40);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "×";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.White;
            this.panelHeader.Controls.Add(this.btnClose);
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(900, 60);
            this.panelHeader.TabIndex = 0;
            // 
            // panelOrderInfo
            // 
            this.panelOrderInfo.BackColor = System.Drawing.Color.White;
            this.panelOrderInfo.Controls.Add(this.lblCreatedDateValue);
            this.panelOrderInfo.Controls.Add(this.lblCreatedDate);
            this.panelOrderInfo.Controls.Add(this.lblSourceValue);
            this.panelOrderInfo.Controls.Add(this.lblSource);
            this.panelOrderInfo.Controls.Add(this.lblStatusValue);
            this.panelOrderInfo.Controls.Add(this.lblStatus);
            this.panelOrderInfo.Controls.Add(this.lblOrderCodeValue);
            this.panelOrderInfo.Controls.Add(this.lblOrderCode);
            this.panelOrderInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelOrderInfo.Location = new System.Drawing.Point(0, 60);
            this.panelOrderInfo.Name = "panelOrderInfo";
            this.panelOrderInfo.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.panelOrderInfo.Size = new System.Drawing.Size(900, 120);
            this.panelOrderInfo.TabIndex = 1;
            this.panelOrderInfo.Paint += new System.Windows.Forms.PaintEventHandler(this.panelOrderInfo_Paint);
            // 
            // lblCreatedDateValue
            // 
            this.lblCreatedDateValue.AutoSize = true;
            this.lblCreatedDateValue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCreatedDateValue.Location = new System.Drawing.Point(535, 45);
            this.lblCreatedDateValue.Name = "lblCreatedDateValue";
            this.lblCreatedDateValue.Size = new System.Drawing.Size(27, 19);
            this.lblCreatedDateValue.TabIndex = 7;
            this.lblCreatedDateValue.Text = "---";
            // 
            // lblCreatedDate
            // 
            this.lblCreatedDate.AutoSize = true;
            this.lblCreatedDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCreatedDate.Location = new System.Drawing.Point(450, 45);
            this.lblCreatedDate.Name = "lblCreatedDate";
            this.lblCreatedDate.Size = new System.Drawing.Size(75, 19);
            this.lblCreatedDate.TabIndex = 6;
            this.lblCreatedDate.Text = "Ngày tạo:";
            // 
            // lblSourceValue
            // 
            this.lblSourceValue.AutoSize = true;
            this.lblSourceValue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSourceValue.Location = new System.Drawing.Point(545, 15);
            this.lblSourceValue.Name = "lblSourceValue";
            this.lblSourceValue.Size = new System.Drawing.Size(27, 19);
            this.lblSourceValue.TabIndex = 5;
            this.lblSourceValue.Text = "---";
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSource.Location = new System.Drawing.Point(450, 15);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(88, 19);
            this.lblSource.TabIndex = 4;
            this.lblSource.Text = "Nguồn đơn:";
            // 
            // lblStatusValue
            // 
            this.lblStatusValue.AutoSize = true;
            this.lblStatusValue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatusValue.Location = new System.Drawing.Point(110, 45);
            this.lblStatusValue.Name = "lblStatusValue";
            this.lblStatusValue.Size = new System.Drawing.Size(27, 19);
            this.lblStatusValue.TabIndex = 3;
            this.lblStatusValue.Text = "---";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(20, 45);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(80, 19);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Trạng thái:";
            // 
            // lblOrderCodeValue
            // 
            this.lblOrderCodeValue.AutoSize = true;
            this.lblOrderCodeValue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblOrderCodeValue.Location = new System.Drawing.Point(130, 15);
            this.lblOrderCodeValue.Name = "lblOrderCodeValue";
            this.lblOrderCodeValue.Size = new System.Drawing.Size(27, 19);
            this.lblOrderCodeValue.TabIndex = 1;
            this.lblOrderCodeValue.Text = "---";
            // 
            // lblOrderCode
            // 
            this.lblOrderCode.AutoSize = true;
            this.lblOrderCode.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblOrderCode.Location = new System.Drawing.Point(20, 15);
            this.lblOrderCode.Name = "lblOrderCode";
            this.lblOrderCode.Size = new System.Drawing.Size(101, 19);
            this.lblOrderCode.TabIndex = 0;
            this.lblOrderCode.Text = "Mã đơn hàng:";
            // 
            // panelCustomerInfo
            // 
            this.panelCustomerInfo.BackColor = System.Drawing.Color.White;
            this.panelCustomerInfo.Controls.Add(this.lblPaymentMethodValue);
            this.panelCustomerInfo.Controls.Add(this.lblPaymentMethod);
            this.panelCustomerInfo.Controls.Add(this.lblDeliveryAddressValue);
            this.panelCustomerInfo.Controls.Add(this.lblDeliveryAddress);
            this.panelCustomerInfo.Controls.Add(this.lblCustomerPhoneValue);
            this.panelCustomerInfo.Controls.Add(this.lblCustomerPhone);
            this.panelCustomerInfo.Controls.Add(this.lblCustomerNameValue);
            this.panelCustomerInfo.Controls.Add(this.lblCustomerName);
            this.panelCustomerInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCustomerInfo.Location = new System.Drawing.Point(0, 181);
            this.panelCustomerInfo.Name = "panelCustomerInfo";
            this.panelCustomerInfo.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.panelCustomerInfo.Size = new System.Drawing.Size(900, 140);
            this.panelCustomerInfo.TabIndex = 3;
            // 
            // lblPaymentMethodValue
            // 
            this.lblPaymentMethodValue.AutoSize = true;
            this.lblPaymentMethodValue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPaymentMethodValue.Location = new System.Drawing.Point(655, 15);
            this.lblPaymentMethodValue.Name = "lblPaymentMethodValue";
            this.lblPaymentMethodValue.Size = new System.Drawing.Size(27, 19);
            this.lblPaymentMethodValue.TabIndex = 7;
            this.lblPaymentMethodValue.Text = "---";
            // 
            // lblPaymentMethod
            // 
            this.lblPaymentMethod.AutoSize = true;
            this.lblPaymentMethod.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPaymentMethod.Location = new System.Drawing.Point(450, 15);
            this.lblPaymentMethod.Name = "lblPaymentMethod";
            this.lblPaymentMethod.Size = new System.Drawing.Size(173, 19);
            this.lblPaymentMethod.TabIndex = 6;
            this.lblPaymentMethod.Text = "Phương thức thanh toán:";
            // 
            // lblDeliveryAddressValue
            // 
            this.lblDeliveryAddressValue.AutoSize = true;
            this.lblDeliveryAddressValue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDeliveryAddressValue.Location = new System.Drawing.Point(165, 75);
            this.lblDeliveryAddressValue.Name = "lblDeliveryAddressValue";
            this.lblDeliveryAddressValue.Size = new System.Drawing.Size(27, 19);
            this.lblDeliveryAddressValue.TabIndex = 5;
            this.lblDeliveryAddressValue.Text = "---";
            // 
            // lblDeliveryAddress
            // 
            this.lblDeliveryAddress.AutoSize = true;
            this.lblDeliveryAddress.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDeliveryAddress.Location = new System.Drawing.Point(20, 75);
            this.lblDeliveryAddress.Name = "lblDeliveryAddress";
            this.lblDeliveryAddress.Size = new System.Drawing.Size(129, 19);
            this.lblDeliveryAddress.TabIndex = 4;
            this.lblDeliveryAddress.Text = "Địa chỉ giao hàng:";
            // 
            // lblCustomerPhoneValue
            // 
            this.lblCustomerPhoneValue.AutoSize = true;
            this.lblCustomerPhoneValue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCustomerPhoneValue.Location = new System.Drawing.Point(145, 45);
            this.lblCustomerPhoneValue.Name = "lblCustomerPhoneValue";
            this.lblCustomerPhoneValue.Size = new System.Drawing.Size(27, 19);
            this.lblCustomerPhoneValue.TabIndex = 3;
            this.lblCustomerPhoneValue.Text = "---";
            // 
            // lblCustomerPhone
            // 
            this.lblCustomerPhone.AutoSize = true;
            this.lblCustomerPhone.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCustomerPhone.Location = new System.Drawing.Point(20, 45);
            this.lblCustomerPhone.Name = "lblCustomerPhone";
            this.lblCustomerPhone.Size = new System.Drawing.Size(101, 19);
            this.lblCustomerPhone.TabIndex = 2;
            this.lblCustomerPhone.Text = "Số điện thoại:";
            // 
            // lblCustomerNameValue
            // 
            this.lblCustomerNameValue.AutoSize = true;
            this.lblCustomerNameValue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCustomerNameValue.Location = new System.Drawing.Point(150, 15);
            this.lblCustomerNameValue.Name = "lblCustomerNameValue";
            this.lblCustomerNameValue.Size = new System.Drawing.Size(27, 19);
            this.lblCustomerNameValue.TabIndex = 1;
            this.lblCustomerNameValue.Text = "---";
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.AutoSize = true;
            this.lblCustomerName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCustomerName.Location = new System.Drawing.Point(20, 15);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(116, 19);
            this.lblCustomerName.TabIndex = 0;
            this.lblCustomerName.Text = "Tên khách hàng:";
            // 
            // panelProductDetails
            // 
            this.panelProductDetails.BackColor = System.Drawing.Color.White;
            this.panelProductDetails.Controls.Add(this.dgvProducts);
            this.panelProductDetails.Controls.Add(this.lblProductDetailsTitle);
            this.panelProductDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelProductDetails.Location = new System.Drawing.Point(0, 322);
            this.panelProductDetails.Name = "panelProductDetails";
            this.panelProductDetails.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.panelProductDetails.Size = new System.Drawing.Size(900, 250);
            this.panelProductDetails.TabIndex = 5;
            // 
            // dgvProducts
            // 
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AllowUserToDeleteRows = false;
            this.dgvProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProducts.BackgroundColor = System.Drawing.Color.White;
            this.dgvProducts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProducts.Location = new System.Drawing.Point(20, 50);
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.RowHeadersWidth = 51;
            this.dgvProducts.RowTemplate.Height = 29;
            this.dgvProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.Size = new System.Drawing.Size(860, 185);
            this.dgvProducts.TabIndex = 1;
            // 
            // lblProductDetailsTitle
            // 
            this.lblProductDetailsTitle.AutoSize = true;
            this.lblProductDetailsTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblProductDetailsTitle.Location = new System.Drawing.Point(20, 15);
            this.lblProductDetailsTitle.Name = "lblProductDetailsTitle";
            this.lblProductDetailsTitle.Size = new System.Drawing.Size(143, 21);
            this.lblProductDetailsTitle.TabIndex = 0;
            this.lblProductDetailsTitle.Text = "Chi tiết sản phẩm";
            // 
            // panelSummary
            // 
            this.panelSummary.BackColor = System.Drawing.Color.White;
            this.panelSummary.Controls.Add(this.lblFinalTotalValue);
            this.panelSummary.Controls.Add(this.lblFinalTotal);
            this.panelSummary.Controls.Add(this.lblDiscountValue);
            this.panelSummary.Controls.Add(this.lblDiscount);
            this.panelSummary.Controls.Add(this.lblTotalItemsValue);
            this.panelSummary.Controls.Add(this.lblTotalItems);
            this.panelSummary.Controls.Add(this.lblNoteTitle);
            this.panelSummary.Controls.Add(this.txtNote);
            this.panelSummary.Controls.Add(this.btnCloseForm);
            this.panelSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSummary.Location = new System.Drawing.Point(0, 573);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.panelSummary.Size = new System.Drawing.Size(900, 227);
            this.panelSummary.TabIndex = 7;
            // 
            // lblFinalTotalValue
            // 
            this.lblFinalTotalValue.AutoSize = true;
            this.lblFinalTotalValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblFinalTotalValue.Location = new System.Drawing.Point(710, 75);
            this.lblFinalTotalValue.Name = "lblFinalTotalValue";
            this.lblFinalTotalValue.Size = new System.Drawing.Size(33, 21);
            this.lblFinalTotalValue.TabIndex = 5;
            this.lblFinalTotalValue.Text = "0 đ";
            // 
            // lblFinalTotal
            // 
            this.lblFinalTotal.AutoSize = true;
            this.lblFinalTotal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblFinalTotal.Location = new System.Drawing.Point(600, 75);
            this.lblFinalTotal.Name = "lblFinalTotal";
            this.lblFinalTotal.Size = new System.Drawing.Size(96, 21);
            this.lblFinalTotal.TabIndex = 4;
            this.lblFinalTotal.Text = "Thành tiền:";
            // 
            // lblDiscountValue
            // 
            this.lblDiscountValue.AutoSize = true;
            this.lblDiscountValue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDiscountValue.ForeColor = System.Drawing.Color.Red;
            this.lblDiscountValue.Location = new System.Drawing.Point(685, 45);
            this.lblDiscountValue.Name = "lblDiscountValue";
            this.lblDiscountValue.Size = new System.Drawing.Size(29, 19);
            this.lblDiscountValue.TabIndex = 3;
            this.lblDiscountValue.Text = "0 đ";
            // 
            // lblDiscount
            // 
            this.lblDiscount.AutoSize = true;
            this.lblDiscount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDiscount.ForeColor = System.Drawing.Color.Red;
            this.lblDiscount.Location = new System.Drawing.Point(600, 45);
            this.lblDiscount.Name = "lblDiscount";
            this.lblDiscount.Size = new System.Drawing.Size(66, 19);
            this.lblDiscount.TabIndex = 2;
            this.lblDiscount.Text = "Giảm giá:";
            // 
            // lblTotalItemsValue
            // 
            this.lblTotalItemsValue.AutoSize = true;
            this.lblTotalItemsValue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTotalItemsValue.Location = new System.Drawing.Point(710, 15);
            this.lblTotalItemsValue.Name = "lblTotalItemsValue";
            this.lblTotalItemsValue.Size = new System.Drawing.Size(29, 19);
            this.lblTotalItemsValue.TabIndex = 1;
            this.lblTotalItemsValue.Text = "0 đ";
            // 
            // lblTotalItems
            // 
            this.lblTotalItems.AutoSize = true;
            this.lblTotalItems.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTotalItems.Location = new System.Drawing.Point(600, 15);
            this.lblTotalItems.Name = "lblTotalItems";
            this.lblTotalItems.Size = new System.Drawing.Size(105, 19);
            this.lblTotalItems.TabIndex = 0;
            this.lblTotalItems.Text = "Tổng tiền hàng:";
            // 
            // lblNoteTitle
            // 
            this.lblNoteTitle.AutoSize = true;
            this.lblNoteTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblNoteTitle.Location = new System.Drawing.Point(20, 15);
            this.lblNoteTitle.Name = "lblNoteTitle";
            this.lblNoteTitle.Size = new System.Drawing.Size(62, 19);
            this.lblNoteTitle.TabIndex = 6;
            this.lblNoteTitle.Text = "Ghi chú:";
            // 
            // txtNote
            // 
            this.txtNote.BackColor = System.Drawing.Color.White;
            this.txtNote.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNote.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtNote.Location = new System.Drawing.Point(20, 40);
            this.txtNote.Multiline = true;
            this.txtNote.Name = "txtNote";
            this.txtNote.ReadOnly = true;
            this.txtNote.Size = new System.Drawing.Size(550, 60);
            this.txtNote.TabIndex = 7;
            // 
            // btnCloseForm
            // 
            this.btnCloseForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnCloseForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseForm.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCloseForm.ForeColor = System.Drawing.Color.White;
            this.btnCloseForm.Location = new System.Drawing.Point(780, 120);
            this.btnCloseForm.Name = "btnCloseForm";
            this.btnCloseForm.Size = new System.Drawing.Size(100, 40);
            this.btnCloseForm.TabIndex = 8;
            this.btnCloseForm.Text = "Đóng";
            this.btnCloseForm.UseVisualStyleBackColor = false;
            this.btnCloseForm.Click += new System.EventHandler(this.btnCloseForm_Click);
            // 
            // panelSeparator1
            // 
            this.panelSeparator1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panelSeparator1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSeparator1.Location = new System.Drawing.Point(0, 180);
            this.panelSeparator1.Name = "panelSeparator1";
            this.panelSeparator1.Size = new System.Drawing.Size(900, 1);
            this.panelSeparator1.TabIndex = 2;
            // 
            // panelSeparator2
            // 
            this.panelSeparator2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panelSeparator2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSeparator2.Location = new System.Drawing.Point(0, 321);
            this.panelSeparator2.Name = "panelSeparator2";
            this.panelSeparator2.Size = new System.Drawing.Size(900, 1);
            this.panelSeparator2.TabIndex = 4;
            // 
            // panelSeparator3
            // 
            this.panelSeparator3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panelSeparator3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSeparator3.Location = new System.Drawing.Point(0, 572);
            this.panelSeparator3.Name = "panelSeparator3";
            this.panelSeparator3.Size = new System.Drawing.Size(900, 1);
            this.panelSeparator3.TabIndex = 6;
            // 
            // OrderDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(900, 800);
            this.Controls.Add(this.panelSummary);
            this.Controls.Add(this.panelSeparator3);
            this.Controls.Add(this.panelProductDetails);
            this.Controls.Add(this.panelSeparator2);
            this.Controls.Add(this.panelCustomerInfo);
            this.Controls.Add(this.panelSeparator1);
            this.Controls.Add(this.panelOrderInfo);
            this.Controls.Add(this.panelHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "OrderDetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chi tiết đơn hàng";
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelOrderInfo.ResumeLayout(false);
            this.panelOrderInfo.PerformLayout();
            this.panelCustomerInfo.ResumeLayout(false);
            this.panelCustomerInfo.PerformLayout();
            this.panelProductDetails.ResumeLayout(false);
            this.panelProductDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.panelSummary.ResumeLayout(false);
            this.panelSummary.PerformLayout();
            this.ResumeLayout(false);

        }

    }
}
