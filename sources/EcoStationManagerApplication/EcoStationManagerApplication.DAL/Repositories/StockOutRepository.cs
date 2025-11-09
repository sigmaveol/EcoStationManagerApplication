using Dapper;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.DAL.SqlQueries;
using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{
    public class StockOutRepository : BaseRepository<StockOut>, IStockOutRepository
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IPackagingInventoryRepository _packagingInventoryRepository;

        public StockOutRepository(IDatabaseHelper databaseHelper, IInventoryRepository inventoryRepository)
            : base(databaseHelper, "StockOut", "stockout_id")
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<IEnumerable<StockOut>> GetByProductAsync(int productId)
        {
            try
            {
                return await _databaseHelper.QueryAsync<StockOut>(
                    StockOutQueries.GetByProduct,
                    new { ProductId = productId }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByProductAsync error - ProductId: {productId} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<StockOut>> GetByReferenceAsync(RefType refType, int refId)
        {
            try
            {
                return await _databaseHelper.QueryAsync<StockOut>(
                    StockOutQueries.GetByReference,
                    new { RefType = refType.ToString(), RefId = refId }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByReferenceAsync error - RefType: {refType}, RefId: {refId} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<StockOut>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _databaseHelper.QueryAsync<StockOut>(
                    StockOutQueries.GetByDateRange,
                    new { StartDate = startDate, EndDate = endDate }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByDateRangeAsync error - Start: {startDate}, End: {endDate} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<StockOut>> GetByPurposeAsync(StockOutPurpose purpose)
        {
            try
            {
                return await _databaseHelper.QueryAsync<StockOut>(
                    StockOutQueries.GetByPurpose,
                    new { Purpose = purpose.ToString() }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByPurposeAsync error - Purpose: {purpose} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<StockOut>> GetByBatchAsync(string batchNo)
        {
            try
            {
                return await _databaseHelper.QueryAsync<StockOut>(
                    StockOutQueries.GetByBatch,
                    new { BatchNo = batchNo }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByBatchAsync error - BatchNo: {batchNo} - {ex.Message}");
                throw;
            }
        }

        public async Task<decimal> GetTotalStockOutValueAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var sql = StockOutQueries.GetTotalStockOutValue;
                var parameters = new DynamicParameters();

                if (startDate.HasValue && endDate.HasValue)
                {
                    sql += " AND so.created_date BETWEEN @StartDate AND @EndDate";
                    parameters.Add("StartDate", startDate.Value);
                    parameters.Add("EndDate", endDate.Value);
                }

                return await _databaseHelper.ExecuteScalarAsync<decimal>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetTotalStockOutValueAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<decimal> GetTotalQuantityByProductAsync(int productId, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var sql = StockOutQueries.GetTotalQuantityByProduct;
                var parameters = new DynamicParameters();
                parameters.Add("ProductId", productId);

                if (startDate.HasValue && endDate.HasValue)
                {
                    sql += " AND so.created_date BETWEEN @StartDate AND @EndDate";
                    parameters.Add("StartDate", startDate.Value);
                    parameters.Add("EndDate", endDate.Value);
                }

                return await _databaseHelper.ExecuteScalarAsync<decimal>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetTotalQuantityByProductAsync error - ProductId: {productId} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<StockOutSummary>> GetTopStockOutProductsAsync(int limit = 10, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var sql = StockOutQueries.GetTopStockOutProducts;
                var parameters = new DynamicParameters();

                if (startDate.HasValue && endDate.HasValue)
                {
                    sql += " AND so.created_date BETWEEN @StartDate AND @EndDate";
                    parameters.Add("StartDate", startDate.Value);
                    parameters.Add("EndDate", endDate.Value);
                }

                sql += " GROUP BY p.product_id, p.name, p.sku ORDER BY total_quantity DESC LIMIT @Limit";
                parameters.Add("Limit", limit);

                return await _databaseHelper.QueryAsync<StockOutSummary>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetTopStockOutProductsAsync error - Limit: {limit} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> StockOutForOrderAsync(int productId, string batchNo, decimal quantity, int orderId, int userId)
        {
            using (var connection = await _databaseHelper.CreateConnectionAsync())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. Kiểm tra tồn kho
                        var canStockOut = await _inventoryRepository.IsStockSufficientAsync(productId, quantity);
                        if (!canStockOut)
                        {
                            _logger.Warning($"Không đủ tồn kho để xuất - ProductId: {productId}, Quantity: {quantity}");
                            return false;
                        }

                        // 2. Giảm tồn kho
                        var reduceSuccess = await _inventoryRepository.ReduceStockAsync(productId, batchNo, quantity);
                        if (!reduceSuccess)
                        {
                            _logger.Error($"Không thể giảm tồn kho - ProductId: {productId}, Batch: {batchNo}, Quantity: {quantity}");
                            transaction.Rollback();
                            return false;
                        }

                        // 3. Ghi nhận xuất kho
                        var notes = $"Xuất kho cho đơn hàng #{orderId}";
                        var affectedRows = await connection.ExecuteAsync(
                            StockOutQueries.StockOutForOrder,
                            new
                            {
                                BatchNo = batchNo,
                                ProductId = productId,
                                Quantity = quantity,
                                Notes = notes,
                                UserId = userId
                            },
                            transaction
                        );

                        if (affectedRows > 0)
                        {
                            transaction.Commit();
                            _logger.Info($"Đã xuất kho cho đơn hàng - ProductId: {productId}, Quantity: {quantity}, OrderId: {orderId}");
                            return true;
                        }

                        transaction.Rollback();
                        return false;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.Error($"StockOutForOrderAsync error - ProductId: {productId}, OrderId: {orderId} - {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public async Task<bool> StockOutMultipleAsync(IEnumerable<StockOut> stockOuts)
        {
            using (var connection = await _databaseHelper.CreateConnectionAsync())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var stockOut in stockOuts)
                        {
                            // Kiểm tra tồn kho cho từng item
                            if (stockOut.RefType == RefType.PRODUCT)
                            {
                                var canStockOut = await _inventoryRepository.IsStockSufficientAsync(
                                    stockOut.RefId, stockOut.Quantity);

                                if (!canStockOut)
                                {
                                    _logger.Warning($"Không đủ tồn kho - RefId: {stockOut.RefId}, Quantity: {stockOut.Quantity}");
                                    transaction.Rollback();
                                    return false;
                                }

                                // Giảm tồn kho
                                var reduceSuccess = await _inventoryRepository.ReduceStockAsync(
                                    stockOut.RefId, stockOut.BatchNo, stockOut.Quantity);

                                if (!reduceSuccess)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                            }
                            else if (stockOut.RefType == RefType.PACKAGING)
                            {
                                var canStockOut = await _packagingInventoryRepository.IsNewPackagingSufficientAsync(
                                    stockOut.RefId, (int)stockOut.Quantity);

                                if (!canStockOut)
                                {
                                    _logger.Warning($"Không đủ bao bì - RefId: {stockOut.RefId}, Quantity: {stockOut.Quantity}");
                                    transaction.Rollback();
                                    return false;
                                }

                                var reduceSuccess = await _packagingInventoryRepository.TransferToInUseAsync(
                                    stockOut.RefId, (int)stockOut.Quantity);

                                if (!reduceSuccess)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                            }

                            // Ghi nhận xuất kho
                            await connection.ExecuteAsync(
                                StockOutQueries.InsertStockOut,
                                new
                                {
                                    stockOut.BatchNo,
                                    stockOut.RefType,
                                    stockOut.RefId,
                                    stockOut.Quantity,
                                    stockOut.Purpose,
                                    stockOut.Notes,
                                    stockOut.CreatedBy
                                },
                                transaction
                            );
                        }

                        transaction.Commit();
                        _logger.Info($"Đã xuất kho {stockOuts.Count()} items");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.Error($"StockOutMultipleAsync error - {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public async Task<IEnumerable<StockOutDetail>> GetStockOutDetailsAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var sql = StockOutQueries.GetStockOutDetails;
                var parameters = new DynamicParameters();

                if (fromDate.HasValue && toDate.HasValue)
                {
                    sql += " AND so.created_date BETWEEN @FromDate AND @ToDate";
                    parameters.Add("FromDate", fromDate.Value);
                    parameters.Add("ToDate", toDate.Value);
                }

                sql += " ORDER BY so.created_date DESC";

                return await _databaseHelper.QueryAsync<StockOutDetail>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetStockOutDetailsAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<PurposeStockOutSummary>> GetStockOutByPurposeAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var sql = StockOutQueries.GetStockOutByPurpose;
                var parameters = new DynamicParameters();

                if (fromDate.HasValue && toDate.HasValue)
                {
                    sql = sql.Replace("WHERE 1=1", "WHERE so.created_date BETWEEN @FromDate AND @ToDate");
                    parameters.Add("FromDate", fromDate.Value);
                    parameters.Add("ToDate", toDate.Value);
                }

                return await _databaseHelper.QueryAsync<PurposeStockOutSummary>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetStockOutByPurposeAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> CanStockOutAsync(int productId, string batchNo, decimal quantity)
        {
            try
            {
                if (quantity <= 0)
                    return false;

                var result = await _databaseHelper.ExecuteScalarAsync<int?>(
                    StockOutQueries.CanStockOut,
                    new { ProductId = productId, BatchNo = batchNo, Quantity = quantity }
                );

                return result.HasValue;
            }
            catch (Exception ex)
            {
                _logger.Error($"CanStockOutAsync error - ProductId: {productId}, Batch: {batchNo}, Quantity: {quantity} - {ex.Message}");
                throw;
            }
        }

        public async Task<(IEnumerable<StockOut> StockOuts, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string search = null,
            RefType? refType = null,
            StockOutPurpose? purpose = null)
        {
            try
            {
                var whereClause = "WHERE 1=1";
                var parameters = new DynamicParameters();

                // Search condition
                if (!string.IsNullOrWhiteSpace(search))
                {
                    whereClause += @" AND (so.batch_no LIKE @Search OR 
                                          p.name LIKE @Search OR 
                                          pk.name LIKE @Search)";
                    parameters.Add("Search", $"%{search}%");
                }

                // RefType filter
                if (refType.HasValue)
                {
                    whereClause += " AND so.ref_type = @RefType";
                    parameters.Add("RefType", refType.Value.ToString());
                }

                // Purpose filter
                if (purpose.HasValue)
                {
                    whereClause += " AND so.purpose = @Purpose";
                    parameters.Add("Purpose", purpose.Value.ToString());
                }

                // Get total count
                var countSql = StockOutQueries.PagedStockOutsCount + " " + whereClause;
                var totalCount = await _databaseHelper.ExecuteScalarAsync<int>(countSql, parameters);

                // Get paged data
                var sql = StockOutQueries.PagedStockOutsBase + " " + whereClause +
                         " ORDER BY so.created_date DESC LIMIT @PageSize OFFSET @Offset";

                parameters.Add("PageSize", pageSize);
                parameters.Add("Offset", (pageNumber - 1) * pageSize);

                var stockOuts = await _databaseHelper.QueryAsync<StockOut>(sql, parameters);
                return (stockOuts, totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetPagedAsync error - Page: {pageNumber}, Size: {pageSize} - {ex.Message}");
                throw;
            }
        }
    }
}
