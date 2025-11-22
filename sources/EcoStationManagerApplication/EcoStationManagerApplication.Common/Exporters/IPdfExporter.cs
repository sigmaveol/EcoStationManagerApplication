using System.Collections.Generic;
using System.Data;

namespace EcoStationManagerApplication.Common.Exporters
{
    /// <summary>
    /// Interface cho PDF Exporter - Tầng Common
    /// </summary>
    public interface IPdfExporter
    {
        /// <summary>
        /// Export dữ liệu ra file PDF
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu cần export</typeparam>
        /// <param name="data">Danh sách dữ liệu</param>
        /// <param name="filePath">Đường dẫn file để lưu</param>
        /// <param name="title">Tiêu đề tài liệu</param>
        /// <param name="headers">Tiêu đề các cột (tùy chọn)</param>
        void ExportToPdf<T>(IEnumerable<T> data, string filePath, string title = "Danh sách", Dictionary<string, string> headers = null);

        /// <summary>
        /// Export DataTable ra file PDF
        /// </summary>
        /// <param name="dataTable">DataTable cần export</param>
        /// <param name="filePath">Đường dẫn file để lưu</param>
        /// <param name="title">Tiêu đề tài liệu</param>
        /// <param name="headers">Tiêu đề các cột (tùy chọn)</param>
        void ExportToPdf(DataTable dataTable, string filePath, string title = "Danh sách", Dictionary<string, string> headers = null);
    }
}

