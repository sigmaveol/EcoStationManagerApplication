using Dapper;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Database;
using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{
    // ==================== PRODUCT REPOSITORY IMPLEMENTATION ====================
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository() : base("Products", "product_id", true) { }
        public ProductRepository(IDbHelper dbHelper) : base(dbHelper, "Products", "product_id", true) { }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            var sql = @"SELECT p.*, c.name as CategoryName 
                       FROM Products p 
                       LEFT JOIN Categories c ON p.category_id = c.category_id 
                       WHERE p.category_id = @CategoryId AND p.is_active = 1 
                       ORDER BY p.name";
            return await _dbHelper.QueryAsync<Product>(sql, new { CategoryId = categoryId });
        }

        public async Task<Product> GetProductWithVariantsAsync(int productId)
        {
            var sql = @"SELECT p.*, v.* 
                       FROM Products p 
                       LEFT JOIN Variants v ON p.product_id = v.product_id 
                       WHERE p.product_id = @ProductId AND p.is_active = 1 AND v.is_active = 1";

            using (var multi = await _dbHelper.QueryMultipleAsync(sql, new { ProductId = productId }))
            {
                var product = await multi.ReadFirstOrDefaultAsync<Product>();
                if (product != null)
                {
                    var variants = await multi.ReadAsync<Variant>();
                    // Variants will be available in the result
                }
                return product;
            }
        }

        public async Task<IEnumerable<Product>> GetActiveProductsAsync()
        {
            var sql = @"SELECT p.*, c.name as CategoryName 
                       FROM Products p 
                       LEFT JOIN Categories c ON p.category_id = c.category_id 
                       WHERE p.is_active = 1 
                       ORDER BY p.name";
            return await _dbHelper.QueryAsync<Product>(sql);
        }

        public async Task<Product> GetProductByCodeAsync(string code)
        {
            var sql = "SELECT * FROM Products WHERE code = @Code AND is_active = 1";
            return await _dbHelper.QueryFirstOrDefaultAsync<Product>(sql, new { Code = code });
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string keyword)
        {
            var sql = @"SELECT p.*, c.name as CategoryName 
                       FROM Products p 
                       LEFT JOIN Categories c ON p.category_id = c.category_id 
                       WHERE (p.name LIKE @Keyword OR p.code LIKE @Keyword OR p.description LIKE @Keyword)
                       AND p.is_active = 1 
                       ORDER BY p.name";
            return await _dbHelper.QueryAsync<Product>(sql, new { Keyword = $"%{keyword}%" });
        }

        public async Task<IEnumerable<Product>> GetProductsWithInventoryAsync(int stationId)
        {
            var sql = @"SELECT DISTINCT p.*, c.name as CategoryName, i.current_stock
                       FROM Products p
                       LEFT JOIN Categories c ON p.category_id = c.category_id
                       LEFT JOIN Variants v ON p.product_id = v.product_id
                       LEFT JOIN Inventories i ON v.variant_id = i.variant_id AND i.station_id = @StationId
                       WHERE p.is_active = 1
                       ORDER BY p.name";
            return await _dbHelper.QueryAsync<Product>(sql, new { StationId = stationId });
        }

        public async Task<IEnumerable<Product>> GetProductsLowInStockAsync(int stationId)
        {
            var sql = @"SELECT DISTINCT p.*, c.name as CategoryName
                       FROM Products p
                       LEFT JOIN Categories c ON p.category_id = c.category_id
                       LEFT JOIN Variants v ON p.product_id = v.product_id
                       LEFT JOIN Inventories i ON v.variant_id = i.variant_id AND i.station_id = @StationId
                       WHERE p.is_active = 1 
                       AND i.current_stock <= 15
                       ORDER BY p.name";
            return await _dbHelper.QueryAsync<Product>(sql, new { StationId = stationId });
        }
    }

    // ==================== CATEGORY REPOSITORY IMPLEMENTATION ====================
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository() : base("Categories", "category_id", true) { }
        public CategoryRepository(IDbHelper dbHelper) : base(dbHelper, "Categories", "category_id", true) { }

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            var sql = "SELECT * FROM Categories WHERE is_active = 1 ORDER BY sort_order, name";
            return await _dbHelper.QueryAsync<Category>(sql);
        }

        public async Task<IEnumerable<Category>> GetCategoriesByTypeAsync(string categoryType)
        {
            var sql = "SELECT * FROM Categories WHERE category_type = @CategoryType AND is_active = 1 ORDER BY sort_order, name";
            return await _dbHelper.QueryAsync<Category>(sql, new { CategoryType = categoryType });
        }

        public async Task<IEnumerable<Category>> GetChildCategoriesAsync(int parentId)
        {
            var sql = "SELECT * FROM Categories WHERE parent_id = @ParentId AND is_active = 1 ORDER BY sort_order, name";
            return await _dbHelper.QueryAsync<Category>(sql, new { ParentId = parentId });
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithProductsAsync()
        {
            var sql = @"SELECT c.*, COUNT(p.product_id) as ProductCount
                       FROM Categories c
                       LEFT JOIN Products p ON c.category_id = p.category_id AND p.is_active = 1
                       WHERE c.is_active = 1
                       GROUP BY c.category_id, c.name, c.image, c.description, c.category_type, c.parent_id, c.sort_order, c.created_date, c.updated_date, c.is_active
                       ORDER BY c.sort_order, c.name";
            return await _dbHelper.QueryAsync<Category>(sql);
        }

        public async Task<Category> GetCategoryByNameAsync(string name)
        {
            var sql = "SELECT * FROM Categories WHERE name = @Name AND is_active = 1";
            return await _dbHelper.QueryFirstOrDefaultAsync<Category>(sql, new { Name = name });
        }
    }

    // ==================== VARIANT REPOSITORY IMPLEMENTATION ====================
    public class VariantRepository : BaseRepository<Variant>, IVariantRepository
    {
        public VariantRepository() : base("Variants", "variant_id", true) { }
        public VariantRepository(IDbHelper dbHelper) : base(dbHelper, "Variants", "variant_id", true) { }

        public async Task<IEnumerable<Variant>> GetVariantsByProductAsync(int productId)
        {
            var sql = @"SELECT v.*, p.name as ProductName 
                       FROM Variants v 
                       LEFT JOIN Products p ON v.product_id = p.product_id 
                       WHERE v.product_id = @ProductId AND v.is_active = 1 
                       ORDER BY v.name";
            return await _dbHelper.QueryAsync<Variant>(sql, new { ProductId = productId });
        }

        public async Task<Variant> GetVariantBySKUAsync(string sku)
        {
            var sql = "SELECT * FROM Variants WHERE SKU = @SKU AND is_active = 1";
            return await _dbHelper.QueryFirstOrDefaultAsync<Variant>(sql, new { SKU = sku });
        }

        public async Task<Variant> GetVariantByBarcodeAsync(string barcode)
        {
            var sql = "SELECT * FROM Variants WHERE barcode = @Barcode AND is_active = 1";
            return await _dbHelper.QueryFirstOrDefaultAsync<Variant>(sql, new { Barcode = barcode });
        }

        public async Task<Variant> GetVariantWithProductAsync(int variantId)
        {
            var sql = @"SELECT v.*, p.name as ProductName, p.code as ProductCode
                       FROM Variants v
                       LEFT JOIN Products p ON v.product_id = p.product_id
                       WHERE v.variant_id = @VariantId AND v.is_active = 1";
            return await _dbHelper.QueryFirstOrDefaultAsync<Variant>(sql, new { VariantId = variantId });
        }

        public async Task<IEnumerable<Variant>> GetActiveVariantsAsync()
        {
            var sql = @"SELECT v.*, p.name as ProductName 
                       FROM Variants v 
                       LEFT JOIN Products p ON v.product_id = p.product_id 
                       WHERE v.is_active = 1 AND p.is_active = 1 
                       ORDER BY p.name, v.name";
            return await _dbHelper.QueryAsync<Variant>(sql);
        }

        public async Task<IEnumerable<Variant>> GetVariantsLowInStockAsync(int stationId)
        {
            var sql = @"SELECT v.*, p.name as ProductName, i.current_stock
                       FROM Variants v
                       LEFT JOIN Products p ON v.product_id = p.product_id
                       LEFT JOIN Inventories i ON v.variant_id = i.variant_id AND i.station_id = @StationId
                       WHERE v.is_active = 1 
                       AND i.current_stock <= 15
                       ORDER BY p.name, v.name";
            return await _dbHelper.QueryAsync<Variant>(sql, new { StationId = stationId });
        }

        public async Task<IEnumerable<Variant>> GetVariantsByCategoryAsync(int categoryId)
        {
            var sql = @"SELECT v.*, p.name as ProductName
                       FROM Variants v
                       LEFT JOIN Products p ON v.product_id = p.product_id
                       WHERE p.category_id = @CategoryId AND v.is_active = 1 AND p.is_active = 1
                       ORDER BY p.name, v.name";
            return await _dbHelper.QueryAsync<Variant>(sql, new { CategoryId = categoryId });
        }
    }

    // ==================== VARIANT TYPE REPOSITORY IMPLEMENTATION ====================
    public class VariantTypeRepository : BaseRepository<VariantType>, IVariantTypeRepository
    {
        public VariantTypeRepository() : base("VariantTypes", "type_id", true) { }
        public VariantTypeRepository(IDbHelper dbHelper) : base(dbHelper, "VariantTypes", "type_id", true) { }

        public async Task<IEnumerable<VariantType>> GetActiveVariantTypesAsync()
        {
            var sql = "SELECT * FROM VariantTypes WHERE is_active = 1 ORDER BY name";
            return await _dbHelper.QueryAsync<VariantType>(sql);
        }

        public async Task<VariantType> GetVariantTypeByNameAsync(string name)
        {
            var sql = "SELECT * FROM VariantTypes WHERE name = @Name AND is_active = 1";
            return await _dbHelper.QueryFirstOrDefaultAsync<VariantType>(sql, new { Name = name });
        }
    }

    // ==================== COMBO REPOSITORY IMPLEMENTATION ====================
    public class ComboRepository : BaseRepository<Combo>, IComboRepository
    {
        public ComboRepository() : base("Combos", "combo_id", true) { }
        public ComboRepository(IDbHelper dbHelper) : base(dbHelper, "Combos", "combo_id", true) { }

        public async Task<Combo> GetComboWithItemsAsync(int comboId)
        {
            var sql = @"SELECT c.*, ci.*, v.* 
                       FROM Combos c 
                       LEFT JOIN ComboItems ci ON c.combo_id = ci.combo_id 
                       LEFT JOIN Variants v ON ci.variant_id = v.variant_id 
                       WHERE c.combo_id = @ComboId AND c.is_active = 1";

            using (var multi = await _dbHelper.QueryMultipleAsync(sql, new { ComboId = comboId }))
            {
                var combo = await multi.ReadFirstOrDefaultAsync<Combo>();
                if (combo != null)
                {
                    var items = await multi.ReadAsync<ComboItem>();
                    // Items will be available in the result
                }
                return combo;
            }
        }

        public async Task<IEnumerable<Combo>> GetActiveCombosAsync()
        {
            var sql = "SELECT * FROM Combos WHERE is_active = 1 ORDER BY name";
            return await _dbHelper.QueryAsync<Combo>(sql);
        }

        public async Task<Combo> GetComboByCodeAsync(string code)
        {
            var sql = "SELECT * FROM Combos WHERE code = @Code AND is_active = 1";
            return await _dbHelper.QueryFirstOrDefaultAsync<Combo>(sql, new { Code = code });
        }

        public async Task<IEnumerable<Combo>> GetCombosByCategoryAsync(int categoryId)
        {
            var sql = @"SELECT c.* 
                       FROM Combos c
                       LEFT JOIN Products p ON c.combo_id = p.product_id
                       WHERE p.category_id = @CategoryId AND c.is_active = 1
                       ORDER BY c.name";
            return await _dbHelper.QueryAsync<Combo>(sql, new { CategoryId = categoryId });
        }

        public async Task<IEnumerable<Combo>> GetCombosWithItemsAsync()
        {
            var sql = @"SELECT c.*, COUNT(ci.variant_id) as ItemCount
                       FROM Combos c
                       LEFT JOIN ComboItems ci ON c.combo_id = ci.combo_id
                       WHERE c.is_active = 1
                       GROUP BY c.combo_id, c.code, c.name, c.image, c.description, c.total_price, c.is_active, c.created_date, c.updated_date
                       ORDER BY c.name";
            return await _dbHelper.QueryAsync<Combo>(sql);
        }
    }

    // ==================== COMBO ITEM REPOSITORY IMPLEMENTATION ====================
    public class ComboItemRepository : BaseRepository<ComboItem>, IComboItemRepository
    {
        public ComboItemRepository() : base("ComboItems", "combo_id", false) { }
        public ComboItemRepository(IDbHelper dbHelper) : base(dbHelper, "ComboItems", "combo_id", false) { }

        public async Task<IEnumerable<ComboItem>> GetItemsByComboAsync(int comboId)
        {
            var sql = @"SELECT ci.*, v.name as VariantName, v.sku as VariantSKU, v.price as VariantPrice
                       FROM ComboItems ci 
                       LEFT JOIN Variants v ON ci.variant_id = v.variant_id 
                       WHERE ci.combo_id = @ComboId";
            return await _dbHelper.QueryAsync<ComboItem>(sql, new { ComboId = comboId });
        }

        public async Task<bool> DeleteItemsByComboAsync(int comboId)
        {
            var sql = "DELETE FROM ComboItems WHERE combo_id = @ComboId";
            var result = await _dbHelper.ExecuteAsync(sql, new { ComboId = comboId });
            return result > 0;
        }

        public async Task<bool> UpdateComboItemQuantityAsync(int comboId, int variantId, int quantity)
        {
            var sql = "UPDATE ComboItems SET quantity = @Quantity WHERE combo_id = @ComboId AND variant_id = @VariantId";
            var result = await _dbHelper.ExecuteAsync(sql, new { ComboId = comboId, VariantId = variantId, Quantity = quantity });
            return result > 0;
        }

        public async Task<IEnumerable<ComboItem>> GetCombosContainingVariantAsync(int variantId)
        {
            var sql = @"SELECT ci.*, c.name as ComboName
                       FROM ComboItems ci
                       LEFT JOIN Combos c ON ci.combo_id = c.combo_id
                       WHERE ci.variant_id = @VariantId AND c.is_active = 1";
            return await _dbHelper.QueryAsync<ComboItem>(sql, new { VariantId = variantId });
        }

        public async Task<IEnumerable<ComboItem>> GetItemsWithVariantsAsync(int comboId)
        {
            var sql = @"SELECT ci.*, v.name as VariantName, v.sku as VariantSKU, v.price as VariantPrice, p.name as ProductName
                       FROM ComboItems ci
                       LEFT JOIN Variants v ON ci.variant_id = v.variant_id
                       LEFT JOIN Products p ON v.product_id = p.product_id
                       WHERE ci.combo_id = @ComboId";
            return await _dbHelper.QueryAsync<ComboItem>(sql, new { ComboId = comboId });
        }
    }
}