using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    // ==================== CUSTOMER REPOSITORY INTERFACE ====================
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> GetCustomerByPhoneAsync(string phone);
        Task<Customer> GetCustomerByEmailAsync(string email);
        Task<IEnumerable<Customer>> SearchCustomersAsync(string keyword);
        Task<IEnumerable<Customer>> GetActiveCustomersAsync();
        Task<bool> UpdateCustomerPointsAsync(int customerId, int points);
        Task<bool> UpdateCustomerStatusAsync(int customerId, string status);
        Task<int> GetCustomerOrderCountAsync(int customerId);
        Task<decimal> GetCustomerTotalSpentAsync(int customerId);
        Task<IEnumerable<Customer>> GetCustomersByStatusAsync(string status);
        Task<int> GetTotalCustomerCountAsync();
    }

    // ==================== SUPPLIER REPOSITORY INTERFACE ====================
    public interface ISupplierRepository : IRepository<Supplier>
    {
        Task<IEnumerable<Supplier>> GetActiveSuppliersAsync();
        Task<Supplier> GetSupplierByNameAsync(string name);
        Task<IEnumerable<Supplier>> SearchSuppliersAsync(string keyword);
        Task<int> GetSupplierProductCountAsync(int supplierId);
        Task<decimal> GetTotalPurchasesFromSupplierAsync(int supplierId);
        Task<IEnumerable<Supplier>> GetSuppliersByStatusAsync(bool isActive);
        Task<bool> UpdateSupplierContactAsync(int supplierId, string contactPerson, string phone, string email);
    }
}