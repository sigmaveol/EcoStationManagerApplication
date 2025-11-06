using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models.Entities
{
    public class Order
    {
        public int OrderId { get; set; }  // order_id INT PRIMARY KEY AUTO_INCREMENT

        public int? CustomerId { get; set; }  // customer_id INT
        public Customer Customer { get; set; }  // Navigation property

        public OrderSource Source { get; set; } = OrderSource.MANUAL; // source ENUM

        public decimal TotalAmount { get; set; } = 0; // total_amount DECIMAL(10,2)
        public decimal? DiscountedAmount { get; set; } = 0; // discounted_amount DECIMAL(10,2)

        public OrderStatus Status { get; set; } = OrderStatus.DRAFT; // status ENUM
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.UNPAID; // payment_status ENUM
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.CASH; // payment_method ENUM

        public string Address { get; set; } // address VARCHAR(255)
        public string Note { get; set; }    // note TEXT

        public int? UserId { get; set; }    // Người tạo đơn
        public User User { get; set; }      // Navigation property

        public DateTime LastUpdated { get; set; } = DateTime.Now; // last_updated

        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>(); // Chi tiết đơn hàng
    }

    public class OrderDetail
    {
        public int OrderDetailId { get; set; } // order_detail_id INT PRIMARY KEY AUTO_INCREMENT

        public int OrderId { get; set; }       // order_id INT NOT NULL
        public Order Order { get; set; }       // Navigation property

        public int? ProductId { get; set; }    // product_id INT
        public Product Product { get; set; }   // Navigation property

        public decimal? Quantity { get; set; } // quantity DECIMAL(10,2)
        public decimal? UnitPrice { get; set; } // unit_price DECIMAL(10,2)
    }
}
