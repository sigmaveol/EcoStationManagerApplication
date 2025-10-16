using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.DAL.Database;
using EcoStationManagerApplication.Core.Interfaces;

namespace EcoStationManagerApplication.DAL.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
         
        public ProductRepository(DatabaseHelper dbHelper) : base(dbHelper) { }

        protected override string TableName => "Products";
        protected override string PrimaryKeyName => "productID";
        protected override bool HasIsActive => true;

        public async Task<Product> GetByCodeAsync(string productCode)
        {

            var sql = "SELECT * FROM Products WHERE product_code = @ProductCode";
            return await _dbHelper.QueryFirstOrDefaultAsync<Product>(sql, new { ProductCode = productCode });

        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        {
            var sql = "SELECT * FROM Products WHERE categoryID = @CategoryId AND is_active = true ORDER BY name";
            return await _dbHelper.QueryAsync<Product>(sql, new { CategoryId = categoryId });
        }

        public async Task<IEnumerable<Product>> SearchAsync(string keyword)
        {
            var sql = @"SELECT * FROM Products
                        WHERE (name LIKE @Keyword OR description LIKE @Keyword)
                        AND is_active = true
                        ORDER BY name";
            return await _dbHelper.QueryAsync<Product>(sql, new { Keyword = $"%{keyword}%" });
        }

        //public async Task<bool> UpdateStockAsync(int productId, decimal newStock)
        //{
        //    var sql = "UPDATE Products SET stock_quantity = @StockQuantity WHERE productID = @ProductId";
        //    var affectedRows = await _dbHelper.ExecuteAsync(sql, new
        //    {
        //        StockQuantity = newStock,
        //        ProductId = productId
        //    });
        //    return affectedRows > 0;
        //}

        //public async Task<IEnumerable<Product>> GetLowStockProductsAsync(decimal threshold)
        //{
        //    var sql = @"SELECT * FROM Products 
        //               WHERE stock_quantity <= @Threshold 
        //               AND is_active = true 
        //               ORDER BY stock_quantity ASC";

        //    return await _dbHelper.QueryAsync<Product>(sql, new { Threshold = threshold });
        //}

        public async Task<IEnumerable<Product>> GetActiveProductsWithVariantsAsync()
        {
            var sql = @"SELECT p.*, v.* 
                       FROM Products p 
                       LEFT JOIN Variants v ON p.productID = v.productID 
                       WHERE p.is_active = true 
                       ORDER BY p.name";

            // Sử dụng QueryMultiple hoặc separate queries
            return await _dbHelper.QueryAsync<Product>(sql);
        }

        public async Task<bool> DeactivateProductAsync(int productId)
        {
            var sql = "UPDATE Products SET is_active = false WHERE productID = @ProductId";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new { ProductId = productId });
            return affectedRows > 0;
        }

        public async Task<bool> ActivateProductAsync(int productId)
        {
            var sql = "UPDATE Products SET is_active = true WHERE productID = @ProductId";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new { ProductId = productId });
            return affectedRows > 0;
        }

        public override async Task<int> CreateAsync(Product product)
        {
            var sql = @"
                INSERT INTO Products (name, base_price, unit_measure, description, is_active, categoryID) 
                VALUES (@Name, @BasePrice, @UnitMeasure, @Description, @IsActive, @CategoryID);
                SELECT LAST_INSERT_ID();";

            return await _dbHelper.ExecuteScalarAsync<int>(sql, product);
        }

        public override async Task<bool> UpdateAsync(Product product)
        {
            var sql = @"
                UPDATE Products SET 
                    name = @Name,
                    base_price = @BasePrice,
                    unit_measure = @UnitMeasure,
                    description = @Description,
                    is_active = @IsActive,
                    categoryID = @CategoryID
                WHERE productID = @ProductID";

            var affectedRows = await _dbHelper.ExecuteAsync(sql, product);
            return affectedRows > 0;
        }


    }
}
