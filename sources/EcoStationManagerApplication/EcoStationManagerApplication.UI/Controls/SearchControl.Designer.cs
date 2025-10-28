namespace EcoStationManagerApplication.UI.Controls
{
    partial class SearchControl
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
            this.guna2TextBoxSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.guna2PanelContainer = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2PanelContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2TextBoxSearch
            // 
            this.guna2TextBoxSearch.BorderRadius = 2;
            this.guna2TextBoxSearch.BorderThickness = 0;
            this.guna2TextBoxSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.guna2TextBoxSearch.DefaultText = "";
            this.guna2TextBoxSearch.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.guna2TextBoxSearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.guna2TextBoxSearch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBoxSearch.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBoxSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2TextBoxSearch.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBoxSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2TextBoxSearch.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBoxSearch.IconLeft = global::EcoStationManagerApplication.UI.Properties.Resources.search_icon;
            this.guna2TextBoxSearch.IconLeftOffset = new System.Drawing.Point(5, 0);
            this.guna2TextBoxSearch.IconLeftSize = new System.Drawing.Size(30, 30);
            this.guna2TextBoxSearch.Location = new System.Drawing.Point(0, 0);
            this.guna2TextBoxSearch.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.guna2TextBoxSearch.Name = "guna2TextBoxSearch";
            this.guna2TextBoxSearch.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.guna2TextBoxSearch.PlaceholderText = "Tìm kiếm ....";
            this.guna2TextBoxSearch.SelectedText = "";
            this.guna2TextBoxSearch.Size = new System.Drawing.Size(277, 38);
            this.guna2TextBoxSearch.TabIndex = 0;
            this.guna2TextBoxSearch.TextChanged += new System.EventHandler(this.guna2TextBoxSearch_TextChanged);
            this.guna2TextBoxSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.guna2TextBoxSearch_KeyDown);
            // 
            // guna2PanelContainer
            // 
            this.guna2PanelContainer.BackColor = System.Drawing.Color.Transparent;
            this.guna2PanelContainer.Controls.Add(this.guna2TextBoxSearch);
            this.guna2PanelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2PanelContainer.Location = new System.Drawing.Point(0, 0);
            this.guna2PanelContainer.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.guna2PanelContainer.Name = "guna2PanelContainer";
            this.guna2PanelContainer.Size = new System.Drawing.Size(277, 38);
            this.guna2PanelContainer.TabIndex = 1;
            // 
            // SearchControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.guna2PanelContainer);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "SearchControl";
            this.Size = new System.Drawing.Size(277, 38);
            this.guna2PanelContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2TextBox guna2TextBoxSearch;
        private Guna.UI2.WinForms.Guna2Panel guna2PanelContainer;
    }
}
