using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    partial class ProductSelectionForm
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox cmbProduct;
        private NumericUpDown numQuantity;
        private NumericUpDown numUnitPrice;
        private DateTimePicker dtpExpiryDate;
        private CheckBox chkHasExpiryDate;
        private Button btnOK;
        private Button btnCancel;
        private Label lblProduct;
        private Label lblQuantity;
        private Label lblUnitPrice;
        private Label lblExpiryDate;
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
            this.lblProduct = new System.Windows.Forms.Label();
            this.cmbProduct = new System.Windows.Forms.ComboBox();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.numQuantity = new System.Windows.Forms.NumericUpDown();
            this.lblUnitPrice = new System.Windows.Forms.Label();
            this.numUnitPrice = new System.Windows.Forms.NumericUpDown();
            this.lblExpiryDate = new System.Windows.Forms.Label();
            this.chkHasExpiryDate = new System.Windows.Forms.CheckBox();
            this.dtpExpiryDate = new System.Windows.Forms.DateTimePicker();
            this.labelError = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnitPrice)).BeginInit();
            this.SuspendLayout();
            // 
            // lblProduct
            // 
            this.lblProduct.AutoSize = true;
            this.lblProduct.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblProduct.Location = new System.Drawing.Point(20, 20);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(162, 23);
            this.lblProduct.TabIndex = 0;
            this.lblProduct.Text = "Sản phẩm/Bao bì *";
            // 
            // cmbProduct
            // 
            this.cmbProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProduct.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbProduct.Location = new System.Drawing.Point(20, 50);
            this.cmbProduct.Name = "cmbProduct";
            this.cmbProduct.Size = new System.Drawing.Size(390, 31);
            this.cmbProduct.TabIndex = 1;
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblQuantity.Location = new System.Drawing.Point(20, 100);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(96, 23);
            this.lblQuantity.TabIndex = 2;
            this.lblQuantity.Text = "Số lượng *";
            // 
            // numQuantity
            // 
            this.numQuantity.DecimalPlaces = 2;
            this.numQuantity.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numQuantity.Location = new System.Drawing.Point(20, 130);
            this.numQuantity.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numQuantity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numQuantity.Name = "numQuantity";
            this.numQuantity.Size = new System.Drawing.Size(390, 30);
            this.numQuantity.TabIndex = 3;
            this.numQuantity.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            // 
            // lblUnitPrice
            // 
            this.lblUnitPrice.AutoSize = true;
            this.lblUnitPrice.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblUnitPrice.Location = new System.Drawing.Point(20, 170);
            this.lblUnitPrice.Name = "lblUnitPrice";
            this.lblUnitPrice.Size = new System.Drawing.Size(79, 23);
            this.lblUnitPrice.TabIndex = 4;
            this.lblUnitPrice.Text = "Đơn giá *";
            // 
            // numUnitPrice
            // 
            this.numUnitPrice.DecimalPlaces = 0;
            this.numUnitPrice.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numUnitPrice.Location = new System.Drawing.Point(20, 200);
            this.numUnitPrice.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.numUnitPrice.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numUnitPrice.Name = "numUnitPrice";
            this.numUnitPrice.Size = new System.Drawing.Size(390, 30);
            this.numUnitPrice.TabIndex = 5;
            this.numUnitPrice.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // lblExpiryDate
            // 
            this.lblExpiryDate.AutoSize = true;
            this.lblExpiryDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblExpiryDate.Location = new System.Drawing.Point(20, 240);
            this.lblExpiryDate.Name = "lblExpiryDate";
            this.lblExpiryDate.Size = new System.Drawing.Size(117, 23);
            this.lblExpiryDate.TabIndex = 6;
            this.lblExpiryDate.Text = "Hạn sử dụng:";
            // 
            // chkHasExpiryDate
            // 
            this.chkHasExpiryDate.AutoSize = true;
            this.chkHasExpiryDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkHasExpiryDate.Location = new System.Drawing.Point(20, 270);
            this.chkHasExpiryDate.Name = "chkHasExpiryDate";
            this.chkHasExpiryDate.Size = new System.Drawing.Size(120, 24);
            this.chkHasExpiryDate.TabIndex = 7;
            this.chkHasExpiryDate.Text = "Có hạn sử dụng";
            this.chkHasExpiryDate.UseVisualStyleBackColor = true;
            this.chkHasExpiryDate.CheckedChanged += new System.EventHandler(this.ChkHasExpiryDate_CheckedChanged);
            // 
            // dtpExpiryDate
            // 
            this.dtpExpiryDate.Enabled = false;
            this.dtpExpiryDate.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpExpiryDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpExpiryDate.Location = new System.Drawing.Point(150, 268);
            this.dtpExpiryDate.Name = "dtpExpiryDate";
            this.dtpExpiryDate.Size = new System.Drawing.Size(260, 30);
            this.dtpExpiryDate.TabIndex = 8;
            // 
            // labelError
            // 
            this.labelError.AutoSize = true;
            this.labelError.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(24)))), ((int)(((byte)(61)))));
            this.labelError.Location = new System.Drawing.Point(20, 300);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(0, 20);
            this.labelError.TabIndex = 9;
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(250, 310);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 35);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(335, 310);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 35);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // ProductSelectionForm
            // 
            this.AcceptButton = this.btnOK;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(432, 360);
            this.Controls.Add(this.lblProduct);
            this.Controls.Add(this.cmbProduct);
            this.Controls.Add(this.lblQuantity);
            this.Controls.Add(this.numQuantity);
            this.Controls.Add(this.lblUnitPrice);
            this.Controls.Add(this.numUnitPrice);
            this.Controls.Add(this.lblExpiryDate);
            this.Controls.Add(this.chkHasExpiryDate);
            this.Controls.Add(this.dtpExpiryDate);
            this.Controls.Add(this.labelError);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProductSelectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chọn sản phẩm/bao bì";
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnitPrice)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}