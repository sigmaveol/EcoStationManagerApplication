using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    // ==================== TRANSFER REPOSITORY INTERFACE ====================
    public interface ITransferRepository : IRepository<Transfer>
    {
        Task<IEnumerable<Transfer>> GetTransfersByStationAsync(int stationId);
        Task<IEnumerable<Transfer>> GetTransfersByStatusAsync(string status);
        Task<IEnumerable<Transfer>> GetTransfersByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Transfer>> GetTransfersByTypeAsync(string transferType);
        Task<Transfer> GetTransferWithDetailsAsync(int transferId);
        Task<bool> UpdateTransferStatusAsync(int transferId, string status);
    }

    // ==================== TRANSFER DETAIL REPOSITORY INTERFACE ====================
    public interface ITransferDetailRepository : IRepository<TransferDetail>
    {
        Task<IEnumerable<TransferDetail>> GetDetailsByTransferAsync(int transferId);
        Task<bool> DeleteDetailsByTransferAsync(int transferId);
        Task<decimal> GetTransferTotalQuantityAsync(int transferId);
        Task<bool> ValidateTransferDetailsAsync(int transferId);
    }

    // ==================== DELIVERY ASSIGNMENT REPOSITORY INTERFACE ====================
    public interface IDeliveryAssignmentRepository : IRepository<DeliveryAssignment>
    {
        Task<DeliveryAssignment> GetAssignmentByOrderAsync(int orderId);
        Task<IEnumerable<DeliveryAssignment>> GetAssignmentsByDriverAsync(int driverId);
        Task<IEnumerable<DeliveryAssignment>> GetAssignmentsByStatusAsync(string status);
        Task<IEnumerable<DeliveryAssignment>> GetAssignmentsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<DeliveryAssignment>> GetActiveAssignmentsAsync();
        Task<bool> UpdateDeliveryStatusAsync(int assignmentId, string status);
    }

    // ==================== DELIVERY ROUTE REPOSITORY INTERFACE ====================
    public interface IDeliveryRouteRepository : IRepository<DeliveryRoute>
    {
        Task<IEnumerable<DeliveryRoute>> GetRoutesByAssignmentAsync(int assignmentId);
        Task<IEnumerable<DeliveryRoute>> GetRoutesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<DeliveryRoute> GetLatestRoutePointAsync(int assignmentId);
        Task<bool> AddRoutePointAsync(int assignmentId, decimal latitude, decimal longitude);
        Task<bool> ClearRoutePointsAsync(int assignmentId);
    }
}