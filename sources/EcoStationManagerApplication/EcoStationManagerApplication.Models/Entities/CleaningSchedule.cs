using EcoStationManagerApplication.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoStationManagerApplication.Models.Entities
{
    [Table("CleaningSchedules")]
    public class CleaningSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("cs_id")]
        public int CsId { get; set; }

        [Required]
        [Column("cleaning_type")]
        public CleaningType CleaningType { get; set; }

        [Required]
        [Column("cleaning_date")]
        public DateTime CleaningDate { get; set; }

        [Column("cleaning_by")]
        public int? CleaningBy { get; set; }

        [Column("status")]
        public CleaningStatus Status { get; set; } = CleaningStatus.SCHEDULED;

        [Column("cleaned_datetime")]
        public DateTime? CleanedDatetime { get; set; }

        [Column("notes", TypeName = "TEXT")]
        public string Notes { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("CleaningBy")]
        public virtual User CleaningUser { get; set; }
    }
}

