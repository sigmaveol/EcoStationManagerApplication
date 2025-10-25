using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Database;

namespace EcoStationManagerApplication.DAL.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly IDbHelper _dbHelper;
        protected readonly string _tableName;
        protected readonly string _primaryKeyName;
        protected readonly bool _hasIsActive;

        public BaseRepository(IDbHelper dbHelper, string tableName, string primaryKeyName = "Id", bool hasIsActive = true)
        {
            _dbHelper = dbHelper;
            _tableName = tableName;
            _primaryKeyName = primaryKeyName;
            _hasIsActive = hasIsActive;
        }

        // Constructor mặc định sử dụng DbHelper từ AppConfigHelper
        protected BaseRepository(string tableName, string primaryKeyName = "Id", bool hasIsActive = true)
            : this(AppConfigHelper.CreateDbHelper(), tableName, primaryKeyName, hasIsActive)
        {
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            var sql = $"SELECT * FROM {_tableName} WHERE {_primaryKeyName} = @Id";
            if (_hasIsActive)
                sql += " AND is_active = 1";

            return await _dbHelper.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var sql = $"SELECT * FROM {_tableName}";
            if (_hasIsActive)
                sql += " WHERE is_active = 1";

            return await _dbHelper.QueryAsync<T>(sql);
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            // For complex queries, we'll implement specific methods in derived repositories
            var all = await GetAllAsync();
            return all.AsQueryable().Where(predicate);
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.Name != _primaryKeyName && p.Name != "CreatedDate" && p.Name != "UpdatedDate")
                .ToArray();

            var columnNames = string.Join(", ", properties.Select(p => p.Name));
            var parameterNames = string.Join(", ", properties.Select(p => "@" + p.Name));

            var sql = $"INSERT INTO {_tableName} ({columnNames}) VALUES ({parameterNames}); SELECT LAST_INSERT_ID();";

            var id = await _dbHelper.ExecuteScalarAsync<int>(sql, entity);

            // Set the primary key
            var pkProperty = typeof(T).GetProperty(_primaryKeyName);
            if (pkProperty != null && pkProperty.CanWrite)
            {
                pkProperty.SetValue(entity, id);
            }

            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.Name != _primaryKeyName && p.Name != "CreatedDate" && p.Name != "UpdatedDate")
                .ToArray();

            var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

            var sql = $"UPDATE {_tableName} SET {setClause} WHERE {_primaryKeyName} = @{_primaryKeyName}";

            await _dbHelper.ExecuteAsync(sql, entity);
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var sql = $"DELETE FROM {_tableName} WHERE {_primaryKeyName} = @Id";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }

        public virtual async Task<bool> SoftDeleteAsync(int id)
        {
            if (!_hasIsActive)
                throw new InvalidOperationException($"Table {_tableName} does not support soft delete.");

            var sql = $"UPDATE {_tableName} SET is_active = 0 WHERE {_primaryKeyName} = @Id";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }

        public virtual async Task<int> CountAsync()
        {
            var sql = $"SELECT COUNT(*) FROM {_tableName}";
            if (_hasIsActive)
                sql += " WHERE is_active = 1";

            return await _dbHelper.ExecuteScalarAsync<int>(sql);
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            var all = await GetAllAsync();
            return all.AsQueryable().Any(predicate);
        }
    }
}