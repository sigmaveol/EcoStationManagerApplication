using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    partial class DataImportControl
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
            this.panelGoogleSheets = new System.Windows.Forms.Panel();
            this.panelFileImport = new System.Windows.Forms.Panel();
            this.panelManualImport = new System.Windows.Forms.Panel();
            this.txtGoogleSheetsUrl = new System.Windows.Forms.TextBox();
            this.cmbSyncFrequency = new System.Windows.Forms.ComboBox();
            this.btnSyncGoogleSheets = new System.Windows.Forms.Button();
            this.btnImportFile = new System.Windows.Forms.Button();
            this.txtManualCustomer = new System.Windows.Forms.TextBox();
            this.txtManualPhone = new System.Windows.Forms.TextBox();
            this.cmbManualProduct = new System.Windows.Forms.ComboBox();
            this.numManualQuantity = new System.Windows.Forms.NumericUpDown();
            this.txtManualVolume = new System.Windows.Forms.TextBox();
            this.btnAddManualOrder = new System.Windows.Forms.Button();

            // Khai báo các biến tạm thời cho labels
            System.Windows.Forms.Label titleLabel_GS;
            System.Windows.Forms.Label descLabel_GS;
            System.Windows.Forms.Label lblUrl;
            System.Windows.Forms.Label lblFrequency;
            System.Windows.Forms.Label lblLastSync;
            System.Windows.Forms.TextBox txtLastSync;
            System.Windows.Forms.Label titleLabel_File;
            System.Windows.Forms.Label descLabel_File;
            System.Windows.Forms.Label titleLabel_Manual;
            System.Windows.Forms.Label descLabel_Manual;
            System.Windows.Forms.Label lblCustomer;
            System.Windows.Forms.Label lblPhone;
            System.Windows.Forms.Label lblProduct;
            System.Windows.Forms.Label lblQuantity;
            System.Windows.Forms.Label lblVolume;

            titleLabel_GS = new System.Windows.Forms.Label();
            descLabel_GS = new System.Windows.Forms.Label();
            lblUrl = new System.Windows.Forms.Label();
            lblFrequency = new System.Windows.Forms.Label();
            lblLastSync = new System.Windows.Forms.Label();
            txtLastSync = new System.Windows.Forms.TextBox();
            titleLabel_File = new System.Windows.Forms.Label();
            descLabel_File = new System.Windows.Forms.Label();
            titleLabel_Manual = new System.Windows.Forms.Label();
            descLabel_Manual = new System.Windows.Forms.Label();
            lblCustomer = new System.Windows.Forms.Label();
            lblPhone = new System.Windows.Forms.Label();
            lblProduct = new System.Windows.Forms.Label();
            lblQuantity = new System.Windows.Forms.Label();
            lblVolume = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.numManualQuantity)).BeginInit();
            this.SuspendLayout();

            // 
            // panelGoogleSheets
            // 
            this.panelGoogleSheets.Size = new System.Drawing.Size(900, 200);
            this.panelGoogleSheets.Location = new System.Drawing.Point(20, 20);
            this.panelGoogleSheets.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelGoogleSheets.Padding = new System.Windows.Forms.Padding(15);
            this.panelGoogleSheets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelGoogleSheets.Controls.Add(titleLabel_GS);
            this.panelGoogleSheets.Controls.Add(descLabel_GS);
            this.panelGoogleSheets.Controls.Add(lblUrl);
            this.panelGoogleSheets.Controls.Add(this.txtGoogleSheetsUrl);
            this.panelGoogleSheets.Controls.Add(lblFrequency);
            this.panelGoogleSheets.Controls.Add(this.cmbSyncFrequency);
            this.panelGoogleSheets.Controls.Add(lblLastSync);
            this.panelGoogleSheets.Controls.Add(txtLastSync);
            this.panelGoogleSheets.Controls.Add(this.btnSyncGoogleSheets);
            this.panelGoogleSheets.Name = "panelGoogleSheets";
            // 
            // titleLabel_GS
            // 
            titleLabel_GS.Text = "Đồng bộ dữ liệu từ Google Sheets/Drive";
            titleLabel_GS.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            titleLabel_GS.AutoSize = true;
            titleLabel_GS.Location = new System.Drawing.Point(15, 15);
            // 
            // descLabel_GS
            // 
            descLabel_GS.Text = "Kết nối với Google Sheets để tự động đồng bộ dữ liệu định kỳ";
            descLabel_GS.Font = new System.Drawing.Font("Segoe UI", 9F);
            descLabel_GS.AutoSize = true;
            descLabel_GS.Location = new System.Drawing.Point(15, 40);
            // 
            // lblUrl
            // 
            lblUrl.Text = "URL Google Sheets";
            lblUrl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblUrl.AutoSize = true;
            lblUrl.Location = new System.Drawing.Point(15, 75);
            // 
            // txtGoogleSheetsUrl
            // 
            this.txtGoogleSheetsUrl.Size = new System.Drawing.Size(500, 25);
            this.txtGoogleSheetsUrl.Location = new System.Drawing.Point(15, 100);
            this.txtGoogleSheetsUrl.Name = "txtGoogleSheetsUrl";
            // 
            // lblFrequency
            // 
            lblFrequency.Text = "Tần suất đồng bộ";
            lblFrequency.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblFrequency.AutoSize = true;
            lblFrequency.Location = new System.Drawing.Point(15, 135);
            // 
            // cmbSyncFrequency
            // 
            this.cmbSyncFrequency.Size = new System.Drawing.Size(200, 25);
            this.cmbSyncFrequency.Location = new System.Drawing.Point(15, 160);
            this.cmbSyncFrequency.Items.AddRange(new object[] { "Thủ công", "Hàng giờ", "Hàng ngày", "Hàng tuần" });
            this.cmbSyncFrequency.SelectedIndex = 0;
            this.cmbSyncFrequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSyncFrequency.Name = "cmbSyncFrequency";
            // 
            // lblLastSync
            // 
            lblLastSync.Text = "Lần đồng bộ cuối";
            lblLastSync.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblLastSync.AutoSize = true;
            lblLastSync.Location = new System.Drawing.Point(250, 135);
            // 
            // txtLastSync
            // 
            txtLastSync.Size = new System.Drawing.Size(200, 25);
            txtLastSync.Location = new System.Drawing.Point(250, 160);
            txtLastSync.Text = "Chưa đồng bộ";
            txtLastSync.ReadOnly = true;
            // 
            // btnSyncGoogleSheets
            // 
            this.btnSyncGoogleSheets.Text = "Đồng bộ ngay";
            this.btnSyncGoogleSheets.Size = new System.Drawing.Size(120, 35);
            this.btnSyncGoogleSheets.Location = new System.Drawing.Point(500, 155);
            this.btnSyncGoogleSheets.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnSyncGoogleSheets.ForeColor = System.Drawing.Color.White;
            this.btnSyncGoogleSheets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSyncGoogleSheets.FlatAppearance.BorderSize = 0;
            this.btnSyncGoogleSheets.Name = "btnSyncGoogleSheets";
            // 
            // panelFileImport
            // 
            this.panelFileImport.Size = new System.Drawing.Size(900, 150);
            this.panelFileImport.Location = new System.Drawing.Point(20, 240);
            this.panelFileImport.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFileImport.Padding = new System.Windows.Forms.Padding(15);
            this.panelFileImport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFileImport.Controls.Add(titleLabel_File);
            this.panelFileImport.Controls.Add(descLabel_File);
            this.panelFileImport.Controls.Add(this.btnImportFile);
            this.panelFileImport.Name = "panelFileImport";
            // 
            // titleLabel_File
            // 
            titleLabel_File.Text = "Nhập từ file Excel/CSV";
            titleLabel_File.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            titleLabel_File.AutoSize = true;
            titleLabel_File.Location = new System.Drawing.Point(15, 15);
            // 
            // descLabel_File
            // 
            descLabel_File.Text = "Tải lên file Excel hoặc CSV để nhập dữ liệu đơn hàng";
            descLabel_File.Font = new System.Drawing.Font("Segoe UI", 9F);
            descLabel_File.AutoSize = true;
            descLabel_File.Location = new System.Drawing.Point(15, 40);
            // 
            // btnImportFile
            // 
            this.btnImportFile.Text = "Chọn file và Nhập dữ liệu";
            this.btnImportFile.Size = new System.Drawing.Size(180, 35);
            this.btnImportFile.Location = new System.Drawing.Point(15, 75);
            this.btnImportFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnImportFile.ForeColor = System.Drawing.Color.White;
            this.btnImportFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportFile.FlatAppearance.BorderSize = 0;
            this.btnImportFile.Name = "btnImportFile";
            // 
            // panelManualImport
            // 
            this.panelManualImport.Size = new System.Drawing.Size(900, 300);
            this.panelManualImport.Location = new System.Drawing.Point(20, 410);
            this.panelManualImport.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelManualImport.Padding = new System.Windows.Forms.Padding(15);
            this.panelManualImport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelManualImport.Controls.Add(titleLabel_Manual);
            this.panelManualImport.Controls.Add(descLabel_Manual);
            this.panelManualImport.Controls.Add(lblCustomer);
            this.panelManualImport.Controls.Add(this.txtManualCustomer);
            this.panelManualImport.Controls.Add(lblPhone);
            this.panelManualImport.Controls.Add(this.txtManualPhone);
            this.panelManualImport.Controls.Add(lblProduct);
            this.panelManualImport.Controls.Add(this.cmbManualProduct);
            this.panelManualImport.Controls.Add(lblQuantity);
            this.panelManualImport.Controls.Add(this.numManualQuantity);
            this.panelManualImport.Controls.Add(lblVolume);
            this.panelManualImport.Controls.Add(this.txtManualVolume);
            this.panelManualImport.Controls.Add(this.btnAddManualOrder);
            this.panelManualImport.Name = "panelManualImport";
            // 
            // titleLabel_Manual
            // 
            titleLabel_Manual.Text = "Nhập thủ công";
            titleLabel_Manual.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            titleLabel_Manual.AutoSize = true;
            titleLabel_Manual.Location = new System.Drawing.Point(15, 15);
            // 
            // descLabel_Manual
            // 
            descLabel_Manual.Text = "Thêm đơn hàng mới thủ công";
            descLabel_Manual.Font = new System.Drawing.Font("Segoe UI", 9F);
            descLabel_Manual.AutoSize = true;
            descLabel_Manual.Location = new System.Drawing.Point(15, 40);
            // 
            // lblCustomer
            // 
            lblCustomer.Text = "Tên khách hàng";
            lblCustomer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblCustomer.AutoSize = true;
            lblCustomer.Location = new System.Drawing.Point(15, 75);
            // 
            // txtManualCustomer
            // 
            this.txtManualCustomer.Size = new System.Drawing.Size(300, 25);
            this.txtManualCustomer.Location = new System.Drawing.Point(15, 100);
            this.txtManualCustomer.Name = "txtManualCustomer";
            // 
            // lblPhone
            // 
            lblPhone.Text = "Số điện thoại";
            lblPhone.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblPhone.AutoSize = true;
            lblPhone.Location = new System.Drawing.Point(330, 75);
            // 
            // txtManualPhone
            // 
            this.txtManualPhone.Size = new System.Drawing.Size(300, 25);
            this.txtManualPhone.Location = new System.Drawing.Point(330, 100);
            this.txtManualPhone.Name = "txtManualPhone";
            // 
            // lblProduct
            // 
            lblProduct.Text = "Sản phẩm";
            lblProduct.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblProduct.AutoSize = true;
            lblProduct.Location = new System.Drawing.Point(15, 135);
            // 
            // cmbManualProduct
            // 
            this.cmbManualProduct.Size = new System.Drawing.Size(300, 25);
            this.cmbManualProduct.Location = new System.Drawing.Point(15, 160);
            this.cmbManualProduct.Items.AddRange(new object[] {
            "Chọn sản phẩm",
            "Dầu gội thiên nhiên 500ml",
            "Sữa tắm thảo dược 500ml",
            "Nước rửa chén 1L"});
            this.cmbManualProduct.SelectedIndex = 0;
            this.cmbManualProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbManualProduct.Name = "cmbManualProduct";
            // 
            // lblQuantity
            // 
            lblQuantity.Text = "Số lượng";
            lblQuantity.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblQuantity.AutoSize = true;
            lblQuantity.Location = new System.Drawing.Point(15, 195);
            // 
            // numManualQuantity
            // 
            this.numManualQuantity.Size = new System.Drawing.Size(100, 25);
            this.numManualQuantity.Location = new System.Drawing.Point(15, 220);
            this.numManualQuantity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numManualQuantity.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numManualQuantity.Name = "numManualQuantity";
            // 
            // lblVolume
            // 
            lblVolume.Text = "Dung tích (L)";
            lblVolume.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblVolume.AutoSize = true;
            lblVolume.Location = new System.Drawing.Point(130, 195);
            // 
            // txtManualVolume
            // 
            this.txtManualVolume.Size = new System.Drawing.Size(100, 25);
            this.txtManualVolume.Location = new System.Drawing.Point(130, 220);
            this.txtManualVolume.Name = "txtManualVolume";
            // 
            // btnAddManualOrder
            // 
            this.btnAddManualOrder.Text = "Thêm đơn hàng";
            this.btnAddManualOrder.Size = new System.Drawing.Size(120, 35);
            this.btnAddManualOrder.Location = new System.Drawing.Point(15, 260);
            this.btnAddManualOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnAddManualOrder.ForeColor = System.Drawing.Color.White;
            this.btnAddManualOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddManualOrder.FlatAppearance.BorderSize = 0;
            this.btnAddManualOrder.Name = "btnAddManualOrder";
            // 
            // DataImportControl
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Size = new System.Drawing.Size(940, 750);
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Controls.Add(this.panelGoogleSheets);
            this.Controls.Add(this.panelFileImport);
            this.Controls.Add(this.panelManualImport);
            this.Name = "DataImportControl";
            ((System.ComponentModel.ISupportInitialize)(this.numManualQuantity)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panelGoogleSheets;
        private Panel panelFileImport;
        private Panel panelManualImport;
        private TextBox txtGoogleSheetsUrl;
        private ComboBox cmbSyncFrequency;
        private Button btnSyncGoogleSheets;
        private Button btnImportFile;
        private TextBox txtManualCustomer;
        private TextBox txtManualPhone;
        private ComboBox cmbManualProduct;
        private NumericUpDown numManualQuantity;
        private TextBox txtManualVolume;
        private Button btnAddManualOrder;
    }
}