using Dapper;
using EcoStationManager.DAL.Database;
using EcoStationManagerApplication.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly IDatabaseHelper _databaseHelper;
        protected readonly ILogHelper _logger;
        protected readonly string _tableName;
        protected readonly string _idColumn;

        protected BaseRepository(IDatabaseHelper databaseHelper, string tableName, string idColumn = "id")
        {
            _databaseHelper = databaseHelper;
            _tableName = tableName;
            _idColumn = idColumn;
            _logger = LogHelperFactory.CreateLogger($"Repo:{tableName}");
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            try
            {
                var sql = $"SELECT * FROM {_tableName} WHERE {_idColumn} = @Id";
                return await _databaseHelper.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
            }
            catch (Exception ex) {
                _logger.Error($"GetByIdAsync error - ID: {id}, Table: {_tableName} - {ex.Message}");
                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                var sql = $"SELECT * FROM {_tableName} WHERE is_active = TRUE";
                return await _databaseHelper.QueryAsync<T>(sql);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetAllAsync error - Table: {_tableName} - {ex.Message}");
                throw;
            }
        }

        public virtual async Task<int> AddAsync(T entity)
        {
            try
            {
                var properties = typeof(T).GetProperties()
                    .Where(p => p.Name != _idColumn) // Exclude ID column for insert
                    .ToList();

                var columns = string.Join(", ", properties.Select(p => p.Name));
                var parameters = string.Join(", ", properties.Select(p => $"@{p.Name}"));

                var sql = $"INSERT INTO {_tableName} ({columns}) VALUES ({parameters}); SELECT LAST_INSERT_ID();";

                return await _databaseHelper.ExecuteScalarAsync<int>(sql, entity);
            }
            catch (Exception ex)
            {
                _logger.Error($"AddAsync error - Table: {_tableName} - {ex.Message}");
                throw;
            }
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                var properties = typeof(T).GetProperties()
                    .Where(p => p.Name != _idColumn) // Exclude ID column from SET clause
                    .ToList();

                var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

                var sql = $"UPDATE {_tableName} SET {setClause} WHERE {_idColumn} = @{_idColumn}";
                var affectedRows = await _databaseHelper.ExecuteAsync(sql, entity);
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"UpdateAsync error - Table: {_tableName} - {ex.Message}");
                throw;
            }
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var sql = $"DELETE FROM {_tableName} WHERE {_idColumn} = @Id";
                var affectedRows = await _databaseHelper.ExecuteAsync(sql, new { Id = id });
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"DeleteAsync error - ID: {id}, Table: {_tableName} - {ex.Message}");
                throw;
            }
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            try
            {
                var sql = $"SELECT 1 FROM {_tableName} WHERE {_idColumn} = @Id";
                var result = await _databaseHelper.ExecuteScalarAsync<int?>(sql, new { Id = id });
                return result.HasValue;
            }
            catch (Exception ex)
            {
                _logger.Error($"ExistsAsync error - ID: {id}, Table: {_tableName} - {ex.Message}");
                throw;
            }
        }
    }

    public abstract class BasePagedRepository<T> : BaseRepository<T>, IPagedRepository<T> where T : class
    {
        protected BasePagedRepository(IDatabaseHelper databaseHelper, string tableName, string idColumn = "id")
            : base(databaseHelper, tableName, idColumn)
        {
        }

        public virtual async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string search = null,
            string sortBy = null,
            bool sortDesc = false)
        {
            try
            {
                var whereClause = BuildWhereClause(search);
                var parameters = new DynamicParameters();
                BuildSearchParameters(parameters, search);

                // Search condition
                if (!string.IsNullOrEmpty(search))
                {
                    var searchCondition = "name LIKE @Search";
                    whereClause += string.IsNullOrEmpty(whereClause) ? $"WHERE {searchCondition}" : $" AND {searchCondition}";
                    parameters.Add("Search", $"%{search}%");
                }

                // Count total records
                var countSql = $"SELECT COUNT(*) FROM {_tableName} {whereClause}";
                var totalCount = await _databaseHelper.ExecuteScalarAsync<int>(countSql, parameters);

                // Build order by clause
                var orderBy = BuildOrderByClause(sortBy, sortDesc);

                // Get paged data
                var sql = $@"
                    SELECT * FROM {_tableName} 
                    {whereClause}
                    {orderBy}
                    LIMIT @PageSize OFFSET @Offset";

                parameters.Add("PageSize", pageSize);
                parameters.Add("Offset", (pageNumber - 1) * pageSize);

                var items = await _databaseHelper.QueryAsync<T>(sql, parameters);
                return (items, totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetPagedAsync error - Table: {_tableName} - {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Build WHERE clause - CÓ THỂ OVERRIDE trong derived classes
        /// </summary>
        protected virtual string BuildWhereClause(string search)
        {
            var conditions = new List<string>();

            // Thêm điều kiện mặc định (nếu có)

            // Thêm điều kiện search
            if (!string.IsNullOrEmpty(search))
            {
                conditions.Add(GetSearchCondition());
            }

            return conditions.Count > 0 ? $"WHERE {string.Join(" AND ", conditions)}" : "";
        }

        protected virtual string GetSearchCondition()
        {
            return "name LIKE @Search";
        }

        /// <summary>
        /// Build search parameters - CÓ THỂ OVERRIDE trong derived classes
        /// </summary>
        protected virtual void BuildSearchParameters(DynamicParameters parameters, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                parameters.Add("Search", $"%{search}%");
            }
        }

        /// <summary>
        /// Build ORDER BY clause - CÓ THỂ OVERRIDE trong derived classes
        /// </summary>
        protected virtual string BuildOrderByClause(string sortBy, bool sortDesc)
        {
            var direction = sortDesc ? "DESC" : "ASC";
            var orderBy = "ORDER BY ";

            switch (sortBy?.ToLower())
            {
                case "name":
                    orderBy += $"name {direction}";
                    break;
                case "created_date":
                    orderBy += $"created_date {direction}";
                    break;
                case "updated_date":
                    orderBy += $"last_updated {direction}";
                    break;
                case "email":
                    orderBy += $"email {direction}";
                    break;
                case "phone":
                    orderBy += $"phone {direction}";
                    break;
                default:
                    orderBy += $"{_idColumn} DESC";
                    break;
            }

            return orderBy;
        }

    }

}
