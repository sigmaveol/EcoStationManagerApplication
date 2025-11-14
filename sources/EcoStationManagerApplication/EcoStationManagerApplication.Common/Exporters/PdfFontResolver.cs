using PdfSharp.Drawing;
using PdfSharp.Fonts;
using System;
using System.Drawing;
using System.IO;

namespace EcoStationManagerApplication.Common.Exporters
{
    /// <summary>
    /// Font Resolver sử dụng fonts hệ thống Windows
    /// </summary>
    public class WindowsFontResolver : IFontResolver
    {
        public byte[] GetFont(string faceName)
        {
            // PdfSharp sẽ tự động tìm font trong hệ thống Windows
            // Trả về null để PdfSharp sử dụng font resolver mặc định
            return null;
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            // Kiểm tra xem font có tồn tại trong hệ thống không
            string normalizedName = NormalizeFontName(familyName);
            
            // Thử tìm font trong hệ thống
            // Sử dụng các font chuẩn PDF: Helvetica, Times, Courier
            if (normalizedName.Contains("arial") || normalizedName.Contains("helvetica"))
            {
                return new FontResolverInfo("Helvetica", isBold, isItalic);
            }
            else if (normalizedName.Contains("times"))
            {
                return new FontResolverInfo("Times-Roman", isBold, isItalic);
            }
            else if (normalizedName.Contains("courier"))
            {
                return new FontResolverInfo("Courier", isBold, isItalic);
            }
            else
            {
                // Mặc định sử dụng Helvetica
                return new FontResolverInfo("Helvetica", isBold, isItalic);
            }
        }

        private string NormalizeFontName(string fontName)
        {
            if (string.IsNullOrWhiteSpace(fontName))
                return "Arial";

            // Chuẩn hóa tên font
            string normalized = fontName.Trim();

            // Map các tên font phổ biến
            if (normalized.Equals("Arial", StringComparison.OrdinalIgnoreCase))
                return "Arial";
            if (normalized.Equals("Helvetica", StringComparison.OrdinalIgnoreCase))
                return "Arial"; // Windows thường dùng Arial thay cho Helvetica
            if (normalized.Equals("Times", StringComparison.OrdinalIgnoreCase) || 
                normalized.Contains("Times"))
                return "Times New Roman";
            if (normalized.Equals("Courier", StringComparison.OrdinalIgnoreCase))
                return "Courier New";

            return normalized;
        }
    }
}

