using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.UI.Common
{
    public static class EnumHelper
    {
        /// <summary>
        /// Lấy tên hiển thị của UserRole
        /// </summary>
        public static string GetDisplayName(this UserRole role)
        {
            switch (role)
            {
                case UserRole.ADMIN:
                    return "Quản trị viên";
                case UserRole.STAFF:
                    return "Nhân viên";
                case UserRole.MANAGER:
                    return "Quản lý";
                case UserRole.DRIVER:
                    return "Tài xế";
                default:
                    return role.ToString();
            }
        }

        /// <summary>
        /// Lấy tên hiển thị của OrderSource
        /// </summary>
        public static string GetDisplayName(this OrderSource source)
        {
            switch (source)
            {
                case OrderSource.GOOGLEFORM:
                    return "Google Form";
                case OrderSource.EXCEL:
                    return "Excel";
                case OrderSource.EMAIL:
                    return "Email";
                case OrderSource.MANUAL:
                    return "Thủ công";
                default:
                    return source.ToString();
            }
        }

        /// <summary>
        /// Lấy tên hiển thị của OrderStatus
        /// </summary>
        public static string GetDisplayName(this OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.DRAFT:
                    return "Nháp";
                case OrderStatus.CONFIRMED:
                    return "Đã xác nhận";
                case OrderStatus.PROCESSING:
                    return "Đang xử lý";
                case OrderStatus.READY:
                    return "Sẵn sàng";
                case OrderStatus.SHIPPED:
                    return "Đang giao";
                case OrderStatus.COMPLETED:
                    return "Hoàn thành";
                case OrderStatus.CANCELLED:
                    return "Đã hủy";
                default:
                    return status.ToString();
            }
        }

        /// <summary>
        /// Lấy tên hiển thị của PaymentStatus
        /// </summary>
        public static string GetDisplayName(this PaymentStatus status)
        {
            switch (status)
            {
                case PaymentStatus.UNPAID:
                    return "Chưa thanh toán";
                case PaymentStatus.PAID:
                    return "Đã thanh toán";
                default:
                    return status.ToString();
            }
        }

        /// <summary>
        /// Lấy tên hiển thị của PaymentMethod
        /// </summary>
        public static string GetDisplayName(this PaymentMethod method)
        {
            switch (method)
            {
                case PaymentMethod.CASH:
                    return "Tiền mặt";
                case PaymentMethod.TRANSFER:
                    return "Chuyển khoản";
                default:
                    return method.ToString();
            }
        }

        /// <summary>
        /// Lấy tên hiển thị của CustomerRank
        /// </summary>
        public static string GetDisplayName(this CustomerRank rank)
        {
            switch (rank)
            {
                case CustomerRank.MEMBER:
                    return "Thành viên";
                case CustomerRank.SILVER:
                    return "Bạc";
                case CustomerRank.GOLD:
                    return "Vàng";
                case CustomerRank.DIAMONDS:
                    return "Kim cương";
                default:
                    return rank.ToString();
            }
        }

        /// <summary>
        /// Lấy tên hiển thị của CategoryType
        /// </summary>
        public static string GetDisplayName(this CategoryType type)
        {
            switch (type)
            {
                case CategoryType.PRODUCT:
                    return "Sản phẩm";
                case CategoryType.SERVICE:
                    return "Dịch vụ";
                case CategoryType.OTHER:
                    return "Khác";
                default:
                    return type.ToString();
            }
        }

        /// <summary>
        /// Lấy tên hiển thị của ProductType
        /// </summary>
        public static string GetDisplayName(this ProductType type)
        {
            switch (type)
            {
                case ProductType.PACKED:
                    return "Đóng gói";
                case ProductType.REFILLED:
                    return "Refill";
                case ProductType.OTHER:
                    return "Khác";
                default:
                    return type.ToString();
            }
        }

        /// <summary>
        /// Lấy tên hiển thị của RefType
        /// </summary>
        public static string GetDisplayName(this RefType refType)
        {
            switch (refType)
            {
                case RefType.PRODUCT:
                    return "Sản phẩm";
                case RefType.PACKAGING:
                    return "Bao bì";
                default:
                    return refType.ToString();
            }
        }

        /// <summary>
        /// Lấy tên hiển thị của StockOutPurpose
        /// </summary>
        public static string GetDisplayName(this StockOutPurpose purpose)
        {
            switch (purpose)
            {
                case StockOutPurpose.SALE:
                    return "Bán hàng";
                case StockOutPurpose.DAMAGE:
                    return "Hao hụt";
                case StockOutPurpose.TRANSFER:
                    return "Chuyển kho";
                default:
                    return purpose.ToString();
            }
        }

        /// <summary>
        /// Lấy tên hiển thị của CleaningType
        /// </summary>
        public static string GetDisplayName(this CleaningType type)
        {
            switch (type)
            {
                case CleaningType.TANK:
                    return "Bể chứa";
                case CleaningType.PACKAGING:
                    return "Bao bì";
                default:
                    return type.ToString();
            }
        }

        /// <summary>
        /// Lấy tên hiển thị của CleaningStatus
        /// </summary>
        public static string GetDisplayName(this CleaningStatus status)
        {
            switch (status)
            {
                case CleaningStatus.SCHEDULED:
                    return "Đã lên lịch";
                case CleaningStatus.COMPLETED:
                    return "Hoàn thành";
                case CleaningStatus.OVERDUE:
                    return "Quá hạn";
                case CleaningStatus.CANCELLED:
                    return "Đã hủy";
                default:
                    return status.ToString();
            }
        }

        /// <summary>
        /// Lấy tên hiển thị của PackagingTransactionType
        /// </summary>
        public static string GetDisplayName(this PackagingTransactionType transactionType)
        {
            switch (transactionType)
            {
                case PackagingTransactionType.ISSUE:
                    return "Phát bao bì";
                case PackagingTransactionType.RETURN:
                    return "Thu hồi bao bì";
                default:
                    return transactionType.ToString();
            }
        }

        /// <summary>
        /// Lấy tên hiển thị của PackagingOwnershipType
        /// </summary>
        public static string GetDisplayName(this PackagingOwnershipType ownershipType)
        {
            switch (ownershipType)
            {
                case PackagingOwnershipType.DEPOSIT:
                    return "Đặt cọc";
                case PackagingOwnershipType.SOLD:
                    return "Bán đứt";
                default:
                    return ownershipType.ToString();
            }
        }

        /// <summary>
        /// Lấy tên hiển thị của DeliveryStatus
        /// </summary>
        public static string GetDisplayName(this DeliveryStatus status)
        {
            switch (status)
            {
                case DeliveryStatus.PENDING:
                    return "Chờ xử lý";
                case DeliveryStatus.INTRANSIT:
                    return "Đang giao";
                case DeliveryStatus.DELIVERED:
                    return "Đã giao";
                case DeliveryStatus.FAILED:
                    return "Thất bại";
                default:
                    return status.ToString();
            }
        }

        /// <summary>
        /// Lấy tên hiển thị của DeliveryPaymentStatus
        /// </summary>
        public static string GetDisplayName(this DeliveryPaymentStatus status)
        {
            switch (status)
            {
                case DeliveryPaymentStatus.UNPAID:
                    return "Chưa thu tiền";
                case DeliveryPaymentStatus.PAID:
                    return "Đã thu tiền";
                default:
                    return status.ToString();
            }
        }

        /// <summary>
        /// Lấy tên hiển thị của ActiveStatus
        /// </summary>
        public static string GetDisplayName(this ActiveStatus status)
        {
            switch (status)
            {
                case ActiveStatus.INACTIVE:
                    return "Ngưng hoạt động";
                case ActiveStatus.ACTIVE:
                    return "Hoạt động";
                default:
                    return status.ToString();
            }
        }

        /// <summary>
        /// Lấy danh sách các giá trị enum dưới dạng key-value pairs cho ComboBox
        /// </summary>
        public static List<KeyValuePair<T, string>> GetEnumList<T>() where T : Enum
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            var list = new List<KeyValuePair<T, string>>();

            foreach (T enumValue in Enum.GetValues(typeof(T)))
            {
                string displayName = GetDisplayName(enumValue);
                list.Add(new KeyValuePair<T, string>(enumValue, displayName));
            }

            return list;
        }

        /// <summary>
        /// Lấy danh sách các giá trị enum với tên hiển thị cho ComboBox
        /// </summary>
        public static Dictionary<T, string> GetEnumDictionary<T>() where T : Enum
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            var dictionary = new Dictionary<T, string>();

            foreach (T enumValue in Enum.GetValues(typeof(T)))
            {
                string displayName = GetDisplayName(enumValue);
                dictionary.Add(enumValue, displayName);
            }

            return dictionary;
        }

        /// <summary>
        /// Lấy tên hiển thị cho enum value
        /// </summary>
        public static string GetDisplayName(Enum enumValue)
        {
            if (enumValue == null) return string.Empty;

            switch (enumValue)
            {
                case UserRole role:
                    return role.GetDisplayName();
                case OrderSource source:
                    return source.GetDisplayName();
                case OrderStatus status:
                    return status.GetDisplayName();
                case PaymentStatus paymentStatus:
                    return paymentStatus.GetDisplayName();
                case PaymentMethod paymentMethod:
                    return paymentMethod.GetDisplayName();
                case CustomerRank rank:
                    return rank.GetDisplayName();
                case CategoryType categoryType:
                    return categoryType.GetDisplayName();
                case ProductType productType:
                    return productType.GetDisplayName();
                case RefType refType:
                    return refType.GetDisplayName();
                case StockOutPurpose purpose:
                    return purpose.GetDisplayName();
                case CleaningType cleaningType:
                    return cleaningType.GetDisplayName();
                case CleaningStatus cleaningStatus:
                    return cleaningStatus.GetDisplayName();
                case PackagingTransactionType transactionType:
                    return transactionType.GetDisplayName();
                case PackagingOwnershipType ownershipType:
                    return ownershipType.GetDisplayName();
                case DeliveryStatus deliveryStatus:
                    return deliveryStatus.GetDisplayName();
                case DeliveryPaymentStatus paymentStatus:
                    return paymentStatus.GetDisplayName();
                case ActiveStatus activeStatus:
                    return activeStatus.GetDisplayName();
                default:
                    return enumValue.ToString();
            }
        }
    }
}
