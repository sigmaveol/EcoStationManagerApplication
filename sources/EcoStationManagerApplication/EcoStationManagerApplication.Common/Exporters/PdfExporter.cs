using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EcoStationManagerApplication.Common.Exporters
{
    /// <summary>
    /// Implementation của IPdfExporter - Sử dụng PdfSharp
    /// </summary>
    public class PdfExporter : IPdfExporter
    {
        private const int ItemsPerPage = 25;
        private const double LeftMargin = 40;
        private const double LineHeight = 20;
        private const double TableTop = 100;

        static PdfExporter()
        {
            // Đăng ký Font Resolver một lần khi class được load
            if (GlobalFontSettings.FontResolver == null)
            {
                GlobalFontSettings.FontResolver = new WindowsFontResolver();
            }
        }

        public void ExportToPdf<T>(IEnumerable<T> data, string filePath, string title = "Danh sách", Dictionary<string, string> headers = null)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

            // Đảm bảo title không null
            if (string.IsNullOrWhiteSpace(title))
                title = "Danh sách";

            var dataList = data.ToList();
            if (!dataList.Any())
                throw new InvalidOperationException("No data to export");

            // Lấy properties của T
            var properties = typeof(T).GetProperties()
                .Where(p => p != null && p.CanRead && !string.IsNullOrWhiteSpace(p.Name) && !IsComplexType(p.PropertyType))
                .ToList();

            if (!properties.Any())
                throw new InvalidOperationException("No valid properties found to export");

            var document = new PdfDocument();
            var page = document.AddPage();
            if (page == null)
                throw new InvalidOperationException("Failed to create PDF page");

            page.Size = PdfSharp.PageSize.A4;
            var gfx = XGraphics.FromPdfPage(page);
            if (gfx == null)
                throw new InvalidOperationException("Failed to create graphics context");

            // Fonts
            // Sử dụng XFontStyleEx cho PdfSharp 6.0+
            // Sử dụng Helvetica (font chuẩn PDF) thay vì Arial để tương thích tốt hơn
            XFont titleFont = null;
            XFont headerFont = null;
            XFont normalFont = null;
            XFont smallFont = null;

            try
            {
                titleFont = new XFont("Arial", 18);
                headerFont = new XFont("Arial", 10);
                normalFont = new XFont("Arial", 9);
                smallFont = new XFont("Arial", 8);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to create fonts: {ex.Message}", ex);
            }

            double yPos = 40;
            int currentPage = 1;
            int itemIndex = 0;

            // Tiêu đề
            string titleText = (title ?? "Danh sách").ToUpper();
            var titleRect = new XRect(LeftMargin, yPos, page.Width - 80, LineHeight);

            gfx.DrawString(titleText, titleFont, XBrushes.Black,
                new XRect(LeftMargin, yPos, 
                
                page.Width - 80, LineHeight),
                XStringFormats.TopCenter);

            yPos += 30;

            // Thông tin xuất
            gfx.DrawString($"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm:ss}", normalFont, XBrushes.Black,
                new XRect(LeftMargin, yPos, page.Width - 80, LineHeight), XStringFormats.TopLeft);
            yPos += 15;
            gfx.DrawString($"Tổng số bản ghi: {dataList.Count}", normalFont, XBrushes.Black,
                new XRect(LeftMargin, yPos, page.Width - 80, LineHeight), XStringFormats.TopLeft);

            // Tính toán độ rộng cột
            double totalWidth = page.Width - 80;
            double[] columnWidths = CalculateColumnWidths(properties.Count, totalWidth);

            // Table header
            yPos = TableTop;
            double xPos = LeftMargin;

            // Draw header background
            gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(46, 125, 50)),
                new XRect(LeftMargin, yPos, totalWidth, LineHeight + 5));

            // Draw header text
            for (int i = 0; i < properties.Count; i++)
            {
                var prop = properties[i];
                if (prop == null || string.IsNullOrWhiteSpace(prop.Name))
                    continue;

                string headerText = headers != null && headers.ContainsKey(prop.Name)
                    ? headers[prop.Name]
                    : prop.Name;

                if (string.IsNullOrWhiteSpace(headerText))
                    headerText = prop.Name ?? "Column";

                gfx.DrawString(headerText, headerFont, XBrushes.White,
                    new XRect(xPos + 2, yPos + 2, columnWidths[i] - 4, LineHeight),
                    XStringFormats.TopLeft);
                xPos += columnWidths[i];
            }

            yPos += LineHeight + 5;

            // Data rows
            foreach (var item in dataList)
            {
                if (item == null)
                    continue;

                // Kiểm tra nếu cần tạo trang mới
                if (itemIndex > 0 && itemIndex % ItemsPerPage == 0)
                {
                    // Thêm footer cho trang cũ
                    if (gfx != null && page != null)
                        AddPageFooter(gfx, page, currentPage, document.Pages.Count, smallFont);

                    page = document.AddPage();
                    if (page == null)
                        throw new InvalidOperationException("Failed to create new PDF page");

                    page.Size = PdfSharp.PageSize.A4;
                    gfx = XGraphics.FromPdfPage(page);
                    if (gfx == null)
                        throw new InvalidOperationException("Failed to create graphics context for new page");

                    yPos = 40;
                    currentPage++;

                    // Vẽ lại header
                    xPos = LeftMargin;
                    gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(46, 125, 50)),
                        new XRect(LeftMargin, yPos, totalWidth, LineHeight + 5));

                    for (int i = 0; i < properties.Count; i++)
                    {
                        var prop = properties[i];
                        if (prop == null || string.IsNullOrWhiteSpace(prop.Name))
                            continue;

                        string headerText = headers != null && headers.ContainsKey(prop.Name)
                            ? headers[prop.Name]
                            : prop.Name;

                        if (string.IsNullOrWhiteSpace(headerText))
                            headerText = prop.Name ?? "Column";

                        gfx.DrawString(headerText, headerFont, XBrushes.White,
                            new XRect(xPos + 2, yPos + 2, columnWidths[i] - 4, LineHeight),
                            XStringFormats.TopLeft);
                        xPos += columnWidths[i];
                    }
                    yPos += LineHeight + 5;
                }

                xPos = LeftMargin;

                // Draw row background (alternating)
                if (itemIndex % 2 == 0 && gfx != null)
                {
                    gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(245, 245, 245)),
                        new XRect(LeftMargin, yPos, totalWidth, LineHeight));
                }

                // Draw row data
                for (int i = 0; i < properties.Count; i++)
                {
                    var prop = properties[i];
                    if (prop == null)
                        continue;

                    try
                    {
                        var value = prop.GetValue(item);
                        string displayValue = FormatValue(value, prop.PropertyType);

                        // Truncate nếu quá dài
                        if (!string.IsNullOrEmpty(displayValue) && displayValue.Length > 20)
                            displayValue = displayValue.Substring(0, 17) + "...";

                        if (string.IsNullOrEmpty(displayValue))
                            displayValue = "";

                        if (gfx != null)
                        {
                            gfx.DrawString(displayValue, normalFont, XBrushes.Black,
                                new XRect(xPos + 2, yPos + 2, columnWidths[i] - 4, LineHeight),
                                XStringFormats.TopLeft);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log error nhưng tiếp tục với các cột khác
                        System.Diagnostics.Debug.WriteLine($"Error rendering property {prop.Name}: {ex.Message}");
                        if (gfx != null)
                        {
                            gfx.DrawString("", normalFont, XBrushes.Black,
                                new XRect(xPos + 2, yPos + 2, columnWidths[i] - 4, LineHeight),
                                XStringFormats.TopLeft);
                        }
                    }

                    xPos += columnWidths[i];
                }

                // Draw row border
                if (gfx != null && page != null)
                {
                    gfx.DrawLine(XPens.LightGray, LeftMargin, yPos + LineHeight,
                        page.Width - 40, yPos + LineHeight);
                }

                yPos += LineHeight;
                itemIndex++;
            }

            // Thêm footer cho tất cả các trang
            if (document != null && document.Pages != null)
            {
                for (int pageIndex = 0; pageIndex < document.Pages.Count; pageIndex++)
                {
                    var pdfPage = document.Pages[pageIndex];
                    if (pdfPage != null)
                    {
                        try
                        {
                            gfx = XGraphics.FromPdfPage(pdfPage);
                            if (gfx != null && smallFont != null)
                            {
                                AddPageFooter(gfx, pdfPage, pageIndex + 1, document.Pages.Count, smallFont);
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error adding footer to page {pageIndex + 1}: {ex.Message}");
                        }
                    }
                }
            }

            if (document != null)
            {
                document.Save(filePath);
                document.Dispose();
            }
        }

        private double[] CalculateColumnWidths(int columnCount, double totalWidth)
        {
            double[] widths = new double[columnCount];
            double widthPerColumn = totalWidth / columnCount;

            for (int i = 0; i < columnCount; i++)
            {
                widths[i] = widthPerColumn;
            }

            return widths;
        }

        private string FormatValue(object value, Type propertyType)
        {
            if (value == null)
                return string.Empty;

            if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
            {
                return Convert.ToDecimal(value).ToString("N0");
            }
            else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
            {
                return Convert.ToDateTime(value).ToString("dd/MM/yyyy");
            }
            else if (propertyType == typeof(int) || propertyType == typeof(int?))
            {
                return Convert.ToInt32(value).ToString();
            }

            return value.ToString();
        }

        private void AddPageFooter(XGraphics gfx, PdfPage page, int currentPage, int totalPages, XFont font)
        {
            gfx.DrawString($"Trang {currentPage} / {totalPages}",
                font, XBrushes.Gray,
                new XRect(LeftMargin, page.Height - 30, page.Width - 80, 20),
                XStringFormats.BottomCenter);
        }

        private bool IsComplexType(Type type)
        {
            if (type == typeof(string) || type == typeof(DateTime) || type == typeof(DateTime?))
                return false;

            if (type.IsPrimitive || type == typeof(decimal) || type == typeof(decimal?))
                return false;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var underlyingType = Nullable.GetUnderlyingType(type);
                return IsComplexType(underlyingType);
            }

            return type.IsClass && type != typeof(string);
        }
    }
}

