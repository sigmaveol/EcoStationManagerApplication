using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace EcoStationManagerApplication.Common.Config
{
    public static class ConnectionHelper
    {
        public static string GetConnectionString()
        {
            return ConfigManager.GetConnectionString();
        }

        public static MySqlConnection CreateConnection()
        {
            var connectionString = GetConnectionString();
            return new MySqlConnection(connectionString);
        }

        public static string GetSafeConnectionString()
        {
            var connectionString = GetConnectionString();
            var builder = new MySqlConnectionStringBuilder(connectionString);

            if (!string.IsNullOrEmpty(builder.Password))
            {
                builder.Password = "***";
            }

            return builder.ToString();
        }

        public static bool TestConnection()
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    connection.Open();
                    return connection.State == ConnectionState.Open;
                }
            }
            catch
            {
                return false;
            }
        }

        public static string GetDatabaseName()
        {
            return ConfigManager.GetDatabaseConfig().Database;
        }

        public static string GetServerName()
        {
            return ConfigManager.GetDatabaseConfig().Server;
        }
    }
}
