using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> GetByPhoneAsync(string phone);

        Task<IEnumerable<Customer>> SearchAsync(string keyword);

        Task<IEnumerable<Customer>> GetByRankAsync(string rank);

        Task<bool> UpdatePointsAsync(int customerId, int pointsToAdd);

        Task<bool> UpdateRankAsync(int customerId, string newRank);

        Task<bool> ToggleActiveAsync(int customerId, bool isActive);

        Task<IEnumerable<Customer>> GetTopCustomersAsync(int limit = 10);

        Task<(IEnumerable<Customer> Customers, int TotalCount)> GetPagedCustomersAsync(
            int pageNumber, int pageSize, string searchKeyword = null, string rank = null
        );
    }

    public interface ISupplierRepository : IRepository<Supplier>
    {
        /// <summary>
        /// Tìm nhà cung cấp theo tên hoặc số điện thoại
        /// </summary>
        Task<IEnumerable<Supplier>> SearchAsync(string keyword);

        /// <summary>
        /// Lấy danh sách nhà cung cấp theo người liên hệ
        /// </summary>
        Task<IEnumerable<Supplier>> GetByContactPersonAsync(string contactPerson);

        /// <summary>
        /// Kiểm tra email đã tồn tại chưa
        /// </summary>
        Task<bool> IsEmailExistsAsync(string email, int? excludeSupplierId = null);

        /// <summary>
        /// Kiểm tra số điện thoại đã tồn tại chưa
        /// </summary>
        Task<bool> IsPhoneExistsAsync(string phone, int? excludeSupplierId = null);

        /// <summary>
        /// Lấy tổng số nhà cung cấp
        /// </summary>
        Task<int> GetTotalSuppliersCountAsync();
    }
}
