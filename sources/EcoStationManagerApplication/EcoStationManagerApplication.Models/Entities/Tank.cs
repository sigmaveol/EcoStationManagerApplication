using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoStationManagerApplication.Models.Entities
{
    [Table("Tanks")]
    public class Tank
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("tank_id")]
        public int TankId { get; set; }

        [Required]
        [Column("station_id")]
        public int StationId { get; set; }

        [Required]
        [StringLength(100)]
        [Column("name")]
        public string Name { get; set; }

        [Column("material")]
        public string Material { get; set; } = "plastic"; // glass, plastic, metal

        [Required]
        [Column("capacity", TypeName = "decimal(10,2)")]
        public decimal Capacity { get; set; }

        [Column("unit")]
        public string Unit { get; set; } = "liter"; // ml, liter, kg

        [Column("current_level", TypeName = "decimal(10,2)")]
        public decimal CurrentLevel { get; set; } = 0;

        [Column("status")]
        public string Status { get; set; } = "active"; // active, maintenance, outoforder

        [Column("last_clean_date")]
        public DateTime? LastCleanDate { get; set; }

        [Column("next_clean_date")]
        public DateTime? NextCleanDate { get; set; }

        [Column("note", TypeName = "TEXT")]
        public string Note { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation property
        [ForeignKey("StationId")]
        public virtual Station Station { get; set; }
    }
}

