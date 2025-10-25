using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Database;
using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{
    public class PackagingRepository : BaseRepository<Packaging>, IPackagingRepository
    {
        public PackagingRepository() : base("Packaging", "packaging_id", true) { }

        public PackagingRepository(IDbHelper dbHelper) : base(dbHelper, "Packaging", "packaging_id", true) { }

        public async Task<Packaging> GetPackagingByQrCodeAsync(string qrCode)
        {
            var sql = "SELECT * FROM Packaging WHERE qr_code = @QrCode";
            return await _dbHelper.QueryFirstOrDefaultAsync<Packaging>(sql, new { QrCode = qrCode });
        }

        public async Task<IEnumerable<Packaging>> GetPackagingsByStatusAsync(string status)
        {
            var sql = "SELECT * FROM Packaging WHERE status = @Status ORDER BY created_date DESC";
            return await _dbHelper.QueryAsync<Packaging>(sql, new { Status = status });
        }

        public async Task<int> GetReuseCountAsync(int packagingId)
        {
            var sql = "SELECT time_reused FROM Packaging WHERE packaging_id = @PackagingId";
            return await _dbHelper.ExecuteScalarAsync<int>(sql, new { PackagingId = packagingId });
        }

        public async Task<bool> UpdatePackagingStatusAsync(int packagingId, string status)
        {
            var sql = "UPDATE Packaging SET status = @Status WHERE packaging_id = @PackagingId";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new { PackagingId = packagingId, Status = status });
            return affectedRows > 0;
        }

        public async Task<bool> IncrementReuseCountAsync(int packagingId)
        {
            var sql = "UPDATE Packaging SET time_reused = time_reused + 1 WHERE packaging_id = @PackagingId";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new { PackagingId = packagingId });
            return affectedRows > 0;
        }

        public async Task<IEnumerable<Packaging>> GetPackagingsNeedCleaningAsync()
        {
            var sql = "SELECT * FROM Packaging WHERE status = 'needcleaning' ORDER BY created_date";
            return await _dbHelper.QueryAsync<Packaging>(sql);
        }

        public async Task<int> GetTotalPackagingCountByStatusAsync(string status)
        {
            var sql = "SELECT COUNT(*) FROM Packaging WHERE status = @Status";
            return await _dbHelper.ExecuteScalarAsync<int>(sql, new { Status = status });
        }
    }

    public class PackagingTransactionRepository : BaseRepository<PackagingTransaction>, IPackagingTransactionRepository
    {
        public PackagingTransactionRepository() : base("PackagingTransactions", "transaction_id", false) { }

        public PackagingTransactionRepository(IDbHelper dbHelper) : base(dbHelper, "PackagingTransactions", "transaction_id", false) { }

        public async Task<IEnumerable<PackagingTransaction>> GetTransactionsByPackagingAsync(int packagingId)
        {
            var sql = @"
                SELECT pt.*, 
                       s.name as StationName,
                       c.name as CustomerName
                FROM PackagingTransactions pt
                LEFT JOIN Stations s ON pt.station_id = s.station_id
                LEFT JOIN Customers c ON pt.customer_id = c.customer_id
                WHERE pt.packaging_id = @PackagingId
                ORDER BY pt.date DESC";
            return await _dbHelper.QueryAsync<PackagingTransaction>(sql, new { PackagingId = packagingId });
        }

        public async Task<IEnumerable<PackagingTransaction>> GetTransactionsByCustomerAsync(int customerId)
        {
            var sql = @"
                SELECT pt.*, 
                       s.name as StationName,
                       p.packaging_id, p.type as PackagingType
                FROM PackagingTransactions pt
                LEFT JOIN Stations s ON pt.station_id = s.station_id
                LEFT JOIN Packaging p ON pt.packaging_id = p.packaging_id
                WHERE pt.customer_id = @CustomerId
                ORDER BY pt.date DESC";
            return await _dbHelper.QueryAsync<PackagingTransaction>(sql, new { CustomerId = customerId });
        }

        public async Task<IEnumerable<PackagingTransaction>> GetTransactionsByStationAsync(int stationId)
        {
            var sql = @"
                SELECT pt.*, 
                       c.name as CustomerName,
                       p.packaging_id, p.type as PackagingType
                FROM PackagingTransactions pt
                LEFT JOIN Customers c ON pt.customer_id = c.customer_id
                LEFT JOIN Packaging p ON pt.packaging_id = p.packaging_id
                WHERE pt.station_id = @StationId
                ORDER BY pt.date DESC";
            return await _dbHelper.QueryAsync<PackagingTransaction>(sql, new { StationId = stationId });
        }

        public async Task<int> GetTotalPackagingIssuedAsync(int stationId, DateTime startDate, DateTime endDate)
        {
            var sql = @"
                SELECT COALESCE(SUM(quantity), 0)
                FROM PackagingTransactions
                WHERE station_id = @StationId 
                AND type = 'issue'
                AND date BETWEEN @StartDate AND @EndDate";
            return await _dbHelper.ExecuteScalarAsync<int>(sql, new { StationId = stationId, StartDate = startDate, EndDate = endDate });
        }

        public async Task<int> GetTotalPackagingReturnedAsync(int stationId, DateTime startDate, DateTime endDate)
        {
            var sql = @"
                SELECT COALESCE(SUM(quantity), 0)
                FROM PackagingTransactions
                WHERE station_id = @StationId 
                AND type = 'return'
                AND date BETWEEN @StartDate AND @EndDate";
            return await _dbHelper.ExecuteScalarAsync<int>(sql, new { StationId = stationId, StartDate = startDate, EndDate = endDate });
        }

        public async Task<int> GetCustomerPackagingBalanceAsync(int customerId)
        {
            var sql = @"
                SELECT 
                    (SELECT COALESCE(SUM(quantity), 0) FROM PackagingTransactions WHERE customer_id = @CustomerId AND type = 'issue') -
                    (SELECT COALESCE(SUM(quantity), 0) FROM PackagingTransactions WHERE customer_id = @CustomerId AND type = 'return') 
                AS balance";
            return await _dbHelper.ExecuteScalarAsync<int>(sql, new { CustomerId = customerId });
        }
    
        
    }


}