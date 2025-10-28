using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class ProductsControl : UserControl
    {
        private List<Product> products;
        private List<Variant> variants;
        private List<Category> categories;
        private string searchTerm = "";

        public ProductsControl()
        {
            InitializeComponent();
            LoadData();
            InitializeControls();
        }

        private void LoadData()
        {
            products = ProductsMockData.GetProducts();
            variants = ProductsMockData.GetVariants();
            categories = ProductsMockData.GetCategories();
        }

        private void InitializeControls()
        {
            // Khởi tạo DataGridViews trước
            InitializeDataGridViews();

            // Bind data
            BindProductsData();
            BindVariantsData();
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

            // Initialize Variants DataGridView
            dataGridViewVariants.Columns.Clear();
            dataGridViewVariants.Columns.Add("VariantSKU", "SKU");
            dataGridViewVariants.Columns.Add("VariantBarcode", "Barcode");
            dataGridViewVariants.Columns.Add("VariantName", "Tên variant");
            dataGridViewVariants.Columns.Add("VariantProduct", "Sản phẩm gốc");
            dataGridViewVariants.Columns.Add("VariantUnit", "Đơn vị");
            dataGridViewVariants.Columns.Add("VariantPrice", "Giá bán");
            dataGridViewVariants.Columns.Add("VariantStatus", "Trạng thái");
            dataGridViewVariants.Columns.Add("VariantAction", "Thao tác");
        }

        private void BindProductsData()
        {
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

        private void BindVariantsData()
        {
            var filteredVariants = variants.Where(variant =>
                variant.Name.ToLower().Contains(searchTerm.ToLower()) ||
                variant.SKU.ToLower().Contains(searchTerm.ToLower()) ||
                variant.Barcode?.Contains(searchTerm) == true
            ).ToList();

            dataGridViewVariants.Rows.Clear();
            foreach (var variant in filteredVariants)
            {
                dataGridViewVariants.Rows.Add(
                    variant.SKU,
                    variant.Barcode,
                    variant.Name,
                    GetProductName(variant.ProductId),
                    variant.Unit,
                    variant.Price.ToString("N0") + "₫",
                    variant.IsActive ? "Hoạt động" : "Ngưng",
                    "👁️"
                );
            }
        }

        private string GetCategoryName(int? categoryId)
        {
            if (!categoryId.HasValue) return "Chưa phân loại";
            return categories.FirstOrDefault(c => c.CategoryId == categoryId.Value)?.Name ?? "Chưa phân loại";
        }

        private string GetProductName(int productId)
        {
            return products.FirstOrDefault(p => p.ProductId == productId)?.Name ?? "N/A";
        }

        #region Event Handlers
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            searchTerm = txtSearch.Text;
            BindProductsData();
            BindVariantsData();
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Thêm sản phẩm mới", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Refresh data when switching tabs
            if (tabControl.SelectedTab == tabPageProducts)
            {
                BindProductsData();
            }
            else if (tabControl.SelectedTab == tabPageVariants)
            {
                BindVariantsData();
            }
        }

        private void dataGridViewProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridViewProducts.Columns["ProductAction"].Index)
            {
                var productCode = dataGridViewProducts.Rows[e.RowIndex].Cells["ProductCode"].Value.ToString();
                MessageBox.Show($"Xem chi tiết sản phẩm: {productCode}", "Chi tiết sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dataGridViewVariants_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridViewVariants.Columns["VariantAction"].Index)
            {
                var sku = dataGridViewVariants.Rows[e.RowIndex].Cells["VariantSKU"].Value.ToString();
                MessageBox.Show($"Xem chi tiết variant: {sku}", "Chi tiết variant", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

    #region Data Models
    public class Product
    {
        public int ProductId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? CategoryId { get; set; }
        public string UnitMeasure { get; set; }
        public decimal? BasePrice { get; set; }
        public string ProductType { get; set; }
        public bool IsActive { get; set; }
    }

    public class Variant
    {
        public int VariantId { get; set; }
        public string SKU { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public int ProductId { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }

    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }

    public static class ProductsMockData
    {
        public static List<Product> GetProducts()
        {
            return new List<Product>
        {
            new Product {
                ProductId = 1,
                Code = "SP001",
                Name = "Áo thun cotton",
                CategoryId = 1,
                UnitMeasure = "Cái",
                BasePrice = 150000,
                ProductType = "Vật lý",
                IsActive = true
            },
            new Product {
                ProductId = 2,
                Code = "SP002",
                Name = "Quần jeans",
                CategoryId = 2,
                UnitMeasure = "Cái",
                BasePrice = 350000,
                ProductType = "Vật lý",
                IsActive = true
            },
            new Product {
                ProductId = 3,
                Code = "SP003",
                Name = "Voucher giảm giá",
                CategoryId = null,
                UnitMeasure = "Lượt",
                BasePrice = 50000,
                ProductType = "Dịch vụ",
                IsActive = true
            }
        };
        }

        public static List<Variant> GetVariants()
        {
            return new List<Variant>
        {
            new Variant {
                VariantId = 1,
                SKU = "SP001-S",
                Barcode = "8936041234567",
                Name = "Áo thun cotton - Size S",
                ProductId = 1,
                Unit = "Cái",
                Price = 150000,
                IsActive = true
            },
            new Variant {
                VariantId = 2,
                SKU = "SP001-M",
                Barcode = "8936041234574",
                Name = "Áo thun cotton - Size M",
                ProductId = 1,
                Unit = "Cái",
                Price = 150000,
                IsActive = true
            },
            new Variant {
                VariantId = 3,
                SKU = "SP002-32",
                Barcode = "8936041234581",
                Name = "Quần jeans - Size 32",
                ProductId = 2,
                Unit = "Cái",
                Price = 350000,
                IsActive = true
            }
        };
        }

        public static List<Category> GetCategories()
        {
            return new List<Category>
        {
            new Category { CategoryId = 1, Name = "Áo" },
            new Category { CategoryId = 2, Name = "Quần" },
            new Category { CategoryId = 3, Name = "Phụ kiện" }
        };
        }
    }
    #endregion
}