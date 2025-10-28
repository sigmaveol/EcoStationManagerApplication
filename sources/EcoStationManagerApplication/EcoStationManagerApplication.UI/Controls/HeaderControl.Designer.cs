using Guna;

namespace EcoStationManagerApplication.UI.Controls
{
    partial class HeaderControl
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
            this.guna2PanelHeader = new Guna.UI2.WinForms.Guna2GradientPanel();
            this.guna2DateTimePicker1 = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.guna2PanelNofication = new Guna.UI2.WinForms.Guna2Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.guna2ButtonNofication = new Guna.UI2.WinForms.Guna2Button();
            this.guna2PanelTitleHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.labelTitleHeader = new System.Windows.Forms.Label();
            this.borderLeftHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.searchControl = new EcoStationManagerApplication.UI.Controls.SearchControl();
            this.guna2PanelHeader.SuspendLayout();
            this.guna2PanelNofication.SuspendLayout();
            this.guna2PanelTitleHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2PanelHeader
            // 
            this.guna2PanelHeader.Controls.Add(this.guna2DateTimePicker1);
            this.guna2PanelHeader.Controls.Add(this.guna2PanelNofication);
            this.guna2PanelHeader.Controls.Add(this.guna2PanelTitleHeader);
            this.guna2PanelHeader.Controls.Add(this.searchControl);
            this.guna2PanelHeader.CustomBorderColor = System.Drawing.Color.Silver;
            this.guna2PanelHeader.CustomBorderThickness = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.guna2PanelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2PanelHeader.FillColor = System.Drawing.Color.LightGray;
            this.guna2PanelHeader.FillColor2 = System.Drawing.Color.LightGray;
            this.guna2PanelHeader.Location = new System.Drawing.Point(0, 0);
            this.guna2PanelHeader.Name = "guna2PanelHeader";
            this.guna2PanelHeader.Size = new System.Drawing.Size(1169, 77);
            this.guna2PanelHeader.TabIndex = 0;
            // 
            // guna2DateTimePicker1
            // 
            this.guna2DateTimePicker1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.guna2DateTimePicker1.BackColor = System.Drawing.Color.Transparent;
            this.guna2DateTimePicker1.Checked = true;
            this.guna2DateTimePicker1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.guna2DateTimePicker1.Location = new System.Drawing.Point(477, 15);
            this.guna2DateTimePicker1.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.guna2DateTimePicker1.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.guna2DateTimePicker1.Name = "guna2DateTimePicker1";
            this.guna2DateTimePicker1.Size = new System.Drawing.Size(288, 40);
            this.guna2DateTimePicker1.TabIndex = 8;
            this.guna2DateTimePicker1.Value = new System.DateTime(2025, 10, 27, 20, 4, 9, 841);
            // 
            // guna2PanelNofication
            // 
            this.guna2PanelNofication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2PanelNofication.BackColor = System.Drawing.Color.Transparent;
            this.guna2PanelNofication.Controls.Add(this.label1);
            this.guna2PanelNofication.Controls.Add(this.guna2ButtonNofication);
            this.guna2PanelNofication.FillColor = System.Drawing.Color.Transparent;
            this.guna2PanelNofication.Location = new System.Drawing.Point(1095, 11);
            this.guna2PanelNofication.Name = "guna2PanelNofication";
            this.guna2PanelNofication.Size = new System.Drawing.Size(50, 50);
            this.guna2PanelNofication.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Red;
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(36, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "5";
            // 
            // guna2ButtonNofication
            // 
            this.guna2ButtonNofication.BackColor = System.Drawing.Color.Transparent;
            this.guna2ButtonNofication.BorderRadius = 25;
            this.guna2ButtonNofication.Cursor = System.Windows.Forms.Cursors.Hand;
            this.guna2ButtonNofication.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2ButtonNofication.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2ButtonNofication.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2ButtonNofication.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2ButtonNofication.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2ButtonNofication.FillColor = System.Drawing.Color.White;
            this.guna2ButtonNofication.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2ButtonNofication.ForeColor = System.Drawing.Color.White;
            this.guna2ButtonNofication.HoverState.FillColor = System.Drawing.Color.Gray;
            this.guna2ButtonNofication.Image = global::EcoStationManagerApplication.UI.Properties.Resources.bell_icon;
            this.guna2ButtonNofication.Location = new System.Drawing.Point(0, 0);
            this.guna2ButtonNofication.Name = "guna2ButtonNofication";
            this.guna2ButtonNofication.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.guna2ButtonNofication.Size = new System.Drawing.Size(50, 50);
            this.guna2ButtonNofication.TabIndex = 6;
            // 
            // guna2PanelTitleHeader
            // 
            this.guna2PanelTitleHeader.BackColor = System.Drawing.Color.Transparent;
            this.guna2PanelTitleHeader.BorderColor = System.Drawing.Color.Transparent;
            this.guna2PanelTitleHeader.Controls.Add(this.labelTitleHeader);
            this.guna2PanelTitleHeader.Controls.Add(this.borderLeftHeader);
            this.guna2PanelTitleHeader.FillColor = System.Drawing.Color.Transparent;
            this.guna2PanelTitleHeader.Location = new System.Drawing.Point(20, 11);
            this.guna2PanelTitleHeader.Name = "guna2PanelTitleHeader";
            this.guna2PanelTitleHeader.Size = new System.Drawing.Size(182, 48);
            this.guna2PanelTitleHeader.TabIndex = 5;
            // 
            // labelTitleHeader
            // 
            this.labelTitleHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitleHeader.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitleHeader.ForeColor = System.Drawing.Color.Blue;
            this.labelTitleHeader.Location = new System.Drawing.Point(8, 0);
            this.labelTitleHeader.Name = "labelTitleHeader";
            this.labelTitleHeader.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.labelTitleHeader.Size = new System.Drawing.Size(174, 48);
            this.labelTitleHeader.TabIndex = 1;
            this.labelTitleHeader.Text = "Dashboard";
            this.labelTitleHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // borderLeftHeader
            // 
            this.borderLeftHeader.BorderRadius = 3;
            this.borderLeftHeader.Dock = System.Windows.Forms.DockStyle.Left;
            this.borderLeftHeader.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.borderLeftHeader.Location = new System.Drawing.Point(0, 0);
            this.borderLeftHeader.Name = "borderLeftHeader";
            this.borderLeftHeader.Size = new System.Drawing.Size(8, 48);
            this.borderLeftHeader.TabIndex = 0;
            // 
            // searchControl
            // 
            this.searchControl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.searchControl.BackColor = System.Drawing.Color.Transparent;
            this.searchControl.Location = new System.Drawing.Point(824, 15);
            this.searchControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.searchControl.Name = "searchControl";
            this.searchControl.PlaceholderText = "Tìm kiếm ....";
            this.searchControl.SearchText = "";
            this.searchControl.Size = new System.Drawing.Size(231, 40);
            this.searchControl.TabIndex = 4;
            // 
            // HeaderControl
            // 
            this.Controls.Add(this.guna2PanelHeader);
            this.Name = "HeaderControl";
            this.Size = new System.Drawing.Size(1169, 77);
            this.guna2PanelHeader.ResumeLayout(false);
            this.guna2PanelNofication.ResumeLayout(false);
            this.guna2PanelNofication.PerformLayout();
            this.guna2PanelTitleHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2GradientPanel guna2PanelHeader;
        private SearchControl searchControl;
        private Guna.UI2.WinForms.Guna2Panel guna2PanelTitleHeader;
        private Guna.UI2.WinForms.Guna2Panel borderLeftHeader;
        private System.Windows.Forms.Label labelTitleHeader;
        private Guna.UI2.WinForms.Guna2Button guna2ButtonNofication;
        private Guna.UI2.WinForms.Guna2Panel guna2PanelNofication;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2DateTimePicker guna2DateTimePicker1;
    }
}
