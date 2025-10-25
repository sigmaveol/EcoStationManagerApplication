using EcoStationManagerApplication.Core.Interfaces.IRepositories;
using EcoStationManagerApplication.DAL.Database;
using EcoStationManagerApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{
    public class VariantRepository : BaseRepository<Variant>, IVariantRepository
    {
        public VariantRepository(DatabaseHelper dbHelper) : base(dbHelper) { }
        protected override string TableName => "Variants";
        protected override string PrimaryKeyName => "variantID";
        protected override bool HasIsActive => true;

        public async Task<Variant> GetByBarcodeAsync(string barcode)
        {
            var sql = "SELECT * FROM Variants WHERE barcode = @Barcode";
            return await _dbHelper.QueryFirstOrDefaultAsync<Variant>(sql, new { Barcode = barcode });
        }

        public async Task<Variant> GetBySKUAsync(string sku)
        {
            var sql = "SELECT * FROM Variants WHERE SKU = @SKU";
            return await _dbHelper.QueryFirstOrDefaultAsync<Variant>(sql, new { SKU = sku });
        }

        public async Task<IEnumerable<Variant>> GetByProductAsync(int productId)
        {
            var sql = "SELECT * FROM Variants WHERE productID = @ProductId AND is_active = true ORDER BY name";
            return await _dbHelper.QueryAsync<Variant>(sql, new { ProductId = productId });
        }

        public async Task<IEnumerable<Variant>> GetActiveVariantsAsync()
        {
            var sql = "SELECT * FROM Variants WHERE is_active = true ORDER BY name";
            return await _dbHelper.QueryAsync<Variant>(sql);
        }

        public async Task<bool> UpdateStockAsync(int variantId, decimal newStock)
        {
            var sql = "UPDATE Variants SET current_stock = @Stock WHERE variantID = @VariantId";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new
            {
                Stock = newStock,
                VariantId = variantId
            });
            return affectedRows > 0;
        }


    }
}
