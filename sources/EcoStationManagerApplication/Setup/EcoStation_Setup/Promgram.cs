//using System;
//using System.Diagnostics;
//using System.IO;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

//namespace Setup.EcoStation_Setup
//{
//    public partial class SetupWizard : Form
//    {
//        private int currentStep = 0;
//        private readonly string[] stepTitles = {
//            "Chào mừng",
//            "Kiểm tra hệ thống",
//            "Cài đặt MySQL",
//            "Cấu hình Database",
//            "Khởi tạo dữ liệu",
//            "Hoàn tất"
//        };

//        public SetupWizard()
//        {
//            InitializeComponent();
//            UpdateStepDisplay();
//        }

//        private void UpdateStepDisplay()
//        {
//            lblStepTitle.Text = stepTitles[currentStep];
//            progressBar.Value = (currentStep + 1) * 100 / stepTitles.Length;

//            btnPrevious.Enabled = currentStep > 0;
//            btnNext.Text = currentStep == stepTitles.Length - 1 ? "Hoàn tất" : "Tiếp theo";
//        }

//        private async void btnNext_Click(object sender, EventArgs e)
//        {
//            switch (currentStep)
//            {
//                case 0: // Welcome
//                    await ExecuteStep1_SystemCheck();
//                    break;
//                case 1: // System Check
//                    await ExecuteStep2_InstallMySQL();
//                    break;
//                case 2: // MySQL Installation
//                    await ExecuteStep3_ConfigureDatabase();
//                    break;
//                case 3: // Database Configuration
//                    await ExecuteStep4_InitializeData();
//                    break;
//                case 4: // Data Initialization
//                    await ExecuteStep5_Finish();
//                    break;
//                case 5: // Finish
//                    Application.Exit();
//                    return;
//            }

//            currentStep++;
//            UpdateStepDisplay();
//        }

//        private async Task ExecuteStep1_SystemCheck()
//        {
//            AppendLog("🔍 Kiểm tra hệ thống...");

//            // Kiểm tra .NET Framework
//            if (!CheckDotNetFramework())
//            {
//                AppendLog("❌ .NET Framework 4.7.2 không được cài đặt");
//                throw new Exception("Yêu cầu .NET Framework 4.7.2");
//            }

//            // Kiểm tra disk space
//            if (!CheckDiskSpace())
//            {
//                AppendLog("❌ Không đủ dung lượng ổ cứng (cần ít nhất 500MB)");
//                throw new Exception("Không đủ dung lượng ổ cứng");
//            }

//            AppendLog("✅ Kiểm tra hệ thống hoàn tất");
//        }

//        private async Task ExecuteStep2_InstallMySQL()
//        {
//            AppendLog("🗄️ Kiểm tra MySQL...");

//            if (!CheckMySQLInstalled())
//            {
//                AppendLog("📥 MySQL chưa được cài đặt, tiến hành cài đặt...");
//                await InstallMySQL();
//            }
//            else
//            {
//                AppendLog("✅ MySQL đã được cài đặt");
//            }
//        }

//        private async Task ExecuteStep3_ConfigureDatabase()
//        {
//            AppendLog("⚙️ Cấu hình database...");

//            // Tạo database và user
//            await CreateDatabaseAndUser();
//            AppendLog("✅ Database đã được cấu hình");
//        }

//        private async Task ExecuteStep4_InitializeData()
//        {
//            AppendLog("📊 Khởi tạo dữ liệu mẫu...");

//            // Chạy SQL scripts
//            await ExecuteSqlScript("init_data.sql");
//            await ExecuteSqlScript("sample_data.sql");

//            AppendLog("✅ Dữ liệu mẫu đã được khởi tạo");
//        }

//        private async Task ExecuteStep5_Finish()
//        {
//            AppendLog("🎉 Thiết lập hoàn tất!");

//            // Cập nhật app.config với connection string mới
//            UpdateAppConfig();

//            // Tạo shortcut
//            CreateDesktopShortcut();
//        }

//        private void AppendLog(string message)
//        {
//            txtLog.AppendText($"{DateTime.Now:HH:mm:ss} {message}\r\n");
//            txtLog.ScrollToCaret();
//        }
//    }
//}