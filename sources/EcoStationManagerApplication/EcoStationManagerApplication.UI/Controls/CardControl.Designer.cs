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
            this.panelCard = new Guna.UI2.WinForms.Guna2Panel();
            this.pictureIcon = new Guna.UI2.WinForms.Guna2PictureBox();
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelValue = new System.Windows.Forms.Label();
            this.labelSubInfo = new System.Windows.Forms.Label();
            this.labelChange = new System.Windows.Forms.Label();
            this.panelCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // panelCard
            // 
            this.panelCard.BorderRadius = 15;
            this.panelCard.Controls.Add(this.pictureIcon);
            this.panelCard.Controls.Add(this.labelTitle);
            this.panelCard.Controls.Add(this.labelValue);
            this.panelCard.Controls.Add(this.labelSubInfo);
            this.panelCard.Controls.Add(this.labelChange);
            this.panelCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCard.FillColor = System.Drawing.Color.White;
            this.panelCard.Location = new System.Drawing.Point(0, 0);
            this.panelCard.Name = "panelCard";
            this.panelCard.Padding = new System.Windows.Forms.Padding(10);
            this.panelCard.Size = new System.Drawing.Size(300, 195);
            this.panelCard.TabIndex = 0;
            // 
            // pictureIcon
            // 
            this.pictureIcon.BackColor = System.Drawing.SystemColors.Control;
            this.pictureIcon.FillColor = System.Drawing.Color.Black;
            this.pictureIcon.ImageRotate = 0F;
            this.pictureIcon.Location = new System.Drawing.Point(13, 66);
            this.pictureIcon.Name = "pictureIcon";
            this.pictureIcon.Size = new System.Drawing.Size(55, 52);
            this.pictureIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureIcon.TabIndex = 0;
            this.pictureIcon.TabStop = false;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.labelTitle.ForeColor = System.Drawing.Color.Gray;
            this.labelTitle.Location = new System.Drawing.Point(13, 21);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(164, 23);
            this.labelTitle.TabIndex = 1;
            this.labelTitle.Text = "Doanh thu hôm nay";
            // 
            // labelValue
            // 
            this.labelValue.AutoSize = true;
            this.labelValue.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelValue.ForeColor = System.Drawing.Color.Black;
            this.labelValue.Location = new System.Drawing.Point(98, 77);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(189, 41);
            this.labelValue.TabIndex = 2;
            this.labelValue.Text = "12,000,000đ";
            // 
            // labelSubInfo
            // 
            this.labelSubInfo.AutoSize = true;
            this.labelSubInfo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelSubInfo.ForeColor = System.Drawing.Color.Gray;
            this.labelSubInfo.Location = new System.Drawing.Point(66, 140);
            this.labelSubInfo.Name = "labelSubInfo";
            this.labelSubInfo.Size = new System.Drawing.Size(92, 20);
            this.labelSubInfo.TabIndex = 3;
            this.labelSubInfo.Text = "23 đơn hàng";
            // 
            // labelChange
            // 
            this.labelChange.AutoSize = true;
            this.labelChange.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelChange.ForeColor = System.Drawing.Color.Green;
            this.labelChange.Location = new System.Drawing.Point(215, 13);
            this.labelChange.Name = "labelChange";
            this.labelChange.Size = new System.Drawing.Size(71, 31);
            this.labelChange.TabIndex = 4;
            this.labelChange.Text = "+12%";
            // 
            // CardControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelCard);
            this.Name = "CardControl";
            this.Size = new System.Drawing.Size(300, 195);
            this.panelCard.ResumeLayout(false);
            this.panelCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Guna.UI2.WinForms.Guna2Panel panelCard;
        private Guna.UI2.WinForms.Guna2PictureBox pictureIcon;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelValue;
        private System.Windows.Forms.Label labelSubInfo;
        private System.Windows.Forms.Label labelChange;
    }
}
