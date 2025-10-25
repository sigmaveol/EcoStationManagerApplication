using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Database;
using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{
    public class StationRepository : BaseRepository<Station>, IStationRepository
    {
        public StationRepository() : base("Stations", "station_id", true) { }

        public StationRepository(IDbHelper dbHelper) : base(dbHelper, "Stations", "station_id", true) { }

        public async Task<IEnumerable<Station>> GetActiveStationsAsync()
        {
            var sql = "SELECT * FROM Stations WHERE is_active = 1 ORDER BY name";
            return await _dbHelper.QueryAsync<Station>(sql);
        }

        public async Task<IEnumerable<Station>> GetStationsByTypeAsync(string stationType)
        {
            var sql = "SELECT * FROM Stations WHERE station_type = @StationType AND is_active = 1 ORDER BY name";
            return await _dbHelper.QueryAsync<Station>(sql, new { StationType = stationType });
        }

        public async Task<IEnumerable<Station>> GetChildStationsAsync(int parentStationId)
        {
            var sql = "SELECT * FROM Stations WHERE parent_station_id = @ParentStationId AND is_active = 1 ORDER BY name";
            return await _dbHelper.QueryAsync<Station>(sql, new { ParentStationId = parentStationId });
        }

        public async Task<Station> GetStationByNameAsync(string name)
        {
            var sql = "SELECT * FROM Stations WHERE name = @Name AND is_active = 1";
            return await _dbHelper.QueryFirstOrDefaultAsync<Station>(sql, new { Name = name });
        }

        public async Task<bool> UpdateStationManagerAsync(int stationId, int managerId)
        {
            var sql = "UPDATE Stations SET manager = @ManagerId, updated_date = NOW() WHERE station_id = @StationId";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new { StationId = stationId, ManagerId = managerId });
            return affectedRows > 0;
        }

        public async Task<IEnumerable<Station>> GetStationsByManagerAsync(int managerId)
        {
            var sql = "SELECT * FROM Stations WHERE manager = @ManagerId AND is_active = 1 ORDER BY name";
            return await _dbHelper.QueryAsync<Station>(sql, new { ManagerId = managerId });
        }
    }

    public class TankRepository : BaseRepository<Tank>, ITankRepository
    {
        public TankRepository() : base("Tanks", "tank_id", false) { }

        public TankRepository(IDbHelper dbHelper) : base(dbHelper, "Tanks", "tank_id", false) { }

        public async Task<IEnumerable<Tank>> GetTanksByStationAsync(int stationId)
        {
            var sql = @"
                SELECT t.*, s.name as StationName
                FROM Tanks t
                LEFT JOIN Stations s ON t.station_id = s.station_id
                WHERE t.station_id = @StationId
                ORDER BY t.name";
            return await _dbHelper.QueryAsync<Tank>(sql, new { StationId = stationId });
        }

        public async Task<Tank> GetTankByNameAsync(string name, int stationId)
        {
            var sql = "SELECT * FROM Tanks WHERE name = @Name AND station_id = @StationId";
            return await _dbHelper.QueryFirstOrDefaultAsync<Tank>(sql, new { Name = name, StationId = stationId });
        }

        public async Task<bool> UpdateTankLevelAsync(int tankId, decimal currentLevel)
        {
            var sql = "UPDATE Tanks SET current_level = @CurrentLevel WHERE tank_id = @TankId";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new { TankId = tankId, CurrentLevel = currentLevel });
            return affectedRows > 0;
        }

        public async Task<IEnumerable<Tank>> GetTanksNeedCleaningAsync()
        {
            var sql = @"
                SELECT t.*, s.name as StationName
                FROM Tanks t
                LEFT JOIN Stations s ON t.station_id = s.station_id
                WHERE t.next_clean_date <= CURDATE()
                AND t.status = 'active'
                ORDER BY t.next_clean_date";
            return await _dbHelper.QueryAsync<Tank>(sql);
        }

        public async Task<bool> UpdateTankCleaningDateAsync(int tankId, DateTime lastCleanDate, DateTime nextCleanDate)
        {
            var sql = @"
                UPDATE Tanks 
                SET last_clean_date = @LastCleanDate, next_clean_date = @NextCleanDate 
                WHERE tank_id = @TankId";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new
            {
                TankId = tankId,
                LastCleanDate = lastCleanDate,
                NextCleanDate = nextCleanDate
            });
            return affectedRows > 0;
        }

        public async Task<bool> UpdateTankStatusAsync(int tankId, string status)
        {
            var sql = "UPDATE Tanks SET status = @Status WHERE tank_id = @TankId";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new { TankId = tankId, Status = status });
            return affectedRows > 0;
        }
    }

    public class CleaningScheduleRepository : BaseRepository<CleaningSchedule>, ICleaningScheduleRepository
    {
        public CleaningScheduleRepository() : base("CleaningSchedules", "cs_id", false) { }

        public CleaningScheduleRepository(IDbHelper dbHelper) : base(dbHelper, "CleaningSchedules", "cs_id", false) { }

        public async Task<IEnumerable<CleaningSchedule>> GetCleaningSchedulesByStationAsync(int stationId)
        {
            var sql = @"
                SELECT cs.*, 
                       s.name as StationName,
                       u.fullname as CleaningByName
                FROM CleaningSchedules cs
                LEFT JOIN Stations s ON cs.station_id = s.station_id
                LEFT JOIN Users u ON cs.cleaning_by = u.user_id
                WHERE cs.station_id = @StationId
                ORDER BY cs.cleaning_date DESC";
            return await _dbHelper.QueryAsync<CleaningSchedule>(sql, new { StationId = stationId });
        }

        public async Task<IEnumerable<CleaningSchedule>> GetOverdueCleaningSchedulesAsync()
        {
            var sql = @"
                SELECT cs.*, 
                       s.name as StationName,
                       u.fullname as CleaningByName
                FROM CleaningSchedules cs
                LEFT JOIN Stations s ON cs.station_id = s.station_id
                LEFT JOIN Users u ON cs.cleaning_by = u.user_id
                WHERE cs.cleaning_date < CURDATE()
                AND cs.status = 'scheduled'
                ORDER BY cs.cleaning_date";
            return await _dbHelper.QueryAsync<CleaningSchedule>(sql);
        }

        public async Task<bool> UpdateCleaningStatusAsync(int cleaningScheduleId, string status)
        {
            var sql = "UPDATE CleaningSchedules SET status = @Status WHERE cs_id = @CleaningScheduleId";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new { CleaningScheduleId = cleaningScheduleId, Status = status });
            return affectedRows > 0;
        }

        public async Task<IEnumerable<CleaningSchedule>> GetSchedulesByDateRangeAsync(DateTime start, DateTime end)
        {
            var sql = @"
                SELECT cs.*, 
                       s.name as StationName,
                       u.fullname as CleaningByName
                FROM CleaningSchedules cs
                LEFT JOIN Stations s ON cs.station_id = s.station_id
                LEFT JOIN Users u ON cs.cleaning_by = u.user_id
                WHERE cs.cleaning_date BETWEEN @Start AND @End
                ORDER BY cs.cleaning_date, cs.station_id";
            return await _dbHelper.QueryAsync<CleaningSchedule>(sql, new { Start = start, End = end });
        }

        public async Task<bool> CompleteCleaningAsync(int cleaningScheduleId, int cleanedBy, string notes = null)
        {
            var sql = @"
                UPDATE CleaningSchedules 
                SET status = 'completed', cleaning_by = @CleanedBy, notes = @Notes
                WHERE cs_id = @CleaningScheduleId";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new
            {
                CleaningScheduleId = cleaningScheduleId,
                CleanedBy = cleanedBy,
                Notes = notes
            });
            return affectedRows > 0;
        }
    }

}