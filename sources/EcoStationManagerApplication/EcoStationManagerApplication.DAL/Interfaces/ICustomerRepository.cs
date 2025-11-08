using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        /// <summary>
        /// Lấy thông tin khách hàng theo số điện thoại.
        /// </summary>
        /// <param name="phone">Số điện thoại của khách hàng.</param>
        /// <returns>Đối tượng <see cref="Customer"/> tương ứng, hoặc null nếu không tìm thấy.</returns>
        Task<Customer> GetByPhoneAsync(string phone);

        /// <summary>
        /// Tìm kiếm khách hàng theo từ khóa (tên, email, số điện thoại...).
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm.</param>
        /// <returns>Danh sách khách hàng khớp với từ khóa.</returns>
        Task<IEnumerable<Customer>> SearchAsync(string keyword);

        /// <summary>
        /// Lấy danh sách khách hàng theo hạng thành viên (rank).
        /// </summary>
        /// <param name="rank">Hạng thành viên, ví dụ: MEMBER, SILVER, GOLD, DIAMONDS.</param>
        /// <returns>Danh sách khách hàng thuộc hạng chỉ định.</returns>
        Task<IEnumerable<Customer>> GetByRankAsync(string rank);

        /// <summary>
        /// Cập nhật điểm thưởng cho khách hàng.
        /// </summary>
        /// <param name="customerId">ID khách hàng cần cập nhật.</param>
        /// <param name="pointsToAdd">Số điểm cộng thêm (có thể âm nếu trừ điểm).</param>
        /// <returns>True nếu cập nhật thành công, ngược lại là false.</returns>
        Task<bool> UpdatePointsAsync(int customerId, int pointsToAdd);

        /// <summary>
        /// Cập nhật hạng thành viên (rank) cho khách hàng.
        /// </summary>
        /// <param name="customerId">ID khách hàng cần thay đổi hạng.</param>
        /// <param name="newRank">Tên hạng mới (ví dụ: SILVER, GOLD, DIAMONDS).</param>
        /// <returns>True nếu cập nhật thành công, ngược lại là false.</returns>
        Task<bool> UpdateRankAsync(int customerId, string newRank);

        /// <summary>
        /// Bật hoặc tắt trạng thái hoạt động của khách hàng.
        /// </summary>
        /// <param name="customerId">ID khách hàng cần thay đổi trạng thái.</param>
        /// <param name="isActive">True để kích hoạt, false để vô hiệu hóa.</param>
        /// <returns>True nếu cập nhật thành công, ngược lại là false.</returns>
        Task<bool> ToggleActiveAsync(int customerId, bool isActive);

        /// <summary>
        /// Lấy danh sách khách hàng có tổng điểm cao nhất.
        /// </summary>
        /// <param name="limit">Số lượng khách hàng tối đa cần lấy (mặc định = 10).</param>
        /// <returns>Danh sách khách hàng có điểm cao nhất.</returns>
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
