using Dapper;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.DAL.SqlQueries;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {

        public ProductRepository(IDatabaseHelper databaseHelper) 
            : base(databaseHelper, "Products", "product_id")
        { }

        public async Task<Product> GetBySkuAsync(string sku)
        {
            try
            {
                var sql = "SELECT * FROM Products WHERE sku = @Sku";
                return await _databaseHelper.QueryFirstOrDefaultAsync<Product>(sql, new { Sku = sku });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetBySkuAsync error - SKU: {sku} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        {
            try
            {
                var sql = "SELECT * FROM Products WHERE category_id = @CategoryId AND is_active = TRUE ORDER BY name";
                return await _databaseHelper.QueryAsync<Product>(sql, new { CategoryId = categoryId });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByCategoryAsync error - CategoryId: {categoryId} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Product>> GetByTypeAsync(ProductType? productType)
        {
            try
            {
                var sql = "SELECT * FROM Products WHERE product_type = @ProductType AND is_active = TRUE ORDER BY name";
                return await _databaseHelper.QueryAsync<Product>(sql, new { ProductType = productType });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByTypeAsync error - ProductType: {productType} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Product>> GetActiveProductsAsync()
        {
            try
            {
                var sql = "SELECT * FROM Products WHERE is_active = TRUE ORDER BY name";
                return await _databaseHelper.QueryAsync<Product>(sql);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetActiveProductsAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync()
        {
            try
            {
                return await _databaseHelper.QueryAsync<Product>(ProductQueries.GetLowStockProducts);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetLowStockProductsAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Product>> SearchAsync(string keyword, ProductType? productType = null)
        {
            try
            {
                var sql = @"SELECT * FROM Products 
                           WHERE (name LIKE @Keyword OR sku LIKE @Keyword)";

                var parameters = new DynamicParameters();
                parameters.Add("Keyword", $"%{keyword}%");

                if (productType.HasValue)
                {
                    sql += " AND product_type = @ProductType";
                    parameters.Add("ProductType", productType);
                }

                sql += " ORDER BY name";

                return await _databaseHelper.QueryAsync<Product>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.Error($"SearchAsync error - Keyword: {keyword}, Type: {productType} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> IsSkuExistsAsync(string sku, int? excludeProductId = null)
        {
            try
            {
                var sql = "SELECT 1 FROM Products WHERE sku = @Sku";
                var parameters = new DynamicParameters();
                parameters.Add("Sku", sku);

                if (excludeProductId.HasValue)
                {
                    sql += " AND product_id != @ExcludeProductId";
                    parameters.Add("ExcludeProductId", excludeProductId.Value);
                }

                var result = await _databaseHelper.ExecuteScalarAsync<int?>(sql, parameters);
                return result.HasValue;
            }
            catch (Exception ex)
            {
                _logger.Error($"IsSkuExistsAsync error - SKU: {sku}, ExcludeId: {excludeProductId} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdatePriceAsync(int productId, decimal newPrice)
        {
            try
            {
                var sql = "UPDATE Products SET price = @NewPrice WHERE product_id = @ProductId";
                var affectedRows = await _databaseHelper.ExecuteAsync(sql, new
                {
                    ProductId = productId,
                    NewPrice = newPrice
                });

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"UpdatePriceAsync error - ProductId: {productId}, Price: {newPrice} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateUnitAsync(int productId, string unit)
        {
            try
            {
                var sql = "UPDATE Products SET unit = @Unit WHERE product_id = @ProductId";
                var affectedRows = await _databaseHelper.ExecuteAsync(sql, new
                {
                    ProductId = productId,
                    Unit = unit
                });

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"UpdateUnitAsync error - ProductId: {productId}, Unit: {unit} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateMinStockLevelAsync(int productId, decimal minStockLevel)
        {
            try
            {
                var sql = "UPDATE Products SET min_stock_level = @MinStockLevel WHERE product_id = @ProductId";
                var affectedRows = await _databaseHelper.ExecuteAsync(sql, new
                {
                    ProductId = productId,
                    MinStockLevel = minStockLevel
                });

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"UpdateMinStockLevelAsync error - ProductId: {productId}, MinStock: {minStockLevel} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ToggleActiveAsync(int productId, bool isActive)
        {
            try
            {
                var sql = "UPDATE Products SET is_active = @IsActive WHERE product_id = @ProductId";
                var affectedRows = await _databaseHelper.ExecuteAsync(sql, new
                {
                    ProductId = productId,
                    IsActive = isActive
                });

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"ToggleActiveAsync error - ProductId: {productId}, IsActive: {isActive} - {ex.Message}");
                throw;
            }
        }

        public async Task<(IEnumerable<Product> Products, int TotalCount)> GetPagedProductsAsync(
            int pageNumber,
            int pageSize,
            string searchKeyword = null,
            int? categoryId = null,
            string productType = null,
            bool? isActive = null)
        {
            try
            {
                var whereClause = "WHERE 1=1";
                var parameters = new DynamicParameters();

                // Search condition
                if (!string.IsNullOrWhiteSpace(searchKeyword))
                {
                    whereClause += " AND (p.name LIKE @SearchKeyword OR p.sku LIKE @SearchKeyword)";
                    parameters.Add("SearchKeyword", $"%{searchKeyword}%");
                }

                // Category filter
                if (categoryId.HasValue)
                {
                    whereClause += " AND p.category_id = @CategoryId";
                    parameters.Add("CategoryId", categoryId.Value);
                }

                // Product type filter
                if (!string.IsNullOrWhiteSpace(productType))
                {
                    whereClause += " AND p.product_type = @ProductType";
                    parameters.Add("ProductType", productType);
                }

                // Active status filter
                if (isActive.HasValue)
                {
                    whereClause += " AND p.is_active = @IsActive";
                    parameters.Add("IsActive", isActive.Value);
                }

                // Get total count
                var countSql = $"SELECT COUNT(*) FROM Products p {whereClause}";
                var totalCount = await _databaseHelper.ExecuteScalarAsync<int>(countSql, parameters);

                // Get paged data
                var sql = $@"
                    SELECT p.*, c.name as category_name 
                    FROM Products p
                    LEFT JOIN Categories c ON p.category_id = c.category_id
                    {whereClause}
                    ORDER BY p.name ASC
                    LIMIT @PageSize OFFSET @Offset";

                parameters.Add("PageSize", pageSize);
                parameters.Add("Offset", (pageNumber - 1) * pageSize);

                var products = await _databaseHelper.QueryAsync<Product>(sql, parameters);
                return (products, totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetPagedProductsAsync error - Page: {pageNumber}, Size: {pageSize} - {ex.Message}");
                throw;
            }
        }
    
    }
}
