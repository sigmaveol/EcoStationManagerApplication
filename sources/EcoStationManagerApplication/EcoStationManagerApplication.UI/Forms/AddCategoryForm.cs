using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.UI.Common;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class AddCategoryForm : Form
    {
        private int? _categoryId;
        private Category _existingCategory;
        private bool _isEditMode => _categoryId.HasValue;

        public AddCategoryForm() : this(null) { }

        public AddCategoryForm(int? categoryId)
        {
            _categoryId = categoryId;
            InitializeComponent();
            InitializeForm();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _ = SaveCategory();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void InitializeForm()
        {
            labelError.Text = "";
            labelError.Visible = false;
            this.Text = _isEditMode ? "Sửa danh mục" : "Thêm danh mục";
            lblTitle.Text = _isEditMode ? "Sửa danh mục" : "Thêm danh mục";
            btnSave.Text = _isEditMode ? "Cập nhật" : "Lưu";

            if (_isEditMode)
            {
                _ = LoadCategoryAsync(_categoryId.Value);
            }
        }

        private async Task LoadCategoryAsync(int categoryId)
        {
            try
            {
                var categoryResult = await AppServices.CategoryService.GetCategoryByIdAsync(categoryId);
                if (categoryResult.Success && categoryResult.Data != null)
                {
                    _existingCategory = categoryResult.Data;
                    txtName.Text = _existingCategory.Name;
                    var typeText = _existingCategory.CategoryType.ToString();
                    var index = cmbType.Items.IndexOf(typeText);
                    cmbType.SelectedIndex = index >= 0 ? index : 0;
                    chkIsActive.Checked = _existingCategory.IsActive == ActiveStatus.ACTIVE;
                }
                else
                {
                    ShowError(categoryResult.Message ?? "Không thể tải danh mục.");
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private bool ValidateForm()
        {
            labelError.Text = "";
            labelError.Visible = false;

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                ShowError("Vui lòng nhập tên danh mục.");
                txtName.Focus();
                return false;
            }

            if (txtName.Text.Length > 255)
            {
                ShowError("Tên danh mục không được vượt quá 255 ký tự.");
                txtName.Focus();
                return false;
            }

            if (cmbType.SelectedItem == null)
            {
                ShowError("Vui lòng chọn loại danh mục.");
                cmbType.Focus();
                return false;
            }

            return true;
        }

        private void ShowError(string message)
        {
            labelError.Text = message;
            labelError.Visible = true;
        }

        private async Task SaveCategory()
        {
            if (!ValidateForm()) return;

            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            try
            {
                var parsedType = Enum.TryParse<CategoryType>(cmbType.SelectedItem?.ToString(), true, out var ct) ? ct : CategoryType.PRODUCT;
                if (_isEditMode)
                {
                    var updated = new Category
                    {
                        CategoryId = _categoryId.Value,
                        Name = txtName.Text.Trim(),
                        CategoryType = parsedType,
                        IsActive = chkIsActive.Checked ? ActiveStatus.ACTIVE : ActiveStatus.INACTIVE,
                        CreatedDate = _existingCategory?.CreatedDate ?? DateTime.Now
                    };

                    var validateResult = await AppServices.CategoryService.ValidateCategoryAsync(updated);
                    if (!validateResult.Success)
                    {
                        ShowError(validateResult.Message ?? "Dữ liệu danh mục không hợp lệ.");
                        return;
                    }

                    var nameExistsResult = await AppServices.CategoryService.IsCategoryNameExistsAsync(updated.Name, updated.CategoryId);
                    if (nameExistsResult.Success && nameExistsResult.Data)
                    {
                        ShowError($"Tên danh mục '{updated.Name}' đã tồn tại.");
                        return;
                    }

                    var updateResult = await AppServices.CategoryService.UpdateCategoryAsync(updated);
                    if (updateResult.Success)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        ShowError(updateResult.Message ?? "Không thể cập nhật danh mục.");
                    }
                }
                else
                {
                    var category = new Category
                    {
                        Name = txtName.Text.Trim(),
                        CategoryType = parsedType,
                        IsActive = chkIsActive.Checked ? ActiveStatus.ACTIVE : ActiveStatus.INACTIVE,
                        CreatedDate = DateTime.Now
                    };

                    var validateResult = await AppServices.CategoryService.ValidateCategoryAsync(category);
                    if (!validateResult.Success)
                    {
                        ShowError(validateResult.Message ?? "Dữ liệu danh mục không hợp lệ.");
                        return;
                    }

                    var nameExistsResult = await AppServices.CategoryService.IsCategoryNameExistsAsync(category.Name);
                    if (nameExistsResult.Success && nameExistsResult.Data)
                    {
                        ShowError($"Tên danh mục '{category.Name}' đã tồn tại.");
                        return;
                    }

                    var createResult = await AppServices.CategoryService.CreateCategoryAsync(category);
                    if (createResult.Success)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        ShowError(createResult.Message ?? "Không thể tạo danh mục.");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
            finally
            {
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
            }
        }
    }
}
