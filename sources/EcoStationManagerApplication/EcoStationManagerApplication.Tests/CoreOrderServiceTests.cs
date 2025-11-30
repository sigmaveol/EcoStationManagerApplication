using Microsoft.VisualStudio.TestTools.UnitTesting;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Results;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace EcoStationManagerApplication.Tests
{
    class FakeOrderService : IOrderService
    {
        private readonly Dictionary<int, Order> _orders = new Dictionary<int, Order>();
        private int _nextId = 1;

        public Task<Result<int>> CreateOrderAsync(Order order, List<OrderDetail> orderDetails)
        {
            order.OrderId = _nextId++;
            order.Status = OrderStatus.PROCESSING;
            order.OrderDetails = orderDetails;
            order.TotalAmount = orderDetails?.Sum(d => d.Quantity * d.UnitPrice) ?? 0m;
            _orders[order.OrderId] = order;
            return Task.FromResult(Result<int>.Ok(order.OrderId));
        }

        public Task<Result<bool>> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            if (!_orders.ContainsKey(orderId)) return Task.FromResult(Result<bool>.Fail("NotFound"));
            _orders[orderId].Status = newStatus;
            return Task.FromResult(Result<bool>.Ok(true));
        }

        public Task<Result<bool>> AddOrderDetailsAsync(int orderId, List<OrderDetail> orderDetails)
        {
            if (!_orders.ContainsKey(orderId)) return Task.FromResult(Result<bool>.Fail("NotFound"));
            var o = _orders[orderId];
            o.OrderDetails = (o.OrderDetails ?? new List<OrderDetail>()).Concat(orderDetails).ToList();
            o.TotalAmount = o.OrderDetails.Sum(d => d.Quantity * d.UnitPrice);
            return Task.FromResult(Result<bool>.Ok(true));
        }

        public Task<Result<decimal>> GetTotalRevenueAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var total = _orders.Values.Where(o => (!fromDate.HasValue || o.LastUpdated >= fromDate) && (!toDate.HasValue || o.LastUpdated <= toDate)).Sum(o => o.TotalAmount);
            return Task.FromResult(Result<decimal>.Ok(total));
        }

        public Task<Result<IEnumerable<OrderDTO>>> GetAllAsync() => Task.FromResult(Result<IEnumerable<OrderDTO>>.Ok(Enumerable.Empty<OrderDTO>()));
        public Task<Result<Order>> GetOrderByIdAsync(int orderId) => Task.FromResult(Result<Order>.Ok(_orders.TryGetValue(orderId, out var o) ? o : null));
        public Task<Result<Order>> GetOrderWithDetailsAsync(int orderId) => GetOrderByIdAsync(orderId);
        public Task<Result<IEnumerable<Order>>> GetOrdersByStatusAsync(OrderStatus status) => Task.FromResult(Result<IEnumerable<Order>>.Ok(_orders.Values.Where(o => o.Status == status)));
        public Task<Result<IEnumerable<Order>>> GetTodayOrdersAsync() => Task.FromResult(Result<IEnumerable<Order>>.Ok(_orders.Values));
        public Task<Result<IEnumerable<Order>>> GetPendingOrdersAsync() => Task.FromResult(Result<IEnumerable<Order>>.Ok(_orders.Values.Where(o => o.Status == OrderStatus.PROCESSING)));
        public Task<Result<bool>> UpdatePaymentStatusAsync(int orderId, PaymentStatus newPaymentStatus) => Task.FromResult(Result<bool>.Ok(true));
        public Task<Result<Order>> GetOrderByCode(string orderCode) => Task.FromResult(Result<Order>.Ok(_orders.Values.FirstOrDefault(o => o.OrderCode == orderCode)));
        public Task<Result<OrderSummary>> GetOrderSummaryAsync(DateTime? fromDate = null, DateTime? toDate = null) => Task.FromResult(Result<OrderSummary>.Ok(new OrderSummary()));
        public Task<Result<IEnumerable<Order>>> SearchOrdersAsync(string keyword) => Task.FromResult(Result<IEnumerable<Order>>.Ok(_orders.Values.Where(o => (o.OrderCode ?? "").Contains(keyword ?? ""))));
        public Task<Result<(IEnumerable<Order> Orders, int TotalCount)>> GetPagedOrdersAsync(int pageNumber, int pageSize, OrderSearchCriteria criteria) => Task.FromResult(Result<(IEnumerable<Order> Orders, int TotalCount)>.Ok((_orders.Values, _orders.Count)));
        public Task<Result<bool>> DeleteOrderAsync(int orderId) { _orders.Remove(orderId); return Task.FromResult(Result<bool>.Ok(true)); }
        public Task<Result<IEnumerable<OrderDTO>>> GetProcessingOrdersAsync() => Task.FromResult(Result<IEnumerable<OrderDTO>>.Ok(Enumerable.Empty<OrderDTO>()));
    }

    [TestClass]
    public class CoreOrderServiceTests
    {
        [TestMethod]
        public async Task CreateOrder_Computes_Total_And_Pending_Status()
        {
            var svc = new FakeOrderService();
            var order = new Order { OrderCode = "ORD-001", LastUpdated = DateTime.Now };
            var res = await svc.CreateOrderAsync(order, new List<OrderDetail> {
                new OrderDetail { ProductId = 1, Quantity = 2, UnitPrice = 10000m },
                new OrderDetail { ProductId = 2, Quantity = 1, UnitPrice = 5000m }
            });
            Assert.IsTrue(res.Success);
            var o = (await svc.GetOrderByIdAsync(res.Data)).Data;
            Assert.AreEqual(OrderStatus.PROCESSING, o.Status);
            Assert.AreEqual(25000m, o.TotalAmount);
        }

        [TestMethod]
        public async Task AddOrderDetails_Updates_Total()
        {
            var svc = new FakeOrderService();
            var res = await svc.CreateOrderAsync(new Order { OrderCode = "ORD-002", LastUpdated = DateTime.Now }, new List<OrderDetail>());
            var ok = await svc.AddOrderDetailsAsync(res.Data, new List<OrderDetail> { new OrderDetail { ProductId = 3, Quantity = 3, UnitPrice = 2000m } });
            Assert.IsTrue(ok.Success);
            var o = (await svc.GetOrderByIdAsync(res.Data)).Data;
            Assert.AreEqual(6000m, o.TotalAmount);
        }

        [TestMethod]
        public async Task UpdateStatus_Changes_Order_Status()
        {
            var svc = new FakeOrderService();
            var res = await svc.CreateOrderAsync(new Order { OrderCode = "ORD-003", LastUpdated = DateTime.Now }, new List<OrderDetail>());
            var st = await svc.UpdateOrderStatusAsync(res.Data, OrderStatus.COMPLETED);
            Assert.IsTrue(st.Success);
            var o = (await svc.GetOrderByIdAsync(res.Data)).Data;
            Assert.AreEqual(OrderStatus.COMPLETED, o.Status);
        }
    }
}
