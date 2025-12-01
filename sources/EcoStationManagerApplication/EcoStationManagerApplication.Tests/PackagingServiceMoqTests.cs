using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using EcoStationManagerApplication.Core.Services;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class PackagingServiceMoqTests
    {
        private Mock<IUnitOfWork> _uow;
        private Mock<IPackagingRepository> _repo;
        private Mock<IPackagingInventoryRepository> _invRepo;
        private Mock<IPackagingTransactionRepository> _tranRepo;
        private PackagingService _svc;

        [TestInitialize]
        public void Setup()
        {
            _uow = new Mock<IUnitOfWork>();
            _repo = new Mock<IPackagingRepository>();
            _invRepo = new Mock<IPackagingInventoryRepository>();
            _tranRepo = new Mock<IPackagingTransactionRepository>();
            _uow.Setup(u => u.Packaging).Returns(_repo.Object);
            _uow.Setup(u => u.PackagingInventories).Returns(_invRepo.Object);
            _uow.Setup(u => u.PackagingTransactions).Returns(_tranRepo.Object);
            _svc = new PackagingService(_uow.Object);
        }

        [TestMethod]
        public async Task GetByBarcode_Empty_Fail()
        {
            var res = await _svc.GetPackagingByBarcodeAsync("");
            Assert.IsFalse(res.Success);
        }

        [TestMethod]
        public async Task DeletePackaging_InUseOrHasStock_Fail()
        {
            _repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Packaging { PackagingId = 1 });
            _invRepo.Setup(r => r.GetPackagingQuantitiesAsync(1)).ReturnsAsync(new Models.DTOs.PackagingQuantities { QtyNew = 1 });
            var res = await _svc.DeletePackagingAsync(1);
            Assert.IsFalse(res.Success);
        }
    }
}

