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
            private static readonly ILogHelper _logger = LogHelperFactory.CreateLogger($"EnumHandler<{typeof(T).Name}>");

            public override void SetValue(IDbDataParameter parameter, T value)
            {
                // Convert enum sang string để lưu vào database
                // Đảm bảo giá trị không null
                if (EqualityComparer<T>.Default.Equals(value, default(T)))
                {
                    // Nếu là default value, sử dụng giá trị mặc định của enum (thường là giá trị đầu tiên)
                    var enumValues = Enum.GetValues(typeof(T));
                    if (enumValues.Length > 0)
                    {
                        parameter.Value = enumValues.GetValue(0).ToString();
                    }
                    else
                    {
                        parameter.Value = value.ToString();
                    }
                }
                else
                {
                    parameter.Value = value.ToString();
                }
                
                // Log để debug
                _logger.Info($"SetValue for {typeof(T).Name}: {value} -> '{parameter.Value}'");
            }

            public override T Parse(object value)
            {
                try
                {
                    // Xử lý null
                    if (value == null || value == DBNull.Value)
                        return default;

                    // Xử lý empty string hoặc chỉ có khoảng trắng
                    if (value is string stringValue)
                    {
                        // Trim và kiểm tra empty
                        var trimmedValue = stringValue?.Trim() ?? "";
                        if (string.IsNullOrEmpty(trimmedValue))
                        {
                            _logger.Warning($"Empty or whitespace enum value for {typeof(T).Name}, using default: {default(T)}");
                            return default;
                        }

                        // Thử parse enum (case-insensitive)
                        if (Enum.TryParse<T>(trimmedValue, true, out var result))
                            return result;

                        // Nếu không parse được, log warning và return default
                        _logger.Warning($"Cannot parse '{trimmedValue}' as {typeof(T).Name}, using default: {default(T)}");
                        return default;
                    }
                    
                    // Xử lý int
                    if (value is int intValue)
                    {
                        if (Enum.IsDefined(typeof(T), intValue))
                        {
                            return (T)Enum.ToObject(typeof(T), intValue);
                        }
                        else
                        {
                            _logger.Warning($"Integer value {intValue} is not defined in {typeof(T).Name}, using default: {default(T)}");
                            return default;
                        }
                    }

                    // Fallback: return default
                    _logger.Warning($"Unexpected value type {value?.GetType().Name} for {typeof(T).Name}, using default: {default(T)}");
                    return default;
                }
                catch (Exception ex)
                {
                    // Nếu có exception, log và return default thay vì throw
                    _logger.Error($"Error parsing {typeof(T).Name} from value '{value}': {ex.Message}");
                    return default;
                }
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
                catch (System.Data.DataException dataEx)
                {
                    // Xử lý lỗi parse (thường do enum hoặc type conversion)
                    _logger.Error($"QueryAsync DataException: {sql} - {dataEx.Message}");
                    if (dataEx.InnerException != null)
                    {
                        _logger.Error($"Inner exception: {dataEx.InnerException.Message}");
                    }
                    
                    // Nếu là lỗi parse enum, thử query raw và xử lý thủ công
                    // Hoặc return empty list để tránh crash
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
                catch (System.Data.DataException dataEx)
                {
                    // Xử lý lỗi parse (thường do enum hoặc type conversion)
                    _logger.Error($"QueryFirstOrDefaultAsync DataException: {sql} - {dataEx.Message}");
                    if (dataEx.InnerException != null)
                    {
                        _logger.Error($"Inner exception: {dataEx.InnerException.Message}");
                    }
                    
                    // Return default value để tránh crash
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