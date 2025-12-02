using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using EcoStationManagerApplication.Core.Services;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Models.Enums;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class PackagingTransactionServiceMoqTests
    {
        private Mock<IUnitOfWork> _uow;
        private Mock<IPackagingTransactionRepository> _tranRepo;
        private Mock<IPackagingInventoryRepository> _invRepo;
        private Mock<IPackagingService> _packSvc;
        private Mock<ICustomerService> _custSvc;
        private Mock<IPackagingInventoryService> _packInvSvc;
        private Mock<IProductService> _prodSvc;
        private PackagingTransactionService _svc;

        [TestInitialize]
        public void Setup()
        {
            _uow = new Mock<IUnitOfWork>();
            _tranRepo = new Mock<IPackagingTransactionRepository>();
            _invRepo = new Mock<IPackagingInventoryRepository>();
            _packSvc = new Mock<IPackagingService>();
            _custSvc = new Mock<ICustomerService>();
            _packInvSvc = new Mock<IPackagingInventoryService>();
            _prodSvc = new Mock<IProductService>();
            _uow.Setup(u => u.PackagingTransactions).Returns(_tranRepo.Object);
            _uow.Setup(u => u.PackagingInventories).Returns(_invRepo.Object);
            _svc = new PackagingTransactionService(_uow.Object, _packSvc.Object, _custSvc.Object, _packInvSvc.Object, _prodSvc.Object);
        }

        [TestMethod]
        public async Task IssuePackaging_NotEnough_Fail()
        {
            _invRepo.Setup(r => r.GetByPackagingAsync(1)).ReturnsAsync(new Models.Entities.PackagingInventory { QtyCleaned = 0, QtyNew = 0 });
            var res = await _svc.IssuePackagingAsync(1, 10, 5, 10000m, 1, PackagingOwnershipType.DEPOSIT, null, "Test");
            Assert.IsFalse(res.Success);
        }
    }
}
