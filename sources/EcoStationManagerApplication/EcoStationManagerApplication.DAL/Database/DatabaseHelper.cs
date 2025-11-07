using Dapper;
using EcoStationManagerApplication.Common.Config;
using EcoStationManagerApplication.Common.Logging;
using EcoStationManagerApplication.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace EcoStationManager.DAL.Database
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

            _logger.Info($"DatabaseHelper initialized - Server: {_dbConfig.Server}, Database: {_dbConfig.Database}");
        }

        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
            _dbConfig = ConfigManager.GetDatabaseConfig();
            _logger = LogHelperFactory.CreateLogger("DatabaseHelper");
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

        public async Task<IDbTransaction> BeginTransactionAsync()
        {
            var connection = await CreateConnectionAsync();
            return connection.BeginTransaction();
        }

        public async Task<IDbTransaction> BeginTransactionAsync(IsolationLevel isolationLevel)
        {
            var connection = await CreateConnectionAsync();
            return connection.BeginTransaction(isolationLevel);
        }

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