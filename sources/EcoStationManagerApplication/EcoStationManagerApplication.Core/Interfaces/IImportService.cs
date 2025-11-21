using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    /// <summary>
    /// Interface cho Import Service - xử lý nhập dữ liệu từ file Excel/CSV
    /// </summary>
    public interface IImportService
    {
        /// <summary>
        /// Import đơn hàng từ file Excel/CSV
        /// </summary>
        /// <param name="filePath">Đường dẫn file</param>
        /// <param name="source">Nguồn đơn hàng (EXCEL, EMAIL, GOOGLEFORM)</param>
        /// <returns>Kết quả import với danh sách đơn hàng đã tạo và lỗi (nếu có)</returns>
        Task<Result<ImportResult>> ImportOrdersFromFileAsync(string filePath, Models.Enums.OrderSource source);

        /// <summary>
        /// Import khách hàng từ file Excel/CSV
        /// </summary>
        Task<Result<ImportResult>> ImportCustomersFromFileAsync(string filePath);

        /// <summary>
        /// Validate file trước khi import
        /// </summary>
        Task<Result<bool>> ValidateImportFileAsync(string filePath);
    }

    /// <summary>
    /// Kết quả import
    /// </summary>
    public class ImportResult
    {
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<int> CreatedOrderIds { get; set; } = new List<int>();
        public List<int> CreatedCustomerIds { get; set; } = new List<int>();
    }
}

