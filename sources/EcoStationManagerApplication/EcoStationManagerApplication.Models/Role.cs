using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models
{
    public class Role
    {
        public int RoleID { get; set; }
        public string Name { get; set; }
        public string Permissions { get; set; } // JSON string
        public DateTime CreatedDate { get; set; }
    }
}
