using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface ISystemSettingService : IService<SystemSetting>
    {
        Task<SystemSetting> GetSettingByKeyAsync(string key);
        Task<bool> UpdateSettingAsync(string key, string value);
        Task<Dictionary<string, string>> GetAllSettingsAsync();
        Task<bool> UpdateSettingWithDataTypeAsync(string key, string value, string dataType);
        Task<T> GetSettingValueAsync<T>(string key, T defaultValue = default);
        Task<bool> InitializeDefaultSettingsAsync();
        Task<bool> BackupSettingsAsync(); // THIẾU
    }

    public interface IModuleService : IService<Module>
    {
        Task<IEnumerable<Module>> GetActiveModulesAsync();
        Task<bool> UpdateModuleStatusAsync(string moduleKey, bool isActive);
        Task<Module> GetModuleByKeyAsync(string moduleKey);
        Task<IEnumerable<Module>> GetModulesByStatusAsync(bool isActive);
        Task<bool> InitializeModulesAsync();
        Task<bool> ValidateModuleConfigurationAsync(string moduleKey); // THIẾU
    }
}