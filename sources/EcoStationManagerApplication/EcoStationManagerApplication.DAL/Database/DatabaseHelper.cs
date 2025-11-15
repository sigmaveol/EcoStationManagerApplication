using Dapper;
using EcoStationManagerApplication.Common.Config;
using EcoStationManagerApplication.Common.Logging;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Database
{
    public class DatabaseHelper : IDatabaseHelper
    {
        private readonly string _connectionString;
        private readonly ILogHelper _logger;
        private readonly DatabaseConfig _dbConfig;

        public DatabaseHelper()
        {
            _dbConfig = ConfigManager.GetDatabaseConfig();
            _connectionString = ConfigManager.GetConnectionString();
            _logger = LogHelperFactory.CreateLogger("DatabaseHelper");

            // Cấu hình Dapper để tự động map snake_case (order_id) sang PascalCase (OrderId)
            ConfigureDapperMapping();

        }

        private static void ConfigureDapperMapping()
        {
            try
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                RegisterCustomTypeMappings();
                RegisterEnumHandlers();
            }
            catch (Exception ex)
            {
                var logger = LogHelperFactory.CreateLogger("DatabaseHelper");
                logger.Error($"Dapper mapping configuration failed: {ex.Message}");
            }
        }

        private static void RegisterCustomTypeMappings()
        {
            try
            {
                var entityAssembly = typeof(Order).Assembly;
                var entityTypes = entityAssembly.GetTypes()
                    .Where(t => t.Namespace == "EcoStationManagerApplication.Models.Entities" && t.IsClass && !t.IsAbstract);

                foreach (var type in entityTypes)
                {
                    SqlMapper.SetTypeMap(type, new CustomPropertyTypeMap(type, (modelType, columnName) =>
                    {
                        var pascalName = SnakeCaseToPascalCase(columnName);
                        return modelType.GetProperty(pascalName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ??
                               modelType.GetProperty(columnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    }));
                }
            }
            catch (Exception ex)
            {
                var logger = LogHelperFactory.CreateLogger("DatabaseHelper");
                logger.Error($"RegisterCustomTypeMappings error: {ex.Message}");
            }
        }

        private static void RegisterEnumHandlers()
        {
            var enumTypes = new[]
            {
                typeof(OrderSource), typeof(OrderStatus), typeof(PaymentStatus), typeof(PaymentMethod),
                typeof(CustomerRank), typeof(CategoryType), typeof(ProductType), typeof(RefType),
                typeof(StockOutPurpose), typeof(CleaningType), typeof(CleaningStatus),
                typeof(PackagingTransactionType), typeof(PackagingOwnershipType), typeof(DeliveryStatus),
                typeof(DeliveryPaymentStatus), typeof(ActiveStatus)
            };

            foreach (var enumType in enumTypes)
            {
                var handlerType = typeof(GenericEnumHandler<>).MakeGenericType(enumType);
                var handler = Activator.CreateInstance(handlerType);
                SqlMapper.AddTypeHandler(enumType, (SqlMapper.ITypeHandler)handler);
            }
        }

        private static string SnakeCaseToPascalCase(string snakeCase) =>
            string.IsNullOrEmpty(snakeCase) ? snakeCase :
            string.Join("", snakeCase.Split('_').Select(part => 
                part.Length > 0 ? char.ToUpper(part[0]) + part.Substring(1).ToLower() : part));

        private class GenericEnumHandler<T> : SqlMapper.TypeHandler<T> where T : struct, Enum
        {
            private static readonly ILogHelper _logger = LogHelperFactory.CreateLogger($"EnumHandler<{typeof(T).Name}>");

            public override void SetValue(IDbDataParameter parameter, T value)
            {
                if (EqualityComparer<T>.Default.Equals(value, default(T)))
                {
                    var enumValues = Enum.GetValues(typeof(T));
                    parameter.Value = enumValues.Length > 0 ? enumValues.GetValue(0).ToString() : value.ToString();
                }
                else
                {
                    parameter.Value = value.ToString();
                }
            }

            public override T Parse(object value)
            {
                try
                {
                    if (value == null || value == DBNull.Value) return default;
                    
                    if (value is string str)
                    {
                        var trimmed = str?.Trim() ?? "";
                        return string.IsNullOrEmpty(trimmed) ? default :
                               Enum.TryParse<T>(trimmed, true, out var result) ? result : default;
                    }
                    
                    if (value is int intVal && Enum.IsDefined(typeof(T), intVal))
                        return (T)Enum.ToObject(typeof(T), intVal);

                    return default;
                }
                catch { return default; }
            }
        }

        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
            _dbConfig = ConfigManager.GetDatabaseConfig();
            _logger = LogHelperFactory.CreateLogger("DatabaseHelper");

            // Cũng cấu hình mapping cho constructor này
            ConfigureDapperMapping();
        }

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                if (ConfigManager.IsDevelopment())
                {
                    _logger.Info($"Connection opened - State: {connection.State}");
                }

                return connection;
            }
            catch (Exception ex)
            {
                _logger.Error($"Không thể kết nối đến database: {ex.Message}");
                _logger.Error($"Connection string: {ConnectionHelper.GetSafeConnectionString()}");

                // Đảm bảo connection được dispose nếu có lỗi
                connection?.Dispose();
                throw new Exception($"Lỗi kết nối database: {ex.Message}", ex);
            }
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using (var connection = await CreateConnectionAsync())
                {
                    var result = await connection.ExecuteScalarAsync<string>("SELECT 'OK'");
                    return result == "OK";
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Test connection failed: {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null, IDbTransaction transaction = null)
        {
            if (transaction != null)
                return await transaction.Connection.QueryAsync<T>(sql, parameters, transaction);

            using (var connection = await CreateConnectionAsync())
            {
                try
                {
                    return await connection.QueryAsync<T>(sql, parameters);
                }
                catch (System.Data.DataException dataEx)
                {
                    _logger.Error($"QueryAsync DataException: {sql} - {dataEx.Message}");
                    if (dataEx.InnerException != null)
                        _logger.Error($"Inner exception: {dataEx.InnerException.Message}");
                    _logger.Warning("Returning empty list due to parse error. Please check database data integrity.");
                    return Enumerable.Empty<T>();
                }
                catch (Exception ex)
                {
                    _logger.Error($"QueryAsync error: {sql} - {ex.Message}");
                    throw;
                }
            }
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null, IDbTransaction transaction = null)
        {
            if (transaction != null)
                return await transaction.Connection.QueryFirstOrDefaultAsync<T>(sql, parameters, transaction);

            using (var connection = await CreateConnectionAsync())
            {
                try
                {
                    return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
                }
                catch (System.Data.DataException dataEx)
                {
                    _logger.Error($"QueryFirstOrDefaultAsync DataException: {sql} - {dataEx.Message}");
                    if (dataEx.InnerException != null)
                        _logger.Error($"Inner exception: {dataEx.InnerException.Message}");
                    _logger.Warning("Returning default value due to parse error. Please check database data integrity.");
                    return default(T);
                }
                catch (Exception ex)
                {
                    _logger.Error($"QueryFirstOrDefaultAsync error: {sql} - {ex.Message}");
                    throw;
                }
            }
        }

        public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object parameters = null, IDbTransaction transaction = null)
        {
            if (transaction != null)
                return await transaction.Connection.QuerySingleOrDefaultAsync<T>(sql, parameters, transaction);

            using (var connection = await CreateConnectionAsync())
            {
                try
                {
                    return await connection.QuerySingleOrDefaultAsync<T>(sql, parameters);
                }
                catch (Exception ex)
                {
                    _logger.Error($"QuerySingleOrDefaultAsync error: {sql} - {ex.Message}");
                    throw;
                }
            }
        }

        public async Task<int> ExecuteAsync(string sql, object parameters = null, IDbTransaction transaction = null)
        {
            if (transaction != null)
                return await transaction.Connection.ExecuteAsync(sql, parameters, transaction);

            using (var connection = await CreateConnectionAsync())
            {
                try
                {
                    return await connection.ExecuteAsync(sql, parameters);
                }
                catch (Exception ex)
                {
                    _logger.Error($"ExecuteAsync error: {sql} - {ex.Message}");
                    throw;
                }
            }
        }

        public async Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string procedureName, object parameters = null, IDbTransaction transaction = null)
        {
            var command = new CommandDefinition(procedureName, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
            
            if (transaction != null)
                return await transaction.Connection.QueryAsync<T>(command);

            using (var connection = await CreateConnectionAsync())
            {
                try
                {
                    return await connection.QueryAsync<T>(command);
                }
                catch (Exception ex)
                {
                    _logger.Error($"ExecuteStoredProcedureAsync<T> error: {procedureName} - {ex.Message}");
                    throw;
                }
            }
        }

        public async Task<int> ExecuteStoredProcedureAsync(string procedureName, object parameters = null, IDbTransaction transaction = null)
        {
            var command = new CommandDefinition(procedureName, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
            
            if (transaction != null)
                return await transaction.Connection.ExecuteAsync(command);

            using (var connection = await CreateConnectionAsync())
            {
                try
                {
                    return await connection.ExecuteAsync(command);
                }
                catch (Exception ex)
                {
                    _logger.Error($"ExecuteStoredProcedureAsync error: {procedureName} - {ex.Message}");
                    throw;
                }
            }
        }

        public async Task<IDbTransaction> BeginTransactionAsync() => 
            (await CreateConnectionAsync()).BeginTransaction();

        public async Task<IDbTransaction> BeginTransactionAsync(IsolationLevel isolationLevel) => 
            (await CreateConnectionAsync()).BeginTransaction(isolationLevel);

        public async Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object parameters = null, IDbTransaction transaction = null)
        {
            if (transaction != null)
                return await transaction.Connection.QueryMultipleAsync(sql, parameters, transaction);

            using (var connection = await CreateConnectionAsync())
            {
                try
                {
                    return await connection.QueryMultipleAsync(sql, parameters);
                }
                catch (Exception ex)
                {
                    _logger.Error($"QueryMultipleAsync error: {sql} - {ex.Message}");
                    throw;
                }
            }
        }

        public async Task<T> ExecuteScalarAsync<T>(string sql, object parameters = null, IDbTransaction transaction = null)
        {
            if (transaction != null)
                return await transaction.Connection.ExecuteScalarAsync<T>(sql, parameters, transaction);

            using (var connection = await CreateConnectionAsync())
            {
                try
                {
                    return await connection.ExecuteScalarAsync<T>(sql, parameters);
                }
                catch (Exception ex)
                {
                    _logger.Error($"ExecuteScalarAsync error: {sql} - {ex.Message}");
                    throw;
                }
            }
        }
    }
}