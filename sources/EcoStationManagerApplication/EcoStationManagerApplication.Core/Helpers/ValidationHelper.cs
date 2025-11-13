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

        public static List<string> ValidateOrderDetail(OrderDetail orderDetail)
        {
            var errors = new List<string>();

            if (orderDetail.OrderId <= 0)
                errors.Add("ID đơn hàng không hợp lệ");

            if (orderDetail.ProductId <= 0)
                errors.Add("ID sản phẩm không hợp lệ");

            if (orderDetail.Quantity <= 0)
                errors.Add("Số lượng phải lớn hơn 0");

            if (orderDetail.UnitPrice < 0)
                errors.Add("Đơn giá không được âm");

            if (orderDetail.Quantity > 1000000) // Giới hạn thực tế
                errors.Add("Số lượng quá lớn");

            if (orderDetail.UnitPrice > 1000000000) // Giới hạn 1 tỷ
                errors.Add("Đơn giá quá lớn");

            return errors;
        }

        public static List<string> ValidateOrderDetailForUpdate(OrderDetail orderDetail)
        {
            var errors = ValidateOrderDetail(orderDetail);

            if (orderDetail.OrderDetailId <= 0)
                errors.Add("ID chi tiết đơn hàng không hợp lệ");

            return errors;
        }

        public static List<string> ValidateOrder(Order order)
        {
            var errors = new List<string>();

            if (order.TotalAmount < 0)
                errors.Add("Tổng tiền không được âm");

            if (order.DiscountedAmount < 0)
                errors.Add("Số tiền giảm giá không được âm");

            if (order.DiscountedAmount > order.TotalAmount)
                errors.Add("Số tiền giảm giá không được lớn hơn tổng tiền");

            if (order.CustomerId.HasValue && order.CustomerId <= 0)
                errors.Add("ID khách hàng không hợp lệ");

            if (order.UserId <= 0)
                errors.Add("ID người tạo không hợp lệ");

            if (!string.IsNullOrEmpty(order.Address) && order.Address.Length > 500)
                errors.Add("Địa chỉ không được vượt quá 500 ký tự");

            if (!string.IsNullOrEmpty(order.Note) && order.Note.Length > 1000)
                errors.Add("Ghi chú không được vượt quá 1000 ký tự");

            return errors;
        }

        public static List<string> ValidateCustomer(Customer customer)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(customer.Name))
                errors.Add("Tên khách hàng không được để trống");

            if (customer.Name?.Length > 255)
                errors.Add("Tên khách hàng không được vượt quá 255 ký tự");

            if (!string.IsNullOrWhiteSpace(customer.Phone) && !IsValidPhone(customer.Phone))
                errors.Add("Số điện thoại không hợp lệ");

            if (customer.TotalPoint < 0)
                errors.Add("Điểm tích lũy không được âm");

            return errors;
        }

        public static List<string> ValidateSupplier(Supplier supplier)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(supplier.Name))
                errors.Add("Tên nhà cung cấp không được để trống");

            if (supplier.Name?.Length > 150)
                errors.Add("Tên nhà cung cấp không được vượt quá 150 ký tự");

            if (!string.IsNullOrWhiteSpace(supplier.Phone) && !IsValidPhone(supplier.Phone))
                errors.Add("Số điện thoại nhà cung cấp không hợp lệ");

            if (!string.IsNullOrWhiteSpace(supplier.Email) && !IsValidEmail(supplier.Email))
                errors.Add("Email nhà cung cấp không hợp lệ");

            return errors;
        }

        public static List<string> ValidatePackagingTransaction(PackagingTransaction transaction)
        {
            var errors = new List<string>();

            if (transaction.PackagingId <= 0)
                errors.Add("ID bao bì không hợp lệ");

            if (transaction.Quantity <= 0)
                errors.Add("Số lượng phải lớn hơn 0");

            if (transaction.DepositPrice < 0)
                errors.Add("Giá ký quỹ không được âm");

            if (transaction.RefundAmount < 0)
                errors.Add("Số tiền hoàn trả không được âm");

            return errors;
        }

        public static List<string> ValidateInventory(Inventory inventory)
        {
            var errors = new List<string>();

            if (inventory.ProductId <= 0)
                errors.Add("ID sản phẩm không hợp lệ");

            if (inventory.Quantity < 0)
                errors.Add("Số lượng tồn kho không được âm");

            if (!string.IsNullOrWhiteSpace(inventory.BatchNo) && inventory.BatchNo.Length > 100)
                errors.Add("Số lô không được vượt quá 100 ký tự");

            if (inventory.ExpiryDate.HasValue && inventory.ExpiryDate.Value < DateTime.Today)
                errors.Add("Hạn sử dụng không được ở trong quá khứ");

            return errors;
        }

        public static List<string> ValidateStockOperation(int productId, string batchNo, decimal quantity)
        {
            var errors = new List<string>();

            if (productId <= 0)
                errors.Add("ID sản phẩm không hợp lệ");

            if (string.IsNullOrWhiteSpace(batchNo))
                errors.Add("Số lô không được để trống");

            if (batchNo?.Length > 100)
                errors.Add("Số lô không được vượt quá 100 ký tự");

            if (quantity <= 0)
                errors.Add("Số lượng phải lớn hơn 0");

            if (quantity > 1000000) // Giới hạn thực tế
                errors.Add("Số lượng quá lớn");

            return errors;
        }

        public static List<string> ValidateQuantityUpdate(int inventoryId, decimal newQuantity)
        {
            var errors = new List<string>();

            if (inventoryId <= 0)
                errors.Add("ID tồn kho không hợp lệ");

            if (newQuantity < 0)
                errors.Add("Số lượng tồn kho không được âm");

            if (newQuantity > 1000000)
                errors.Add("Số lượng quá lớn");

            return errors;
        }

        public static List<string> ValidateStockIn(StockIn stockIn)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(stockIn.BatchNo))
                errors.Add("Số lô không được để trống");

            if (stockIn.BatchNo?.Length > 100)
                errors.Add("Số lô không được vượt quá 100 ký tự");

            if (stockIn.RefId <= 0)
                errors.Add("ID tham chiếu không hợp lệ");

            if (stockIn.Quantity <= 0)
                errors.Add("Số lượng nhập phải lớn hơn 0");

            if (stockIn.UnitPrice < 0)
                errors.Add("Đơn giá không được âm");

            if (stockIn.CreatedBy <= 0)
                errors.Add("ID người tạo không hợp lệ");

            if (stockIn.ExpiryDate.HasValue && stockIn.ExpiryDate.Value < DateTime.Today)
                errors.Add("Hạn sử dụng không được ở trong quá khứ");

            return errors;
        }

        public static List<string> ValidateStockOut(StockOut stockOut)
        {
            var errors = new List<string>();

            if (stockOut.RefId <= 0)
                errors.Add("ID tham chiếu không hợp lệ");

            if (stockOut.Quantity <= 0)
                errors.Add("Số lượng xuất phải lớn hơn 0");

            if (stockOut.CreatedBy <= 0)
                errors.Add("ID người tạo không hợp lệ");

            return errors;
        }

        public static List<string> ValidateUser(User user)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(user.Username))
                errors.Add("Tên đăng nhập không được để trống");

            if (user.Username?.Length > 100)
                errors.Add("Tên đăng nhập không được vượt quá 100 ký tự");

            if (!IsValidUsername(user.Username))
                errors.Add("Tên đăng nhập không hợp lệ (chỉ cho phép chữ, số, gạch dưới)");

            if (string.IsNullOrWhiteSpace(user.PasswordHash))
                errors.Add("Mật khẩu không được để trống");

            if (string.IsNullOrWhiteSpace(user.Fullname))
                errors.Add("Họ tên không được để trống");

            if (user.Fullname?.Length > 255)
                errors.Add("Họ tên không được vượt quá 255 ký tự");

            return errors;
        }

        public static List<string> ValidateOrderQuantity(decimal quantity, decimal availableStock)
        {
            var errors = new List<string>();

            if (quantity <= 0)
                errors.Add("Số lượng phải lớn hơn 0");

            if (quantity > availableStock)
                errors.Add($"Số lượng vượt quá tồn kho hiện có ({availableStock})");

            if (quantity > 10000) // Giới hạn business
                errors.Add("Số lượng quá lớn cho một đơn hàng");

            return errors;
        }

        public static List<string> ValidatePrice(decimal price)
        {
            var errors = new List<string>();

            if (price < 0)
                errors.Add("Giá không được âm");

            if (price > 1000000000) // 1 tỷ
                errors.Add("Giá quá lớn");

            return errors;
        }

        public static bool IsValidDateRange(DateTime startDate, DateTime endDate)
        {
            return startDate <= endDate && startDate >= DateTime.MinValue && endDate <= DateTime.MaxValue;
        }

        public static List<string> ValidateDateRange(DateTime startDate, DateTime endDate)
        {
            var errors = new List<string>();

            if (startDate > endDate)
                errors.Add("Ngày bắt đầu không được sau ngày kết thúc");

            if (startDate < DateTime.MinValue || endDate > DateTime.MaxValue)
                errors.Add("Khoảng thời gian không hợp lệ");

            if ((endDate - startDate).TotalDays > 365)
                errors.Add("Khoảng thời gian không được vượt quá 1 năm");

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

        private static bool IsValidUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return false;
            return Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$");
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
