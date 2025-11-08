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
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(IDatabaseHelper databaseHelper)
            : base(databaseHelper, "Customers", "customer_id")
        {
        }

        public async Task<Customer> GetByPhoneAsync(string phone)
        {
            try
            {
                var sql = "SELECT * FROM Customers WHERE phone = @Phone AND is_active = TRUE";
                return await _databaseHelper.QueryFirstOrDefaultAsync<Customer>(sql, new { Phone = phone });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByPhoneAsync error - Phone: {phone} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Customer>> SearchAsync(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return await GetAllAsync();

                var sql = @"
                    SELECT * FROM Customers 
                    WHERE is_active = TRUE 
                    AND (name LIKE @Keyword OR phone LIKE @Keyword OR email LIKE @Keyword)
                    ORDER BY name";

                return await _databaseHelper.QueryAsync<Customer>(sql, new { Keyword = $"%{keyword}%" });
            }
            catch (Exception ex)
            {
                _logger.Error($"SearchAsync error - Keyword: {keyword} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Customer>> GetByRankAsync(string rank)
        {
            try
            {
                var sql = "SELECT * FROM Customers WHERE rank = @Rank AND is_active = TRUE ORDER BY name";
                return await _databaseHelper.QueryAsync<Customer>(sql, new { Rank = rank });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByRankAsync error - Rank: {rank} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdatePointsAsync(int customerId, int pointsToAdd)
        {
            try
            {
                var sql = @"
                    UPDATE Customers 
                    SET total_point = total_point + @PointsToAdd 
                    WHERE customer_id = @CustomerId";

                var affectedRows = await _databaseHelper.ExecuteAsync(sql, new
                {
                    CustomerId = customerId,
                    PointsToAdd = pointsToAdd
                });

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"UpdatePointsAsync error - CustomerId: {customerId}, Points: {pointsToAdd} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateRankAsync(int customerId, string newRank)
        {
            try
            {
                var sql = "UPDATE Customers SET rank = @NewRank WHERE customer_id = @CustomerId";
                var affectedRows = await _databaseHelper.ExecuteAsync(sql, new
                {
                    CustomerId = customerId,
                    NewRank = newRank
                });

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"UpdateRankAsync error - CustomerId: {customerId}, Rank: {newRank} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ToggleActiveAsync(int customerId, bool isActive)
        {
            try
            {
                var sql = "UPDATE Customers SET is_active = @IsActive WHERE customer_id = @CustomerId";
                var affectedRows = await _databaseHelper.ExecuteAsync(sql, new
                {
                    CustomerId = customerId,
                    IsActive = isActive
                });

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"ToggleActiveAsync error - CustomerId: {customerId}, IsActive: {isActive} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Customer>> GetTopCustomersAsync(int limit = 10)
        {
            try
            {
                var sql = @"
                    SELECT * FROM Customers 
                    WHERE is_active = TRUE 
                    ORDER BY total_point DESC, name ASC 
                    LIMIT @Limit";

                return await _databaseHelper.QueryAsync<Customer>(sql, new { Limit = limit });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetTopCustomersAsync error - Limit: {limit} - {ex.Message}");
                throw;
            }
        }

        public async Task<(IEnumerable<Customer> Customers, int TotalCount)> GetPagedCustomersAsync(
            int pageNumber, int pageSize, string searchKeyword = null, string rank = null)
        {
            try
            {
                var whereClause = "WHERE is_active = TRUE";
                var parameters = new DynamicParameters();

                // Add search condition
                if (!string.IsNullOrWhiteSpace(searchKeyword))
                {
                    whereClause += " AND (name LIKE @SearchKeyword OR phone LIKE @SearchKeyword OR email LIKE @SearchKeyword)";
                    parameters.Add("SearchKeyword", $"%{searchKeyword}%");
                }

                // Add rank filter
                if (!string.IsNullOrWhiteSpace(rank))
                {
                    whereClause += " AND rank = @Rank";
                    parameters.Add("Rank", rank);
                }

                // Get total count
                var countSql = $"SELECT COUNT(*) FROM Customers {whereClause}";
                var totalCount = await _databaseHelper.ExecuteScalarAsync<int>(countSql, parameters);

                // Get paged data
                var sql = $@"
                    SELECT * FROM Customers 
                    {whereClause}
                    ORDER BY name ASC
                    LIMIT @PageSize OFFSET @Offset";

                parameters.Add("PageSize", pageSize);
                parameters.Add("Offset", (pageNumber - 1) * pageSize);

                var customers = await _databaseHelper.QueryAsync<Customer>(sql, parameters);
                return (customers, totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetPagedCustomersAsync error - Page: {pageNumber}, Size: {pageSize} - {ex.Message}");
                throw;
            }
        }

    }
}
