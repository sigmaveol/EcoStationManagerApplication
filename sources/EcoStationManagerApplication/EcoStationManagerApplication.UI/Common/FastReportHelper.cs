using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using FastReport;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using ClosedXML.Excel;
using EcoStationManagerApplication.Common.Exporters;
// Note: FastReport.OpenSource may not include PDF/Excel export modules
// Using PDFsharp and ClosedXML as alternatives (already in project)

namespace EcoStationManagerApplication.UI.Common
{
    /// <summary>
    /// Helper class for FastReport.NET integration
    /// Note: Install FastReport.NET package via NuGet: Install-Package FastReport.OpenSource
    /// </summary>
    public static class FastReportHelper
    {
        private static string _reportsTemplatePath;

        private static string ReportsTemplatePath
        {
            get
            {
                if (_reportsTemplatePath == null)
                {
                    try
                    {
                        var startupPath = Application.StartupPath;
                        if (string.IsNullOrEmpty(startupPath))
                        {
                            // Fallback nếu Application.StartupPath không có
                            startupPath = AppDomain.CurrentDomain.BaseDirectory;
                        }
                        _reportsTemplatePath = Path.Combine(startupPath, "Reports", "Templates");
                    }
                    catch
                    {
                        // Fallback nếu có lỗi
                        _reportsTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "Templates");
                    }
                }
                return _reportsTemplatePath;
            }
        }

        static FastReportHelper()
        {
            // Không tự động đăng ký Font Resolver ở đây
            // Sẽ đăng ký khi cần thiết trong method export
        }

        /// <summary>
        /// Đảm bảo Font Resolver đã được đăng ký (không dùng cho font chuẩn PDF)
        /// </summary>
        private static void EnsureFontResolver()
        {
            // Không cần thiết cho font chuẩn PDF
            // Chỉ dùng khi cần font tùy chỉnh
        }

        /// <summary>
        /// Get template path for a report type
        /// </summary>
        public static string GetTemplatePath(string reportType)
        {
            var templateName = GetTemplateFileName(reportType);
            var templatePath = Path.Combine(ReportsTemplatePath, templateName);
            
            // Create directory if not exists
            if (!Directory.Exists(ReportsTemplatePath))
            {
                Directory.CreateDirectory(ReportsTemplatePath);
            }

            // If template doesn't exist, create a default one
            if (!File.Exists(templatePath))
            {
                CreateDefaultTemplate(templatePath, reportType);
            }

            return templatePath;
        }

        /// <summary>
        /// Get template file name for report type
        /// </summary>
        private static string GetTemplateFileName(string reportType)
        {
            switch (reportType)
            {
                case "Doanh thu":
                    return "RevenueReport.frx";
                case "Đơn hàng":
                    return "OrderReport.frx";
                case "Khách hàng":
                    return "CustomerReport.frx";
                case "Bao bì":
                    return "PackagingReport.frx";
                case "Tác động môi trường":
                    return "EnvironmentalReport.frx";
                default:
                    return "DefaultReport.frx";
            }
        }

        /// <summary>
        /// Create default template if not exists
        /// </summary>
        private static void CreateDefaultTemplate(string templatePath, string reportType)
        {
            // This will be implemented when FastReport is installed
            // For now, just create the directory structure
        }

        /// <summary>
        /// Prepare report with data
        /// </summary>
        public static void PrepareReport(Report report, DataTable data, string reportType, DateTime fromDate, DateTime toDate)
        {
            report.RegisterData(data, "ReportData");
            report.SetParameterValue("ReportTitle", GetReportTitle(reportType));
            report.SetParameterValue("FromDate", fromDate.ToString("dd/MM/yyyy"));
            report.SetParameterValue("ToDate", toDate.ToString("dd/MM/yyyy"));
            report.SetParameterValue("GeneratedDate", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
        }

        /// <summary>
        /// Export DataTable to PDF using PDFsharp
        /// Note: FastReport.OpenSource may not include PDF export, so we use PDFsharp directly
        /// </summary>
        public static void ExportToPdf(DataTable data, string outputPath, string reportTitle, DateTime fromDate, DateTime toDate)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "DataTable cannot be null");
            
            if (data.Rows == null || data.Rows.Count == 0)
                throw new InvalidOperationException("No data to export");
            
            if (data.Columns == null || data.Columns.Count == 0)
                throw new InvalidOperationException("No columns in data table");

            if (string.IsNullOrWhiteSpace(outputPath))
                throw new ArgumentException("Output path cannot be null or empty", nameof(outputPath));

            // Đảm bảo Font Resolver đã được đăng ký
            // PdfSharp cần Font Resolver để xử lý font, ngay cả với font chuẩn PDF
            try
            {
                if (GlobalFontSettings.FontResolver == null)
                {
                    GlobalFontSettings.FontResolver = new WindowsFontResolver();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Warning: Could not set Font Resolver: {ex.Message}");
            }

            try
            {
                using (var document = new PdfDocument())
                {
                    var page = document.AddPage();
                    if (page == null)
                        throw new InvalidOperationException("Failed to create PDF page");

                    var gfx = XGraphics.FromPdfPage(page);
                    if (gfx == null)
                        throw new InvalidOperationException("Failed to create graphics context");

                    // Sử dụng font Windows thay vì font chuẩn PDF
                    // Font Resolver sẽ tự động map sang font Windows tương ứng
                    XFont font = null;
                    XFont fontRegular = null;
                    
                    try
                    {
                        // Sử dụng Arial (font Windows) thay vì Helvetica
                        // Font Resolver sẽ xử lý mapping
                        font = new XFont("Arial", 12, XFontStyleEx.Bold);
                        fontRegular = new XFont("Arial", 10, XFontStyleEx.Regular);
                    }
                    catch (Exception ex)
                    {
                        // Nếu Arial không hoạt động, thử Times New Roman
                        try
                        {
                            System.Diagnostics.Debug.WriteLine($"Failed to create Arial font: {ex.Message}. Trying Times New Roman.");
                            font = new XFont("Times New Roman", 12, XFontStyleEx.Bold);
                            fontRegular = new XFont("Times New Roman", 10, XFontStyleEx.Regular);
                        }
                        catch (Exception ex2)
                        {
                            // Cuối cùng thử Courier New
                            try
                            {
                                System.Diagnostics.Debug.WriteLine($"Failed to create Times New Roman font: {ex2.Message}. Trying Courier New.");
                                font = new XFont("Courier New", 12, XFontStyleEx.Bold);
                                fontRegular = new XFont("Courier New", 10, XFontStyleEx.Regular);
                            }
                            catch (Exception ex3)
                            {
                                throw new InvalidOperationException($"Failed to create any Windows font. Arial: {ex.Message}, Times New Roman: {ex2.Message}, Courier New: {ex3.Message}", ex);
                            }
                        }
                    }
                    
                    // Kiểm tra font đã được tạo thành công
                    if (font == null || fontRegular == null)
                    {
                        throw new InvalidOperationException("Failed to create fonts - font objects are null");
                    }
                
                double yPos = 40;
                double leftMargin = 40;
                double topMargin = 40;
                double lineHeight = 20;

                // Title
                gfx.DrawString(reportTitle, font, XBrushes.Black, 
                    new XRect(leftMargin, yPos, page.Width - leftMargin * 2, lineHeight),
                    XStringFormats.TopLeft);
                yPos += lineHeight * 1.5;

                // Date range
                gfx.DrawString($"Từ ngày: {fromDate:dd/MM/yyyy} - Đến ngày: {toDate:dd/MM/yyyy}", 
                    fontRegular, XBrushes.Black,
                    new XRect(leftMargin, yPos, page.Width - leftMargin * 2, lineHeight),
                    XStringFormats.TopLeft);
                yPos += lineHeight * 1.5;

                // Table headers
                int columnCount = data.Columns != null ? data.Columns.Count : 0;
                if (columnCount == 0)
                    throw new InvalidOperationException("Data table has no columns");

                double colWidth = (page.Width - leftMargin * 2) / columnCount;
                double xPos = leftMargin;
                
                if (data.Columns != null)
                {
                    foreach (DataColumn column in data.Columns)
                    {
                        if (column == null) continue;
                        
                        string columnName = column.ColumnName ?? "";
                        gfx.DrawRectangle(XPens.Black, 
                            new XRect(xPos, yPos, colWidth, lineHeight));
                        gfx.DrawString(columnName, fontRegular, XBrushes.Black,
                            new XRect(xPos + 5, yPos, colWidth - 10, lineHeight),
                            XStringFormats.TopLeft);
                        xPos += colWidth;
                    }
                }
                yPos += lineHeight;

                // Table data
                if (data.Rows != null)
                {
                    foreach (DataRow row in data.Rows)
                    {
                        if (row == null) continue;
                        
                        xPos = leftMargin;
                        if (data.Columns != null)
                        {
                            foreach (DataColumn column in data.Columns)
                            {
                                if (column == null) continue;
                                
                                string cellValue = "";
                                try
                                {
                                    var cellData = row[column];
                                    cellValue = cellData != null ? cellData.ToString() : "";
                                }
                                catch
                                {
                                    cellValue = "";
                                }
                                
                                gfx.DrawRectangle(XPens.Black,
                                    new XRect(xPos, yPos, colWidth, lineHeight));
                                gfx.DrawString(cellValue, fontRegular, XBrushes.Black,
                                    new XRect(xPos + 5, yPos, colWidth - 10, lineHeight),
                                    XStringFormats.TopLeft);
                                xPos += colWidth;
                            }
                        }
                        yPos += lineHeight;

                        // New page if needed
                        if (yPos > page.Height - topMargin - lineHeight)
                        {
                            page = document.AddPage();
                            if (page == null)
                                throw new InvalidOperationException("Failed to create new PDF page");
                            
                            gfx = XGraphics.FromPdfPage(page);
                            if (gfx == null)
                                throw new InvalidOperationException("Failed to create graphics context for new page");
                            
                            yPos = topMargin;
                        }
                    }
                }

                    document.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi xuất PDF: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Export DataTable to Excel using ClosedXML
        /// Note: FastReport.OpenSource may not include Excel export, so we use ClosedXML directly
        /// </summary>
        public static void ExportToExcel(DataTable data, string outputPath, string reportTitle, DateTime fromDate, DateTime toDate)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "DataTable cannot be null");
            
            if (data.Rows == null || data.Rows.Count == 0)
                throw new InvalidOperationException("No data to export");
            
            if (data.Columns == null || data.Columns.Count == 0)
                throw new InvalidOperationException("No columns in data table");

            if (string.IsNullOrWhiteSpace(outputPath))
                throw new ArgumentException("Output path cannot be null or empty", nameof(outputPath));

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Báo cáo");

                // Title
                worksheet.Cell(1, 1).Value = reportTitle;
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 1).Style.Font.FontSize = 14;

                // Date range
                worksheet.Cell(2, 1).Value = $"Từ ngày: {fromDate:dd/MM/yyyy} - Đến ngày: {toDate:dd/MM/yyyy}";

                // Headers
                int row = 4;
                int col = 1;
                if (data.Columns != null)
                {
                    foreach (DataColumn column in data.Columns)
                    {
                        if (column == null) continue;
                        
                        string columnName = column.ColumnName ?? "";
                        worksheet.Cell(row, col).Value = columnName;
                        worksheet.Cell(row, col).Style.Font.Bold = true;
                        worksheet.Cell(row, col).Style.Fill.BackgroundColor = XLColor.LightGray;
                        col++;
                    }
                }

                // Data
                row = 5;
                if (data.Rows != null)
                {
                    foreach (DataRow dataRow in data.Rows)
                    {
                        if (dataRow == null) continue;
                        
                        col = 1;
                        if (data.Columns != null)
                        {
                            foreach (DataColumn column in data.Columns)
                            {
                                if (column == null) continue;
                                
                                string cellValue = "";
                                try
                                {
                                    var cellData = dataRow[column];
                                    cellValue = cellData != null ? cellData.ToString() : "";
                                }
                                catch
                                {
                                    cellValue = "";
                                }
                                
                                worksheet.Cell(row, col).Value = cellValue;
                                col++;
                            }
                        }
                        row++;
                    }
                }

                // Auto-fit columns
                worksheet.Columns().AdjustToContents();

                workbook.SaveAs(outputPath);
            }
        }

        /// <summary>
        /// Get report title
        /// </summary>
        public static string GetReportTitle(string reportType)
        {
            switch (reportType)
            {
                case "Doanh thu":
                    return "Báo cáo Doanh thu";
                case "Đơn hàng":
                    return "Báo cáo Đơn hàng";
                case "Khách hàng":
                    return "Báo cáo Tần suất Khách hàng Quay lại";
                case "Bao bì":
                    return "Báo cáo Tỷ lệ Thu hồi Bao bì";
                case "Tác động môi trường":
                    return "Báo cáo Tác động Môi trường";
                default:
                    return "Báo cáo";
            }
        }
    }
}

