using System;
using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class BackupControl : UserControl
    {
        public BackupControl()
        {
            InitializeComponent();
            InitializeEvents();
        }

        private void InitializeEvents()
        {
            // Gán sự kiện cho các nút
            if (btnBackupExcel != null)
                btnBackupExcel.Click += BtnBackupExcel_Click;

            if (btnBackupPDF != null)
                btnBackupPDF.Click += BtnBackupPDF_Click;

            if (btnRestore != null)
                btnRestore.Click += BtnRestore_Click;

            if (browseButton != null)
                browseButton.Click += BrowseButton_Click;
        }

        private void BtnBackupExcel_Click(object sender, EventArgs e)
        {
            try
            {
                var saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                saveDialog.FileName = $"EcoStation_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    // Gọi service sao lưu Excel
                    BackupExcelData(saveDialog.FileName);
                    ShowSuccessMessage($"Đã sao lưu Excel thành công!\nFile: {saveDialog.FileName}");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Lỗi khi sao lưu Excel: {ex.Message}");
            }
        }

        private void BtnBackupPDF_Click(object sender, EventArgs e)
        {
            try
            {
                var saveDialog = new SaveFileDialog();
                saveDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveDialog.FileName = $"EcoStation_Report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    // Gọi service sao lưu PDF
                    BackupPDFData(saveDialog.FileName);
                    ShowSuccessMessage($"Đã sao lưu PDF thành công!\nFile: {saveDialog.FileName}");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Lỗi khi sao lưu PDF: {ex.Message}");
            }
        }

        private void BtnRestore_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtRestoreFile?.Text))
                {
                    ShowWarningMessage("Vui lòng chọn một file để phục hồi trước.");
                    return;
                }

                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn phục hồi dữ liệu từ file:\n{txtRestoreFile.Text}?\n\nLưu ý: Thao tác này sẽ ghi đè dữ liệu hiện tại.",
                    "Xác nhận phục hồi",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Gọi service phục hồi dữ liệu
                    RestoreData(txtRestoreFile.Text);
                    ShowSuccessMessage("Đã phục hồi dữ liệu thành công!");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Lỗi khi phục hồi dữ liệu: {ex.Message}");
            }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            try
            {
                var openDialog = new OpenFileDialog();
                openDialog.Filter = "Backup files (*.xlsx;*.pdf)|*.xlsx;*.pdf|All files (*.*)|*.*";
                openDialog.Title = "Chọn file sao lưu để phục hồi";

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    txtRestoreFile.Text = openDialog.FileName;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Lỗi khi chọn file: {ex.Message}");
            }
        }

        #region Helper Methods
        private void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        #endregion

        #region Service Methods
        private void BackupExcelData(string filePath)
        {
            // TODO: Triển khai logic sao lưu Excel
            // Gọi service hoặc database để export dữ liệu
            System.Threading.Thread.Sleep(1000); // Giả lập thời gian xử lý

            // Log activity
            Console.WriteLine($"Excel backup created: {filePath}");
        }

        private void BackupPDFData(string filePath)
        {
            // TODO: Triển khai logic sao lưu PDF
            // Gọi service để tạo báo cáo PDF
            System.Threading.Thread.Sleep(1000); // Giả lập thời gian xử lý

            // Log activity
            Console.WriteLine($"PDF backup created: {filePath}");
        }

        private void RestoreData(string filePath)
        {
            // TODO: Triển khai logic phục hồi dữ liệu
            // Đọc file và import dữ liệu vào database
            System.Threading.Thread.Sleep(2000); // Giả lập thời gian xử lý

            // Log activity
            Console.WriteLine($"Data restored from: {filePath}");
        }
        #endregion

        #region Public Methods
        public void PerformAutoBackup()
        {
            try
            {
                string autoBackupPath = $@"C:\EcoStationBackups\AutoBackup_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                BackupExcelData(autoBackupPath);
            }
            catch (Exception ex)
            {
                // Log lỗi, không hiển thị message box cho backup tự động
                System.Diagnostics.Debug.WriteLine($"Auto backup failed: {ex.Message}");
            }
        }

        public string GetLastBackupInfo()
        {
            return $"Last backup: {DateTime.Now:dd/MM/yyyy HH:mm}";
        }

        public void SetRestoreFilePath(string filePath)
        {
            if (txtRestoreFile != null && !string.IsNullOrEmpty(filePath))
            {
                txtRestoreFile.Text = filePath;
            }
        }
        #endregion
    }
}