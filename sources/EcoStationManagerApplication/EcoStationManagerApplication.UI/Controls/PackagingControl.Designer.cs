using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    partial class PackagingControl
    {
        private System.ComponentModel.IContainer components = null;

        private Panel headerPanel;
        private Panel statsPanel;
        private Panel packagingStatusPanel;
        private Panel reuseStatsPanel;
        private Button btnAddPackaging;
        private TextBox txtDistributed;
        private TextBox txtRecovered;
        private TextBox txtRecoveryRate;
        private DataGridView dgvPackaging;
        private DataGridView dgvReuseStats;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        // Designer sẽ chạy code này để vẽ giao diện
        private void InitializeComponent()
        {
            this.titleLabelHeader = new System.Windows.Forms.Label();
            this.tableLayoutStats = new System.Windows.Forms.TableLayoutPanel();
            this.titleLabelStats = new System.Windows.Forms.Label();
            this.lblDistributed = new System.Windows.Forms.Label();
            this.txtDistributed = new System.Windows.Forms.TextBox();
            this.lblRecovered = new System.Windows.Forms.Label();
            this.txtRecovered = new System.Windows.Forms.TextBox();
            this.lblRecoveryRate = new System.Windows.Forms.Label();
            this.txtRecoveryRate = new System.Windows.Forms.TextBox();
            this.titleLabelPackaging = new System.Windows.Forms.Label();
            this.titleLabelReuse = new System.Windows.Forms.Label();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.btnAddPackaging = new System.Windows.Forms.Button();
            this.statsPanel = new System.Windows.Forms.Panel();
            this.packagingStatusPanel = new System.Windows.Forms.Panel();
            this.dgvPackaging = new System.Windows.Forms.DataGridView();
            this.reuseStatsPanel = new System.Windows.Forms.Panel();
            this.dgvReuseStats = new System.Windows.Forms.DataGridView();
            this.tableLayoutStats.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.statsPanel.SuspendLayout();
            this.packagingStatusPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPackaging)).BeginInit();
            this.reuseStatsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReuseStats)).BeginInit();
            this.SuspendLayout();
            // 
            // titleLabelHeader
            // 
            this.titleLabelHeader.AutoSize = true;
            this.titleLabelHeader.Dock = System.Windows.Forms.DockStyle.Left;
            this.titleLabelHeader.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.titleLabelHeader.Location = new System.Drawing.Point(0, 0);
            this.titleLabelHeader.Name = "titleLabelHeader";
            this.titleLabelHeader.Size = new System.Drawing.Size(288, 35);
            this.titleLabelHeader.TabIndex = 0;
            this.titleLabelHeader.Text = "Bao bì & Vòng tuần hoàn";
            // 
            // tableLayoutStats
            // 
            this.tableLayoutStats.ColumnCount = 3;
            this.tableLayoutStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutStats.Controls.Add(this.titleLabelStats, 0, 0);
            this.tableLayoutStats.Controls.Add(this.lblDistributed, 0, 1);
            this.tableLayoutStats.Controls.Add(this.txtDistributed, 0, 2);
            this.tableLayoutStats.Controls.Add(this.lblRecovered, 1, 1);
            this.tableLayoutStats.Controls.Add(this.txtRecovered, 1, 2);
            this.tableLayoutStats.Controls.Add(this.lblRecoveryRate, 2, 1);
            this.tableLayoutStats.Controls.Add(this.txtRecoveryRate, 2, 2);
            this.tableLayoutStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutStats.Location = new System.Drawing.Point(15, 15);
            this.tableLayoutStats.Name = "tableLayoutStats";
            this.tableLayoutStats.RowCount = 3;
            this.tableLayoutStats.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutStats.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutStats.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutStats.Size = new System.Drawing.Size(870, 70);
            this.tableLayoutStats.TabIndex = 0;
            // 
            // titleLabelStats
            // 
            this.titleLabelStats.AutoSize = true;
            this.titleLabelStats.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.titleLabelStats.Location = new System.Drawing.Point(3, 0);
            this.titleLabelStats.Name = "titleLabelStats";
            this.titleLabelStats.Size = new System.Drawing.Size(141, 25);
            this.titleLabelStats.TabIndex = 0;
            this.titleLabelStats.Text = "Quản lý bao bì";
            // 
            // lblDistributed
            // 
            this.lblDistributed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDistributed.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDistributed.Location = new System.Drawing.Point(3, 25);
            this.lblDistributed.Name = "lblDistributed";
            this.lblDistributed.Size = new System.Drawing.Size(284, 30);
            this.lblDistributed.TabIndex = 1;
            this.lblDistributed.Text = "Số lượng phát ra";
            // 
            // txtDistributed
            // 
            this.txtDistributed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDistributed.Location = new System.Drawing.Point(3, 58);
            this.txtDistributed.Name = "txtDistributed";
            this.txtDistributed.ReadOnly = true;
            this.txtDistributed.Size = new System.Drawing.Size(284, 26);
            this.txtDistributed.TabIndex = 2;
            // 
            // lblRecovered
            // 
            this.lblRecovered.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRecovered.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblRecovered.Location = new System.Drawing.Point(293, 25);
            this.lblRecovered.Name = "lblRecovered";
            this.lblRecovered.Size = new System.Drawing.Size(284, 30);
            this.lblRecovered.TabIndex = 3;
            this.lblRecovered.Text = "Số lượng thu hồi";
            // 
            // txtRecovered
            // 
            this.txtRecovered.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRecovered.Location = new System.Drawing.Point(293, 58);
            this.txtRecovered.Name = "txtRecovered";
            this.txtRecovered.ReadOnly = true;
            this.txtRecovered.Size = new System.Drawing.Size(284, 26);
            this.txtRecovered.TabIndex = 4;
            // 
            // lblRecoveryRate
            // 
            this.lblRecoveryRate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRecoveryRate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblRecoveryRate.Location = new System.Drawing.Point(583, 25);
            this.lblRecoveryRate.Name = "lblRecoveryRate";
            this.lblRecoveryRate.Size = new System.Drawing.Size(284, 30);
            this.lblRecoveryRate.TabIndex = 5;
            this.lblRecoveryRate.Text = "Tỷ lệ thu hồi";
            // 
            // txtRecoveryRate
            // 
            this.txtRecoveryRate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRecoveryRate.Location = new System.Drawing.Point(583, 58);
            this.txtRecoveryRate.Name = "txtRecoveryRate";
            this.txtRecoveryRate.ReadOnly = true;
            this.txtRecoveryRate.Size = new System.Drawing.Size(284, 26);
            this.txtRecoveryRate.TabIndex = 6;
            // 
            // titleLabelPackaging
            // 
            this.titleLabelPackaging.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleLabelPackaging.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.titleLabelPackaging.Location = new System.Drawing.Point(15, 15);
            this.titleLabelPackaging.Name = "titleLabelPackaging";
            this.titleLabelPackaging.Size = new System.Drawing.Size(870, 30);
            this.titleLabelPackaging.TabIndex = 1;
            this.titleLabelPackaging.Text = "Tình trạng bao bì";
            // 
            // titleLabelReuse
            // 
            this.titleLabelReuse.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleLabelReuse.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.titleLabelReuse.Location = new System.Drawing.Point(15, 15);
            this.titleLabelReuse.Name = "titleLabelReuse";
            this.titleLabelReuse.Size = new System.Drawing.Size(870, 30);
            this.titleLabelReuse.TabIndex = 1;
            this.titleLabelReuse.Text = "Thống kê tái sử dụng";
            // 
            // headerPanel
            // 
            this.headerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.headerPanel.Controls.Add(this.titleLabelHeader);
            this.headerPanel.Controls.Add(this.btnAddPackaging);
            this.headerPanel.Location = new System.Drawing.Point(20, 20);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(900, 60);
            this.headerPanel.TabIndex = 0;
            // 
            // btnAddPackaging
            // 
            this.btnAddPackaging.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnAddPackaging.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAddPackaging.FlatAppearance.BorderSize = 0;
            this.btnAddPackaging.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddPackaging.ForeColor = System.Drawing.Color.White;
            this.btnAddPackaging.Location = new System.Drawing.Point(780, 0);
            this.btnAddPackaging.Name = "btnAddPackaging";
            this.btnAddPackaging.Size = new System.Drawing.Size(120, 60);
            this.btnAddPackaging.TabIndex = 1;
            this.btnAddPackaging.Text = "+ Thêm bao bì";
            this.btnAddPackaging.UseVisualStyleBackColor = false;
            // 
            // statsPanel
            // 
            this.statsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statsPanel.BackColor = System.Drawing.Color.White;
            this.statsPanel.Controls.Add(this.tableLayoutStats);
            this.statsPanel.Location = new System.Drawing.Point(20, 90);
            this.statsPanel.Name = "statsPanel";
            this.statsPanel.Padding = new System.Windows.Forms.Padding(15);
            this.statsPanel.Size = new System.Drawing.Size(900, 100);
            this.statsPanel.TabIndex = 1;
            // 
            // packagingStatusPanel
            // 
            this.packagingStatusPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.packagingStatusPanel.BackColor = System.Drawing.Color.White;
            this.packagingStatusPanel.Controls.Add(this.dgvPackaging);
            this.packagingStatusPanel.Controls.Add(this.titleLabelPackaging);
            this.packagingStatusPanel.Location = new System.Drawing.Point(20, 210);
            this.packagingStatusPanel.Name = "packagingStatusPanel";
            this.packagingStatusPanel.Padding = new System.Windows.Forms.Padding(15);
            this.packagingStatusPanel.Size = new System.Drawing.Size(900, 200);
            this.packagingStatusPanel.TabIndex = 2;
            // 
            // dgvPackaging
            // 
            this.dgvPackaging.ColumnHeadersHeight = 29;
            this.dgvPackaging.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPackaging.Location = new System.Drawing.Point(15, 45);
            this.dgvPackaging.Name = "dgvPackaging";
            this.dgvPackaging.RowHeadersWidth = 51;
            this.dgvPackaging.Size = new System.Drawing.Size(870, 140);
            this.dgvPackaging.TabIndex = 0;
            // 
            // reuseStatsPanel
            // 
            this.reuseStatsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reuseStatsPanel.BackColor = System.Drawing.Color.White;
            this.reuseStatsPanel.Controls.Add(this.dgvReuseStats);
            this.reuseStatsPanel.Controls.Add(this.titleLabelReuse);
            this.reuseStatsPanel.Location = new System.Drawing.Point(20, 430);
            this.reuseStatsPanel.Name = "reuseStatsPanel";
            this.reuseStatsPanel.Padding = new System.Windows.Forms.Padding(15);
            this.reuseStatsPanel.Size = new System.Drawing.Size(900, 180);
            this.reuseStatsPanel.TabIndex = 3;
            // 
            // dgvReuseStats
            // 
            this.dgvReuseStats.ColumnHeadersHeight = 29;
            this.dgvReuseStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReuseStats.Location = new System.Drawing.Point(15, 45);
            this.dgvReuseStats.Name = "dgvReuseStats";
            this.dgvReuseStats.RowHeadersWidth = 51;
            this.dgvReuseStats.Size = new System.Drawing.Size(870, 120);
            this.dgvReuseStats.TabIndex = 0;
            // 
            // PackagingControl
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.headerPanel);
            this.Controls.Add(this.statsPanel);
            this.Controls.Add(this.packagingStatusPanel);
            this.Controls.Add(this.reuseStatsPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PackagingControl";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(940, 640);
            this.tableLayoutStats.ResumeLayout(false);
            this.tableLayoutStats.PerformLayout();
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.statsPanel.ResumeLayout(false);
            this.packagingStatusPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPackaging)).EndInit();
            this.reuseStatsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReuseStats)).EndInit();
            this.ResumeLayout(false);

        }

        private Label titleLabelHeader;
        private TableLayoutPanel tableLayoutStats;
        private Label titleLabelStats;
        private Label lblDistributed;
        private Label lblRecovered;
        private Label lblRecoveryRate;
        private Label titleLabelPackaging;
        private Label titleLabelReuse;

        // HÀM HELPER SetupDataGridStyle ĐÃ BỊ XÓA (đã chuyển sang file .cs)
    }
}