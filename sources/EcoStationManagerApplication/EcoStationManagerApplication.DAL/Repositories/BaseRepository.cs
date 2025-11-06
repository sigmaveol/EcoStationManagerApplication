using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly IDatabaseHelper _databaseHelper;
        protected readonly string _tableName;
        protected readonly string _idColumn;

        protected BaseRepository(IDatabaseHelper databaseHelper, string tableName, string idColumn = "id")
        {
            _databaseHelper = databaseHelper;
            _tableName = tableName;
            _idColumn = idColumn;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            using var connection = await _databaseHelper.CreateConnectionAsync();
            var sql = $"SELECT * FROM {_tableName} WHERE {_idColumn} = @Id";
            return await connection.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            using var connection = await _databaseHelper.CreateConnectionAsync();
            var sql = $"SELECT * FROM {_tableName} WHERE is_active = TRUE";
            return await connection.QueryAsync<T>(sql);
        }

        public virtual async Task<int> AddAsync(T entity)
        {
            using var connection = await _databaseHelper.CreateConnectionAsync();
            var properties = typeof(T).GetProperties();
            var columns = string.Join(", ", properties.Select(p => p.Name));
            var parameters = string.Join(", ", properties.Select(p => $"@{p.Name}"));

            var sql = $"INSERT INTO {_tableName} ({columns}) VALUES ({parameters}); SELECT LAST_INSERT_ID();";
            return await connection.ExecuteScalarAsync<int>(sql, entity);
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            using var connection = await _databaseHelper.CreateConnectionAsync();
            var properties = typeof(T).GetProperties();
            var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

            var sql = $"UPDATE {_tableName} SET {setClause} WHERE {_idColumn} = @{_idColumn}";
            var affectedRows = await connection.ExecuteAsync(sql, entity);
            return affectedRows > 0;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            using var connection = await _databaseHelper.CreateConnectionAsync();
            var sql = $"DELETE FROM {_tableName} WHERE {_idColumn} = @Id";
            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            using var connection = await _databaseHelper.CreateConnectionAsync();
            var sql = $"SELECT 1 FROM {_tableName} WHERE {_idColumn} = @Id";
            var result = await connection.ExecuteScalarAsync<int?>(sql, new { Id = id });
            return result.HasValue;
        }
    }
}
