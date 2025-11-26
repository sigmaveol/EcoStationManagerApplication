using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Common.Utilities
{
    public static class FormatHelper
    {
        /// <summary>
        /// Format tiền tệ
        /// </summary>
        public static string FormatCurrency(decimal amount)
        {
            return amount > 0 ? amount.ToString("N0") : "-";
        }

        /// <summary>
        /// Format tiền tệ với đơn vị
        /// </summary>
        public static string FormatCurrencyWithUnit(decimal amount)
        {
            return amount > 0 ? $"{amount:N0} VNĐ" : "-";
        }

        /// <summary>
        /// Format số lượng
        /// </summary>
        public static string FormatQuantity(decimal quantity)
        {
            return quantity.ToString("N2");
        }

        /// <summary>
        /// Format số lượng nguyên
        /// </summary>
        public static string FormatIntegerQuantity(int quantity)
        {
            return quantity.ToString("N0");
        }

        /// <summary>
        /// Format ngày tháng
        /// </summary>
        public static string FormatDate(DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// Format ngày giờ
        /// </summary>
        public static string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy HH:mm");
        }
    }
}
