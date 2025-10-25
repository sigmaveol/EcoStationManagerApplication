using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface IStationService : IService<Station>
    {
        Task<IEnumerable<Station>> GetActiveStationsAsync();
        Task<IEnumerable<Station>> GetStationsByTypeAsync(string stationType);
        Task<IEnumerable<Station>> GetChildStationsAsync(int parentStationId);
        Task<IEnumerable<Station>> GetStationsByManagerAsync(int managerId);
        Task<bool> UpdateStationStatusAsync(int stationId, bool isActive);
        Task<Station> CreateStationAsync(Station station);
        Task<IEnumerable<Station>> GetStationsWithInventoryAsync();
    }

    public interface ITankService : IService<Tank>
    {
        Task<IEnumerable<Tank>> GetTanksByStationAsync(int stationId);
        Task<Tank> GetTankByNameAsync(string name, int stationId);
        Task<bool> UpdateTankLevelAsync(int tankId, decimal currentLevel);
        Task<IEnumerable<Tank>> GetTanksNeedCleaningAsync();
        Task<bool> UpdateTankStatusAsync(int tankId, string status);
        Task<IEnumerable<Tank>> GetLowLevelTanksAsync(decimal threshold);
        Task<bool> ScheduleTankCleaningAsync(int tankId, DateTime cleaningDate);
        Task<Tank> CreateTankAsync(Tank tank);
    }

    public interface ICleaningService : IService<CleaningSchedule>
    {
        Task<IEnumerable<CleaningSchedule>> GetCleaningSchedulesByStationAsync(int stationId);
        Task<IEnumerable<CleaningSchedule>> GetOverdueCleaningSchedulesAsync();
        Task<bool> UpdateCleaningStatusAsync(int cleaningScheduleId, string status);
        Task<IEnumerable<CleaningSchedule>> GetSchedulesByDateRangeAsync(DateTime start, DateTime end);
        Task<IEnumerable<CleaningSchedule>> GetPendingCleaningSchedulesAsync();
        Task<bool> CompleteCleaningAsync(int cleaningScheduleId, int cleanedBy, string notes);
        Task<bool> ScheduleCleaningAsync(CleaningSchedule schedule);
    }
}