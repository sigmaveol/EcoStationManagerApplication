using Microsoft.VisualStudio.TestTools.UnitTesting;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Results;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EcoStationManagerApplication.Tests
{
    class FakeStockInService : IStockInService
    {
        private readonly List<StockIn> _stockIns = new List<StockIn>();
        public Task<Result<StockIn>> CreateStockInAsync(StockIn stockIn) { stockIn.StockInId = _stockIns.Count + 1; _stockIns.Add(stockIn); return Task.FromResult(Result<StockIn>.Ok(stockIn)); }
        public Task<Result<decimal>> GetTotalStockInValueAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            decimal total = 0m; foreach (var s in _stockIns) total += (s.Quantity * s.UnitPrice);
            return Task.FromResult(Result<decimal>.Ok(total));
        }
        public Task<Result<StockIn>> GetStockInByIdAsync(int stockInId) => Task.FromResult(Result<StockIn>.Ok(_stockIns.Find(s => s.StockInId == stockInId)));
        public Task<Result<List<StockIn>>> GetStockInByProductAsync(int productId) => Task.FromResult(Result<List<StockIn>>.Ok(_stockIns));
        public Task<Result<List<StockIn>>> GetStockInByDateRangeAsync(DateTime startDate, DateTime endDate) => Task.FromResult(Result<List<StockIn>>.Ok(_stockIns));
        public Task<Result<List<StockInDetail>>> GetStockInDetailsByDateRangeAsync(DateTime startDate, DateTime endDate) => Task.FromResult(Result<List<StockInDetail>>.Ok(new List<StockInDetail>()));
        public Task<Result<List<StockInDetail>>> GetStockInDetailsByBatchAsync(string batchNo) => Task.FromResult(Result<List<StockInDetail>>.Ok(new List<StockInDetail>()));
        public Task<Result<List<StockIn>>> CreateMultipleStockInsAsync(List<StockIn> stockIns) => Task.FromResult(Result<List<StockIn>>.Ok(stockIns));
        public Task<Result<List<StockInSummary>>> GetTopStockInProductsAsync(int limit = 10, DateTime? startDate = null, DateTime? endDate = null) => Task.FromResult(Result<List<StockInSummary>>.Ok(new List<StockInSummary>()));
    }

    class FakeStockOutService : IStockOutService
    {
        private readonly List<StockOut> _stockOuts = new List<StockOut>();
        public Task<Result<StockOut>> CreateStockOutAsync(StockOut stockOut) { stockOut.StockOutId = _stockOuts.Count + 1; _stockOuts.Add(stockOut); return Task.FromResult(Result<StockOut>.Ok(stockOut)); }
        public Task<Result<bool>> StockOutForOrderAsync(int productId, string batchNo, decimal quantity, int orderId, int userId) => Task.FromResult(Result<bool>.Ok(true));
        public Task<Result<decimal>> GetTotalStockOutValueAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            decimal total = 0m; foreach (var s in _stockOuts) total += s.Quantity; // Không có UnitPrice cho StockOut, giả sử tổng số lượng
            return Task.FromResult(Result<decimal>.Ok(total));
        }
        public Task<Result<StockOut>> GetStockOutByIdAsync(int stockOutId) => Task.FromResult(Result<StockOut>.Ok(_stockOuts.Find(s => s.StockOutId == stockOutId)));
        public Task<Result<List<StockOut>>> GetStockOutByProductAsync(int productId) => Task.FromResult(Result<List<StockOut>>.Ok(_stockOuts));
        public Task<Result<List<StockOut>>> GetStockOutByDateRangeAsync(DateTime startDate, DateTime endDate) => Task.FromResult(Result<List<StockOut>>.Ok(_stockOuts));
        public Task<Result<List<StockOutDetail>>> GetStockOutDetailsByDateRangeAsync(DateTime startDate, DateTime endDate) => Task.FromResult(Result<List<StockOutDetail>>.Ok(new List<StockOutDetail>()));
        public Task<Result<List<StockOut>>> CreateMultipleStockOutsAsync(List<StockOut> stockOuts) => Task.FromResult(Result<List<StockOut>>.Ok(stockOuts));
        public Task<Result<List<StockOutSummary>>> GetTopStockOutProductsAsync(int limit = 10, DateTime? startDate = null, DateTime? endDate = null) => Task.FromResult(Result<List<StockOutSummary>>.Ok(new List<StockOutSummary>()));
    }

    [TestClass]
    public class CoreStockInOutServiceTests
    {
        [TestMethod]
        public async Task CreateStockIn_And_TotalValue()
        {
            var svc = new FakeStockInService();
            var si = new StockIn { Quantity = 10m, UnitPrice = 10000m };
            var r = await svc.CreateStockInAsync(si);
            Assert.IsTrue(r.Success);
            var total = await svc.GetTotalStockInValueAsync();
            Assert.AreEqual(100000m, total.Data);
        }

        [TestMethod]
        public async Task CreateStockOut_And_TotalValue()
        {
            var svc = new FakeStockOutService();
            var so = new StockOut { Quantity = 75000m };
            var r = await svc.CreateStockOutAsync(so);
            Assert.IsTrue(r.Success);
            var total = await svc.GetTotalStockOutValueAsync();
            Assert.AreEqual(75000m, total.Data);
        }
    }
}
