using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using EcoStationManagerApplication.DAL.Database;
using EcoStationManagerApplication.Core.Interfaces;

namespace EcoStationManagerApplication.DAL.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly DatabaseHelper _dbHelper;
        protected abstract string TableName { get; }
        protected abstract string PrimaryKeyName { get; }
        protected virtual bool HasIsActive => false;

        protected BaseRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            var sql = $"SELECT * FROM {TableName} WHERE {PrimaryKeyName} = @Id";
            return await _dbHelper.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var sql = $"SELECT * FROM {TableName}";
            return await _dbHelper.QueryAsync<T>(sql);
        }

        public virtual async Task<IEnumerable<T>> GetActiveAsync()
        {
            if (!HasIsActive)
                return await GetAllAsync();

            var sql = $"SELECT * FROM {TableName} WHERE is_active = true";
            return await _dbHelper.QueryAsync<T>(sql);
        }

        public abstract Task<int> CreateAsync(T entity);
        public abstract Task<bool> UpdateAsync(T entity);

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var sql = $"DELETE FROM {TableName} WHERE {PrimaryKeyName} = @Id";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }
        public virtual async Task<bool> SoftDeleteAsync(int id)
        {
            if (!HasIsActive)
                return await DeleteAsync(id);

            var sql = $"UPDATE {TableName} SET is_active = false WHERE {PrimaryKeyName} = @Id";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            var sql = $"SELECT COUNT(1) FROM {TableName} WHERE {PrimaryKeyName} = @Id";
            var count = await _dbHelper.ExecuteScalarAsync<int>(sql, new { Id = id });
            return count > 0;
        }

        public virtual async Task<int> GetCountAsync()
        {
            var sql = $"SELECT COUNT(*) FROM {TableName}";
            return await _dbHelper.ExecuteScalarAsync<int>(sql);
        }
    }

}
