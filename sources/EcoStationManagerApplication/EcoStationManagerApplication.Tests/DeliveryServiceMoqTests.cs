using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using EcoStationManagerApplication.Core.Services;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class DeliveryServiceMoqTests
    {
        private Mock<IUnitOfWork> _uow;
        private Mock<IDeliveryRepository> _repo;
        private DeliveryService _svc;

        [TestInitialize]
        public void Setup()
        {
            _uow = new Mock<IUnitOfWork>();
            _repo = new Mock<IDeliveryRepository>();
            _uow.Setup(u => u.Deliveries).Returns(_repo.Object);
            _svc = new DeliveryService(_uow.Object);
        }

        [TestMethod]
        public async Task Create_Invalid_Fails()
        {
            var res = await _svc.CreateAsync(new DeliveryAssignment { OrderId = 0, DriverId = 0 });
            Assert.IsFalse(res.Success);
        }

        [TestMethod]
        public async Task UpdateStatus_Ok()
        {
            _repo.Setup(r => r.UpdateStatusAsync(1, DeliveryStatus.INTRANSIT)).ReturnsAsync(true);
            var res = await _svc.UpdateStatusAsync(1, DeliveryStatus.INTRANSIT);
            Assert.IsTrue(res.Success);
        }

        [TestMethod]
        public async Task GetTotalCOD_Ok()
        {
            _repo.Setup(r => r.GetTotalCODByDriverAsync(1, null, null)).ReturnsAsync(100m);
            var res = await _svc.GetTotalCODByDriverAsync(1, null, null);
            Assert.IsTrue(res.Success);
            Assert.AreEqual(100m, res.Data);
        }
    }
}
