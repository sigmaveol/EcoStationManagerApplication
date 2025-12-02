using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public interface IGoogleIntegrationConfigRepository : IRepository<GoogleIntegrationConfig>
    {
        Task<GoogleIntegrationConfig> GetActiveBySpreadsheetIdAsync(string spreadsheetId);
        Task<GoogleIntegrationConfig> GetLatestActiveAsync();
        Task<int> AddConfigAsync(GoogleIntegrationConfig config);
        Task<bool> UpdateLastSyncAsync(int integrationId, DateTime syncTime);
        Task<bool> SetActiveAsync(int integrationId, bool isActive);
        Task<bool> UpdateConfigFieldsAsync(int integrationId, string apiKey, string sheetName);
    }
}
