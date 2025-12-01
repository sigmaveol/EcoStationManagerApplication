using Dapper;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.DAL.SqlQueries;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{
    public class InventoryRepository : BaseRepository<Inventory>, IInventoryRepository
    {
        public InventoryRepository(IDatabaseHelper databaseHelper)
            : base(databaseHelper, "Inventories", "inventory_id")
        { }

        public async Task<Inventory> GetByProductAndBatchAsync(int productId, string batchNo)
        {
            try
            {
                var sql = "SELECT * FROM Inventories WHERE product_id = @ProductId AND batch_no = @BatchNo";
                return await _databaseHelper.QueryFirstOrDefaultAsync<Inventory>(sql, new
                {
                    ProductId = productId,
                    BatchNo = batchNo
                });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByProductAndBatchAsync error - ProductId: {productId}, Batch: {batchNo} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Inventory>> GetByProductAsync(int productId)
        {
            try
            {
                var sql = "SELECT * FROM Inventories WHERE product_id = @ProductId ORDER BY expiry_date ASC";
                return await _databaseHelper.QueryAsync<Inventory>(sql, new { ProductId = productId });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByProductAsync error - ProductId: {productId} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Inventory>> GetLowStockItemsAsync()
        {
            try
            {
                return await _databaseHelper.QueryAsync<Inventory>(InventoryQueries.GetLowStockItems);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetLowStockItemsAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Inventory>> GetExpiringItemsAsync(int daysThreshold = 15)
        {
            try
            {
                return await _databaseHelper.QueryAsync<Inventory>(InventoryQueries.GetExpiringItems, new { DaysThreshold = daysThreshold });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetExpiringItemsAsync error - DaysThreshold: {daysThreshold} - {ex.Message}");
                throw;
            }
        }

        public async Task<decimal> GetTotalStockQuantityAsync(int productId)
        {
            try
            {
                var sql = "SELECT COALESCE(SUM(quantity), 0) FROM Inventories WHERE product_id = @ProductId";
                return await _databaseHelper.ExecuteScalarAsync<decimal>(sql, new { ProductId = productId });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetTotalStockQuantityAsync error - ProductId: {productId} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Inventory>> GetInventoryHistoryAsync(int productId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var sql = @"SELECT * FROM Inventories 
                           WHERE product_id = @ProductId";

                var parameters = new DynamicParameters();
                parameters.Add("ProductId", productId);

                if (fromDate.HasValue)
                {
                    sql += " AND last_updated >= @FromDate";
                    parameters.Add("FromDate", fromDate.Value);
                }

                if (toDate.HasValue)
                {
                    sql += " AND last_updated <= @ToDate";
                    parameters.Add("ToDate", toDate.Value);
                }

                sql += " ORDER BY last_updated DESC";

                return await _databaseHelper.QueryAsync<Inventory>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetInventoryHistoryAsync error - ProductId: {productId} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> AddStockAsync(int productId, string batchNo, decimal quantity, DateTime? expiryDate)
        {
            try
            {
                // Kiểm tra xem đã có inventory cho product và batch này chưa
                var existing = await GetByProductAndBatchAsync(productId, batchNo);

                if (existing != null)
                {
                    // Cập nhật số lượng nếu đã tồn tại
                    var newQuantity = existing.Quantity + quantity;
                    return await UpdateStockQuantityAsync(existing.InventoryId, newQuantity);
                }
                else
                {
                    // Thêm mới nếu chưa tồn tại
                    var sql = @"
                        INSERT INTO Inventories (batch_no, product_id, quantity, expiry_date)
                        VALUES (@BatchNo, @ProductId, @Quantity, @ExpiryDate)";

                    var affectedRows = await _databaseHelper.ExecuteAsync(sql, new
                    {
                        BatchNo = batchNo,
                        ProductId = productId,
                        Quantity = quantity,
                        ExpiryDate = expiryDate
                    });

                    return affectedRows > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"AddStockAsync error - ProductId: {productId}, Batch: {batchNo}, Quantity: {quantity} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateStockQuantityAsync(int inventoryId, decimal newQuantity)
        {
            try
            {
                var sql = "UPDATE Inventories SET quantity = @NewQuantity WHERE inventory_id = @InventoryId";
                var affectedRows = await _databaseHelper.ExecuteAsync(sql, new
                {
                    InventoryId = inventoryId,
                    NewQuantity = newQuantity
                });

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"UpdateStockQuantityAsync error - InventoryId: {inventoryId}, Quantity: {newQuantity} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ReduceStockAsync(int productId, string batchNo, decimal quantity)
        {
            try
            {
                var inventory = await GetByProductAndBatchAsync(productId, batchNo);
                if (inventory == null)
                {
                    _logger.Warning($"ReduceStockAsync - Inventory not found for ProductId: {productId}, Batch: {batchNo}");
                    return false;
                }

                if (inventory.Quantity < quantity)
                {
                    _logger.Warning($"ReduceStockAsync - Insufficient stock. Available: {inventory.Quantity}, Required: {quantity}");
                    return false;
                }

                var newQuantity = inventory.Quantity - quantity;
                return await UpdateStockQuantityAsync(inventory.InventoryId, newQuantity);
            }
            catch (Exception ex)
            {
                _logger.Error($"ReduceStockAsync error - ProductId: {productId}, Batch: {batchNo}, Quantity: {quantity} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> IsStockSufficientAsync(int productId, decimal requiredQuantity)
        {
            try
            {
                var totalStock = await GetTotalStockQuantityAsync(productId);
                return totalStock >= requiredQuantity;
            }
            catch (Exception ex)
            {
                _logger.Error($"IsStockSufficientAsync error - ProductId: {productId}, Required: {requiredQuantity} - {ex.Message}");
                throw;
            }
        }

        public async Task<(IEnumerable<Inventory> Inventories, int TotalCount)> GetPagedInventoriesAsync(
            int pageNumber,
            int pageSize,
            string search = null,
            bool includeExpired = false)
        {
            try
            {
                var whereClause = "WHERE 1=1";
                var parameters = new DynamicParameters();

                // Search condition
                if (!string.IsNullOrWhiteSpace(search))
                {
                    whereClause += @" AND (p.name LIKE @Search OR i.batch_no LIKE @Search)";
                    parameters.Add("Search", $"%{search}%");
                }

                // Expired items filter
                if (!includeExpired)
                {
                    whereClause += " AND (i.expiry_date IS NULL OR i.expiry_date >= CURDATE())";
                }

                // Get total count
                var countSql = $@"
                    SELECT COUNT(*) 
                    FROM Inventories i
                    JOIN Products p ON i.product_id = p.product_id
                    {whereClause}";

                var totalCount = await _databaseHelper.ExecuteScalarAsync<int>(countSql, parameters);

                // Get paged data
                var sql = $@"
                    SELECT i.*, p.name as product_name, p.sku, p.min_stock_level
                    FROM Inventories i
                    JOIN Products p ON i.product_id = p.product_id
                    {whereClause}
                    ORDER BY p.name ASC, i.expiry_date ASC
                    LIMIT @PageSize OFFSET @Offset";

                parameters.Add("PageSize", pageSize);
                parameters.Add("Offset", (pageNumber - 1) * pageSize);

                var inventories = await _databaseHelper.QueryAsync<Inventory>(sql, parameters);
                return (inventories, totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetPagedInventoriesAsync error - Page: {pageNumber}, Size: {pageSize} - {ex.Message}");
                throw;
            }
        }
    }
}
