using PdfSharp.Fonts;
using System;
using System.IO;

namespace EcoStationManagerApplication.Common.Exporters
{
    /// <summary>
    /// Font Resolver cho PdfSharp/MigraDoc 1.50
    /// Hỗ trợ load font từ Windows Fonts folder
    /// </summary>
    public class WindowsFontResolver : IFontResolver
    {
        // Cache font data để tránh đọc file nhiều lần
        private static byte[] _timesNewRomanRegular;
        private static byte[] _timesNewRomanBold;
        private static byte[] _timesNewRomanItalic;
        private static byte[] _timesNewRomanBoldItalic;
        private static byte[] _arialRegular;
        private static byte[] _arialBold;

        public string DefaultFontName => "Times New Roman";

        public byte[] GetFont(string faceName)
        {
            // faceName format: "FontName#B" hoặc "FontName#BI" etc.
            switch (faceName.ToLowerInvariant())
            {
                // Times New Roman
                case "times new roman#regular":
                case "times new roman":
                    return GetTimesNewRomanRegular();

                case "times new roman#bold":
                    return GetTimesNewRomanBold();

                case "times new roman#italic":
                    return GetTimesNewRomanItalic();

                case "times new roman#bolditalic":
                    return GetTimesNewRomanBoldItalic();

                // Arial
                case "arial#regular":
                case "arial":
                    return GetArialRegular();

                case "arial#bold":
                    return GetArialBold();

                // Fallback to Times New Roman
                default:
                    System.Diagnostics.Debug.WriteLine($"Font not found: {faceName}, using Times New Roman");
                    return GetTimesNewRomanRegular();
            }
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            string faceName;

            // Normalize font family name
            var normalizedName = familyName?.ToLowerInvariant() ?? "times new roman";

            switch (normalizedName)
            {
                case "times new roman":
                case "times":
                    faceName = GetTimesNewRomanFaceName(isBold, isItalic);
                    break;

                case "arial":
                    faceName = GetArialFaceName(isBold, isItalic);
                    break;

                case "helvetica":
                    // Map Helvetica to Arial (similar sans-serif)
                    faceName = GetArialFaceName(isBold, isItalic);
                    break;

                default:
                    // Fallback to Times New Roman
                    faceName = GetTimesNewRomanFaceName(isBold, isItalic);
                    break;
            }

            return new FontResolverInfo(faceName);
        }

        #region Font Face Names

        private string GetTimesNewRomanFaceName(bool isBold, bool isItalic)
        {
            if (isBold && isItalic) return "Times New Roman#BoldItalic";
            if (isBold) return "Times New Roman#Bold";
            if (isItalic) return "Times New Roman#Italic";
            return "Times New Roman#Regular";
        }

        private string GetArialFaceName(bool isBold, bool isItalic)
        {
            if (isBold && isItalic) return "Arial#Bold"; // Arial không có BoldItalic trong Windows mặc định
            if (isBold) return "Arial#Bold";
            if (isItalic) return "Arial#Regular"; // Fallback
            return "Arial#Regular";
        }

        #endregion

        #region Font Loaders

        private byte[] GetTimesNewRomanRegular()
        {
            return _timesNewRomanRegular ?? (_timesNewRomanRegular = LoadFontFile("times.ttf"));
        }

        private byte[] GetTimesNewRomanBold()
        {
            return _timesNewRomanBold ?? (_timesNewRomanBold = LoadFontFile("timesbd.ttf"));
        }

        private byte[] GetTimesNewRomanItalic()
        {
            return _timesNewRomanItalic ?? (_timesNewRomanItalic = LoadFontFile("timesi.ttf"));
        }

        private byte[] GetTimesNewRomanBoldItalic()
        {
            return _timesNewRomanBoldItalic ?? (_timesNewRomanBoldItalic = LoadFontFile("timesbi.ttf"));
        }

        private byte[] GetArialRegular()
        {
            return _arialRegular ?? (_arialRegular = LoadFontFile("arial.ttf"));
        }

        private byte[] GetArialBold()
        {
            return _arialBold ?? (_arialBold = LoadFontFile("arialbd.ttf"));
        }

        private byte[] LoadFontFile(string fileName)
        {
            // Thử các đường dẫn khác nhau
            string[] fontPaths = new[]
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), fileName),
                Path.Combine(@"C:\Windows\Fonts", fileName),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Fonts", fileName),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName)
            };

            foreach (var path in fontPaths)
            {
                try
                {
                    if (File.Exists(path))
                    {
                        return File.ReadAllBytes(path);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error loading font from {path}: {ex.Message}");
                }
            }

            throw new FileNotFoundException($"Font file not found: {fileName}. Searched in: {string.Join(", ", fontPaths)}");
        }

        #endregion
    }
}