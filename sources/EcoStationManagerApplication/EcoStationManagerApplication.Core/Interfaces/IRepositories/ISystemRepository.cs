using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface ISystemSettingRepository : IRepository<SystemSetting>
    {
        Task<SystemSetting> GetSettingByKeyAsync(string key);
        Task<bool> UpdateSettingAsync(string key, string value);
        Task<Dictionary<string, string>> GetAllSettingsAsync();
        Task<bool> UpdateSettingWithDataTypeAsync(string key, string value, string dataType);
        // THIẾU: GetSettingsByPrefixAsync
        Task<Dictionary<string, string>> GetSettingsByPrefixAsync(string prefix); // ĐÃ THÊM
    }

    public interface IModuleRepository : IRepository<Module>
    {
        Task<IEnumerable<Module>> GetActiveModulesAsync();
        Task<bool> UpdateModuleStatusAsync(string moduleKey, bool isActive);
        Task<Module> GetModuleByKeyAsync(string moduleKey);
        Task<IEnumerable<Module>> GetModulesByStatusAsync(bool isActive);
        // THIẾU: GetModuleConfigurationAsync
        Task<Dictionary<string, object>> GetModuleConfigurationAsync(string moduleKey); // ĐÃ THÊM
    }
}