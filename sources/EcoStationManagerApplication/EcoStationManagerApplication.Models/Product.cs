using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public decimal? BasePrice { get; set; }
        public string UnitMeasure { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public int? CategoryID { get; set; }

        // Navigation properties
        public Category Category { get; set; }
        public List<Variant> Variants { get; set; } = new List<Variant>();
    }
}
