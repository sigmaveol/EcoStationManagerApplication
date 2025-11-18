using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EcoStationManagerApplication.UI.Common
{
    /// <summary>
    /// Helper class để quản lý permissions cho từng UserRole
    /// </summary>
    public static class RolePermissionHelper
    {
        // Định nghĩa các permissions
        public const string ORDER_MANAGE = "order_manage";
        public const string INVENTORY_VIEW = "inventory_view";
        public const string INVENTORY_EDIT = "inventory_edit";
        public const string REPORT_VIEW = "report_view";
        public const string SETTINGS_EDIT = "settings_edit";
        public const string USER_MANAGE = "user_manage";
        public const string PRODUCT_MANAGE = "product_manage";
        public const string CUSTOMER_MANAGE = "customer_manage";
        public const string STOCK_IN = "stock_in";
        public const string STOCK_OUT = "stock_out";
        public const string PAYMENT_MANAGE = "payment_manage";

        /// <summary>
        /// Lấy danh sách permissions cho một role
        /// </summary>
        public static List<string> GetPermissions(UserRole role)
        {
            switch (role)
            {
                case UserRole.ADMIN:
                    return new List<string>
                    {
                        ORDER_MANAGE,
                        INVENTORY_VIEW,
                        INVENTORY_EDIT,
                        REPORT_VIEW,
                        SETTINGS_EDIT,
                        USER_MANAGE,
                        PRODUCT_MANAGE,
                        CUSTOMER_MANAGE,
                        STOCK_IN,
                        STOCK_OUT,
                        PAYMENT_MANAGE
                    };

                case UserRole.MANAGER:
                    return new List<string>
                    {
                        ORDER_MANAGE,
                        INVENTORY_VIEW,
                        INVENTORY_EDIT,
                        REPORT_VIEW,
                        PRODUCT_MANAGE,
                        CUSTOMER_MANAGE,
                        STOCK_IN,
                        STOCK_OUT,
                        PAYMENT_MANAGE
                    };

                case UserRole.STAFF:
                    return new List<string>
                    {
                        ORDER_MANAGE,
                        INVENTORY_VIEW,
                        PRODUCT_MANAGE,
                        CUSTOMER_MANAGE,
                        STOCK_OUT,
                        PAYMENT_MANAGE
                    };

                case UserRole.DRIVER:
                    return new List<string>
                    {
                        ORDER_MANAGE,
                        INVENTORY_VIEW,
                        REPORT_VIEW
                    };

                default:
                    return new List<string>();
            }
        }

        /// <summary>
        /// Kiểm tra xem role có permission cụ thể không
        /// </summary>
        public static bool HasPermission(UserRole role, string permission)
        {
            var permissions = GetPermissions(role);
            return permissions.Contains(permission);
        }

        /// <summary>
        /// Lấy tên hiển thị của role
        /// </summary>
        public static string GetRoleDisplayName(UserRole role)
        {
            switch (role)
            {
                case UserRole.ADMIN:
                    return "Quản trị viên";
                case UserRole.MANAGER:
                    return "Quản lý trạm";
                case UserRole.STAFF:
                    return "Nhân viên";
                case UserRole.DRIVER:
                    return "Tài xế";
                default:
                    return role.ToString();
            }
        }

        /// <summary>
        /// Lấy mô tả của role
        /// </summary>
        public static string GetRoleDescription(UserRole role)
        {
            switch (role)
            {
                case UserRole.ADMIN:
                    return "Toàn quyền truy cập hệ thống, có thể quản lý tất cả tính năng và người dùng";
                case UserRole.MANAGER:
                    return "Quản lý hoạt động trạm, có thể quản lý đơn hàng, kho, sản phẩm, khách hàng và xem báo cáo";
                case UserRole.STAFF:
                    return "Thao tác cơ bản: quản lý đơn hàng, xem kho, quản lý sản phẩm, khách hàng, xuất kho và thanh toán";
                case UserRole.DRIVER:
                    return "Xem đơn hàng, xem kho và xem báo cáo để thực hiện giao hàng";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả các features mà role có thể truy cập
        /// </summary>
        public static List<string> GetAccessibleFeatures(UserRole role)
        {
            var features = new List<string>();

            if (HasPermission(role, ORDER_MANAGE))
                features.Add("Quản lý đơn hàng");

            if (HasPermission(role, INVENTORY_VIEW))
                features.Add("Xem tồn kho");

            if (HasPermission(role, INVENTORY_EDIT))
                features.Add("Chỉnh sửa tồn kho");

            if (HasPermission(role, PRODUCT_MANAGE))
                features.Add("Quản lý sản phẩm & Bao bì");

            if (HasPermission(role, CUSTOMER_MANAGE))
                features.Add("Quản lý khách hàng");

            if (HasPermission(role, STOCK_IN))
                features.Add("Nhập kho");

            if (HasPermission(role, STOCK_OUT))
                features.Add("Xuất kho");

            if (HasPermission(role, PAYMENT_MANAGE))
                features.Add("Quản lý thanh toán");

            if (HasPermission(role, REPORT_VIEW))
                features.Add("Xem báo cáo");

            if (HasPermission(role, SETTINGS_EDIT))
                features.Add("Cài đặt hệ thống");

            if (HasPermission(role, USER_MANAGE))
                features.Add("Quản lý người dùng");

            return features;
        }
    }
}

