using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface IPackagingService : IService<Packaging>
    {
        Task<Packaging> GetPackagingByQrCodeAsync(string qrCode);
        Task<IEnumerable<Packaging>> GetPackagingsByStatusAsync(string status);
        Task<bool> UpdatePackagingStatusAsync(int packagingId, string status);
        Task<int> GetReuseCountAsync(int packagingId);
        Task<IEnumerable<Packaging>> GetPackagingsNeedCleaningAsync();
        Task<IEnumerable<Packaging>> GetPackagingsByTypeAsync(string type);
        Task<bool> IssuePackagingToCustomerAsync(int packagingId, int customerId, int stationId);
        Task<bool> ReturnPackagingFromCustomerAsync(int packagingId, int customerId, int stationId);
    }

    public interface IPackagingTransactionService : IService<PackagingTransaction>
    {
        Task<IEnumerable<PackagingTransaction>> GetTransactionsByPackagingAsync(int packagingId);
        Task<IEnumerable<PackagingTransaction>> GetTransactionsByCustomerAsync(int customerId);
        Task<IEnumerable<PackagingTransaction>> GetTransactionsByStationAsync(int stationId);
        Task<int> GetTotalPackagingIssuedAsync(int stationId, DateTime startDate, DateTime endDate);
        Task<int> GetTotalPackagingReturnedAsync(int stationId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<PackagingTransaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<int> GetCustomerPackagingBalanceAsync(int customerId);
        Task<bool> RecordPackagingTransactionAsync(PackagingTransaction transaction);
        Task<int> CalculateCustomerPointsAsync(int customerId);
    }
}