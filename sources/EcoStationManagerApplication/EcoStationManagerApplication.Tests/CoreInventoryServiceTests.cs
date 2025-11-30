using Microsoft.VisualStudio.TestTools.UnitTesting;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Results;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EcoStationManagerApplication.Tests
{
    class FakeInventoryService : IInventoryService
    {
        private readonly Dictionary<(int productId, string batch), Inventory> _inv = new Dictionary<(int, string), Inventory>();

        public Task<Result<Inventory>> AddStockAsync(int productId, string batchNo, decimal quantity, DateTime? expiryDate)
        {
            var key = (productId, batchNo);
            if (_inv.TryGetValue(key, out var i)) i.Quantity += quantity;
            else { i = new Inventory { InventoryId = _inv.Count + 1, ProductId = productId, BatchNo = batchNo, Quantity = quantity, ExpiryDate = expiryDate }; _inv[key] = i; }
            return Task.FromResult(Result<Inventory>.Ok(i));
        }

        public Task<Result<bool>> ReduceStockAsync(int productId, string batchNo, decimal quantity)
        {
            var key = (productId, batchNo);
            if (!_inv.TryGetValue(key, out var i) || i.Quantity < quantity) return Task.FromResult(Result<bool>.Fail("Insufficient"));
            i.Quantity -= quantity; _inv[key] = i; return Task.FromResult(Result<bool>.Ok(true));
        }

        public Task<Result<bool>> IsStockSufficientAsync(int productId, decimal requiredQuantity)
        {
            decimal total = 0m; foreach (var kv in _inv) if (kv.Key.productId == productId) total += kv.Value.Quantity;
            return Task.FromResult(Result<bool>.Ok(total >= requiredQuantity));
        }

        public Task<Result<decimal>> GetTotalStockQuantityAsync(int productId)
        {
            decimal total = 0m; foreach (var kv in _inv) if (kv.Key.productId == productId) total += kv.Value.Quantity;
            return Task.FromResult(Result<decimal>.Ok(total));
        }

        public Task<Result<Inventory>> GetInventoryByIdAsync(int inventoryId) => Task.FromResult(Result<Inventory>.Ok(null));
        public Task<Result<List<Inventory>>> GetAllAsync() => Task.FromResult(Result<List<Inventory>>.Ok(new List<Inventory>(_inv.Values)));
        public Task<Result<List<Inventory>>> GetInventoryByProductAsync(int productId) => Task.FromResult(Result<List<Inventory>>.Ok(new List<Inventory>()));
        public Task<Result<List<Inventory>>> GetLowStockItemsAsync() => Task.FromResult(Result<List<Inventory>>.Ok(new List<Inventory>()));
        public Task<Result<List<Inventory>>> GetExpiringItemsAsync(int daysThreshold = 15) => Task.FromResult(Result<List<Inventory>>.Ok(new List<Inventory>()));
        public Task<Result<bool>> UpdateStockQuantityAsync(int inventoryId, decimal newQuantity) => Task.FromResult(Result<bool>.Ok(true));
        public Task<Result<List<Inventory>>> GetInventoryHistoryAsync(int productId, DateTime? fromDate = null, DateTime? toDate = null) => Task.FromResult(Result<List<Inventory>>.Ok(new List<Inventory>()));
    }

    [TestClass]
    public class CoreInventoryServiceTests
    {
        [TestMethod]
        public async Task Add_And_Reduce_Stock_Works()
        {
            var svc = new FakeInventoryService();
            var add = await svc.AddStockAsync(1, "B1", 10m, null);
            Assert.IsTrue(add.Success);
            var ok = await svc.ReduceStockAsync(1, "B1", 3m);
            Assert.IsTrue(ok.Success);
            var total = await svc.GetTotalStockQuantityAsync(1);
            Assert.AreEqual(7m, total.Data);
        }

        [TestMethod]
        public async Task Reduce_Fails_When_Insufficient()
        {
            var svc = new FakeInventoryService();
            await svc.AddStockAsync(2, "B2", 2m, null);
            var ok = await svc.ReduceStockAsync(2, "B2", 5m);
            Assert.IsFalse(ok.Success);
        }
    }
}
