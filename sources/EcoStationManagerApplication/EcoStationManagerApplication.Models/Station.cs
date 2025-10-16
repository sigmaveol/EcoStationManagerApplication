using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models
{
    public class Station
    {
        public int StationID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int? Manager { get; set; } // UserID
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation property
        public User ManagerUser { get; set; }
    }
}
