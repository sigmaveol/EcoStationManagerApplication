using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        [Column("username")]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        [Column("password_hash")]
        public string PasswordHash { get; set; }

        [StringLength(255)]
        [Column("fullname")]
        public string Fullname { get; set; }

        [Required]
        [StringLength(20)]
        [Column("role")]
        public UserRole Role { get; set; } = UserRole.STAFF;

        [Column("is_active")]
        public ActiveStatus IsActive { get; set; } = ActiveStatus.ACTIVE;

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

    [Table("WorkShifts")]
    public class WorkShift
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("shift_id")]
        public int ShiftId { get; set; }
        
        [Required]
        [Column("user_id")]
        public int UserId { get; set; }
        
        [Required]
        [Column("shift_date")]
        public DateTime ShiftDate { get; set; }
        
        [Column("start_time")]
        public TimeSpan? StartTime { get; set; }
        
        [Column("end_time")]
        public TimeSpan? EndTime { get; set; }
        
        [Column("kpi_score", TypeName = "decimal(5,2)")]
        public decimal? KpiScore { get; set; }
        
        [Column("notes")]
        public string Notes { get; set; }
        
        [Column("updated_date")]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        
        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }

    [Table("CleaningSchedules")]
    public class CleaningSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("cs_id")]
        public int CsId { get; set; }

        [Required]
        [StringLength(20)]
        [Column("cleaning_type")]
        public CleaningType CleaningType { get; set; } // TANK, PACKAGING

        [Required]
        [Column("cleaning_date")]
        public DateTime CleaningDate { get; set; }

        [StringLength(20)]
        [Column("status")]
        public CleaningStatus Status { get; set; } = CleaningStatus.SCHEDULED; // SCHEDULED, COMPLETED, OVERDUE, CANCELLED

        [Column("notes")]
        public string Notes { get; set; }

        [Column("cleaning_by")]
        public int? CleaningBy { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("CleaningBy")]
        public virtual User User { get; set; }
    }

}
