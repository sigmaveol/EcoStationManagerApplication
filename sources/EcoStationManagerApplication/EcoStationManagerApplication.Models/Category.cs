using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int? ParentID { get; set; }
        public int SortOrder { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation property
        public Category Parent { get; set; }
    }
}
