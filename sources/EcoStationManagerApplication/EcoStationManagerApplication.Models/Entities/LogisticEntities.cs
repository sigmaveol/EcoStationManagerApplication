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
    [Table("PackagingTransactions")]
    public class PackagingTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("transaction_id")]
        public int TransactionId { get; set; }

        [Required]
        [Column("packaging_id")]
        public int PackagingId { get; set; }

        [Column("ref_product_id")]
        public int? RefProductId { get; set; }

        [Column("customer_id")]
        public int? CustomerId { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        [Required]
        [Column("type")]
        public PackagingTransactionType Type { get; set; }

        [Column("ownership_type")]
        public PackagingOwnershipType OwnershipType { get; set; } = PackagingOwnershipType.DEPOSIT;

        [Column("quantity")]
        public int Quantity { get; set; } = 1;

        [Column("deposit_price", TypeName = "decimal(10,2)")]
        public decimal DepositPrice { get; set; } = 0.00m;

        [Column("refund_amount", TypeName = "decimal(10,2)")]
        public decimal RefundAmount { get; set; } = 0.00m;

        [Column("notes")]
        public string Notes { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("PackagingId")]
        public virtual Packaging Packaging { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }


    [Table("DeliveryAssignments")]
    public class DeliveryAssignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("assignment_id")]
        public int AssignmentId { get; set; }

        [Required]
        [Column("order_id")]
        public int OrderId { get; set; }

        [Required]
        [Column("driver_id")]
        public int DriverId { get; set; }

        [Column("assigned_date")]
        public DateTime AssignedDate { get; set; } = DateTime.Now;

        [Required]
        [Column("status")]
        public DeliveryStatus Status { get; set; } = DeliveryStatus.PENDING;

        [Column("cod_amount", TypeName = "decimal(10,2)")]
        public decimal CodAmount { get; set; } = 0.00m;

        [Column("payment_status")]
        public DeliveryPaymentStatus PaymentStatus { get; set; } = DeliveryPaymentStatus.UNPAID;

        [Column("notes")]
        public string Notes { get; set; }

        // Navigation properties
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [ForeignKey("DriverId")]
        public virtual User Driver { get; set; }
    }
}
