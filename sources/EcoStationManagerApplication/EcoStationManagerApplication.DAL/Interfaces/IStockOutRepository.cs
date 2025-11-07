using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public interface IStockOutRepository : IRepository<StockOut>
    {
        Task<IEnumerable<StockOut>> GetByBatchNoAsync(string batchNo);
        Task<IEnumerable<StockOut>> GetByRefTypeAsync(RefType refType);
        Task<IEnumerable<StockOut>> GetByReferenceAsync(RefType refType, int refId);
        Task<IEnumerable<StockOut>> GetByCreatedByAsync(int createdBy);
        Task<IEnumerable<StockOut>> GetByPurposeAsync(StockOutPurpose purpose);
        Task<IEnumerable<StockOut>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalQuantityByReferenceAsync(RefType refType, int refId);
        Task<decimal> GetTotalStockOutValueAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<(IEnumerable<StockOut> StockOuts, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string search = null,
            RefType? refType = null,
            StockOutPurpose? purpose = null
        );
        Task<IEnumerable<string>> GetDistinctBatchNosAsync();
    }
}
