using Dapper;
using EcoStationManagerApplication.DAL.Database;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{
    public class GoogleIntegrationConfigRepository : BaseRepository<GoogleIntegrationConfig>, IGoogleIntegrationConfigRepository
    {
        public GoogleIntegrationConfigRepository(IDatabaseHelper databaseHelper)
            : base(databaseHelper, "GoogleIntegrationConfig", "integration_id")
        { }

        public async Task<GoogleIntegrationConfig> GetActiveBySpreadsheetIdAsync(string spreadsheetId)
        {
            var sql = "SELECT * FROM GoogleIntegrationConfig WHERE spreadsheet_id = @SpreadsheetId AND is_active = 1 ORDER BY integration_id DESC LIMIT 1";
            return await _databaseHelper.QueryFirstOrDefaultAsync<GoogleIntegrationConfig>(sql, new { SpreadsheetId = spreadsheetId });
        }

        public async Task<GoogleIntegrationConfig> GetLatestActiveAsync()
        {
            var sql = "SELECT * FROM GoogleIntegrationConfig WHERE is_active = 1 ORDER BY integration_id DESC LIMIT 1";
            return await _databaseHelper.QueryFirstOrDefaultAsync<GoogleIntegrationConfig>(sql, new { });
        }

        public async Task<int> AddConfigAsync(GoogleIntegrationConfig config)
        {
            return await AddAsync(config);
        }

        public async Task<bool> UpdateLastSyncAsync(int integrationId, DateTime syncTime)
        {
            var sql = "UPDATE GoogleIntegrationConfig SET last_sync_time = @SyncTime WHERE integration_id = @Id";
            var rows = await _databaseHelper.ExecuteAsync(sql, new { SyncTime = syncTime, Id = integrationId });
            return rows > 0;
        }

        public async Task<bool> SetActiveAsync(int integrationId, bool isActive)
        {
            var sql = "UPDATE GoogleIntegrationConfig SET is_active = @IsActive WHERE integration_id = @Id";
            var rows = await _databaseHelper.ExecuteAsync(sql, new { IsActive = isActive, Id = integrationId });
            return rows > 0;
        }

        public async Task<bool> UpdateConfigFieldsAsync(int integrationId, string apiKey, string sheetName)
        {
            var sql = "UPDATE GoogleIntegrationConfig SET api_key = @ApiKey, sheet_name = @SheetName WHERE integration_id = @Id";
            var rows = await _databaseHelper.ExecuteAsync(sql, new { ApiKey = apiKey, SheetName = sheetName, Id = integrationId });
            return rows > 0;
        }
    }

    public class GoogleOrderMappingRepository : BaseRepository<GoogleOrderMapping>, IGoogleOrderMappingRepository
    {
        public GoogleOrderMappingRepository(IDatabaseHelper databaseHelper)
            : base(databaseHelper, "GoogleOrderMapping", "id")
        { }

        public async Task<int> AddMappingAsync(GoogleOrderMapping mapping)
        {
            return await AddAsync(mapping);
        }

        public async Task<IEnumerable<GoogleOrderMapping>> GetByOrderIdAsync(int orderId)
        {
            var sql = "SELECT * FROM GoogleOrderMapping WHERE order_id = @OrderId ORDER BY synced_at DESC";
            return await _databaseHelper.QueryAsync<GoogleOrderMapping>(sql, new { OrderId = orderId });
        }

        public async Task<IEnumerable<GoogleOrderMapping>> GetByConfigIdAsync(int configId)
        {
            var sql = "SELECT * FROM GoogleOrderMapping WHERE config_id = @ConfigId ORDER BY synced_at DESC";
            return await _databaseHelper.QueryAsync<GoogleOrderMapping>(sql, new { ConfigId = configId });
        }
    }
}
