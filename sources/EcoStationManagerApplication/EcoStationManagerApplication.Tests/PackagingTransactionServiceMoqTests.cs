using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using EcoStationManagerApplication.Core.Services;
using EcoStationManagerApplication.DAL.Interfaces;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class PackagingTransactionServiceMoqTests
    {
        private Mock<IUnitOfWork> _uow;
        private Mock<IPackagingTransactionRepository> _tranRepo;
        private Mock<IPackagingInventoryRepository> _invRepo;
        private PackagingTransactionService _svc;

        [TestInitialize]
        public void Setup()
        {
            _uow = new Mock<IUnitOfWork>();
            _tranRepo = new Mock<IPackagingTransactionRepository>();
            _invRepo = new Mock<IPackagingInventoryRepository>();
            _uow.Setup(u => u.PackagingTransactions).Returns(_tranRepo.Object);
            _uow.Setup(u => u.PackagingInventories).Returns(_invRepo.Object);
            _svc = new PackagingTransactionService(_uow.Object);
        }

        [TestMethod]
        public async Task IssuePackaging_NotEnough_Fail()
        {
            _invRepo.Setup(r => r.GetByPackagingAsync(1)).ReturnsAsync(new Models.Entities.PackagingInventory { QtyCleaned = 0, QtyNew = 0 });
            var res = await _svc.IssuePackagingAsync(1, 10, 5, 10000m, 1, "");
            Assert.IsFalse(res.Success);
        }
    }
}

