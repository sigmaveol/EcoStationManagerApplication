using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    public interface IConfigurationService
    {
        Task SaveConnectionString(string server, string database, string username, string password);
        Task<bool> TestConnection(string connectionString = null);
        Task<Dictionary<string, object>> GetSystemSettings();
        Task UpdateSystemSetting(string key, object value);
        Task ResetToDefaults();
    }

    public class ConfigurationService : IConfigurationService
    {
        private readonly IDbHelper _dbHelper;

        public ConfigurationService(IDbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public async Task SaveConnectionString(string server, string database, string username, string password)
        {
            var connectionString = $"Server={server};Database={database};Uid={username};Pwd={password};CharSet=utf8mb4;Connection Timeout=30;";

            // Test connection first
            if (!await TestConnection(connectionString))
                throw new ConfigurationException("Không thể kết nối với database với thông tin đã cung cấp");

            // Update app.config
            UpdateAppConfigConnectionString(connectionString);

            // Update system settings in database
            await UpdateDatabaseConnectionSettings(server, database, username);
        }

        public async Task<bool> TestConnection(string connectionString = null)
        {
            try
            {
                var testDbHelper = connectionString == null ?
                    _dbHelper : new DbHelper(connectionString);

                using (var connection = await testDbHelper.GetOpenConnectionAsync())
                {
                    var result = await testDbHelper.ExecuteScalarAsync<int>("SELECT 1");
                    return result == 1;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<Dictionary<string, object>> GetSystemSettings()
        {
            var settings = new Dictionary<string, object>();

            // From app.config
            settings.Add("ApplicationName", AppConfigHelper.ApplicationName);
            settings.Add("Version", AppConfigHelper.Version);
            settings.Add("BackupPath", AppConfigHelper.BackupPath);
            settings.Add("LowStockThreshold", AppConfigHelper.LowStockThreshold);

            // From database
            try
            {
                var dbSettings = await _dbHelper.QueryAsync<dynamic>(
                    "SELECT setting_key, setting_value FROM SystemSettings");

                foreach (var setting in dbSettings)
                {
                    settings.Add(setting.setting_key, setting.setting_value);
                }
            }
            catch
            {
                // Database might not be ready yet
            }

            return settings;
        }

        public async Task UpdateSystemSetting(string key, object value)
        {
            // Update in database
            await _dbHelper.ExecuteAsync(
                @"INSERT INTO SystemSettings (setting_key, setting_value, updated_date) 
                  VALUES (@Key, @Value, NOW()) 
                  ON DUPLICATE KEY UPDATE setting_value = @Value, updated_date = NOW()",
                new { Key = key, Value = value.ToString() });
        }

        private void UpdateAppConfigConnectionString(string connectionString)
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");

                connectionStringsSection.ConnectionStrings["EcoStationDB"].ConnectionString = connectionString;

                config.Save();
                ConfigurationManager.RefreshSection("connectionStrings");
            }
            catch (Exception ex)
            {
                throw new ConfigurationException($"Lỗi cập nhật cấu hình: {ex.Message}", ex);
            }
        }
    }

    public class ConfigurationException : Exception
    {
        public ConfigurationException(string message) : base(message) { }
        public ConfigurationException(string message, Exception innerException) : base(message, innerException) { }
    }
}