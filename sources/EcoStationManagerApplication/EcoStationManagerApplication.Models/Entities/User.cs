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
}
