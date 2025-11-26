using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.UI.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class AddPackagingForm : Form
    {
        private int? _packagingId = null;
        private bool _isEditMode => _packagingId.HasValue;

        public AddPackagingForm() : this(null) { }

        public AddPackagingForm(int? packagingId)
        {
            _packagingId = packagingId;
            InitializeComponent();
            InitializeForm();
        }

        private async void InitializeForm()
        {
            labelError.Text = "";
            labelError.Visible = false;
            LoadPackagingTypes();
            txtDepositPrice.Text = "0";

            if (_isEditMode)
            {
                this.Text = lblTitle.Text = "Chỉnh sửa bao bì";
                btnSave.Text = "Cập nhật";
                btnDelete.Visible = AppServices.State.IsAdmin || AppServices.State.IsManager;
                txtBarcode.ReadOnly = true;
                await LoadPackagingData(_packagingId.Value);
            }
            else
            {
                this.Text = lblTitle.Text = "Thêm bao bì mới";
                btnSave.Text = "Lưu";
                btnDelete.Visible = false;
                txtBarcode.ReadOnly = false;
            }
        }

        private async Task LoadPackagingData(int packagingId)
        {
            try
            {
                var result = await AppServices.PackagingService.GetPackagingByIdAsync(packagingId);
                if (result.Success && result.Data != null)
                {
                    var packaging = result.Data;
                    txtBarcode.Text = packaging.Barcode ?? "";
                    txtName.Text = packaging.Name;
                    txtDepositPrice.Text = packaging.DepositPrice.ToString("N0");

                    var selectedType = cmbType.Items.OfType<ComboItem<string>>()
                        .FirstOrDefault(x => x.Value == packaging.Type);
                    if (selectedType != null)
                        cmbType.SelectedItem = selectedType;
                    else
                        cmbType.SelectedIndex = 0;
                }
                else
                {
                    UIHelper.ShowErrorMessage("Không thể tải thông tin bao bì.");
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "tải thông tin bao bì");
            }
        }

        private void LoadPackagingTypes()
        {
            cmbType.Items.Clear();
            cmbType.Items.Add(new ComboItem<string> { Text = "Chai", Value = "bottle" });
            cmbType.Items.Add(new ComboItem<string> { Text = "Hộp", Value = "box" });
            cmbType.Items.Add(new ComboItem<string> { Text = "Thùng", Value = "container" });
            cmbType.DisplayMember = "Text";
            cmbType.ValueMember = "Value";
            cmbType.SelectedIndex = 0;
        }

        private bool ValidateForm()
        {
            labelError.Text = "";
            labelError.Visible = false;

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                ShowError("Vui lòng nhập tên bao bì.");
                txtName.Focus();
                return false;
            }
            if (txtName.Text.Length > 150)
            {
                ShowError("Tên bao bì không được vượt quá 150 ký tự.");
                txtName.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtBarcode.Text) && txtBarcode.Text.Length > 20)
            {
                ShowError("Barcode không được vượt quá 20 ký tự.");
                txtBarcode.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDepositPrice.Text) || !decimal.TryParse(txtDepositPrice.Text.Replace(",", "").Replace(".", ""), out decimal depositPrice) || depositPrice < 0)
            {
                ShowError("Giá ký quỹ phải là số >= 0.");
                txtDepositPrice.Focus();
                return false;
            }
            if (depositPrice > 99999999.99m)
            {
                ShowError("Giá ký quỹ không được vượt quá 99,999,999.99");
                txtDepositPrice.Focus();
                return false;
            }

            return true;
        }

        private void ShowError(string message)
        {
            labelError.Text = message;
            labelError.Visible = true;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            if (!string.IsNullOrWhiteSpace(txtBarcode.Text))
            {
                var barcodeCheck = await AppServices.PackagingService.IsBarcodeExistsAsync(
                    txtBarcode.Text.Trim(), _isEditMode ? _packagingId : null);
                if (barcodeCheck.Success && barcodeCheck.Data)
                {
                    ShowError($"Barcode '{txtBarcode.Text.Trim()}' đã tồn tại.");
                    txtBarcode.Focus();
                    return;
                }
            }

            SetControlsEnabled(false);
            btnSave.Text = _isEditMode ? "Đang cập nhật..." : "Đang lưu...";

            try
            {
                decimal depositPrice = 0;
                if (!string.IsNullOrWhiteSpace(txtDepositPrice.Text))
                {
                    string priceText = txtDepositPrice.Text.Trim().Replace(" ", "");
                    if (priceText.Contains(".") && priceText.Contains(","))
                        priceText = priceText.Replace(".", "").Replace(",", ".");
                    else if (priceText.Contains("."))
                    {
                        var parts = priceText.Split('.');
                        if (parts.Length > 2 || (parts.Length == 2 && parts[1].Length == 3))
                            priceText = priceText.Replace(".", "");
                    }
                    else if (priceText.Contains(","))
                    {
                        var parts = priceText.Split(',');
                        if (parts.Length > 2 || (parts.Length == 2 && parts[1].Length == 3))
                            priceText = priceText.Replace(",", "");
                        else
                            priceText = priceText.Replace(",", ".");
                    }
                    if (!decimal.TryParse(priceText, System.Globalization.NumberStyles.Number,
                        System.Globalization.CultureInfo.InvariantCulture, out depositPrice))
                    {
                        ShowError("Giá ký quỹ không hợp lệ.");
                        return;
                    }
                }

                string type = (cmbType.SelectedItem as ComboItem<string>)?.Value ?? "";

                var packaging = new Packaging
                {
                    Barcode = string.IsNullOrWhiteSpace(txtBarcode.Text) ? null : txtBarcode.Text.Trim(),
                    Name = txtName.Text.Trim(),
                    Type = type,
                    DepositPrice = depositPrice
                };

                if (_isEditMode)
                    packaging.PackagingId = _packagingId.Value;

                var validateResult = await AppServices.PackagingService.ValidatePackagingAsync(packaging);
                if (!validateResult.Success)
                {
                    ShowError(validateResult.Message ?? "Dữ liệu bao bì không hợp lệ.");
                    return;
                }

                bool success = false;
                string message = "";

                if (_isEditMode)
                {
                    var updateResult = await AppServices.PackagingService.UpdatePackagingAsync(packaging);
                    success = updateResult.Success;
                    message = updateResult.Message ?? "Cập nhật bao bì thành công!";
                }
                else
                {
                    var createResult = await AppServices.PackagingService.CreatePackagingAsync(packaging);
                    success = createResult.Success;
                    message = createResult.Message ?? "Thêm bao bì thành công!";
                    if (success) _packagingId = createResult.Data;
                }

                if (success)
                {
                    UIHelper.ShowSuccessMessage(message);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    ShowError(message ?? (_isEditMode ? "Không thể cập nhật bao bì." : "Không thể thêm bao bì."));
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, _isEditMode ? "cập nhật bao bì" : "thêm bao bì");
                ShowError($"Đã xảy ra lỗi khi {(_isEditMode ? "cập nhật" : "thêm")} bao bì. Vui lòng thử lại.");
            }
            finally
            {
                SetControlsEnabled(true);
                btnSave.Text = _isEditMode ? "Cập nhật" : "Lưu";
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (!_isEditMode || !_packagingId.HasValue) return;

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa bao bì này không?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            SetControlsEnabled(false);
            btnDelete.Text = "Đang xóa...";

            try
            {
                var deleteResult = await AppServices.PackagingService.DeletePackagingAsync(_packagingId.Value);
                if (deleteResult.Success)
                {
                    UIHelper.ShowSuccessMessage(deleteResult.Message ?? "Xóa bao bì thành công!");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    UIHelper.ShowErrorMessage(deleteResult.Message ?? "Không thể xóa bao bì.");
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "xóa bao bì");
                UIHelper.ShowErrorMessage("Đã xảy ra lỗi khi xóa bao bì.");
            }
            finally
            {
                SetControlsEnabled(true);
                btnDelete.Text = "Xóa bao bì";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void SetControlsEnabled(bool enabled)
        {
            txtBarcode.Enabled = enabled;
            txtName.Enabled = enabled;
            cmbType.Enabled = enabled;
            txtDepositPrice.Enabled = enabled;
            btnSave.Enabled = enabled;
            btnDelete.Enabled = enabled;
            btnCancel.Enabled = enabled;
        }
    }
}

