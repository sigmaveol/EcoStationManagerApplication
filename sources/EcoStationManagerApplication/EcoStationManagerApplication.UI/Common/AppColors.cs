using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.UI.Common
{
    public static class AppColors
    {
        // Primary Colors
        public static Color Primary = Color.FromArgb(45, 120, 60);     // #1f6b3b
        public static Color PrimaryHover = Color.FromArgb(33, 140, 73); // #218c49
        public static Color PrimaryLight = Color.FromArgb(241, 248, 243);
        public static Color SelectCell = Color.FromArgb(200, 230, 210);

        // Background Colors
        public static Color Background = Color.FromArgb(245, 247, 250); // #f5f7fa
        public static Color CardBackground = Color.White;

        // Text Colors
        public static Color PrimaryText = Color.FromArgb(51, 51, 51);   // #333
        public static Color SecondaryText = Color.FromArgb(85, 85, 85); // #555
        public static Color WhiteText = Color.White;

        // Status Colors
        public static Color Success = Color.FromArgb(31, 107, 59);     // #1f6b3b
        public static Color Warning = Color.FromArgb(249, 115, 22);    // #f97316
        public static Color Error = Color.FromArgb(212, 24, 61);       // #d4183d
        public static Color Info = Color.FromArgb(59, 130, 246);       // #3b82f6
    }

    public static class AppFonts
    {
        public static Font Title = new Font("Segoe UI", 16, FontStyle.Bold);
        public static Font Subtitle = new Font("Segoe UI", 14, FontStyle.Bold);
        public static Font Body = new Font("Segoe UI", 11, FontStyle.Regular);
        public static Font Small = new Font("Segoe UI", 10, FontStyle.Regular);
    }
}
