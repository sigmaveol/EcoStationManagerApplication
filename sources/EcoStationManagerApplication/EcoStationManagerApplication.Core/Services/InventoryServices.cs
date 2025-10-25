using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    // ==================== INVENTORY SERVICE IMPLEMENTATION ====================
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IVariantRepository _variantRepository;

        public InventoryService(IInventoryRepository inventoryRepository, IVariantRepository variantRepository)
        {
            _inventoryRepository = inventoryRepository;
            _variantRepository = variantRepository;
        }

        public async Task<Inventory> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID tồn kho không hợp lệ");
            var inventory = await _inventoryRepository.GetByIdAsync(id);
            return inventory ?? throw new KeyNotFoundException($"Không tìm thấy tồn kho với ID: {id}");
        }

        public async Task<IEnumerable<Inventory>> GetAllAsync()
        {
            return await _inventoryRepository.GetAllAsync();
        }

        public async Task<Inventory> CreateAsync(Inventory entity)
        {
            ValidateInventory(entity);

            // Check if inventory already exists for variant and station
            var existing = await _inventoryRepository.GetByVariantAndStationAsync(entity.VariantId, entity.StationId);
            if (existing != null)
                throw new InvalidOperationException($"Đã tồn tại tồn kho cho sản phẩm tại trạm này");

            return await _inventoryRepository.CreateAsync(entity);
        }

        public async Task<Inventory> UpdateAsync(Inventory entity)
        {
            if (entity.InventoryId <= 0) throw new ArgumentException("ID tồn kho không hợp lệ");
            ValidateInventory(entity);

            var existing = await _inventoryRepository.GetByIdAsync(entity.InventoryId);
            if (existing == null) throw new KeyNotFoundException($"Không tìm thấy tồn kho với ID: {entity.InventoryId}");

            return await _inventoryRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID tồn kho không hợp lệ");
            return await _inventoryRepository.DeleteAsync(id);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            throw new NotSupportedException("Tồn kho không hỗ trợ xóa mềm");
        }

        public async Task<Inventory> GetByVariantAndStationAsync(int variantId, int stationId)
        {
            if (variantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");

            var inventory = await _inventoryRepository.GetByVariantAndStationAsync(variantId, stationId);
            return inventory ?? throw new KeyNotFoundException($"Không tìm thấy tồn kho cho sản phẩm tại trạm");
        }

        public async Task<IEnumerable<Inventory>> GetInventoryByStationAsync(int stationId)
        {
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");
            return await _inventoryRepository.GetInventoryByStationAsync(stationId);
        }

        public async Task<decimal> GetCurrentStockAsync(int variantId, int stationId)
        {
            if (variantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");

            return await _inventoryRepository.GetCurrentStockAsync(variantId, stationId);
        }

        public async Task<decimal> GetAvailableStockAsync(int variantId, int stationId)
        {
            if (variantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");

            return await _inventoryRepository.GetAvailableStockAsync(variantId, stationId);
        }

        public async Task<bool> UpdateStockAsync(int variantId, int stationId, decimal quantity)
        {
            if (variantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");
            if (quantity < 0) throw new ArgumentException("Số lượng không được âm");

            return await _inventoryRepository.UpdateStockAsync(variantId, stationId, quantity);
        }

        public async Task<bool> ReserveStockAsync(int variantId, int stationId, decimal quantity)
        {
            if (variantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");
            if (quantity <= 0) throw new ArgumentException("Số lượng phải lớn hơn 0");

            var availableStock = await _inventoryRepository.GetAvailableStockAsync(variantId, stationId);
            if (availableStock < quantity)
                throw new InvalidOperationException($"Không đủ tồn kho. Tồn kho khả dụng: {availableStock}");

            return await _inventoryRepository.ReserveStockAsync(variantId, stationId, quantity);
        }

        public async Task<bool> ReleaseReservedStockAsync(int variantId, int stationId, decimal quantity)
        {
            if (variantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");
            if (quantity <= 0) throw new ArgumentException("Số lượng phải lớn hơn 0");

            return await _inventoryRepository.ReleaseReservedStockAsync(variantId, stationId, quantity);
        }

        public async Task<bool> AdjustStockAsync(int variantId, int stationId, decimal adjustment, string reason)
        {
            if (variantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");
            if (string.IsNullOrWhiteSpace(reason)) throw new ArgumentException("Lý do điều chỉnh không được trống");

            var currentStock = await _inventoryRepository.GetCurrentStockAsync(variantId, stationId);
            var newStock = currentStock + adjustment;

            if (newStock < 0)
                throw new InvalidOperationException("Không thể điều chỉnh tồn kho xuống dưới 0");

            return await _inventoryRepository.UpdateStockAsync(variantId, stationId, newStock);
        }

        public async Task<IEnumerable<Inventory>> GetLowStockItemsAsync(int stationId)
        {
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");
            return await _inventoryRepository.GetLowStockItemsAsync(stationId);
        }

        public async Task<IEnumerable<Inventory>> GetOutOfStockItemsAsync(int stationId)
        {
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");
            return await _inventoryRepository.GetOutOfStockItemsAsync(stationId);
        }

        public async Task<Dictionary<string, object>> GetInventorySummaryAsync(int stationId)
        {
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");

            var inventory = await _inventoryRepository.GetInventoryByStationAsync(stationId);
            var lowStockItems = await _inventoryRepository.GetLowStockItemsAsync(stationId);
            var outOfStockItems = await _inventoryRepository.GetOutOfStockItemsAsync(stationId);

            return new Dictionary<string, object>
            {
                ["StationId"] = stationId,
                ["TotalItems"] = inventory.Count(),
                ["LowStockItems"] = lowStockItems.Count(),
                ["OutOfStockItems"] = outOfStockItems.Count(),
                ["TotalValue"] = await CalculateTotalInventoryValue(inventory),
                ["LastUpdated"] = DateTime.Now
            };
        }

        private async Task<decimal> CalculateTotalInventoryValue(IEnumerable<Inventory> inventory)
        {
            decimal totalValue = 0;
            foreach (var item in inventory)
            {
                var variant = await _variantRepository.GetByIdAsync(item.VariantId);
                if (variant != null)
                {
                    totalValue += item.CurrentStock * variant.Price;
                }
            }
            return totalValue;
        }

        private void ValidateInventory(Inventory inventory)
        {
            if (inventory == null) throw new ArgumentNullException(nameof(inventory));
            if (inventory.VariantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            if (inventory.StationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");
            if (inventory.CurrentStock < 0) throw new ArgumentException("Tồn kho hiện tại không được âm");
            if (inventory.ReservedStock < 0) throw new ArgumentException("Tồn kho đặt trước không được âm");
        }
    }

    // ==================== BATCH SERVICE IMPLEMENTATION ====================
    public class BatchService : IBatchService
    {
        private readonly IBatchRepository _batchRepository;

        public BatchService(IBatchRepository batchRepository)
        {
            _batchRepository = batchRepository;
        }

        public async Task<Batch> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID lô hàng không hợp lệ");
            var batch = await _batchRepository.GetByIdAsync(id);
            return batch ?? throw new KeyNotFoundException($"Không tìm thấy lô hàng với ID: {id}");
        }

        public async Task<IEnumerable<Batch>> GetAllAsync()
        {
            return await _batchRepository.GetAllAsync();
        }

        public async Task<Batch> CreateAsync(Batch entity)
        {
            ValidateBatch(entity);

            // Check if batch number already exists for variant
            var existing = await _batchRepository.GetBatchByBatchNoAsync(entity.BatchNo, entity.VariantId);
            if (existing != null)
                throw new InvalidOperationException($"Số lô {entity.BatchNo} đã tồn tại cho sản phẩm này");

            return await _batchRepository.CreateAsync(entity);
        }

        public async Task<Batch> UpdateAsync(Batch entity)
        {
            if (entity.BatchId <= 0) throw new ArgumentException("ID lô hàng không hợp lệ");
            ValidateBatch(entity);

            var existing = await _batchRepository.GetByIdAsync(entity.BatchId);
            if (existing == null) throw new KeyNotFoundException($"Không tìm thấy lô hàng với ID: {entity.BatchId}");

            return await _batchRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID lô hàng không hợp lệ");
            return await _batchRepository.DeleteAsync(id);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            throw new NotSupportedException("Lô hàng không hỗ trợ xóa mềm");
        }

        public async Task<IEnumerable<Batch>> GetBatchesByVariantAsync(int variantId)
        {
            if (variantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            return await _batchRepository.GetBatchesByVariantAsync(variantId);
        }

        public async Task<IEnumerable<Batch>> GetExpiringBatchesAsync(int daysThreshold)
        {
            if (daysThreshold <= 0) throw new ArgumentException("Ngưỡng ngày phải lớn hơn 0");
            return await _batchRepository.GetExpiringBatchesAsync(daysThreshold);
        }

        public async Task<IEnumerable<Batch>> GetBatchesByStationAsync(int stationId)
        {
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");
            return await _batchRepository.GetBatchesByStationAsync(stationId);
        }

        public async Task<IEnumerable<Batch>> GetBatchesNearExpiryAsync(int days)
        {
            if (days <= 0) throw new ArgumentException("Số ngày phải lớn hơn 0");
            return await _batchRepository.GetBatchesNearExpiryAsync(days);
        }

        public async Task<IEnumerable<Batch>> GetExpiredBatchesAsync()
        {
            return await _batchRepository.GetExpiredBatchesAsync();
        }

        public async Task<bool> UpdateBatchQuantityAsync(int batchId, decimal quantity)
        {
            if (batchId <= 0) throw new ArgumentException("ID lô hàng không hợp lệ");
            if (quantity < 0) throw new ArgumentException("Số lượng không được âm");

            return await _batchRepository.UpdateBatchQuantityAsync(batchId, quantity);
        }

        public async Task<bool> UpdateBatchQualityStatusAsync(int batchId, string qualityStatus)
        {
            if (batchId <= 0) throw new ArgumentException("ID lô hàng không hợp lệ");
            if (string.IsNullOrWhiteSpace(qualityStatus)) throw new ArgumentException("Trạng thái chất lượng không được trống");

            var validStatuses = new[] { "good", "pending", "expired", "rejected", "other" };
            if (!validStatuses.Contains(qualityStatus))
                throw new ArgumentException($"Trạng thái chất lượng không hợp lệ: {qualityStatus}");

            return await _batchRepository.UpdateBatchQualityStatusAsync(batchId, qualityStatus);
        }

        public async Task<bool> DisposeBatchAsync(int batchId, string reason)
        {
            if (batchId <= 0) throw new ArgumentException("ID lô hàng không hợp lệ");
            if (string.IsNullOrWhiteSpace(reason)) throw new ArgumentException("Lý do hủy không được trống");

            return await _batchRepository.UpdateBatchQualityStatusAsync(batchId, "rejected");
        }

        public async Task<Batch> CreateBatchAsync(Batch batch)
        {
            return await CreateAsync(batch);
        }

        public async Task<Dictionary<string, object>> GetBatchStatisticsAsync(int stationId)
        {
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");

            var batches = await _batchRepository.GetBatchesByStationAsync(stationId);
            var expiringBatches = await _batchRepository.GetExpiringBatchesAsync(30);
            var expiredBatches = await _batchRepository.GetExpiredBatchesAsync();

            return new Dictionary<string, object>
            {
                ["StationId"] = stationId,
                ["TotalBatches"] = batches.Count(),
                ["ExpiringSoon"] = expiringBatches.Count(),
                ["Expired"] = expiredBatches.Count(),
                ["GoodQuality"] = batches.Count(b => b.QualityStatus.Equals("good")),
                ["NeedAttention"] = batches.Count(b => b.QualityStatus.Equals("good")),
                ["TotalQuantity"] = batches.Sum(b => b.CurrentQuantity)
            };
        }

        private void ValidateBatch(Batch batch)
        {
            if (batch == null) throw new ArgumentNullException(nameof(batch));
            if (string.IsNullOrWhiteSpace(batch.BatchNo)) throw new ArgumentException("Số lô không được trống");
            if (batch.VariantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            if (batch.StationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");
            if (batch.InitialQuantity < 0) throw new ArgumentException("Số lượng ban đầu không được âm");
            if (batch.CurrentQuantity < 0) throw new ArgumentException("Số lượng hiện tại không được âm");

            if (batch.ExpiryDate.HasValue && batch.ManufactureDate.HasValue)
            {
                if (batch.ExpiryDate <= batch.ManufactureDate)
                    throw new ArgumentException("Ngày hết hạn phải sau ngày sản xuất");
            }
        }
    }

    // ==================== STOCK IN SERVICE IMPLEMENTATION ====================
    public class StockInService : IStockInService
    {
        private readonly IStockInRepository _stockInRepository;
        private readonly IInventoryRepository _inventoryRepository;

        public StockInService(IStockInRepository stockInRepository, IInventoryRepository inventoryRepository)
        {
            _stockInRepository = stockInRepository;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<StockIn> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID nhập kho không hợp lệ");
            var stockIn = await _stockInRepository.GetByIdAsync(id);
            return stockIn ?? throw new KeyNotFoundException($"Không tìm thấy phiếu nhập kho với ID: {id}");
        }

        public async Task<IEnumerable<StockIn>> GetAllAsync()
        {
            return await _stockInRepository.GetAllAsync();
        }

        public async Task<StockIn> CreateAsync(StockIn entity)
        {
            ValidateStockIn(entity);
            return await _stockInRepository.CreateAsync(entity);
        }

        public async Task<StockIn> UpdateAsync(StockIn entity)
        {
            if (entity.StockInId <= 0) throw new ArgumentException("ID nhập kho không hợp lệ");
            ValidateStockIn(entity);

            var existing = await _stockInRepository.GetByIdAsync(entity.StockInId);
            if (existing == null) throw new KeyNotFoundException($"Không tìm thấy phiếu nhập kho với ID: {entity.StockInId}");

            return await _stockInRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID nhập kho không hợp lệ");
            return await _stockInRepository.DeleteAsync(id);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            throw new NotSupportedException("Phiếu nhập kho không hỗ trợ xóa mềm");
        }

        public async Task<IEnumerable<StockIn>> GetStockInsByStationAsync(int stationId)
        {
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");
            return await _stockInRepository.GetStockInsByStationAsync(stationId);
        }

        public async Task<IEnumerable<StockIn>> GetStockInsBySupplierAsync(int supplierId)
        {
            if (supplierId <= 0) throw new ArgumentException("ID nhà cung cấp không hợp lệ");
            return await _stockInRepository.GetStockInsBySupplierAsync(supplierId);
        }

        public async Task<IEnumerable<StockIn>> GetStockInsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate) throw new ArgumentException("Ngày bắt đầu phải trước ngày kết thúc");
            return await _stockInRepository.GetStockInsByDateRangeAsync(startDate, endDate);
        }

        public async Task<decimal> GetTotalStockInValueAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate) throw new ArgumentException("Ngày bắt đầu phải trước ngày kết thúc");
            return await _stockInRepository.GetTotalStockInValueAsync(startDate, endDate);
        }

        public async Task<bool> ProcessStockInAsync(StockIn stockIn, Batch batch)
        {
            ValidateStockIn(stockIn);

            // Create stock in record
            var createdStockIn = await _stockInRepository.CreateAsync(stockIn);

            // Update inventory
            var currentStock = await _inventoryRepository.GetCurrentStockAsync(stockIn.VariantId, stockIn.StationId ?? 0);
            var newStock = currentStock + stockIn.Quantity;
            await _inventoryRepository.UpdateStockAsync(stockIn.VariantId, stockIn.StationId ?? 0, newStock);

            return createdStockIn != null;
        }

        public async Task<Dictionary<string, object>> GetStockInSummaryAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate) throw new ArgumentException("Ngày bắt đầu phải trước ngày kết thúc");

            var stockIns = await _stockInRepository.GetStockInsByDateRangeAsync(startDate, endDate);
            var totalValue = await _stockInRepository.GetTotalStockInValueAsync(startDate, endDate);

            return new Dictionary<string, object>
            {
                ["Period"] = $"{startDate:dd/MM/yyyy} - {endDate:dd/MM/yyyy}",
                ["TotalTransactions"] = stockIns.Count(),
                ["TotalQuantity"] = stockIns.Sum(si => si.Quantity),
                ["TotalValue"] = totalValue,
                ["AverageValue"] = stockIns.Any() ? totalValue / stockIns.Count() : 0,
                ["TopSuppliers"] = stockIns.GroupBy(si => si.SupplierId)
                                          .OrderByDescending(g => g.Sum(si => si.Quantity))
                                          .Take(5)
            };
        }

        private void ValidateStockIn(StockIn stockIn)
        {
            if (stockIn == null) throw new ArgumentNullException(nameof(stockIn));
            if (stockIn.VariantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            if (stockIn.Quantity <= 0) throw new ArgumentException("Số lượng phải lớn hơn 0");
            if (stockIn.UnitPrice < 0) throw new ArgumentException("Đơn giá không được âm");
        }
    }

    // ==================== STOCK OUT SERVICE IMPLEMENTATION ====================
    public class StockOutService : IStockOutService
    {
        private readonly IStockOutRepository _stockOutRepository;
        private readonly IInventoryRepository _inventoryRepository;

        public StockOutService(IStockOutRepository stockOutRepository, IInventoryRepository inventoryRepository)
        {
            _stockOutRepository = stockOutRepository;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<StockOut> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID xuất kho không hợp lệ");
            var stockOut = await _stockOutRepository.GetByIdAsync(id);
            return stockOut ?? throw new KeyNotFoundException($"Không tìm thấy phiếu xuất kho với ID: {id}");
        }

        public async Task<IEnumerable<StockOut>> GetAllAsync()
        {
            return await _stockOutRepository.GetAllAsync();
        }

        public async Task<StockOut> CreateAsync(StockOut entity)
        {
            ValidateStockOut(entity);

            // Check available stock
            var availableStock = await _inventoryRepository.GetAvailableStockAsync(entity.VariantId, entity.StationId);
            if (availableStock < entity.Quantity)
                throw new InvalidOperationException($"Không đủ tồn kho. Tồn kho khả dụng: {availableStock}");

            return await _stockOutRepository.CreateAsync(entity);
        }

        public async Task<StockOut> UpdateAsync(StockOut entity)
        {
            if (entity.StockOutId <= 0) throw new ArgumentException("ID xuất kho không hợp lệ");
            ValidateStockOut(entity);

            var existing = await _stockOutRepository.GetByIdAsync(entity.StockOutId);
            if (existing == null) throw new KeyNotFoundException($"Không tìm thấy phiếu xuất kho với ID: {entity.StockOutId}");

            return await _stockOutRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID xuất kho không hợp lệ");
            return await _stockOutRepository.DeleteAsync(id);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            throw new NotSupportedException("Phiếu xuất kho không hỗ trợ xóa mềm");
        }

        public async Task<IEnumerable<StockOut>> GetStockOutsByStationAsync(int stationId)
        {
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");
            return await _stockOutRepository.GetStockOutsByStationAsync(stationId);
        }

        public async Task<IEnumerable<StockOut>> GetStockOutsByOrderAsync(int orderId)
        {
            if (orderId <= 0) throw new ArgumentException("ID đơn hàng không hợp lệ");
            return await _stockOutRepository.GetStockOutsByOrderAsync(orderId);
        }

        public async Task<IEnumerable<StockOut>> GetStockOutsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate) throw new ArgumentException("Ngày bắt đầu phải trước ngày kết thúc");
            return await _stockOutRepository.GetStockOutsByDateRangeAsync(startDate, endDate);
        }

        public async Task<decimal> GetTotalStockOutQuantityAsync(DateTime startDate, DateTime endDate, int? variantId = null)
        {
            if (startDate > endDate) throw new ArgumentException("Ngày bắt đầu phải trước ngày kết thúc");
            return await _stockOutRepository.GetTotalStockOutQuantityAsync(startDate, endDate, variantId);
        }

        public async Task<bool> ProcessStockOutAsync(StockOut stockOut)
        {
            ValidateStockOut(stockOut);

            // Check and reserve stock
            var availableStock = await _inventoryRepository.GetAvailableStockAsync(stockOut.VariantId, stockOut.StationId);
            if (availableStock < stockOut.Quantity)
                throw new InvalidOperationException($"Không đủ tồn kho. Tồn kho khả dụng: {availableStock}");

            // Create stock out record
            var createdStockOut = await _stockOutRepository.CreateAsync(stockOut);

            // Update inventory
            var currentStock = await _inventoryRepository.GetCurrentStockAsync(stockOut.VariantId, stockOut.StationId);
            var newStock = currentStock - stockOut.Quantity;
            await _inventoryRepository.UpdateStockAsync(stockOut.VariantId, stockOut.StationId, newStock);

            return createdStockOut != null;
        }

        public async Task<Dictionary<string, object>> GetStockOutSummaryAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate) throw new ArgumentException("Ngày bắt đầu phải trước ngày kết thúc");

            var stockOuts = await _stockOutRepository.GetStockOutsByDateRangeAsync(startDate, endDate);
            var totalQuantity = await _stockOutRepository.GetTotalStockOutQuantityAsync(startDate, endDate);

            return new Dictionary<string, object>
            {
                ["Period"] = $"{startDate:dd/MM/yyyy} - {endDate:dd/MM/yyyy}",
                ["TotalTransactions"] = stockOuts.Count(),
                ["TotalQuantity"] = totalQuantity,
                ["ByPurpose"] = stockOuts.GroupBy(so => so.Purpose)
                                        .ToDictionary(g => g.Key, g => g.Sum(so => so.Quantity)),
                ["TopProducts"] = stockOuts.GroupBy(so => so.VariantId)
                                          .OrderByDescending(g => g.Sum(so => so.Quantity))
                                          .Take(5)
            };
        }

        private void ValidateStockOut(StockOut stockOut)
        {
            if (stockOut == null) throw new ArgumentNullException(nameof(stockOut));
            if (stockOut.VariantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            if (stockOut.StationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");
            if (stockOut.Quantity <= 0) throw new ArgumentException("Số lượng phải lớn hơn 0");
            if (string.IsNullOrWhiteSpace(stockOut.Purpose)) throw new ArgumentException("Mục đích không được trống");
        }
    }

    // ==================== STOCK ALERT SERVICE IMPLEMENTATION ====================
    public class StockAlertService : IStockAlertService
    {
        private readonly IStockAlertRepository _stockAlertRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IBatchRepository _batchRepository;

        public StockAlertService(IStockAlertRepository stockAlertRepository, IInventoryRepository inventoryRepository, IBatchRepository batchRepository)
        {
            _stockAlertRepository = stockAlertRepository;
            _inventoryRepository = inventoryRepository;
            _batchRepository = batchRepository;
        }

        public async Task<StockAlert> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID cảnh báo không hợp lệ");
            var alert = await _stockAlertRepository.GetByIdAsync(id);
            return alert ?? throw new KeyNotFoundException($"Không tìm thấy cảnh báo với ID: {id}");
        }

        public async Task<IEnumerable<StockAlert>> GetAllAsync()
        {
            return await _stockAlertRepository.GetAllAsync();
        }

        public async Task<StockAlert> CreateAsync(StockAlert entity)
        {
            ValidateStockAlert(entity);
            return await _stockAlertRepository.CreateAsync(entity);
        }

        public async Task<StockAlert> UpdateAsync(StockAlert entity)
        {
            if (entity.AlertId <= 0) throw new ArgumentException("ID cảnh báo không hợp lệ");
            ValidateStockAlert(entity);

            var existing = await _stockAlertRepository.GetByIdAsync(entity.AlertId);
            if (existing == null) throw new KeyNotFoundException($"Không tìm thấy cảnh báo với ID: {entity.AlertId}");

            return await _stockAlertRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID cảnh báo không hợp lệ");
            return await _stockAlertRepository.DeleteAsync(id);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            return await _stockAlertRepository.DeactivateAlertAsync(id);
        }

        public async Task<IEnumerable<StockAlert>> GetActiveAlertsAsync()
        {
            return await _stockAlertRepository.GetActiveAlertsAsync();
        }

        public async Task<IEnumerable<StockAlert>> GetAlertsByStationAsync(int stationId)
        {
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");
            return await _stockAlertRepository.GetAlertsByStationAsync(stationId);
        }

        public async Task<IEnumerable<StockAlert>> GetAlertsByTypeAsync(string alertType)
        {
            if (string.IsNullOrWhiteSpace(alertType)) throw new ArgumentException("Loại cảnh báo không được trống");
            return await _stockAlertRepository.GetAlertsByTypeAsync(alertType);
        }

        public async Task<bool> DeactivateAlertAsync(int alertId)
        {
            if (alertId <= 0) throw new ArgumentException("ID cảnh báo không hợp lệ");
            return await _stockAlertRepository.DeactivateAlertAsync(alertId);
        }

        public async Task<bool> CreateLowStockAlertAsync(int variantId, int stationId, decimal threshold)
        {
            if (variantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");
            if (threshold < 0) throw new ArgumentException("Ngưỡng không được âm");

            return await _stockAlertRepository.CreateLowStockAlertAsync(variantId, stationId, threshold);
        }

        public async Task<bool> CreateExpiryAlertAsync(int variantId, int stationId, int daysThreshold)
        {
            if (variantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");
            if (daysThreshold <= 0) throw new ArgumentException("Ngưỡng ngày phải lớn hơn 0");

            return await _stockAlertRepository.CreateExpiryAlertAsync(variantId, stationId, daysThreshold);
        }

        public async Task<bool> CheckAndCreateAlertsAsync()
        {
            var lowStockItems = await _inventoryRepository.GetLowStockItemsAsync(1); // Station 1 as example
            foreach (var item in lowStockItems)
            {
                await CreateLowStockAlertAsync(item.VariantId, item.StationId, 15); // Default threshold
            }

            var expiringBatches = await _batchRepository.GetExpiringBatchesAsync(15);
            foreach (var batch in expiringBatches)
            {
                await CreateExpiryAlertAsync(batch.VariantId, batch.StationId, 15);
            }

            return true;
        }

        public async Task<Dictionary<string, int>> GetAlertStatisticsAsync()
        {
            var activeAlerts = await _stockAlertRepository.GetActiveAlertsAsync();
            var lowStockCount = await _stockAlertRepository.GetAlertCountByTypeAsync("low_stock");
            var expiryCount = await _stockAlertRepository.GetAlertCountByTypeAsync("expiry_warning");

            return new Dictionary<string, int>
            {
                ["TotalActiveAlerts"] = activeAlerts.Count(),
                ["LowStockAlerts"] = lowStockCount,
                ["ExpiryAlerts"] = expiryCount,
                ["OtherAlerts"] = activeAlerts.Count(a => a.AlertType != "low_stock" && a.AlertType != "expiry_warning")
            };
        }

        private void ValidateStockAlert(StockAlert alert)
        {
            if (alert == null) throw new ArgumentNullException(nameof(alert));
            if (alert.VariantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            if (alert.StationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");
            if (string.IsNullOrWhiteSpace(alert.AlertType)) throw new ArgumentException("Loại cảnh báo không được trống");
            if (alert.ThresholdValue < 0) throw new ArgumentException("Giá trị ngưỡng không được âm");
        }
    }

    // ==================== STOCK AUDIT SERVICE IMPLEMENTATION ====================
    public class StockAuditService : IStockAuditService
    {
        private readonly IStockAuditRepository _stockAuditRepository;
        private readonly IInventoryRepository _inventoryRepository;

        public StockAuditService(IStockAuditRepository stockAuditRepository, IInventoryRepository inventoryRepository)
        {
            _stockAuditRepository = stockAuditRepository;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<StockAudit> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID kiểm kê không hợp lệ");
            var audit = await _stockAuditRepository.GetByIdAsync(id);
            return audit ?? throw new KeyNotFoundException($"Không tìm thấy phiếu kiểm kê với ID: {id}");
        }

        public async Task<IEnumerable<StockAudit>> GetAllAsync()
        {
            return await _stockAuditRepository.GetAllAsync();
        }

        public async Task<StockAudit> CreateAsync(StockAudit entity)
        {
            ValidateStockAudit(entity);

            // Calculate variance
            entity.Variance = entity.ActualQuantity - entity.SystemQuantity;

            // Set default status if not provided
            if (string.IsNullOrWhiteSpace(entity.Status))
                entity.Status = "pending";

            return await _stockAuditRepository.CreateAsync(entity);
        }

        public async Task<StockAudit> UpdateAsync(StockAudit entity)
        {
            if (entity.AuditId <= 0) throw new ArgumentException("ID kiểm kê không hợp lệ");
            ValidateStockAudit(entity);

            var existing = await _stockAuditRepository.GetByIdAsync(entity.AuditId);
            if (existing == null) throw new KeyNotFoundException($"Không tìm thấy phiếu kiểm kê với ID: {entity.AuditId}");

            // Recalculate variance
            entity.Variance = entity.ActualQuantity - entity.SystemQuantity;

            return await _stockAuditRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID kiểm kê không hợp lệ");
            return await _stockAuditRepository.DeleteAsync(id);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            throw new NotSupportedException("Phiếu kiểm kê không hỗ trợ xóa mềm");
        }

        public async Task<IEnumerable<StockAudit>> GetAuditsByStationAsync(int stationId)
        {
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");
            return await _stockAuditRepository.GetAuditsByStationAsync(stationId);
        }

        public async Task<IEnumerable<StockAudit>> GetAuditsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate) throw new ArgumentException("Ngày bắt đầu phải trước ngày kết thúc");
            return await _stockAuditRepository.GetAuditsByDateRangeAsync(startDate, endDate);
        }

        public async Task<IEnumerable<StockAudit>> GetPendingAuditsAsync()
        {
            return await _stockAuditRepository.GetPendingAuditsAsync();
        }

        public async Task<bool> UpdateAuditStatusAsync(int auditId, string status)
        {
            if (auditId <= 0) throw new ArgumentException("ID kiểm kê không hợp lệ");
            if (string.IsNullOrWhiteSpace(status)) throw new ArgumentException("Trạng thái không được trống");

            var validStatuses = new[] { "pending", "approved", "rejected", "other" };
            if (!validStatuses.Contains(status))
                throw new ArgumentException($"Trạng thái không hợp lệ: {status}");

            // If approved, update inventory
            if (status == "approved")
            {
                var audit = await _stockAuditRepository.GetByIdAsync(auditId);
                if (audit != null)
                {
                    await _inventoryRepository.UpdateStockAsync(audit.VariantId, audit.StationId, audit.ActualQuantity);
                }
            }

            return await _stockAuditRepository.UpdateAuditStatusAsync(auditId, status);
        }

        public async Task<bool> CreateStockAuditAsync(StockAudit audit)
        {
            ValidateStockAudit(audit);

            // Get current system quantity
            var systemQuantity = await _inventoryRepository.GetCurrentStockAsync(audit.VariantId, audit.StationId);
            audit.SystemQuantity = systemQuantity;
            audit.Variance = audit.ActualQuantity - systemQuantity;

            // Set default values
            if (string.IsNullOrWhiteSpace(audit.Status))
                audit.Status = "pending";

            audit.AuditDate = DateTime.Now;

            await _stockAuditRepository.CreateAsync(audit);
            return true;
        }

        public async Task<Dictionary<string, object>> GetAuditSummaryAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate) throw new ArgumentException("Ngày bắt đầu phải trước ngày kết thúc");

            var audits = await _stockAuditRepository.GetAuditsByDateRangeAsync(startDate, endDate);
            var totalVarianceValue = await _stockAuditRepository.GetTotalVarianceValueAsync(startDate, endDate);

            var approvedAudits = audits.Where(a => a.Status == "approved");
            var pendingAudits = audits.Where(a => a.Status == "pending");
            var rejectedAudits = audits.Where(a => a.Status == "rejected");

            return new Dictionary<string, object>
            {
                ["TotalAudits"] = audits.Count(),
                ["ApprovedAudits"] = approvedAudits.Count(),
                ["PendingAudits"] = pendingAudits.Count(),
                ["RejectedAudits"] = rejectedAudits.Count(),
                ["TotalVarianceValue"] = totalVarianceValue,
                ["AverageVariance"] = audits.Any() ? audits.Average(a => Math.Abs(a.Variance)) : 0,
                ["Period"] = $"{startDate:dd/MM/yyyy} - {endDate:dd/MM/yyyy}"
            };
        }

        private void ValidateStockAudit(StockAudit audit)
        {
            if (audit == null) throw new ArgumentNullException(nameof(audit));
            if (audit.VariantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            if (audit.StationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");
            if (audit.AuditedBy <= 0) throw new ArgumentException("ID người kiểm kê không hợp lệ");
            if (audit.SystemQuantity < 0) throw new ArgumentException("Số lượng hệ thống không được âm");
            if (audit.ActualQuantity < 0) throw new ArgumentException("Số lượng thực tế không được âm");
        }
    }

    // ==================== VARIANT STOCK RULE SERVICE IMPLEMENTATION ====================
    public class VariantStockRuleService : IVariantStockRuleService
    {
        private readonly IVariantStockRuleRepository _variantStockRuleRepository;
        private readonly IVariantRepository _variantRepository;

        public VariantStockRuleService(IVariantStockRuleRepository variantStockRuleRepository, IVariantRepository variantRepository)
        {
            _variantStockRuleRepository = variantStockRuleRepository;
            _variantRepository = variantRepository;
        }

        public async Task<VariantStockRule> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID quy tắc không hợp lệ");
            var rule = await _variantStockRuleRepository.GetByIdAsync(id);
            return rule ?? throw new KeyNotFoundException($"Không tìm thấy quy tắc với ID: {id}");
        }

        public async Task<IEnumerable<VariantStockRule>> GetAllAsync()
        {
            return await _variantStockRuleRepository.GetAllAsync();
        }

        public async Task<VariantStockRule> CreateAsync(VariantStockRule entity)
        {
            ValidateStockRule(entity);

            // Check if rule already exists for variant and station
            var existing = await _variantStockRuleRepository.GetRuleByVariantAndStationAsync(entity.VariantId, entity.StationId);
            if (existing != null)
                throw new InvalidOperationException($"Đã tồn tại quy tắc cho sản phẩm tại trạm này");

            return await _variantStockRuleRepository.CreateAsync(entity);
        }

        public async Task<VariantStockRule> UpdateAsync(VariantStockRule entity)
        {
            if (entity.RuleId <= 0) throw new ArgumentException("ID quy tắc không hợp lệ");
            ValidateStockRule(entity);

            var existing = await _variantStockRuleRepository.GetByIdAsync(entity.RuleId);
            if (existing == null) throw new KeyNotFoundException($"Không tìm thấy quy tắc với ID: {entity.RuleId}");

            return await _variantStockRuleRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID quy tắc không hợp lệ");
            return await _variantStockRuleRepository.DeleteAsync(id);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            throw new NotSupportedException("Quy tắc tồn kho không hỗ trợ xóa mềm");
        }

        public async Task<VariantStockRule> GetRuleByVariantAndStationAsync(int variantId, int stationId)
        {
            if (variantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");

            var rule = await _variantStockRuleRepository.GetRuleByVariantAndStationAsync(variantId, stationId);
            return rule ?? throw new KeyNotFoundException($"Không tìm thấy quy tắc cho sản phẩm tại trạm");
        }

        public async Task<IEnumerable<VariantStockRule>> GetRulesByStationAsync(int stationId)
        {
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");
            return await _variantStockRuleRepository.GetRulesByStationAsync(stationId);
        }

        public async Task<bool> UpdateStockRuleAsync(int variantId, int stationId, decimal minStock, decimal maxStock, int expiryDays)
        {
            if (variantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");
            if (minStock < 0) throw new ArgumentException("Tồn kho tối thiểu không được âm");
            if (maxStock < 0) throw new ArgumentException("Tồn kho tối đa không được âm");
            if (minStock >= maxStock) throw new ArgumentException("Tồn kho tối thiểu phải nhỏ hơn tồn kho tối đa");
            if (expiryDays < 0) throw new ArgumentException("Số ngày cảnh báo hết hạn không được âm");

            return await _variantStockRuleRepository.UpdateStockRuleAsync(variantId, stationId, minStock, maxStock, expiryDays);
        }

        public async Task<bool> ApplyDefaultRulesAsync(int variantId, int stationId)
        {
            if (variantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");

            var variant = await _variantRepository.GetByIdAsync(variantId);
            if (variant == null) throw new KeyNotFoundException($"Không tìm thấy biến thể với ID: {variantId}");

            // Apply default rules based on product type or other criteria
            decimal minStock = 15;
            decimal maxStock = 1000;
            int expiryDays = 15;

            var existingRule = await _variantStockRuleRepository.GetRuleByVariantAndStationAsync(variantId, stationId);
            if (existingRule != null)
            {
                return await _variantStockRuleRepository.UpdateStockRuleAsync(variantId, stationId, minStock, maxStock, expiryDays);
            }
            else
            {
                var newRule = new VariantStockRule
                {
                    VariantId = variantId,
                    StationId = stationId,
                    MinStock = minStock,
                    MaxStock = maxStock,
                    ExpiryDays = expiryDays
                };
                await _variantStockRuleRepository.CreateAsync(newRule);
                return true;
            }
        }

        public async Task<Dictionary<string, object>> GetRuleEffectivenessAsync(int stationId)
        {
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");

            var rules = await _variantStockRuleRepository.GetRulesByStationAsync(stationId);
            var rulesNeedingUpdate = await _variantStockRuleRepository.GetRulesNeedingUpdateAsync();

            return new Dictionary<string, object>
            {
                ["TotalRules"] = rules.Count(),
                ["RulesApplied"] = rules.Count(r => r.MinStock > 0 || r.MaxStock > 0),
                ["RulesNeedingUpdate"] = rulesNeedingUpdate.Count(r => r.StationId == stationId),
                ["AverageMinStock"] = rules.Any() ? rules.Average(r => r.MinStock) : 0,
                ["AverageMaxStock"] = rules.Any() ? rules.Average(r => r.MaxStock) : 0,
                ["StationId"] = stationId
            };
        }

        private void ValidateStockRule(VariantStockRule rule)
        {
            if (rule == null) throw new ArgumentNullException(nameof(rule));
            if (rule.VariantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            if (rule.StationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");
            if (rule.MinStock < 0) throw new ArgumentException("Tồn kho tối thiểu không được âm");
            if (rule.MaxStock < 0) throw new ArgumentException("Tồn kho tối đa không được âm");
            if (rule.MinStock >= rule.MaxStock) throw new ArgumentException("Tồn kho tối thiểu phải nhỏ hơn tồn kho tối đa");
            if (rule.ExpiryDays < 0) throw new ArgumentException("Số ngày cảnh báo hết hạn không được âm");
        }
    }
}
