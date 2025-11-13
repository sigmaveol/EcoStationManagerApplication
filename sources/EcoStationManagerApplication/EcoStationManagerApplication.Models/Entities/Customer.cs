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
    [Table("Customers")]
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("customer_id")]
        public int CustomerId { get; set; }

        [StringLength(50)]
        [Column("customer_code")]
        public string CustomerCode { get; set; }

        [Required]
        [StringLength(255)]
        [Column("name")]
        public string Name { get; set; }

        [StringLength(50)]
        [Column("phone")]
        public string Phone { get; set; }

        [Column("total_point")]
        public int TotalPoint { get; set; } = 0;

        [StringLength(20)]
        [Column("rank")]
        public string Rank { get; set; } = "MEMBER";

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

    [Table("Suppliers")]
    public class Supplier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("supplier_id")]
        public int SupplierId { get; set; }

        [Required]
        [StringLength(150)]
        [Column("name")]
        public string Name { get; set; }

        [StringLength(150)]
        [Column("contact_person")]
        public string ContactPerson { get; set; }

        [StringLength(30)]
        [Column("phone")]
        public string Phone { get; set; }

        [StringLength(150)]
        [Column("email")]
        public string Email { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
