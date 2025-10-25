using EcoStationManagerApplication.Core.Exceptions;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    public class OrderService
    {
        //    private readonly IOrderRepository _orderRepository;
        //    private readonly IOrderDetailRepository _orderDetailRepository;
        //    private readonly ICustomerRepository _customerRepository;
        //    private readonly IInventoryService _inventoryService;

        //    public OrderService(
        //        IOrderRepository orderRepository,
        //        IOrderDetailRepository orderDetailRepository,
        //        ICustomerRepository customerRepository,
        //        IInventoryService inventoryService)
        //        : base(orderRepository)
        //    {
        //        _orderRepository = orderRepository;
        //        _orderDetailRepository = orderDetailRepository;
        //        _customerRepository = customerRepository;
        //        _inventoryService = inventoryService;
        //    }

        //    public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status)
        //    {
        //        try
        //        {
        //            if (string.IsNullOrWhiteSpace(status))
        //                throw new ValidationException("Trạng thái đơn hàng không được để trống");

        //            return await _orderRepository.GetOrdersByStatusAsync(status);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new ServiceException($"Lỗi khi lấy đơn hàng theo trạng thái {status}", ex);
        //        }
        //    }

        //    public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId)
        //    {
        //        try
        //        {
        //            if (customerId <= 0)
        //                throw new ValidationException("ID khách hàng không hợp lệ");

        //            var customer = await _customerRepository.GetByIdAsync(customerId);
        //            if (customer == null)
        //                throw new NotFoundException($"Khách hàng với ID {customerId} không tồn tại");

        //            return await _orderRepository.GetOrdersByCustomerAsync(customerId);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new ServiceException($"Lỗi khi lấy đơn hàng theo khách hàng {customerId}", ex);
        //        }
        //    }

        //    public async Task<IEnumerable<Order>> GetOrdersByStationAsync(int stationId)
        //    {
        //        try
        //        {
        //            if (stationId <= 0)
        //                throw new ValidationException("ID trạm không hợp lệ");

        //            return await _orderRepository.GetOrdersByStationAsync(stationId);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new ServiceException($"Lỗi khi lấy đơn hàng theo trạm {stationId}", ex);
        //        }
        //    }

        //    public async Task<Order> GetOrderWithDetailsAsync(int orderId)
        //    {
        //        try
        //        {
        //            if (orderId <= 0)
        //                throw new ValidationException("ID đơn hàng không hợp lệ");

        //            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);
        //            if (order == null)
        //                throw new NotFoundException($"Đơn hàng với ID {orderId} không tồn tại");

        //            return order;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new ServiceException($"Lỗi khi lấy chi tiết đơn hàng {orderId}", ex);
        //        }
        //    }

        //    public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        //    {
        //        try
        //        {
        //            if (orderId <= 0)
        //                throw new ValidationException("ID đơn hàng không hợp lệ");

        //            if (string.IsNullOrWhiteSpace(status))
        //                throw new ValidationException("Trạng thái không được để trống");

        //            var order = await _orderRepository.GetByIdAsync(orderId);
        //            if (order == null)
        //                throw new NotFoundException($"Đơn hàng với ID {orderId} không tồn tại");

        //            // Validate status transition
        //            if (!IsValidStatusTransition(order.Status, status))
        //                throw new BusinessRuleException($"Không thể chuyển từ trạng thái {order.Status} sang {status}");

        //            return await _orderRepository.UpdateOrderStatusAsync(orderId, status);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new ServiceException($"Lỗi khi cập nhật trạng thái đơn hàng {orderId}", ex);
        //        }
        //    }

        //    public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        //    {
        //        try
        //        {
        //            if (startDate > endDate)
        //                throw new ValidationException("Ngày bắt đầu không thể lớn hơn ngày kết thúc");

        //            return await _orderRepository.GetOrdersByDateRangeAsync(startDate, endDate);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new ServiceException("Lỗi khi lấy đơn hàng theo khoảng thời gian", ex);
        //        }
        //    }

        //    public async Task<decimal> GetTotalRevenueAsync(DateTime startDate, DateTime endDate)
        //    {
        //        try
        //        {
        //            if (startDate > endDate)
        //                throw new ValidationException("Ngày bắt đầu không thể lớn hơn ngày kết thúc");

        //            return await _orderRepository.GetTotalRevenueAsync(startDate, endDate);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new ServiceException("Lỗi khi tính tổng doanh thu", ex);
        //        }
        //    }

        //    public async Task<int> GetOrderCountByStatusAsync(string status)
        //    {
        //        try
        //        {
        //            if (string.IsNullOrWhiteSpace(status))
        //                throw new ValidationException("Trạng thái không được để trống");

        //            return await _orderRepository.GetOrderCountByStatusAsync(status);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new ServiceException($"Lỗi khi đếm số đơn hàng theo trạng thái {status}", ex);
        //        }
        //    }

        //    public async Task<Order> CreateOrderWithDetailsAsync(Order order, List<OrderDetail> details)
        //    {
        //        try
        //        {
        //            if (order == null)
        //                throw new ArgumentNullException(nameof(order));

        //            if (details == null || !details.Any())
        //                throw new ValidationException("Đơn hàng phải có ít nhất một sản phẩm");

        //            // Validate order data
        //            await ValidateOrderAsync(order);

        //            // Check stock availability
        //            await CheckStockAvailabilityAsync(details, order.StationId ?? 0);

        //            // Create order
        //            var createdOrder = await _orderRepository.CreateAsync(order);

        //            // Add order details
        //            foreach (var detail in details)
        //            {
        //                detail.OrderId = createdOrder.OrderId;
        //                await _orderDetailRepository.CreateAsync(detail);
        //            }

        //            // Reserve stock
        //            await ReserveStockForOrderAsync(details, order.StationId ?? 0);

        //            return createdOrder;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new ServiceException("Lỗi khi tạo đơn hàng với chi tiết", ex);
        //        }
        //    }

        //    public async Task<bool> CancelOrderAsync(int orderId, string reason)
        //    {
        //        try
        //        {
        //            if (orderId <= 0)
        //                throw new ValidationException("ID đơn hàng không hợp lệ");

        //            if (string.IsNullOrWhiteSpace(reason))
        //                throw new ValidationException("Lý do hủy không được để trống");

        //            var order = await _orderRepository.GetByIdAsync(orderId);
        //            if (order == null)
        //                throw new NotFoundException($"Đơn hàng với ID {orderId} không tồn tại");

        //            if (order.Status == "completed" || order.Status == "cancelled")
        //                throw new BusinessRuleException($"Không thể hủy đơn hàng ở trạng thái {order.Status}");

        //            // Release reserved stock
        //            var details = await _orderDetailRepository.GetDetailsByOrderAsync(orderId);
        //            await ReleaseStockForOrderAsync(details.ToList(), order.StationId ?? 0);

        //            // Update order status
        //            order.Status = "cancelled";
        //            order.Note = $"Đã hủy: {reason}. {order.Note}";

        //            await _orderRepository.UpdateAsync(order);
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new ServiceException($"Lỗi khi hủy đơn hàng {orderId}", ex);
        //        }
        //    }

        //    public async Task<bool> ValidateOrderAsync(Order order)
        //    {
        //        try
        //        {
        //            if (order == null)
        //                throw new ArgumentNullException(nameof(order));

        //            if (order.TotalAmount < 0)
        //                throw new ValidationException("Tổng tiền không thể âm");

        //            if (order.CustomerId.HasValue)
        //            {
        //                var customer = await _customerRepository.GetByIdAsync(order.CustomerId.Value);
        //                if (customer == null)
        //                    throw new ValidationException("Khách hàng không tồn tại");
        //            }

        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new ServiceException("Lỗi khi validate đơn hàng", ex);
        //        }
        //    }

        //    #region Private Methods

        //    private bool IsValidStatusTransition(string currentStatus, string newStatus)
        //    {
        //        var validTransitions = new Dictionary<string, List<string>>
        //        {
        //            { "draft", new List<string> { "confirmed", "cancelled" } },
        //            { "confirmed", new List<string> { "processing", "cancelled" } },
        //            { "processing", new List<string> { "ready", "cancelled" } },
        //            { "ready", new List<string> { "shipped", "cancelled" } },
        //            { "shipped", new List<string> { "completed", "cancelled" } },
        //            { "completed", new List<string> { } },
        //            { "cancelled", new List<string> { } }
        //        };

        //        return validTransitions.ContainsKey(currentStatus) &&
        //               validTransitions[currentStatus].Contains(newStatus);
        //    }

        //    private async Task CheckStockAvailabilityAsync(List<OrderDetail> details, int stationId)
        //    {
        //        foreach (var detail in details)
        //        {
        //            if (detail.VariantId.HasValue)
        //            {
        //                var availableStock = await _inventoryService.GetAvailableStockAsync(detail.VariantId.Value, stationId);
        //                if (availableStock < detail.Quantity)
        //                {
        //                    throw new BusinessRuleException($"Sản phẩm không đủ tồn kho. Cần: {detail.Quantity}, Có: {availableStock}");
        //                }
        //            }
        //        }
        //    }

        //    private async Task ReserveStockForOrderAsync(List<OrderDetail> details, int stationId)
        //    {
        //        foreach (var detail in details)
        //        {
        //            if (detail.VariantId.HasValue)
        //            {
        //                await _inventoryService.ReserveStockAsync(detail.VariantId.Value, stationId, detail.Quantity);
        //            }
        //        }
        //    }

        //    private async Task ReleaseStockForOrderAsync(List<OrderDetail> details, int stationId)
        //    {
        //        foreach (var detail in details)
        //        {
        //            if (detail.VariantId.HasValue)
        //            {
        //                await _inventoryService.ReleaseReservedStockAsync(detail.VariantId.Value, stationId, detail.Quantity);
        //            }
        //        }
        //    }

        //}

        //public class OrderDetailService : BaseService<OrderDetail>, IOrderDetailService
        //{
        //    private readonly IOrderDetailRepository _orderDetailRepository;
        //    private readonly IOrderRepository _orderRepository;

        //    public OrderDetailService(
        //        IOrderDetailRepository orderDetailRepository,
        //        IOrderRepository orderRepository)
        //        : base(orderDetailRepository)
        //    {
        //        _orderDetailRepository = orderDetailRepository;
        //        _orderRepository = orderRepository;
        //    }

        //    public async Task<IEnumerable<OrderDetail>> GetDetailsByOrderAsync(int orderId)
        //    {
        //        try
        //        {
        //            if (orderId <= 0)
        //                throw new ValidationException("ID đơn hàng không hợp lệ");

        //            var order = await _orderRepository.GetByIdAsync(orderId);
        //            if (order == null)
        //                throw new NotFoundException($"Đơn hàng với ID {orderId} không tồn tại");

        //            return await _orderDetailRepository.GetDetailsByOrderAsync(orderId);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new ServiceException($"Lỗi khi lấy chi tiết đơn hàng {orderId}", ex);
        //        }
        //    }

        //    public async Task<bool> DeleteDetailsByOrderAsync(int orderId)
        //    {
        //        try
        //        {
        //            if (orderId <= 0)
        //                throw new ValidationException("ID đơn hàng không hợp lệ");

        //            var order = await _orderRepository.GetByIdAsync(orderId);
        //            if (order == null)
        //                throw new NotFoundException($"Đơn hàng với ID {orderId} không tồn tại");

        //            if (order.Status != "draft")
        //                throw new BusinessRuleException("Chỉ có thể xóa chi tiết đơn hàng ở trạng thái draft");

        //            return await _orderDetailRepository.DeleteDetailsByOrderAsync(orderId);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new ServiceException($"Lỗi khi xóa chi tiết đơn hàng {orderId}", ex);
        //        }
        //    }

        //    public async Task<decimal> GetOrderTotalAsync(int orderId)
        //    {
        //        try
        //        {
        //            if (orderId <= 0)
        //                throw new ValidationException("ID đơn hàng không hợp lệ");

        //            var order = await _orderRepository.GetByIdAsync(orderId);
        //            if (order == null)
        //                throw new NotFoundException($"Đơn hàng với ID {orderId} không tồn tại");

        //            return await _orderDetailRepository.GetOrderTotalAsync(orderId);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new ServiceException($"Lỗi khi tính tổng tiền đơn hàng {orderId}", ex);
        //        }
        //    }

        //    public async Task<IEnumerable<OrderDetail>> GetBestSellingItemsAsync(DateTime startDate, DateTime endDate)
        //    {
        //        try
        //        {
        //            if (startDate > endDate)
        //                throw new ValidationException("Ngày bắt đầu không thể lớn hơn ngày kết thúc");

        //            return await _orderDetailRepository.GetBestSellingItemsAsync(startDate, endDate);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new ServiceException("Lỗi khi lấy sản phẩm bán chạy", ex);
        //        }
        //    }

        //    public async Task<bool> AddDetailsToOrderAsync(int orderId, List<OrderDetail> details)
        //    {
        //        try
        //        {
        //            if (orderId <= 0)
        //                throw new ValidationException("ID đơn hàng không hợp lệ");

        //            if (details == null || !details.Any())
        //                throw new ValidationException("Danh sách chi tiết không được để trống");

        //            var order = await _orderRepository.GetByIdAsync(orderId);
        //            if (order == null)
        //                throw new NotFoundException($"Đơn hàng với ID {orderId} không tồn tại");

        //            if (order.Status != "draft")
        //                throw new BusinessRuleException("Chỉ có thể thêm chi tiết vào đơn hàng ở trạng thái draft");

        //            foreach (var detail in details)
        //            {
        //                detail.OrderId = orderId;
        //                await _orderDetailRepository.CreateAsync(detail);
        //            }

        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new ServiceException($"Lỗi khi thêm chi tiết vào đơn hàng {orderId}", ex);
        //        }
        //    }

        //    public async Task<bool> UpdateOrderDetailQuantityAsync(int orderDetailId, decimal quantity)
        //    {
        //        try
        //        {
        //            if (orderDetailId <= 0)
        //                throw new ValidationException("ID chi tiết đơn hàng không hợp lệ");

        //            if (quantity <= 0)
        //                throw new ValidationException("Số lượng phải lớn hơn 0");

        //            var orderDetail = await _orderDetailRepository.GetByIdAsync(orderDetailId);
        //            if (orderDetail == null)
        //                throw new NotFoundException($"Chi tiết đơn hàng với ID {orderDetailId} không tồn tại");

        //            var order = await _orderRepository.GetByIdAsync(orderDetail.OrderId);
        //            if (order.Status != "draft")
        //                throw new BusinessRuleException("Chỉ có thể cập nhật số lượng ở đơn hàng trạng thái draft");

        //            orderDetail.Quantity = quantity;
        //            await _orderDetailRepository.UpdateAsync(orderDetail);

        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new ServiceException($"Lỗi khi cập nhật số lượng chi tiết đơn hàng {orderDetailId}", ex);
        //        }
        //    }

        //    public async Task<IEnumerable<OrderDetail>> GetOrderDetailsWithProductsAsync(int orderId)
        //    {
        //        try
        //        {
        //            if (orderId <= 0)
        //                throw new ValidationException("ID đơn hàng không hợp lệ");

        //            var order = await _orderRepository.GetByIdAsync(orderId);
        //            if (order == null)
        //                throw new NotFoundException($"Đơn hàng với ID {orderId} không tồn tại");

        //            return await _orderDetailRepository.GetOrderDetailsWithProductsAsync(orderId);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new ServiceException($"Lỗi khi lấy chi tiết đơn hàng với sản phẩm {orderId}", ex);
        //        }
        //    }
    }
}
