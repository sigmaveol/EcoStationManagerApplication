using Dapper;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Database;
using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{
    // ==================== TRANSFER REPOSITORY IMPLEMENTATION ====================
    public class TransferRepository : BaseRepository<Transfer>, ITransferRepository
    {
        public TransferRepository() : base("Transfers", "transfer_id", false) { }
        public TransferRepository(IDbHelper dbHelper) : base(dbHelper, "Transfers", "transfer_id", false) { }

        public async Task<IEnumerable<Transfer>> GetTransfersByStationAsync(int stationId)
        {
            var sql = @"SELECT t.*, fs.name as FromStationName, ts.name as ToStationName, u.fullname as CreatedByName
                       FROM Transfers t
                       LEFT JOIN Stations fs ON t.from_station_id = fs.station_id
                       LEFT JOIN Stations ts ON t.to_station_id = ts.station_id
                       LEFT JOIN Users u ON t.created_by = u.user_id
                       WHERE t.from_station_id = @StationId OR t.to_station_id = @StationId
                       ORDER BY t.created_date DESC";
            return await _dbHelper.QueryAsync<Transfer>(sql, new { StationId = stationId });
        }

        public async Task<IEnumerable<Transfer>> GetTransfersByStatusAsync(string status)
        {
            var sql = @"SELECT t.*, fs.name as FromStationName, ts.name as ToStationName
                       FROM Transfers t
                       LEFT JOIN Stations fs ON t.from_station_id = fs.station_id
                       LEFT JOIN Stations ts ON t.to_station_id = ts.station_id
                       WHERE t.status = @Status
                       ORDER BY t.created_date DESC";
            return await _dbHelper.QueryAsync<Transfer>(sql, new { Status = status });
        }

        public async Task<IEnumerable<Transfer>> GetTransfersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var sql = @"SELECT t.*, fs.name as FromStationName, ts.name as ToStationName
                       FROM Transfers t
                       LEFT JOIN Stations fs ON t.from_station_id = fs.station_id
                       LEFT JOIN Stations ts ON t.to_station_id = ts.station_id
                       WHERE t.created_date BETWEEN @StartDate AND @EndDate
                       ORDER BY t.created_date DESC";
            return await _dbHelper.QueryAsync<Transfer>(sql, new { StartDate = startDate, EndDate = endDate });
        }

        public async Task<IEnumerable<Transfer>> GetTransfersByTypeAsync(string transferType)
        {
            var sql = @"SELECT t.*, fs.name as FromStationName, ts.name as ToStationName
                       FROM Transfers t
                       LEFT JOIN Stations fs ON t.from_station_id = fs.station_id
                       LEFT JOIN Stations ts ON t.to_station_id = ts.station_id
                       WHERE t.status = @TransferType
                       ORDER BY t.created_date DESC";
            return await _dbHelper.QueryAsync<Transfer>(sql, new { TransferType = transferType });
        }

        public async Task<Transfer> GetTransferWithDetailsAsync(int transferId)
        {
            var sql = @"SELECT t.*, fs.name as FromStationName, ts.name as ToStationName,
                               td.*, v.name as VariantName, v.sku as VariantSKU
                       FROM Transfers t
                       LEFT JOIN Stations fs ON t.from_station_id = fs.station_id
                       LEFT JOIN Stations ts ON t.to_station_id = ts.station_id
                       LEFT JOIN TransferDetails td ON t.transfer_id = td.transfer_id
                       LEFT JOIN Variants v ON td.variant_id = v.variant_id
                       WHERE t.transfer_id = @TransferId";
            return await _dbHelper.QueryFirstOrDefaultAsync<Transfer>(sql, new { TransferId = transferId });
        }

        public async Task<bool> UpdateTransferStatusAsync(int transferId, string status)
        {
            var sql = "UPDATE Transfers SET status = @Status, updated_date = NOW() WHERE transfer_id = @TransferId";
            var result = await _dbHelper.ExecuteAsync(sql, new { TransferId = transferId, Status = status });
            return result > 0;
        }
    }

    // ==================== TRANSFER DETAIL REPOSITORY IMPLEMENTATION ====================
    public class TransferDetailRepository : BaseRepository<TransferDetail>, ITransferDetailRepository
    {
        public TransferDetailRepository() : base("TransferDetails", "transfer_id", false) { }
        public TransferDetailRepository(IDbHelper dbHelper) : base(dbHelper, "TransferDetails", "transfer_id", false) { }

        public async Task<IEnumerable<TransferDetail>> GetDetailsByTransferAsync(int transferId)
        {
            var sql = @"SELECT td.*, v.name as VariantName, v.sku as VariantSKU, v.unit as VariantUnit
                       FROM TransferDetails td
                       JOIN Variants v ON td.variant_id = v.variant_id
                       WHERE td.transfer_id = @TransferId
                       ORDER BY v.name";
            return await _dbHelper.QueryAsync<TransferDetail>(sql, new { TransferId = transferId });
        }

        public async Task<bool> DeleteDetailsByTransferAsync(int transferId)
        {
            var sql = "DELETE FROM TransferDetails WHERE transfer_id = @TransferId";
            var result = await _dbHelper.ExecuteAsync(sql, new { TransferId = transferId });
            return result > 0;
        }

        public async Task<decimal> GetTransferTotalQuantityAsync(int transferId)
        {
            var sql = "SELECT COALESCE(SUM(quantity), 0) FROM TransferDetails WHERE transfer_id = @TransferId";
            return await _dbHelper.ExecuteScalarAsync<decimal>(sql, new { TransferId = transferId });
        }

        public async Task<bool> ValidateTransferDetailsAsync(int transferId)
        {
            var sql = @"SELECT COUNT(*) FROM TransferDetails td
                       JOIN Variants v ON td.variant_id = v.variant_id
                       WHERE td.transfer_id = @TransferId AND v.is_active = 1";
            var validCount = await _dbHelper.ExecuteScalarAsync<int>(sql, new { TransferId = transferId });

            var totalCount = await _dbHelper.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM TransferDetails WHERE transfer_id = @TransferId",
                new { TransferId = transferId });

            return validCount == totalCount;
        }
    }

    // ==================== DELIVERY ASSIGNMENT REPOSITORY IMPLEMENTATION ====================
    public class DeliveryAssignmentRepository : BaseRepository<DeliveryAssignment>, IDeliveryAssignmentRepository
    {
        public DeliveryAssignmentRepository() : base("DeliveryAssignments", "assignment_id", false) { }
        public DeliveryAssignmentRepository(IDbHelper dbHelper) : base(dbHelper, "DeliveryAssignments", "assignment_id", false) { }

        public async Task<DeliveryAssignment> GetAssignmentByOrderAsync(int orderId)
        {
            var sql = @"SELECT da.*, u.fullname as DriverName, o.order_id
                       FROM DeliveryAssignments da
                       JOIN Users u ON da.driver_id = u.user_id
                       JOIN Orders o ON da.order_id = o.order_id
                       WHERE da.order_id = @OrderId
                       ORDER BY da.assigned_date DESC
                       LIMIT 1";
            return await _dbHelper.QueryFirstOrDefaultAsync<DeliveryAssignment>(sql, new { OrderId = orderId });
        }

        public async Task<IEnumerable<DeliveryAssignment>> GetAssignmentsByDriverAsync(int driverId)
        {
            var sql = @"SELECT da.*, o.order_id, o.total_amount, c.name as CustomerName
                       FROM DeliveryAssignments da
                       JOIN Orders o ON da.order_id = o.order_id
                       JOIN Customers c ON o.customer_id = c.customer_id
                       WHERE da.driver_id = @DriverId
                       ORDER BY da.assigned_date DESC";
            return await _dbHelper.QueryAsync<DeliveryAssignment>(sql, new { DriverId = driverId });
        }

        public async Task<IEnumerable<DeliveryAssignment>> GetAssignmentsByStatusAsync(string status)
        {
            var sql = @"SELECT da.*, u.fullname as DriverName, o.order_id, c.name as CustomerName
                       FROM DeliveryAssignments da
                       JOIN Users u ON da.driver_id = u.user_id
                       JOIN Orders o ON da.order_id = o.order_id
                       JOIN Customers c ON o.customer_id = c.customer_id
                       WHERE da.status = @Status
                       ORDER BY da.assigned_date DESC";
            return await _dbHelper.QueryAsync<DeliveryAssignment>(sql, new { Status = status });
        }

        public async Task<IEnumerable<DeliveryAssignment>> GetAssignmentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var sql = @"SELECT da.*, u.fullname as DriverName, o.order_id, c.name as CustomerName
                       FROM DeliveryAssignments da
                       JOIN Users u ON da.driver_id = u.user_id
                       JOIN Orders o ON da.order_id = o.order_id
                       JOIN Customers c ON o.customer_id = c.customer_id
                       WHERE da.assigned_date BETWEEN @StartDate AND @EndDate
                       ORDER BY da.assigned_date DESC";
            return await _dbHelper.QueryAsync<DeliveryAssignment>(sql, new { StartDate = startDate, EndDate = endDate });
        }

        public async Task<IEnumerable<DeliveryAssignment>> GetActiveAssignmentsAsync()
        {
            var sql = @"SELECT da.*, u.fullname as DriverName, o.order_id, c.name as CustomerName
                       FROM DeliveryAssignments da
                       JOIN Users u ON da.driver_id = u.user_id
                       JOIN Orders o ON da.order_id = o.order_id
                       JOIN Customers c ON o.customer_id = c.customer_id
                       WHERE da.status IN ('pending', 'intransit')
                       ORDER BY da.assigned_date DESC";
            return await _dbHelper.QueryAsync<DeliveryAssignment>(sql);
        }

        public async Task<bool> UpdateDeliveryStatusAsync(int assignmentId, string status)
        {
            var sql = "UPDATE DeliveryAssignments SET status = @Status WHERE assignment_id = @AssignmentId";
            var result = await _dbHelper.ExecuteAsync(sql, new { AssignmentId = assignmentId, Status = status });
            return result > 0;
        }
    }

    // ==================== DELIVERY ROUTE REPOSITORY IMPLEMENTATION ====================
    public class DeliveryRouteRepository : BaseRepository<DeliveryRoute>, IDeliveryRouteRepository
    {
        public DeliveryRouteRepository() : base("DeliveryRoutes", "route_id", false) { }
        public DeliveryRouteRepository(IDbHelper dbHelper) : base(dbHelper, "DeliveryRoutes", "route_id", false) { }

        public async Task<IEnumerable<DeliveryRoute>> GetRoutesByAssignmentAsync(int assignmentId)
        {
            var sql = @"SELECT dr.*, da.assignment_id
                       FROM DeliveryRoutes dr
                       JOIN DeliveryAssignments da ON dr.assignment_id = da.assignment_id
                       WHERE dr.assignment_id = @AssignmentId
                       ORDER BY dr.recorded_time";
            return await _dbHelper.QueryAsync<DeliveryRoute>(sql, new { AssignmentId = assignmentId });
        }

        public async Task<IEnumerable<DeliveryRoute>> GetRoutesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var sql = @"SELECT dr.*, da.assignment_id, u.fullname as DriverName
                       FROM DeliveryRoutes dr
                       JOIN DeliveryAssignments da ON dr.assignment_id = da.assignment_id
                       JOIN Users u ON da.driver_id = u.user_id
                       WHERE dr.recorded_time BETWEEN @StartDate AND @EndDate
                       ORDER BY dr.recorded_time";
            return await _dbHelper.QueryAsync<DeliveryRoute>(sql, new { StartDate = startDate, EndDate = endDate });
        }

        public async Task<DeliveryRoute> GetLatestRoutePointAsync(int assignmentId)
        {
            var sql = @"SELECT dr.*
                       FROM DeliveryRoutes dr
                       WHERE dr.assignment_id = @AssignmentId
                       ORDER BY dr.recorded_time DESC
                       LIMIT 1";
            return await _dbHelper.QueryFirstOrDefaultAsync<DeliveryRoute>(sql, new { AssignmentId = assignmentId });
        }

        public async Task<bool> AddRoutePointAsync(int assignmentId, decimal latitude, decimal longitude)
        {
            var sql = @"INSERT INTO DeliveryRoutes (assignment_id, latitude, longitude, recorded_time, source)
                       VALUES (@AssignmentId, @Latitude, @Longitude, NOW(), 'app')";
            var result = await _dbHelper.ExecuteAsync(sql, new
            {
                AssignmentId = assignmentId,
                Latitude = latitude,
                Longitude = longitude
            });
            return result > 0;
        }

        public async Task<bool> ClearRoutePointsAsync(int assignmentId)
        {
            var sql = "DELETE FROM DeliveryRoutes WHERE assignment_id = @AssignmentId";
            var result = await _dbHelper.ExecuteAsync(sql, new { AssignmentId = assignmentId });
            return result > 0;
        }
    }
}