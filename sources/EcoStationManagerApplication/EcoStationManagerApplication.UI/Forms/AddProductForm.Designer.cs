using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms // Đảm bảo đúng namespace
{
    partial class AddProductForm
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

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblSKU = new System.Windows.Forms.Label();
            this.txtSKU = new System.Windows.Forms.TextBox();
            this.lblPrice = new System.Windows.Forms.Label();
            this.numPrice = new System.Windows.Forms.NumericUpDown();
            this.lblStock = new System.Windows.Forms.Label();
            this.numStock = new System.Windows.Forms.NumericUpDown();
            this.lblAlertLevel = new System.Windows.Forms.Label();
            this.numAlertLevel = new System.Windows.Forms.NumericUpDown();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAlertLevel)).BeginInit();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(25, 20);
            this.lblTitle.Size = new System.Drawing.Size(186, 25);
            this.lblTitle.Text = "Thêm Sản Phẩm Mới";

            // lblName
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(27, 70);
            this.lblName.Size = new System.Drawing.Size(88, 15);
            this.lblName.Text = "Tên sản phẩm (*)";

            // txtName
            this.txtName.Location = new System.Drawing.Point(30, 90);
            this.txtName.Size = new System.Drawing.Size(320, 23);
            this.txtName.TabIndex = 0;

            // lblSKU
            this.lblSKU.AutoSize = true;
            this.lblSKU.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSKU.Location = new System.Drawing.Point(27, 130);
            this.lblSKU.Size = new System.Drawing.Size(64, 15);
            this.lblSKU.Text = "Mã SKU (*)";

            // txtSKU
            this.txtSKU.Location = new System.Drawing.Point(30, 150);
            this.txtSKU.Size = new System.Drawing.Size(150, 23);
            this.txtSKU.TabIndex = 1;

            // lblPrice
            this.lblPrice.AutoSize = true;
            this.lblPrice.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPrice.Location = new System.Drawing.Point(200, 130);
            this.lblPrice.Size = new System.Drawing.Size(49, 15);
            this.lblPrice.Text = "Giá bán";

            // numPrice
            this.numPrice.Location = new System.Drawing.Point(200, 150);
            this.numPrice.Size = new System.Drawing.Size(150, 23);
            this.numPrice.TabIndex = 2;
            this.numPrice.Maximum = 10000000;
            this.numPrice.DecimalPlaces = 0;
            this.numPrice.ThousandsSeparator = true;

            // lblStock
            this.lblStock.AutoSize = true;
            this.lblStock.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblStock.Location = new System.Drawing.Point(27, 190);
            this.lblStock.Size = new System.Drawing.Size(89, 15);
            this.lblStock.Text = "Tồn kho ban đầu";

            // numStock
            this.numStock.Location = new System.Drawing.Point(30, 210);
            this.numStock.Size = new System.Drawing.Size(150, 23);
            this.numStock.TabIndex = 3;

            // lblAlertLevel
            this.lblAlertLevel.AutoSize = true;
            this.lblAlertLevel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblAlertLevel.Location = new System.Drawing.Point(200, 190);
            this.lblAlertLevel.Size = new System.Drawing.Size(83, 15);
            this.lblAlertLevel.Text = "Mức cảnh báo";

            // numAlertLevel
            this.numAlertLevel.Location = new System.Drawing.Point(200, 210);
            this.numAlertLevel.Size = new System.Drawing.Size(150, 23);
            this.numAlertLevel.TabIndex = 4;
            this.numAlertLevel.Value = 10;

            // btnSave
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(46, 125, 50);
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(250, 260);
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // btnCancel
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(140, 260);
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // AddProductForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(384, 321);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.numAlertLevel);
            this.Controls.Add(this.lblAlertLevel);
            this.Controls.Add(this.numStock);
            this.Controls.Add(this.lblStock);
            this.Controls.Add(this.numPrice);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.txtSKU);
            this.Controls.Add(this.lblSKU);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddProductForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thêm Sản Phẩm Mới";
            ((System.ComponentModel.ISupportInitialize)(this.numPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAlertLevel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}