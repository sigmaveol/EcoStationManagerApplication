using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public interface IDatabaseHelper
    {
        // === CONNECTION MANAGEMENT ===
        Task<IDbConnection> CreateConnectionAsync();
        Task<bool> TestConnectionAsync();

        // === BASIC DAPPER OPERATIONS ===
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null, IDbTransaction transaction = null);
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null, IDbTransaction transaction = null);
        Task<T> QuerySingleOrDefaultAsync<T>(string sql, object parameters = null, IDbTransaction transaction = null);
        Task<int> ExecuteAsync(string sql, object parameters = null, IDbTransaction transaction = null);

        // === ADVANCED OPERATIONS ===
        Task<Dapper.SqlMapper.GridReader> QueryMultipleAsync(string sql, object parameters = null, IDbTransaction transaction = null);
        Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string procedureName, object parameters = null, IDbTransaction transaction = null);
        Task<int> ExecuteStoredProcedureAsync(string procedureName, object parameters = null, IDbTransaction transaction = null);

        // === TRANSACTION SUPPORT ===
        Task<IDbTransaction> BeginTransactionAsync();
        Task<IDbTransaction> BeginTransactionAsync(IsolationLevel isolationLevel);

        // === UTILITY METHODS ===
        Task<T> ExecuteScalarAsync<T>(string sql, object parameters = null, IDbTransaction transaction = null);
    }

}
