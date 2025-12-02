using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcoStationManagerApplication.Core.Services;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class OrderDetailServiceMoqTests
    {
        private Mock<IUnitOfWork> _uow;
        private Mock<IOrderDetailRepository> _repo;
        private Mock<IOrderRepository> _orderRepo;
        private Mock<IInventoryRepository> _invRepo;
        private OrderDetailService _svc;

        [TestInitialize]
        public void Setup()
        {
            _uow = new Mock<IUnitOfWork>();
            _repo = new Mock<IOrderDetailRepository>();
            _orderRepo = new Mock<IOrderRepository>();
            _invRepo = new Mock<IInventoryRepository>();
            _uow.Setup(u => u.OrderDetails).Returns(_repo.Object);
            _uow.Setup(u => u.Orders).Returns(_orderRepo.Object);
            _uow.Setup(u => u.Inventories).Returns(_invRepo.Object);
            _svc = new OrderDetailService(_uow.Object);
        }

        [TestMethod]
        public async Task AddRange_InvalidOrderStatus_Fail()
        {
            _orderRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Order { OrderId = 1, Status = OrderStatus.COMPLETED });
            var res = await _svc.AddOrderDetailsRangeAsync(new List<OrderDetail> { new OrderDetail { OrderId = 1, ProductId = 1, Quantity = 1m } });
            Assert.IsFalse(res.Success);
        }

        [TestMethod]
        public async Task AddRange_InsufficientStock_Fail()
        {
            _orderRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(new Order { OrderId = 2, Status = OrderStatus.CONFIRMED });
            _invRepo.Setup(r => r.IsStockSufficientAsync(1, 5m)).ReturnsAsync(false);
            _invRepo.Setup(r => r.GetTotalStockQuantityAsync(1)).ReturnsAsync(2m);
            var res = await _svc.AddOrderDetailsRangeAsync(new List<OrderDetail> { new OrderDetail { OrderId = 2, ProductId = 1, Quantity = 5m } });
            Assert.IsFalse(res.Success);
        }
    }
}
