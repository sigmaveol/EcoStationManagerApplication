using System;
using System.Collections.Generic;

namespace EcoStationManagerApplication.Models.Entities
{

    // Station, WorkShift
    public class Station
    {
        public int station_id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string station_type { get; set; } // ENUM: 'warehouse', 'refill', 'hybrid', 'other'
        public DateTime created_date { get; set; }
        public DateTime updated_date { get; set; }
        public int? manager { get; set; } // User_id của quản lý trạm, có thể null
        public int? parent_station_id { get; set; }
        public bool is_active { get; set; }
    }

    public class Tank
    {
        public int TankId { get; set; }
        public int StationId { get; set; }
        public string Name { get; set; }
        public string Material { get; set; }
        public decimal Capacity { get; set; }
        public string Unit { get; set; }
        public decimal CurrentLevel { get; set; }
        public string Status { get; set; }
        public DateTime? LastCleanDate { get; set; }
        public DateTime? NextCleanDate { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        public Station Station { get; set; }
    }

    public class CleaningSchedule
    {
        public int CsId { get; set; }
        public int StationId { get; set; }
        public string CleaningType { get; set; }
        public DateTime CleaningDate { get; set; }
        public int? CleaningBy { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }

        // Navigation properties
        public Station Station { get; set; }
        public User CleaningByUser { get; set; }
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

        // Navigation properties
        public User User { get; set; }
    }
}