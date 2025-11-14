using Dapper;
using EcoStationManagerApplication.DAL.Database;
using EcoStationManagerApplication.Common.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
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
                var properties = GetMappedProperties()
                    .Where(p => p.Name != _idColumn) // Exclude ID column for insert
                    .ToList();

                var columns = string.Join(", ", properties.Select(p => ToSnakeCase(p.Name)));
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

        private string ToSnakeCase(string name)
        {
            return string.Concat(name.Select((x, i) =>
                i > 0 && char.IsUpper(x) ? "_" + char.ToLower(x) : char.ToLower(x).ToString()));
        }

        private string ToPascalCase(string snakeCase)
        {
            if (string.IsNullOrEmpty(snakeCase))
                return snakeCase;

            var parts = snakeCase.Split('_');
            return string.Join("", parts.Select(p => 
                string.IsNullOrEmpty(p) ? "" : char.ToUpper(p[0]) + (p.Length > 1 ? p.Substring(1).ToLower() : "")));
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                // Tìm property name tương ứng với _idColumn (convert snake_case sang PascalCase)
                var idPropertyName = ToPascalCase(_idColumn);
                var idProperty = typeof(T).GetProperty(idPropertyName);
                if (idProperty == null)
                {
                    // Fallback: thử tìm property có tên giống _idColumn (case-insensitive)
                    idProperty = typeof(T).GetProperties()
                        .FirstOrDefault(p => p.Name.Equals(_idColumn, StringComparison.OrdinalIgnoreCase) ||
                                           ToSnakeCase(p.Name).Equals(_idColumn, StringComparison.OrdinalIgnoreCase));
                    idPropertyName = idProperty?.Name ?? idPropertyName;
                }

                var properties = GetMappedProperties()
                    .Where(p => !p.Name.Equals(idPropertyName, StringComparison.OrdinalIgnoreCase) && 
                               !ToSnakeCase(p.Name).Equals(_idColumn, StringComparison.OrdinalIgnoreCase)) // Exclude ID column from SET clause
                    .ToList();

                var setClause = string.Join(", ", properties.Select(p => $"{ToSnakeCase(p.Name)} = @{p.Name}"));

                var sql = $"UPDATE {_tableName} SET {setClause} WHERE {ToSnakeCase(_idColumn)} = @{idPropertyName}";
                var affectedRows = await _databaseHelper.ExecuteAsync(sql, entity);
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"UpdateAsync error - Table: {_tableName} - {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Lấy các properties được map vào database (exclude navigation properties)
        /// </summary>
        protected virtual IEnumerable<PropertyInfo> GetMappedProperties()
        {
            return typeof(T).GetProperties()
                .Where(p =>
                {
                    // Exclude navigation properties (complex types that are not primitives)
                    var propType = p.PropertyType;
                    
                    // Exclude if has NotMapped attribute
                    if (p.GetCustomAttribute<NotMappedAttribute>() != null)
                        return false;

                    // Exclude if it's a navigation property (complex type, not primitive, not enum, not nullable primitive)
                    if (propType.IsClass && 
                        propType != typeof(string) && 
                        propType != typeof(DateTime) && 
                        propType != typeof(DateTime?) &&
                        !propType.IsPrimitive &&
                        !propType.IsEnum &&
                        !IsNullablePrimitive(propType))
                    {
                        return false;
                    }

                    return true;
                });
        }

        /// <summary>
        /// Kiểm tra xem type có phải là nullable primitive không
        /// </summary>
        private bool IsNullablePrimitive(Type type)
        {
            if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(Nullable<>))
                return false;

            var underlyingType = Nullable.GetUnderlyingType(type);
            return underlyingType != null && 
                   (underlyingType.IsPrimitive || 
                    underlyingType == typeof(decimal) || 
                    underlyingType == typeof(DateTime) ||
                    underlyingType.IsEnum);
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
