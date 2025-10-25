using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    // ==================== CUSTOMER SERVICE INTERFACE ====================
    public interface ICustomerService : IService<Customer>
    {
        Task<Customer> GetCustomerByPhoneAsync(string phone);
        Task<Customer> GetCustomerByEmailAsync(string email);
        Task<IEnumerable<Customer>> SearchCustomersAsync(string keyword);
        Task<IEnumerable<Customer>> GetActiveCustomersAsync();
        Task<bool> UpdateCustomerPointsAsync(int customerId, int points);
        Task<bool> UpdateCustomerStatusAsync(int customerId, string status);
        Task<int> GetCustomerOrderCountAsync(int customerId);
        Task<decimal> GetCustomerTotalSpentAsync(int customerId);
        Task<bool> AddCustomerPointsAsync(int customerId, int points);
        Task<Dictionary<string, object>> GetCustomerStatisticsAsync(int customerId);
    }

    // ==================== SUPPLIER SERVICE INTERFACE ====================
    public interface ISupplierService : IService<Supplier>
    {
        Task<IEnumerable<Supplier>> GetActiveSuppliersAsync();
        Task<Supplier> GetSupplierByNameAsync(string name);
        Task<IEnumerable<Supplier>> SearchSuppliersAsync(string keyword);
        Task<int> GetSupplierProductCountAsync(int supplierId);
        Task<decimal> GetTotalPurchasesFromSupplierAsync(int supplierId);
        Task<bool> ValidateSupplierAsync(int supplierId);
        Task<bool> UpdateSupplierContactAsync(int supplierId, string contactPerson, string phone, string email);
        Task<Dictionary<string, object>> GetSupplierPerformanceAsync(int supplierId);
    }
}