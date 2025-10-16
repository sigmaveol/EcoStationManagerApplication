using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace EcoStationManagerApplication.DAL.Database
{
    public class DatabaseHelper : IDisposable
    {
        private readonly string _connectionString;
        private IDbConnection _connection;
        private bool _disposed = false;

        public DatabaseHelper()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["EcoStationDB"]?.ConnectionString;

            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException($"Connection string {"EcoStationDB"} not found in configuration file.");
            }
        }

        /// <summary>
        /// Gets or creates a database connection
        /// </summary>
        public IDbConnection GetConnection() {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                _connection = new MySqlConnection(_connectionString);
                _connection.Open();
            }
            return _connection;
        }

        /// <summary>
        /// Query multiple entities
        /// </summary>
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null)
        {
            using (var connection = GetConnection())
                return await connection.QueryAsync<T>(sql, parameters);
        }

        /// <summary>
        /// Query single entity
        /// </summary>
        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null)
        {
            using (var connection = GetConnection())
                return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
        }

        /// <summary>
        /// Execute command (INSERT, UPDATE, DELETE)
        /// </summary>
        public async Task<int> ExecuteAsync(string sql, object parameters = null)
        {
            using (var connection = GetConnection())
                return await connection.ExecuteAsync(sql, parameters);
        }

        /// <summary>
        /// Execute scalar
        /// </summary>
        public async Task<T> ExecuteScalarAsync<T>(string sql, object parameters = null)
        {
            using (var connection = GetConnection())
                return await connection.ExecuteScalarAsync<T>(sql, parameters);
        }

        /// <summary>
        /// Query multiple result sets
        /// </summary>
        public async Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object parameters = null)
        {
            var connection = GetConnection();
            return await connection.QueryMultipleAsync(sql, parameters);
        }

        /// <summary>
        /// Begin transaction
        /// </summary>
        public IDbTransaction BeginTransaction()
        {
            var connection = GetConnection();
            return connection.BeginTransaction();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _connection?.Close();
                    _connection?.Dispose();
                }
                _disposed = true;
            }
        }
    }



}
