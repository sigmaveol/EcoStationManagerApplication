using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface IPackagingRepository : IRepository<Packaging>
    {
        Task<Packaging> GetPackagingByQrCodeAsync(string qrCode);
        Task<IEnumerable<Packaging>> GetPackagingsByStatusAsync(string status);
        Task<bool> UpdatePackagingStatusAsync(int packagingId, string status);
        Task<int> GetReuseCountAsync(int packagingId);
        Task<IEnumerable<Packaging>> GetPackagingsNeedCleaningAsync();
        Task<IEnumerable<Packaging>> GetPackagingsByTypeAsync(string type);
        // THIẾU: GetPackagingStatisticsAsync
        Task<Dictionary<string, int>> GetPackagingStatisticsAsync(); // ĐÃ THÊM
    }

    public interface IPackagingTransactionRepository : IRepository<PackagingTransaction>
    {
        Task<IEnumerable<PackagingTransaction>> GetTransactionsByPackagingAsync(int packagingId);
        Task<IEnumerable<PackagingTransaction>> GetTransactionsByCustomerAsync(int customerId);
        Task<IEnumerable<PackagingTransaction>> GetTransactionsByStationAsync(int stationId);
        Task<int> GetTotalPackagingIssuedAsync(int stationId, DateTime startDate, DateTime endDate);
        Task<int> GetTotalPackagingReturnedAsync(int stationId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<PackagingTransaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<int> GetCustomerPackagingBalanceAsync(int customerId);
        // THIẾU: GetTransactionStatisticsAsync
        Task<Dictionary<string, int>> GetTransactionStatisticsAsync(DateTime startDate, DateTime endDate); // ĐÃ THÊM
    }
}