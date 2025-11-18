using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using EcoStationManagerApplication.UI.Properties;

namespace EcoStationManagerApplication.UI.Common
{
    public static class EmailNotificationHelper
    {
        /// <summary>
        /// Gửi email thông báo khi mã PIN thay đổi (nếu có cấu hình SMTP và kết nối mạng)
        /// </summary>
        public static async Task SendPinChangeNotificationAsync(string newPin)
        {
            try
            {
                if (!NetworkInterface.GetIsNetworkAvailable())
                    return;

                var adminEmail = string.IsNullOrWhiteSpace(Settings.Default.AdminEmail)
                    ? Settings.Default.CompanyEmail
                    : Settings.Default.AdminEmail;

                if (string.IsNullOrWhiteSpace(adminEmail))
                    return;

                if (string.IsNullOrWhiteSpace(Settings.Default.SmtpHost) ||
                    string.IsNullOrWhiteSpace(Settings.Default.SmtpUsername) ||
                    string.IsNullOrWhiteSpace(Settings.Default.SmtpPassword))
                {
                    return;
                }

                using (var message = new MailMessage())
                {
                    var fromAddress = new MailAddress(
                        Settings.Default.SmtpUsername,
                        Settings.Default.CompanyName ?? "EcoStation Manager");

                    message.From = fromAddress;
                    message.To.Add(adminEmail);
                    message.Subject = "Thông báo cập nhật mã PIN System Settings";
                    message.Body =
                        $"Mã PIN truy cập System Settings đã được cập nhật vào {DateTime.Now:dd/MM/yyyy HH:mm}.\r\n\r\n" +
                        $"Mã PIN mới: {newPin}\r\n\r\n" +
                        "Vui lòng lưu ý bảo mật thông tin này.";

                    using (var smtpClient = new SmtpClient(Settings.Default.SmtpHost, Settings.Default.SmtpPort > 0 ? Settings.Default.SmtpPort : 587))
                    {
                        smtpClient.EnableSsl = Settings.Default.SmtpEnableSsl;
                        smtpClient.Credentials = new NetworkCredential(Settings.Default.SmtpUsername, Settings.Default.SmtpPassword);
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                        await smtpClient.SendMailAsync(message).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SendPinChangeNotificationAsync error: {ex.Message}");
            }
        }
    }
}

