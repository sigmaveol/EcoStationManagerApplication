using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.Models.Results;
using EcoStationManagerApplication.Models.DTOs;

namespace EcoStationManagerApplication.Tests
{
    // FakeOrderService: tránh gọi DB thật, mô phỏng lưu trữ trong bộ nhớ
    class TestOrderService : IOrderService
    {
        private readonly List<Order> _orders = new List<Order>();
        private int _nextId = 1;

        public Task<Result<int>> CreateOrderAsync(Order order, List<OrderDetail> orderDetails)
        {
            // Validate: yêu cầu tên khách hàng không rỗng (ví dụ theo yêu cầu)
            if (order.Customer == null || string.IsNullOrWhiteSpace(order.Customer.Name))
            {
                throw new ArgumentException("Customer name is required");
            }

            order.OrderId = _nextId++;
            order.OrderDetails = orderDetails ?? new List<OrderDetail>();
            order.TotalAmount = order.OrderDetails.Sum(d => d.Quantity * d.UnitPrice);
            order.Status = OrderStatus.CONFIRMED;
            _orders.Add(order);
            return Task.FromResult(Result<int>.Ok(order.OrderId));
        }

        public Task<Result<bool>> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            var o = _orders.FirstOrDefault(x => x.OrderId == orderId);
            if (o == null) return Task.FromResult(Result<bool>.Fail("NotFound"));
            o.Status = newStatus;
            return Task.FromResult(Result<bool>.Ok(true));
        }

        public Task<Result<bool>> AddOrderDetailsAsync(int orderId, List<OrderDetail> orderDetails)
        {
            var o = _orders.FirstOrDefault(x => x.OrderId == orderId);
            if (o == null) return Task.FromResult(Result<bool>.Fail("NotFound"));
            var current = o.OrderDetails?.ToList() ?? new List<OrderDetail>();
            if (orderDetails != null && orderDetails.Count > 0)
            {
                current.AddRange(orderDetails);
            }
            o.OrderDetails = current;
            o.TotalAmount = current.Sum(d => d.Quantity * d.UnitPrice);
            return Task.FromResult(Result<bool>.Ok(true));
        }

        // Trả về danh sách đơn hàng dạng DTO để đếm số lượng
        public Task<Result<IEnumerable<OrderDTO>>> GetAllAsync()
        {
            var dtos = _orders.Select(o => new OrderDTO
            {
                OrderId = o.OrderId,
                OrderCode = o.OrderCode,
                CustomerId = o.CustomerId,
                CustomerName = o.Customer?.Name,
                Source = o.Source,
                TotalAmount = o.TotalAmount,
                DiscountedAmount = o.DiscountedAmount,
                Status = o.Status,
                PaymentStatus = o.PaymentStatus,
                LastUpdated = o.LastUpdated
            });
            return Task.FromResult(Result<IEnumerable<OrderDTO>>.Ok(dtos));
        }

        // Các phương thức không dùng trong test: trả về dữ liệu tối thiểu
        public Task<Result<Order>> GetOrderByCode(string orderCode)
            => Task.FromResult(Result<Order>.Ok(_orders.FirstOrDefault(o => o.OrderCode == orderCode)));
        public Task<Result<Order>> GetOrderByIdAsync(int orderId)
            => Task.FromResult(Result<Order>.Ok(_orders.FirstOrDefault(o => o.OrderId == orderId)));
        public Task<Result<Order>> GetOrderWithDetailsAsync(int orderId)
            => GetOrderByIdAsync(orderId);
        public Task<Result<IEnumerable<Order>>> GetOrdersByStatusAsync(OrderStatus status)
            => Task.FromResult(Result<IEnumerable<Order>>.Ok(_orders.Where(o => o.Status == status)));
        public Task<Result<IEnumerable<Order>>> GetTodayOrdersAsync()
            => Task.FromResult(Result<IEnumerable<Order>>.Ok(_orders));
        public Task<Result<IEnumerable<Order>>> GetPendingOrdersAsync()
            => Task.FromResult(Result<IEnumerable<Order>>.Ok(_orders.Where(o => o.Status == OrderStatus.PROCESSING)));
        public Task<Result<bool>> UpdatePaymentStatusAsync(int orderId, PaymentStatus newPaymentStatus)
            => Task.FromResult(Result<bool>.Ok(true));
        public Task<Result<OrderSummary>> GetOrderSummaryAsync(DateTime? fromDate = null, DateTime? toDate = null)
            => Task.FromResult(Result<OrderSummary>.Ok(new OrderSummary()));
        public Task<Result<decimal>> GetTotalRevenueAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var total = _orders.Where(o => (!fromDate.HasValue || o.LastUpdated >= fromDate)
                                         && (!toDate.HasValue || o.LastUpdated <= toDate))
                               .Sum(o => o.TotalAmount);
            return Task.FromResult(Result<decimal>.Ok(total));
        }
        public Task<Result<IEnumerable<Order>>> SearchOrdersAsync(string keyword)
            => Task.FromResult(Result<IEnumerable<Order>>.Ok(_orders.Where(o => (o.OrderCode ?? "").Contains(keyword ?? ""))));
        public Task<Result<(IEnumerable<Order> Orders, int TotalCount)>> GetPagedOrdersAsync(int pageNumber, int pageSize, OrderSearchCriteria criteria)
            => Task.FromResult(Result<(IEnumerable<Order> Orders, int TotalCount)>.Ok((_orders.AsEnumerable(), _orders.Count)));
        public Task<Result<bool>> DeleteOrderAsync(int orderId)
        {
            var removed = _orders.RemoveAll(o => o.OrderId == orderId) > 0;
            return Task.FromResult(Result<bool>.Ok(removed));
        }
        public Task<Result<IEnumerable<OrderDTO>>> GetProcessingOrdersAsync()
            => Task.FromResult(Result<IEnumerable<OrderDTO>>.Ok(Enumerable.Empty<OrderDTO>()));
    }

    [TestClass]
    public class OrderServiceAddAndUpdateTests
    {
        [TestMethod]
        public async Task AddOrder_Valid_Increases_Count_By_One()
        {
            // Arrange: khởi tạo service và đơn hàng hợp lệ
            var svc = new TestOrderService();
            var before = (await svc.GetAllAsync()).Data.Count();
            var order = new Order
            {
                OrderCode = "ORD-ADD-001",
                Customer = new Customer { Name = "Khách A" },
                LastUpdated = DateTime.Now,
                Source = OrderSource.MANUAL
            };
            var details = new List<OrderDetail>
            {
                new OrderDetail { ProductId = 1, Quantity = 2, UnitPrice = 10000m }
            };

            // Act: thêm đơn hàng
            var res = await svc.CreateOrderAsync(order, details);

            // Assert: thành công và số lượng tăng 1
            Assert.IsTrue(res.Success);
            var after = (await svc.GetAllAsync()).Data.Count();
            Assert.AreEqual(before + 1, after);
        }

        [TestMethod]
        public async Task AddOrder_With_Empty_CustomerName_Throws()
        {
            // Arrange: đơn hàng thiếu dữ liệu (tên khách hàng rỗng)
            var svc = new TestOrderService();
            var badOrder = new Order
            {
                OrderCode = "ORD-ADD-002",
                Customer = new Customer { Name = "" },
                LastUpdated = DateTime.Now,
                Source = OrderSource.MANUAL
            };

            // Act + Assert: kỳ vọng ném exception
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await svc.CreateOrderAsync(badOrder, new List<OrderDetail>());
            });
        }

        [TestMethod]
        public async Task UpdateStatus_Changes_Order_Status()
        {
            // Arrange: tạo đơn và thêm vào hệ thống
            var svc = new TestOrderService();
            var order = new Order
            {
                OrderCode = "ORD-STATUS-001",
                Customer = new Customer { Name = "Khách B" },
                LastUpdated = DateTime.Now,
                Source = OrderSource.MANUAL
            };
            var create = await svc.CreateOrderAsync(order, new List<OrderDetail>());
            Assert.IsTrue(create.Success);

            // Act: cập nhật trạng thái sang COMPLETED
            var upd = await svc.UpdateOrderStatusAsync(create.Data, OrderStatus.COMPLETED);

            // Assert: trạng thái đã được cập nhật
            Assert.IsTrue(upd.Success);
            var saved = (await svc.GetOrderByIdAsync(create.Data)).Data;
            Assert.AreEqual(OrderStatus.COMPLETED, saved.Status);
        }
    }
}
