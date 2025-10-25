using System;

namespace EcoStationManagerApplication.Models.Entities
{
    public class Packaging
    {
        public int PackagingId { get; set; }
        public string Type { get; set; }
        public string QrCode { get; set; }
        public string Status { get; set; }
        public bool HasQrCode { get; set; }
        public int TimeReused { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class PackagingTransaction
    {
        public int TransactionId { get; set; }
        public int StationId { get; set; }
        public string Type { get; set; }
        public int PointsEarned { get; set; }
        public DateTime Date { get; set; }
        public int? CustomerId { get; set; }
        public int? PackagingId { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; }

        // Navigation properties
        public Station Station { get; set; }
        public Customer Customer { get; set; }
        public Packaging Packaging { get; set; }
    }
}