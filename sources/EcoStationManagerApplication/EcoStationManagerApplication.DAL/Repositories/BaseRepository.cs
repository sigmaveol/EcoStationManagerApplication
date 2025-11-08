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
}
