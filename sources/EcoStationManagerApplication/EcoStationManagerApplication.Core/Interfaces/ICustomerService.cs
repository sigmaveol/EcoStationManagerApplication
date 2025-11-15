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
        /// <summary>
        /// Tìm kiếm khách hàng theo tên hoặc số điện thoại. Nếu searchTerm rỗng, trả về tất cả khách hàng.
        /// </summary>
        Task<Result<IEnumerable<Customer>>> SearchCustomersAsync(string searchTerm);

        /// <summary>
        /// Lấy thông tin khách hàng theo ID
        /// </summary>
        Task<Result<Customer>> GetCustomerByIdAsync(int customerId);

        /// <summary>
        /// Lấy thông tin khách hàng theo mã khách hàng
        /// </summary>
        Task<Result<Customer>> GetCustomerByCodeAsync(string customerCode);

        /// <summary>
        /// Lấy khách hàng theo rank
        /// </summary>
        Task<Result<IEnumerable<Customer>>> GetCustomersByRankAsync(CustomerRank rank);

        /// <summary>
        /// Thêm khách hàng mới
        /// </summary>
        Task<Result<int>> CreateCustomerAsync(Customer customer);

        /// <summary>
        /// Cập nhật thông tin khách hàng
        /// </summary>
        Task<Result<bool>> UpdateCustomerAsync(Customer customer);

        /// <summary>
        /// Cập nhật điểm tích lũy cho khách hàng
        /// </summary>
        Task<Result<bool>> UpdateCustomerPointsAsync(int customerId, int points);

        /// <summary>
        /// Thêm điểm tích lũy cho khách hàng
        /// </summary>
        Task<Result<bool>> AddCustomerPointsAsync(int customerId, int points);

        /// <summary>
        /// Cập nhật rank cho khách hàng
        /// </summary>
        Task<Result<bool>> UpdateCustomerRankAsync(int customerId, CustomerRank rank);

        /// <summary>
        /// Kiểm tra số điện thoại đã tồn tại chưa
        /// </summary>
        Task<Result<bool>> CheckPhoneNumberExistsAsync(string phoneNumber);

        /// <summary>
        /// Kiểm tra mã khách hàng đã tồn tại chưa
        /// </summary>
        Task<Result<bool>> CheckCustomerCodeExistsAsync(string customerCode);
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
