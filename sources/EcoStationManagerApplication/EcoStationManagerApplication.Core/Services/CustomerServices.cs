using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    // ==================== CUSTOMER SERVICE IMPLEMENTATION ====================
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID khách hàng không hợp lệ");
            var customer = await _customerRepository.GetByIdAsync(id);
            return customer ?? throw new KeyNotFoundException($"Không tìm thấy khách hàng với ID: {id}");
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _customerRepository.GetActiveCustomersAsync();
        }

        public async Task<Customer> CreateAsync(Customer entity)
        {
            ValidateCustomer(entity);

            // Check duplicate phone
            var existingByPhone = await _customerRepository.GetCustomerByPhoneAsync(entity.Phone);
            if (existingByPhone != null)
                throw new InvalidOperationException($"Số điện thoại {entity.Phone} đã tồn tại");

            // Set defaults
            entity.Status = "active";
            entity.TotalPoint = 0;
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;

            return await _customerRepository.CreateAsync(entity);
        }

        public async Task<Customer> UpdateAsync(Customer entity)
        {
            if (entity.CustomerId <= 0) throw new ArgumentException("ID khách hàng không hợp lệ");
            ValidateCustomer(entity);

            var existing = await _customerRepository.GetByIdAsync(entity.CustomerId);
            if (existing == null) throw new KeyNotFoundException($"Không tìm thấy khách hàng với ID: {entity.CustomerId}");

            // Check phone uniqueness
            if (existing.Phone != entity.Phone)
            {
                var customerWithPhone = await _customerRepository.GetCustomerByPhoneAsync(entity.Phone);
                if (customerWithPhone != null) throw new InvalidOperationException("Số điện thoại đã được sử dụng");
            }

            entity.UpdatedDate = DateTime.Now;
            return await _customerRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID khách hàng không hợp lệ");

            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null) throw new KeyNotFoundException($"Không tìm thấy khách hàng với ID: {id}");

            // Check if customer has orders
            var orderCount = await _customerRepository.GetCustomerOrderCountAsync(id);
            if (orderCount > 0) throw new InvalidOperationException("Không thể xóa khách hàng đã có đơn hàng");

            return await _customerRepository.DeleteAsync(id);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID khách hàng không hợp lệ");
            return await _customerRepository.UpdateCustomerStatusAsync(id, "inactive");
        }

        public async Task<Customer> GetCustomerByPhoneAsync(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) throw new ArgumentException("Số điện thoại không được trống");
            return await _customerRepository.GetCustomerByPhoneAsync(phone);
        }

        public async Task<Customer> GetCustomerByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email không được trống");
            return await _customerRepository.GetCustomerByEmailAsync(email);
        }

        public async Task<IEnumerable<Customer>> SearchCustomersAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword) || keyword.Length < 2)
                throw new ArgumentException("Từ khóa phải có ít nhất 2 ký tự");
            return await _customerRepository.SearchCustomersAsync(keyword);
        }

        public async Task<IEnumerable<Customer>> GetActiveCustomersAsync()
        {
            return await _customerRepository.GetActiveCustomersAsync();
        }

        public async Task<bool> UpdateCustomerPointsAsync(int customerId, int points)
        {
            if (customerId <= 0) throw new ArgumentException("ID khách hàng không hợp lệ");
            if (points < 0) throw new ArgumentException("Điểm không được âm");

            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null) throw new KeyNotFoundException($"Không tìm thấy khách hàng với ID: {customerId}");

            return await _customerRepository.UpdateCustomerPointsAsync(customerId, points);
        }

        public async Task<bool> UpdateCustomerStatusAsync(int customerId, string status)
        {
            if (customerId <= 0) throw new ArgumentException("ID khách hàng không hợp lệ");
            if (!IsValidCustomerStatus(status)) throw new ArgumentException("Trạng thái không hợp lệ");

            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null) throw new KeyNotFoundException($"Không tìm thấy khách hàng với ID: {customerId}");

            return await _customerRepository.UpdateCustomerStatusAsync(customerId, status);
        }

        public async Task<int> GetCustomerOrderCountAsync(int customerId)
        {
            if (customerId <= 0) throw new ArgumentException("ID khách hàng không hợp lệ");
            return await _customerRepository.GetCustomerOrderCountAsync(customerId);
        }

        public async Task<decimal> GetCustomerTotalSpentAsync(int customerId)
        {
            if (customerId <= 0) throw new ArgumentException("ID khách hàng không hợp lệ");
            return await _customerRepository.GetCustomerTotalSpentAsync(customerId);
        }

        public async Task<bool> AddCustomerPointsAsync(int customerId, int points)
        {
            if (customerId <= 0) throw new ArgumentException("ID khách hàng không hợp lệ");
            if (points <= 0) throw new ArgumentException("Điểm phải lớn hơn 0");

            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null) throw new KeyNotFoundException($"Không tìm thấy khách hàng với ID: {customerId}");

            var newPoints = customer.TotalPoint + points;
            return await _customerRepository.UpdateCustomerPointsAsync(customerId, newPoints);
        }

        public async Task<Dictionary<string, object>> GetCustomerStatisticsAsync(int customerId)
        {
            if (customerId <= 0) throw new ArgumentException("ID khách hàng không hợp lệ");

            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null) throw new KeyNotFoundException($"Không tìm thấy khách hàng với ID: {customerId}");

            var orderCount = await _customerRepository.GetCustomerOrderCountAsync(customerId);
            var totalSpent = await _customerRepository.GetCustomerTotalSpentAsync(customerId);

            return new Dictionary<string, object>
            {
                ["CustomerId"] = customerId,
                ["Name"] = customer.Name,
                ["TotalOrders"] = orderCount,
                ["TotalSpent"] = totalSpent,
                ["LoyaltyPoints"] = customer.TotalPoint,
                ["Status"] = customer.Status,
                ["MemberSince"] = customer.CreatedDate
            };
        }

        private void ValidateCustomer(Customer customer)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));
            if (string.IsNullOrWhiteSpace(customer.Name)) throw new ArgumentException("Tên không được trống");
            if (string.IsNullOrWhiteSpace(customer.Phone)) throw new ArgumentException("Số điện thoại không được trống");
        }

        private bool IsValidCustomerStatus(string status)
        {
            var validStatuses = new[] { "active", "inactive", "suspended" };
            return Array.Exists(validStatuses, s => s == status);
        }
    }

    // ==================== SUPPLIER SERVICE IMPLEMENTATION ====================
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierService(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<Supplier> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID nhà cung cấp không hợp lệ");
            var supplier = await _supplierRepository.GetByIdAsync(id);
            return supplier ?? throw new KeyNotFoundException($"Không tìm thấy nhà cung cấp với ID: {id}");
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            return await _supplierRepository.GetActiveSuppliersAsync();
        }

        public async Task<Supplier> CreateAsync(Supplier entity)
        {
            ValidateSupplier(entity);

            var existing = await _supplierRepository.GetSupplierByNameAsync(entity.Name);
            if (existing != null) throw new InvalidOperationException($"Tên nhà cung cấp {entity.Name} đã tồn tại");

            // Set defaults
            entity.IsActive = true;
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;

            return await _supplierRepository.CreateAsync(entity);
        }

        public async Task<Supplier> UpdateAsync(Supplier entity)
        {
            if (entity.SupplierId <= 0) throw new ArgumentException("ID nhà cung cấp không hợp lệ");
            ValidateSupplier(entity);

            var existing = await _supplierRepository.GetByIdAsync(entity.SupplierId);
            if (existing == null) throw new KeyNotFoundException($"Không tìm thấy nhà cung cấp với ID: {entity.SupplierId}");

            // Check name uniqueness
            if (existing.Name != entity.Name)
            {
                var supplierWithName = await _supplierRepository.GetSupplierByNameAsync(entity.Name);
                if (supplierWithName != null) throw new InvalidOperationException("Tên nhà cung cấp đã được sử dụng");
            }

            entity.UpdatedDate = DateTime.Now;
            return await _supplierRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID nhà cung cấp không hợp lệ");
            return await _supplierRepository.DeleteAsync(id);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID nhà cung cấp không hợp lệ");
            return await _supplierRepository.SoftDeleteAsync(id);
        }

        public async Task<IEnumerable<Supplier>> GetActiveSuppliersAsync()
        {
            return await _supplierRepository.GetActiveSuppliersAsync();
        }

        public async Task<Supplier> GetSupplierByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Tên nhà cung cấp không được trống");
            return await _supplierRepository.GetSupplierByNameAsync(name);
        }

        public async Task<IEnumerable<Supplier>> SearchSuppliersAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword) || keyword.Length < 2)
                throw new ArgumentException("Từ khóa phải có ít nhất 2 ký tự");
            return await _supplierRepository.SearchSuppliersAsync(keyword);
        }

        public async Task<int> GetSupplierProductCountAsync(int supplierId)
        {
            if (supplierId <= 0) throw new ArgumentException("ID nhà cung cấp không hợp lệ");
            return await _supplierRepository.GetSupplierProductCountAsync(supplierId);
        }

        public async Task<decimal> GetTotalPurchasesFromSupplierAsync(int supplierId)
        {
            if (supplierId <= 0) throw new ArgumentException("ID nhà cung cấp không hợp lệ");
            return await _supplierRepository.GetTotalPurchasesFromSupplierAsync(supplierId);
        }

        public async Task<bool> ValidateSupplierAsync(int supplierId)
        {
            if (supplierId <= 0) throw new ArgumentException("ID nhà cung cấp không hợp lệ");
            var supplier = await _supplierRepository.GetByIdAsync(supplierId);
            return supplier?.IsActive == true;
        }

        public async Task<bool> UpdateSupplierContactAsync(int supplierId, string contactPerson, string phone, string email)
        {
            if (supplierId <= 0) throw new ArgumentException("ID nhà cung cấp không hợp lệ");
            return await _supplierRepository.UpdateSupplierContactAsync(supplierId, contactPerson, phone, email);
        }

        public async Task<Dictionary<string, object>> GetSupplierPerformanceAsync(int supplierId)
        {
            if (supplierId <= 0) throw new ArgumentException("ID nhà cung cấp không hợp lệ");

            var supplier = await _supplierRepository.GetByIdAsync(supplierId);
            if (supplier == null) throw new KeyNotFoundException($"Không tìm thấy nhà cung cấp với ID: {supplierId}");

            var productCount = await _supplierRepository.GetSupplierProductCountAsync(supplierId);
            var totalPurchases = await _supplierRepository.GetTotalPurchasesFromSupplierAsync(supplierId);

            return new Dictionary<string, object>
            {
                ["SupplierId"] = supplierId,
                ["Name"] = supplier.Name,
                ["ProductsSupplied"] = productCount,
                ["TotalPurchases"] = totalPurchases,
                ["IsActive"] = supplier.IsActive,
                ["ContactPerson"] = supplier.ContactPerson
            };
        }

        private void ValidateSupplier(Supplier supplier)
        {
            if (supplier == null) throw new ArgumentNullException(nameof(supplier));
            if (string.IsNullOrWhiteSpace(supplier.Name)) throw new ArgumentException("Tên nhà cung cấp không được trống");
        }
    }
}