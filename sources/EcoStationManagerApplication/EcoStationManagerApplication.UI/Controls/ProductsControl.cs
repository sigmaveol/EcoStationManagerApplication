using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.UI.Common;
using EcoStationManagerApplication.UI.Forms;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class ProductsControl : UserControl
    {
        private List<ProductDTO> products;
        private List<Packaging> packagings;
        private List<CategoryDTO> categories;
        private string searchTerm = "";

        public ProductsControl()
        {
            InitializeComponent();
            InitializeControls();
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                // Load Products từ database
                var productsResult = await AppServices.ProductService.GetAllProductsAsync();
                if (productsResult.Success && productsResult.Data != null)
                {
                    products = productsResult.Data.Select(p => new ProductDTO
                    {
                        ProductId = p.ProductId,
                        Code = p.Sku ?? "",
                        Name = p.Name,
                        CategoryId = p.CategoryId,
                        UnitMeasure = p.Unit,
                        BasePrice = p.Price,
                        ProductType = p.ProductType.ToString(),
                        IsActive = p.IsActive == ActiveStatus.ACTIVE
                    }).ToList();
                }
                else
                {
                    products = new List<ProductDTO>();
                }

                // Load Packagings từ database
                var packagingsResult = await AppServices.PackagingService.GetAllPackagingsAsync();
                if (packagingsResult.Success && packagingsResult.Data != null)
                {
                    packagings = packagingsResult.Data.ToList();
                }
                else
                {
                    packagings = new List<Packaging>();
                }

                // Load Categories từ database
                var categoriesResult = await AppServices.CategoryService.GetActiveCategoriesAsync();
                if (categoriesResult.Success && categoriesResult.Data != null)
                {
                    categories = categoriesResult.Data.Select(c => new CategoryDTO
                    {
                        CategoryId = c.CategoryId,
                        Name = c.Name
                    }).ToList();
                }
                else
                {
                    categories = new List<CategoryDTO>();
                }

                // Bind data vào UI
                BindProductsData();
                BindPackagingsData();
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "tải dữ liệu sản phẩm và bao bì");
                products = new List<ProductDTO>();
                packagings = new List<Packaging>();
                categories = new List<CategoryDTO>();
            }
        }

        private void InitializeControls()
        {
            // Khởi tạo DataGridViews trước
            InitializeDataGridViews();
        }

        private void InitializeDataGridViews()
        {
            // Initialize Products DataGridView
            dataGridViewProducts.Columns.Clear();
            dataGridViewProducts.Columns.Add("ProductCode", "Mã SP");
            dataGridViewProducts.Columns.Add("ProductName", "Tên sản phẩm");
            dataGridViewProducts.Columns.Add("ProductCategory", "Danh mục");
            dataGridViewProducts.Columns.Add("ProductUnit", "Đơn vị");
            dataGridViewProducts.Columns.Add("ProductPrice", "Giá cơ bản");
            dataGridViewProducts.Columns.Add("ProductType", "Loại");
            dataGridViewProducts.Columns.Add("ProductStatus", "Trạng thái");
            dataGridViewProducts.Columns.Add("ProductAction", "Thao tác");

            // Initialize Packagings DataGridView
            dataGridViewVariants.Columns.Clear();
            dataGridViewVariants.Columns.Add("PackagingBarcode", "Barcode");
            dataGridViewVariants.Columns.Add("PackagingName", "Tên bao bì");
            dataGridViewVariants.Columns.Add("PackagingType", "Loại");
            dataGridViewVariants.Columns.Add("DepositPrice", "Giá ký quỹ");
            dataGridViewVariants.Columns.Add("PackagingAction", "Thao tác");
        }

        private void BindProductsData()
        {
            if (products == null)
            {
                dataGridViewProducts.Rows.Clear();
                return;
            }

            var filteredProducts = products.Where(product =>
                product.Name.ToLower().Contains(searchTerm.ToLower()) ||
                product.Code.ToLower().Contains(searchTerm.ToLower())
            ).ToList();

            dataGridViewProducts.Rows.Clear();
            foreach (var product in filteredProducts)
            {
                dataGridViewProducts.Rows.Add(
                    product.Code,
                    product.Name,
                    GetCategoryName(product.CategoryId),
                    product.UnitMeasure,
                    product.BasePrice?.ToString("N0") + "₫",
                    product.ProductType,
                    product.IsActive ? "Hoạt động" : "Ngưng",
                    "👁️"
                );
            }
        }

        private void BindPackagingsData()
        {
            if (packagings == null)
            {
                dataGridViewVariants.Rows.Clear();
                return;
            }

            var filteredPackagings = packagings.Where(packaging =>
                packaging.Name.ToLower().Contains(searchTerm.ToLower()) ||
                packaging.Barcode?.ToLower().Contains(searchTerm.ToLower()) == true ||
                packaging.Type?.ToLower().Contains(searchTerm.ToLower()) == true
            ).ToList();

            dataGridViewVariants.Rows.Clear();
            foreach (var packaging in filteredPackagings)
            {
                dataGridViewVariants.Rows.Add(
                    packaging.Barcode ?? "",
                    packaging.Name,
                    packaging.Type ?? "",
                    packaging.DepositPrice.ToString("N0") + "₫",
                    "👁️"
                );
            }
        }

        private string GetCategoryName(int? categoryId)
        {
            if (!categoryId.HasValue) return "Chưa phân loại";
            return categories.FirstOrDefault(c => c.CategoryId == categoryId.Value)?.Name ?? "Chưa phân loại";
        }


        #region Event Handlers
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            searchTerm = txtSearch.Text;
            BindProductsData();
            BindPackagingsData();
        }

        private async void btnAddProduct_Click(object sender, EventArgs e)
        {
            // Tìm MainForm
            Form mainForm = this.FindForm();
            while (mainForm != null && !(mainForm is MainForm))
            {
                mainForm = mainForm.ParentForm ?? mainForm.Owner;
            }

            using (var addProductForm = new AddProductForm())
            {
                DialogResult result;
                if (mainForm != null)
                {
                    // Hiển thị với hiệu ứng làm mờ MainForm
                    result = FormHelper.ShowModalWithDim(mainForm, addProductForm);
                }
                else
                {
                    // Fallback nếu không tìm thấy MainForm
                    result = addProductForm.ShowDialog();
                }

                if (result == DialogResult.OK)
                {
                    // Refresh danh sách sản phẩm sau khi thêm thành công
                    await RefreshProductsData();
                }
            }
            await RefreshProductsData();
        }

        private async Task RefreshProductsData()
        {
            await LoadDataAsync();
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Refresh data when switching tabs
            if (tabControl.SelectedTab == tabPageProducts)
            {
                BindProductsData();
            }
            else if (tabControl.SelectedTab == tabPagePackagings)
            {
                BindPackagingsData();
            }
        }

        private async void dataGridViewProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == dataGridViewProducts.Columns["ProductAction"].Index)
            {
                // Lấy ProductId từ dòng được chọn
                var productCode = dataGridViewProducts.Rows[e.RowIndex].Cells["ProductCode"].Value?.ToString();
                if (string.IsNullOrEmpty(productCode)) return;

                // Tìm product theo Code (SKU)
                var product = products?.FirstOrDefault(p => p.Code == productCode);
                if (product == null) return;

                // Tìm MainForm
                Form mainForm = this.FindForm();
                while (mainForm != null && !(mainForm is MainForm))
                {
                    mainForm = mainForm.ParentForm ?? mainForm.Owner;
                }

                // Mở form Edit
                using (var editProductForm = new AddProductForm(product.ProductId))
                {
                    DialogResult result;
                    if (mainForm != null)
                    {
                        result = FormHelper.ShowModalWithDim(mainForm, editProductForm);
                    }
                    else
                    {
                        result = editProductForm.ShowDialog();
                    }

                    if (result == DialogResult.OK)
                    {
                        // Refresh danh sách sản phẩm sau khi cập nhật thành công
                        await RefreshProductsData();
                    }
                }
            }
        }

        private void dataGridViewVariants_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridViewVariants.Columns["PackagingAction"].Index)
            {
                var packagingId = dataGridViewVariants.Rows[e.RowIndex].Cells["PackagingId"].Value;
                if (packagingId != null && int.TryParse(packagingId.ToString(), out int id))
                {
                    MessageBox.Show($"Xem chi tiết bao bì: {id}", "Chi tiết bao bì", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // TODO: Mở form chi tiết/sửa bao bì
                }
            }
        }

        private void btnAddProduct_MouseEnter(object sender, EventArgs e)
        {
            btnAddProduct.FillColor = Color.FromArgb(33, 140, 73);
        }

        private void btnAddProduct_MouseLeave(object sender, EventArgs e)
        {
            btnAddProduct.FillColor = Color.FromArgb(31, 107, 59);
        }
        #endregion
    }
}