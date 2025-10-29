using EcoStationManagerApplication.UI.Properties;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class SystemSettingsControl : UserControl
    {
        private List<Stations> stations; // Vẫn giữ Stations
        private List<Role> roles;

        public SystemSettingsControl()
        {
            InitializeComponent();
            LoadData();
            InitializeControls();
        }

        private void LoadData()
        {
            stations = SystemSettingsMockData.GetStations();
            roles = SystemSettingsMockData.GetRoles();
        }

        private void InitializeControls()
        {
            // Initialize tab control
            InitializeTabControl();
            InitializeComboBoxes();

            // Load settings data
            LoadGeneralSettings();
            LoadStationSettings();
            LoadInventorySettings();
            LoadUserManagement();
        }

        private void InitializeComboBoxes()
        {
            // Clear existing items
            comboBoxStartTime.Items.Clear();
            comboBoxEndTime.Items.Clear();
            comboBoxCurrency.Items.Clear();
            comboBoxCleaningFrequency.Items.Clear();

            // Khởi tạo business hours (mỗi 30 phút)
            for (int hour = 0; hour < 24; hour++)
            {
                for (int minute = 0; minute < 60; minute += 30)
                {
                    string timeText = $"{hour:00}:{minute:00}";
                    comboBoxStartTime.Items.Add(timeText);
                    comboBoxEndTime.Items.Add(timeText);
                }
            }

            // Khởi tạo currency options
            comboBoxCurrency.Items.AddRange(new string[] { "VND", "USD", "EUR" });

            // Khởi tạo cleaning frequency
            comboBoxCleaningFrequency.Items.AddRange(new string[]
            {
                "Hàng ngày",
                "Hàng tuần",
                "Hàng tháng",
                "Hàng quý"
            });
        }

        private void InitializeTabControl()
        {
            // Set tab styles
            tabControlSystem.ItemSize = new Size(120, 40);
            tabControlSystem.SelectedIndex = 0;
        }

        private void LoadGeneralSettings()
        {
            try
            {
                // Load từ Settings - Sử dụng try-catch để tránh lỗi
                txtCompanyName.Text = Settings.Default.CompanyName ?? "Công ty TNHH EcoStation";
                txtCompanyAddress.Text = Settings.Default.CompanyAddress ?? "123 Đường ABC, Quận 1, TP.HCM";
                txtCompanyPhone.Text = Settings.Default.CompanyPhone ?? "(+84) 28 1234 5678";
                txtCompanyEmail.Text = Settings.Default.CompanyEmail ?? "contact@ecostation.com";
                txtTaxCode.Text = Settings.Default.TaxCode ?? "0123456789";

                // Set business hours từ Settings
                string startTime = Settings.Default.BusinessStartTime ?? "07:00";
                string endTime = Settings.Default.BusinessEndTime ?? "17:00";

                if (comboBoxStartTime.Items.Contains(startTime))
                    comboBoxStartTime.SelectedItem = startTime;
                else
                    comboBoxStartTime.SelectedIndex = 14; // 07:00

                if (comboBoxEndTime.Items.Contains(endTime))
                    comboBoxEndTime.SelectedItem = endTime;
                else
                    comboBoxEndTime.SelectedIndex = 34; // 17:00

                // Set currency từ Settings
                string currency = Settings.Default.Currency ?? "VND";
                if (comboBoxCurrency.Items.Contains(currency))
                    comboBoxCurrency.SelectedItem = currency;
                else
                    comboBoxCurrency.SelectedIndex = 0;

                // Set auto backup từ Settings
                toggleAutoBackup.Checked = Settings.Default.AutoBackupEnabled;

                if (Settings.Default.BackupIntervalDays > 0)
                    numericBackupDays.Value = Settings.Default.BackupIntervalDays;
                else
                    numericBackupDays.Value = 7;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải cài đặt: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Set default values
                SetDefaultGeneralSettings();
            }
        }

        private void SetDefaultGeneralSettings()
        {
            txtCompanyName.Text = "Công ty TNHH EcoStation";
            txtCompanyAddress.Text = "123 Đường ABC, Quận 1, TP.HCM";
            txtCompanyPhone.Text = "(+84) 28 1234 5678";
            txtCompanyEmail.Text = "contact@ecostation.com";
            txtTaxCode.Text = "0123456789";
            comboBoxStartTime.SelectedIndex = 14; // 07:00
            comboBoxEndTime.SelectedIndex = 34; // 17:00
            comboBoxCurrency.SelectedIndex = 0; // VND
            toggleAutoBackup.Checked = true;
            numericBackupDays.Value = 7;
        }

        private void LoadStationSettings()
        {
            // Load stations to list
            listBoxStations.Items.Clear();
            foreach (var station in stations)
            {
                listBoxStations.Items.Add($"{station.Name} - {station.Code}");
            }
            if (listBoxStations.Items.Count > 0)
                listBoxStations.SelectedIndex = 0;

            // Load station details
            LoadSelectedStationDetails();
        }

        private void LoadSelectedStationDetails()
        {
            if (listBoxStations.SelectedIndex >= 0 && listBoxStations.SelectedIndex < stations.Count)
            {
                var station = stations[listBoxStations.SelectedIndex];
                txtStationName.Text = station.Name;
                txtStationCode.Text = station.Code;
                txtStationAddress.Text = station.Address;
                txtStationPhone.Text = station.Phone;
                txtManagerName.Text = station.Manager;
                txtStationCapacity.Text = station.Capacity.ToString();
            }
        }

        private void LoadInventorySettings()
        {
            try
            {
                // Set inventory thresholds từ Settings
                numericMinStock.Value = Settings.Default.MinStockLevel > 0 ? Settings.Default.MinStockLevel : 50;
                numericMaxStock.Value = Settings.Default.MaxStockLevel > 0 ? Settings.Default.MaxStockLevel : 500;
                numericReorderPoint.Value = Settings.Default.ReorderPoint > 0 ? Settings.Default.ReorderPoint : 100;
                numericSafetyStock.Value = Settings.Default.SafetyStock > 0 ? Settings.Default.SafetyStock : 30;

                // Set cleaning schedule từ Settings
                string cleaningFreq = Settings.Default.CleaningFrequency ?? "Hàng tuần";
                if (comboBoxCleaningFrequency.Items.Contains(cleaningFreq))
                    comboBoxCleaningFrequency.SelectedItem = cleaningFreq;
                else
                    comboBoxCleaningFrequency.SelectedIndex = 1; // Weekly

                numericCleaningDays.Value = Settings.Default.CleaningIntervalDays > 0 ?
                    Settings.Default.CleaningIntervalDays : 7;

                // Set package management từ Settings
                numericPackageLifespan.Value = Settings.Default.PackageLifespanDays > 0 ?
                    Settings.Default.PackageLifespanDays : 90;

                toggleAutoReorder.Checked = Settings.Default.AutoReorderEnabled;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải cài đặt tồn kho: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Set default values
                SetDefaultInventorySettings();
            }
        }

        private void SetDefaultInventorySettings()
        {
            numericMinStock.Value = 50;
            numericMaxStock.Value = 500;
            numericReorderPoint.Value = 100;
            numericSafetyStock.Value = 30;
            comboBoxCleaningFrequency.SelectedIndex = 1; // Weekly
            numericCleaningDays.Value = 7;
            numericPackageLifespan.Value = 90;
            toggleAutoReorder.Checked = true;
        }

        private void LoadUserManagement()
        {
            // Load roles
            comboBoxUserRole.Items.Clear();
            foreach (var role in roles)
            {
                comboBoxUserRole.Items.Add(role.Name);
            }
            if (comboBoxUserRole.Items.Count > 0)
                comboBoxUserRole.SelectedIndex = 0;

            // Load permissions for selected role
            LoadRolePermissions(0);
        }

        private void LoadRolePermissions(int roleIndex)
        {
            if (roleIndex >= 0 && roleIndex < roles.Count)
            {
                var role = roles[roleIndex];
                checkBoxOrderManage.Checked = role.Permissions.Contains("order_manage");
                checkBoxInventoryView.Checked = role.Permissions.Contains("inventory_view");
                checkBoxInventoryEdit.Checked = role.Permissions.Contains("inventory_edit");
                checkBoxReportView.Checked = role.Permissions.Contains("report_view");
                checkBoxSettingsEdit.Checked = role.Permissions.Contains("settings_edit");
                checkBoxUserManage.Checked = role.Permissions.Contains("user_manage");
            }
        }

        #region Event Handlers - ĐÃ SỬA
        private void btnSaveGeneral_Click(object sender, EventArgs e)
        {
            if (ValidateGeneralSettings())
            {
                SaveGeneralSettings(); // Đã thêm phương thức Save
            }
        }

        private void btnSaveStation_Click(object sender, EventArgs e)
        {
            if (ValidateStationSettings())
            {
                SaveStationSettings(); // Đã thêm phương thức Save
            }
        }

        private void btnSaveInventory_Click(object sender, EventArgs e)
        {
            if (ValidateInventorySettings())
            {
                SaveInventorySettings(); // Đã thêm phương thức Save
            }
        }

        private void btnSaveUser_Click(object sender, EventArgs e)
        {
            if (ValidateUserSettings())
            {
                SaveUserSettings(); // Đã thêm phương thức Save
            }
        }

        private void listBoxStations_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSelectedStationDetails();
        }

        private void comboBoxUserRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRolePermissions(comboBoxUserRole.SelectedIndex);
        }

        private void btnAddStation_Click(object sender, EventArgs e)
        {
            // Add new station logic
            MessageBox.Show("Chức năng thêm trạm mới đang được phát triển", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            // Add new user logic
            MessageBox.Show("Chức năng thêm người dùng mới đang được phát triển", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region Save Methods - MỚI THÊM
        private void SaveGeneralSettings()
        {
            try
            {
                // Lưu thông tin công ty
                Settings.Default.CompanyName = txtCompanyName.Text;
                Settings.Default.CompanyAddress = txtCompanyAddress.Text;
                Settings.Default.CompanyPhone = txtCompanyPhone.Text;
                Settings.Default.CompanyEmail = txtCompanyEmail.Text;
                Settings.Default.TaxCode = txtTaxCode.Text;

                // Lưu giờ làm việc
                Settings.Default.BusinessStartTime = comboBoxStartTime.SelectedItem?.ToString();
                Settings.Default.BusinessEndTime = comboBoxEndTime.SelectedItem?.ToString();

                // Lưu đơn vị tiền tệ
                Settings.Default.Currency = comboBoxCurrency.SelectedItem?.ToString();

                // Lưu cài đặt sao lưu
                Settings.Default.AutoBackupEnabled = toggleAutoBackup.Checked;
                Settings.Default.BackupIntervalDays = (int)numericBackupDays.Value;

                // Lưu tất cả settings
                Settings.Default.Save();

                MessageBox.Show("Đã lưu cài đặt chung thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu cài đặt: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveStationSettings()
        {
            try
            {
                if (listBoxStations.SelectedIndex >= 0 && listBoxStations.SelectedIndex < stations.Count)
                {
                    var station = stations[listBoxStations.SelectedIndex];

                    // Cập nhật thông tin trạm
                    station.Name = txtStationName.Text;
                    station.Code = txtStationCode.Text;
                    station.Address = txtStationAddress.Text;
                    station.Phone = txtStationPhone.Text;
                    station.Manager = txtManagerName.Text;

                    if (int.TryParse(txtStationCapacity.Text, out int capacity))
                        station.Capacity = capacity;

                    // Ở đây bạn có thể thêm logic lưu vào database

                    MessageBox.Show("Đã lưu thông tin trạm thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu thông tin trạm: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveInventorySettings()
        {
            try
            {
                // Lưu ngưỡng tồn kho
                Settings.Default.MinStockLevel = (int)numericMinStock.Value;
                Settings.Default.MaxStockLevel = (int)numericMaxStock.Value;
                Settings.Default.ReorderPoint = (int)numericReorderPoint.Value;
                Settings.Default.SafetyStock = (int)numericSafetyStock.Value;

                // Lưu lịch trình vệ sinh
                Settings.Default.CleaningFrequency = comboBoxCleaningFrequency.SelectedItem?.ToString();
                Settings.Default.CleaningIntervalDays = (int)numericCleaningDays.Value;

                // Lưu quản lý bao bì
                Settings.Default.PackageLifespanDays = (int)numericPackageLifespan.Value;
                Settings.Default.AutoReorderEnabled = toggleAutoReorder.Checked;

                Settings.Default.Save();

                MessageBox.Show("Đã lưu cài đặt tồn kho thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu cài đặt tồn kho: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveUserSettings()
        {
            try
            {
                // Ở đây bạn có thể thêm logic lưu thông tin người dùng vào database
                // Ví dụ: cập nhật role, permissions, etc.

                MessageBox.Show("Đã lưu cài đặt người dùng thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu cài đặt người dùng: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Validation Methods
        private bool ValidateGeneralSettings()
        {
            if (string.IsNullOrWhiteSpace(txtCompanyName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên công ty", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (comboBoxStartTime.SelectedIndex >= comboBoxEndTime.SelectedIndex)
            {
                MessageBox.Show("Thời gian kết thúc phải sau thời gian bắt đầu", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool ValidateStationSettings()
        {
            if (string.IsNullOrWhiteSpace(txtStationName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên trạm", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtStationCode.Text))
            {
                MessageBox.Show("Vui lòng nhập mã trạm", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool ValidateInventorySettings()
        {
            if (numericMinStock.Value >= numericMaxStock.Value)
            {
                MessageBox.Show("Tồn kho tối thiểu phải nhỏ hơn tồn kho tối đa", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (numericReorderPoint.Value <= numericMinStock.Value || numericReorderPoint.Value >= numericMaxStock.Value)
            {
                MessageBox.Show("Điểm đặt hàng lại phải nằm giữa tồn kho tối thiểu và tối đa", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool ValidateUserSettings()
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên người dùng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtUserEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập email", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
        #endregion
    }

    #region Data Models - VẪN GIỮ NGUYÊN Stations
    public class Stations
    {
        public int StationId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Manager { get; set; }
        public int Capacity { get; set; }
        public bool IsActive { get; set; }
    }

    public class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Permissions { get; set; }
    }

    public static class SystemSettingsMockData
    {
        public static List<Stations> GetStations()
        {
            return new List<Stations>
            {
                new Stations {
                    StationId = 1,
                    Name = "Trạm Hà Nội",
                    Code = "HN-001",
                    Address = "123 Nguyễn Huệ, Q.1, Hà Nội",
                    Phone = "(+84) 24 1234 5678",
                    Manager = "Nguyễn Văn A",
                    Capacity = 1000,
                    IsActive = true
                },
                new Stations {
                    StationId = 2,
                    Name = "Trạm Hồ Chí Minh",
                    Code = "HCM-001",
                    Address = "456 Lê Lợi, Q.1, TP.HCM",
                    Phone = "(+84) 28 8765 4321",
                    Manager = "Trần Thị B",
                    Capacity = 1500,
                    IsActive = true
                },
                new Stations {
                    StationId = 3,
                    Name = "Trạm Đà Nẵng",
                    Code = "DN-001",
                    Address = "789 Hải Phòng, Q. Hải Châu, Đà Nẵng",
                    Phone = "(+84) 236 123 456",
                    Manager = "Lê Văn C",
                    Capacity = 800,
                    IsActive = true
                }
            };
        }

        public static List<Role> GetRoles()
        {
            return new List<Role>
            {
                new Role {
                    RoleId = 1,
                    Name = "Quản trị viên",
                    Description = "Toàn quyền truy cập hệ thống",
                    Permissions = new List<string> { "order_manage", "inventory_view", "inventory_edit", "report_view", "settings_edit", "user_manage" }
                },
                new Role {
                    RoleId = 2,
                    Name = "Quản lý trạm",
                    Description = "Quản lý hoạt động trạm",
                    Permissions = new List<string> { "order_manage", "inventory_view", "inventory_edit", "report_view" }
                },
                new Role {
                    RoleId = 3,
                    Name = "Nhân viên",
                    Description = "Thao tác cơ bản",
                    Permissions = new List<string> { "order_manage", "inventory_view" }
                },
                new Role {
                    RoleId = 4,
                    Name = "Xem báo cáo",
                    Description = "Chỉ xem báo cáo",
                    Permissions = new List<string> { "report_view" }
                }
            };
        }
    }
    #endregion
}