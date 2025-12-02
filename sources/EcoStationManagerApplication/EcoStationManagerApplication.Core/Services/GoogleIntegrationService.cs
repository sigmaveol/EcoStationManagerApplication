using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    public class GoogleIntegrationService : BaseService, IGoogleIntegrationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GoogleIntegrationService(IUnitOfWork unitOfWork)
            : base("GoogleIntegrationService")
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<GoogleIntegrationConfig>> GetOrCreateActiveConfigAsync(string sheetUrl, string spreadsheetId, string apiKey, string sheetName = null)
        {
            try
            {
                var existing = await _unitOfWork.GoogleIntegrationConfigs.GetActiveBySpreadsheetIdAsync(spreadsheetId);
                if (existing != null)
                {
                    if (!string.IsNullOrWhiteSpace(apiKey) || !string.IsNullOrWhiteSpace(sheetName))
                    {
                        await _unitOfWork.GoogleIntegrationConfigs.UpdateConfigFieldsAsync(existing.IntegrationId, apiKey, sheetName);
                        existing.ApiKey = string.IsNullOrWhiteSpace(apiKey) ? existing.ApiKey : apiKey;
                        existing.SheetName = string.IsNullOrWhiteSpace(sheetName) ? existing.SheetName : sheetName;
                    }
                    return Result<GoogleIntegrationConfig>.Ok(existing);
                }

                var config = new GoogleIntegrationConfig
                {
                    SheetUrl = sheetUrl,
                    SpreadsheetId = spreadsheetId,
                    ApiKey = apiKey,
                    SheetName = sheetName,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };
                var id = await _unitOfWork.GoogleIntegrationConfigs.AddConfigAsync(config);
                config.IntegrationId = id;
                return Result<GoogleIntegrationConfig>.Ok(config);
            }
            catch (Exception ex)
            {
                return HandleException<GoogleIntegrationConfig>(ex, "tạo/cập nhật cấu hình Google Sheets");
            }
        }

        public async Task<Result<GoogleIntegrationConfig>> GetLatestActiveConfigAsync()
        {
            try
            {
                var config = await _unitOfWork.GoogleIntegrationConfigs.GetLatestActiveAsync();
                if (config == null)
                {
                    return Result<GoogleIntegrationConfig>.Fail("Chưa có cấu hình Google Sheets đang hoạt động");
                }
                return Result<GoogleIntegrationConfig>.Ok(config);
            }
            catch (Exception ex)
            {
                return HandleException<GoogleIntegrationConfig>(ex, "lấy cấu hình Google Sheets");
            }
        }

        public async Task<Result<bool>> UpdateLastSyncAsync(int integrationId, DateTime syncTime)
        {
            try
            {
                var ok = await _unitOfWork.GoogleIntegrationConfigs.UpdateLastSyncAsync(integrationId, syncTime);
                return ok ? Result<bool>.Ok(true) : Result<bool>.Fail("Không thể cập nhật thời gian đồng bộ");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "cập nhật thời gian đồng bộ");
            }
        }

        public async Task<Result<bool>> SaveOrderMappingsAsync(int integrationId, List<(int rowIndex, int orderId)> mappings)
        {
            try
            {
                foreach (var (rowIndex, orderId) in mappings)
                {
                    var map = new GoogleOrderMapping
                    {
                        SheetRowIndex = rowIndex,
                        OrderId = orderId,
                        SyncedAt = DateTime.Now,
                        ConfigId = integrationId
                    };
                    await _unitOfWork.GoogleOrderMappings.AddMappingAsync(map);
                }
                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "lưu mapping đơn hàng Google Sheets");
            }
        }
    }
}
