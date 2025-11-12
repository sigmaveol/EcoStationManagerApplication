using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    partial class StaffControl
    {
        private System.ComponentModel.IContainer components = null;

        private Panel headerPanel;
        private Panel assignmentPanel;
        private Panel kpiPanel;
        private Button btnAddStaff;
        private DataGridView dgvAssignments;
        private DataGridView dgvKPI;

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
            this.titleLabelHeader = new System.Windows.Forms.Label();
            this.titleLabelAssign = new System.Windows.Forms.Label();
            this.titleLabelKPI = new System.Windows.Forms.Label();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.btnAddStaff = new System.Windows.Forms.Button();
            this.assignmentPanel = new System.Windows.Forms.Panel();
            this.dgvAssignments = new System.Windows.Forms.DataGridView();
            this.kpiPanel = new System.Windows.Forms.Panel();
            this.dgvKPI = new System.Windows.Forms.DataGridView();
            this.headerPanel.SuspendLayout();
            this.assignmentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssignments)).BeginInit();
            this.kpiPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKPI)).BeginInit();
            this.SuspendLayout();
            // 
            // titleLabelHeader
            // 
            this.titleLabelHeader.AutoSize = true;
            this.titleLabelHeader.Dock = System.Windows.Forms.DockStyle.Left;
            this.titleLabelHeader.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.titleLabelHeader.Location = new System.Drawing.Point(0, 0);
            this.titleLabelHeader.Name = "titleLabelHeader";
            this.titleLabelHeader.Size = new System.Drawing.Size(248, 37);
            this.titleLabelHeader.TabIndex = 0;
            this.titleLabelHeader.Text = "Nhân sự & Giao vận";
            // 
            // titleLabelAssign
            // 
            this.titleLabelAssign.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleLabelAssign.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.titleLabelAssign.Location = new System.Drawing.Point(15, 15);
            this.titleLabelAssign.Name = "titleLabelAssign";
            this.titleLabelAssign.Size = new System.Drawing.Size(870, 30);
            this.titleLabelAssign.TabIndex = 1;
            this.titleLabelAssign.Text = "Phân công nhân viên";
            // 
            // titleLabelKPI
            // 
            this.titleLabelKPI.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleLabelKPI.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.titleLabelKPI.Location = new System.Drawing.Point(15, 15);
            this.titleLabelKPI.Name = "titleLabelKPI";
            this.titleLabelKPI.Size = new System.Drawing.Size(870, 30);
            this.titleLabelKPI.TabIndex = 1;
            this.titleLabelKPI.Text = "Quản lý ca làm & KPI";
            // 
            // headerPanel
            // 
            this.headerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.headerPanel.Controls.Add(this.titleLabelHeader);
            this.headerPanel.Controls.Add(this.btnAddStaff);
            this.headerPanel.Location = new System.Drawing.Point(20, 20);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(900, 60);
            this.headerPanel.TabIndex = 0;
            // 
            // btnAddStaff
            // 
            this.btnAddStaff.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnAddStaff.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAddStaff.FlatAppearance.BorderSize = 0;
            this.btnAddStaff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddStaff.ForeColor = System.Drawing.Color.White;
            this.btnAddStaff.Location = new System.Drawing.Point(760, 0);
            this.btnAddStaff.Name = "btnAddStaff";
            this.btnAddStaff.Size = new System.Drawing.Size(140, 60);
            this.btnAddStaff.TabIndex = 1;
            this.btnAddStaff.Text = "+ Thêm nhân viên";
            this.btnAddStaff.UseVisualStyleBackColor = false;
            // 
            // assignmentPanel
            // 
            this.assignmentPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assignmentPanel.BackColor = System.Drawing.Color.White;
            this.assignmentPanel.Controls.Add(this.dgvAssignments);
            this.assignmentPanel.Controls.Add(this.titleLabelAssign);
            this.assignmentPanel.Location = new System.Drawing.Point(20, 90);
            this.assignmentPanel.Name = "assignmentPanel";
            this.assignmentPanel.Padding = new System.Windows.Forms.Padding(15);
            this.assignmentPanel.Size = new System.Drawing.Size(900, 250);
            this.assignmentPanel.TabIndex = 1;
            // 
            // dgvAssignments
            // 
            this.dgvAssignments.ColumnHeadersHeight = 29;
            this.dgvAssignments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAssignments.Location = new System.Drawing.Point(15, 45);
            this.dgvAssignments.Name = "dgvAssignments";
            this.dgvAssignments.RowHeadersWidth = 51;
            this.dgvAssignments.Size = new System.Drawing.Size(870, 190);
            this.dgvAssignments.TabIndex = 0;
            // 
            // kpiPanel
            // 
            this.kpiPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kpiPanel.BackColor = System.Drawing.Color.White;
            this.kpiPanel.Controls.Add(this.dgvKPI);
            this.kpiPanel.Controls.Add(this.titleLabelKPI);
            this.kpiPanel.Location = new System.Drawing.Point(20, 360);
            this.kpiPanel.Name = "kpiPanel";
            this.kpiPanel.Padding = new System.Windows.Forms.Padding(15);
            this.kpiPanel.Size = new System.Drawing.Size(900, 250);
            this.kpiPanel.TabIndex = 2;
            // 
            // dgvKPI
            // 
            this.dgvKPI.ColumnHeadersHeight = 29;
            this.dgvKPI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvKPI.Location = new System.Drawing.Point(15, 45);
            this.dgvKPI.Name = "dgvKPI";
            this.dgvKPI.RowHeadersWidth = 51;
            this.dgvKPI.Size = new System.Drawing.Size(870, 190);
            this.dgvKPI.TabIndex = 0;
            // 
            // StaffControl
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.headerPanel);
            this.Controls.Add(this.assignmentPanel);
            this.Controls.Add(this.kpiPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "StaffControl";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(940, 640);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.assignmentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssignments)).EndInit();
            this.kpiPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKPI)).EndInit();
            this.ResumeLayout(false);

        }

        private Label titleLabelHeader;
        private Label titleLabelAssign;
        private Label titleLabelKPI;
    }
}