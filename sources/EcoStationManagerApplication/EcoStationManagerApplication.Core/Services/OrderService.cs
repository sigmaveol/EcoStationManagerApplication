using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Core.Helpers;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
            : base("OrderService")
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<OrderDTO>>> GetAllAsync()
        {
            try
            {
                var ordersResult = await _unitOfWork.Orders.GetAllAsync();
                if (!ordersResult.Any()) 
                {
                    return NotFoundError<IEnumerable<OrderDTO>>("đơn hàng");

                }

                var orderDTOs = new List<OrderDTO>();

                foreach (var order in ordersResult)
                {
                    var orderDTO = MappingHelper.MapToOrderDTO(order);
                    if (orderDTO != null)
                    {
                        orderDTOs.Add(orderDTO);
                    }
                };
                return Result<IEnumerable<OrderDTO>>.Ok(orderDTOs, $"Đã thấy thành công {orderDTOs.Count} đơn hàng");

            }
            catch (Exception ex) 
            {
                return HandleException<IEnumerable<OrderDTO>> (ex, "lấy thông tin đơn hàng");    
            }
        }
        public async Task<Result<IEnumerable<OrderDTO>>> GetProcessingOrdersAsync()
        {
            try
            {
                var ordersResult = await _unitOfWork.Orders.GetAllAsync();
                if (!ordersResult.Any())
                {
                    return NotFoundError<IEnumerable<OrderDTO>>("đơn hàng");
                }

                // Danh sách các trạng thái đang trong quá trình xử lý (bao gồm cả đơn nháp)
                var processingStatuses = new List<OrderStatus>
                {
                    OrderStatus.DRAFT,        // Nháp
                    OrderStatus.CONFIRMED,    // Mới
                    OrderStatus.PROCESSING,   // Đang xử lý
                    OrderStatus.READY,        // Chuẩn bị
                    OrderStatus.SHIPPED       // Đang giao
                };

                var orderDTOs = new List<OrderDTO>();

                foreach (var order in ordersResult)
                {
                    // Chỉ lấy đơn hàng có trạng thái đang xử lý
                    if (processingStatuses.Contains(order.Status))
                    {
                        var orderDTO = MappingHelper.MapToOrderDTO(order);
                        if (orderDTO != null)
                        {
                            orderDTOs.Add(orderDTO);
                        }
                    }
                }

                if (!orderDTOs.Any())
                {
                    return NotFoundError<IEnumerable<OrderDTO>>("đơn hàng đang xử lý");
                }

                return Result<IEnumerable<OrderDTO>>.Ok(orderDTOs, $"Đã thấy thành công {orderDTOs.Count} đơn hàng đang xử lý");

            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<OrderDTO>>(ex, "lấy thông tin đơn hàng đang xử lý");
            }
        }

        public async Task<Result<Order>> GetOrderByCode(string orderCode)
        {
            try
            {
                var order = await _unitOfWork.Orders.GetByOrderCodeAsync(orderCode);
                if (order == null)
                    return NotFoundError<Order>("Đơn hàng");

                return Result<Order>.Ok(order, "Lấy thông tin đơn hàng thành công");
            }
            catch (Exception ex)
            {
                return HandleException<Order>(ex, "lấy thông tin đơn hàng");
            }
        }

        public async Task<Result<Order>> GetOrderByIdAsync(int orderId)
        {
            try
            {
                if (orderId <= 0)
                    return Result<Order>.Fail("ID đơn hàng không hợp lệ");

                var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
                if (order == null)
                    return NotFoundError<Order>("Đơn hàng", orderId);

                return Result<Order>.Ok(order, "Lấy thông tin đơn hàng thành công");
            }
            catch (Exception ex)
            {
                return HandleException<Order>(ex, "lấy thông tin đơn hàng");
            }
        }

        public async Task<Result<Order>> GetOrderWithDetailsAsync(int orderId)
        {
            try
            {
                if (orderId <= 0)
                    return Result<Order>.Fail("ID đơn hàng không hợp lệ");

                var order = await _unitOfWork.Orders.GetOrderWithDetailsAsync(orderId);
                if (order == null)
                    return NotFoundError<Order>("Đơn hàng", orderId);

                return Result<Order>.Ok(order, "Lấy thông tin đơn hàng chi tiết thành công");
            }
            catch (Exception ex)
            {
                return HandleException<Order>(ex, "lấy thông tin đơn hàng chi tiết");
            }
        }

        public async Task<Result<IEnumerable<Order>>> GetOrdersByStatusAsync(OrderStatus status)
        {
            try
            {
                var orders = await _unitOfWork.Orders.GetByStatusAsync(status);
                var message = orders.Any()
                    ? $"Tìm thấy {orders.Count()} đơn hàng với trạng thái {status}"
                    : $"Không có đơn hàng nào với trạng thái {status}";

                return Result<IEnumerable<Order>>.Ok(orders, message);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<Order>>(ex, "lấy đơn hàng theo trạng thái");
            }
        }

        public async Task<Result<IEnumerable<Order>>> GetTodayOrdersAsync()
        {
            try
            {
                var orders = await _unitOfWork.Orders.GetTodayOrdersAsync();
                var message = orders.Any()
                    ? $"Hôm nay có {orders.Count()} đơn hàng"
                    : "Hôm nay chưa có đơn hàng nào";

                return Result<IEnumerable<Order>>.Ok(orders, message);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<Order>>(ex, "lấy đơn hàng hôm nay");
            }
        }

        public async Task<Result<IEnumerable<Order>>> GetPendingOrdersAsync()
        {
            try
            {
                var orders = await _unitOfWork.Orders.GetPendingOrdersAsync();
                var message = orders.Any()
                    ? $"Có {orders.Count()} đơn hàng cần xử lý"
                    : "Không có đơn hàng nào cần xử lý";

                return Result<IEnumerable<Order>>.Ok(orders, message);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<Order>>(ex, "lấy đơn hàng chờ xử lý");
            }
        }

        public async Task<Result<int>> CreateOrderAsync(Order order, List<OrderDetail> orderDetails)
        {
            bool transactionStartedHere = false;
            try
            {
                // Chỉ mở transaction nếu chưa có transaction nào đang chạy
                // (để tránh lỗi khi được gọi từ ImportService với nhiều đơn hàng)
                if (!_unitOfWork.IsTransactionActive())
                {
                    await _unitOfWork.BeginTransactionAsync();
                    transactionStartedHere = true;
                }
                
                // Validate dữ liệu - nhưng bỏ qua check DiscountedAmount > TotalAmount 
                // vì TotalAmount sẽ được tính sau từ orderDetails
                var validationErrors = ValidationHelper.ValidateOrder(order, skipTotalAmountCheck: true);
                if (validationErrors.Any())
                {
                    if (transactionStartedHere)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                    }
                    return ValidationError<int>(validationErrors);
                }

                if (orderDetails == null || !orderDetails.Any())
                    return Result<int>.Fail("Đơn hàng phải có ít nhất 1 sản phẩm");

                // Validate order details
                foreach (var detail in orderDetails)
                {
                    var detailErrors = ValidationHelper.ValidateOrderDetail(detail);
                    if (detailErrors.Any())
                        return ValidationError<int>(detailErrors);
                }

                // Kiểm tra khách hàng tồn tại
                if (order.CustomerId.HasValue && order.CustomerId.Value > 0)
                {
                    var customer = await _unitOfWork.Customers.GetByIdAsync(order.CustomerId.Value);
                    if (customer == null)
                        return Result<int>.Fail("Khách hàng không tồn tại");
                }
                else
                {
                    order.CustomerId = null;
                }                

                // 1. Tạo order
                var orderId = await _unitOfWork.Orders.AddAsync(order);
                if (orderId <= 0)
                    throw new Exception("Không thể tạo đơn hàng");

                // 2. Thêm order details
                foreach (var detail in orderDetails)
                {
                    detail.OrderId = orderId;
                }
                var detailsSuccess = await _unitOfWork.OrderDetails.AddRangeAsync(orderDetails);
                if (!detailsSuccess)
                    throw new Exception("Không thể thêm chi tiết đơn hàng");

                // 3. Tính toán tổng tiền
                var discount = order.DiscountedAmount < 0 ? 0 : order.DiscountedAmount;

                var totalAmount = orderDetails.Sum(d => d.Quantity * d.UnitPrice) - discount;

                if (totalAmount < 0)
                    totalAmount = 0;

                order.TotalAmount = totalAmount;
                order.OrderId = orderId;


                var updateSuccess = await _unitOfWork.Orders.UpdateAsync(order);
                if (!updateSuccess)
                    throw new Exception("Không thể cập nhật tổng tiền đơn hàng");

                // Chỉ commit nếu transaction được mở trong method này
                if (transactionStartedHere)
                {
                    await _unitOfWork.CommitTransactionAsync();
                }
                _logger.Info($"Đã tạo đơn hàng mới: #{orderId} - Tổng tiền: {totalAmount:N0}");

                return Result<int>.Ok(orderId, $"Tạo đơn hàng #{orderId} thành công");
            }
            catch (Exception ex)
            {
                // Chỉ rollback nếu transaction được mở trong method này
                if (transactionStartedHere)
                {
                    try
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                    }
                    catch
                    {
                        // Ignore rollback errors
                    }
                }
                return HandleException<int>(ex, "tạo đơn hàng");
            }
        }

        public async Task<Result<bool>> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            try
            {
                if (orderId <= 0)
                    return Result<bool>.Fail("ID đơn hàng không hợp lệ");

                // Kiểm tra đơn hàng tồn tại
                var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
                if (order == null)
                    return NotFoundError<bool>("Đơn hàng", orderId);

                
                if (order.Status == OrderStatus.COMPLETED || order.Status == OrderStatus.COMPLETED)
                {
                    return Result<bool>.Fail($"Đơn hàng đã xử lí xong. Không được cập nhật trạng thái.");
                }

                if (newStatus == OrderStatus.CANCELLED && order.Status != OrderStatus.CANCELLED)
                {
                    // Hoàn trả tồn kho nếu hủy đơn
                    var rollbackResult = await RollbackStockForOrderAsync(orderId);
                    if (!rollbackResult.Success)
                        return Result<bool>.Fail($"Lỗi hoàn trả tồn kho: {rollbackResult.Message}");
                }

                if (newStatus == OrderStatus.COMPLETED)
                {
                    await _unitOfWork.Orders.UpdatePaymentStatusAsync(orderId, PaymentStatus.PAID);
                }

                // Cập nhật trạng thái
                var success = await _unitOfWork.Orders.UpdateOrderStatusAsync(orderId, newStatus);
                if (success)
                {
                    _logger.Info($"Đã cập nhật trạng thái đơn hàng #{orderId}: {order.Status} -> {newStatus}");
                    return Result<bool>.Ok(true, $"Đã cập nhật trạng thái đơn hàng thành {newStatus}");
                }

                return Result<bool>.Fail("Cập nhật trạng thái đơn hàng thất bại");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "cập nhật trạng thái đơn hàng");
            }
        }

        public async Task<Result<bool>> UpdatePaymentStatusAsync(int orderId, PaymentStatus newPaymentStatus)
        {
            try
            {
                if (orderId <= 0)
                    return Result<bool>.Fail("ID đơn hàng không hợp lệ");

                // Kiểm tra đơn hàng tồn tại
                var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
                if (order == null)
                    return NotFoundError<bool>("Đơn hàng", orderId);

                var success = await _unitOfWork.Orders.UpdatePaymentStatusAsync(orderId, newPaymentStatus);
                if (success)
                {
                    _logger.Info($"Đã cập nhật trạng thái thanh toán đơn hàng #{orderId}: {newPaymentStatus}");
                    return Result<bool>.Ok(true, $"Đã cập nhật trạng thái thanh toán thành {newPaymentStatus}");
                }

                return Result<bool>.Fail("Cập nhật trạng thái thanh toán thất bại");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "cập nhật trạng thái thanh toán");
            }
        }

        public async Task<Result<bool>> AddOrderDetailsAsync(int orderId, List<OrderDetail> orderDetails)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (orderId <= 0)
                    return Result<bool>.Fail("ID đơn hàng không hợp lệ");

                if (orderDetails == null || !orderDetails.Any())
                    return Result<bool>.Fail("Danh sách chi tiết đơn hàng không được để trống");

                // Kiểm tra đơn hàng tồn tại và có thể chỉnh sửa
                var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
                if (order == null)
                    return NotFoundError<bool>("Đơn hàng", orderId);

                if (order.Status != OrderStatus.DRAFT && order.Status != OrderStatus.CONFIRMED)
                    return BusinessError<bool>("Chỉ có thể thêm sản phẩm vào đơn hàng ở trạng thái DRAFT hoặc CONFIRMED");

                // Validate và thêm order details
                foreach (var detail in orderDetails)
                {
                    var detailErrors = ValidationHelper.ValidateOrderDetail(detail);
                    if (detailErrors.Any())
                        return ValidationError<bool>(detailErrors);

                    detail.OrderId = orderId;

                    // Kiểm tra tồn kho
                    var canStockOut = await _unitOfWork.Inventories.IsStockSufficientAsync(
                        detail.ProductId, detail.Quantity);

                    if (!canStockOut)
                    {
                        var product = await _unitOfWork.Products.GetByIdAsync(detail.ProductId);
                        var productName = product?.Name ?? $"ID {detail.ProductId}";
                        return BusinessError<bool>($"Không đủ tồn kho cho sản phẩm: {productName}");
                    }
                }

                var success = await _unitOfWork.OrderDetails.AddRangeAsync(orderDetails);
                if (!success)
                    throw new Exception("Không thể thêm chi tiết đơn hàng");

                // Cập nhật tổng tiền đơn hàng
                var allDetails = await _unitOfWork.OrderDetails.GetByOrderAsync(orderId);
                var newTotalAmount = allDetails.Sum(d => d.Quantity * d.UnitPrice);

                var discount = order.DiscountedAmount;

                // Không cho tổng tiền âm
                order.TotalAmount = Math.Max(0, newTotalAmount - discount);
                var updateSuccess = await _unitOfWork.Orders.UpdateAsync(order);
                if (!updateSuccess)
                    throw new Exception("Không thể cập nhật tổng tiền đơn hàng");

                await _unitOfWork.CommitTransactionAsync();
                _logger.Info($"Đã thêm {orderDetails.Count} sản phẩm vào đơn hàng #{orderId}");

                return Result<bool>.Ok(true, $"Đã thêm {orderDetails.Count} sản phẩm vào đơn hàng");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return HandleException<bool>(ex, "thêm chi tiết đơn hàng");
            }
        }

        public async Task<Result<decimal>> GetTotalRevenueAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var totalRevenue = await _unitOfWork.Orders.GetTotalRevenueAsync(fromDate, toDate);
                return Result<decimal>.Ok(totalRevenue, "Lấy tổng doanh thu thành công");
            }
            catch (Exception ex)
            {
                return HandleException<decimal>(ex, "lấy thống kê đơn hàng");
            }
        }

        public async Task<Result<OrderSummary>> GetOrderSummaryAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var totalRevenue = await _unitOfWork.Orders.GetTotalRevenueAsync(fromDate, toDate);
                var orderCount = await _unitOfWork.Orders.GetOrderCountAsync(fromDate, toDate);
                var pendingOrders = await _unitOfWork.Orders.GetPendingOrdersAsync();
                var todayOrders = await _unitOfWork.Orders.GetTodayOrdersAsync();

                var summary = new OrderSummary
                {
                    TotalRevenue = totalRevenue,
                    TotalOrders = orderCount,
                    PendingOrderCount = pendingOrders.Count(),
                    TodayOrderCount = todayOrders.Count(),
                    AverageOrderValue = orderCount > 0 ? totalRevenue / orderCount : 0
                };

                return Result<OrderSummary>.Ok(summary, "Lấy thống kê đơn hàng thành công");
            }
            catch (Exception ex)
            {
                return HandleException<OrderSummary>(ex, "lấy thống kê đơn hàng");
            }
        }

        public async Task<Result<IEnumerable<Order>>> SearchOrdersAsync(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return Result<IEnumerable<Order>>.Fail("Từ khóa tìm kiếm không được để trống");

                var orders = await _unitOfWork.Orders.SearchOrdersAsync(keyword);
                var message = orders.Any()
                    ? $"Tìm thấy {orders.Count()} đơn hàng"
                    : "Không tìm thấy đơn hàng nào";

                return Result<IEnumerable<Order>>.Ok(orders, message);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<Order>>(ex, "tìm kiếm đơn hàng");
            }
        }

        public async Task<Result<(IEnumerable<Order> Orders, int TotalCount)>> GetPagedOrdersAsync(
            int pageNumber, int pageSize, OrderSearchCriteria criteria)
        {
            try
            {
                if (pageNumber <= 0 || pageSize <= 0)
                    return Result<(IEnumerable<Order> Orders, int TotalCount)>.Fail("Số trang và kích thước trang phải lớn hơn 0");

                var result = await _unitOfWork.Orders.GetPagedOrdersAsync(
                    pageNumber,
                    pageSize,
                    criteria.SearchKeyword,
                    criteria.Status,
                    criteria.PaymentStatus,
                    criteria.ProductType,
                    criteria.Source,
                    criteria.MinTotal,
                    criteria.CustomerId,
                    criteria.UserId,
                    criteria.FromDate,
                    criteria.ToDate
                );

                var message = result.Orders.Any()
                    ? $"Tìm thấy {result.TotalCount} đơn hàng (trang {pageNumber})"
                    : "Không tìm thấy đơn hàng nào";

                return Result<(IEnumerable<Order> Orders, int TotalCount)>.Ok(result, message);
            }
            catch (Exception ex)
            {
                return HandleException<(IEnumerable<Order> Orders, int TotalCount)>(ex, "lấy danh sách đơn hàng phân trang");
            }
        }

        #region Private Methods

        /// <summary>
        ///  đang thiếu UserId, sau sẽ thêm logic
        /// </summary>
        /// <param name="orderId"> ID đơn hàng </param>
        /// <returns></returns>
        private async Task<Result<bool>> ProcessStockOutForOrderAsync(int orderId)
        {
            try
            {
                var orderDetails = await _unitOfWork.OrderDetails.GetByOrderAsync(orderId);
                if (!orderDetails.Any())
                    return Result<bool>.Fail("Đơn hàng không có chi tiết");

                foreach (var detail in orderDetails)
                {

                    // Giả sử sử dụng batch mặc định, trong thực tế cần logic phức tạp hơn
                    var stockOutSuccess = await _unitOfWork.StockOut.StockOutForOrderAsync(
                        detail.ProductId, "DEFAULT", detail.Quantity, orderId, 1); // UserId = 1 tạm thời

                    if (!stockOutSuccess)
                    {
                        _logger.Error($"Không thể xuất kho cho sản phẩm {detail.ProductId} trong đơn hàng #{orderId}");
                        return Result<bool>.Fail($"Không thể xuất kho cho sản phẩm ID {detail.ProductId}");
                    }
                }

                return Result<bool>.Ok(true, "Đã xử lý xuất kho cho đơn hàng");
            }
            catch (Exception ex)
            {
                _logger.Error($"ProcessStockOutForOrderAsync error - OrderId: {orderId} - {ex.Message}");
                return Result<bool>.Fail($"Lỗi xử lý xuất kho: {ex.Message}");
            }
        }

        private async Task<Result<bool>> RollbackStockForOrderAsync(int orderId)
        {
            try
            {
                // Logic hoàn trả tồn kho khi hủy đơn hàng
                // Trong thực tế cần tracking số lượng đã xuất để hoàn trả chính xác
                _logger.Info($"Đã hoàn trả tồn kho cho đơn hàng hủy: #{orderId}");
                return Result<bool>.Ok(true, "Đã hoàn trả tồn kho");
            }
            catch (Exception ex)
            {
                _logger.Error($"RollbackStockForOrderAsync error - OrderId: {orderId} - {ex.Message}");
                return Result<bool>.Fail($"Lỗi hoàn trả tồn kho: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteOrderAsync(int orderId)
        {
            try
            {
                if (orderId <= 0)
                    return Result<bool>.Fail("ID đơn hàng không hợp lệ");

                // Kiểm tra đơn hàng tồn tại
                var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
                if (order == null)
                    return NotFoundError<bool>("Đơn hàng", orderId);

                var cannotDeleteStatuses = new[]
                {
                    OrderStatus.PROCESSING,
                    OrderStatus.READY,
                    OrderStatus.SHIPPED,
                    OrderStatus.COMPLETED
                };
                // Kiểm tra trạng thái đơn hàng - chỉ cho phép xóa đơn nháp hoặc đã hủy
                // Có thể mở rộng logic này nếu cần
                if (cannotDeleteStatuses.Contains(order.Status))
                {
                    return Result<bool>.Fail("Không thể xóa đơn hàng đang trong quá trình xử lý hoặc đã hoàn thành");
                }

                bool transactionStarted = false;
                try
                {
                    // Chỉ mở transaction nếu chưa có transaction nào đang chạy
                    try
                    {
                        await _unitOfWork.BeginTransactionAsync();
                        transactionStarted = true;
                    }
                    catch (InvalidOperationException ex) when (ex.Message.Contains("already started"))
                    {
                        // Transaction đã được mở từ bên ngoài, không cần mở lại
                        transactionStarted = false;
                    }
                    // 1. Xóa tất cả OrderDetails trước
                    var deleteDetailsResult = await _unitOfWork.OrderDetails.DeleteByOrderAsync(orderId);
                    if (!deleteDetailsResult)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return Result<bool>.Fail("Không thể xóa chi tiết đơn hàng");
                    }

                    // 2. Xóa Order
                    var deleteOrderResult = await _unitOfWork.Orders.DeleteAsync(orderId);
                    if (!deleteOrderResult)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return Result<bool>.Fail("Không thể xóa đơn hàng");
                    }

                    // Chỉ commit nếu transaction được mở trong method này
                    if (transactionStarted)
                    {
                        await _unitOfWork.CommitTransactionAsync();
                    }
                    _logger.Info($"Đã xóa đơn hàng: #{orderId}");

                    return Result<bool>.Ok(true, $"Đã xóa đơn hàng #{orderId} thành công");
                }
                catch (Exception ex)
                {
                    // Chỉ rollback nếu transaction được mở trong method này
                    if (transactionStarted)
                    {
                        try
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                        }
                        catch
                        {
                            // Ignore rollback errors
                        }
                    }
                    throw;
                }
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "xóa đơn hàng");
            }
        }

        private bool CanUpdateOrderStatus(OrderStatus currentStatus, OrderStatus newStatus)
        {
            // Define allowed status transitions
            var allowedTransitions = new Dictionary<OrderStatus, List<OrderStatus>>
            {
                { OrderStatus.DRAFT, new List<OrderStatus> { OrderStatus.CONFIRMED, OrderStatus.CANCELLED } },
                { OrderStatus.CONFIRMED, new List<OrderStatus> { OrderStatus.PROCESSING, OrderStatus.CANCELLED } },
                { OrderStatus.PROCESSING, new List<OrderStatus> { OrderStatus.READY, OrderStatus.CANCELLED } },
                { OrderStatus.READY, new List<OrderStatus> { OrderStatus.SHIPPED, OrderStatus.CANCELLED } },
                { OrderStatus.SHIPPED, new List<OrderStatus> { OrderStatus.COMPLETED } },
                { OrderStatus.COMPLETED, new List<OrderStatus> { } },
                { OrderStatus.CANCELLED, new List<OrderStatus> { } }
            };

            return allowedTransitions.ContainsKey(currentStatus) &&
                   allowedTransitions[currentStatus].Contains(newStatus);
        }

        #endregion
    }
}
