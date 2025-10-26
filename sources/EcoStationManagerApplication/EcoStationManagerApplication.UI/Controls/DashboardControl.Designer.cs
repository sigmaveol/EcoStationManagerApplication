using Guna.UI2.WinForms;

namespace EcoStationManagerApplication.UI.Controls
{
    partial class DashboardControl
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
            this.guna2PanelContent = new Guna.UI2.WinForms.Guna2Panel();
            this.cardControl1 = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.cardControl2 = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.cardControl3 = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.cardControl4 = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.guna2PanelContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2PanelContent
            // 
            this.guna2PanelContent.Controls.Add(this.cardControl4);
            this.guna2PanelContent.Controls.Add(this.cardControl3);
            this.guna2PanelContent.Controls.Add(this.cardControl2);
            this.guna2PanelContent.Controls.Add(this.cardControl1);
            this.guna2PanelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2PanelContent.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.guna2PanelContent.Location = new System.Drawing.Point(0, 0);
            this.guna2PanelContent.Name = "guna2PanelContent";
            this.guna2PanelContent.Size = new System.Drawing.Size(800, 712);
            this.guna2PanelContent.TabIndex = 8;
            // 
            // cardControl1
            // 
            this.cardControl1.Location = new System.Drawing.Point(58, 69);
            this.cardControl1.Name = "cardControl1";
            this.cardControl1.Size = new System.Drawing.Size(307, 192);
            this.cardControl1.TabIndex = 0;
            // 
            // cardControl2
            // 
            this.cardControl2.Location = new System.Drawing.Point(421, 69);
            this.cardControl2.Name = "cardControl2";
            this.cardControl2.Size = new System.Drawing.Size(307, 192);
            this.cardControl2.TabIndex = 0;
            // 
            // cardControl3
            // 
            this.cardControl3.Location = new System.Drawing.Point(58, 301);
            this.cardControl3.Name = "cardControl3";
            this.cardControl3.Size = new System.Drawing.Size(307, 192);
            this.cardControl3.TabIndex = 0;
            // 
            // cardControl4
            // 
            this.cardControl4.Location = new System.Drawing.Point(421, 301);
            this.cardControl4.Name = "cardControl4";
            this.cardControl4.Size = new System.Drawing.Size(307, 192);
            this.cardControl4.TabIndex = 0;
            // 
            // DashboardControl
            // 
            this.Controls.Add(this.guna2PanelContent);
            this.Name = "DashboardControl";
            this.Size = new System.Drawing.Size(800, 712);
            this.guna2PanelContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Guna.UI2.WinForms.Guna2Panel guna2PanelContent;
        private CardControl cardControl1;
        private CardControl cardControl4;
        private CardControl cardControl3;
        private CardControl cardControl2;
    }
}
