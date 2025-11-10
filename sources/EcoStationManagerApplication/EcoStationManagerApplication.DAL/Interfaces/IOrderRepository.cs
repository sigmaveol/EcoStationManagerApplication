using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        /// <summary>
        /// Lấy đơn hàng theo code
        /// </summary>
        Task<Order> GetByOrderCodeAsync(string orderCode);

        /// <summary>
        /// Lấy đơn hàng theo trạng thái
        /// </summary>
        Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status);

        /// <summary>
        /// Lấy đơn hàng theo khách hàng
        /// </summary>
        Task<IEnumerable<Order>> GetByCustomerAsync(int customerId);

        /// <summary>
        /// Lấy đơn hàng theo người tạo
        /// </summary>
        Task<IEnumerable<Order>> GetByUserAsync(int userId);

        /// <summary>
        /// Lấy đơn hàng theo khoảng thời gian
        /// </summary>
        Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Lấy đơn hàng hôm nay
        /// </summary>
        Task<IEnumerable<Order>> GetTodayOrdersAsync();

        /// <summary>
        /// Lấy đơn hàng với chi tiết đầy đủ
        /// </summary>
        Task<Order> GetOrderWithDetailsAsync(int orderId);

        /// <summary>
        /// Lấy tổng doanh thu
        /// </summary>
        Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Lấy số lượng đơn hàng
        /// </summary>
        Task<int> GetOrderCountAsync(DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Lấy doanh thu theo tháng
        /// </summary>
        Task<decimal> GetMonthlyRevenueAsync(int year, int month);

        /// <summary>
        /// Tìm kiếm đơn hàng
        /// </summary>
        Task<IEnumerable<Order>> SearchOrdersAsync(string keyword);

        /// <summary>
        /// Cập nhật trạng thái đơn hàng
        /// </summary>
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus);

        /// <summary>
        /// Cập nhật trạng thái thanh toán
        /// </summary>
        Task<bool> UpdatePaymentStatusAsync(int orderId, PaymentStatus newPaymentStatus);

        /// <summary>
        /// Lấy đơn hàng cần xử lý (DRAFT, CONFIRMED, PROCESSING)
        /// </summary>
        Task<IEnumerable<Order>> GetPendingOrdersAsync();

        /// <summary>
        /// Phân trang đơn hàng
        /// </summary>
        Task<(IEnumerable<Order> Orders, int TotalCount)> GetPagedOrdersAsync(
            int pageNumber,
            int pageSize,
            string search = null,
            OrderStatus? status = null,
            PaymentStatus? paymentStatus = null,
            ProductType? productType = null,
            OrderSource? source = null,
            decimal? minTotal = null,
            int? customerId = null,
            int? userId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null
        );

    }

    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        /// <summary>
        /// Lấy chi tiết đơn hàng theo orderId
        /// </summary>
        Task<IEnumerable<OrderDetail>> GetByOrderAsync(int orderId);

        /// <summary>
        /// Lấy chi tiết đơn hàng với thông tin sản phẩm
        /// </summary>
        Task<IEnumerable<OrderDetail>> GetOrderDetailsWithProductsAsync(int orderId);

        /// <summary>
        /// Thêm nhiều chi tiết đơn hàng
        /// </summary>
        Task<bool> AddRangeAsync(IEnumerable<OrderDetail> orderDetails);

        /// <summary>
        /// Xóa tất cả chi tiết theo orderId
        /// </summary>
        Task<bool> DeleteByOrderAsync(int orderId);

        /// <summary>
        /// Cập nhật nhiều chi tiết cùng lúc (batch update)
        /// </summary>
        Task<bool> UpdateRangeAsync(IEnumerable<OrderDetail> orderDetails);

        /// <summary>
        /// Cập nhật số lượng sản phẩm trong chi tiết đơn hàng
        /// </summary>
        Task<bool> UpdateQuantityAsync(int orderDetailId, decimal quantity);

        /// <summary>
        /// Cập nhật đơn giá sản phẩm trong chi tiết đơn hàng
        /// </summary>
        Task<bool> UpdateUnitPriceAsync(int orderDetailId, decimal unitPrice);

        /// <summary>
        /// Lấy tổng số lượng sản phẩm đã bán
        /// </summary>
        Task<decimal> GetTotalSoldQuantityAsync(int productId, DateTime? fromDate = null, DateTime? toDate = null);

        /// <summary>
        /// Lấy top sản phẩm bán chạy
        /// </summary>
        Task<IEnumerable<ProductSales>> GetTopSellingProductsAsync(int limit = 10, DateTime? fromDate = null, DateTime? toDate = null);

        /// <summary>
        /// Lấy doanh thu theo sản phẩm
        /// </summary>
        Task<IEnumerable<ProductRevenue>> GetProductRevenueAsync(DateTime? fromDate = null, DateTime? toDate = null);

        /// <summary>
        /// Kiểm tra sản phẩm có trong đơn hàng nào không
        /// </summary>
        Task<bool> IsProductInAnyOrderAsync(int productId);
    }
}
