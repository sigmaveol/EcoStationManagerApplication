using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcoStationManagerApplication.Core.Services;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.Models.Results;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class StockInServiceMoqTests
    {
        private Mock<IUnitOfWork> _uow;
        private Mock<IStockInRepository> _repo;
        private Mock<IInventoryService> _invSvc;
        private Mock<IPackagingInventoryService> _packInvSvc;
        private StockInService _svc;

        [TestInitialize]
        public void Setup()
        {
            _uow = new Mock<IUnitOfWork>();
            _repo = new Mock<IStockInRepository>();
            _invSvc = new Mock<IInventoryService>();
            _packInvSvc = new Mock<IPackagingInventoryService>();
            _uow.Setup(u => u.StockIn).Returns(_repo.Object);
            _svc = new StockInService(_uow.Object, _invSvc.Object, _packInvSvc.Object);
        }

        [TestMethod]
        public async Task Create_Product_Path_Success()
        {
            var stockIn = new StockIn { RefType = RefType.PRODUCT, RefId = 1, BatchNo = "B1", Quantity = 5m, ExpiryDate = DateTime.Today.AddDays(10) };
            _repo.Setup(r => r.IsBatchExistsAsync("B1", RefType.PRODUCT, 1)).ReturnsAsync(false);
            _uow.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _repo.Setup(r => r.AddAsync(stockIn)).ReturnsAsync(101);
            _invSvc.Setup(s => s.AddStockAsync(1, "B1", 5m, stockIn.ExpiryDate)).ReturnsAsync(Result<Inventory>.Ok(new Inventory()));
            _uow.Setup(u => u.CommitTransactionAsync()).Returns(Task.CompletedTask);
            _repo.Setup(r => r.GetByIdAsync(101)).ReturnsAsync(new StockIn { StockInId = 101 });

            var res = await _svc.CreateStockInAsync(stockIn);
            Assert.IsTrue(res.Success);
            _uow.Verify(u => u.BeginTransactionAsync(), Times.Once);
            _uow.Verify(u => u.CommitTransactionAsync(), Times.Once);
        }

        [TestMethod]
        public async Task Create_Packaging_Path_Success()
        {
            var stockIn = new StockIn { RefType = RefType.PACKAGING, RefId = 2, BatchNo = "", Quantity = 3m };
            _repo.Setup(r => r.IsBatchExistsAsync("", RefType.PACKAGING, 2)).ReturnsAsync(false);
            _uow.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _repo.Setup(r => r.AddAsync(stockIn)).ReturnsAsync(102);
            _packInvSvc.Setup(s => s.UpdatePackagingQuantitiesAsync(2, It.Is<PackagingQuantities>(q => q.QtyNew == 3))).ReturnsAsync(Result<bool>.Ok(true));
            _uow.Setup(u => u.CommitTransactionAsync()).Returns(Task.CompletedTask);
            _repo.Setup(r => r.GetByIdAsync(102)).ReturnsAsync(new StockIn { StockInId = 102 });

            var res = await _svc.CreateStockInAsync(stockIn);
            Assert.IsTrue(res.Success);
            _packInvSvc.Verify(s => s.UpdatePackagingQuantitiesAsync(2, It.IsAny<PackagingQuantities>()), Times.Once);
        }

        [TestMethod]
        public async Task Create_BatchExists_Fails()
        {
            var stockIn = new StockIn { RefType = RefType.PRODUCT, RefId = 1, BatchNo = "B1", Quantity = 5m, ExpiryDate = DateTime.Today.AddDays(10) };
            _repo.Setup(r => r.IsBatchExistsAsync("B1", RefType.PRODUCT, 1)).ReturnsAsync(true);
            var res = await _svc.CreateStockInAsync(stockIn);
            Assert.IsFalse(res.Success);
            _uow.Verify(u => u.BeginTransactionAsync(), Times.Never);
        }

        [TestMethod]
        public async Task CreateMultiple_Valid_All_Success()
        {
            var items = new List<StockIn>
            {
                new StockIn { RefType = RefType.PRODUCT, RefId = 1, BatchNo = "B1", Quantity = 2m, ExpiryDate = DateTime.Today.AddDays(5) },
                new StockIn { RefType = RefType.PACKAGING, RefId = 2, BatchNo = "", Quantity = 4m }
            };

            _repo.Setup(r => r.IsBatchExistsAsync("B1", RefType.PRODUCT, 1)).ReturnsAsync(false);
            _repo.Setup(r => r.IsBatchExistsAsync("", RefType.PACKAGING, 2)).ReturnsAsync(false);
            _uow.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _repo.Setup(r => r.AddAsync(It.IsAny<StockIn>())).ReturnsAsync(200);
            _invSvc.Setup(s => s.AddStockAsync(1, "B1", 2m, It.IsAny<DateTime?>())).ReturnsAsync(Result<Inventory>.Ok(new Inventory()));
            _packInvSvc.Setup(s => s.UpdatePackagingQuantitiesAsync(2, It.Is<PackagingQuantities>(q => q.QtyNew == 4))).ReturnsAsync(Result<bool>.Ok(true));
            _repo.Setup(r => r.GetByIdAsync(200)).ReturnsAsync(new StockIn { StockInId = 200 });
            _uow.Setup(u => u.CommitTransactionAsync()).Returns(Task.CompletedTask);

            var res = await _svc.CreateMultipleStockInsAsync(items);
            Assert.IsTrue(res.Success);
        }
    }
}
