using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace EcoStationManagerApplication.UI.Controls
{
    /// <summary>
    /// Helper methods for ReportControl - shared utilities
    /// </summary>
    public static class ReportControlHelpers
    {
        public static Guna2Panel CreateKPICard(string label, string value, string icon = "")
        {
            var card = new Guna2Panel();
            card.BackColor = Color.White;
            card.BorderRadius = 8;
            card.FillColor = Color.White;
            card.Padding = new Padding(15);
            card.ShadowDecoration.BorderRadius = 8;
            card.ShadowDecoration.Color = Color.FromArgb(50, 0, 0, 0);
            card.ShadowDecoration.Depth = 3;
            card.ShadowDecoration.Enabled = true;

            var lblIcon = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 20, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(15, 15)
            };

            var lblLabel = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.FromArgb(85, 85, 85),
                AutoSize = true,
                Location = new Point(75, 15)
            };

            var lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 107, 59),
                AutoSize = true,
                Location = new Point(15, 50)
            };

            card.Controls.Add(lblIcon);
            card.Controls.Add(lblLabel);
            card.Controls.Add(lblValue);

            return card;
        }

        public static string FormatCurrency(decimal amount)
        {
            if (amount >= 1000000)
                return (amount / 1000000).ToString("F2") + "M";
            else if (amount >= 1000)
                return (amount / 1000).ToString("F1") + "K";
            else
                return amount.ToString("N0");
        }

        public static string GetOrderSourceName(Models.Enums.OrderSource source)
        {
            switch (source)
            {
                case Models.Enums.OrderSource.GOOGLEFORM:
                    return "Google Form";
                case Models.Enums.OrderSource.EXCEL:
                    return "Excel";
                case Models.Enums.OrderSource.EMAIL:
                    return "Email";
                case Models.Enums.OrderSource.MANUAL:
                    return "Thủ công";
                default:
                    return "Khác";
            }
        }
    }
}

