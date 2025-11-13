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
    [Table("Orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("order_id")]
        public int OrderId { get; set; }

        [Column("customer_id")]
        public int? CustomerId { get; set; }

        [StringLength(50)]
        [Column("order_code")]
        public string OrderCode { get; set; }

        [Required]
        [Column("source")]
        public OrderSource Source { get; set; } = OrderSource.MANUAL;

        [Required]
        [Column("total_amount", TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; } = 0;

        [Column("discounted_amount", TypeName = "decimal(10,2)")]
        public decimal DiscountedAmount { get; set; } = 0;

        [Required]
        [Column("status")]
        public OrderStatus Status { get; set; } = OrderStatus.DRAFT;

        [Required]
        [Column("payment_status")]
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.UNPAID;

        [Required]
        [Column("payment_method")]
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.CASH;

        [StringLength(255)]
        [Column("address")]
        public string Address { get; set; }

        [Column("note")]
        public string Note { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        [Column("last_updated")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        // Navigation property
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        // Navigation property for details
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }

    [Table("OrderDetails")]
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("order_detail_id")]
        public int OrderDetailId { get; set; }

        [Required]
        [Column("order_id")]
        public int OrderId { get; set; }

        [Required]
        [Column("product_id")]
        public int ProductId { get; set; } 

        [Required]
        [Column("quantity", TypeName = "decimal(10,2)")]
        public decimal Quantity { get; set; }

        [Required]
        [Column("unit_price", TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }

        // Navigation property
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        // Navigation property
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

    }
}
