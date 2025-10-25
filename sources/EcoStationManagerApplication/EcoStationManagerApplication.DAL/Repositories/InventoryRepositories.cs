using Dapper;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Database;
using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{
    // ==================== INVENTORY REPOSITORY IMPLEMENTATION ====================
    public class InventoryRepository : BaseRepository<Inventory>, IInventoryRepository
    {
        public InventoryRepository() : base("Inventories", "inventory_id", false) { }
        public InventoryRepository(IDbHelper dbHelper) : base(dbHelper, "Inventories", "inventory_id", false) { }

        public async Task<Inventory> GetByVariantAndStationAsync(int variantId, int stationId)
        {
            var sql = @"SELECT i.*, v.name as VariantName 
                       FROM Inventories i
                       JOIN Variants v ON i.variant_id = v.variant_id
                       WHERE i.variant_id = @VariantId AND i.station_id = @StationId";
            return await _dbHelper.QueryFirstOrDefaultAsync<Inventory>(sql, new { VariantId = variantId, StationId = stationId });
        }

        public async Task<IEnumerable<Inventory>> GetInventoryByStationAsync(int stationId)
        {
            var sql = @"SELECT i.*, v.name as VariantName, v.sku as VariantSKU
                       FROM Inventories i
                       JOIN Variants v ON i.variant_id = v.variant_id
                       WHERE i.station_id = @StationId
                       ORDER BY v.name";
            return await _dbHelper.QueryAsync<Inventory>(sql, new { StationId = stationId });
        }

        public async Task<decimal> GetCurrentStockAsync(int variantId, int stationId)
        {
            var sql = "SELECT COALESCE(current_stock, 0) FROM Inventories WHERE variant_id = @VariantId AND station_id = @StationId";
            return await _dbHelper.ExecuteScalarAsync<decimal>(sql, new { VariantId = variantId, StationId = stationId });
        }

        public async Task<decimal> GetAvailableStockAsync(int variantId, int stationId)
        {
            var sql = "SELECT COALESCE(current_stock - reserved_stock, 0) FROM Inventories WHERE variant_id = @VariantId AND station_id = @StationId";
            return await _dbHelper.ExecuteScalarAsync<decimal>(sql, new { VariantId = variantId, StationId = stationId });
        }

        public async Task<bool> UpdateStockAsync(int variantId, int stationId, decimal quantity)
        {
            var sql = "UPDATE Inventories SET current_stock = @Quantity WHERE variant_id = @VariantId AND station_id = @StationId";
            var result = await _dbHelper.ExecuteAsync(sql, new { VariantId = variantId, StationId = stationId, Quantity = quantity });
            return result > 0;
        }

        public async Task<bool> ReserveStockAsync(int variantId, int stationId, decimal quantity)
        {
            var sql = @"UPDATE Inventories 
                       SET reserved_stock = reserved_stock + @Quantity 
                       WHERE variant_id = @VariantId AND station_id = @StationId 
                       AND (current_stock - reserved_stock) >= @Quantity";
            var result = await _dbHelper.ExecuteAsync(sql, new { VariantId = variantId, StationId = stationId, Quantity = quantity });
            return result > 0;
        }

        public async Task<bool> ReleaseReservedStockAsync(int variantId, int stationId, decimal quantity)
        {
            var sql = @"UPDATE Inventories 
                       SET reserved_stock = reserved_stock - @Quantity 
                       WHERE variant_id = @VariantId AND station_id = @StationId 
                       AND reserved_stock >= @Quantity";
            var result = await _dbHelper.ExecuteAsync(sql, new { VariantId = variantId, StationId = stationId, Quantity = quantity });
            return result > 0;
        }

        public async Task<IEnumerable<Inventory>> GetLowStockItemsAsync(int stationId)
        {
            var sql = @"SELECT i.*, v.name as VariantName, vsr.min_stock
                       FROM Inventories i
                       JOIN Variants v ON i.variant_id = v.variant_id
                       LEFT JOIN VariantStockRules vsr ON i.variant_id = vsr.variant_id AND i.station_id = vsr.station_id
                       WHERE i.station_id = @StationId 
                       AND i.current_stock <= COALESCE(vsr.min_stock, 15)";
            return await _dbHelper.QueryAsync<Inventory>(sql, new { StationId = stationId });
        }

        public async Task<IEnumerable<Inventory>> GetOutOfStockItemsAsync(int stationId)
        {
            var sql = @"SELECT i.*, v.name as VariantName
                       FROM Inventories i
                       JOIN Variants v ON i.variant_id = v.variant_id
                       WHERE i.station_id = @StationId AND i.current_stock <= 0";
            return await _dbHelper.QueryAsync<Inventory>(sql, new { StationId = stationId });
        }
    }

    // ==================== BATCH REPOSITORY IMPLEMENTATION ====================
    public class BatchRepository : BaseRepository<Batch>, IBatchRepository
    {
        public BatchRepository() : base("Batches", "batch_id", false) { }
        public BatchRepository(IDbHelper dbHelper) : base(dbHelper, "Batches", "batch_id", false) { }

        public async Task<IEnumerable<Batch>> GetBatchesByVariantAsync(int variantId)
        {
            var sql = @"SELECT b.*, v.name as VariantName 
                       FROM Batches b
                       JOIN Variants v ON b.variant_id = v.variant_id
                       WHERE b.variant_id = @VariantId
                       ORDER BY b.expiry_date";
            return await _dbHelper.QueryAsync<Batch>(sql, new { VariantId = variantId });
        }

        public async Task<IEnumerable<Batch>> GetExpiringBatchesAsync(int daysThreshold)
        {
            var sql = @"SELECT b.*, v.name as VariantName 
                       FROM Batches b
                       JOIN Variants v ON b.variant_id = v.variant_id
                       WHERE b.expiry_date <= DATE_ADD(CURDATE(), INTERVAL @DaysThreshold DAY)
                       AND b.expiry_date >= CURDATE()
                       AND b.quality_status = 'good'
                       ORDER BY b.expiry_date";
            return await _dbHelper.QueryAsync<Batch>(sql, new { DaysThreshold = daysThreshold });
        }

        public async Task<IEnumerable<Batch>> GetBatchesByStationAsync(int stationId)
        {
            var sql = @"SELECT b.*, v.name as VariantName 
                       FROM Batches b
                       JOIN Variants v ON b.variant_id = v.variant_id
                       WHERE b.station_id = @StationId
                       ORDER BY b.expiry_date";
            return await _dbHelper.QueryAsync<Batch>(sql, new { StationId = stationId });
        }

        public async Task<IEnumerable<Batch>> GetBatchesNearExpiryAsync(int days)
        {
            var sql = @"SELECT b.*, v.name as VariantName 
                       FROM Batches b
                       JOIN Variants v ON b.variant_id = v.variant_id
                       WHERE b.expiry_date BETWEEN CURDATE() AND DATE_ADD(CURDATE(), INTERVAL @Days DAY)
                       AND b.quality_status = 'good'
                       ORDER BY b.expiry_date";
            return await _dbHelper.QueryAsync<Batch>(sql, new { Days = days });
        }

        public async Task<IEnumerable<Batch>> GetExpiredBatchesAsync()
        {
            var sql = @"SELECT b.*, v.name as VariantName 
                       FROM Batches b
                       JOIN Variants v ON b.variant_id = v.variant_id
                       WHERE b.expiry_date < CURDATE()
                       ORDER BY b.expiry_date";
            return await _dbHelper.QueryAsync<Batch>(sql);
        }

        public async Task<bool> UpdateBatchQuantityAsync(int batchId, decimal quantity)
        {
            var sql = "UPDATE Batches SET current_quantity = @Quantity WHERE batch_id = @BatchId";
            var result = await _dbHelper.ExecuteAsync(sql, new { BatchId = batchId, Quantity = quantity });
            return result > 0;
        }

        public async Task<bool> UpdateBatchQualityStatusAsync(int batchId, string qualityStatus)
        {
            var sql = "UPDATE Batches SET quality_status = @QualityStatus WHERE batch_id = @BatchId";
            var result = await _dbHelper.ExecuteAsync(sql, new { BatchId = batchId, QualityStatus = qualityStatus });
            return result > 0;
        }

        public async Task<Batch> GetBatchByBatchNoAsync(string batchNo, int variantId)
        {
            var sql = "SELECT * FROM Batches WHERE batch_no = @BatchNo AND variant_id = @VariantId";
            return await _dbHelper.QueryFirstOrDefaultAsync<Batch>(sql, new { BatchNo = batchNo, VariantId = variantId });
        }
    }

    // ==================== STOCK IN REPOSITORY IMPLEMENTATION ====================
    public class StockInRepository : BaseRepository<StockIn>, IStockInRepository
    {
        public StockInRepository() : base("StockIn", "stockin_id", false) { }
        public StockInRepository(IDbHelper dbHelper) : base(dbHelper, "StockIn", "stockin_id", false) { }

        public async Task<IEnumerable<StockIn>> GetStockInsByStationAsync(int stationId)
        {
            var sql = @"SELECT si.*, v.name as VariantName, s.name as SupplierName
                       FROM StockIn si
                       LEFT JOIN Variants v ON si.variant_id = v.variant_id
                       LEFT JOIN Suppliers s ON si.supplier_id = s.supplier_id
                       WHERE si.station_id = @StationId
                       ORDER BY si.created_date DESC";
            return await _dbHelper.QueryAsync<StockIn>(sql, new { StationId = stationId });
        }

        public async Task<IEnumerable<StockIn>> GetStockInsBySupplierAsync(int supplierId)
        {
            var sql = @"SELECT si.*, v.name as VariantName
                       FROM StockIn si
                       JOIN Variants v ON si.variant_id = v.variant_id
                       WHERE si.supplier_id = @SupplierId
                       ORDER BY si.created_date DESC";
            return await _dbHelper.QueryAsync<StockIn>(sql, new { SupplierId = supplierId });
        }

        public async Task<IEnumerable<StockIn>> GetStockInsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var sql = @"SELECT si.*, v.name as VariantName, s.name as SupplierName
                       FROM StockIn si
                       LEFT JOIN Variants v ON si.variant_id = v.variant_id
                       LEFT JOIN Suppliers s ON si.supplier_id = s.supplier_id
                       WHERE si.created_date BETWEEN @StartDate AND @EndDate
                       ORDER BY si.created_date DESC";
            return await _dbHelper.QueryAsync<StockIn>(sql, new { StartDate = startDate, EndDate = endDate });
        }

        public async Task<IEnumerable<StockIn>> GetStockInsByBatchAsync(int batchId)
        {
            var sql = @"SELECT si.*, v.name as VariantName
                       FROM StockIn si
                       JOIN Variants v ON si.variant_id = v.variant_id
                       WHERE si.batch_id = @BatchId
                       ORDER BY si.created_date DESC";
            return await _dbHelper.QueryAsync<StockIn>(sql, new { BatchId = batchId });
        }

        public async Task<IEnumerable<StockIn>> GetRecentStockInsAsync(int count)
        {
            var sql = @"SELECT si.*, v.name as VariantName
                       FROM StockIn si
                       JOIN Variants v ON si.variant_id = v.variant_id
                       ORDER BY si.created_date DESC
                       LIMIT @Count";
            return await _dbHelper.QueryAsync<StockIn>(sql, new { Count = count });
        }

        public async Task<decimal> GetTotalStockInValueAsync(DateTime startDate, DateTime endDate)
        {
            var sql = @"SELECT COALESCE(SUM(si.quantity * COALESCE(si.unit_price, 0)), 0)
                       FROM StockIn si
                       WHERE si.created_date BETWEEN @StartDate AND @EndDate";
            return await _dbHelper.ExecuteScalarAsync<decimal>(sql, new { StartDate = startDate, EndDate = endDate });
        }
    }

    // ==================== STOCK OUT REPOSITORY IMPLEMENTATION ====================
    public class StockOutRepository : BaseRepository<StockOut>, IStockOutRepository
    {
        public StockOutRepository() : base("StockOut", "stockout_id", false) { }
        public StockOutRepository(IDbHelper dbHelper) : base(dbHelper, "StockOut", "stockout_id", false) { }

        public async Task<IEnumerable<StockOut>> GetStockOutsByStationAsync(int stationId)
        {
            var sql = @"SELECT so.*, v.name as VariantName, o.order_id
                       FROM StockOut so
                       JOIN Variants v ON so.variant_id = v.variant_id
                       LEFT JOIN Orders o ON so.order_id = o.order_id
                       WHERE so.station_id = @StationId
                       ORDER BY so.created_date DESC";
            return await _dbHelper.QueryAsync<StockOut>(sql, new { StationId = stationId });
        }

        public async Task<IEnumerable<StockOut>> GetStockOutsByOrderAsync(int orderId)
        {
            var sql = @"SELECT so.*, v.name as VariantName
                       FROM StockOut so
                       JOIN Variants v ON so.variant_id = v.variant_id
                       WHERE so.order_id = @OrderId
                       ORDER BY so.created_date DESC";
            return await _dbHelper.QueryAsync<StockOut>(sql, new { OrderId = orderId });
        }

        public async Task<IEnumerable<StockOut>> GetStockOutsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var sql = @"SELECT so.*, v.name as VariantName, o.order_id
                       FROM StockOut so
                       JOIN Variants v ON so.variant_id = v.variant_id
                       LEFT JOIN Orders o ON so.order_id = o.order_id
                       WHERE so.created_date BETWEEN @StartDate AND @EndDate
                       ORDER BY so.created_date DESC";
            return await _dbHelper.QueryAsync<StockOut>(sql, new { StartDate = startDate, EndDate = endDate });
        }

        public async Task<IEnumerable<StockOut>> GetStockOutsByPurposeAsync(string purpose)
        {
            var sql = @"SELECT so.*, v.name as VariantName
                       FROM StockOut so
                       JOIN Variants v ON so.variant_id = v.variant_id
                       WHERE so.purpose = @Purpose
                       ORDER BY so.created_date DESC";
            return await _dbHelper.QueryAsync<StockOut>(sql, new { Purpose = purpose });
        }

        public async Task<decimal> GetTotalStockOutQuantityAsync(DateTime startDate, DateTime endDate, int? variantId = null)
        {
            var sql = "SELECT COALESCE(SUM(quantity), 0) FROM StockOut WHERE created_date BETWEEN @StartDate AND @EndDate";

            if (variantId.HasValue)
            {
                sql += " AND variant_id = @VariantId";
                return await _dbHelper.ExecuteScalarAsync<decimal>(sql, new { StartDate = startDate, EndDate = endDate, VariantId = variantId.Value });
            }

            return await _dbHelper.ExecuteScalarAsync<decimal>(sql, new { StartDate = startDate, EndDate = endDate });
        }
    }

    // ==================== STOCK ALERT REPOSITORY IMPLEMENTATION ====================
    public class StockAlertRepository : BaseRepository<StockAlert>, IStockAlertRepository
    {
        public StockAlertRepository() : base("StockAlerts", "alert_id", false) { }
        public StockAlertRepository(IDbHelper dbHelper) : base(dbHelper, "StockAlerts", "alert_id", false) { }

        public async Task<IEnumerable<StockAlert>> GetActiveAlertsAsync()
        {
            var sql = @"SELECT sa.*, v.name as VariantName
                       FROM StockAlerts sa
                       JOIN Variants v ON sa.variant_id = v.variant_id
                       WHERE sa.is_active = 1
                       ORDER BY sa.created_date DESC";
            return await _dbHelper.QueryAsync<StockAlert>(sql);
        }

        public async Task<IEnumerable<StockAlert>> GetAlertsByStationAsync(int stationId)
        {
            var sql = @"SELECT sa.*, v.name as VariantName
                       FROM StockAlerts sa
                       JOIN Variants v ON sa.variant_id = v.variant_id
                       WHERE sa.station_id = @StationId AND sa.is_active = 1
                       ORDER BY sa.created_date DESC";
            return await _dbHelper.QueryAsync<StockAlert>(sql, new { StationId = stationId });
        }

        public async Task<IEnumerable<StockAlert>> GetAlertsByTypeAsync(string alertType)
        {
            var sql = @"SELECT sa.*, v.name as VariantName
                       FROM StockAlerts sa
                       JOIN Variants v ON sa.variant_id = v.variant_id
                       WHERE sa.alert_type = @AlertType AND sa.is_active = 1
                       ORDER BY sa.created_date DESC";
            return await _dbHelper.QueryAsync<StockAlert>(sql, new { AlertType = alertType });
        }

        public async Task<int> GetAlertCountByTypeAsync(string alertType)
        {
            var sql = "SELECT COUNT(*) FROM StockAlerts WHERE alert_type = @AlertType AND is_active = 1";
            return await _dbHelper.ExecuteScalarAsync<int>(sql, new { AlertType = alertType });
        }

        public async Task<bool> DeactivateAlertAsync(int alertId)
        {
            var sql = "UPDATE StockAlerts SET is_active = 0 WHERE alert_id = @AlertId";
            var result = await _dbHelper.ExecuteAsync(sql, new { AlertId = alertId });
            return result > 0;
        }

        public async Task<bool> CreateLowStockAlertAsync(int variantId, int stationId, decimal threshold)
        {
            var sql = @"INSERT INTO StockAlerts (variant_id, station_id, alert_type, threshold_value, is_active)
                       VALUES (@VariantId, @StationId, 'low_stock', @Threshold, 1)";
            var result = await _dbHelper.ExecuteAsync(sql, new { VariantId = variantId, StationId = stationId, Threshold = threshold });
            return result > 0;
        }

        public async Task<bool> CreateExpiryAlertAsync(int variantId, int stationId, int daysThreshold)
        {
            var sql = @"INSERT INTO StockAlerts (variant_id, station_id, alert_type, threshold_value, is_active)
                       VALUES (@VariantId, @StationId, 'expiry_warning', @Threshold, 1)";
            var result = await _dbHelper.ExecuteAsync(sql, new { VariantId = variantId, StationId = stationId, Threshold = daysThreshold });
            return result > 0;
        }
    }

    // ==================== STOCK AUDIT REPOSITORY IMPLEMENTATION ====================
    public class StockAuditRepository : BaseRepository<StockAudit>, IStockAuditRepository
    {
        public StockAuditRepository() : base("StockAudits", "audit_id", false) { }
        public StockAuditRepository(IDbHelper dbHelper) : base(dbHelper, "StockAudits", "audit_id", false) { }

        public async Task<IEnumerable<StockAudit>> GetAuditsByStationAsync(int stationId)
        {
            var sql = @"SELECT sa.*, v.name as VariantName, u.fullname as AuditedByName
                       FROM StockAudits sa
                       JOIN Variants v ON sa.variant_id = v.variant_id
                       JOIN Users u ON sa.audited_by = u.user_id
                       WHERE sa.station_id = @StationId
                       ORDER BY sa.audit_date DESC";
            return await _dbHelper.QueryAsync<StockAudit>(sql, new { StationId = stationId });
        }

        public async Task<IEnumerable<StockAudit>> GetAuditsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var sql = @"SELECT sa.*, v.name as VariantName, u.fullname as AuditedByName
                       FROM StockAudits sa
                       JOIN Variants v ON sa.variant_id = v.variant_id
                       JOIN Users u ON sa.audited_by = u.user_id
                       WHERE sa.audit_date BETWEEN @StartDate AND @EndDate
                       ORDER BY sa.audit_date DESC";
            return await _dbHelper.QueryAsync<StockAudit>(sql, new { StartDate = startDate, EndDate = endDate });
        }

        public async Task<IEnumerable<StockAudit>> GetPendingAuditsAsync()
        {
            var sql = @"SELECT sa.*, v.name as VariantName
                       FROM StockAudits sa
                       JOIN Variants v ON sa.variant_id = v.variant_id
                       WHERE sa.status = 'pending'
                       ORDER BY sa.audit_date";
            return await _dbHelper.QueryAsync<StockAudit>(sql);
        }

        public async Task<bool> UpdateAuditStatusAsync(int auditId, string status)
        {
            var sql = "UPDATE StockAudits SET status = @Status WHERE audit_id = @AuditId";
            var result = await _dbHelper.ExecuteAsync(sql, new { AuditId = auditId, Status = status });
            return result > 0;
        }

        public async Task<decimal> GetTotalVarianceValueAsync(DateTime startDate, DateTime endDate)
        {
            var sql = @"SELECT COALESCE(SUM(sa.variance * COALESCE(v.price, 0)), 0)
                       FROM StockAudits sa
                       JOIN Variants v ON sa.variant_id = v.variant_id
                       WHERE sa.audit_date BETWEEN @StartDate AND @EndDate";
            return await _dbHelper.ExecuteScalarAsync<decimal>(sql, new { StartDate = startDate, EndDate = endDate });
        }
    }

    // ==================== VARIANT STOCK RULE REPOSITORY IMPLEMENTATION ====================
    public class VariantStockRuleRepository : BaseRepository<VariantStockRule>, IVariantStockRuleRepository
    {
        public VariantStockRuleRepository() : base("VariantStockRules", "rule_id", false) { }
        public VariantStockRuleRepository(IDbHelper dbHelper) : base(dbHelper, "VariantStockRules", "rule_id", false) { }

        public async Task<VariantStockRule> GetRuleByVariantAndStationAsync(int variantId, int stationId)
        {
            var sql = @"SELECT vsr.*, v.name as VariantName
                       FROM VariantStockRules vsr
                       JOIN Variants v ON vsr.variant_id = v.variant_id
                       WHERE vsr.variant_id = @VariantId AND vsr.station_id = @StationId";
            return await _dbHelper.QueryFirstOrDefaultAsync<VariantStockRule>(sql, new { VariantId = variantId, StationId = stationId });
        }

        public async Task<IEnumerable<VariantStockRule>> GetRulesByStationAsync(int stationId)
        {
            var sql = @"SELECT vsr.*, v.name as VariantName
                       FROM VariantStockRules vsr
                       JOIN Variants v ON vsr.variant_id = v.variant_id
                       WHERE vsr.station_id = @StationId
                       ORDER BY v.name";
            return await _dbHelper.QueryAsync<VariantStockRule>(sql, new { StationId = stationId });
        }

        public async Task<IEnumerable<VariantStockRule>> GetRulesNeedingUpdateAsync()
        {
            var sql = @"SELECT vsr.*, v.name as VariantName
                       FROM VariantStockRules vsr
                       JOIN Variants v ON vsr.variant_id = v.variant_id
                       WHERE v.is_active = 1";
            return await _dbHelper.QueryAsync<VariantStockRule>(sql);
        }

        public async Task<bool> UpdateStockRuleAsync(int variantId, int stationId, decimal minStock, decimal maxStock, int expiryDays)
        {
            var sql = @"UPDATE VariantStockRules 
                       SET min_stock = @MinStock, max_stock = @MaxStock, expiry_days = @ExpiryDays 
                       WHERE variant_id = @VariantId AND station_id = @StationId";
            var result = await _dbHelper.ExecuteAsync(sql, new
            {
                VariantId = variantId,
                StationId = stationId,
                MinStock = minStock,
                MaxStock = maxStock,
                ExpiryDays = expiryDays
            });
            return result > 0;
        }
    }
}