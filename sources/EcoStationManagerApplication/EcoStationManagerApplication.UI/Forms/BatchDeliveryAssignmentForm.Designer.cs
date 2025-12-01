using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    partial class BatchDeliveryAssignmentForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvOrders = new System.Windows.Forms.DataGridView();
            this.cmbDriver = new Guna.UI2.WinForms.Guna2ComboBox();
            this.btnAssignSelected = new Guna.UI2.WinForms.Guna2Button();
            this.btnAutoAssign = new Guna.UI2.WinForms.Guna2Button();
            this.btnCancel = new Guna.UI2.WinForms.Guna2Button();
            this.btnSelectAll = new Guna.UI2.WinForms.Guna2Button();
            this.btnDeselectAll = new Guna.UI2.WinForms.Guna2Button();
            this.lblStats = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelGrid = new System.Windows.Forms.Panel();
            this.panelAssignment = new System.Windows.Forms.Panel();
            this.lblDriver = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).BeginInit();
            this.panelGrid.SuspendLayout();
            this.panelAssignment.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvOrders
            // 
            this.dgvOrders.AllowUserToAddRows = false;
            this.dgvOrders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrders.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvOrders.Location = new System.Drawing.Point(0, 0);
            this.dgvOrders.Name = "dgvOrders";
            this.dgvOrders.RowHeadersWidth = 51;
            this.dgvOrders.RowTemplate.Height = 24;
            this.dgvOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrders.Size = new System.Drawing.Size(838, 348);
            this.dgvOrders.TabIndex = 0;
            // 
            // cmbDriver
            // 
            this.cmbDriver.BackColor = System.Drawing.Color.Transparent;
            this.cmbDriver.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbDriver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDriver.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbDriver.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbDriver.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbDriver.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbDriver.ItemHeight = 30;
            this.cmbDriver.Location = new System.Drawing.Point(100, 5);
            this.cmbDriver.Name = "cmbDriver";
            this.cmbDriver.Size = new System.Drawing.Size(250, 36);
            this.cmbDriver.TabIndex = 1;
            // 
            // btnAssignSelected
            // 
            this.btnAssignSelected.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAssignSelected.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnAssignSelected.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnAssignSelected.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnAssignSelected.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnAssignSelected.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAssignSelected.ForeColor = System.Drawing.Color.White;
            this.btnAssignSelected.Location = new System.Drawing.Point(372, 5);
            this.btnAssignSelected.Name = "btnAssignSelected";
            this.btnAssignSelected.Size = new System.Drawing.Size(228, 36);
            this.btnAssignSelected.TabIndex = 2;
            this.btnAssignSelected.Text = "Phân công cho đơn đã chọn";
            this.btnAssignSelected.Click += new System.EventHandler(this.BtnAssignSelected_Click);
            // 
            // btnAutoAssign
            // 
            this.btnAutoAssign.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAutoAssign.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnAutoAssign.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnAutoAssign.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnAutoAssign.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnAutoAssign.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAutoAssign.ForeColor = System.Drawing.Color.White;
            this.btnAutoAssign.Location = new System.Drawing.Point(617, 3);
            this.btnAutoAssign.Name = "btnAutoAssign";
            this.btnAutoAssign.Size = new System.Drawing.Size(194, 36);
            this.btnAutoAssign.TabIndex = 3;
            this.btnAutoAssign.Text = "Phân công tự động";
            this.btnAutoAssign.Click += new System.EventHandler(this.BtnAutoAssign_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCancel.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCancel.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCancel.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCancel.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(721, 50);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 36);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Hủy";
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSelectAll.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSelectAll.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSelectAll.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSelectAll.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.btnSelectAll.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSelectAll.ForeColor = System.Drawing.Color.White;
            this.btnSelectAll.Location = new System.Drawing.Point(0, 50);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(120, 36);
            this.btnSelectAll.TabIndex = 5;
            this.btnSelectAll.Text = "Chọn tất cả";
            this.btnSelectAll.Click += new System.EventHandler(this.BtnSelectAll_Click);
            // 
            // btnDeselectAll
            // 
            this.btnDeselectAll.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnDeselectAll.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnDeselectAll.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnDeselectAll.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnDeselectAll.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.btnDeselectAll.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnDeselectAll.ForeColor = System.Drawing.Color.White;
            this.btnDeselectAll.Location = new System.Drawing.Point(130, 50);
            this.btnDeselectAll.Name = "btnDeselectAll";
            this.btnDeselectAll.Size = new System.Drawing.Size(129, 36);
            this.btnDeselectAll.TabIndex = 6;
            this.btnDeselectAll.Text = "Bỏ chọn tất cả";
            this.btnDeselectAll.Click += new System.EventHandler(this.BtnDeselectAll_Click);
            // 
            // lblStats
            // 
            this.lblStats.AutoSize = true;
            this.lblStats.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblStats.Location = new System.Drawing.Point(289, 66);
            this.lblStats.Name = "lblStats";
            this.lblStats.Size = new System.Drawing.Size(151, 20);
            this.lblStats.TabIndex = 7;
            this.lblStats.Text = "Đã chọn: 0 / Tổng: 0";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(0, 28);
            this.lblTitle.TabIndex = 8;
            // 
            // panelGrid
            // 
            this.panelGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelGrid.Controls.Add(this.dgvOrders);
            this.panelGrid.Location = new System.Drawing.Point(20, 60);
            this.panelGrid.Name = "panelGrid";
            this.panelGrid.Size = new System.Drawing.Size(840, 350);
            this.panelGrid.TabIndex = 9;
            // 
            // panelAssignment
            // 
            this.panelAssignment.Controls.Add(this.lblDriver);
            this.panelAssignment.Controls.Add(this.cmbDriver);
            this.panelAssignment.Controls.Add(this.btnAssignSelected);
            this.panelAssignment.Controls.Add(this.btnAutoAssign);
            this.panelAssignment.Controls.Add(this.btnSelectAll);
            this.panelAssignment.Controls.Add(this.btnDeselectAll);
            this.panelAssignment.Controls.Add(this.lblStats);
            this.panelAssignment.Controls.Add(this.btnCancel);
            this.panelAssignment.Location = new System.Drawing.Point(23, 439);
            this.panelAssignment.Name = "panelAssignment";
            this.panelAssignment.Size = new System.Drawing.Size(840, 111);
            this.panelAssignment.TabIndex = 10;
            // 
            // lblDriver
            // 
            this.lblDriver.AutoSize = true;
            this.lblDriver.Location = new System.Drawing.Point(0, 10);
            this.lblDriver.Name = "lblDriver";
            this.lblDriver.Size = new System.Drawing.Size(75, 16);
            this.lblDriver.TabIndex = 8;
            this.lblDriver.Text = "Chọn tài xế:";
            // 
            // BatchDeliveryAssignmentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.panelAssignment);
            this.Controls.Add(this.panelGrid);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BatchDeliveryAssignmentForm";
            this.Padding = new System.Windows.Forms.Padding(20);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Phân công đơn hàng";
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).EndInit();
            this.panelGrid.ResumeLayout(false);
            this.panelAssignment.ResumeLayout(false);
            this.panelAssignment.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvOrders;
        private Guna2ComboBox cmbDriver;
        private Guna2Button btnAssignSelected;
        private Guna2Button btnAutoAssign;
        private Guna2Button btnCancel;
        private Guna2Button btnSelectAll;
        private Guna2Button btnDeselectAll;
        private System.Windows.Forms.Label lblStats;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelGrid;
        private System.Windows.Forms.Panel panelAssignment;
        private System.Windows.Forms.Label lblDriver;
    }
}