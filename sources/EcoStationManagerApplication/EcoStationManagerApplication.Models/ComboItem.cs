using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models
{
    public class ComboItem
    {
        public int ComboItemID { get; set; }
        public decimal Quantity { get; set; }
        public int ComboID { get; set; }
        public int VariantID { get; set; }

        // Navigation properties
        public Combo Combo { get; set; }
        public Variant Variant { get; set; }
    }
}
