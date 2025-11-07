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

    public class WorkShift
    {
        public int ShiftId { get; set; }
        public int UserId { get; set; }
        public DateTime ShiftDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public decimal? KpiScore { get; set; }
        public string Notes { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class CleaningSchedule
    {
        public int CsId { get; set; }
        public CleaningType CleaningType { get; set; }
        public DateTime CleaningDate { get; set; }
        public CleaningStatus Status { get; set; }
        public string Notes { get; set; }
        public int? CleaningBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
