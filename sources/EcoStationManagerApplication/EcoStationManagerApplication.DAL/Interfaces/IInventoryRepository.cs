using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public interface IInventoryRepository : IRepository<Inventory>
    {
        /// <summary>
        /// Lấy tồn kho theo sản phẩm và lô
        /// </summary>
        Task<Inventory> GetByProductAndBatchAsync(int productId, string batchNo);

        /// <summary>
        /// Lấy tất cả tồn kho theo sản phẩm
        /// </summary>
        Task<IEnumerable<Inventory>> GetByProductAsync(int productId);

        /// <summary>
        /// Lấy sản phẩm sắp hết hàng
        /// </summary>
        Task<IEnumerable<Inventory>> GetLowStockItemsAsync();

        /// <summary>
        /// Lấy sản phẩm sắp hết hạn
        /// </summary>
        Task<IEnumerable<Inventory>> GetExpiringItemsAsync(int daysThreshold = 15);

        /// <summary>
        /// Lấy tổng số lượng tồn kho theo sản phẩm
        /// </summary>
        Task<decimal> GetTotalStockQuantityAsync(int productId);

        /// <summary>
        /// Lấy lịch sử tồn kho theo sản phẩm
        /// </summary>
        Task<IEnumerable<Inventory>> GetInventoryHistoryAsync(int productId, DateTime? fromDate = null, DateTime? toDate = null);
    }

        /// <summary>
        /// Thêm số lượng vào tồn kho
        /// </summary>
        Task<bool> AddStockAsync(int productId, string batchNo, decimal quantity, DateTime? expiryDate);

        /// <summary>
        /// Cập nhật số lượng tồn kho
        /// </summary>
        Task<bool> UpdateStockQuantityAsync(int inventoryId, decimal newQuantity);

        /// <summary>
        /// Giảm số lượng tồn kho
        /// </summary>
        Task<bool> ReduceStockAsync(int productId, string batchNo, decimal quantity);

        /// <summary>
        /// Kiểm tra số lượng tồn kho có đủ không
        /// </summary>
        Task<bool> IsStockSufficientAsync(int productId, decimal requiredQuantity);

        Task<(IEnumerable<Inventory> Inventories, int TotalCount)> GetPagedInventoriesAsync(
            int pageNumber,
            int pageSize,
            string search = null,
            bool includeExpired = false
        );
    }
}
