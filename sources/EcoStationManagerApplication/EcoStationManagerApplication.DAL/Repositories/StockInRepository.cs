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
    public class StockInRepository : BaseRepository<StockIn>, IStockInRepository
    {
        public StockInRepository(IDatabaseHelper databaseHelper)
            : base(databaseHelper, "StockIn", "stockin_id")
        {
        }

        public async Task<IEnumerable<StockIn>> GetByProductAsync(int productId)
        {
            try
            {
                return await _databaseHelper.QueryAsync<StockIn>(
                    StockInQueries.GetByProduct,
                    new { ProductId = productId }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByProductAsync error - ProductId: {productId} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<StockIn>> GetBySupplierAsync(int supplierId)
        {
            try
            {
                return await _databaseHelper.QueryAsync<StockIn>(
                    StockInQueries.GetBySupplier,
                    new { SupplierId = supplierId }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetBySupplierAsync error - SupplierId: {supplierId} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<StockIn>> GetByReferenceAsync(RefType refType, int refId)
        {
            try
            {
                return await _databaseHelper.QueryAsync<StockIn>(
                    StockInQueries.GetByReference,
                    new { RefType = refType.ToString(), RefId = refId }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByReferenceAsync error - RefType: {refType}, RefId: {refId} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<StockIn>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _databaseHelper.QueryAsync<StockIn>(
                    StockInQueries.GetByDateRange,
                    new { StartDate = startDate, EndDate = endDate }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByDateRangeAsync error - Start: {startDate}, End: {endDate} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<StockIn>> GetByBatchAsync(string batchNo)
        {
            try
            {
                return await _databaseHelper.QueryAsync<StockIn>(
                    StockInQueries.GetByBatch,
                    new { BatchNo = batchNo }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByBatchAsync error - BatchNo: {batchNo} - {ex.Message}");
                throw;
            }
        }

        public async Task<decimal> GetTotalStockInValueAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var sql = StockInQueries.GetTotalStockInValue;
                var parameters = new DynamicParameters();

                if (startDate.HasValue && endDate.HasValue)
                {
                    sql += " AND si.created_date BETWEEN @StartDate AND @EndDate";
                    parameters.Add("StartDate", startDate.Value);
                    parameters.Add("EndDate", endDate.Value);
                }

                return await _databaseHelper.ExecuteScalarAsync<decimal>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetTotalStockInValueAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<decimal> GetTotalQuantityByProductAsync(int productId, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var sql = StockInQueries.GetTotalQuantityByProduct;
                var parameters = new DynamicParameters();
                parameters.Add("ProductId", productId);

                if (startDate.HasValue && endDate.HasValue)
                {
                    sql += " AND si.created_date BETWEEN @StartDate AND @EndDate";
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

        public async Task<IEnumerable<StockInSummary>> GetTopStockInProductsAsync(int limit = 10, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var sql = StockInQueries.GetTopStockInProducts;
                var parameters = new DynamicParameters();

                if (startDate.HasValue && endDate.HasValue)
                {
                    sql += " AND si.created_date BETWEEN @StartDate AND @EndDate";
                    parameters.Add("StartDate", startDate.Value);
                    parameters.Add("EndDate", endDate.Value);
                }

                sql += " GROUP BY p.product_id, p.name, p.sku ORDER BY total_quantity DESC LIMIT @Limit";
                parameters.Add("Limit", limit);

                return await _databaseHelper.QueryAsync<StockInSummary>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetTopStockInProductsAsync error - Limit: {limit} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<StockInDetail>> GetStockInDetailsAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var sql = StockInQueries.GetStockInDetails;
                var parameters = new DynamicParameters();

                if (fromDate.HasValue && toDate.HasValue)
                {
                    sql += " AND si.created_date BETWEEN @FromDate AND @ToDate";
                    parameters.Add("FromDate", fromDate.Value);
                    parameters.Add("ToDate", toDate.Value);
                }

                sql += " ORDER BY si.created_date DESC";

                return await _databaseHelper.QueryAsync<StockInDetail>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetStockInDetailsAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<SupplierStockInSummary>> GetStockInBySupplierAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var sql = StockInQueries.GetStockInBySupplier;
                var parameters = new DynamicParameters();

                if (fromDate.HasValue && toDate.HasValue)
                {
                    sql = sql.Replace("WHERE 1=1", "WHERE si.created_date BETWEEN @FromDate AND @ToDate");
                    parameters.Add("FromDate", fromDate.Value);
                    parameters.Add("ToDate", toDate.Value);
                }

                return await _databaseHelper.QueryAsync<SupplierStockInSummary>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetStockInBySupplierAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> IsBatchExistsAsync(string batchNo, RefType refType, int refId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(batchNo))
                    return false;

                var result = await _databaseHelper.ExecuteScalarAsync<int?>(
                    StockInQueries.IsBatchExists,
                    new
                    {
                        BatchNo = batchNo,
                        RefType = refType.ToString(),
                        RefId = refId
                    }
                );

                return result.HasValue;
            }
            catch (Exception ex)
            {
                _logger.Error($"IsBatchExistsAsync error - BatchNo: {batchNo}, RefType: {refType}, RefId: {refId} - {ex.Message}");
                throw;
            }
        }

        public async Task<(IEnumerable<StockIn> StockIns, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string search = null,
            RefType? refType = null)
        {
            try
            {
                var whereClause = "WHERE 1=1";
                var parameters = new DynamicParameters();

                // Search condition
                if (!string.IsNullOrWhiteSpace(search))
                {
                    whereClause += @" AND (si.batch_no LIKE @Search OR 
                                          p.name LIKE @Search OR 
                                          pk.name LIKE @Search OR
                                          s.name LIKE @Search)";
                    parameters.Add("Search", $"%{search}%");
                }

                // RefType filter
                if (refType.HasValue)
                {
                    whereClause += " AND si.ref_type = @RefType";
                    parameters.Add("RefType", refType.Value.ToString());
                }

                // Get total count
                var countSql = StockInQueries.PagedStockInsCount + " " + whereClause;
                var totalCount = await _databaseHelper.ExecuteScalarAsync<int>(countSql, parameters);

                // Get paged data
                var sql = StockInQueries.PagedStockInsBase + " " + whereClause +
                         " ORDER BY si.created_date DESC LIMIT @PageSize OFFSET @Offset";

                parameters.Add("PageSize", pageSize);
                parameters.Add("Offset", (pageNumber - 1) * pageSize);

                var stockIns = await _databaseHelper.QueryAsync<StockIn>(sql, parameters);
                return (stockIns, totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetPagedAsync error - Page: {pageNumber}, Size: {pageSize} - {ex.Message}");
                throw;
            }
        }
    }
}
