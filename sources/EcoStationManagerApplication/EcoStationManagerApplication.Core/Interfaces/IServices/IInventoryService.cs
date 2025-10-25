using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    // ==================== INVENTORY SERVICE INTERFACE ====================
    public interface IInventoryService : IService<Inventory>
    {
        Task<Inventory> GetByVariantAndStationAsync(int variantId, int stationId);
        Task<IEnumerable<Inventory>> GetInventoryByStationAsync(int stationId);
        Task<decimal> GetCurrentStockAsync(int variantId, int stationId);
        Task<decimal> GetAvailableStockAsync(int variantId, int stationId);
        Task<bool> UpdateStockAsync(int variantId, int stationId, decimal quantity);
        Task<bool> ReserveStockAsync(int variantId, int stationId, decimal quantity);
        Task<bool> ReleaseReservedStockAsync(int variantId, int stationId, decimal quantity);
        Task<bool> AdjustStockAsync(int variantId, int stationId, decimal adjustment, string reason);
        Task<IEnumerable<Inventory>> GetLowStockItemsAsync(int stationId);
        Task<IEnumerable<Inventory>> GetOutOfStockItemsAsync(int stationId);
        Task<Dictionary<string, object>> GetInventorySummaryAsync(int stationId);
    }

    // ==================== BATCH SERVICE INTERFACE ====================
    public interface IBatchService : IService<Batch>
    {
        Task<IEnumerable<Batch>> GetBatchesByVariantAsync(int variantId);
        Task<IEnumerable<Batch>> GetExpiringBatchesAsync(int daysThreshold);
        Task<IEnumerable<Batch>> GetBatchesByStationAsync(int stationId);
        Task<IEnumerable<Batch>> GetBatchesNearExpiryAsync(int days);
        Task<IEnumerable<Batch>> GetExpiredBatchesAsync();
        Task<bool> UpdateBatchQuantityAsync(int batchId, decimal quantity);
        Task<bool> UpdateBatchQualityStatusAsync(int batchId, string qualityStatus);
        Task<bool> DisposeBatchAsync(int batchId, string reason);
        Task<Batch> CreateBatchAsync(Batch batch);
        Task<Dictionary<string, object>> GetBatchStatisticsAsync(int stationId);
    }

    // ==================== STOCK IN SERVICE INTERFACE ====================
    public interface IStockInService : IService<StockIn>
    {
        Task<IEnumerable<StockIn>> GetStockInsByStationAsync(int stationId);
        Task<IEnumerable<StockIn>> GetStockInsBySupplierAsync(int supplierId);
        Task<IEnumerable<StockIn>> GetStockInsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalStockInValueAsync(DateTime startDate, DateTime endDate);
        Task<bool> ProcessStockInAsync(StockIn stockIn, Batch batch);
        Task<Dictionary<string, object>> GetStockInSummaryAsync(DateTime startDate, DateTime endDate);
    }

    // ==================== STOCK OUT SERVICE INTERFACE ====================
    public interface IStockOutService : IService<StockOut>
    {
        Task<IEnumerable<StockOut>> GetStockOutsByStationAsync(int stationId);
        Task<IEnumerable<StockOut>> GetStockOutsByOrderAsync(int orderId);
        Task<IEnumerable<StockOut>> GetStockOutsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalStockOutQuantityAsync(DateTime startDate, DateTime endDate, int? variantId = null);
        Task<bool> ProcessStockOutAsync(StockOut stockOut);
        Task<Dictionary<string, object>> GetStockOutSummaryAsync(DateTime startDate, DateTime endDate);
    }

    // ==================== STOCK ALERT SERVICE INTERFACE ====================
    public interface IStockAlertService : IService<StockAlert>
    {
        Task<IEnumerable<StockAlert>> GetActiveAlertsAsync();
        Task<IEnumerable<StockAlert>> GetAlertsByStationAsync(int stationId);
        Task<IEnumerable<StockAlert>> GetAlertsByTypeAsync(string alertType);
        Task<bool> DeactivateAlertAsync(int alertId);
        Task<bool> CreateLowStockAlertAsync(int variantId, int stationId, decimal threshold);
        Task<bool> CreateExpiryAlertAsync(int variantId, int stationId, int daysThreshold);
        Task<bool> CheckAndCreateAlertsAsync();
        Task<Dictionary<string, int>> GetAlertStatisticsAsync();
    }

    // ==================== STOCK AUDIT SERVICE INTERFACE ====================
    public interface IStockAuditService : IService<StockAudit>
    {
        Task<IEnumerable<StockAudit>> GetAuditsByStationAsync(int stationId);
        Task<IEnumerable<StockAudit>> GetAuditsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<StockAudit>> GetPendingAuditsAsync();
        Task<bool> UpdateAuditStatusAsync(int auditId, string status);
        Task<bool> CreateStockAuditAsync(StockAudit audit);
        Task<Dictionary<string, object>> GetAuditSummaryAsync(DateTime startDate, DateTime endDate);
    }

    // ==================== VARIANT STOCK RULE SERVICE INTERFACE ====================
    public interface IVariantStockRuleService : IService<VariantStockRule>
    {
        Task<VariantStockRule> GetRuleByVariantAndStationAsync(int variantId, int stationId);
        Task<IEnumerable<VariantStockRule>> GetRulesByStationAsync(int stationId);
        Task<bool> UpdateStockRuleAsync(int variantId, int stationId, decimal minStock, decimal maxStock, int expiryDays);
        Task<bool> ApplyDefaultRulesAsync(int variantId, int stationId);
        Task<Dictionary<string, object>> GetRuleEffectivenessAsync(int stationId);
    }
}