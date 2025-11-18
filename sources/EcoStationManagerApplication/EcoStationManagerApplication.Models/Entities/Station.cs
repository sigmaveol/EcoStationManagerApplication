using EcoStationManagerApplication.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoStationManagerApplication.Models.Entities
{
    [Table("Stations")]
    public class Station
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("station_id")]
        public int StationId { get; set; }

        [StringLength(255)]
        [Column("name")]
        public string Name { get; set; }

        [Column("address", TypeName = "TEXT")]
        public string Address { get; set; }

        [StringLength(50)]
        [Column("phone")]
        public string Phone { get; set; }

        [StringLength(20)]
        [Column("station_type")]
        public string StationType { get; set; } = "refill"; // warehouse, refill, hybrid, other (ENUM)

        [Column("manager")]
        public int? Manager { get; set; } // User_id của quản lý trạm

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Column("updated_date")]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("Manager")]
        public virtual User ManagerUser { get; set; }
    }
}

