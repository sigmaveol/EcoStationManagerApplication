using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface ICustomerService
    {
        Task<Result<Customer>> GetCustomerByIdAsync(int customerId);
        Task<Result<List<Customer>>> GetAllCustomersAsync();
        Task<Result<List<Customer>>> GetActiveCustomersAsync();
        Task<Result<Customer>> CreateCustomerAsync(Customer customer);
        Task<Result<Customer>> UpdateCustomerAsync(Customer customer);
        Task<Result<bool>> DeleteCustomerAsync(int customerId);
        Task<Result<bool>> ToggleCustomerStatusAsync(int customerId, bool isActive);
        Task<Result<Customer>> GetCustomerByPhoneAsync(string phone);
        Task<Result<List<Customer>>> SearchCustomersAsync(string keyword);
        Task<Result<List<Customer>>> GetCustomersByRankAsync(CustomerRank rank);
        Task<Result<bool>> UpdateCustomerPointsAsync(int customerId, int pointsToAdd);
        Task<Result<bool>> UpdateCustomerRankAsync(int customerId, CustomerRank newRank);
        Task<Result<List<Customer>>> GetTopCustomersAsync(int limit = 10);
    }

    public interface ISupplierService
    {
        Task<Result<Supplier>> GetSupplierByIdAsync(int supplierId);
        Task<Result<List<Supplier>>> GetAllSuppliersAsync();
        Task<Result<Supplier>> CreateSupplierAsync(Supplier supplier);
        Task<Result<Supplier>> UpdateSupplierAsync(Supplier supplier);
        Task<Result<bool>> DeleteSupplierAsync(int supplierId);
        Task<Result<List<Supplier>>> SearchSuppliersAsync(string keyword);
        Task<Result<List<Supplier>>> GetSuppliersByContactPersonAsync(string contactPerson);
        Task<Result<bool>> ValidateSupplierAsync(Supplier supplier);
        Task<Result<bool>> IsEmailExistsAsync(string email, int? excludeSupplierId = null);
        Task<Result<bool>> IsPhoneExistsAsync(string phone, int? excludeSupplierId = null);
    }
}
