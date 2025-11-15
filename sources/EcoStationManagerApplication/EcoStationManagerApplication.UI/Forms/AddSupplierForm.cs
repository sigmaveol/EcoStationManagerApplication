using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.UI.Common;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using AppServices = EcoStationManagerApplication.UI.Common.AppServices;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class AddSupplierForm : Form
    {
        private Supplier _supplier = null; // null = Add mode, có giá trị = Edit mode
        private bool _isEditMode => _supplier != null;

        /// <summary>
        /// Constructor cho chế độ Thêm mới
        /// </summary>
        public AddSupplierForm() : this(null)
        {
        }

        /// <summary>
        /// Constructor cho chế độ Chỉnh sửa
        /// </summary>
        /// <param name="supplier">Supplier cần chỉnh sửa. Null = chế độ thêm mới</param>
        public AddSupplierForm(Supplier supplier)
        {
            _supplier = supplier;
            InitializeComponent();
            InitializeForm();
        }

        private async void InitializeForm()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new System.Drawing.Size(700, 500);

            // Ẩn label lỗi ban đầu
            if (labelError != null)
            {
                labelError.Text = "";
                labelError.Visible = false;
            }

            // Thiết lập chế độ Add hoặc Edit
            if (_isEditMode)
            {
                // Chế độ Edit: Load dữ liệu nhà cung cấp
                this.Text = "Chỉnh sửa nhà cung cấp";
                if (lblTitle != null)
                    lblTitle.Text = "Chỉnh sửa nhà cung cấp";
                if (btnSave != null)
                    btnSave.Text = "Cập nhật";
                await LoadSupplierData();
            }
            else
            {
                // Chế độ Add
                this.Text = "Thêm nhà cung cấp mới";
                if (lblTitle != null)
                    lblTitle.Text = "Thêm nhà cung cấp mới";
                if (btnSave != null)
                    btnSave.Text = "Lưu";
            }

            // Thiết lập placeholder và validation
            SetupValidation();
        }

        private async Task LoadSupplierData()
        {
            try
            {
                if (_supplier == null) return;

                // Load dữ liệu vào form
                if (txtName != null)
                    txtName.Text = _supplier.Name ?? "";
                if (txtContactPerson != null)
                    txtContactPerson.Text = _supplier.ContactPerson ?? "";
                if (txtPhone != null)
                    txtPhone.Text = _supplier.Phone ?? "";
                if (txtEmail != null)
                    txtEmail.Text = _supplier.Email ?? "";
                if (txtAddress != null)
                    txtAddress.Text = _supplier.Address ?? "";
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải thông tin nhà cung cấp: {ex.Message}");
            }
        }

        private void SetupValidation()
        {
            // Chỉ cho phép nhập số cho SĐT
            if (txtPhone != null)
            {
                txtPhone.KeyPress += TxtPhone_KeyPress;
            }

            // Validate email format
            if (txtEmail != null)
            {
                txtEmail.Leave += TxtEmail_Leave;
            }
        }

        private void TxtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép nhập số, backspace, delete
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtEmail_Leave(object sender, EventArgs e)
        {
            if (txtEmail != null && !string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                if (!IsValidEmail(txtEmail.Text))
                {
                    MessageBox.Show("Email không đúng định dạng!", "Cảnh báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                }
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate các trường bắt buộc
                if (string.IsNullOrWhiteSpace(txtName?.Text))
                {
                    ShowError("Vui lòng nhập tên nhà cung cấp.");
                    if (txtName != null) txtName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtContactPerson?.Text))
                {
                    ShowError("Vui lòng nhập người liên hệ.");
                    if (txtContactPerson != null) txtContactPerson.Focus();
                    return;
                }

                // Validate email nếu có
                if (!string.IsNullOrWhiteSpace(txtEmail?.Text))
                {
                    if (!IsValidEmail(txtEmail.Text))
                    {
                        ShowError("Email không đúng định dạng.");
                        if (txtEmail != null) txtEmail.Focus();
                        return;
                    }
                }

                // Ẩn lỗi nếu validation thành công
                if (labelError != null)
                {
                    labelError.Visible = false;
                }

                // Tạo hoặc cập nhật Supplier
                if (_isEditMode)
                {
                    // Chế độ Edit: Cập nhật thông tin
                    _supplier.Name = txtName.Text.Trim();
                    _supplier.ContactPerson = txtContactPerson.Text.Trim();
                    _supplier.Phone = txtPhone?.Text?.Trim();
                    _supplier.Email = txtEmail?.Text?.Trim();
                    _supplier.Address = txtAddress?.Text?.Trim();

                    var result = await AppServices.SupplierService.UpdateSupplierAsync(_supplier);
                    if (!result.Success)
                    {
                        ShowError(result.Message ?? "Không thể cập nhật nhà cung cấp.");
                        return;
                    }

                    MessageBox.Show("Đã cập nhật nhà cung cấp thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Chế độ Add: Tạo mới
                    var supplier = new Supplier
                    {
                        Name = txtName.Text.Trim(),
                        ContactPerson = txtContactPerson.Text.Trim(),
                        Phone = txtPhone?.Text?.Trim(),
                        Email = txtEmail?.Text?.Trim(),
                        Address = txtAddress?.Text?.Trim(),
                        CreatedAt = DateTime.Now
                    };

                    var result = await AppServices.SupplierService.CreateSupplierAsync(supplier);
                    if (!result.Success)
                    {
                        ShowError(result.Message ?? "Không thể tạo nhà cung cấp mới.");
                        return;
                    }

                    MessageBox.Show("Đã tạo nhà cung cấp mới thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi: {ex.Message}");
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ShowError(string message)
        {
            if (labelError != null)
            {
                labelError.Text = message;
                labelError.Visible = true;
                labelError.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panelMain_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
