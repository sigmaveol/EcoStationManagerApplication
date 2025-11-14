 using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface IOrderService
    {
        Task<Result<Order>> GetOrderByCode(string orderCode);
        /// <summary>
        /// Lây tất cả đơn hàng
        /// </summary>
        /// <returns></returns>
        Task<Result<IEnumerable<OrderDTO>>> GetAllAsync();

        Task<Result<IEnumerable<OrderDTO>>> GetProcessingOrdersAsync();

        /// <summary>
        /// Lấy thông tin đơn hàng theo ID
        /// </summary>
        Task<Result<Order>> GetOrderByIdAsync(int orderId);

        /// <summary>
        /// Lấy thông tin đơn hàng với chi tiết đầy đủ
        /// </summary>
        Task<Result<Order>> GetOrderWithDetailsAsync(int orderId);

        /// <summary>
        /// Lấy đơn hàng theo trạng thái
        /// </summary>
        Task<Result<IEnumerable<Order>>> GetOrdersByStatusAsync(OrderStatus status);

        /// <summary>
        /// Lấy đơn hàng hôm nay
        /// </summary>
        Task<Result<IEnumerable<Order>>> GetTodayOrdersAsync();

        /// <summary>
        /// Lấy đơn hàng cần xử lý
        /// </summary>
        Task<Result<IEnumerable<Order>>> GetPendingOrdersAsync();

        /// <summary>
        /// Tạo đơn hàng mới
        /// </summary>
        Task<Result<int>> CreateOrderAsync(Order order, List<OrderDetail> orderDetails);

        /// <summary>
        /// Cập nhật trạng thái đơn hàng
        /// </summary>
        Task<Result<bool>> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus);

        /// <summary>
        /// Cập nhật trạng thái thanh toán
        /// </summary>
        Task<Result<bool>> UpdatePaymentStatusAsync(int orderId, PaymentStatus newPaymentStatus);

        /// <summary>
        /// Thêm chi tiết đơn hàng
        /// </summary>
        Task<Result<bool>> AddOrderDetailsAsync(int orderId, List<OrderDetail> orderDetails);

        /// <summary>
        /// Lấy tổng doanh thu theo thời gian
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        Task<Result<decimal>> GetTotalRevenueAsync(DateTime? fromDate = null, DateTime? toDate = null);

        /// <summary>
        /// Lấy thống kê đơn hàng
        /// </summary>
        Task<Result<OrderSummary>> GetOrderSummaryAsync(DateTime? fromDate = null, DateTime? toDate = null);

        /// <summary>
        /// Tìm kiếm đơn hàng
        /// </summary>
        Task<Result<IEnumerable<Order>>> SearchOrdersAsync(string keyword);

        /// <summary>
        /// Lấy danh sách đơn hàng phân trang
        /// </summary>
        Task<Result<(IEnumerable<Order> Orders, int TotalCount)>> GetPagedOrdersAsync(
            int pageNumber, int pageSize, OrderSearchCriteria criteria);
    }
}
