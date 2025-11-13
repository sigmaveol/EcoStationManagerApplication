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

        /// <summary>
        /// DatabaseHelper constructor mặc định, lấy cấu hình từ ConfigManager.
        /// </summary>
        public DatabaseHelper()
        {
            _dbConfig = ConfigManager.GetDatabaseConfig();
            _connectionString = ConfigManager.GetConnectionString();
            _logger = LogHelperFactory.CreateLogger("DatabaseHelper");

            // Cấu hình Dapper để tự động map snake_case (order_id) sang PascalCase (OrderId)
            ConfigureDapperMapping();

            _logger.Info($"DatabaseHelper initialized - Server: {_dbConfig.Server}, Database: {_dbConfig.Database}");
        }

        /// <summary>
        /// Cấu hình Dapper để tự động map snake_case sang PascalCase
        /// </summary>
        private static void ConfigureDapperMapping()
        {
            try
            {
                // CÁCH 1: Sử dụng MatchNamesWithUnderscores (đơn giản nhất)
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                // CÁCH 2: Custom type mapping cho tất cả entities (chắc chắn hơn)
                RegisterCustomTypeMappings();

                // CÁCH 3: Đăng ký enum handlers
                RegisterEnumHandlers();

                //_logger.Info("Dapper mapping configured successfully");
            }
            catch (Exception ex)
            {
                var logger = LogHelperFactory.CreateLogger("DatabaseHelper");
                logger.Error($"Dapper mapping configuration failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Đăng ký custom type mappings cho các entities
        /// </summary>
        private static void RegisterCustomTypeMappings()
        {
            try
            {
                // Sử dụng assembly chứa entities thay vì GetExecutingAssembly()
                // Vì entities nằm trong assembly Models, không phải DAL
                var entityAssembly = typeof(Order).Assembly;
                var entityTypes = entityAssembly
                    .GetTypes()
                    .Where(t => t.Namespace == "EcoStationManagerApplication.Models.Entities" &&
                               t.IsClass && !t.IsAbstract);

                foreach (var type in entityTypes)
                {
                    SqlMapper.SetTypeMap(type, new CustomPropertyTypeMap(type,
                        (modelType, columnName) =>
                        {
                            // Ưu tiên tìm property theo PascalCase, sau đó theo exact name
                            var pascalName = SnakeCaseToPascalCase(columnName);
                            return modelType.GetProperty(pascalName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ??
                                   modelType.GetProperty(columnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        }));
                }
            }
            catch (Exception ex)
            {
                // Log error nhưng không throw để không làm crash ứng dụng
                var logger = LogHelperFactory.CreateLogger("DatabaseHelper");
                logger.Error($"RegisterCustomTypeMappings error: {ex.Message}");
            }
        }

        /// <summary>
        /// Đăng ký handlers cho các enum types
        /// </summary>
        private static void RegisterEnumHandlers()
        {
            // Generic enum handler
            SqlMapper.AddTypeHandler(new GenericEnumHandler<OrderSource>());
            SqlMapper.AddTypeHandler(new GenericEnumHandler<OrderStatus>());
            SqlMapper.AddTypeHandler(new GenericEnumHandler<PaymentStatus>());
            SqlMapper.AddTypeHandler(new GenericEnumHandler<PaymentMethod>());
            SqlMapper.AddTypeHandler(new GenericEnumHandler<CustomerRank>());
            SqlMapper.AddTypeHandler(new GenericEnumHandler<CategoryType>());
            SqlMapper.AddTypeHandler(new GenericEnumHandler<ProductType>());
            SqlMapper.AddTypeHandler(new GenericEnumHandler<RefType>());
            SqlMapper.AddTypeHandler(new GenericEnumHandler<StockOutPurpose>());
            SqlMapper.AddTypeHandler(new GenericEnumHandler<CleaningType>());
            SqlMapper.AddTypeHandler(new GenericEnumHandler<CleaningStatus>());
            SqlMapper.AddTypeHandler(new GenericEnumHandler<PackagingTransactionType>());
            SqlMapper.AddTypeHandler(new GenericEnumHandler<PackagingOwnershipType>());
            SqlMapper.AddTypeHandler(new GenericEnumHandler<DeliveryStatus>());
            SqlMapper.AddTypeHandler(new GenericEnumHandler<DeliveryPaymentStatus>());
            SqlMapper.AddTypeHandler(new GenericEnumHandler<ActiveStatus>());
        }

        /// <summary>
        /// Chuyển đổi snake_case sang PascalCase
        /// </summary>
        private static string SnakeCaseToPascalCase(string snakeCase)
        {
            if (string.IsNullOrEmpty(snakeCase))
                return snakeCase;

            return string.Join("", snakeCase.Split('_')
                .Select(part => part.Length > 0
                    ? char.ToUpper(part[0]) + part.Substring(1).ToLower()
                    : part));
        }

        /// <summary>
        /// Generic enum handler cho Dapper
        /// </summary>
        private class GenericEnumHandler<T> : SqlMapper.TypeHandler<T> where T : struct, Enum
        {
            public override void SetValue(IDbDataParameter parameter, T value)
            {
                parameter.Value = value.ToString();
            }

            public override T Parse(object value)
            {
                if (value == null) return default;

                if (value is string stringValue)
                {
                    if (Enum.TryParse<T>(stringValue, true, out var result))
                        return result;
                }
                else if (value is int intValue && Enum.IsDefined(typeof(T), intValue))
                {
                    return (T)Enum.ToObject(typeof(T), intValue);
                }

                return default;
            }
        }

        /// <summary>
        /// DatabaseHelper constructor với connection string tùy chọn.
        /// </summary>
        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
            _dbConfig = ConfigManager.GetDatabaseConfig();
            _logger = LogHelperFactory.CreateLogger("DatabaseHelper");

            // Cũng cấu hình mapping cho constructor này
            ConfigureDapperMapping();
        }

        /// <summary>
        /// Tạo và mở kết nối đến database MySQL.
        /// </summary>
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

        /// <summary>
        /// Kiểm tra kết nối đến database có thành công hay không.
        /// </summary>
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

        /// <summary>
        /// Test mapping cho Order entity
        /// </summary>
        public async Task<bool> TestOrderMappingAsync()
        {
            try
            {
                var sql = @"
                    SELECT 
                        order_id, order_code, customer_id, source, total_amount,
                        discounted_amount, status, payment_status, payment_method,
                        address, note, user_id, last_updated
                    FROM Orders 
                    LIMIT 1";

                var order = await QueryFirstOrDefaultAsync<Order>(sql);

                if (order != null)
                {
                    bool mappingSuccessful = order.OrderId > 0 &&
                                           order.TotalAmount >= 0 &&
                                           order.LastUpdated > DateTime.MinValue;

                    _logger.Info($"Order mapping test - Successful: {mappingSuccessful}, " +
                                $"OrderId: {order.OrderId}, TotalAmount: {order.TotalAmount}, " +
                                $"Status: {order.Status}, Source: {order.Source}");

                    return mappingSuccessful;
                }

                _logger.Warning("Order mapping test - No data found");
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error($"Order mapping test failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Thực hiện truy vấn và trả về danh sách kết quả.
        /// </summary>
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null, IDbTransaction transaction = null)
        {
            if (transaction != null)
            {
                return await transaction.Connection.QueryAsync<T>(sql, parameters, transaction);
            }

            using (var connection = await CreateConnectionAsync())
            {
                try
                {
                    return await connection.QueryAsync<T>(sql, parameters);
                }
                catch (Exception ex)
                {
                    _logger.Error($"QueryAsync error: {sql} - {ex.Message}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Thực hiện truy vấn và trả về bản ghi đầu tiên hoặc mặc định.
        /// </summary>
        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null, IDbTransaction transaction = null)
        {
            if (transaction != null)
            {
                return await transaction.Connection.QueryFirstOrDefaultAsync<T>(sql, parameters, transaction);
            }

            using (var connection = await CreateConnectionAsync())
            {
                try
                {
                    return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
                }
                catch (Exception ex)
                {
                    _logger.Error($"QueryFirstOrDefaultAsync error: {sql} - {ex.Message}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Thực hiện truy vấn và trả về một bản ghi duy nhất hoặc mặc định.
        /// </summary>
        public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object parameters = null, IDbTransaction transaction = null)
        {
            if (transaction != null)
            {
                return await transaction.Connection.QuerySingleOrDefaultAsync<T>(sql, parameters, transaction);
            }

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

        /// <summary>
        /// Thực thi câu lệnh SQL không trả về dữ liệu (INSERT, UPDATE, DELETE).
        /// </summary>
        public async Task<int> ExecuteAsync(string sql, object parameters = null, IDbTransaction transaction = null)
        {
            if (transaction != null)
            {
                return await transaction.Connection.ExecuteAsync(sql, parameters, transaction);
            }

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

        /// <summary>
        /// Thực thi stored procedure và trả về danh sách kết quả.
        /// </summary>
        public async Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string procedureName, object parameters = null, IDbTransaction transaction = null)
        {
            if (transaction != null)
            {
                return await transaction.Connection.QueryAsync<T>(
                    procedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    transaction: transaction
                );
            }

            using (var connection = await CreateConnectionAsync())
            {
                try
                {
                    return await connection.QueryAsync<T>(
                        procedureName,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }
                catch (Exception ex)
                {
                    _logger.Error($"ExecuteStoredProcedureAsync<T> error: {procedureName} - {ex.Message}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Thực thi stored procedure và trả về số lượng bản ghi bị ảnh hưởng.
        /// </summary>
        public async Task<int> ExecuteStoredProcedureAsync(string procedureName, object parameters = null, IDbTransaction transaction = null)
        {
            if (transaction != null)
            {
                return await transaction.Connection.ExecuteAsync(
                    procedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    transaction: transaction
                );
            }

            using (var connection = await CreateConnectionAsync())
            {
                try
                {
                    return await connection.ExecuteAsync(
                        procedureName,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }
                catch (Exception ex)
                {
                    _logger.Error($"ExecuteStoredProcedureAsync error: {procedureName} - {ex.Message}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Bắt đầu một transaction mới trên connection mặc định.
        /// </summary>
        public async Task<IDbTransaction> BeginTransactionAsync()
        {
            var connection = await CreateConnectionAsync();
            return connection.BeginTransaction();
        }

        /// <summary>
        /// Bắt đầu một transaction mới với IsolationLevel tùy chọn.
        /// </summary>
        public async Task<IDbTransaction> BeginTransactionAsync(IsolationLevel isolationLevel)
        {
            var connection = await CreateConnectionAsync();
            return connection.BeginTransaction(isolationLevel);
        }

        /// <summary>
        /// Thực hiện truy vấn nhiều kết quả trong cùng một câu lệnh SQL.
        /// </summary>
        public async Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object parameters = null, IDbTransaction transaction = null)
        {
            if (transaction != null)
            {
                return await transaction.Connection.QueryMultipleAsync(sql, parameters, transaction);
            }

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

        /// <summary>
        /// Thực thi truy vấn trả về một giá trị duy nhất.
        /// </summary>
        public async Task<T> ExecuteScalarAsync<T>(string sql, object parameters = null, IDbTransaction transaction = null)
        {
            if (transaction != null)
            {
                return await transaction.Connection.ExecuteScalarAsync<T>(sql, parameters, transaction);
            }

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