using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcoStationManagerApplication.Core.Services;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class NotificationServiceMoqTests
    {
        private Mock<IUnitOfWork> _uow;
        private Mock<INotificationRepository> _notiRepo;
        private Mock<IInventoryRepository> _invRepo;
        private Mock<ICleaningScheduleRepository> _csRepo;
        private NotificationService _svc;

        [TestInitialize]
        public void Setup()
        {
            _uow = new Mock<IUnitOfWork>();
            _notiRepo = new Mock<INotificationRepository>();
            _invRepo = new Mock<IInventoryRepository>();
            _csRepo = new Mock<ICleaningScheduleRepository>();
            _uow.Setup(u => u.Notifications).Returns(_notiRepo.Object);
            _uow.Setup(u => u.Inventories).Returns(_invRepo.Object);
            _uow.Setup(u => u.CleaningSchedules).Returns(_csRepo.Object);
            _svc = new NotificationService(_uow.Object);
        }

        [TestMethod]
        public async Task GenerateAutoNotifications_LowStock_AddsNotifications()
        {
            var invs = new List<Inventory> { new Inventory { Quantity = 10m, MinStockLevel = 40m, ProductName = "P" } };
            _invRepo.Setup(r => r.GetLowStockItemsAsync()).ReturnsAsync(invs);
            _invRepo.Setup(r => r.GetExpiringItemsAsync(15)).ReturnsAsync(new List<Inventory>());
            _csRepo.Setup(r => r.GetOverdueSchedulesAsync()).ReturnsAsync(new List<CleaningSchedule>());
            var res = await _svc.GenerateAutoNotificationsAsync();
            Assert.IsTrue(res.Success);
            _notiRepo.Verify(r => r.AddIfNotExistsAsync(It.IsAny<Notification>()), Times.AtLeastOnce);
        }
    }
}
