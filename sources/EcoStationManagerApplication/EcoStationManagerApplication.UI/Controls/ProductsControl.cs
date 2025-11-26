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
    public partial class ProductsControl : UserControl, IRefreshableControl
    {
        private List<ProductDTO> products;
        private List<Packaging> packagings;
        private List<Category> categories;
        private string searchTerm = "";

        public ProductsControl()
        {
            InitializeComponent();
            InitializeControls();
            _ = LoadDataAsync();
        }

        public void RefreshData()
        {
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
                var categoriesResult = await AppServices.CategoryService.GetAllCategoriesAsync();
                if (categoriesResult.Success && categoriesResult.Data != null)
                {
                    categories = categoriesResult.Data.ToList();
                }
                else
                {
                    categories = new List<Category>();
                }

                // Bind data vào UI
                BindProductsData();
                BindPackagingsData();
                BindCategoriesData();
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "tải dữ liệu sản phẩm và bao bì");
                products = new List<ProductDTO>();
                packagings = new List<Packaging>();
                categories = new List<Category>();
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
            dataGridViewPackagings.Columns.Clear();
            var colPackagingId = new DataGridViewTextBoxColumn { Name = "PackagingId", HeaderText = "ID", Visible = false };
            dataGridViewPackagings.Columns.Add(colPackagingId);
            dataGridViewPackagings.Columns.Add("PackagingBarcode", "Barcode");
            dataGridViewPackagings.Columns.Add("PackagingName", "Tên bao bì");
            dataGridViewPackagings.Columns.Add("PackagingType", "Loại");
            dataGridViewPackagings.Columns.Add("DepositPrice", "Giá ký quỹ");
            dataGridViewPackagings.Columns.Add("PackagingAction", "Thao tác");


            dataGridViewCategories.Columns.Clear();
            dataGridViewCategories.Columns.Add("CategoryName", "Tên danh mục");
            dataGridViewCategories.Columns.Add("CategoryType", "Loại danh mục");
            dataGridViewCategories.Columns.Add("CreatedDate", "Ngày tạo");
            dataGridViewCategories.Columns.Add("IsActive", "Trạng thái");
            dataGridViewCategories.Columns["CreatedDate"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
        }

        private void BindCategoriesData()
        {
            if (categories == null)
            {
                dataGridViewCategories.Rows.Clear();
                return;
            }

            var filteredCategories = categories.Where(category =>
                category.Name.ToLower().Contains(searchTerm.ToLower()) ||
                category.CategoryType.ToString().ToLower().Contains(searchTerm.ToLower())
            ).ToList();

            dataGridViewCategories.Rows.Clear();
            foreach (var category in filteredCategories)
            {
                dataGridViewCategories.Rows.Add(
                    category.Name,
                    DisplayCategoryType(category.CategoryType),
                    category.CreatedDate,
                    category.IsActive == ActiveStatus.ACTIVE ? "Hoạt động" : "Ngưng",
                    "👁️"
                );
            }
        }

        private string DisplayCategoryType(CategoryType categoryType)
        {
            switch (categoryType)
            {
                case CategoryType.PRODUCT:
                    return "Sản phẩm";
                case CategoryType.SERVICE:
                    return "Dịch vụ";
                case CategoryType.OTHER:
                    return "Khác";
                default:
                    return "Sản phẩm";
            }
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
                dataGridViewPackagings.Rows.Clear();
                return;
            }

            var filteredPackagings = packagings.Where(packaging =>
                packaging.Name.ToLower().Contains(searchTerm.ToLower()) ||
                packaging.Barcode?.ToLower().Contains(searchTerm.ToLower()) == true ||
                packaging.Type?.ToLower().Contains(searchTerm.ToLower()) == true
            ).ToList();

            dataGridViewPackagings.Rows.Clear();
            foreach (var packaging in filteredPackagings)
            {
                int rowIndex = dataGridViewPackagings.Rows.Add(
                    packaging.PackagingId,
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
            Form mainForm = this.FindForm();
            while (mainForm != null && !(mainForm is MainForm))
            {
                mainForm = mainForm.ParentForm ?? mainForm.Owner;
            }

            if (tabControl.SelectedTab == tabPageProducts)
            {
                using (var addProductForm = new AddProductForm())
                {
                    DialogResult result = mainForm != null
                        ? FormHelper.ShowModalWithDim(mainForm, addProductForm)
                        : addProductForm.ShowDialog();

                    if (result == DialogResult.OK)
                        await RefreshProductsData();
                }
            }
            else if (tabControl.SelectedTab == tabPagePackagings)
            {
                using (var addPackagingForm = new AddPackagingForm())
                {
                    DialogResult result = mainForm != null
                        ? FormHelper.ShowModalWithDim(mainForm, addPackagingForm)
                        : addPackagingForm.ShowDialog();

                    if (result == DialogResult.OK)
                        await RefreshPackagingsData();
                }
            }
        }

        private async Task RefreshProductsData()
        {
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
            BindProductsData();
        }

        private async Task RefreshPackagingsData()
        {
            var packagingsResult = await AppServices.PackagingService.GetAllPackagingsAsync();
            if (packagingsResult.Success && packagingsResult.Data != null)
            {
                packagings = packagingsResult.Data.ToList();
            }
            else
            {
                packagings = new List<Packaging>();
            }
            BindPackagingsData();
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabPageProducts)
            {
                btnAddProduct.Text = "Thêm sản phẩm";
                BindProductsData();
            }
            else if (tabControl.SelectedTab == tabPagePackagings)
            {
                btnAddProduct.Text = "Thêm bao bì";
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

        private async void dataGridViewPackagings_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == dataGridViewPackagings.Columns["PackagingAction"].Index)
            {
                var packagingIdCell = dataGridViewPackagings.Rows[e.RowIndex].Cells["PackagingId"];
                if (packagingIdCell?.Value == null || !int.TryParse(packagingIdCell.Value.ToString(), out int packagingId))
                    return;

                Form mainForm = this.FindForm();
                while (mainForm != null && !(mainForm is MainForm))
                {
                    mainForm = mainForm.ParentForm ?? mainForm.Owner;
                }

                using (var editPackagingForm = new AddPackagingForm(packagingId))
                {
                    DialogResult result;
                    if (mainForm != null)
                    {
                        result = FormHelper.ShowModalWithDim(mainForm, editPackagingForm);
                    }
                    else
                    {
                        result = editPackagingForm.ShowDialog();
                    }

                    if (result == DialogResult.OK)
                    {
                        await RefreshPackagingsData();
                    }
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