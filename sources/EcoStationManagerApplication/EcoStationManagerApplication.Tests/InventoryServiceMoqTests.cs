using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcoStationManagerApplication.Core.Services;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Core.Interfaces;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class InventoryServiceMoqTests
    {
        private Mock<IUnitOfWork> _uow;
        private Mock<IInventoryRepository> _repo;
        private Mock<IProductService> _productSvc;
        private InventoryService _svc;

        [TestInitialize]
        public void Setup()
        {
            _uow = new Mock<IUnitOfWork>();
            _repo = new Mock<IInventoryRepository>();
            _productSvc = new Mock<IProductService>();
            _uow.Setup(u => u.Inventories).Returns(_repo.Object);
            _svc = new InventoryService(_uow.Object, _productSvc.Object);
        }

        [TestMethod]
        public async Task AddStock_InvalidInputs_Fail()
        {
            var r1 = await _svc.AddStockAsync(0, "B1", 1, DateTime.Today.AddDays(1));
            Assert.IsFalse(r1.Success);
            var r2 = await _svc.AddStockAsync(1, "", 1, DateTime.Today.AddDays(1));
            Assert.IsFalse(r2.Success);
            var r3 = await _svc.AddStockAsync(1, "B1", 0, DateTime.Today.AddDays(1));
            Assert.IsFalse(r3.Success);
            var r4 = await _svc.AddStockAsync(1, "B1", 1, DateTime.Today.AddDays(-1));
            Assert.IsFalse(r4.Success);
        }

        [TestMethod]
        public async Task AddStock_Valid_Success()
        {
            _productSvc.Setup(p => p.GetProductByIdAsync(1)).ReturnsAsync(Result<Product>.Ok(new Product { ProductId = 1 }));
            _repo.Setup(r => r.AddStockAsync(1, "B1", 5m, It.IsAny<DateTime?>())).ReturnsAsync(true);
            _repo.Setup(r => r.GetByProductAndBatchAsync(1, "B1")).ReturnsAsync(new Inventory { ProductId = 1, BatchNo = "B1" });
            var res = await _svc.AddStockAsync(1, "B1", 5m, DateTime.Today.AddDays(10));
            Assert.IsTrue(res.Success);
            Assert.AreEqual("B1", res.Data.BatchNo);
        }

        [TestMethod]
        public async Task ReduceStock_Insufficient_Fail()
        {
            _repo.Setup(r => r.IsStockSufficientAsync(1, 10m)).ReturnsAsync(false);
            _repo.Setup(r => r.GetTotalStockQuantityAsync(1)).ReturnsAsync(2m);
            var res = await _svc.ReduceStockAsync(1, "B1", 10m);
            Assert.IsFalse(res.Success);
        }

        [TestMethod]
        public async Task IsStockSufficient_Success()
        {
            _repo.Setup(r => r.IsStockSufficientAsync(2, 3m)).ReturnsAsync(true);
            var res = await _svc.IsStockSufficientAsync(2, 3m);
            Assert.IsTrue(res.Success);
            Assert.IsTrue(res.Data);
        }
    }
}

