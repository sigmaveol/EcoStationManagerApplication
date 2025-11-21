using EcoStationManagerApplication.Models.Enums;
using System;

namespace EcoStationManagerApplication.Models.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public UserRole Role { get; set; }
        public ActiveStatus IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CreateUserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public UserRole Role { get; set; }
    }

    public class UpdateUserDTO
    {
        public int UserId { get; set; }
        public string Fullname { get; set; }
        public UserRole Role { get; set; }
        public ActiveStatus IsActive { get; set; }
    }

    public class ChangePasswordDTO
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class WorkShiftDTO
    {
        public int ShiftId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserFullname { get; set; }
        public DateTime ShiftDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public decimal? KpiScore { get; set; }
        public string Notes { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class CreateWorkShiftDTO
    {
        public int UserId { get; set; }
        public DateTime ShiftDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string Notes { get; set; }
    }

    public class UpdateWorkShiftDTO
    {
        public int ShiftId { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public decimal? KpiScore { get; set; }
        public string Notes { get; set; }
    }

    public class WorkShiftFilterDTO
    {
        public int? UserId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public UserRole? Role { get; set; }
    }
}

