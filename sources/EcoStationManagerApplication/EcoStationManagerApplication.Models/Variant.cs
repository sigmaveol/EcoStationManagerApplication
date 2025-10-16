using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models
{
    public class Variant
    {
        public int VariantID { get; set; }
        public string Barcode { get; set; }
        public string SKU { get; set; }
        public string Name { get; set; }
        public decimal BasePrice { get; set; }
        public string Unit { get; set; }
        public decimal MinStock { get; set; }
        public decimal MaxStock { get; set; }
        public string QRCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public int ProductID { get; set; }

        // Navigation property
        public Product Product { get; set; }
    }
}
