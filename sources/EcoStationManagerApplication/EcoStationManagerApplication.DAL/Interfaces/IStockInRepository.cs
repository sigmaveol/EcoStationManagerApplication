using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public interface IStockInRepository : IRepository<StockIn>
    {
        Task<IEnumerable<StockIn>> GetByBatchNoAsync(string batchNo);
        Task<IEnumerable<StockIn>> GetByRefTypeAsync(RefType refType);
        Task<IEnumerable<StockIn>> GetByReferenceAsync(RefType refType, int refId);
        Task<IEnumerable<StockIn>> GetBySupplierAsync(int supplierId);
        Task<IEnumerable<StockIn>> GetByCreatedByAsync(int createdBy);
        Task<IEnumerable<StockIn>> GetExpiringStockAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<StockIn>> GetExpiredStockAsync();
        Task<decimal> GetTotalQuantityByReferenceAsync(RefType refType, int refId);
        Task<bool> BatchNoExistsAsync(string batchNo);
        Task<(IEnumerable<StockIn> StockIns, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string search = null,
            RefType? refType = null
        );
        Task<IEnumerable<StockIn>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalStockInValueAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<string>> GetDistinctBatchNosAsync();
    }
}
