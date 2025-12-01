using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Web.Script.Serialization;

namespace Setup
{
    public partial class Setup : Form
    {
        private readonly JavaScriptSerializer _serializer = new JavaScriptSerializer();
        public Setup()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            txtInstallPath.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "EcoStationManager");
            txtServer.Text = "localhost";
            txtDatabase.Text = "EcoStationManager";
            txtUser.Text = "root";
            txtPassword.Text = "";
            txtPort.Text = "3306";
        }
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtInstallPath.Text = fbd.SelectedPath;
                }
            }
        }
        private void btnInstall_Click(object sender, EventArgs e)
        {
            progressBar.Value = 0;
            AppendLog("Bắt đầu cài đặt");
            var installDir = txtInstallPath.Text.Trim();
            if (string.IsNullOrEmpty(installDir))
            {
                MessageBox.Show("Vui lòng chọn thư mục cài đặt");
                return;
            }
            try
            {
                if (!Directory.Exists(installDir)) Directory.CreateDirectory(installDir);
                progressBar.Value = 10;
                var setupExeDir = Path.GetDirectoryName(Application.ExecutablePath);
                var sourceDir = Path.GetFullPath(Path.Combine(setupExeDir, "..", "EcoStationManagerApplication.UI", "bin", "Debug"));
                if (!Directory.Exists(sourceDir))
                {
                    throw new DirectoryNotFoundException(sourceDir);
                }
                AppendLog("Sao chép tập tin ứng dụng");
                CopyDirectory(sourceDir, installDir);
                progressBar.Value = 70;
                var appSettings = BuildAppSettings();
                var appSettingsPath = Path.Combine(installDir, "appsettings.json");
                var json = _serializer.Serialize(appSettings);
                File.WriteAllText(appSettingsPath, json);
                progressBar.Value = 90;
                var logsDir = Path.Combine(installDir, "logs");
                if (!Directory.Exists(logsDir)) Directory.CreateDirectory(logsDir);
                AppendLog("Hoàn tất cài đặt");
                progressBar.Value = 100;
                MessageBox.Show("Cài đặt thành công");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cài đặt: " + ex.Message);
                AppendLog("Lỗi: " + ex.Message);
            }
        }
        private void btnUninstall_Click(object sender, EventArgs e)
        {
            var installDir = txtInstallPath.Text.Trim();
            if (string.IsNullOrEmpty(installDir))
            {
                MessageBox.Show("Vui lòng chọn thư mục cài đặt");
                return;
            }
            try
            {
                if (Directory.Exists(installDir))
                {
                    Directory.Delete(installDir, true);
                    AppendLog("Đã gỡ cài đặt");
                    MessageBox.Show("Gỡ cài đặt thành công");
                }
                else
                {
                    MessageBox.Show("Thư mục cài đặt không tồn tại");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gỡ cài đặt: " + ex.Message);
                AppendLog("Lỗi: " + ex.Message);
            }
        }
        private void CopyDirectory(string sourceDir, string destDir)
        {
            foreach (var file in Directory.GetFiles(sourceDir))
            {
                var name = Path.GetFileName(file);
                File.Copy(file, Path.Combine(destDir, name), true);
            }
            foreach (var dir in Directory.GetDirectories(sourceDir))
            {
                var name = Path.GetFileName(dir);
                var targetSub = Path.Combine(destDir, name);
                if (!Directory.Exists(targetSub)) Directory.CreateDirectory(targetSub);
                CopyDirectory(dir, targetSub);
            }
        }
        private object BuildAppSettings()
        {
            var db = new
            {
                Server = txtServer.Text.Trim(),
                Database = txtDatabase.Text.Trim(),
                UserId = txtUser.Text.Trim(),
                Password = txtPassword.Text,
                Port = int.TryParse(txtPort.Text.Trim(), out var p) ? p : 3306,
                ConnectionTimeout = 30,
                CommandTimeout = 120,
                AllowZeroDateTime = false,
                ConvertZeroDateTime = true,
                MaxPoolSize = 100,
                MinPoolSize = 1
            };
            var app = new
            {
                Name = "EcoStation Manager",
                Version = "1.0.0",
                Environment = "Production",
                SessionTimeout = 30,
                MaxLoginAttempts = 5,
                Theme = "Light"
            };
            var log = new
            {
                LogLevel = "Information",
                LogPath = "logs",
                MaxFileSize = 10,
                EnableConsole = true,
                EnableFile = true
            };
            var ui = new
            {
                PrimaryColor = "#2C3E50",
                SecondaryColor = "#3498DB",
                SuccessColor = "#27AE60",
                WarningColor = "#F39C12",
                DangerColor = "#E74C3C",
                FontFamily = "Segoe UI",
                FontSize = 9,
                EnableAnimations = true,
                GunaTheme = "Light"
            };
            return new { Database = db, Application = app, Logging = log, UI = ui };
        }
        private void AppendLog(string message)
        {
            txtLog.AppendText(DateTime.Now.ToString("HH:mm:ss") + " " + message + Environment.NewLine);
            txtLog.ScrollToCaret();
        }
    }
}
