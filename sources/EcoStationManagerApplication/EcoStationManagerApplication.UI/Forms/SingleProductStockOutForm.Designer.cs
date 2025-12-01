using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    partial class SingleProductStockOutForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle;
        private Label lblProduct;
        private Guna2ComboBox cmbProduct;
        private Label lblQuantity;
        private Guna2TextBox txtQuantity;
        private Label lblStock;
        private Guna2Button btnExport;
        private Guna2Button btnCancel;
        private Label lblBatch;
        private Guna2ComboBox cmbBatch;
        private Label labelError;


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
            this.lblProduct = new System.Windows.Forms.Label();
            this.cmbProduct = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.txtQuantity = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblStock = new System.Windows.Forms.Label();
            this.btnExport = new Guna.UI2.WinForms.Guna2Button();
            this.btnCancel = new Guna.UI2.WinForms.Guna2Button();
            this.lblBatch = new System.Windows.Forms.Label();
            this.cmbBatch = new Guna.UI2.WinForms.Guna2ComboBox();
            this.labelError = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(301, 37);
            this.lblTitle.TabIndex = 7;
            this.lblTitle.Text = "Chọn sản phẩm bao bì";
            // 
            // lblProduct
            // 
            this.lblProduct.AutoSize = true;
            this.lblProduct.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblProduct.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblProduct.Location = new System.Drawing.Point(22, 80);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(90, 23);
            this.lblProduct.TabIndex = 6;
            this.lblProduct.Text = "Sản phẩm";
            // 
            // cmbProduct
            // 
            this.cmbProduct.BackColor = System.Drawing.Color.Transparent;
            this.cmbProduct.BorderRadius = 8;
            this.cmbProduct.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProduct.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.cmbProduct.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.cmbProduct.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbProduct.ForeColor = System.Drawing.Color.Black;
            this.cmbProduct.ItemHeight = 30;
            this.cmbProduct.Location = new System.Drawing.Point(26, 106);
            this.cmbProduct.Name = "cmbProduct";
            this.cmbProduct.Size = new System.Drawing.Size(469, 36);
            this.cmbProduct.TabIndex = 5;
            this.cmbProduct.SelectedIndexChanged += new System.EventHandler(this.cmbProduct_SelectedIndexChanged);
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblQuantity.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblQuantity.Location = new System.Drawing.Point(22, 160);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(83, 23);
            this.lblQuantity.TabIndex = 4;
            this.lblQuantity.Text = "Số lượng";
            // 
            // txtQuantity
            // 
            this.txtQuantity.BorderRadius = 8;
            this.txtQuantity.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtQuantity.DefaultText = "";
            this.txtQuantity.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtQuantity.Location = new System.Drawing.Point(26, 186);
            this.txtQuantity.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.PlaceholderText = "Nhập số lượng";
            this.txtQuantity.SelectedText = "";
            this.txtQuantity.Size = new System.Drawing.Size(200, 40);
            this.txtQuantity.TabIndex = 3;
            this.txtQuantity.TextChanged += new System.EventHandler(this.txtQuantity_TextChanged);
            // 
            // lblStock
            // 
            this.lblStock.AutoSize = true;
            this.lblStock.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblStock.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblStock.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.lblStock.Location = new System.Drawing.Point(250, 194);
            this.lblStock.Name = "lblStock";
            this.lblStock.Size = new System.Drawing.Size(76, 23);
            this.lblStock.TabIndex = 2;
            this.lblStock.Text = "Tồn kho:";
            // 
            // btnExport
            // 
            this.btnExport.BorderRadius = 8;
            this.btnExport.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Location = new System.Drawing.Point(267, 348);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(110, 40);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "OK";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BorderRadius = 8;
            this.btnCancel.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(401, 348);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 40);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblBatch
            // 
            this.lblBatch.AutoSize = true;
            this.lblBatch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblBatch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblBatch.Location = new System.Drawing.Point(22, 240);
            this.lblBatch.Name = "lblBatch";
            this.lblBatch.Size = new System.Drawing.Size(74, 23);
            this.lblBatch.TabIndex = 8;
            this.lblBatch.Text = "Lô hàng";
            // 
            // cmbBatch
            // 
            this.cmbBatch.BackColor = System.Drawing.Color.Transparent;
            this.cmbBatch.BorderRadius = 8;
            this.cmbBatch.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbBatch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBatch.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.cmbBatch.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.cmbBatch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbBatch.ForeColor = System.Drawing.Color.Black;
            this.cmbBatch.ItemHeight = 30;
            this.cmbBatch.Location = new System.Drawing.Point(26, 266);
            this.cmbBatch.Name = "cmbBatch";
            this.cmbBatch.Size = new System.Drawing.Size(473, 36);
            this.cmbBatch.TabIndex = 9;
            this.cmbBatch.SelectedIndexChanged += new System.EventHandler(this.cmbBatch_SelectedIndexChanged);
            this.cmbBatch.TextChanged += new System.EventHandler(this.cmbBatch_SelectedIndexChanged);
            this.cmbBatch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbBatch_KeyPress);
            // 
            // labelError
            // 
            this.labelError.AutoSize = true;
            this.labelError.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(24)))), ((int)(((byte)(61)))));
            this.labelError.Location = new System.Drawing.Point(26, 310);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(0, 20);
            this.labelError.TabIndex = 10;
            // 
            // SingleProductStockOutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 427);
            this.Controls.Add(this.labelError);
            this.Controls.Add(this.cmbBatch);
            this.Controls.Add(this.lblBatch);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.lblStock);
            this.Controls.Add(this.txtQuantity);
            this.Controls.Add(this.lblQuantity);
            this.Controls.Add(this.cmbProduct);
            this.Controls.Add(this.lblProduct);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SingleProductStockOutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Xuất 1 sản phẩm";
            this.Load += new System.EventHandler(this.SingleProductStockOutForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SingleProductStockOutForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}