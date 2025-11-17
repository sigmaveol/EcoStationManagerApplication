using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using FastReport;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using ClosedXML.Excel;
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
        private static readonly string ReportsTemplatePath = Path.Combine(
            Application.StartupPath, "Reports", "Templates");

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
            if (data == null || data.Rows.Count == 0)
                throw new InvalidOperationException("No data to export");

            using (var document = new PdfDocument())
            {
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var font = new XFont("Arial", 12, XFontStyleEx.Bold);
                var fontRegular = new XFont("Arial", 10);
                
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
                double colWidth = (page.Width - leftMargin * 2) / data.Columns.Count;
                double xPos = leftMargin;
                
                foreach (DataColumn column in data.Columns)
                {
                    gfx.DrawRectangle(XPens.Black, 
                        new XRect(xPos, yPos, colWidth, lineHeight));
                    gfx.DrawString(column.ColumnName, fontRegular, XBrushes.Black,
                        new XRect(xPos + 5, yPos, colWidth - 10, lineHeight),
                        XStringFormats.TopLeft);
                    xPos += colWidth;
                }
                yPos += lineHeight;

                // Table data
                foreach (DataRow row in data.Rows)
                {
                    xPos = leftMargin;
                    foreach (DataColumn column in data.Columns)
                    {
                        gfx.DrawRectangle(XPens.Black,
                            new XRect(xPos, yPos, colWidth, lineHeight));
                        gfx.DrawString(row[column].ToString(), fontRegular, XBrushes.Black,
                            new XRect(xPos + 5, yPos, colWidth - 10, lineHeight),
                            XStringFormats.TopLeft);
                        xPos += colWidth;
                    }
                    yPos += lineHeight;

                    // New page if needed
                    if (yPos > page.Height - topMargin - lineHeight)
                    {
                        page = document.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        yPos = topMargin;
                    }
                }

                document.Save(outputPath);
            }
        }

        /// <summary>
        /// Export DataTable to Excel using ClosedXML
        /// Note: FastReport.OpenSource may not include Excel export, so we use ClosedXML directly
        /// </summary>
        public static void ExportToExcel(DataTable data, string outputPath, string reportTitle, DateTime fromDate, DateTime toDate)
        {
            if (data == null || data.Rows.Count == 0)
                throw new InvalidOperationException("No data to export");

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
                foreach (DataColumn column in data.Columns)
                {
                    worksheet.Cell(row, col).Value = column.ColumnName;
                    worksheet.Cell(row, col).Style.Font.Bold = true;
                    worksheet.Cell(row, col).Style.Fill.BackgroundColor = XLColor.LightGray;
                    col++;
                }

                // Data
                row = 5;
                foreach (DataRow dataRow in data.Rows)
                {
                    col = 1;
                    foreach (DataColumn column in data.Columns)
                    {
                        worksheet.Cell(row, col).Value = dataRow[column].ToString();
                        col++;
                    }
                    row++;
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

