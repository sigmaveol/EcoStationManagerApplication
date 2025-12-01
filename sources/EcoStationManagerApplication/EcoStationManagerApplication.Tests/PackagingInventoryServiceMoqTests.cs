using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using EcoStationManagerApplication.Core.Services;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class PackagingInventoryServiceMoqTests
    {
        private Mock<IUnitOfWork> _uow;
        private Mock<IPackagingInventoryRepository> _repo;
        private Mock<IPackagingService> _packSvc;
        private PackagingInventoryService _svc;

        [TestInitialize]
        public void Setup()
        {
            _uow = new Mock<IUnitOfWork>();
            _repo = new Mock<IPackagingInventoryRepository>();
            _packSvc = new Mock<IPackagingService>();
            _uow.Setup(u => u.PackagingInventories).Returns(_repo.Object);
            _svc = new PackagingInventoryService(_uow.Object, _packSvc.Object);
        }

        [TestMethod]
        public async Task UpdateQuantities_Invalid_Fails()
        {
            var res = await _svc.UpdatePackagingQuantitiesAsync(0, new PackagingQuantities());
            Assert.IsFalse(res.Success);
        }

        [TestMethod]
        public async Task UpdateQuantities_Valid_Ok()
        {
            _packSvc.Setup(s => s.GetPackagingByIdAsync(1)).ReturnsAsync(EcoStationManagerApplication.Models.Results.Result<Packaging>.Ok(new Packaging { PackagingId = 1 }));
            _repo.Setup(r => r.UpdateQuantitiesAsync(1, It.IsAny<PackagingQuantities>())).ReturnsAsync(true);
            var res = await _svc.UpdatePackagingQuantitiesAsync(1, new PackagingQuantities { QtyNew = 1 });
            Assert.IsTrue(res.Success);
        }

        [TestMethod]
        public async Task TransferToInUse_Sufficient_Ok()
        {
            _repo.Setup(r => r.IsNewPackagingSufficientAsync(1, 2)).ReturnsAsync(true);
            _repo.Setup(r => r.TransferToInUseAsync(1, 2)).ReturnsAsync(true);
            var res = await _svc.TransferToInUseAsync(1, 2);
            Assert.IsTrue(res.Success);
        }

        [TestMethod]
        public async Task MarkAsDamaged_NotEnough_Fails()
        {
            _repo.Setup(r => r.GetByPackagingAsync(1)).ReturnsAsync(new PackagingInventory { QtyNew = 0, QtyInUse = 0, QtyReturned = 0, QtyNeedCleaning = 0, QtyCleaned = 0 });
            var res = await _svc.MarkAsDamagedAsync(1, 1);
            Assert.IsFalse(res.Success);
        }
    }
}
