using System;

namespace EcoStationManagerApplication.Models.DTOs
{
    public class StationDTO
    {
        public int StationId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string StationType { get; set; }
        public int? Manager { get; set; }
        public string ManagerName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class CreateStationDTO
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string StationType { get; set; }
        public int? Manager { get; set; }
    }

    public class UpdateStationDTO
    {
        public int StationId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string StationType { get; set; }
        public int? Manager { get; set; }
        public bool IsActive { get; set; }
    }

    public class TankDTO
    {
        public int TankId { get; set; }
        public int StationId { get; set; }
        public string StationName { get; set; }
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
    }

    public class CreateTankDTO
    {
        public int StationId { get; set; }
        public string Name { get; set; }
        public string Material { get; set; }
        public decimal Capacity { get; set; }
        public string Unit { get; set; }
        public string Note { get; set; }
    }

    public class UpdateTankDTO
    {
        public int TankId { get; set; }
        public string Name { get; set; }
        public string Material { get; set; }
        public decimal Capacity { get; set; }
        public string Unit { get; set; }
        public decimal CurrentLevel { get; set; }
        public string Status { get; set; }
        public DateTime? LastCleanDate { get; set; }
        public DateTime? NextCleanDate { get; set; }
        public string Note { get; set; }
    }
}

