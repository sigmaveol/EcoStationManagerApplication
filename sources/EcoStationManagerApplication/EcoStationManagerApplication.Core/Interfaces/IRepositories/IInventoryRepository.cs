using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    // ==================== INVENTORY REPOSITORY INTERFACE ====================
    public interface IInventoryRepository : IRepository<Inventory>
    {
        Task<Inventory> GetByVariantAndStationAsync(int variantId, int stationId);
        Task<IEnumerable<Inventory>> GetInventoryByStationAsync(int stationId);
        Task<decimal> GetCurrentStockAsync(int variantId, int stationId);
        Task<decimal> GetAvailableStockAsync(int variantId, int stationId);
        Task<bool> UpdateStockAsync(int variantId, int stationId, decimal quantity);
        Task<bool> ReserveStockAsync(int variantId, int stationId, decimal quantity);
        Task<bool> ReleaseReservedStockAsync(int variantId, int stationId, decimal quantity);
        Task<IEnumerable<Inventory>> GetLowStockItemsAsync(int stationId);
        Task<IEnumerable<Inventory>> GetOutOfStockItemsAsync(int stationId);
    }

    // ==================== BATCH REPOSITORY INTERFACE ====================
    public interface IBatchRepository : IRepository<Batch>
    {
        Task<IEnumerable<Batch>> GetBatchesByVariantAsync(int variantId);
        Task<IEnumerable<Batch>> GetExpiringBatchesAsync(int daysThreshold);
        Task<IEnumerable<Batch>> GetBatchesByStationAsync(int stationId);
        Task<IEnumerable<Batch>> GetBatchesNearExpiryAsync(int days);
        Task<IEnumerable<Batch>> GetExpiredBatchesAsync();
        Task<bool> UpdateBatchQuantityAsync(int batchId, decimal quantity);
        Task<bool> UpdateBatchQualityStatusAsync(int batchId, string qualityStatus);
        Task<Batch> GetBatchByBatchNoAsync(string batchNo, int variantId);
    }

    // ==================== STOCK IN REPOSITORY INTERFACE ====================
    public interface IStockInRepository : IRepository<StockIn>
    {
        Task<IEnumerable<StockIn>> GetStockInsByStationAsync(int stationId);
        Task<IEnumerable<StockIn>> GetStockInsBySupplierAsync(int supplierId);
        Task<IEnumerable<StockIn>> GetStockInsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<StockIn>> GetStockInsByBatchAsync(int batchId);
        Task<IEnumerable<StockIn>> GetRecentStockInsAsync(int count);
        Task<decimal> GetTotalStockInValueAsync(DateTime startDate, DateTime endDate);
    }

    // ==================== STOCK OUT REPOSITORY INTERFACE ====================
    public interface IStockOutRepository : IRepository<StockOut>
    {
        Task<IEnumerable<StockOut>> GetStockOutsByStationAsync(int stationId);
        Task<IEnumerable<StockOut>> GetStockOutsByOrderAsync(int orderId);
        Task<IEnumerable<StockOut>> GetStockOutsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<StockOut>> GetStockOutsByPurposeAsync(string purpose);
        Task<decimal> GetTotalStockOutQuantityAsync(DateTime startDate, DateTime endDate, int? variantId = null);
    }

    // ==================== STOCK ALERT REPOSITORY INTERFACE ====================
    public interface IStockAlertRepository : IRepository<StockAlert>
    {
        Task<IEnumerable<StockAlert>> GetActiveAlertsAsync();
        Task<IEnumerable<StockAlert>> GetAlertsByStationAsync(int stationId);
        Task<IEnumerable<StockAlert>> GetAlertsByTypeAsync(string alertType);
        Task<int> GetAlertCountByTypeAsync(string alertType);
        Task<bool> DeactivateAlertAsync(int alertId);
        Task<bool> CreateLowStockAlertAsync(int variantId, int stationId, decimal threshold);
        Task<bool> CreateExpiryAlertAsync(int variantId, int stationId, int daysThreshold);
    }

    // ==================== STOCK AUDIT REPOSITORY INTERFACE ====================
    public interface IStockAuditRepository : IRepository<StockAudit>
    {
        Task<IEnumerable<StockAudit>> GetAuditsByStationAsync(int stationId);
        Task<IEnumerable<StockAudit>> GetAuditsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<StockAudit>> GetPendingAuditsAsync();
        Task<bool> UpdateAuditStatusAsync(int auditId, string status);
        Task<decimal> GetTotalVarianceValueAsync(DateTime startDate, DateTime endDate);
    }

    // ==================== VARIANT STOCK RULE REPOSITORY INTERFACE ====================
    public interface IVariantStockRuleRepository : IRepository<VariantStockRule>
    {
        Task<VariantStockRule> GetRuleByVariantAndStationAsync(int variantId, int stationId);
        Task<IEnumerable<VariantStockRule>> GetRulesByStationAsync(int stationId);
        Task<IEnumerable<VariantStockRule>> GetRulesNeedingUpdateAsync();
        Task<bool> UpdateStockRuleAsync(int variantId, int stationId, decimal minStock, decimal maxStock, int expiryDays);
    }
}