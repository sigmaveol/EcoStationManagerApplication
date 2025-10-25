using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    // ==================== TRANSFER SERVICE INTERFACE ====================
    public interface ITransferService : IService<Transfer>
    {
        Task<IEnumerable<Transfer>> GetTransfersByStationAsync(int stationId);
        Task<IEnumerable<Transfer>> GetTransfersByStatusAsync(string status);
        Task<IEnumerable<Transfer>> GetTransfersByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Transfer> GetTransferWithDetailsAsync(int transferId);
        Task<bool> CreateTransferWithDetailsAsync(Transfer transfer, List<TransferDetail> details);
        Task<bool> UpdateTransferStatusAsync(int transferId, string status);
        Task<bool> ProcessTransferAsync(int transferId, int processedBy);
        Task<bool> CancelTransferAsync(int transferId, string reason);
        Task<Dictionary<string, object>> GetTransferStatisticsAsync(int stationId);
    }

    // ==================== TRANSFER DETAIL SERVICE INTERFACE ====================
    public interface ITransferDetailService : IService<TransferDetail>
    {
        Task<IEnumerable<TransferDetail>> GetDetailsByTransferAsync(int transferId);
        Task<bool> DeleteDetailsByTransferAsync(int transferId);
        Task<bool> AddDetailsToTransferAsync(int transferId, List<TransferDetail> details);
        Task<decimal> GetTransferTotalQuantityAsync(int transferId);
        Task<bool> ValidateTransferDetailsAsync(int transferId);
    }

    // ==================== DELIVERY ASSIGNMENT SERVICE INTERFACE ====================
    public interface IDeliveryAssignmentService : IService<DeliveryAssignment>
    {
        Task<DeliveryAssignment> GetAssignmentByOrderAsync(int orderId);
        Task<IEnumerable<DeliveryAssignment>> GetAssignmentsByDriverAsync(int driverId);
        Task<IEnumerable<DeliveryAssignment>> GetAssignmentsByStatusAsync(string status);
        Task<IEnumerable<DeliveryAssignment>> GetAssignmentsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<DeliveryAssignment>> GetTodayAssignmentsAsync(int driverId);
        Task<bool> UpdateDeliveryStatusAsync(int assignmentId, string status);
        Task<bool> AssignDriverToOrderAsync(int orderId, int driverId);
        Task<bool> CompleteDeliveryAsync(int assignmentId, string notes);
        Task<bool> CancelDeliveryAsync(int assignmentId, string reason);
        Task<Dictionary<string, object>> GetDeliveryPerformanceAsync(int driverId);
    }

    // ==================== DELIVERY ROUTE SERVICE INTERFACE ====================
    public interface IDeliveryRouteService : IService<DeliveryRoute>
    {
        Task<IEnumerable<DeliveryRoute>> GetRoutesByAssignmentAsync(int assignmentId);
        Task<IEnumerable<DeliveryRoute>> GetRoutesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<bool> AddRoutePointAsync(int assignmentId, decimal latitude, decimal longitude, DateTime recordedTime);
        Task<bool> ImportRouteFromExcelAsync(int assignmentId, string filePath);
        Task<IEnumerable<DeliveryRoute>> GetRouteByAssignmentAndDateAsync(int assignmentId, DateTime date);
        Task<bool> ClearRoutePointsAsync(int assignmentId);
        Task<Dictionary<string, object>> GetRouteStatisticsAsync(int assignmentId);
    }
}