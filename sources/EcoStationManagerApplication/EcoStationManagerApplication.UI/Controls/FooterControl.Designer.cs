using EcoStationManagerApplication.UI.Common;
using Guna.UI2.WinForms;

namespace EcoStationManagerApplication.UI.Controls
{
    partial class FooterControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.guna2PanelFooter = new Guna.UI2.WinForms.Guna2Panel();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lblAppName = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblSupportInfo = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblCopyright = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblVersion = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2Separator1 = new Guna.UI2.WinForms.Guna2Separator();
            this.guna2PanelFooter.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2PanelFooter
            // 
            this.guna2PanelFooter.Controls.Add(this.tableLayoutPanel);
            this.guna2PanelFooter.Controls.Add(this.guna2Separator1);
            this.guna2PanelFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2PanelFooter.FillColor = System.Drawing.SystemColors.Control;
            this.guna2PanelFooter.Location = new System.Drawing.Point(0, 0);
            this.guna2PanelFooter.Name = "guna2PanelFooter";
            this.guna2PanelFooter.Size = new System.Drawing.Size(800, 61);
            this.guna2PanelFooter.TabIndex = 0;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.lblAppName, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.lblSupportInfo, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.lblCopyright, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.lblVersion, 1, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 1);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(800, 60);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // lblAppName
            // 
            this.lblAppName.BackColor = System.Drawing.Color.Transparent;
            this.lblAppName.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblAppName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(107)))), ((int)(((byte)(59)))));
            this.lblAppName.Location = new System.Drawing.Point(3, 3);
            this.lblAppName.Name = "lblAppName";
            this.lblAppName.Size = new System.Drawing.Size(164, 25);
            this.lblAppName.TabIndex = 0;
            this.lblAppName.Text = "EcoStation Manager";
            this.lblAppName.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSupportInfo
            // 
            this.lblSupportInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblSupportInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSupportInfo.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSupportInfo.ForeColor = System.Drawing.Color.Gray;
            this.lblSupportInfo.Location = new System.Drawing.Point(3, 33);
            this.lblSupportInfo.Name = "lblSupportInfo";
            this.lblSupportInfo.Size = new System.Drawing.Size(253, 19);
            this.lblSupportInfo.TabIndex = 3;
            this.lblSupportInfo.Text = "Hỗ trợ kỹ thuật: hungminhtobe@gmail.com";
            this.lblSupportInfo.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCopyright
            // 
            this.lblCopyright.BackColor = System.Drawing.Color.Transparent;
            this.lblCopyright.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblCopyright.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCopyright.ForeColor = System.Drawing.Color.DimGray;
            this.lblCopyright.Location = new System.Drawing.Point(503, 3);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(294, 22);
            this.lblCopyright.TabIndex = 4;
            this.lblCopyright.Text = "© 2025 Green Core Tech. All rights reserved.";
            this.lblCopyright.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVersion
            // 
            this.lblVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblVersion.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.ForeColor = System.Drawing.Color.Gray;
            this.lblVersion.Location = new System.Drawing.Point(595, 33);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(202, 19);
            this.lblVersion.TabIndex = 5;
            this.lblVersion.Text = "Phiên bản: 1.0.0 | Build: 2025.11.27";
            this.lblVersion.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // guna2Separator1
            // 
            this.guna2Separator1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Separator1.FillColor = System.Drawing.Color.Silver;
            this.guna2Separator1.Location = new System.Drawing.Point(0, 0);
            this.guna2Separator1.Name = "guna2Separator1";
            this.guna2Separator1.Size = new System.Drawing.Size(800, 1);
            this.guna2Separator1.TabIndex = 0;
            // 
            // FooterControl
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.guna2PanelFooter);
            this.Name = "FooterControl";
            this.Size = new System.Drawing.Size(800, 61);
            this.guna2PanelFooter.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna2Panel guna2PanelFooter;
        private Guna2Separator guna2Separator1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private Guna2HtmlLabel lblAppName;
        private Guna2HtmlLabel lblSupportInfo;
        private Guna2HtmlLabel lblVersion;
        private Guna2HtmlLabel lblCopyright;
    }
}