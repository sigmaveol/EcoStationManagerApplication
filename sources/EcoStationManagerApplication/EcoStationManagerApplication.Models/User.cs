using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public bool IsActive { get; set; }
        public int RoleID { get; set; }
        public DateTime CreatedDate { get; set; }

        public Role Role { get; set; }

    }
}
