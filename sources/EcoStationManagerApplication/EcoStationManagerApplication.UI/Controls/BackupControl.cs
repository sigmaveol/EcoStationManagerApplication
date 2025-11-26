using EcoStationManagerApplication.Common.Exporters;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.UI.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class BackupControl : UserControl
    {
        private readonly IExportService _exportService;
        private readonly IExcelExporter _excelExporter;
        private readonly IPdfExporter _pdfExporter;
        private DateTime? _lastBackupTime;
        private bool _isProcessing;

        public BackupControl()
        {
            InitializeComponent();
            _exportService = AppServices.ExportService ?? throw new InvalidOperationException("Không thể khởi tạo dịch vụ export.");
            _excelExporter = new ExcelExporter();
            _pdfExporter = new PdfExporter();
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

        private async void BtnBackupExcel_Click(object sender, EventArgs e)
        {
            if (_isProcessing) return;

            try
            {
                using (var saveDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    FileName = $"EcoStation_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                    Title = "Chọn nơi lưu file sao lưu Excel"
                })
                {
                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        await RunWithProcessingStateAsync(() => BackupExcelDataAsync(saveDialog.FileName));
                        ShowSuccessMessage($"Đã sao lưu Excel thành công!\nFile: {saveDialog.FileName}");
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                ShowWarningMessage(ex.Message);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Lỗi khi sao lưu Excel: {ex.Message}");
            }
        }

        private async void BtnBackupPDF_Click(object sender, EventArgs e)
        {
            if (_isProcessing) return;

            try
            {
                using (var saveDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    FileName = $"EcoStation_Report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
                    Title = "Chọn nơi lưu file sao lưu PDF"
                })
                {
                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        await RunWithProcessingStateAsync(() => BackupPdfDataAsync(saveDialog.FileName));
                        ShowSuccessMessage($"Đã sao lưu PDF thành công!\nFile: {saveDialog.FileName}");
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                ShowWarningMessage(ex.Message);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Lỗi khi sao lưu PDF: {ex.Message}");
            }
        }

        private async void BtnRestore_Click(object sender, EventArgs e)
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
                    await RunWithProcessingStateAsync(() =>
                    {
                        RestoreData(txtRestoreFile.Text);
                        return Task.CompletedTask;
                    });
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
        private async Task BackupExcelDataAsync(string filePath)
        {
            ValidateBackupPath(filePath);

            var orderData = await GetOrderExportDataAsync();
            if (orderData == null || !orderData.Any())
                throw new InvalidOperationException("Không có dữ liệu để sao lưu.");

            var headers = GetOrderColumnHeaders();
            _excelExporter.ExportToExcel(orderData, filePath, "SAO LƯU ĐƠN HÀNG", headers);

            UpdateLastBackupTime();
        }

        private async Task BackupPdfDataAsync(string filePath)
        {
            ValidateBackupPath(filePath);

            var orderData = await GetOrderExportDataAsync();
            if (orderData == null || !orderData.Any())
                throw new InvalidOperationException("Không có dữ liệu để sao lưu.");

            var headers = GetOrderColumnHeaders();
            _pdfExporter.ExportToPdf(orderData, filePath, "DANH SÁCH ĐƠN HÀNG", headers);

            UpdateLastBackupTime();
        }

        private void RestoreData(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                throw new FileNotFoundException("Không tìm thấy file sao lưu để phục hồi.", filePath);

            var ext = Path.GetExtension(filePath)?.ToLowerInvariant();
            if (ext == ".pdf")
            {
                throw new InvalidOperationException("Không hỗ trợ phục hồi từ file PDF. Vui lòng chọn file Excel/CSV.");
            }

            var importService = AppServices.ImportService;
            if (importService == null)
                throw new InvalidOperationException("Không thể khởi tạo dịch vụ import.");

            var validate = importService.ValidateImportFileAsync(filePath).GetAwaiter().GetResult();
            if (!validate.Success || !validate.Data)
            {
                var reason = string.IsNullOrEmpty(validate.Message) ? "File không hợp lệ để import" : validate.Message;
                throw new InvalidOperationException(reason);
            }

            if (ext == ".xlsx" || ext == ".xls")
            {
                var templateImport = importService.ImportOrdersFromExcelTemplateAsync(filePath).GetAwaiter().GetResult();
                if (templateImport.Success)
                {
                    ShowImportSummary(templateImport.Data, templateImport.Message);
                    return;
                }
            }

            var fallbackImport = importService.ImportOrdersFromFileAsync(filePath, EcoStationManagerApplication.Models.Enums.OrderSource.EXCEL).GetAwaiter().GetResult();
            if (!fallbackImport.Success)
            {
                var msg = string.IsNullOrEmpty(fallbackImport.Message) ? "Phục hồi thất bại" : fallbackImport.Message;
                throw new InvalidOperationException(msg);
            }

            ShowImportSummary(fallbackImport.Data, fallbackImport.Message);
        }

        private async Task<List<OrderExportDTO>> GetOrderExportDataAsync()
        {
            var data = await _exportService.GetOrdersForExportAsync("all");
            return data?.OrderByDescending(o => o.NgayTao).ToList() ?? new List<OrderExportDTO>();
        }

        private Dictionary<string, string> GetOrderColumnHeaders() => new Dictionary<string, string>
        {
            { "STT", "STT" },
            { "MaDon", "Mã đơn" },
            { "KhachHang", "Khách hàng" },
            { "Nguon", "Nguồn" },
            { "TrangThai", "Trạng thái" },
            { "TongTien", "Tổng tiền" },
            { "ThanhToan", "Thanh toán" },
            { "NgayTao", "Ngày tạo" }
        };

        private void ValidateBackupPath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Đường dẫn file sao lưu không hợp lệ.", nameof(filePath));

            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private void UpdateLastBackupTime()
        {
            _lastBackupTime = DateTime.Now;
        }

        private void ShowImportSummary(ImportResult result, string message)
        {
            var summary = $"{message}\n\nThành công: {result.SuccessCount}\nLỗi: {result.ErrorCount}";
            if (result.Errors != null && result.Errors.Count > 0)
            {
                var firstErrors = string.Join("\n", result.Errors.Take(5));
                summary += $"\n\nMột số lỗi:\n{firstErrors}";
            }
            ShowSuccessMessage(summary);
        }
        #endregion

        #region Public Methods
        public async Task PerformAutoBackupAsync()
        {
            try
            {
                string autoBackupDirectory = @"C:\EcoStationBackups";
                Directory.CreateDirectory(autoBackupDirectory);

                string autoBackupPath = Path.Combine(autoBackupDirectory, $"AutoBackup_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
                await BackupExcelDataAsync(autoBackupPath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Auto backup failed: {ex.Message}");
            }
        }

        public string GetLastBackupInfo()
        {
            return _lastBackupTime.HasValue
                ? $"Last backup: {_lastBackupTime:dd/MM/yyyy HH:mm}"
                : "Chưa có bản sao lưu nào.";
        }

        public void SetRestoreFilePath(string filePath)
        {
            if (txtRestoreFile != null && !string.IsNullOrEmpty(filePath))
            {
                txtRestoreFile.Text = filePath;
            }
        }
        #endregion

        #region Private Helpers
        private async Task RunWithProcessingStateAsync(Func<Task> operation)
        {
            SetProcessingState(true);
            try
            {
                await operation();
            }
            finally
            {
                SetProcessingState(false);
            }
        }

        private void SetProcessingState(bool isProcessing)
        {
            _isProcessing = isProcessing;
            UseWaitCursor = isProcessing;

            if (btnBackupExcel != null) btnBackupExcel.Enabled = !isProcessing;
            if (btnBackupPDF != null) btnBackupPDF.Enabled = !isProcessing;
            if (btnRestore != null) btnRestore.Enabled = !isProcessing;
            if (browseButton != null) browseButton.Enabled = !isProcessing;
        }
        #endregion
    }
}