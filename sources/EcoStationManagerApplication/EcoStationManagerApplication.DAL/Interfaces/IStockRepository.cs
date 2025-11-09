using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public interface IStockInRepository : IRepository<StockIn>
    {
        /// <summary>
        /// Lấy lịch sử nhập kho theo sản phẩm
        /// </summary>
        Task<IEnumerable<StockIn>> GetByProductAsync(int productId);

        /// <summary>
        /// Lấy lịch sử nhập kho theo nhà cung cấp
        /// </summary>
        Task<IEnumerable<StockIn>> GetBySupplierAsync(int supplierId);

        /// <summary>
        /// Lấy lịch sử nhập kho theo reference (PRODUCT/PACKAGING)
        /// </summary>
        Task<IEnumerable<StockIn>> GetByReferenceAsync(RefType refType, int refId);

        /// <summary>
        /// Lấy lịch sử nhập kho theo khoảng thời gian
        /// </summary>
        Task<IEnumerable<StockIn>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Lấy lịch sử nhập kho theo số lô
        /// </summary>
        Task<IEnumerable<StockIn>> GetByBatchAsync(string batchNo);

        /// <summary>
        /// Lấy tổng giá trị nhập kho
        /// </summary>
        Task<decimal> GetTotalStockInValueAsync(DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Lấy tổng số lượng nhập kho theo sản phẩm
        /// </summary>
        Task<decimal> GetTotalQuantityByProductAsync(int productId, DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Lấy top sản phẩm nhập nhiều nhất
        /// </summary>
        Task<IEnumerable<StockInSummary>> GetTopStockInProductsAsync(int limit = 10, DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Lấy nhật ký nhập kho với thông tin chi tiết
        /// </summary>
        Task<IEnumerable<StockInDetail>> GetStockInDetailsAsync(DateTime? fromDate = null, DateTime? toDate = null);

        /// <summary>
        /// Thống kê nhập kho theo nhà cung cấp
        /// </summary>
        Task<IEnumerable<SupplierStockInSummary>> GetStockInBySupplierAsync(DateTime? fromDate = null, DateTime? toDate = null);

        /// <summary>
        /// Kiểm tra số lô đã tồn tại
        /// </summary>
        Task<bool> IsBatchExistsAsync(string batchNo, RefType refType, int refId);

        Task<(IEnumerable<StockIn> StockIns, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string search = null,
            RefType? refType = null
        );
    }

    public interface IStockOutRepository : IRepository<StockOut>
    {
        /// <summary>
        /// Lấy lịch sử xuất kho theo sản phẩm
        /// </summary>
        Task<IEnumerable<StockOut>> GetByProductAsync(int productId);

        /// <summary>
        /// Lấy lịch sử xuất kho theo reference (PRODUCT/PACKAGING)
        /// </summary>
        Task<IEnumerable<StockOut>> GetByReferenceAsync(RefType refType, int refId);

        /// <summary>
        /// Lấy lịch sử xuất kho theo khoảng thời gian
        /// </summary>
        Task<IEnumerable<StockOut>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Lấy lịch sử xuất kho theo mục đích (SALE, DAMAGE, TRANSFER)
        /// </summary>
        Task<IEnumerable<StockOut>> GetByPurposeAsync(StockOutPurpose purpose);

        /// <summary>
        /// Lấy lịch sử xuất kho theo số lô
        /// </summary>
        Task<IEnumerable<StockOut>> GetByBatchAsync(string batchNo);

        /// <summary>
        /// Lấy tổng giá trị xuất kho
        /// </summary>
        Task<decimal> GetTotalStockOutValueAsync(DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Lấy tổng số lượng xuất kho theo sản phẩm
        /// </summary>
        Task<decimal> GetTotalQuantityByProductAsync(int productId, DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Lấy top sản phẩm xuất nhiều nhất
        /// </summary>
        Task<IEnumerable<StockOutSummary>> GetTopStockOutProductsAsync(int limit = 10, DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Xuất kho cho đơn hàng
        /// </summary>
        Task<bool> StockOutForOrderAsync(int productId, string batchNo, decimal quantity, int orderId, int userId);

        /// <summary>
        /// Xuất kho nhiều sản phẩm cùng lúc
        /// </summary>
        Task<bool> StockOutMultipleAsync(IEnumerable<StockOut> stockOuts);

        /// <summary>
        /// Lấy nhật ký xuất kho với thông tin chi tiết
        /// </summary>
        Task<IEnumerable<StockOutDetail>> GetStockOutDetailsAsync(DateTime? fromDate = null, DateTime? toDate = null);

        /// <summary>
        /// Thống kê xuất kho theo mục đích
        /// </summary>
        Task<IEnumerable<PurposeStockOutSummary>> GetStockOutByPurposeAsync(DateTime? fromDate = null, DateTime? toDate = null);

        /// <summary>
        /// Kiểm tra có thể xuất kho (đủ số lượng)
        /// </summary>
        Task<bool> CanStockOutAsync(int productId, string batchNo, decimal quantity);

        Task<(IEnumerable<StockOut> StockOuts, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string search = null,
            RefType? refType = null,
            StockOutPurpose? purpose = null
        );
    }
}
