using Dapper;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{
    public class PackagingRepository : BaseRepository<Packaging>, IPackagingRepository
    {
        public PackagingRepository(IDatabaseHelper databaseHelper)
            : base(databaseHelper, "Packaging", "packaging_id")
        { }

        public async Task<Packaging> GetByBarcodeAsync(string barcode)
        {
            try
            {
                var sql = "SELECT * FROM Packaging WHERE barcode = @Barcode";
                return await _databaseHelper.QueryFirstOrDefaultAsync<Packaging>(sql, new { Barcode = barcode });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByBarcodeAsync error - Barcode: {barcode} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Packaging>> GetByTypeAsync(string type)
        {
            try
            {
                var sql = "SELECT * FROM Packaging WHERE type = @Type ORDER BY name";
                return await _databaseHelper.QueryAsync<Packaging>(sql, new { Type = type });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByTypeAsync error - Type: {type} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Packaging>> SearchAsync(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return await GetAllAsync();

                var sql = @"
                    SELECT * FROM Packaging 
                    WHERE name LIKE @Keyword 
                       OR barcode LIKE @Keyword
                    ORDER BY name";

                return await _databaseHelper.QueryAsync<Packaging>(sql, new { Keyword = $"%{keyword}%" });
            }
            catch (Exception ex)
            {
                _logger.Error($"SearchAsync error - Keyword: {keyword} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> IsBarcodeExistsAsync(string barcode, int? excludePackagingId = null)
        {
            try
            {
                var sql = "SELECT 1 FROM Packaging WHERE barcode = @Barcode";
                var parameters = new DynamicParameters();
                parameters.Add("Barcode", barcode);

                if (excludePackagingId.HasValue)
                {
                    sql += " AND packaging_id != @ExcludePackagingId";
                    parameters.Add("ExcludePackagingId", excludePackagingId.Value);
                }

                var result = await _databaseHelper.ExecuteScalarAsync<int?>(sql, parameters);
                return result.HasValue;
            }
            catch (Exception ex)
            {
                _logger.Error($"IsBarcodeExistsAsync error - Barcode: {barcode}, ExcludeId: {excludePackagingId} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateDepositPriceAsync(int packagingId, decimal newPrice)
        {
            try
            {
                var sql = "UPDATE Packaging SET deposit_price = @NewPrice WHERE packaging_id = @PackagingId";
                var affectedRows = await _databaseHelper.ExecuteAsync(sql, new
                {
                    PackagingId = packagingId,
                    NewPrice = newPrice
                });

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"UpdateDepositPriceAsync error - PackagingId: {packagingId}, Price: {newPrice} - {ex.Message}");
                throw;
            }
        }

        public async Task<int> GetTotalPackagingCountAsync()
        {
            try
            {
                var sql = "SELECT COUNT(*) FROM Packaging";
                return await _databaseHelper.ExecuteScalarAsync<int>(sql);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetTotalPackagingCountAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<(IEnumerable<Packaging> Packagings, int TotalCount)> GetPagedPackagingsAsync(
            int pageNumber,
            int pageSize,
            string search = null,
            string type = null)
        {
            try
            {
                var whereClause = "WHERE 1=1";
                var parameters = new DynamicParameters();

                // Search condition
                if (!string.IsNullOrWhiteSpace(search))
                {
                    whereClause += " AND (name LIKE @Search OR barcode LIKE @Search)";
                    parameters.Add("Search", $"%{search}%");
                }

                // Type filter
                if (!string.IsNullOrWhiteSpace(type))
                {
                    whereClause += " AND type = @Type";
                    parameters.Add("Type", type);
                }

                // Get total count
                var countSql = $"SELECT COUNT(*) FROM Packaging {whereClause}";
                var totalCount = await _databaseHelper.ExecuteScalarAsync<int>(countSql, parameters);

                // Get paged data
                var sql = $@"
                    SELECT * FROM Packaging 
                    {whereClause}
                    ORDER BY name ASC
                    LIMIT @PageSize OFFSET @Offset";

                parameters.Add("PageSize", pageSize);
                parameters.Add("Offset", (pageNumber - 1) * pageSize);

                var packagings = await _databaseHelper.QueryAsync<Packaging>(sql, parameters);
                return (packagings, totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetPagedPackagingsAsync error - Page: {pageNumber}, Size: {pageSize} - {ex.Message}");
                throw;
            }
        }

    }
}
