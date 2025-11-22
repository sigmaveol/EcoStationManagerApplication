using EcoStationManagerApplication.Common.Helpers;
using EcoStationManagerApplication.UI.Common;
using EcoStationManagerApplication.UI.Controls;
using EcoStationManagerApplication.UI.Forms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {   
            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Cấu hình để xử lý exception không bắt được
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // Cài đặt ngôn ngữ tiếng Việt
            Thread.CurrentThread.CurrentCulture = new CultureInfo("vi-VN");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("vi-VN");

            // Tối ưu GC cho ứng dụng desktop
            GCSettings.LatencyMode = GCLatencyMode.Batch;

            // Hiển thị LoginForm trước
            Application.Run(new LoginForm());
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                MessageBox.Show(
                    $"Đã xảy ra lỗi không mong muốn:\n\n{e.Exception.Message}\n\nChi tiết: {e.Exception.StackTrace}",
                    "Lỗi ứng dụng",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch
            {
                // Nếu không thể hiển thị message box, ghi log hoặc bỏ qua
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var ex = e.ExceptionObject as Exception;
                MessageBox.Show(
                    $"Đã xảy ra lỗi:\n\n{ex?.Message ?? "Unknown error"}\n\nỨng dụng sẽ đóng.",
                    "Lỗi nghiêm trọng",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch
            {
                // Nếu không thể hiển thị message box, bỏ qua
            }
        }
    }
}
