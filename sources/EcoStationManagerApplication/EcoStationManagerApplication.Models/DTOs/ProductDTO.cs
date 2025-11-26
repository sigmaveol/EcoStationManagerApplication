using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models.DTOs
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? CategoryId { get; set; }
        public string UnitMeasure { get; set; }
        public decimal? BasePrice { get; set; }
        public string ProductType { get; set; }
        public bool IsActive { get; set; }
    }

    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }

}
