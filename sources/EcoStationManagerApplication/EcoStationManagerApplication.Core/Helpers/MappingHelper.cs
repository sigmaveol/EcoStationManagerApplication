using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Helpers
{
    public static class MappingHelper
    {
        // ===== ORDER MAPPINGS =====
        public static OrderDTO MapToOrderDTO(Order order)
        {
            if (order == null) return null;

            return new OrderDTO
            {
                OrderId = order.OrderId,
                OrderCode = order.OrderCode,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer?.Name,
                Source = order.Source,
                TotalAmount = order.TotalAmount,
                DiscountedAmount = order.DiscountedAmount,
                Status = order.Status,
                PaymentStatus = order.PaymentStatus,
                LastUpdated = order.LastUpdated,
                OrderDetails = order.OrderDetails?.Select(MapToOrderDetailDTO).ToList() ?? new List<OrderDetailDTO>()
            };
        }

        public static OrderDetailDTO MapToOrderDetailDTO(OrderDetail orderDetail)
        {
            if (orderDetail == null) return null;

            return new OrderDetailDTO
            {
                OrderDetailId = orderDetail.OrderDetailId,
                OrderId = orderDetail.OrderId,
                ProductId = orderDetail.ProductId,
                ProductName = orderDetail.Product?.Name,
                ProductSKU = orderDetail.Product?.Sku,
                Quantity = orderDetail.Quantity,
                UnitPrice = orderDetail.UnitPrice,
            };
        }

        //public static OrderSummaryDTO MapToOrderSummaryDTO(Order order)
        //{
        //    if (order == null) return null;

        //    return new OrderSummaryDTO
        //    {
        //        OrderId = order.OrderId,
        //        OrderCode = order.OrderCode,
        //        CustomerName = order.Customer?.Name,
        //        TotalAmount = order.TotalAmount,
        //        Status = order.Status,
        //        PaymentStatus = order.PaymentStatus,
        //        LastUpdated = order.LastUpdated
        //    };
        //}

        //// ===== PRODUCT MAPPINGS =====
        //public static ProductDTO MapToProductDTO(Product product)
        //{
        //    if (product == null) return null;

        //    return new ProductDTO
        //    {
        //        ProductId = product.ProductId,
        //        SKU = product.SKU,
        //        Name = product.Name,
        //        ProductType = product.ProductType,
        //        Unit = product.Unit,
        //        Price = product.Price,
        //        MinStockLevel = product.MinStockLevel,
        //        CategoryId = product.CategoryId,
        //        CategoryName = product.Category?.Name,
        //        IsActive = product.IsActive,
        //        CreatedDate = product.CreatedDate
        //    };
        //}

        //public static Product MapToProduct(ProductDTO dto)
        //{
        //    if (dto == null) return null;

        //    return new Product
        //    {
        //        ProductId = dto.ProductId,
        //        SKU = dto.SKU,
        //        Name = dto.Name,
        //        ProductType = dto.ProductType,
        //        Unit = dto.Unit,
        //        Price = dto.Price,
        //        MinStockLevel = dto.MinStockLevel,
        //        CategoryId = dto.CategoryId,
        //        IsActive = dto.IsActive,
        //        CreatedDate = dto.CreatedDate
        //    };
        //}

        //// ===== CUSTOMER MAPPINGS =====
        //public static CustomerDTO MapToCustomerDTO(Customer customer)
        //{
        //    if (customer == null) return null;

        //    return new CustomerDTO
        //    {
        //        CustomerId = customer.CustomerId,
        //        CustomerCode = customer.CustomerCode,
        //        Name = customer.Name,
        //        Phone = customer.Phone,
        //        Email = customer.Email,
        //        TotalPoint = customer.TotalPoint,
        //        Rank = customer.Rank,
        //        IsActive = customer.IsActive,
        //        CreatedDate = customer.CreatedDate
        //    };
        //}

        //public static Customer MapToCustomer(CustomerDTO dto)
        //{
        //    if (dto == null) return null;

        //    return new Customer
        //    {
        //        CustomerId = dto.CustomerId,
        //        CustomerCode = dto.CustomerCode,
        //        Name = dto.Name,
        //        Phone = dto.Phone,
        //        Email = dto.Email,
        //        TotalPoint = dto.TotalPoint,
        //        Rank = dto.Rank,
        //        IsActive = dto.IsActive,
        //        CreatedDate = dto.CreatedDate
        //    };
        //}

        //// ===== INVENTORY MAPPINGS =====
        //public static InventoryDTO MapToInventoryDTO(Inventory inventory)
        //{
        //    if (inventory == null) return null;

        //    return new InventoryDTO
        //    {
        //        InventoryId = inventory.InventoryId,
        //        BatchNo = inventory.BatchNo,
        //        ProductId = inventory.ProductId,
        //        ProductName = inventory.Product?.Name,
        //        Quantity = inventory.Quantity,
        //        ExpiryDate = inventory.ExpiryDate,
        //        LastUpdated = inventory.LastUpdated,
        //        IsExpired = inventory.ExpiryDate.HasValue && inventory.ExpiryDate.Value < DateTime.Today,
        //        DaysToExpiry = inventory.ExpiryDate.HasValue ?
        //            (inventory.ExpiryDate.Value - DateTime.Today).Days : (int?)null
        //    };
        //}

        //public static InventorySummaryDTO MapToInventorySummaryDTO(Inventory inventory, decimal minStockLevel)
        //{
        //    if (inventory == null) return null;

        //    var alertLevel = GetStockAlertLevel(inventory.Quantity, minStockLevel);

        //    return new InventorySummaryDTO
        //    {
        //        ProductId = inventory.ProductId,
        //        ProductName = inventory.Product?.Name,
        //        SKU = inventory.Product?.SKU,
        //        BatchNo = inventory.BatchNo,
        //        Quantity = inventory.Quantity,
        //        MinStockLevel = minStockLevel,
        //        AlertLevel = alertLevel,
        //        ExpiryDate = inventory.ExpiryDate
        //    };
        //}

        //// ===== PACKAGING MAPPINGS =====
        //public static PackagingDTO MapToPackagingDTO(Packaging packaging)
        //{
        //    if (packaging == null) return null;

        //    return new PackagingDTO
        //    {
        //        PackagingId = packaging.PackagingId,
        //        Barcode = packaging.Barcode,
        //        Name = packaging.Name,
        //        Type = packaging.Type,
        //        DepositPrice = packaging.DepositPrice
        //    };
        //}

        //public static PackagingInventoryDTO MapToPackagingInventoryDTO(PackagingInventory packagingInventory)
        //{
        //    if (packagingInventory == null) return null;

        //    return new PackagingInventoryDTO
        //    {
        //        PkInvId = packagingInventory.PkInvId,
        //        PackagingId = packagingInventory.PackagingId,
        //        PackagingName = packagingInventory.Packaging?.Name,
        //        QtyNew = packagingInventory.QtyNew,
        //        QtyInUse = packagingInventory.QtyInUse,
        //        QtyReturned = packagingInventory.QtyReturned,
        //        QtyNeedCleaning = packagingInventory.QtyNeedCleaning,
        //        QtyCleaned = packagingInventory.QtyCleaned,
        //        QtyDamaged = packagingInventory.QtyDamaged,
        //        LastUpdated = packagingInventory.LastUpdated,
        //        TotalAvailable = packagingInventory.QtyNew + packagingInventory.QtyCleaned
        //    };
        //}

        // ===== USER MAPPINGS =====
        public static UserDTO MapToUserDTO(User user)
        {
            if (user == null) return null;

            return new UserDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                Fullname = user.Fullname,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedDate = user.CreatedDate
            };
        }

        public static User MapToUser(UserDTO dto)
        {
            if (dto == null) return null;

            return new User
            {
                UserId = dto.UserId,
                Username = dto.Username,
                Fullname = dto.Fullname,
                Role = dto.Role,
                IsActive = dto.IsActive,
                CreatedDate = dto.CreatedDate
            };
        }

        //// ===== DASHBOARD MAPPINGS =====
        //public static DashboardStatsDTO MapToDashboardStatsDTO(
        //    decimal totalRevenue,
        //    int totalOrders,
        //    int totalCustomers,
        //    int lowStockItems)
        //{
        //    return new DashboardStatsDTO
        //    {
        //        TotalRevenue = totalRevenue,
        //        TotalOrders = totalOrders,
        //        TotalCustomers = totalCustomers,
        //        LowStockItems = lowStockItems,
        //        AverageOrderValue = totalOrders > 0 ? totalRevenue / totalOrders : 0
        //    };
        //}

        //public static ChartDataDTO MapToChartDataDTO(string label, decimal value)
        //{
        //    return new ChartDataDTO
        //    {
        //        Label = label,
        //        Value = value
        //    };
        //}

        //// ===== HELPER METHODS =====
        //private static string GetStockAlertLevel(decimal currentStock, decimal minStockLevel)
        //{
        //    if (currentStock <= 0) return "OUT_OF_STOCK";
        //    if (currentStock <= minStockLevel) return "LOW";
        //    if (currentStock <= minStockLevel * 2) return "MEDIUM";
        //    return "NORMAL";
        //}

        //// Bulk mapping methods
        //public static List<OrderDTO> MapToOrderDTOList(IEnumerable<Order> orders)
        //{
        //    return orders?.Select(MapToOrderDTO).ToList() ?? new List<OrderDTO>();
        //}

        //public static List<ProductDTO> MapToProductDTOList(IEnumerable<Product> products)
        //{
        //    return products?.Select(MapToProductDTO).ToList() ?? new List<ProductDTO>();
        //}

        //public static List<CustomerDTO> MapToCustomerDTOList(IEnumerable<Customer> customers)
        //{
        //    return customers?.Select(MapToCustomerDTO).ToList() ?? new List<CustomerDTO>();
        //}

        //public static List<InventoryDTO> MapToInventoryDTOList(IEnumerable<Inventory> inventories)
        //{
        //    return inventories?.Select(MapToInventoryDTO).ToList() ?? new List<InventoryDTO>();
        //}

        //// Map for updates (ignore null properties)
        //public static void MapForUpdate(Product source, Product target)
        //{
        //    if (source == null || target == null) return;

        //    if (!string.IsNullOrEmpty(source.Name)) target.Name = source.Name;
        //    if (!string.IsNullOrEmpty(source.SKU)) target.SKU = source.SKU;
        //    if (source.Price >= 0) target.Price = source.Price;
        //    if (source.MinStockLevel >= 0) target.MinStockLevel = source.MinStockLevel;
        //    if (source.CategoryId.HasValue) target.CategoryId = source.CategoryId;
        //    target.IsActive = source.IsActive;
        //}

        //public static void MapForUpdate(Customer source, Customer target)
        //{
        //    if (source == null || target == null) return;

        //    if (!string.IsNullOrEmpty(source.Name)) target.Name = source.Name;
        //    if (!string.IsNullOrEmpty(source.Phone)) target.Phone = source.Phone;
        //    if (!string.IsNullOrEmpty(source.Email)) target.Email = source.Email;
        //    if (source.TotalPoint >= 0) target.TotalPoint = source.TotalPoint;
        //    target.Rank = source.Rank;
        //    target.IsActive = source.IsActive;
        //}
    }
}
