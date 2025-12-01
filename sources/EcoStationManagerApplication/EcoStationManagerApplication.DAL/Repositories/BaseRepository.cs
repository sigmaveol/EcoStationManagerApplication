using Dapper;
using EcoStationManagerApplication.DAL.Database;
using EcoStationManagerApplication.Common.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
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
                var sql = $"SELECT * FROM {_tableName}";
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

                var dp = new DynamicParameters();
                foreach (var p in ColumnMappingCache<T>.Properties.Where(p => p != ColumnMappingCache<T>.IdProperty))
                {
                    dp.Add("@" + ColumnMappingCache<T>.PropertyToColumn[p.Name], p.GetValue(entity));
                }

                return await _databaseHelper.ExecuteScalarAsync<int>(
                    ColumnMappingCache<T>.InsertSql,
                    dp
                );
            }
            catch (System.Data.DataException ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.Message);
                throw;
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
                var dp = new DynamicParameters();

                foreach (var p in ColumnMappingCache<T>.Properties.Where(p => p != ColumnMappingCache<T>.IdProperty))
                {
                    dp.Add("@" + ColumnMappingCache<T>.PropertyToColumn[p.Name], p.GetValue(entity));
                }

                dp.Add("@IdParam", ColumnMappingCache<T>.IdProperty.GetValue(entity));

                var rows = await _databaseHelper.ExecuteAsync(
                    ColumnMappingCache<T>.UpdateSql,
                    dp
                );

                return rows > 0;
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
