using EcoStationManagerApplication.Core.Helpers;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    public class OrderDetailService : BaseService, IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailService(IUnitOfWork unitOfWork)
            : base("OrderDetailService")
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<OrderDetail>> GetOrderDetailByIdAsync(int orderDetailId)
        {
            try
            {
                if (orderDetailId <= 0)
                    return Result<OrderDetail>.Fail("ID chi tiết đơn hàng không hợp lệ");

                var orderDetail = await _unitOfWork.OrderDetails.GetByIdAsync(orderDetailId);
                if (orderDetail == null)
                    return NotFoundError<OrderDetail>("Chi tiết đơn hàng", orderDetailId);

                return Result<OrderDetail>.Ok(orderDetail, "Lấy thông tin chi tiết đơn hàng thành công");
            }
            catch (Exception ex)
            {
                return HandleException<OrderDetail>(ex, "lấy thông tin chi tiết đơn hàng");
            }
        }

        public async Task<Result<IEnumerable<OrderDetail>>> GetOrderDetailsByOrderAsync(int orderId)
        {
            try
            {
                if (orderId <= 0)
                    return Result<IEnumerable<OrderDetail>>.Fail("ID đơn hàng không hợp lệ");

                // Kiểm tra đơn hàng tồn tại
                var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
                if (order == null)
                    return NotFoundError<IEnumerable<OrderDetail>>("Đơn hàng", orderId);

                var orderDetails = await _unitOfWork.OrderDetails.GetByOrderAsync(orderId);
                var message = orderDetails.Any()
                    ? $"Tìm thấy {orderDetails.Count()} chi tiết đơn hàng"
                    : "Đơn hàng không có chi tiết nào";

                return Result<IEnumerable<OrderDetail>>.Ok(orderDetails, message);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<OrderDetail>>(ex, "lấy chi tiết đơn hàng");
            }
        }

        public async Task<Result<int>> AddOrderDetailAsync(OrderDetail orderDetail)
        {
            try
            {
                // Validate dữ liệu
                var validationErrors = ValidationHelper.ValidateOrderDetail(orderDetail);
                if (validationErrors.Any())
                    return ValidationError<int>(validationErrors);

                // Kiểm tra đơn hàng tồn tại
                var order = await _unitOfWork.Orders.GetByIdAsync(orderDetail.OrderId);
                if (order == null)
                    return NotFoundError<int>("Đơn hàng", orderDetail.OrderId);

                // Kiểm tra sản phẩm tồn tại
                var product = await _unitOfWork.Products.GetByIdAsync(orderDetail.ProductId);
                if (product == null)
                    return NotFoundError<int>("Sản phẩm", orderDetail.ProductId);

                // Kiểm tra tồn kho
                var canStockOut = await _unitOfWork.Inventories.IsStockSufficientAsync(
                    orderDetail.ProductId, orderDetail.Quantity);

                if (!canStockOut)
                {
                    var availableStock = await _unitOfWork.Inventories.GetTotalStockQuantityAsync(orderDetail.ProductId);
                    return BusinessError<int>($"Không đủ tồn kho. Tồn kho hiện có: {availableStock}, yêu cầu: {orderDetail.Quantity}");
                }

                // Thêm chi tiết đơn hàng
                var orderDetailId = await _unitOfWork.OrderDetails.AddAsync(orderDetail);
                if (orderDetailId <= 0)
                    return Result<int>.Fail("Thêm chi tiết đơn hàng thất bại");

                // Cập nhật tổng tiền đơn hàng
                await UpdateOrderTotalAsync(orderDetail.OrderId);

                _logger.Info($"Đã thêm chi tiết đơn hàng: Order #{orderDetail.OrderId}, Product #{orderDetail.ProductId}, Quantity: {orderDetail.Quantity}");

                return Result<int>.Ok(orderDetailId, "Thêm chi tiết đơn hàng thành công");
            }
            catch (Exception ex)
            {
                return HandleException<int>(ex, "thêm chi tiết đơn hàng");
            }
        }

        public async Task<Result<bool>> AddOrderDetailsRangeAsync(List<OrderDetail> orderDetails)
        {
            await _unitOfWork.BeginTransactionAsync();
            {
                try
                {
                    if (orderDetails == null || !orderDetails.Any())
                        return Result<bool>.Fail("Danh sách chi tiết đơn hàng không được để trống");

                    // Kiểm tra tất cả orderDetails có cùng OrderId
                    var orderId = orderDetails.First().OrderId;
                    if (orderDetails.Any(od => od.OrderId != orderId))
                        return Result<bool>.Fail("Tất cả chi tiết đơn hàng phải thuộc cùng một đơn hàng");

                    // Kiểm tra đơn hàng tồn tại
                    var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
                    if (order == null)
                        return NotFoundError<bool>("Đơn hàng", orderId);

                    // Validate từng order detail
                    foreach (var orderDetail in orderDetails)
                    {
                        var validationErrors = ValidationHelper.ValidateOrderDetail(orderDetail);
                        if (validationErrors.Any())
                            return ValidationError<bool>(validationErrors);

                        // Kiểm tra sản phẩm tồn tại
                        var product = await _unitOfWork.Products.GetByIdAsync(orderDetail.ProductId);
                        if (product == null)
                            return NotFoundError<bool>("Sản phẩm", orderDetail.ProductId);

                        // Kiểm tra tồn kho
                        var canStockOut = await _unitOfWork.Inventories.IsStockSufficientAsync(
                            orderDetail.ProductId, orderDetail.Quantity);

                        if (!canStockOut)
                        {
                            var availableStock = await _unitOfWork.Inventories.GetTotalStockQuantityAsync(orderDetail.ProductId);
                            return BusinessError<bool>($"Không đủ tồn kho cho sản phẩm {product.Name}. Tồn kho: {availableStock}, yêu cầu: {orderDetail.Quantity}");
                        }
                    }

                    // Thêm tất cả chi tiết đơn hàng
                    var success = await _unitOfWork.OrderDetails.AddRangeAsync(orderDetails);
                    if (!success)
                        throw new Exception("Không thể thêm chi tiết đơn hàng");

                    // Cập nhật tổng tiền đơn hàng
                    await UpdateOrderTotalAsync(orderId);

                    await _unitOfWork.CommitTransactionAsync();
                    _logger.Info($"Đã thêm {orderDetails.Count} chi tiết đơn hàng cho Order #{orderId}");

                    return Result<bool>.Ok(true, $"Đã thêm {orderDetails.Count} chi tiết đơn hàng thành công");
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return HandleException<bool>(ex, "thêm nhiều chi tiết đơn hàng");
                }
            }
        }

        public async Task<Result<bool>> UpdateOrderDetailAsync(OrderDetail orderDetail)
        {
            try
            {
                // Validate dữ liệu
                var validationErrors = ValidationHelper.ValidateOrderDetailForUpdate(orderDetail);
                if (validationErrors.Any())
                    return ValidationError<bool>(validationErrors);

                // Kiểm tra chi tiết đơn hàng tồn tại
                var existingDetail = await _unitOfWork.OrderDetails.GetByIdAsync(orderDetail.OrderDetailId);
                if (existingDetail == null)
                    return NotFoundError<bool>("Chi tiết đơn hàng", orderDetail.OrderDetailId);

                // Kiểm tra sản phẩm tồn tại (nếu có thay đổi)
                if (existingDetail.ProductId != orderDetail.ProductId)
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(orderDetail.ProductId);
                    if (product == null)
                        return NotFoundError<bool>("Sản phẩm", orderDetail.ProductId);
                }

                // Kiểm tra tồn kho (nếu số lượng thay đổi)
                if (existingDetail.Quantity != orderDetail.Quantity)
                {
                    var quantityChange = orderDetail.Quantity - existingDetail.Quantity;
                    var canStockOut = await _unitOfWork.Inventories.IsStockSufficientAsync(
                        orderDetail.ProductId, quantityChange);

                    if (!canStockOut)
                    {
                        var availableStock = await _unitOfWork.Inventories.GetTotalStockQuantityAsync(orderDetail.ProductId);
                        return BusinessError<bool>($"Không đủ tồn kho. Tồn kho hiện có: {availableStock}, yêu cầu thêm: {quantityChange}");
                    }
                }

                // Cập nhật chi tiết đơn hàng
                var success = await _unitOfWork.OrderDetails.UpdateAsync(orderDetail);
                if (!success)
                    return Result<bool>.Fail("Cập nhật chi tiết đơn hàng thất bại");

                // Cập nhật tổng tiền đơn hàng
                await UpdateOrderTotalAsync(orderDetail.OrderId);

                _logger.Info($"Đã cập nhật chi tiết đơn hàng: #{orderDetail.OrderDetailId}");

                return Result<bool>.Ok(true, "Cập nhật chi tiết đơn hàng thành công");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "cập nhật chi tiết đơn hàng");
            }
        }

        public async Task<Result<bool>> UpdateOrderDetailQuantityAsync(int orderDetailId, decimal newQuantity)
        {
            try
            {
                if (orderDetailId <= 0)
                    return Result<bool>.Fail("ID chi tiết đơn hàng không hợp lệ");

                if (newQuantity <= 0)
                    return Result<bool>.Fail("Số lượng phải lớn hơn 0");

                // Kiểm tra chi tiết đơn hàng tồn tại
                var orderDetail = await _unitOfWork.OrderDetails.GetByIdAsync(orderDetailId);
                if (orderDetail == null)
                    return NotFoundError<bool>("Chi tiết đơn hàng", orderDetailId);

                // Kiểm tra tồn kho
                var quantityChange = newQuantity - orderDetail.Quantity;
                var canStockOut = await _unitOfWork.Inventories.IsStockSufficientAsync(
                    orderDetail.ProductId, quantityChange);

                if (!canStockOut)
                {
                    var availableStock = await _unitOfWork.Inventories.GetTotalStockQuantityAsync(orderDetail.ProductId);
                    return BusinessError<bool>($"Không đủ tồn kho. Tồn kho hiện có: {availableStock}, yêu cầu thêm: {quantityChange}");
                }

                // Cập nhật số lượng
                var success = await _unitOfWork.OrderDetails.UpdateQuantityAsync(orderDetailId, newQuantity);
                if (!success)
                    return Result<bool>.Fail("Cập nhật số lượng thất bại");

                // Cập nhật tổng tiền đơn hàng
                await UpdateOrderTotalAsync(orderDetail.OrderId);

                _logger.Info($"Đã cập nhật số lượng chi tiết đơn hàng #{orderDetailId}: {orderDetail.Quantity} -> {newQuantity}");

                return Result<bool>.Ok(true, "Cập nhật số lượng thành công");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "cập nhật số lượng chi tiết đơn hàng");
            }
        }

        public async Task<Result<bool>> UpdateOrderDetailUnitPriceAsync(int orderDetailId, decimal newUnitPrice)
        {
            try
            {
                if (orderDetailId <= 0)
                    return Result<bool>.Fail("ID chi tiết đơn hàng không hợp lệ");

                if (newUnitPrice < 0)
                    return Result<bool>.Fail("Đơn giá không được âm");

                // Kiểm tra chi tiết đơn hàng tồn tại
                var orderDetail = await _unitOfWork.OrderDetails.GetByIdAsync(orderDetailId);
                if (orderDetail == null)
                    return NotFoundError<bool>("Chi tiết đơn hàng", orderDetailId);

                // Cập nhật đơn giá
                var success = await _unitOfWork.OrderDetails.UpdateUnitPriceAsync(orderDetailId, newUnitPrice);
                if (!success)
                    return Result<bool>.Fail("Cập nhật đơn giá thất bại");

                // Cập nhật tổng tiền đơn hàng
                await UpdateOrderTotalAsync(orderDetail.OrderId);

                _logger.Info($"Đã cập nhật đơn giá chi tiết đơn hàng #{orderDetailId}: {orderDetail.UnitPrice} -> {newUnitPrice}");

                return Result<bool>.Ok(true, "Cập nhật đơn giá thành công");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "cập nhật đơn giá chi tiết đơn hàng");
            }
        }

        public async Task<Result<bool>> DeleteOrderDetailAsync(int orderDetailId)
        {
            try
            {
                if (orderDetailId <= 0)
                    return Result<bool>.Fail("ID chi tiết đơn hàng không hợp lệ");

                // Kiểm tra chi tiết đơn hàng tồn tại
                var orderDetail = await _unitOfWork.OrderDetails.GetByIdAsync(orderDetailId);
                if (orderDetail == null)
                    return NotFoundError<bool>("Chi tiết đơn hàng", orderDetailId);

                // Xóa chi tiết đơn hàng
                var success = await _unitOfWork.OrderDetails.DeleteAsync(orderDetailId);
                if (!success)
                    return Result<bool>.Fail("Xóa chi tiết đơn hàng thất bại");

                // Cập nhật tổng tiền đơn hàng
                await UpdateOrderTotalAsync(orderDetail.OrderId);

                _logger.Info($"Đã xóa chi tiết đơn hàng: #{orderDetailId}");

                return Result<bool>.Ok(true, "Xóa chi tiết đơn hàng thành công");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "xóa chi tiết đơn hàng");
            }
        }

        public async Task<Result<bool>> DeleteOrderDetailsByOrderAsync(int orderId)
        {
            try
            {
                if (orderId <= 0)
                    return Result<bool>.Fail("ID đơn hàng không hợp lệ");

                // Kiểm tra đơn hàng tồn tại
                var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
                if (order == null)
                    return NotFoundError<bool>("Đơn hàng", orderId);

                // Xóa tất cả chi tiết đơn hàng
                var success = await _unitOfWork.OrderDetails.DeleteByOrderAsync(orderId);
                if (!success)
                    return Result<bool>.Fail("Xóa chi tiết đơn hàng thất bại");

                // Cập nhật tổng tiền đơn hàng về 0
                order.TotalAmount = 0;
                await _unitOfWork.Orders.UpdateAsync(order);

                _logger.Info($"Đã xóa tất cả chi tiết đơn hàng cho Order #{orderId}");

                return Result<bool>.Ok(true, "Đã xóa tất cả chi tiết đơn hàng");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "xóa chi tiết đơn hàng theo order");
            }
        }

        public async Task<Result<decimal>> CalculateOrderTotalAsync(int orderId)
        {
            try
            {
                if (orderId <= 0)
                    return Result<decimal>.Fail("ID đơn hàng không hợp lệ");

                // Kiểm tra đơn hàng tồn tại
                var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
                if (order == null)
                    return NotFoundError<decimal>("Đơn hàng", orderId);

                // Lấy tất cả chi tiết đơn hàng
                var orderDetails = await _unitOfWork.OrderDetails.GetByOrderAsync(orderId);
                var totalAmount = orderDetails.Sum(od => od.Quantity * od.UnitPrice);

                return Result<decimal>.Ok(totalAmount, $"Tổng tiền đơn hàng: {totalAmount:N0}");
            }
            catch (Exception ex)
            {
                return HandleException<decimal>(ex, "tính tổng tiền đơn hàng");
            }
        }

        public async Task<Result<bool>> IsProductInAnyOrderAsync(int productId)
        {
            try
            {
                if (productId <= 0)
                    return Result<bool>.Fail("ID sản phẩm không hợp lệ");

                var exists = await _unitOfWork.OrderDetails.IsProductInAnyOrderAsync(productId);
                var message = exists ? "Sản phẩm đã có trong đơn hàng" : "Sản phẩm chưa có trong đơn hàng nào";

                return Result<bool>.Ok(exists, message);
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "kiểm tra sản phẩm trong đơn hàng");
            }
        }

        public async Task<Result<decimal>> GetTotalSoldQuantityAsync(int productId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                if (productId <= 0)
                    return Result<decimal>.Fail("ID sản phẩm không hợp lệ");

                // Validate date range
                if (fromDate.HasValue && toDate.HasValue)
                {
                    var dateErrors = ValidationHelper.ValidateDateRange(fromDate.Value, toDate.Value);
                    if (dateErrors.Any())
                        return ValidationError<decimal>(dateErrors);
                }

                var totalQuantity = await _unitOfWork.OrderDetails.GetTotalSoldQuantityAsync(productId, fromDate, toDate);

                var dateRangeText = fromDate.HasValue ? $" từ {fromDate.Value:dd/MM/yyyy} đến {toDate.Value:dd/MM/yyyy}" : "";
                var message = $"Tổng số lượng đã bán{dateRangeText}: {totalQuantity}";

                return Result<decimal>.Ok(totalQuantity, message);
            }
            catch (Exception ex)
            {
                return HandleException<decimal>(ex, "lấy tổng số lượng đã bán");
            }
        }

        public async Task<Result<IEnumerable<ProductSales>>> GetTopSellingProductsAsync(int limit = 10, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                if (limit <= 0 || limit > 100)
                    return Result<IEnumerable<ProductSales>>.Fail("Số lượng sản phẩm phải từ 1 đến 100");

                // Validate date range
                if (fromDate.HasValue && toDate.HasValue)
                {
                    var dateErrors = ValidationHelper.ValidateDateRange(fromDate.Value, toDate.Value);
                    if (dateErrors.Any())
                        return ValidationError<IEnumerable<ProductSales>>(dateErrors);
                }

                var topProducts = await _unitOfWork.OrderDetails.GetTopSellingProductsAsync(limit, fromDate, toDate);

                var message = topProducts.Any()
                    ? $"Tìm thấy {topProducts.Count()} sản phẩm bán chạy nhất"
                    : "Không có dữ liệu sản phẩm bán chạy";

                return Result<IEnumerable<ProductSales>>.Ok(topProducts, message);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<ProductSales>>(ex, "lấy top sản phẩm bán chạy");
            }
        }

        public async Task<Result<IEnumerable<ProductRevenue>>> GetProductRevenueAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                // Validate date range
                if (fromDate.HasValue && toDate.HasValue)
                {
                    var dateErrors = ValidationHelper.ValidateDateRange(fromDate.Value, toDate.Value);
                    if (dateErrors.Any())
                        return ValidationError<IEnumerable<ProductRevenue>>(dateErrors);
                }

                var productRevenue = await _unitOfWork.OrderDetails.GetProductRevenueAsync(fromDate, toDate);

                var message = productRevenue.Any()
                    ? $"Tìm thấy doanh thu của {productRevenue.Count()} sản phẩm"
                    : "Không có dữ liệu doanh thu sản phẩm";

                return Result<IEnumerable<ProductRevenue>>.Ok(productRevenue, message);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<ProductRevenue>>(ex, "lấy doanh thu theo sản phẩm");
            }
        }

        #region Private Methods

        private async Task UpdateOrderTotalAsync(int orderId)
        {
            try
            {
                var totalAmount = await CalculateOrderTotalAsync(orderId);
                if (totalAmount.Success)
                {
                    var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
                    if (order != null)
                    {
                        order.TotalAmount = totalAmount.Data;
                        await _unitOfWork.Orders.UpdateAsync(order);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"UpdateOrderTotalAsync error - OrderId: {orderId} - {ex.Message}");
            }
        }

        #endregion
    }
}
