using Guna.UI2.WinForms;

namespace EcoStationManagerApplication.UI.Controls
{
    partial class CardControl
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
            this.labelChange = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.labelSubInfo = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.labelValue = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.labelTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.pictureIcon = new Guna.UI2.WinForms.Guna2PictureBox();
            this.panelCard = new Guna.UI2.WinForms.Guna2Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureIcon)).BeginInit();
            this.panelCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelChange
            // 
            this.labelChange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.labelChange.BackColor = System.Drawing.Color.Transparent;
            this.labelChange.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelChange.ForeColor = System.Drawing.Color.Green;
            this.labelChange.Location = new System.Drawing.Point(205, 19);
            this.labelChange.Name = "labelChange";
            this.labelChange.Size = new System.Drawing.Size(3, 2);
            this.labelChange.TabIndex = 4;
            this.labelChange.Text = null;
            // 
            // labelSubInfo
            // 
            this.labelSubInfo.BackColor = System.Drawing.Color.Transparent;
            this.labelSubInfo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelSubInfo.ForeColor = System.Drawing.Color.Gray;
            this.labelSubInfo.Location = new System.Drawing.Point(14, 169);
            this.labelSubInfo.Name = "labelSubInfo";
            this.labelSubInfo.Size = new System.Drawing.Size(3, 2);
            this.labelSubInfo.TabIndex = 3;
            this.labelSubInfo.Text = null;
            // 
            // labelValue
            // 
            this.labelValue.BackColor = System.Drawing.Color.Transparent;
            this.labelValue.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelValue.ForeColor = System.Drawing.Color.Black;
            this.labelValue.Location = new System.Drawing.Point(14, 111);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(3, 2);
            this.labelValue.TabIndex = 2;
            this.labelValue.Text = null;
            // 
            // labelTitle
            // 
            this.labelTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.ForeColor = System.Drawing.Color.Gray;
            this.labelTitle.Location = new System.Drawing.Point(14, 78);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(3, 2);
            this.labelTitle.TabIndex = 1;
            this.labelTitle.Text = null;
            // 
            // pictureIcon
            // 
            this.pictureIcon.BackColor = System.Drawing.Color.Transparent;
            this.pictureIcon.FillColor = System.Drawing.Color.Transparent;
            this.pictureIcon.ImageRotate = 0F;
            this.pictureIcon.Location = new System.Drawing.Point(14, 12);
            this.pictureIcon.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureIcon.Name = "pictureIcon";
            this.pictureIcon.Size = new System.Drawing.Size(40, 40);
            this.pictureIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureIcon.TabIndex = 0;
            this.pictureIcon.TabStop = false;
            // 
            // panelCard
            // 
            this.panelCard.AutoScroll = true;
            this.panelCard.AutoSize = true;
            this.panelCard.BackColor = System.Drawing.Color.Transparent;
            this.panelCard.BorderColor = System.Drawing.Color.Transparent;
            this.panelCard.BorderRadius = 15;
            this.panelCard.Controls.Add(this.pictureIcon);
            this.panelCard.Controls.Add(this.labelTitle);
            this.panelCard.Controls.Add(this.labelValue);
            this.panelCard.Controls.Add(this.labelSubInfo);
            this.panelCard.Controls.Add(this.labelChange);
            this.panelCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCard.FillColor = System.Drawing.Color.White;
            this.panelCard.Location = new System.Drawing.Point(10, 10);
            this.panelCard.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelCard.Name = "panelCard";
            this.panelCard.Padding = new System.Windows.Forms.Padding(11, 10, 11, 10);
            this.panelCard.Size = new System.Drawing.Size(264, 230);
            this.panelCard.TabIndex = 0;
            // 
            // CardControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panelCard);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.MaximumSize = new System.Drawing.Size(300, 250);
            this.Name = "CardControl";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(284, 250);
            ((System.ComponentModel.ISupportInitialize)(this.pictureIcon)).EndInit();
            this.panelCard.ResumeLayout(false);
            this.panelCard.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna2HtmlLabel labelChange;
        private Guna2HtmlLabel labelSubInfo;
        private Guna2HtmlLabel labelValue;
        private Guna2HtmlLabel labelTitle;
        private Guna.UI2.WinForms.Guna2PictureBox pictureIcon;
        private Guna.UI2.WinForms.Guna2Panel panelCard;
    }
}
