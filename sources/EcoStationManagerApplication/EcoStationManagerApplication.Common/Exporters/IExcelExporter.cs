using System.Collections.Generic;
using System.Data;

namespace EcoStationManagerApplication.Common.Exporters
{
    /// <summary>
    /// Interface cho Excel Exporter - Tầng Common
    /// </summary>
    public interface IExcelExporter
    {
        /// <summary>
        /// Export dữ liệu ra file Excel
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu cần export</typeparam>
        /// <param name="data">Danh sách dữ liệu</param>
        /// <param name="filePath">Đường dẫn file để lưu</param>
        /// <param name="worksheetName">Tên worksheet</param>
        /// <param name="headers">Tiêu đề các cột (tùy chọn)</param>
        void ExportToExcel<T>(IEnumerable<T> data, string filePath, string worksheetName = "Sheet1", Dictionary<string, string> headers = null);

        /// <summary>
        /// Export DataTable ra file Excel
        /// </summary>
        /// <param name="dataTable">DataTable cần export</param>
        /// <param name="filePath">Đường dẫn file để lưu</param>
        /// <param name="worksheetName">Tên worksheet</param>
        /// <param name="headers">Tiêu đề các cột (tùy chọn)</param>
        void ExportToExcel(DataTable dataTable, string filePath, string worksheetName = "Sheet1", Dictionary<string, string> headers = null);
    }
}

