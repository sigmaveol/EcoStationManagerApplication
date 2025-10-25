using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models
{
     public class OrderDetail
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int? ComboID { get; set; }
        public int? VariantID { get; set; }
        public decimal LineTotal { get; set; }

        // Navigation properties
        public Order Order { get; set; }
        public Combo Combo { get; set; }
        public Variant Variant { get; set; }
    }
}
