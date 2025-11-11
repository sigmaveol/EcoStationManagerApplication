using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface IStockOutService
    {
        Task<Result<StockOut>> GetStockOutByIdAsync(int stockOutId);
        Task<Result<List<StockOut>>> GetStockOutByProductAsync(int productId);
        Task<Result<List<StockOut>>> GetStockOutByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Result<StockOut>> CreateStockOutAsync(StockOut stockOut);
        Task<Result<bool>> StockOutForOrderAsync(int productId, string batchNo, decimal quantity, int orderId, int userId);
        Task<Result<decimal>> GetTotalStockOutValueAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<Result<List<StockOutSummary>>> GetTopStockOutProductsAsync(int limit = 10, DateTime? startDate = null, DateTime? endDate = null);
    }
}
