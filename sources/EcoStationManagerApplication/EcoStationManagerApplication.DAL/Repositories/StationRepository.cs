using Dapper;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.DAL.SqlQueries;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{
    public class StationRepository : BaseRepository<Station>, IStationRepository
    {
        public StationRepository(IDatabaseHelper databaseHelper)
            : base(databaseHelper, "Stations", "station_id")
        {
        }

        public async Task<IEnumerable<Station>> GetActiveStationsAsync()
        {
            try
            {
                return await _databaseHelper.QueryAsync<Station>(StationQueries.GetActive);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetActiveStationsAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Station>> GetByManagerAsync(int managerId)
        {
            try
            {
                return await _databaseHelper.QueryAsync<Station>(
                    StationQueries.GetByManager,
                    new { ManagerId = managerId }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByManagerAsync error - ManagerId: {managerId} - {ex.Message}");
                throw;
            }
        }

        public async Task<Station> GetByNameAsync(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return null;

                return await _databaseHelper.QueryFirstOrDefaultAsync<Station>(
                    StationQueries.GetByName,
                    new { Name = name }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByNameAsync error - Name: {name} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Station>> SearchStationsAsync(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return await GetAllAsync();

                return await _databaseHelper.QueryAsync<Station>(
                    StationQueries.Search,
                    new { Keyword = $"%{keyword}%" }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"SearchStationsAsync error - Keyword: {keyword} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ToggleStatusAsync(int stationId, bool isActive)
        {
            try
            {
                if (stationId <= 0)
                    return false;

                var affectedRows = await _databaseHelper.ExecuteAsync(
                    StationQueries.ToggleStatus,
                    new { StationId = stationId, IsActive = isActive, UpdatedDate = DateTime.Now }
                );

                if (affectedRows > 0)
                {
                    var status = isActive ? "kích hoạt" : "vô hiệu hóa";
                    _logger.Info($"Đã {status} trạm - StationId: {stationId}");
                }

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"ToggleStatusAsync error - StationId: {stationId}, IsActive: {isActive} - {ex.Message}");
                throw;
            }
        }
    }
}

