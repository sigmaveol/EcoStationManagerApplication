using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models.Entities
{
    public class User
    {
        public int UserId { get; set; }                 // user_id INT PRIMARY KEY AUTO_INCREMENT
        public string Username { get; set; }           // username VARCHAR(100) UNIQUE NOT NULL
        public string PasswordHash { get; set; }       // password_hash VARCHAR(255) NOT NULL
        public string Fullname { get; set; }           // fullname VARCHAR(255)
        public UserRole Role { get; set; } = UserRole.STAFF;  // role ENUM('ADMIN','STAFF','MANAGER','DRIVER') DEFAULT 'STAFF'
        public bool IsActive { get; set; } = true;    // is_active BOOLEAN DEFAULT TRUE
        public DateTime CreatedDate { get; set; } = DateTime.Now;  // created_date DATETIME DEFAULT CURRENT_TIMESTAMP
    }
}
