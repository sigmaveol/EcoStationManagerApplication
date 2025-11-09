using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models.DTOs
{
    public class UserWithShift
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }
        public int? CurrentShiftId { get; set; }
        public string ShiftStatus { get; set; }
        public DateTime? ShiftStartTime { get; set; }
        public DateTime? ShiftEndTime { get; set; }
        public decimal? CurrentKpiScore { get; set; }
    }

    public class UserRoleStats
    {
        public UserRole Role { get; set; }
        public string RoleName { get; set; }
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
    }
}
