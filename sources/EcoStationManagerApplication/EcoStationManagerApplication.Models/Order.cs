using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public string Source { get; set; } // GoogleForm, Excel, Email, Manual
        public decimal TotalAmount { get; set; }
        public decimal DiscountedAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public string Status { get; set; } // Draft, Confirmed, Processing, etc.
        public string Note { get; set; }
        public string PaymentMethod { get; set; } // Cash, Transfer, EWallet
        public string PaymentStatus { get; set; } // Unpaid, Paid, Partial, Refunded
        public int? UserID { get; set; }
        public int? CustomerID { get; set; }
        public int? StationID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        // Navigation properties
        public User User { get; set; }
        public Customer Customer { get; set; }
        public Station Station { get; set; }
    }
}
