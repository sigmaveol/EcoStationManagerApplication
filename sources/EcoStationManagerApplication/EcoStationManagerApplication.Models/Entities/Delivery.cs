using System;
using System.Collections.Generic;

namespace EcoStationManagerApplication.Models.Entities
{
    public class Transfer
    {
        public int TransferId { get; set; }
        public string Code { get; set; }
        public int FromStationId { get; set; }
        public int ToStationId { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        // Navigation properties
        public Station FromStation { get; set; }
        public Station ToStation { get; set; }
        public User CreatedByUser { get; set; }
        public List<TransferDetail> TransferDetails { get; set; } = new List<TransferDetail>();
    }

    public class TransferDetail
    {
        public int TransferId { get; set; }
        public int VariantId { get; set; }
        public decimal Quantity { get; set; }

        // Navigation properties
        public Transfer Transfer { get; set; }
        public Variant Variant { get; set; }
    }

    public class DeliveryAssignment
    {
        public int AssignmentId { get; set; }
        public int OrderId { get; set; }
        public int DriverId { get; set; }
        public DateTime AssignedDate { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }

        // Navigation properties
        public Order Order { get; set; }
        public User Driver { get; set; }
        public List<DeliveryRoute> DeliveryRoutes { get; set; } = new List<DeliveryRoute>();
    }

    public class DeliveryRoute
    {
        public int RouteId { get; set; }
        public int AssignmentId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public DateTime RecordedTime { get; set; }
        public string Source { get; set; }

        // Navigation properties
        public DeliveryAssignment Assignment { get; set; }
    }

}