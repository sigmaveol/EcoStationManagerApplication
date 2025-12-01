using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcoStationManagerApplication.Core.Services;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.Models.Results;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class StockOutServiceMoqTests
    {
        private Mock<IUnitOfWork> _uow;
        private Mock<IStockOutRepository> _repo;
        private Mock<IInventoryService> _invSvc;
        private Mock<IPackagingInventoryService> _packInvSvc;
        private StockOutService _svc;

        [TestInitialize]
        public void Setup()
        {
            _uow = new Mock<IUnitOfWork>();
            _repo = new Mock<IStockOutRepository>();
            _invSvc = new Mock<IInventoryService>();
            _packInvSvc = new Mock<IPackagingInventoryService>();
            _uow.Setup(u => u.StockOut).Returns(_repo.Object);
            _svc = new StockOutService(_uow.Object, _invSvc.Object, _packInvSvc.Object);
        }

        [TestMethod]
        public async Task Create_Product_Insufficient_Fails()
        {
            var s = new StockOut { RefType = RefType.PRODUCT, RefId = 1, BatchNo = "B1", Quantity = 10m };
            _invSvc.Setup(svc => svc.IsStockSufficientAsync(1, 10m)).ReturnsAsync(Result<bool>.Ok(false));
            _invSvc.Setup(svc => svc.GetTotalStockQuantityAsync(1)).ReturnsAsync(Result<decimal>.Ok(5m));
            var res = await _svc.CreateStockOutAsync(s);
            Assert.IsFalse(res.Success);
        }

        [TestMethod]
        public async Task Create_Product_Success()
        {
            var s = new StockOut { RefType = RefType.PRODUCT, RefId = 1, BatchNo = "B1", Quantity = 2m };
            _invSvc.Setup(svc => svc.IsStockSufficientAsync(1, 2m)).ReturnsAsync(Result<bool>.Ok(true));
            _uow.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _repo.Setup(r => r.AddAsync(s)).ReturnsAsync(300);
            _invSvc.Setup(svc => svc.ReduceStockAsync(1, "B1", 2m)).ReturnsAsync(Result<bool>.Ok(true));
            _repo.Setup(r => r.GetByIdAsync(300)).ReturnsAsync(new StockOut { StockOutId = 300 });
            _uow.Setup(u => u.CommitTransactionAsync()).Returns(Task.CompletedTask);
            var res = await _svc.CreateStockOutAsync(s);
            Assert.IsTrue(res.Success);
        }

        [TestMethod]
        public async Task Create_Packaging_Success()
        {
            var s = new StockOut { RefType = RefType.PACKAGING, RefId = 2, Quantity = 3m };
            _packInvSvc.Setup(svc => svc.IsNewPackagingSufficientAsync(2, 3)).ReturnsAsync(Result<bool>.Ok(true));
            _uow.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _repo.Setup(r => r.AddAsync(s)).ReturnsAsync(301);
            _packInvSvc.Setup(svc => svc.TransferToInUseAsync(2, 3)).ReturnsAsync(Result<bool>.Ok(true));
            _repo.Setup(r => r.GetByIdAsync(301)).ReturnsAsync(new StockOut { StockOutId = 301 });
            _uow.Setup(u => u.CommitTransactionAsync()).Returns(Task.CompletedTask);
            var res = await _svc.CreateStockOutAsync(s);
            Assert.IsTrue(res.Success);
        }

        [TestMethod]
        public async Task StockOutForOrder_Valid_Ok()
        {
            _repo.Setup(r => r.StockOutForOrderAsync(1, "B1", 2m, 10, 5)).ReturnsAsync(true);
            var res = await _svc.StockOutForOrderAsync(1, "B1", 2m, 10, 5);
            Assert.IsTrue(res.Success);
        }
    }
}
