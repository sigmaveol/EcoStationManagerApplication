using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Database;
using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{
    public class SystemSettingRepository : BaseRepository<SystemSetting>, ISystemSettingRepository
    {
        public SystemSettingRepository() : base("SystemSettings", "ss_id", false) { }

        public SystemSettingRepository(IDbHelper dbHelper) : base(dbHelper, "SystemSettings", "ss_id", false) { }

        public async Task<SystemSetting> GetSettingByKeyAsync(string key)
        {
            var sql = "SELECT * FROM SystemSettings WHERE setting_key = @Key";
            return await _dbHelper.QueryFirstOrDefaultAsync<SystemSetting>(sql, new { Key = key });
        }

        public async Task<bool> UpdateSettingAsync(string key, string value)
        {
            var sql = @"
                UPDATE SystemSettings 
                SET setting_value = @Value, updated_date = NOW() 
                WHERE setting_key = @Key";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new { Key = key, Value = value });
            return affectedRows > 0;
        }

        public async Task<Dictionary<string, string>> GetAllSettingsAsync()
        {
            var sql = "SELECT setting_key, setting_value FROM SystemSettings";
            var settings = await _dbHelper.QueryAsync<(string Key, string Value)>(sql);

            var result = new Dictionary<string, string>();
            foreach (var setting in settings)
            {
                result[setting.Key] = setting.Value;
            }
            return result;
        }

        public async Task<string> GetSettingValueAsync(string key, string defaultValue = "")
        {
            var setting = await GetSettingByKeyAsync(key);
            return setting?.SettingValue ?? defaultValue;
        }

        public async Task<bool> UpdateSettingWithTypeAsync(string key, string value, string dataType)
        {
            var sql = @"
                UPDATE SystemSettings 
                SET setting_value = @Value, data_type = @DataType, updated_date = NOW() 
                WHERE setting_key = @Key";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new { Key = key, Value = value, DataType = dataType });
            return affectedRows > 0;
        }
    }

    public class ModuleRepository : BaseRepository<Module>, IModuleRepository
    {
        public ModuleRepository() : base("Modules", "module_id", true) { }

        public ModuleRepository(IDbHelper dbHelper) : base(dbHelper, "Modules", "module_id", true) { }

        public async Task<IEnumerable<Module>> GetActiveModulesAsync()
        {
            var sql = "SELECT * FROM Modules WHERE is_active = 1 ORDER BY module_id";
            return await _dbHelper.QueryAsync<Module>(sql);
        }

        public async Task<bool> UpdateModuleStatusAsync(string moduleKey, bool isActive)
        {
            var sql = "UPDATE Modules SET is_active = @IsActive WHERE module_key = @ModuleKey";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new { ModuleKey = moduleKey, IsActive = isActive });
            return affectedRows > 0;
        }

        public async Task<Module> GetModuleByKeyAsync(string moduleKey)
        {
            var sql = "SELECT * FROM Modules WHERE module_key = @ModuleKey";
            return await _dbHelper.QueryFirstOrDefaultAsync<Module>(sql, new { ModuleKey = moduleKey });
        }

        public async Task<bool> IsModuleActiveAsync(string moduleKey)
        {
            var sql = "SELECT COUNT(*) FROM Modules WHERE module_key = @ModuleKey AND is_active = 1";
            var count = await _dbHelper.ExecuteScalarAsync<int>(sql, new { ModuleKey = moduleKey });
            return count > 0;
        }

        public async Task<IEnumerable<Module>> GetAllModulesAsync()
        {
            var sql = "SELECT * FROM Modules ORDER BY module_id";
            return await _dbHelper.QueryAsync<Module>(sql);
        }
    }
}