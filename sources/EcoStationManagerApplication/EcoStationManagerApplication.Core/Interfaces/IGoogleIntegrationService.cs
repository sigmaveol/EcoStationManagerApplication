using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface IGoogleIntegrationService
    {
        Task<Result<GoogleIntegrationConfig>> GetOrCreateActiveConfigAsync(string sheetUrl, string spreadsheetId, string apiKey, string sheetName = null);
        Task<Result<GoogleIntegrationConfig>> GetLatestActiveConfigAsync();
        Task<Result<bool>> UpdateLastSyncAsync(int integrationId, DateTime syncTime);
        Task<Result<bool>> SaveOrderMappingsAsync(int integrationId, List<(int rowIndex, int orderId)> mappings);
    }
}
