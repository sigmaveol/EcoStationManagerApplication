using Dapper;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Database;
using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{
    // ==================== CUSTOMER REPOSITORY IMPLEMENTATION ====================
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository() : base("Customers", "customer_id", true) { }
        public CustomerRepository(IDbHelper dbHelper) : base(dbHelper, "Customers", "customer_id", true) { }

        public async Task<Customer> GetCustomerByPhoneAsync(string phone)
        {
            var sql = "SELECT * FROM Customers WHERE phone = @Phone AND status = 'active'";
            return await _dbHelper.QueryFirstOrDefaultAsync<Customer>(sql, new { Phone = phone });
        }

        public async Task<Customer> GetCustomerByEmailAsync(string email)
        {
            var sql = "SELECT * FROM Customers WHERE email = @Email AND status = 'active'";
            return await _dbHelper.QueryFirstOrDefaultAsync<Customer>(sql, new { Email = email });
        }

        public async Task<IEnumerable<Customer>> SearchCustomersAsync(string keyword)
        {
            var sql = @"SELECT * FROM Customers 
                       WHERE (name LIKE @Keyword OR phone LIKE @Keyword OR email LIKE @Keyword)
                       AND status = 'active'
                       ORDER BY name";
            return await _dbHelper.QueryAsync<Customer>(sql, new { Keyword = $"%{keyword}%" });
        }

        public async Task<IEnumerable<Customer>> GetActiveCustomersAsync()
        {
            var sql = "SELECT * FROM Customers WHERE status = 'active' ORDER BY name";
            return await _dbHelper.QueryAsync<Customer>(sql);
        }

        public async Task<bool> UpdateCustomerPointsAsync(int customerId, int points)
        {
            var sql = "UPDATE Customers SET total_point = @Points WHERE customer_id = @CustomerId";
            var result = await _dbHelper.ExecuteAsync(sql, new { CustomerId = customerId, Points = points });
            return result > 0;
        }

        public async Task<bool> UpdateCustomerStatusAsync(int customerId, string status)
        {
            var sql = "UPDATE Customers SET status = @Status WHERE customer_id = @CustomerId";
            var result = await _dbHelper.ExecuteAsync(sql, new { CustomerId = customerId, Status = status });
            return result > 0;
        }

        public async Task<int> GetCustomerOrderCountAsync(int customerId)
        {
            var sql = "SELECT COUNT(*) FROM Orders WHERE customer_id = @CustomerId AND is_active = 1";
            return await _dbHelper.ExecuteScalarAsync<int>(sql, new { CustomerId = customerId });
        }

        public async Task<decimal> GetCustomerTotalSpentAsync(int customerId)
        {
            var sql = @"SELECT COALESCE(SUM(total_amount), 0) FROM Orders 
                       WHERE customer_id = @CustomerId AND status = 'completed' AND is_active = 1";
            return await _dbHelper.ExecuteScalarAsync<decimal>(sql, new { CustomerId = customerId });
        }

        public async Task<IEnumerable<Customer>> GetCustomersByStatusAsync(string status)
        {
            var sql = "SELECT * FROM Customers WHERE status = @Status ORDER BY name";
            return await _dbHelper.QueryAsync<Customer>(sql, new { Status = status });
        }

        public async Task<int> GetTotalCustomerCountAsync()
        {
            var sql = "SELECT COUNT(*) FROM Customers WHERE status = 'active'";
            return await _dbHelper.ExecuteScalarAsync<int>(sql);
        }
    }

    // ==================== SUPPLIER REPOSITORY IMPLEMENTATION ====================
    public class SupplierRepository : BaseRepository<Supplier>, ISupplierRepository
    {
        public SupplierRepository() : base("Suppliers", "supplier_id", true) { }
        public SupplierRepository(IDbHelper dbHelper) : base(dbHelper, "Suppliers", "supplier_id", true) { }

        public async Task<IEnumerable<Supplier>> GetActiveSuppliersAsync()
        {
            var sql = "SELECT * FROM Suppliers WHERE is_active = 1 ORDER BY name";
            return await _dbHelper.QueryAsync<Supplier>(sql);
        }

        public async Task<Supplier> GetSupplierByNameAsync(string name)
        {
            var sql = "SELECT * FROM Suppliers WHERE name = @Name AND is_active = 1";
            return await _dbHelper.QueryFirstOrDefaultAsync<Supplier>(sql, new { Name = name });
        }

        public async Task<IEnumerable<Supplier>> SearchSuppliersAsync(string keyword)
        {
            var sql = @"SELECT * FROM Suppliers 
                       WHERE (name LIKE @Keyword OR contact_person LIKE @Keyword OR email LIKE @Keyword)
                       AND is_active = 1
                       ORDER BY name";
            return await _dbHelper.QueryAsync<Supplier>(sql, new { Keyword = $"%{keyword}%" });
        }

        public async Task<int> GetSupplierProductCountAsync(int supplierId)
        {
            var sql = "SELECT COUNT(DISTINCT variant_id) FROM StockIn WHERE supplier_id = @SupplierId";
            return await _dbHelper.ExecuteScalarAsync<int>(sql, new { SupplierId = supplierId });
        }

        public async Task<decimal> GetTotalPurchasesFromSupplierAsync(int supplierId)
        {
            var sql = @"SELECT COALESCE(SUM(quantity * COALESCE(unit_price, 0)), 0) 
                       FROM StockIn WHERE supplier_id = @SupplierId";
            return await _dbHelper.ExecuteScalarAsync<decimal>(sql, new { SupplierId = supplierId });
        }

        public async Task<IEnumerable<Supplier>> GetSuppliersByStatusAsync(bool isActive)
        {
            var sql = "SELECT * FROM Suppliers WHERE is_active = @IsActive ORDER BY name";
            return await _dbHelper.QueryAsync<Supplier>(sql, new { IsActive = isActive });
        }

        public async Task<bool> UpdateSupplierContactAsync(int supplierId, string contactPerson, string phone, string email)
        {
            var sql = @"UPDATE Suppliers 
                       SET contact_person = @ContactPerson, phone = @Phone, email = @Email 
                       WHERE supplier_id = @SupplierId";

            var result = await _dbHelper.ExecuteAsync(sql, new
            {
                SupplierId = supplierId,
                ContactPerson = contactPerson,
                Phone = phone,
                Email = email
            });
            return result > 0;
        }
    }
}