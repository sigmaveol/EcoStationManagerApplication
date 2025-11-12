using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    partial class SettingsControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Khai báo tất cả controls
        private Panel headerPanel;
        private FlowLayoutPanel settingsSection;
        private Panel systemInfoPanel;
        private TextBox txtBusinessName;
        private TextBox txtAddress;
        private TextBox txtPhone;
        private TextBox txtEmail;
        private CheckBox chkNewOrderNotification;
        private CheckBox chkLowStockAlert;
        private CheckBox chkPackagingReturn;
        private CheckBox chkDailyReport;
        private NumericUpDown numStockAlertLevel;
        private ComboBox cmbDefaultUnit;
        private NumericUpDown numMaxReuse;
        private NumericUpDown numCleaningCycle;
        private NumericUpDown numTargetRecoveryRate;
        private Button btnSaveBusinessInfo;
        private Button btnSaveNotificationSettings;
        private Button btnSaveInventorySettings;
        private Button btnSavePackagingSettings;
        private Button btnExportSystemLog;
        private Button btnResetSettings;

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

            // Dispose tất cả controls
            headerPanel?.Dispose();
            settingsSection?.Dispose();
            systemInfoPanel?.Dispose();
            txtBusinessName?.Dispose();
            txtAddress?.Dispose();
            txtPhone?.Dispose();
            txtEmail?.Dispose();
            chkNewOrderNotification?.Dispose();
            chkLowStockAlert?.Dispose();
            chkPackagingReturn?.Dispose();
            chkDailyReport?.Dispose();
            numStockAlertLevel?.Dispose();
            cmbDefaultUnit?.Dispose();
            numMaxReuse?.Dispose();
            numCleaningCycle?.Dispose();
            numTargetRecoveryRate?.Dispose();
            btnSaveBusinessInfo?.Dispose();
            btnSaveNotificationSettings?.Dispose();
            btnSaveInventorySettings?.Dispose();
            btnSavePackagingSettings?.Dispose();
            btnExportSystemLog?.Dispose();
            btnResetSettings?.Dispose();

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SuspendLayout();

            // 
            // SettingsControl
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Size = new System.Drawing.Size(940, 920);
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Name = "SettingsControl";

            this.ResumeLayout(false);
        }

        #endregion
    }
}