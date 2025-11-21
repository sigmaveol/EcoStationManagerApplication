using EcoStationManagerApplication.Core.Composition;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Models.Enums;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
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

        private async void BtnImportFile_Click(object sender, EventArgs e)
        {
            try
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls|CSV files (*.csv)|*.csv";
                openFileDialog.Title = "Chọn file để nhập dữ liệu";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Hiển thị dialog chọn loại import
                    var importTypeDialog = new Form
                    {
                        Text = "Chọn loại dữ liệu cần import",
                        Size = new Size(300, 150),
                        StartPosition = FormStartPosition.CenterParent,
                        FormBorderStyle = FormBorderStyle.FixedDialog,
                        MaximizeBox = false,
                        MinimizeBox = false
                    };

                    var btnImportOrders = new Button
                    {
                        Text = "Import Đơn hàng",
                        Dock = DockStyle.Top,
                        Height = 40
                    };

                    var btnImportCustomers = new Button
                    {
                        Text = "Import Khách hàng",
                        Dock = DockStyle.Top,
                        Height = 40
                    };

                    OrderSource? selectedSource = null;
                    bool importCustomers = false;

                    btnImportOrders.Click += (s, args) =>
                    {
                        // Chọn nguồn đơn hàng
                        var sourceDialog = new Form
                        {
                            Text = "Chọn nguồn đơn hàng",
                            Size = new Size(250, 200),
                            StartPosition = FormStartPosition.CenterParent,
                            FormBorderStyle = FormBorderStyle.FixedDialog,
                            MaximizeBox = false,
                            MinimizeBox = false
                        };

                        var btnExcel = new Button { Text = "Từ Excel", Dock = DockStyle.Top, Height = 35 };
                        var btnEmail = new Button { Text = "Từ Email", Dock = DockStyle.Top, Height = 35 };
                        var btnGoogleForm = new Button { Text = "Từ Google Form", Dock = DockStyle.Top, Height = 35 };

                        btnExcel.Click += (s2, args2) => { selectedSource = OrderSource.EXCEL; sourceDialog.DialogResult = DialogResult.OK; sourceDialog.Close(); };
                        btnEmail.Click += (s2, args2) => { selectedSource = OrderSource.EMAIL; sourceDialog.DialogResult = DialogResult.OK; sourceDialog.Close(); };
                        btnGoogleForm.Click += (s2, args2) => { selectedSource = OrderSource.GOOGLEFORM; sourceDialog.DialogResult = DialogResult.OK; sourceDialog.Close(); };

                        sourceDialog.Controls.Add(btnGoogleForm);
                        sourceDialog.Controls.Add(btnEmail);
                        sourceDialog.Controls.Add(btnExcel);

                        if (sourceDialog.ShowDialog() == DialogResult.OK)
                        {
                            importTypeDialog.DialogResult = DialogResult.OK;
                            importTypeDialog.Close();
                        }
                    };

                    btnImportCustomers.Click += (s, args) =>
                    {
                        importCustomers = true;
                        importTypeDialog.DialogResult = DialogResult.OK;
                        importTypeDialog.Close();
                    };

                    importTypeDialog.Controls.Add(btnImportCustomers);
                    importTypeDialog.Controls.Add(btnImportOrders);

                    if (importTypeDialog.ShowDialog() == DialogResult.OK)
                    {
                        await ImportDataFromFile(openFileDialog.FileName, selectedSource, importCustomers);
                    }
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

        private async Task ImportDataFromFile(string filePath, OrderSource? source, bool importCustomers)
        {
            try
            {
                var importService = ServiceRegistry.ImportService;

                // Validate file trước
                var validateResult = await importService.ValidateImportFileAsync(filePath);
                if (!validateResult.Success)
                {
                    MessageBox.Show($"File không hợp lệ: {validateResult.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Cursor = Cursors.WaitCursor;

                if (importCustomers)
                {
                    // Import khách hàng
                    var result = await importService.ImportCustomersFromFileAsync(filePath);
                    if (result.Success)
                    {
                        var message = $"Import hoàn tất!\n\n" +
                                    $"Thành công: {result.Data.SuccessCount}\n" +
                                    $"Lỗi: {result.Data.ErrorCount}";

                        if (result.Data.Errors.Any())
                        {
                            message += "\n\nChi tiết lỗi:\n" + string.Join("\n", result.Data.Errors.Take(10));
                            if (result.Data.Errors.Count > 10)
                            {
                                message += $"\n... và {result.Data.Errors.Count - 10} lỗi khác";
                            }
                        }

                        MessageBox.Show(message, "Kết quả Import",
                            MessageBoxButtons.OK,
                            result.Data.ErrorCount > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Lỗi import: {result.Message}", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (source.HasValue)
                {
                    // Import đơn hàng
                    var result = await importService.ImportOrdersFromFileAsync(filePath, source.Value);
                    if (result.Success)
                    {
                        var message = $"Import hoàn tất!\n\n" +
                                    $"Thành công: {result.Data.SuccessCount}\n" +
                                    $"Lỗi: {result.Data.ErrorCount}";

                        if (result.Data.Errors.Any())
                        {
                            message += "\n\nChi tiết lỗi:\n" + string.Join("\n", result.Data.Errors.Take(10));
                            if (result.Data.Errors.Count > 10)
                            {
                                message += $"\n... và {result.Data.Errors.Count - 10} lỗi khác";
                            }
                        }

                        MessageBox.Show(message, "Kết quả Import",
                            MessageBoxButtons.OK,
                            result.Data.ErrorCount > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Lỗi import: {result.Message}", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi import file: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
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