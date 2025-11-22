using EcoStationManagerApplication.Models.Enums;
using System;

namespace EcoStationManagerApplication.Models.DTOs
{
    public class CleaningScheduleDTO
    {
        public int CsId { get; set; }
        public CleaningType CleaningType { get; set; }
        public DateTime CleaningDate { get; set; }
        public int? CleaningBy { get; set; }
        public string CleaningByName { get; set; }
        public CleaningStatus Status { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CreateCleaningScheduleDTO
    {
        public CleaningType CleaningType { get; set; }
        public DateTime CleaningDate { get; set; }
        public int? CleaningBy { get; set; }
        public string Notes { get; set; }
    }

    public class UpdateCleaningScheduleDTO
    {
        public int CsId { get; set; }
        public CleaningType CleaningType { get; set; }
        public DateTime CleaningDate { get; set; }
        public int? CleaningBy { get; set; }
        public CleaningStatus Status { get; set; }
        public string Notes { get; set; }
    }

    public class CleaningScheduleFilterDTO
    {
        public CleaningType? CleaningType { get; set; }
        public CleaningStatus? Status { get; set; }
        public int? CleaningBy { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}

