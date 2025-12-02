using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcoStationManagerApplication.Core.Services;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.Models.Results;
using EcoStationManagerApplication.Models.DTOs;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class ExportServiceMoqTests
    {
        private Mock<IOrderService> _orderSvc;
        private Mock<ICustomerService> _custSvc;
        private ExportService _svc;

        [TestInitialize]
        public void Setup()
        {
            _orderSvc = new Mock<IOrderService>();
            _custSvc = new Mock<ICustomerService>();
            _svc = new ExportService(_orderSvc.Object, _custSvc.Object);
        }

        [TestMethod]
        public async Task GetOrdersForExport_FilterCompleted_MapsCustomerName()
        {
            var orders = new List<OrderDTO>
            {
                new OrderDTO { OrderId = 1, OrderCode = "O1", CustomerId = 10, Source = OrderSource.MANUAL, Status = OrderStatus.COMPLETED, TotalAmount = 100m, PaymentStatus = PaymentStatus.PAID, LastUpdated = DateTime.Today },
                new OrderDTO { OrderId = 2, OrderCode = "O2", CustomerId = null, Source = OrderSource.EXCEL, Status = OrderStatus.READY, TotalAmount = 50m, PaymentStatus = PaymentStatus.UNPAID, LastUpdated = DateTime.Today }
            };
            _orderSvc.Setup(s => s.GetProcessingOrdersAsync()).Returns(Task.FromResult(Result<IEnumerable<OrderDTO>>.Ok(orders)));
            _custSvc.Setup(c => c.GetCustomerByIdAsync(10)).ReturnsAsync(Result<Customer>.Ok(new Customer { CustomerId = 10, Name = "C1" }));

            var res = await _svc.GetOrdersForExportAsync("completed");
            Assert.AreEqual(1, res.Count);
            Assert.AreEqual("C1", res[0].KhachHang);
            Assert.AreEqual("Hoàn thành", res[0].TrangThai);
        }
    }
}
