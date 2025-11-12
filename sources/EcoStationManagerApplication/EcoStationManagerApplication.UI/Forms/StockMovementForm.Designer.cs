using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms // Đảm bảo đúng namespace
{
    partial class StockMovementForm
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
            this.lblProductInfo = new System.Windows.Forms.Label();
            this.grpAction = new System.Windows.Forms.GroupBox();
            this.radStockIn = new System.Windows.Forms.RadioButton();
            this.radStockOut = new System.Windows.Forms.RadioButton();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.numQuantity = new System.Windows.Forms.NumericUpDown();
            this.lblNote = new System.Windows.Forms.Label();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpAction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).BeginInit();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(25, 20);
            this.lblTitle.Size = new System.Drawing.Size(126, 25);
            this.lblTitle.Text = "Nhập/Xuất Kho";

            // lblProductInfo
            this.lblProductInfo.AutoSize = true;
            this.lblProductInfo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblProductInfo.Location = new System.Drawing.Point(27, 60);
            this.lblProductInfo.Size = new System.Drawing.Size(171, 19);
            this.lblProductInfo.Text = "Sản phẩm: [Tên sản phẩm]";

            // grpAction
            this.grpAction.Controls.Add(this.radStockOut);
            this.grpAction.Controls.Add(this.radStockIn);
            this.grpAction.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpAction.Location = new System.Drawing.Point(30, 100);
            this.grpAction.Size = new System.Drawing.Size(320, 60);
            this.grpAction.TabIndex = 0;
            this.grpAction.TabStop = false;
            this.grpAction.Text = "Hành động";

            // radStockIn
            this.radStockIn.AutoSize = true;
            this.radStockIn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.radStockIn.Location = new System.Drawing.Point(20, 25);
            this.radStockIn.Size = new System.Drawing.Size(78, 19);
            this.radStockIn.TabIndex = 0;
            this.radStockIn.TabStop = true;
            this.radStockIn.Text = "Nhập kho";
            this.radStockIn.UseVisualStyleBackColor = true;

            // radStockOut
            this.radStockOut.AutoSize = true;
            this.radStockOut.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.radStockOut.Location = new System.Drawing.Point(170, 25);
            this.radStockOut.Size = new System.Drawing.Size(73, 19);
            this.radStockOut.TabIndex = 1;
            this.radStockOut.TabStop = true;
            this.radStockOut.Text = "Xuất kho";
            this.radStockOut.UseVisualStyleBackColor = true;

            // lblQuantity
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblQuantity.Location = new System.Drawing.Point(27, 180);
            this.lblQuantity.Size = new System.Drawing.Size(57, 15);
            this.lblQuantity.Text = "Số lượng";

            // numQuantity
            this.numQuantity.Location = new System.Drawing.Point(30, 200);
            this.numQuantity.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numQuantity.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            this.numQuantity.Size = new System.Drawing.Size(150, 23);
            this.numQuantity.TabIndex = 1;
            this.numQuantity.Value = new decimal(new int[] { 1, 0, 0, 0 });

            // lblNote
            this.lblNote.AutoSize = true;
            this.lblNote.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblNote.Location = new System.Drawing.Point(27, 240);
            this.lblNote.Size = new System.Drawing.Size(50, 15);
            this.lblNote.Text = "Ghi chú";

            // txtNote
            this.txtNote.Location = new System.Drawing.Point(30, 260);
            this.txtNote.Size = new System.Drawing.Size(320, 23);
            this.txtNote.TabIndex = 2;

            // btnSave
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(46, 125, 50);
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(250, 310);
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // btnCancel
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(140, 310);
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // StockMovementForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(384, 371);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtNote);
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.numQuantity);
            this.Controls.Add(this.lblQuantity);
            this.Controls.Add(this.grpAction);
            this.Controls.Add(this.lblProductInfo);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StockMovementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Nhập/Xuất Kho";
            this.grpAction.ResumeLayout(false);
            this.grpAction.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}