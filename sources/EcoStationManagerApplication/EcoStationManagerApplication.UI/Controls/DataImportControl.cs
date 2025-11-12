using System;
using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class DataImportControl : UserControl
    {
        public DataImportControl()
        {
            InitializeComponent();
            InitializeEvents();
        }

        private void InitializeEvents()
        {
            // Gán sự kiện cho các nút
            if (btnSyncGoogleSheets != null)
                btnSyncGoogleSheets.Click += BtnSyncGoogleSheets_Click;

            if (btnImportFile != null)
                btnImportFile.Click += BtnImportFile_Click;

            if (btnAddManualOrder != null)
                btnAddManualOrder.Click += BtnAddManualOrder_Click;
        }

        private void BtnSyncGoogleSheets_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtGoogleSheetsUrl?.Text))
                {
                    MessageBox.Show("Vui lòng nhập URL Google Sheets", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show("Bắt đầu đồng bộ với Google Sheets...", "Đồng bộ dữ liệu");

                // TODO: Thêm logic đồng bộ Google Sheets
                SyncWithGoogleSheets(txtGoogleSheetsUrl.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đồng bộ: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnImportFile_Click(object sender, EventArgs e)
        {
            try
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls|CSV files (*.csv)|*.csv";
                openFileDialog.Title = "Chọn file để nhập dữ liệu";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show($"Đã chọn file: {openFileDialog.FileName}", "Nhập dữ liệu");

                    // TODO: Thêm logic xử lý file
                    ImportDataFromFile(openFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi nhập file: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAddManualOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtManualCustomer?.Text) ||
                    string.IsNullOrEmpty(txtManualPhone?.Text) ||
                    cmbManualProduct?.SelectedIndex <= 0)
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate phone number
                if (!IsValidPhoneNumber(txtManualPhone.Text))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate volume
                if (!decimal.TryParse(txtManualVolume.Text, out decimal volume) || volume <= 0)
                {
                    MessageBox.Show("Dung tích phải là số dương", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Đã thêm đơn hàng thủ công thành công!", "Thành công");

                // TODO: Thêm logic lưu đơn hàng
                SaveManualOrder(
                    txtManualCustomer.Text,
                    txtManualPhone.Text,
                    cmbManualProduct.SelectedItem.ToString(),
                    (int)numManualQuantity.Value,
                    volume
                );

                // Clear form after success
                ClearManualForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm đơn hàng: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Helper Methods
        private bool IsValidPhoneNumber(string phone)
        {
            // Simple phone validation
            return !string.IsNullOrEmpty(phone) && phone.Length >= 10 && phone.Length <= 15;
        }

        private void ClearManualForm()
        {
            if (txtManualCustomer != null) txtManualCustomer.Text = string.Empty;
            if (txtManualPhone != null) txtManualPhone.Text = string.Empty;
            if (cmbManualProduct != null) cmbManualProduct.SelectedIndex = 0;
            if (numManualQuantity != null) numManualQuantity.Value = 1;
            if (txtManualVolume != null) txtManualVolume.Text = string.Empty;
        }
        #endregion

        #region Service Methods
        private void SyncWithGoogleSheets(string url)
        {
            // TODO: Triển khai logic đồng bộ Google Sheets
            System.Threading.Thread.Sleep(2000); // Giả lập thời gian xử lý
            Console.WriteLine($"Synced with Google Sheets: {url}");
        }

        private void ImportDataFromFile(string filePath)
        {
            // TODO: Triển khai logic nhập dữ liệu từ file
            System.Threading.Thread.Sleep(1500); // Giả lập thời gian xử lý
            Console.WriteLine($"Imported data from: {filePath}");
        }

        private void SaveManualOrder(string customer, string phone, string product, int quantity, decimal volume)
        {
            // TODO: Triển khai logic lưu đơn hàng thủ công
            System.Threading.Thread.Sleep(1000); // Giả lập thời gian xử lý
            Console.WriteLine($"Saved manual order: {customer}, {product}, {quantity}");
        }
        #endregion

        #region Public Methods
        public void SetGoogleSheetsUrl(string url)
        {
            if (txtGoogleSheetsUrl != null && !string.IsNullOrEmpty(url))
            {
                txtGoogleSheetsUrl.Text = url;
            }
        }

        public string GetCurrentGoogleSheetsUrl()
        {
            return txtGoogleSheetsUrl?.Text ?? string.Empty;
        }

        public void TriggerAutoSync()
        {
            if (!string.IsNullOrEmpty(txtGoogleSheetsUrl?.Text))
            {
                SyncWithGoogleSheets(txtGoogleSheetsUrl.Text);
            }
        }
        #endregion
    }
}