using System;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Diagnostics;
using EcoStationManagerApplication.Core.Interfaces;

namespace EcoStationManagerApplication.DAL.Database
{

    public interface IDbHelper : IDisposable
    {
        Task<IDbConnection> GetOpenConnectionAsync();
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null);
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null);
        Task<T> QuerySingleOrDefaultAsync<T>(string sql, object parameters = null);
        Task<int> ExecuteAsync(string sql, object parameters = null);
        Task<T> ExecuteScalarAsync<T>(string sql, object parameters = null);
        Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object parameters = null);
        Task ExecuteInTransactionAsync(Func<IDbConnection, IDbTransaction, Task> action);
    }

    public class DbHelper : IDbHelper
    {
        private readonly string _connectionString;
        private IDbConnection _connection;

        public DbHelper() 
        {
            _connectionString = GetConnectionStringFromConfig();
        }

        public DbHelper(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        private string GetConnectionStringFromConfig()
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["EcoStationDB"]?.ConnectionString;

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Không tìm thấy connection string 'EcoStationDB' trong file app.config");
                }

                return connectionString;
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Lỗi đọc connection string từ app.config: {ex.Message}");
                throw new DatabaseException("Lỗi cấu hình kết nối cơ sở dữ liệu. Vui lòng kiểm tra file app.config.", ex);
            }
        }

        public static string GetAppSetting(string key, string defaultValue = "")
        {
            try
            {
                return ConfigurationManager.AppSettings[key] ?? defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static T GetAppSetting<T>(string key, T defaultValue = default(T))
        {
            try
            {
                var value = ConfigurationManager.AppSettings[key];
                if (string.IsNullOrEmpty(value))
                    return defaultValue;

                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }

        public async Task<IDbConnection> GetOpenConnectionAsync()
        {
            try
            {
                if (_connection == null || _connection.State != ConnectionState.Open)
                {
                    _connection = new MySqlConnection(_connectionString);
                    await ((MySqlConnection)_connection).OpenAsync();
                }
                return _connection;
            }
            catch (MySqlException ex)
            {
                Trace.TraceError($"MySQL error while opening connection: {ex.Message}");
                throw new DatabaseException("Lỗi kết nối cơ sở dữ liệu. Vui lòng kiểm tra kết nối mạng và thử lại.", ex);
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Unexpected error while opening connection: {ex.Message}");
                throw new DatabaseException("Lỗi không mong muốn khi kết nối cơ sở dữ liệu.", ex);
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null)
        {
            using (var connection = await GetOpenConnectionAsync())
            {
                try
                {
                    Trace.TraceInformation($"Executing query: {sql}");
                    return await connection.QueryAsync<T>(sql, parameters);
                }
                catch (MySqlException ex)
                {
                    Trace.TraceError($"MySQL error executing query: {ex.Message}");
                    throw new DatabaseException("Lỗi truy vấn dữ liệu. Vui lòng thử lại.", ex);
                }
                catch (Exception ex)
                {
                    Trace.TraceError($"Unexpected error executing query: {ex.Message}");
                    throw new DatabaseException("Lỗi không mong muốn khi truy vấn dữ liệu.", ex);
                }
            }
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null)
        {
            using (var connection = await GetOpenConnectionAsync())
            {
                try
                {
                    Trace.TraceInformation($"Executing query (FirstOrDefault): {sql}");
                    return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
                }
                catch (MySqlException ex)
                {
                    Trace.TraceError($"MySQL error executing query: {ex.Message}");
                    throw new DatabaseException("Lỗi truy vấn dữ liệu. Vui lòng thử lại.", ex);
                }
                catch (Exception ex)
                {
                    Trace.TraceError($"Unexpected error executing query: {ex.Message}");
                    throw new DatabaseException("Lỗi không mong muốn khi truy vấn dữ liệu.", ex);
                }
            }
        }

        public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object parameters = null)
        {
            using (var connection = await GetOpenConnectionAsync())
            {
                try
                {
                    Trace.TraceInformation($"Executing query (SingleOrDefault): {sql}");
                    return await connection.QuerySingleOrDefaultAsync<T>(sql, parameters);
                }
                catch (MySqlException ex)
                {
                    Trace.TraceError($"MySQL error executing query: {ex.Message}");
                    throw new DatabaseException("Lỗi truy vấn dữ liệu. Vui lòng thử lại.", ex);
                }
                catch (Exception ex)
                {
                    Trace.TraceError($"Unexpected error executing query: {ex.Message}");
                    throw new DatabaseException("Lỗi không mong muốn khi truy vấn dữ liệu.", ex);
                }
            }
        }

        public async Task<int> ExecuteAsync(string sql, object parameters = null)
        {
            using (var connection = await GetOpenConnectionAsync())
            {
                try
                {
                    Trace.TraceInformation($"Executing command: {sql}");
                    return await connection.ExecuteAsync(sql, parameters);
                }
                catch (MySqlException ex)
                {
                    Trace.TraceError($"MySQL error executing command: {ex.Message}");

                    // Xử lý các lỗi MySQL phổ biến
                    switch (ex.Number)
                    {
                        case 1062: // Duplicate entry
                            throw new DatabaseException("Dữ liệu đã tồn tại trong hệ thống.", ex);
                        case 1451: // Foreign key constraint
                            throw new DatabaseException("Không thể xóa dữ liệu vì có dữ liệu liên quan.", ex);
                        case 1452: // Cannot add foreign key constraint
                            throw new DatabaseException("Dữ liệu tham chiếu không tồn tại.", ex);
                        default:
                            throw new DatabaseException("Lỗi thực thi lệnh cơ sở dữ liệu.", ex);
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError($"Unexpected error executing command: {ex.Message}");
                    throw new DatabaseException("Lỗi không mong muốn khi thực thi lệnh.", ex);
                }
            }
        }

        public async Task<T> ExecuteScalarAsync<T>(string sql, object parameters = null)
        {
            using (var connection = await GetOpenConnectionAsync())
            {
                try
                {
                    Trace.TraceInformation($"Executing scalar: {sql}");
                    return await connection.ExecuteScalarAsync<T>(sql, parameters);
                }
                catch (MySqlException ex)
                {
                    Trace.TraceError($"MySQL error executing scalar: {ex.Message}");
                    throw new DatabaseException("Lỗi thực thi lệnh. Vui lòng thử lại.", ex);
                }
                catch (Exception ex)
                {
                    Trace.TraceError($"Unexpected error executing scalar: {ex.Message}");
                    throw new DatabaseException("Lỗi không mong muốn khi thực thi lệnh.", ex);
                }
            }
        }

        public async Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object parameters = null)
        {
            var connection = await GetOpenConnectionAsync();
            try
            {
                Trace.TraceInformation($"Executing query multiple: {sql}");
                return await connection.QueryMultipleAsync(sql, parameters);
            }
            catch (MySqlException ex)
            {
                connection.Dispose();
                Trace.TraceError($"MySQL error executing query multiple: {ex.Message}");
                throw new DatabaseException("Lỗi truy vấn dữ liệu. Vui lòng thử lại.", ex);
            }
            catch (Exception ex)
            {
                connection.Dispose();
                Trace.TraceError($"Unexpected error executing query multiple: {ex.Message}");
                throw new DatabaseException("Lỗi không mong muốn khi truy vấn dữ liệu.", ex);
            }
        }

        public async Task ExecuteInTransactionAsync(Func<IDbConnection, IDbTransaction, Task> action)
        {
            using (var connection = await GetOpenConnectionAsync())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        Trace.TraceInformation("Beginning transaction");
                        await action(connection, transaction);
                        transaction.Commit();
                        Trace.TraceInformation("Transaction committed");
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError($"Transaction failed, rolling back: {ex.Message}");
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception rollbackEx)
                        {
                            Trace.TraceError($"Error during transaction rollback: {rollbackEx.Message}");
                        }

                        if (ex is MySqlException mySqlEx)
                        {
                            switch (mySqlEx.Number)
                            {
                                case 1062:
                                    throw new DatabaseException("Dữ liệu đã tồn tại trong hệ thống.", mySqlEx);
                                case 1451:
                                    throw new DatabaseException("Không thể xóa dữ liệu vì có dữ liệu liên quan.", mySqlEx);
                                case 1452:
                                    throw new DatabaseException("Dữ liệu tham chiếu không tồn tại.", mySqlEx);
                                default:
                                    throw new DatabaseException("Lỗi cơ sở dữ liệu trong giao dịch.", mySqlEx);
                            }
                        }

                        throw new DatabaseException("Lỗi trong quá trình xử lý giao dịch.", ex);
                    }
                }
            }
        }

        public void Dispose()
        {
            try
            {
                _connection?.Close();
                _connection?.Dispose();
                _connection = null;
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Error disposing database connection: {ex.Message}");
            }
        }
    }

    // Custom exception cho database
    public class DatabaseException : Exception
    {
        public DatabaseException(string message) : base(message) { }
        public DatabaseException(string message, Exception innerException) : base(message, innerException) { }
    }

    // Static helper class để dễ dàng truy cập cấu hình
    public static class AppConfigHelper
    {
        public static string ConnectionString => ConfigurationManager.ConnectionStrings["EcoStationDB"]?.ConnectionString;

        public static string ApplicationName => GetAppSetting("ApplicationName", "EcoStation Manager");
        public static string Version => GetAppSetting("Version", "1.0.0");
        public static string Environment => GetAppSetting("Environment", "Development");

        public static bool EnableLogging => GetAppSetting("EnableLogging", true);
        public static string LogLevel => GetAppSetting("LogLevel", "Information");

        public static string ExportPath => GetAppSetting("ExportPath", @"C:\EcoStation\Exports\");
        public static string BackupPath => GetAppSetting("BackupPath", @"C:\EcoStation\Backups\");
        public static string LogPath => GetAppSetting("LogPath", @"C:\EcoStation\Logs\");

        public static bool AutoBackupEnabled => GetAppSetting("AutoBackupEnabled", true);
        public static int AutoBackupIntervalHours => GetAppSetting("AutoBackupIntervalHours", 24);
        public static int MaxLoginAttempts => GetAppSetting("MaxLoginAttempts", 3);
        public static int SessionTimeoutMinutes => GetAppSetting("SessionTimeoutMinutes", 30);

        public static int LowStockThreshold => GetAppSetting("LowStockThreshold", 10);
        public static int ExpiryWarningDays => GetAppSetting("ExpiryWarningDays", 15);
        public static decimal DefaultTaxRate => GetAppSetting("DefaultTaxRate", 0.1m);
        public static decimal MaxOrderAmount => GetAppSetting("MaxOrderAmount", 10000000m);

        private static T GetAppSetting<T>(string key, T defaultValue)
        {
            try
            {
                var value = ConfigurationManager.AppSettings[key];
                if (string.IsNullOrEmpty(value))
                    return defaultValue;

                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }

        public static DbHelper CreateDbHelper()
        {
            return new DbHelper();
        }
    }
}