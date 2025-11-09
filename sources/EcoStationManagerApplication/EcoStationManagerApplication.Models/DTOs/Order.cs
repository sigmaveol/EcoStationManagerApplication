using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models.DTOs
{
    public class OrderSummary
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int PendingOrderCount { get; set; }
        public int TodayOrderCount { get; set; }
        public decimal AverageOrderValue { get; set; }
    }

    public class OrderSearchCriteria
    {
        public string SearchKeyword { get; set; }
        public OrderStatus? Status { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public ProductType? ProductType { get; set; }
        public OrderSource Source { get; set; }
        public decimal? MinTotal { get; set; }
        public int? CustomerId { get; set; }
        public int? UserId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
