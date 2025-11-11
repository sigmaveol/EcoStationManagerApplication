using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface IInventoryService
    {
        /// <summary>
        /// Lấy thông tin chi tiết một bản ghi tồn kho theo ID.
        /// </summary>
        Task<Result<Inventory>> GetInventoryByIdAsync(int inventoryId);

        Task<Result<List<Inventory>>> GetAllAsync();

        /// <summary>
        /// Lấy danh sách các bản ghi tồn kho theo mã sản phẩm.
        /// </summary>
        Task<Result<List<Inventory>>> GetInventoryByProductAsync(int productId);

        /// <summary>
        /// Tính tổng số lượng tồn kho hiện tại của một sản phẩm.
        /// </summary>
        Task<Result<decimal>> GetTotalStockQuantityAsync(int productId);

        /// <summary>
        /// Lấy danh sách sản phẩm có số lượng tồn dưới ngưỡng cảnh báo.
        /// </summary>
        Task<Result<List<Inventory>>> GetLowStockItemsAsync();

        /// <summary>
        /// Lấy danh sách sản phẩm sắp hết hạn trong khoảng thời gian quy định.
        /// </summary>
        Task<Result<List<Inventory>>> GetExpiringItemsAsync(int daysThreshold = 15);

        /// <summary>
        /// Ghi nhận nhập kho: thêm lô hàng mới hoặc bổ sung tồn kho cho sản phẩm.
        /// </summary>
        Task<Result<Inventory>> AddStockAsync(
            int productId, string batchNo, decimal quantity, DateTime? expiryDate);

        /// <summary>
        /// Cập nhật số lượng tồn kho của một bản ghi cụ thể (ví dụ: điều chỉnh thủ công).
        /// </summary>
        Task<Result<bool>> UpdateStockQuantityAsync(int inventoryId, decimal newQuantity);

        /// <summary>
        /// Giảm tồn kho (xuất kho) cho một sản phẩm, ví dụ khi bán hàng hoặc sử dụng nội bộ.
        /// </summary>
        Task<Result<bool>> ReduceStockAsync(
            int productId, string batchNo, decimal quantity);

        /// <summary>
        /// Kiểm tra xem tồn kho có đủ đáp ứng số lượng yêu cầu hay không.
        /// </summary>
        Task<Result<bool>> IsStockSufficientAsync(int productId, decimal requiredQuantity);

        /// <summary>
        /// Lấy danh sách cảnh báo tồn kho (sản phẩm sắp hết, hết hạn...).
        /// </summary>
        //Task<Result<List<InventoryAlert>>> GetInventoryAlertsAsync();

        /// <summary>
        /// Lấy thông tin tổng hợp tồn kho: tổng sản phẩm, giá trị tồn, cảnh báo...
        /// </summary>
        //Task<Result<InventorySummary>> GetInventorySummaryAsync();

        /// <summary>
        /// Lấy lịch sử nhập – xuất của một sản phẩm trong khoảng thời gian cụ thể.
        /// </summary>
        Task<Result<List<Inventory>>> GetInventoryHistoryAsync(
            int productId, DateTime? fromDate = null, DateTime? toDate = null);

        /// <summary>
        /// Lấy danh sách tồn kho phân trang, có thể tìm kiếm hoặc lọc theo điều kiện.
        /// </summary>
        //Task<Result<PagedResult<Inventory>>> GetPagedInventoriesAsync(
        //int pageNumber,
        //int pageSize,
        //string search = null,
        //bool includeExpired = false);

    }
}
