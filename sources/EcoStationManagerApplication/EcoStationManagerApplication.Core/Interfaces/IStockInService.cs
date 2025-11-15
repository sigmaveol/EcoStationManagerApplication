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
    public interface IStockInService
    {
        Task<Result<StockIn>> GetStockInByIdAsync(int stockInId);
        Task<Result<List<StockIn>>> GetStockInByProductAsync(int productId);
        Task<Result<List<StockIn>>> GetStockInByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Result<List<StockInDetail>>> GetStockInDetailsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Result<List<StockInDetail>>> GetStockInDetailsByBatchAsync(string batchNo);
        Task<Result<StockIn>> CreateStockInAsync(StockIn stockIn);
        Task<Result<List<StockIn>>> CreateMultipleStockInsAsync(List<StockIn> stockIns);
        Task<Result<decimal>> GetTotalStockInValueAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<Result<List<StockInSummary>>> GetTopStockInProductsAsync(int limit = 10, DateTime? startDate = null, DateTime? endDate = null);
    }
}
