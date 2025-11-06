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
        Task<IDbConnection> CreateConnectionAsync();
        Task<bool> TestConnectionAsync();
        Task<int> ExecuteNonQueryAsync(string sql, object parameters = null);
        Task<T> ExecuteScalarAsync<T>(string sql, object parameters = null);
        Task<DataTable> ExecuteDataTableAsync(string sql, object parameters = null);
    }
}
