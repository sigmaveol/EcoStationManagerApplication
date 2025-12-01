using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public interface IDeliveryRepository : IRepository<DeliveryAssignment>
    {
        Task<IEnumerable<DeliveryAssignment>> GetByDriverAsync(int driverId);
        Task<IEnumerable<DeliveryAssignment>> GetByStatusAsync(DeliveryStatus status);
        Task<IEnumerable<DeliveryAssignment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<DeliveryAssignment>> GetPendingDeliveriesAsync();
        Task<IEnumerable<DeliveryAssignment>> GetCompletedDeliveriesAsync();
        Task<decimal> GetTotalCODByDriverAsync(int driverId, DateTime? startDate = null, DateTime? endDate = null);
        Task<bool> UpdateStatusAsync(int assignmentId, DeliveryStatus newStatus);
        Task<bool> UpdatePaymentStatusAsync(int assignmentId, DeliveryPaymentStatus newStatus);
        Task<(IEnumerable<DeliveryAssignment> Deliveries, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string search = null,
            DeliveryStatus? status = null,
            DeliveryPaymentStatus? paymentStatus = null
        );
    }
}
