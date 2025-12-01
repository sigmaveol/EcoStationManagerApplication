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
            var colProductId = new DataGridViewTextBoxColumn { Name = "ProductId", HeaderText = "ID", Visible = false };
            dataGridViewProducts.Columns.Add(colProductId);
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
            var colCategoryId = new DataGridViewTextBoxColumn { Name = "CategoryId", HeaderText = "ID", Visible = false };
            dataGridViewCategories.Columns.Add(colCategoryId);
            dataGridViewCategories.Columns.Add("CategoryName", "Tên danh mục");
            dataGridViewCategories.Columns.Add("CategoryType", "Loại danh mục");
            dataGridViewCategories.Columns.Add("CreatedDate", "Ngày tạo");
            dataGridViewCategories.Columns.Add("IsActive", "Trạng thái");
            dataGridViewCategories.Columns.Add("CategoryAction", "Thao tác");
            dataGridViewCategories.Columns["CreatedDate"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            dataGridViewCategories.CellClick += dataGridViewCategories_CellClick;
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
                    category.CategoryId,
                    category.Name,
                    DisplayCategoryType(category.CategoryType),
                    category.CreatedDate,
                    category.IsActive == ActiveStatus.ACTIVE ? "Hoạt động" : "Ngưng",
                    "⋯"
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
                    product.ProductId,
                    product.Code,
                    product.Name,
                    GetCategoryName(product.CategoryId),
                    product.UnitMeasure,
                    product.BasePrice?.ToString("N0") + "₫",
                    product.ProductType,
                    product.IsActive ? "Hoạt động" : "Ngưng",
                    "⋯"
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
                    "⋯"
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
            else if (tabControl.SelectedTab == tabPageCategories)
            {
                using (var addCategoryForm = new AddCategoryForm())
                {
                    DialogResult result = mainForm != null
                        ? FormHelper.ShowModalWithDim(mainForm, addCategoryForm)
                        : addCategoryForm.ShowDialog();

                    if (result == DialogResult.OK)
                        await RefreshCategoriesData();
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
            else if (tabControl.SelectedTab == tabPageCategories)
            {
                btnAddProduct.Text = "Thêm danh mục";
                BindCategoriesData();
            }
        }

        private async void dataGridViewProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == dataGridViewProducts.Columns["ProductAction"].Index)
            {
                var idCell = dataGridViewProducts.Rows[e.RowIndex].Cells["ProductId"];
                if (idCell?.Value == null || !int.TryParse(idCell.Value.ToString(), out int productId)) return;

                var productDto = products?.FirstOrDefault(p => p.ProductId == productId);
                if (productDto == null) return;

                var menu = new ContextMenuStrip();
                var editItem = new ToolStripMenuItem("Sửa");
                var toggleItem = new ToolStripMenuItem(productDto.IsActive ? "Vô hiệu hóa" : "Kích hoạt");
                var deleteItem = new ToolStripMenuItem("Xóa");

                editItem.Click += async (_, __) =>
                {
                    using (var editProductForm = new AddProductForm(productId))
                    {
                        var result = editProductForm.ShowDialog(this.FindForm());
                        if (result == DialogResult.OK)
                            await RefreshProductsData();
                    }
                };

                toggleItem.Click += async (_, __) =>
                {
                    var newStatus = !productDto.IsActive;
                    var toggleResult = await AppServices.ProductService.ToggleProductStatusAsync(productId, newStatus);
                    if (toggleResult.Success)
                    {
                        await RefreshProductsData();
                    }
                    else
                    {
                        MessageBox.Show(toggleResult.Message ?? "Thay đổi trạng thái thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                deleteItem.Click += async (_, __) =>
                {
                    if (MessageBox.Show("Xác nhận xóa sản phẩm?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var deleteResult = await AppServices.ProductService.DeleteProductAsync(productId);
                        if (deleteResult.Success)
                        {
                            await RefreshProductsData();
                        }
                        else
                        {
                            MessageBox.Show(deleteResult.Message ?? "Xóa sản phẩm thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                };

                menu.Items.AddRange(new ToolStripItem[] { editItem, toggleItem, deleteItem });
                var cellLocProd = dataGridViewProducts.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Location;
                var screenPoint = dataGridViewProducts.PointToScreen(new Point(cellLocProd.X + 20, cellLocProd.Y + 20));
                menu.Show(screenPoint);
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

                var menu = new ContextMenuStrip();
                var editItem = new ToolStripMenuItem("Sửa");
                var deleteItem = new ToolStripMenuItem("Xóa");

                editItem.Click += async (_, __) =>
                {
                    using (var editPackagingForm = new AddPackagingForm(packagingId))
                    {
                        var result = editPackagingForm.ShowDialog(this.FindForm());
                        if (result == DialogResult.OK)
                            await RefreshPackagingsData();
                    }
                };

                deleteItem.Click += async (_, __) =>
                {
                    if (MessageBox.Show("Xác nhận xóa bao bì?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var deleteResult = await AppServices.PackagingService.DeletePackagingAsync(packagingId);
                        if (deleteResult.Success)
                        {
                            await RefreshPackagingsData();
                        }
                        else
                        {
                            MessageBox.Show(deleteResult.Message ?? "Xóa bao bì thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                };

                menu.Items.AddRange(new ToolStripItem[] { editItem, deleteItem });
                var cellLocPkg = dataGridViewPackagings.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Location;
                var screenPoint = dataGridViewPackagings.PointToScreen(new Point(cellLocPkg.X + 20, cellLocPkg.Y + 20));
                menu.Show(screenPoint);
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
        
        private async Task RefreshCategoriesData()
        {
            var categoriesResult = await AppServices.CategoryService.GetAllCategoriesAsync();
            if (categoriesResult.Success && categoriesResult.Data != null)
            {
                categories = categoriesResult.Data.ToList();
            }
            else
            {
                categories = new List<Category>();
            }
            BindCategoriesData();
        }

        private async void dataGridViewCategories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != dataGridViewCategories.Columns["CategoryAction"].Index) return;

            var idCell = dataGridViewCategories.Rows[e.RowIndex].Cells["CategoryId"];
            if (idCell?.Value == null || !int.TryParse(idCell.Value.ToString(), out int categoryId)) return;

            var categoryResult = await AppServices.CategoryService.GetCategoryByIdAsync(categoryId);
            if (!categoryResult.Success || categoryResult.Data == null) return;
            var category = categoryResult.Data;

            var menu = new ContextMenuStrip();
            var editItem = new ToolStripMenuItem("Sửa");
            var toggleItem = new ToolStripMenuItem(category.IsActive == ActiveStatus.ACTIVE ? "Vô hiệu hóa" : "Kích hoạt");
            var deleteItem = new ToolStripMenuItem("Xóa");
            editItem.Click += async (_, __) =>
            {
                using (var editCategoryForm = new AddCategoryForm(category.CategoryId))
                {
                    DialogResult result = editCategoryForm.ShowDialog(this.FindForm());
                    if (result == DialogResult.OK)
                    {
                        await RefreshCategoriesData();
                    }
                }
            };
            toggleItem.Click += async (_, __) =>
            {
                var newStatus = category.IsActive != ActiveStatus.ACTIVE;
                var toggleResult = await AppServices.CategoryService.ToggleCategoryStatusAsync(category.CategoryId, newStatus);
                if (toggleResult.Success)
                {
                    await RefreshCategoriesData();
                }
                else
                {
                    MessageBox.Show(toggleResult.Message ?? "Thay đổi trạng thái thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            deleteItem.Click += async (_, __) =>
            {
                if (MessageBox.Show("Xác nhận xóa danh mục?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var deleteResult = await AppServices.CategoryService.DeleteCategoryAsync(category.CategoryId);
                    if (deleteResult.Success)
                    {
                        await RefreshCategoriesData();
                    }
                    else
                    {
                        MessageBox.Show(deleteResult.Message ?? "Xóa danh mục thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };
            menu.Items.AddRange(new ToolStripItem[] { editItem, toggleItem, deleteItem });
            var cellLocCat = dataGridViewCategories.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Location;
            var screenPoint = dataGridViewCategories.PointToScreen(new Point(cellLocCat.X + 20, cellLocCat.Y + 20));
            menu.Show(screenPoint);
        }
        #endregion
    }
}
