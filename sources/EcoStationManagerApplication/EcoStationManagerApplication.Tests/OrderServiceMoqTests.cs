using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcoStationManagerApplication.Core.Services;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class OrderServiceMoqTests
    {
        private Mock<IUnitOfWork> _uow;
        private Mock<IOrderRepository> _orderRepo;
        private Mock<IOrderDetailRepository> _detailRepo;
        private Mock<ICustomerRepository> _customerRepo;
        private OrderService _svc;

        [TestInitialize]
        public void Setup()
        {
            _uow = new Mock<IUnitOfWork>();
            _orderRepo = new Mock<IOrderRepository>();
            _detailRepo = new Mock<IOrderDetailRepository>();
            _customerRepo = new Mock<ICustomerRepository>();
            _uow.Setup(u => u.Orders).Returns(_orderRepo.Object);
            _uow.Setup(u => u.OrderDetails).Returns(_detailRepo.Object);
            _uow.Setup(u => u.Customers).Returns(_customerRepo.Object);
            _svc = new OrderService(_uow.Object);
        }

        [TestMethod]
        public async Task CreateOrder_Computes_Total_Success()
        {
            var order = new Order { CustomerId = 1, DiscountedAmount = 10m };
            var details = new List<OrderDetail>
            {
                new OrderDetail { ProductId = 1, Quantity = 2m, UnitPrice = 50m },
                new OrderDetail { ProductId = 2, Quantity = 1m, UnitPrice = 100m }
            };

            _customerRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Customer { CustomerId = 1 });
            _orderRepo.Setup(r => r.AddAsync(order)).ReturnsAsync(1000);
            _detailRepo.Setup(r => r.AddRangeAsync(It.IsAny<List<OrderDetail>>())).ReturnsAsync(true);
            _orderRepo.Setup(r => r.UpdateAsync(It.Is<Order>(o => o.TotalAmount == 190m))).ReturnsAsync(true);
            _uow.Setup(u => u.IsTransactionActive()).Returns(false);
            _uow.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _uow.Setup(u => u.CommitTransactionAsync()).Returns(Task.CompletedTask);

            var res = await _svc.CreateOrderAsync(order, details);
            Assert.IsTrue(res.Success);
            Assert.AreEqual(1000, res.Data);
        }

        [TestMethod]
        public async Task UpdateOrderStatus_NotFound_Fails()
        {
            _orderRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Order)null);
            var res = await _svc.UpdateOrderStatusAsync(1, OrderStatus.CONFIRMED);
            Assert.IsFalse(res.Success);
        }

        [TestMethod]
        public async Task AddOrderDetails_InvalidStatus_Fails()
        {
            _orderRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Order { OrderId = 1, Status = OrderStatus.SHIPPED });
            var res = await _svc.AddOrderDetailsAsync(1, new List<OrderDetail> { new OrderDetail { ProductId = 1, Quantity = 1m, UnitPrice = 10m } });
            Assert.IsFalse(res.Success);
        }
    }
}
