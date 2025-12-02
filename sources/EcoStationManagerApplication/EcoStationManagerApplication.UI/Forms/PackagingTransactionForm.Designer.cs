using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    partial class PackagingTransactionForm
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
            this.labelError = new System.Windows.Forms.Label();
            this.lblHoldingsInfo = new System.Windows.Forms.Label();
            this.lblOwnershipType = new System.Windows.Forms.Label();
            this.cmbOwnershipType = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblIsProductPackaging = new System.Windows.Forms.Label();
            this.toggleIsProductPackaging = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.lblProduct = new System.Windows.Forms.Label();
            this.cmbProduct = new Guna.UI2.WinForms.Guna2ComboBox();
            this.txtNotes = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtAmount = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblAmountDescription = new System.Windows.Forms.Label();
            this.lblAmount = new System.Windows.Forms.Label();
            this.numQuantity = new System.Windows.Forms.NumericUpDown();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.cmbPackaging = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblPackaging = new System.Windows.Forms.Label();
            this.lblCustomerInfo = new System.Windows.Forms.Label();
            this.btnCancel = new Guna.UI2.WinForms.Guna2Button();
            this.btnSave = new Guna.UI2.WinForms.Guna2Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.labelError);
            this.panelMain.Controls.Add(this.lblHoldingsInfo);
            this.panelMain.Controls.Add(this.lblOwnershipType);
            this.panelMain.Controls.Add(this.cmbOwnershipType);
            this.panelMain.Controls.Add(this.lblIsProductPackaging);
            this.panelMain.Controls.Add(this.toggleIsProductPackaging);
            this.panelMain.Controls.Add(this.lblProduct);
            this.panelMain.Controls.Add(this.cmbProduct);
            this.panelMain.Controls.Add(this.txtNotes);
            this.panelMain.Controls.Add(this.lblNotes);
            this.panelMain.Controls.Add(this.txtAmount);
            this.panelMain.Controls.Add(this.lblAmountDescription);
            this.panelMain.Controls.Add(this.lblAmount);
            this.panelMain.Controls.Add(this.numQuantity);
            this.panelMain.Controls.Add(this.lblQuantity);
            this.panelMain.Controls.Add(this.cmbPackaging);
            this.panelMain.Controls.Add(this.lblPackaging);
            this.panelMain.Controls.Add(this.lblCustomerInfo);
            this.panelMain.Controls.Add(this.btnCancel);
            this.panelMain.Controls.Add(this.btnSave);
            this.panelMain.Controls.Add(this.lblTitle);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(30);
            this.panelMain.Size = new System.Drawing.Size(600, 770);
            this.panelMain.TabIndex = 0;
            // 
            // labelError
            // 
            this.labelError.AutoSize = true;
            this.labelError.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(24)))), ((int)(((byte)(61)))));
            this.labelError.Location = new System.Drawing.Point(30, 600);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(0, 20);
            this.labelError.TabIndex = 14;
            // 
            // lblHoldingsInfo
            // 
            this.lblHoldingsInfo.AutoSize = true;
            this.lblHoldingsInfo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblHoldingsInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.lblHoldingsInfo.Location = new System.Drawing.Point(316, 230);
            this.lblHoldingsInfo.Name = "lblHoldingsInfo";
            this.lblHoldingsInfo.Size = new System.Drawing.Size(0, 20);
            this.lblHoldingsInfo.TabIndex = 13;
            this.lblHoldingsInfo.Visible = false;
            // 
            // lblOwnershipType
            // 
            this.lblOwnershipType.AutoSize = true;
            this.lblOwnershipType.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblOwnershipType.Location = new System.Drawing.Point(26, 369);
            this.lblOwnershipType.Name = "lblOwnershipType";
            this.lblOwnershipType.Size = new System.Drawing.Size(89, 23);
            this.lblOwnershipType.TabIndex = 17;
            this.lblOwnershipType.Text = "Hình thức:";
            // 
            // cmbOwnershipType
            // 
            this.cmbOwnershipType.BackColor = System.Drawing.Color.Transparent;
            this.cmbOwnershipType.BorderRadius = 5;
            this.cmbOwnershipType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbOwnershipType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOwnershipType.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbOwnershipType.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbOwnershipType.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbOwnershipType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbOwnershipType.ItemHeight = 30;
            this.cmbOwnershipType.Location = new System.Drawing.Point(27, 395);
            this.cmbOwnershipType.Name = "cmbOwnershipType";
            this.cmbOwnershipType.Size = new System.Drawing.Size(540, 36);
            this.cmbOwnershipType.TabIndex = 16;
            // 
            // lblIsProductPackaging
            // 
            this.lblIsProductPackaging.AutoSize = true;
            this.lblIsProductPackaging.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblIsProductPackaging.Location = new System.Drawing.Point(30, 573);
            this.lblIsProductPackaging.Name = "lblIsProductPackaging";
            this.lblIsProductPackaging.Size = new System.Drawing.Size(196, 23);
            this.lblIsProductPackaging.TabIndex = 19;
            this.lblIsProductPackaging.Text = "Là bao bì của sản phẩm:";
            // 
            // toggleIsProductPackaging
            // 
            this.toggleIsProductPackaging.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.toggleIsProductPackaging.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.toggleIsProductPackaging.CheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.toggleIsProductPackaging.CheckedState.InnerColor = System.Drawing.Color.White;
            this.toggleIsProductPackaging.Location = new System.Drawing.Point(260, 577);
            this.toggleIsProductPackaging.Name = "toggleIsProductPackaging";
            this.toggleIsProductPackaging.Size = new System.Drawing.Size(47, 25);
            this.toggleIsProductPackaging.TabIndex = 18;
            this.toggleIsProductPackaging.UncheckedState.BorderColor = System.Drawing.Color.Gray;
            this.toggleIsProductPackaging.UncheckedState.FillColor = System.Drawing.Color.Gray;
            this.toggleIsProductPackaging.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.toggleIsProductPackaging.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.toggleIsProductPackaging.CheckedChanged += new System.EventHandler(this.toggleIsProductPackaging_CheckedChanged);
            // 
            // lblProduct
            // 
            this.lblProduct.AutoSize = true;
            this.lblProduct.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblProduct.Location = new System.Drawing.Point(30, 613);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(91, 23);
            this.lblProduct.TabIndex = 21;
            this.lblProduct.Text = "Sản phẩm:";
            this.lblProduct.Visible = false;
            // 
            // cmbProduct
            // 
            this.cmbProduct.BackColor = System.Drawing.Color.Transparent;
            this.cmbProduct.BorderRadius = 5;
            this.cmbProduct.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProduct.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbProduct.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbProduct.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbProduct.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbProduct.ItemHeight = 30;
            this.cmbProduct.Location = new System.Drawing.Point(31, 641);
            this.cmbProduct.Name = "cmbProduct";
            this.cmbProduct.Size = new System.Drawing.Size(540, 36);
            this.cmbProduct.TabIndex = 20;
            this.cmbProduct.Visible = false;
            // 
            // txtNotes
            // 
            this.txtNotes.BorderRadius = 5;
            this.txtNotes.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNotes.DefaultText = "";
            this.txtNotes.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtNotes.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtNotes.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtNotes.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtNotes.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtNotes.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtNotes.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtNotes.Location = new System.Drawing.Point(30, 472);
            this.txtNotes.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.PlaceholderText = "Ghi chú (tùy chọn)";
            this.txtNotes.SelectedText = "";
            this.txtNotes.Size = new System.Drawing.Size(540, 80);
            this.txtNotes.TabIndex = 12;
            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblNotes.Location = new System.Drawing.Point(30, 443);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(73, 23);
            this.lblNotes.TabIndex = 11;
            this.lblNotes.Text = "Ghi chú:";
            // 
            // txtAmount
            // 
            this.txtAmount.BorderRadius = 5;
            this.txtAmount.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtAmount.DefaultText = "";
            this.txtAmount.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtAmount.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtAmount.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtAmount.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtAmount.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtAmount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtAmount.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtAmount.Location = new System.Drawing.Point(31, 314);
            this.txtAmount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.PlaceholderText = "0";
            this.txtAmount.SelectedText = "";
            this.txtAmount.Size = new System.Drawing.Size(540, 42);
            this.txtAmount.TabIndex = 10;
            // 
            // lblAmountDescription
            // 
            this.lblAmountDescription.AutoSize = true;
            this.lblAmountDescription.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblAmountDescription.ForeColor = System.Drawing.Color.Gray;
            this.lblAmountDescription.Location = new System.Drawing.Point(30, 260);
            this.lblAmountDescription.Name = "lblAmountDescription";
            this.lblAmountDescription.Size = new System.Drawing.Size(0, 19);
            this.lblAmountDescription.TabIndex = 9;
            // 
            // lblAmount
            // 
            this.lblAmount.AutoSize = true;
            this.lblAmount.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblAmount.Location = new System.Drawing.Point(30, 230);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(0, 23);
            this.lblAmount.TabIndex = 8;
            // 
            // numQuantity
            // 
            this.numQuantity.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.numQuantity.Location = new System.Drawing.Point(30, 189);
            this.numQuantity.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numQuantity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numQuantity.Name = "numQuantity";
            this.numQuantity.Size = new System.Drawing.Size(540, 27);
            this.numQuantity.TabIndex = 7;
            this.numQuantity.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblQuantity.Location = new System.Drawing.Point(30, 159);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(82, 23);
            this.lblQuantity.TabIndex = 6;
            this.lblQuantity.Text = "Số lượng:";
            // 
            // cmbPackaging
            // 
            this.cmbPackaging.BackColor = System.Drawing.Color.Transparent;
            this.cmbPackaging.BorderRadius = 5;
            this.cmbPackaging.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbPackaging.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPackaging.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbPackaging.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbPackaging.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbPackaging.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbPackaging.ItemHeight = 30;
            this.cmbPackaging.Location = new System.Drawing.Point(30, 111);
            this.cmbPackaging.Name = "cmbPackaging";
            this.cmbPackaging.Size = new System.Drawing.Size(540, 36);
            this.cmbPackaging.TabIndex = 5;
            this.cmbPackaging.SelectedIndexChanged += new System.EventHandler(this.cmbPackaging_SelectedIndexChanged);
            // 
            // lblPackaging
            // 
            this.lblPackaging.AutoSize = true;
            this.lblPackaging.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPackaging.Location = new System.Drawing.Point(30, 83);
            this.lblPackaging.Name = "lblPackaging";
            this.lblPackaging.Size = new System.Drawing.Size(62, 23);
            this.lblPackaging.TabIndex = 4;
            this.lblPackaging.Text = "Bao bì:";
            // 
            // lblCustomerInfo
            // 
            this.lblCustomerInfo.AutoSize = true;
            this.lblCustomerInfo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCustomerInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.lblCustomerInfo.Location = new System.Drawing.Point(30, 60);
            this.lblCustomerInfo.Name = "lblCustomerInfo";
            this.lblCustomerInfo.Size = new System.Drawing.Size(0, 23);
            this.lblCustomerInfo.TabIndex = 3;
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
            this.btnCancel.Location = new System.Drawing.Point(301, 701);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 48);
            this.btnCancel.TabIndex = 2;
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
            this.btnSave.Location = new System.Drawing.Point(428, 701);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(142, 48);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Xác nhận";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(30, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(0, 37);
            this.lblTitle.TabIndex = 0;
            // 
            // PackagingTransactionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 770);
            this.Controls.Add(this.panelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PackagingTransactionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Giao dịch bao bì";
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel panelMain;
        private System.Windows.Forms.Label lblTitle;
        private Guna.UI2.WinForms.Guna2Button btnSave;
        private Guna.UI2.WinForms.Guna2Button btnCancel;
        private System.Windows.Forms.Label lblCustomerInfo;
        private System.Windows.Forms.Label lblPackaging;
        private Guna.UI2.WinForms.Guna2ComboBox cmbPackaging;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.NumericUpDown numQuantity;
        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.Label lblAmountDescription;
        private Guna.UI2.WinForms.Guna2TextBox txtAmount;
        private System.Windows.Forms.Label lblNotes;
        private Guna.UI2.WinForms.Guna2TextBox txtNotes;
        private System.Windows.Forms.Label lblHoldingsInfo;
        private System.Windows.Forms.Label labelError;
        private System.Windows.Forms.Label lblOwnershipType;
        private Guna.UI2.WinForms.Guna2ComboBox cmbOwnershipType;
        private System.Windows.Forms.Label lblIsProductPackaging;
        private Guna.UI2.WinForms.Guna2ToggleSwitch toggleIsProductPackaging;
        private System.Windows.Forms.Label lblProduct;
        private Guna.UI2.WinForms.Guna2ComboBox cmbProduct;
    }
}
