using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface IStationRepository : IRepository<Station>
    {
        Task<IEnumerable<Station>> GetActiveStationsAsync();
        Task<IEnumerable<Station>> GetStationsByTypeAsync(string stationType);
        Task<IEnumerable<Station>> GetChildStationsAsync(int parentStationId);
        Task<IEnumerable<Station>> GetStationsByManagerAsync(int managerId);
        Task<bool> UpdateStationStatusAsync(int stationId, bool isActive);
        // THIẾU: GetStationWithUsersAsync
        Task<Station> GetStationWithUsersAsync(int stationId); // ĐÃ THÊM
    }

    public interface ITankRepository : IRepository<Tank>
    {
        Task<IEnumerable<Tank>> GetTanksByStationAsync(int stationId);
        Task<Tank> GetTankByNameAsync(string name, int stationId);
        Task<bool> UpdateTankLevelAsync(int tankId, decimal currentLevel);
        Task<IEnumerable<Tank>> GetTanksNeedCleaningAsync();
        Task<bool> UpdateTankStatusAsync(int tankId, string status);
        Task<IEnumerable<Tank>> GetLowLevelTanksAsync(decimal threshold);
        // THIẾU: GetTankUsageStatisticsAsync
        Task<Dictionary<string, decimal>> GetTankUsageStatisticsAsync(int stationId); // ĐÃ THÊM
    }
}