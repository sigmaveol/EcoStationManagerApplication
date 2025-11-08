using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Helpers
{
    public static class ValidationHelper
    {
        public static List<string> ValidateCategory(Category category)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(category.Name))
                errors.Add("Tên danh mục không được để trống");
            else if (category.Name.Length > 255)
                errors.Add("Tên danh mục không được vượt quá 255 ký tự");

            // Kiểm tra enum CategoryType hợp lệ
            if (!Enum.IsDefined(typeof(CategoryType), category.CategoryType))
                errors.Add("Loại danh mục không hợp lệ");

            return errors;
        }

        public static List<string> ValidateProduct(Product product)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(product.Name))
                errors.Add("Tên sản phẩm không được để trống");
            else if (product.Name.Length > 255)
                errors.Add("Tên sản phẩm không được vượt quá 255 ký tự");

            if (string.IsNullOrWhiteSpace(product.SKU))
                errors.Add("SKU không được để trống");
            else if (!IsValidSku(product.SKU))
                errors.Add("SKU không hợp lệ (chỉ cho phép chữ, số, gạch ngang)");

            if (string.IsNullOrWhiteSpace(product.Unit))
                errors.Add("Đơn vị tính không được để trống");

            if (product.Price < 0)
                errors.Add("Giá sản phẩm không được âm");

            if (product.MinStockLevel < 0)
                errors.Add("Mức tồn kho tối thiểu không được âm");

            // Kiểm tra enum ProductType hợp lệ
            if (!Enum.IsDefined(typeof(ProductType), product.ProductType))
                errors.Add("Loại sản phẩm không hợp lệ");

            return errors;
        }

        public static List<string> ValidatePackaging(Packaging packaging)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(packaging.Name))
                errors.Add("Tên bao bì không được để trống");

            if (packaging.Name?.Length > 150)
                errors.Add("Tên bao bì không được vượt quá 150 ký tự");

            if (string.IsNullOrWhiteSpace(packaging.Barcode))
                errors.Add("Barcode không được để trống");

            if (!IsValidBarcode(packaging.Barcode))
                errors.Add("Barcode không hợp lệ (chỉ cho phép chữ, số)");

            if (packaging.DepositPrice < 0)
                errors.Add("Giá ký quỹ không được âm");

            return errors;
        }

        private static bool IsValidSku(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku)) return false;
            return Regex.IsMatch(sku, @"^[a-zA-Z0-9\-_]+$");
        }

        private static bool IsValidBarcode(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode)) return false;
            return Regex.IsMatch(barcode, @"^[a-zA-Z0-9]+$");
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;
            return Regex.IsMatch(phone, @"^[0-9+\-\s()]{10,15}$");
        }
    }
}
