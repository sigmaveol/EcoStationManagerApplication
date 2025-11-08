using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Lấy thông tin một bản ghi theo ID (khóa chính).
        /// </summary>
        /// <param name="id">ID của bản ghi cần truy vấn.</param>
        /// <returns>Đối tượng kiểu T, hoặc null nếu không tồn tại.</returns>
        Task<T> GetByIdAsync(int id);


        /// <summary>
        /// Lấy toàn bộ danh sách bản ghi trong bảng.
        /// </summary>
        /// <returns>Danh sách IEnumerable chứa các đối tượng kiểu T.</returns>
        Task<IEnumerable<T>> GetAllAsync();


        /// <summary>
        /// Thêm mới một bản ghi vào cơ sở dữ liệu.
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm.</param>
        /// <returns>ID (khóa chính) của bản ghi vừa được thêm.</returns>
        Task<int> AddAsync(T entity);


        /// <summary>
        /// Cập nhật thông tin của một bản ghi hiện có.
        /// </summary>
        /// <param name="entity">Đối tượng chứa dữ liệu đã chỉnh sửa.</param>
        /// <returns>True nếu cập nhật thành công, False nếu thất bại.</returns>
        Task<bool> UpdateAsync(T entity);


        /// <summary>
        /// Xóa một bản ghi khỏi cơ sở dữ liệu dựa vào ID.
        /// </summary>
        /// <param name="id">ID của bản ghi cần xóa.</param>
        /// <returns>True nếu xóa thành công, False nếu không tìm thấy hoặc lỗi.</returns>
        Task<bool> DeleteAsync(int id);


        /// <summary>
        /// Kiểm tra xem bản ghi có tồn tại trong cơ sở dữ liệu hay không.
        /// </summary>
        /// <param name="id">ID của bản ghi cần kiểm tra.</param>
        /// <returns>True nếu tồn tại, False nếu không.</returns>
        Task<bool> ExistsAsync(int id);

    }

    public interface IPagedRepository<T> where T : class
    {
        Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string search = null,
            string sortBy = null,
            bool sortDesc = false);
    }
}
