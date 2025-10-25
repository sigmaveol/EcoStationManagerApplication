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
            this.SuspendLayout();

            // 
            // guna2PanelFooter
            // 
            this.guna2PanelFooter.Dock = System.Windows.Forms.DockStyle.Fill; // fill toàn bộ FooterControl
            this.guna2PanelFooter.FillColor = System.Drawing.Color.Black;
            this.guna2PanelFooter.Location = new System.Drawing.Point(0, 0);
            this.guna2PanelFooter.Name = "guna2PanelFooter";
            this.guna2PanelFooter.Size = new System.Drawing.Size(800, 91); // chiều cao mặc định
            this.guna2PanelFooter.TabIndex = 0;

            // 
            // FooterControl
            // 
            this.Controls.Add(this.guna2PanelFooter);
            this.Name = "FooterControl";
            this.Size = new System.Drawing.Size(800, 91);
            this.ResumeLayout(false);

        }

        #endregion

        private Guna2Panel guna2PanelFooter;
    }
}
