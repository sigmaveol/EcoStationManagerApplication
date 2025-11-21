using EcoStationManagerApplication.Models.Enums;
using System;

namespace EcoStationManagerApplication.Models.DTOs
{
    public class DeliveryAssignmentDTO
    {
        public int AssignmentId { get; set; }
        public int OrderId { get; set; }
        public string OrderCode { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public DateTime AssignedDate { get; set; }
        public DeliveryStatus Status { get; set; }
        public decimal CodAmount { get; set; }
        public DeliveryPaymentStatus PaymentStatus { get; set; }
        public string Notes { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhone { get; set; }
    }

    public class CreateDeliveryAssignmentDTO
    {
        public int OrderId { get; set; }
        public int DriverId { get; set; }
        public decimal CodAmount { get; set; }
        public string Notes { get; set; }
    }

    public class UpdateDeliveryAssignmentDTO
    {
        public int AssignmentId { get; set; }
        public DeliveryStatus Status { get; set; }
        public DeliveryPaymentStatus PaymentStatus { get; set; }
        public string Notes { get; set; }
    }

    public class DeliveryAssignmentFilterDTO
    {
        public int? DriverId { get; set; }
        public int? OrderId { get; set; }
        public DeliveryStatus? Status { get; set; }
        public DeliveryPaymentStatus? PaymentStatus { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}

