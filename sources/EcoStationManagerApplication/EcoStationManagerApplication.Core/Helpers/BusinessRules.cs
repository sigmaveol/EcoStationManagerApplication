//using EcoStationManagerApplication.Models.Enums;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace EcoStationManagerApplication.Core.Helpers
//{
//    public static class BusinessRules
//    {
//        // ===== CUSTOMER RULES =====

//        /// <summary>
//        /// Tính điểm tích lũy dựa trên giá trị đơn hàng
//        /// </summary>
//        public static int CalculateLoyaltyPoints(decimal orderAmount)
//        {
//            // 1 điểm cho mỗi 10,000đ
//            return (int)Math.Floor(orderAmount / 10000);
//        }

//        /// <summary>
//        /// Xác định hạng khách hàng dựa trên tổng điểm
//        /// </summary>
//        public static CustomerRank DetermineCustomerRank(int totalPoints)
//        {
//            return totalPoints switch
//            {
//                >= 10000 => CustomerRank.DIAMONDS,    // 10,000+ điểm
//                >= 5000 => CustomerRank.GOLD,         // 5,000+ điểm
//                >= 1000 => CustomerRank.SILVER,       // 1,000+ điểm
//                _ => CustomerRank.MEMBER              // Dưới 1,000 điểm
//            };
//        }

//        /// <summary>
//        /// Tính tỷ lệ giảm giá theo hạng khách hàng
//        /// </summary>
//        public static decimal GetDiscountRateByRank(CustomerRank rank)
//        {
//            return rank switch
//            {
//                CustomerRank.DIAMONDS => 0.15m,   // 15%
//                CustomerRank.GOLD => 0.10m,       // 10%
//                CustomerRank.SILVER => 0.05m,     // 5%
//                _ => 0m                           // 0%
//            };
//        }

//        // ===== INVENTORY RULES =====

//        /// <summary>
//        /// Kiểm tra có nên cảnh báo tồn kho thấp không
//        /// </summary>
//        public static bool ShouldAlertLowStock(decimal currentStock, decimal minStockLevel)
//        {
//            return currentStock <= minStockLevel * 1.2m; // Cảnh báo khi còn 120% mức tối thiểu
//        }

//        /// <summary>
//        /// Xác định mức độ cảnh báo tồn kho
//        /// </summary>
//        public static StockAlertLevel GetStockAlertLevel(decimal currentStock, decimal minStockLevel)
//        {
//            if (currentStock <= 0) return StockAlertLevel.OUT_OF_STOCK;
//            if (currentStock <= minStockLevel * 0.5m) return StockAlertLevel.CRITICAL;
//            if (currentStock <= minStockLevel) return StockAlertLevel.LOW;
//            if (currentStock <= minStockLevel * 2) return StockAlertLevel.MEDIUM;
//            return StockAlertLevel.NORMAL;
//        }

//        /// <summary>
//        /// Kiểm tra sản phẩm sắp hết hạn
//        /// </summary>
//        public static bool IsProductExpiringSoon(DateTime? expiryDate, int daysThreshold = 15)
//        {
//            if (!expiryDate.HasValue) return false;
//            return (expiryDate.Value - DateTime.Today).TotalDays <= daysThreshold;
//        }

//        // ===== ORDER RULES =====

//        /// <summary>
//        /// Tính số tiền giảm giá tối đa cho đơn hàng
//        /// </summary>
//        public static decimal CalculateMaxDiscountAmount(decimal totalAmount, CustomerRank rank)
//        {
//            var discountRate = GetDiscountRateByRank(rank);
//            var maxDiscount = totalAmount * discountRate;

//            // Giới hạn tối đa 500,000đ
//            return Math.Min(maxDiscount, 500000);
//        }

//        /// <summary>
//        /// Kiểm tra có thể hủy đơn hàng không
//        /// </summary>
//        public static bool CanCancelOrder(OrderStatus status)
//        {
//            return status switch
//            {
//                OrderStatus.DRAFT => true,
//                OrderStatus.CONFIRMED => true,
//                OrderStatus.PROCESSING => true,
//                _ => false
//            };
//        }

//        /// <summary>
//        /// Kiểm tra có thể chỉnh sửa đơn hàng không
//        /// </summary>
//        public static bool CanEditOrder(OrderStatus status)
//        {
//            return status == OrderStatus.DRAFT || status == OrderStatus.CONFIRMED;
//        }

//        // ===== PACKAGING RULES =====

//        /// <summary>
//        /// Tính số tiền hoàn trả bao bì
//        /// </summary>
//        public static decimal CalculatePackagingRefund(decimal depositPrice, int quantity, bool isDamaged = false)
//        {
//            if (isDamaged)
//            {
//                // Hoàn 50% nếu bao bì hỏng
//                return depositPrice * quantity * 0.5m;
//            }

//            return depositPrice * quantity;
//        }

//        /// <summary>
//        /// Kiểm tra số lượng bao bì tối đa cho mỗi khách hàng
//        /// </summary>
//        public static bool ExceedsMaxPackagingPerCustomer(int currentHolding, int requested, CustomerRank rank)
//        {
//            var maxAllowed = rank switch
//            {
//                CustomerRank.DIAMONDS => 20,
//                CustomerRank.GOLD => 15,
//                CustomerRank.SILVER => 10,
//                _ => 5
//            };

//            return currentHolding + requested > maxAllowed;
//        }

//        // ===== CLEANING SCHEDULE RULES =====

//        /// <summary>
//        /// Tính ngày vệ sinh tiếp theo
//        /// </summary>
//        public static DateTime CalculateNextCleaningDate(CleaningType type, DateTime lastCleaning)
//        {
//            return type switch
//            {
//                CleaningType.TANK => lastCleaning.AddDays(30),     // 30 ngày cho bồn
//                CleaningType.PACKAGING => lastCleaning.AddDays(7), // 7 ngày cho bao bì
//                _ => lastCleaning.AddDays(15)                      // 15 ngày mặc định
//            };
//        }

//        /// <summary>
//        /// Kiểm tra có quá hạn vệ sinh không
//        /// </summary>
//        public static bool IsCleaningOverdue(DateTime scheduledDate, int graceDays = 2)
//        {
//            return DateTime.Today > scheduledDate.AddDays(graceDays);
//        }

//        // ===== PRICING RULES =====

//        /// <summary>
//        /// Tính giá bán dựa trên giá nhập và tỷ lệ lợi nhuận
//        /// </summary>
//        public static decimal CalculateSellingPrice(decimal costPrice, ProductType productType)
//        {
//            var profitMargin = productType switch
//            {
//                ProductType.PACKED => 0.3m,    // 30% cho sản phẩm đóng gói
//                ProductType.REFILLED => 0.4m,  // 40% cho sản phẩm refill
//                _ => 0.25m                     // 25% cho loại khác
//            };

//            return costPrice * (1 + profitMargin);
//        }

//        /// <summary>
//        /// Làm tròn giá theo quy tắc kinh doanh
//        /// </summary>
//        public static decimal RoundPrice(decimal price)
//        {
//            // Làm tròn đến 500đ gần nhất
//            return Math.Round(price / 500) * 500;
//        }

//        // ===== VALIDATION RULES =====

//        /// <summary>
//        /// Kiểm tra số lượng đặt hàng hợp lệ
//        /// </summary>
//        public static bool IsValidOrderQuantity(decimal quantity, ProductType productType)
//        {
//            var maxQuantity = productType switch
//            {
//                ProductType.PACKED => 1000,    // 1000 cho sản phẩm đóng gói
//                ProductType.REFILLED => 5000,  // 5000 cho sản phẩm refill (liters)
//                _ => 10000                     // 10000 cho loại khác
//            };

//            return quantity > 0 && quantity <= maxQuantity;
//        }

//        /// <summary>
//        /// Kiểm tra thời gian làm việc hợp lệ
//        /// </summary>
//        public static bool IsValidWorkingHours(TimeSpan startTime, TimeSpan endTime)
//        {
//            return startTime < endTime &&
//                   (endTime - startTime).TotalHours <= 12; // Tối đa 12 giờ
//        }

//        // ===== REPORTING RULES =====

//        /// <summary>
//        /// Phân loại thời gian cho báo cáo
//        /// </summary>
//        public static string GetTimePeriodLabel(DateTime date)
//        {
//            var today = DateTime.Today;

//            if (date.Date == today) return "Hôm nay";
//            if (date.Date == today.AddDays(-1)) return "Hôm qua";
//            if (date >= today.AddDays(-7)) return "7 ngày qua";
//            if (date >= today.AddDays(-30)) return "30 ngày qua";

//            return date.ToString("MM/yyyy");
//        }

//        /// <summary>
//        /// Tính hiệu suất nhân viên
//        /// </summary>
//        public static decimal CalculateKpiScore(int completedOrders, int totalOrders, decimal totalRevenue)
//        {
//            if (totalOrders == 0) return 0;

//            var completionRate = (decimal)completedOrders / totalOrders;
//            var revenueScore = totalRevenue / 1000000; // 1 điểm cho mỗi 1 triệu

//            return (completionRate * 0.6m + revenueScore * 0.4m) * 100;
//        }
//    }

//    public enum StockAlertLevel
//    {
//        NORMAL,
//        MEDIUM,
//        LOW,
//        CRITICAL,
//        OUT_OF_STOCK

//    }
//}
