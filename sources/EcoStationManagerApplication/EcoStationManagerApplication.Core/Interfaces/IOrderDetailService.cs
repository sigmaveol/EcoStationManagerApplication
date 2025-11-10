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
    public interface IOrderDetailService
    {
        /// <summary>
        /// Lấy chi tiết đơn hàng theo ID
        /// </summary>
        Task<Result<OrderDetail>> GetOrderDetailByIdAsync(int orderDetailId);

        /// <summary>
        /// Lấy tất cả chi tiết đơn hàng theo orderId
        /// </summary>
        Task<Result<IEnumerable<OrderDetail>>> GetOrderDetailsByOrderAsync(int orderId);

        /// <summary>
        /// Thêm chi tiết đơn hàng mới
        /// </summary>
        Task<Result<int>> AddOrderDetailAsync(OrderDetail orderDetail);

        /// <summary>
        /// Thêm nhiều chi tiết đơn hàng cùng lúc
        /// </summary>
        Task<Result<bool>> AddOrderDetailsRangeAsync(List<OrderDetail> orderDetails);

        /// <summary>
        /// Cập nhật chi tiết đơn hàng
        /// </summary>
        Task<Result<bool>> UpdateOrderDetailAsync(OrderDetail orderDetail);

        /// <summary>
        /// Cập nhật số lượng sản phẩm trong chi tiết đơn hàng
        /// </summary>
        Task<Result<bool>> UpdateOrderDetailQuantityAsync(int orderDetailId, decimal newQuantity);

        /// <summary>
        /// Cập nhật đơn giá sản phẩm trong chi tiết đơn hàng
        /// </summary>
        Task<Result<bool>> UpdateOrderDetailUnitPriceAsync(int orderDetailId, decimal newUnitPrice);

        /// <summary>
        /// Xóa chi tiết đơn hàng
        /// </summary>
        Task<Result<bool>> DeleteOrderDetailAsync(int orderDetailId);

        /// <summary>
        /// Xóa tất cả chi tiết đơn hàng theo orderId
        /// </summary>
        Task<Result<bool>> DeleteOrderDetailsByOrderAsync(int orderId);

        /// <summary>
        /// Tính tổng tiền của đơn hàng từ chi tiết
        /// </summary>
        Task<Result<decimal>> CalculateOrderTotalAsync(int orderId);

        /// <summary>
        /// Kiểm tra sản phẩm có trong đơn hàng nào không
        /// </summary>
        Task<Result<bool>> IsProductInAnyOrderAsync(int productId);

        /// <summary>
        /// Lấy tổng số lượng sản phẩm đã bán
        /// </summary>
        Task<Result<decimal>> GetTotalSoldQuantityAsync(int productId, DateTime? fromDate = null, DateTime? toDate = null);

        /// <summary>
        /// Lấy top sản phẩm bán chạy
        /// </summary>
        Task<Result<IEnumerable<ProductSales>>> GetTopSellingProductsAsync(int limit = 10, DateTime? fromDate = null, DateTime? toDate = null);

        /// <summary>
        /// Lấy doanh thu theo sản phẩm
        /// </summary>
        Task<Result<IEnumerable<ProductRevenue>>> GetProductRevenueAsync(DateTime? fromDate = null, DateTime? toDate = null);

        /// <summary>
        /// Lấy thống kê sản phẩm theo danh mục
        /// </summary>
        //Task<Result<IEnumerable<CategorySales>>> GetSalesByCategoryAsync(DateTime? fromDate = null, DateTime? toDate = null);
    }
}
