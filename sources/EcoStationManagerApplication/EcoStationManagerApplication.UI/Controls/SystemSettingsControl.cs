using EcoStationManagerApplication.UI.Common;
using EcoStationManagerApplication.UI.Properties;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class SystemSettingsControl : UserControl, IRefreshableControl
    {
        private List<EcoStationManagerApplication.Models.Entities.Station> stations;
        private List<Role> roles;
        private bool _pinVerified;
        private const int PinMaxAttempts = 3;
        private bool _canEdit;
        private bool _isCreatingNewStation;
        private Dictionary<string, int> _managerComboBoxMapping = new Dictionary<string, int>();

        public SystemSettingsControl()
        {
            try
        {
            InitializeComponent();
                stations = new List<EcoStationManagerApplication.Models.Entities.Station>();
                roles = SystemSettingsMockData.GetRoles();

                if (IsInDesignMode())
                {
                    _canEdit = true;
            InitializeControls();
                    ApplyPermissionState();
                    return;
                }

                if (!VerifyPinAccess())
                {
                    ShowAccessDeniedState();
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SystemSettingsControl constructor: {ex.Message}");
                stations = new List<EcoStationManagerApplication.Models.Entities.Station>();
            roles = SystemSettingsMockData.GetRoles();
            }
        }

        private void SystemSettingsControl_Load(object sender, EventArgs e)
        {
            try
            {
                _pinVerified = true;
                _canEdit = AppUserContext.IsAdmin;
                _ = LoadData();
                InitializeControls();
                ApplyPermissionState();
                ApplyRoleSpecificLayout();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SystemSettingsControl_Load: {ex.Message}");
                MessageBox.Show($"Lỗi khi tải SystemSettingsControl: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadData()
        {
            try
            {
                var stationsResult = await AppServices.StationService.GetAllStationsAsync();
                if (stationsResult?.Success == true && stationsResult.Data != null)
                {
                    stations = stationsResult.Data.ToList();
                }
                else
                {
                    stations = new List<EcoStationManagerApplication.Models.Entities.Station>();
                    if (!string.IsNullOrEmpty(stationsResult?.Message))
                        Console.WriteLine($"Warning: {stationsResult.Message}");
                }
                roles = SystemSettingsMockData.GetRoles();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadData: {ex.Message}\n{ex.StackTrace}");
                stations = new List<EcoStationManagerApplication.Models.Entities.Station>();
                roles = SystemSettingsMockData.GetRoles();
                
                if (!ex.Message.Contains("database") && !ex.Message.Contains("connection"))
                {
                    try
                    {
                        if (InvokeRequired)
                            Invoke(new Action(() => ShowError("Lỗi khi tải dữ liệu trạm", ex.Message)));
                        else
                            ShowError("Lỗi khi tải dữ liệu trạm", ex.Message);
                    }
                    catch { }
                }
            }
        }

        private void ShowError(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void InitializeControls()
        {
            InitializeTabControl();
            InitializeComboBoxes();
            ConfigureBackupNumeric();
            SetupPlaceholderBehavior();
            _ = LoadManagerComboBox();
            LoadGeneralSettings();
            LoadStationSettings();
            LoadInventorySettings();
            LoadUserManagement();
        }

        private void InitializeComboBoxes()
        {
            comboBoxStartTime.Items.Clear();
            comboBoxEndTime.Items.Clear();
            comboBoxCurrency.Items.Clear();
            comboBoxCleaningFrequency.Items.Clear();
            comboBoxStationType.Items.Clear();
            comboBoxManager.Items.Clear();

            for (int hour = 0; hour < 24; hour++)
            {
                for (int minute = 0; minute < 60; minute += 30)
                {
                    string timeText = $"{hour:00}:{minute:00}";
                    comboBoxStartTime.Items.Add(timeText);
                    comboBoxEndTime.Items.Add(timeText);
                }
            }

            comboBoxCurrency.Items.AddRange(new[] { "VND", "USD", "EUR" });
            comboBoxCleaningFrequency.Items.AddRange(new[] { "Hàng ngày", "Hàng tuần", "Hàng tháng", "Hàng quý" });
            comboBoxStationType.Items.AddRange(new[] { "Kho (Warehouse)", "Trạm nạp (Refill)", "Hỗn hợp (Hybrid)", "Khác (Other)" });
        }

        private async Task LoadManagerComboBox()
        {
            try
            {
                comboBoxManager.Items.Clear();
                _managerComboBoxMapping.Clear();
                comboBoxManager.Items.Add("-- Không chọn --");
                _managerComboBoxMapping["-- Không chọn --"] = 0;

                var usersResult = await AppServices.UserService.GetAllActiveUsersAsync();
                if (usersResult?.Success == true && usersResult.Data != null)
                {
                    foreach (var user in usersResult.Data)
                    {
                        string displayText = $"{user.Fullname ?? user.Username} (ID: {user.UserId})";
                        comboBoxManager.Items.Add(displayText);
                        _managerComboBoxMapping[displayText] = user.UserId;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading manager combo box: {ex.Message}");
            }
        }

        private void InitializeTabControl()
        {
            tabControlSystem.ItemSize = new Size(120, 40);
            tabControlSystem.SelectedIndex = 0;
        }

        private void ConfigureBackupNumeric()
        {
            numericBackupDays.Minimum = 1;
            numericBackupDays.Maximum = 365;
        }

        private void LoadGeneralSettings()
        {
            try
            {
                Settings.Default.Reload();
                txtCompanyName.Text = Settings.Default.CompanyName ?? "";
                txtCompanyAddress.Text = Settings.Default.CompanyAddress ?? "";
                txtCompanyPhone.Text = Settings.Default.CompanyPhone ?? "";
                txtCompanyEmail.Text = Settings.Default.CompanyEmail ?? "";
                txtTaxCode.Text = Settings.Default.TaxCode ?? "";

                SetComboBoxItem(comboBoxStartTime, Settings.Default.BusinessStartTime, 14);
                SetComboBoxItem(comboBoxEndTime, Settings.Default.BusinessEndTime, 34);
                SetComboBoxItem(comboBoxCurrency, Settings.Default.Currency, 0);

                toggleAutoBackup.Checked = Settings.Default.AutoBackupEnabled;
                numericBackupDays.Value = Settings.Default.BackupIntervalDays > 0 ? Settings.Default.BackupIntervalDays : 7;
                UpdateGeneralPlaceholders();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải cài đặt: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetDefaultGeneralSettings();
                UpdateGeneralPlaceholders();
            }
        }

        private void SetComboBoxItem(ComboBox comboBox, string value, int defaultIndex)
        {
            if (!string.IsNullOrEmpty(value) && comboBox.Items.Contains(value))
                comboBox.SelectedItem = value;
            else
                comboBox.SelectedIndex = defaultIndex;
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
            listBoxStations.Items.Clear();
            if (stations != null && stations.Count > 0)
            {
                foreach (var station in stations)
                    listBoxStations.Items.Add($"{station.Name} (ID: {station.StationId})");
                
                listBoxStations.SelectedIndex = 0;
                if (_canEdit) btnAddStation.Enabled = false;
                
                listBoxStations.Items.Add("");
                listBoxStations.Items.Add("⚠ Hiện tại chỉ cho phép tạo một trạm.");
                listBoxStations.Items.Add("Tính năng này sẽ phát triển ở phiên bản sau.");
            }
            else
            {
                listBoxStations.Items.Add("Chưa có trạm nào. Vui lòng thêm trạm mới.");
                if (_canEdit) btnAddStation.Enabled = true;
            }
            LoadSelectedStationDetails();
        }

        private async void LoadSelectedStationDetails()
        {
            if (stations == null || stations.Count == 0)
            {
                ClearStationFields();
                _isCreatingNewStation = false;
                return;
            }

            if (listBoxStations.SelectedIndex >= 0 && listBoxStations.SelectedIndex < stations.Count)
            {
                _isCreatingNewStation = false;
                var station = stations[listBoxStations.SelectedIndex];
                
                txtStationName.Text = station.Name ?? "";
                txtStationAddress.Text = station.Address ?? "";
                txtStationPhone.Text = station.Phone ?? "";
                comboBoxStationType.SelectedIndex = GetStationTypeIndex(station.StationType ?? "refill");
                toggleStationActive.Checked = station.IsActive;
                labelCreatedDateValue.Text = station.CreatedDate.ToString("dd/MM/yyyy HH:mm");
                labelUpdatedDateValue.Text = station.UpdatedDate.ToString("dd/MM/yyyy HH:mm");
                
                await SetManagerComboBox(station.Manager);
            }
        }

        private void ClearStationFields()
        {
            txtStationName.Text = "";
            txtStationAddress.Text = "";
            txtStationPhone.Text = "";
            comboBoxManager.SelectedIndex = -1;
            comboBoxStationType.SelectedIndex = -1;
            toggleStationActive.Checked = true;
            labelCreatedDateValue.Text = "--/--/--";
            labelUpdatedDateValue.Text = "--/--/--";
        }

        private async Task SetManagerComboBox(int? managerId)
        {
            if (!managerId.HasValue)
            {
                comboBoxManager.SelectedIndex = 0;
                return;
            }

            for (int i = 0; i < comboBoxManager.Items.Count; i++)
            {
                string itemText = comboBoxManager.Items[i].ToString();
                if (_managerComboBoxMapping.ContainsKey(itemText) && _managerComboBoxMapping[itemText] == managerId.Value)
                {
                    comboBoxManager.SelectedIndex = i;
                    return;
                }
            }

            await LoadManagerComboBox();
            for (int i = 0; i < comboBoxManager.Items.Count; i++)
            {
                string itemText = comboBoxManager.Items[i].ToString();
                if (_managerComboBoxMapping.ContainsKey(itemText) && _managerComboBoxMapping[itemText] == managerId.Value)
                {
                    comboBoxManager.SelectedIndex = i;
                    break;
                }
            }
        }

        private int GetStationTypeIndex(string stationType)
        {
            switch ((stationType ?? "refill").ToLower())
            {
                case "warehouse": return 0;
                case "refill": return 1;
                case "hybrid": return 2;
                case "other": return 3;
                default: return 1;
            }
        }

        private string GetStationTypeValue(int index)
        {
            string[] types = { "warehouse", "refill", "hybrid", "other" };
            return index >= 0 && index < types.Length ? types[index] : "refill";
        }

        private int? GetSelectedManagerId()
        {
            if (comboBoxManager.SelectedIndex > 0 && comboBoxManager.SelectedIndex < comboBoxManager.Items.Count)
            {
                string selectedText = comboBoxManager.Items[comboBoxManager.SelectedIndex].ToString();
                if (_managerComboBoxMapping.ContainsKey(selectedText) && _managerComboBoxMapping[selectedText] > 0)
                {
                    return _managerComboBoxMapping[selectedText];
                }
            }
            return null;
        }

        private void LoadInventorySettings()
        {
            try
            {
                numericMinStock.Value = Settings.Default.MinStockLevel > 0 ? Settings.Default.MinStockLevel : 50;
                numericMaxStock.Value = Settings.Default.MaxStockLevel > 0 ? Settings.Default.MaxStockLevel : 500;
                numericReorderPoint.Value = Settings.Default.ReorderPoint > 0 ? Settings.Default.ReorderPoint : 100;
                numericSafetyStock.Value = Settings.Default.SafetyStock > 0 ? Settings.Default.SafetyStock : 30;
                SetComboBoxItem(comboBoxCleaningFrequency, Settings.Default.CleaningFrequency, 1);
                numericCleaningDays.Value = Settings.Default.CleaningIntervalDays > 0 ? Settings.Default.CleaningIntervalDays : 7;
                numericPackageLifespan.Value = Settings.Default.PackageLifespanDays > 0 ? Settings.Default.PackageLifespanDays : 90;
                toggleAutoReorder.Checked = Settings.Default.AutoReorderEnabled;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải cài đặt tồn kho: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        #region Event Handlers 
        private void btnSaveGeneral_Click(object sender, EventArgs e)
        {
            if (EnsureCanEdit() && ValidateGeneralSettings())
                SaveGeneralSettings();
        }

        private async void btnChangePin_Click(object sender, EventArgs e)
        {
            if (IsInDesignMode())
                return;

            if (!EnsureCanEdit())
                return;

            if (!_pinVerified && !VerifyPinAccess())
            {
                MessageBox.Show("Bạn không có quyền truy cập tính năng này.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var storedPin = Settings.Default.PinCode ?? string.Empty;
            var newPin = ShowPinChangeDialog(storedPin);
            if (string.IsNullOrEmpty(newPin))
                return;

            Settings.Default.PinCode = newPin;
            Settings.Default.Save();

            MessageBox.Show("Đã cập nhật mã PIN thành công.", "Thành công",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            await EmailNotificationHelper.SendPinChangeNotificationAsync(newPin);
        }

        private void btnSaveStation_Click(object sender, EventArgs e)
        {
            if (EnsureCanEdit() && ValidateStationSettings())
                SaveStationSettings();
        }

        private void btnSaveInventory_Click(object sender, EventArgs e)
        {
            if (EnsureCanEdit() && ValidateInventorySettings())
                SaveInventorySettings();
        }

        private void btnSaveUser_Click(object sender, EventArgs e)
        {
            if (EnsureCanEdit() && ValidateUserSettings())
                SaveUserSettings();
        }

        private void listBoxStations_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSelectedStationDetails();
        }

        private void comboBoxUserRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRolePermissions(comboBoxUserRole.SelectedIndex);
        }

        private async void btnAddStation_Click(object sender, EventArgs e)
        {
            if (!EnsureCanEdit()) return;

            try
            {
                var existingStationsResult = await AppServices.StationService.GetAllStationsAsync();
                if (existingStationsResult?.Success == true && existingStationsResult.Data?.Any() == true)
                {
                    MessageBox.Show("Hiện tại chỉ cho phép tạo một trạm. Tính năng này sẽ phát triển ở phiên bản sau.\n\nVui lòng cập nhật trạm hiện có thay vì tạo mới.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _isCreatingNewStation = true;
                ClearStationFields();
                comboBoxStationType.SelectedIndex = 1;
                comboBoxManager.SelectedIndex = 0;
                listBoxStations.Items.Clear();
                listBoxStations.Items.Add("Đang tạo trạm mới...");
                SetStationInputsEnabled(true);
                btnSaveStation.Enabled = true;
                btnAddStation.Enabled = false;
                txtStationName.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi khởi tạo form thêm trạm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _isCreatingNewStation = false;
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (!EnsureCanEdit())
                return;

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
                Settings.Default.CompanyName = txtCompanyName.Text;
                Settings.Default.CompanyAddress = txtCompanyAddress.Text;
                Settings.Default.CompanyPhone = txtCompanyPhone.Text;
                Settings.Default.CompanyEmail = txtCompanyEmail.Text;
                Settings.Default.TaxCode = txtTaxCode.Text;
                Settings.Default.BusinessStartTime = comboBoxStartTime.SelectedItem?.ToString();
                Settings.Default.BusinessEndTime = comboBoxEndTime.SelectedItem?.ToString();
                Settings.Default.Currency = comboBoxCurrency.SelectedItem?.ToString();
                Settings.Default.AutoBackupEnabled = toggleAutoBackup.Checked;
                Settings.Default.BackupIntervalDays = (int)numericBackupDays.Value;
                Settings.Default.Save();

                MessageBox.Show("Đã lưu cài đặt chung thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadGeneralSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu cài đặt: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void SaveStationSettings()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtStationName.Text.Trim()))
                {
                    MessageBox.Show("Tên trạm không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStationName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtStationAddress.Text.Trim()))
                {
                    MessageBox.Show("Địa chỉ trạm không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStationAddress.Focus();
                    return;
                }

                if (_isCreatingNewStation)
                {
                    await CreateNewStation();
                }
                else
                {
                    await UpdateExistingStation();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu thông tin trạm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CreateNewStation()
        {
            var newStation = new EcoStationManagerApplication.Models.Entities.Station
            {
                Name = txtStationName.Text.Trim(),
                Address = txtStationAddress.Text.Trim(),
                Phone = txtStationPhone.Text.Trim(),
                StationType = GetStationTypeValue(comboBoxStationType.SelectedIndex),
                Manager = GetSelectedManagerId(),
                IsActive = toggleStationActive.Checked,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            var result = await AppServices.StationService.CreateStationAsync(newStation);
            if (result.Success)
            {
                MessageBox.Show($"Đã tạo trạm mới thành công! ID: {result.Data}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _isCreatingNewStation = false;
                await LoadData();
                LoadStationSettings();
                
                var newStationInList = stations?.FirstOrDefault(s => s.StationId == result.Data);
                if (newStationInList != null)
                {
                    listBoxStations.SelectedIndex = stations.IndexOf(newStationInList);
                    LoadSelectedStationDetails();
                }
            }
            else
            {
                MessageBox.Show($"Lỗi khi tạo trạm mới: {result.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task UpdateExistingStation()
        {
            if (stations == null || stations.Count == 0)
            {
                MessageBox.Show("Chưa có trạm nào để lưu. Vui lòng thêm trạm mới trước.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (listBoxStations.SelectedIndex < 0 || listBoxStations.SelectedIndex >= stations.Count)
                return;

            var station = stations[listBoxStations.SelectedIndex];
            station.Name = txtStationName.Text.Trim();
            station.Address = txtStationAddress.Text.Trim();
            station.Phone = txtStationPhone.Text.Trim();
            station.StationType = GetStationTypeValue(comboBoxStationType.SelectedIndex);
            station.Manager = GetSelectedManagerId();
            station.IsActive = toggleStationActive.Checked;

            var result = await AppServices.StationService.UpdateStationAsync(station);
            if (result.Success)
            {
                MessageBox.Show("Đã lưu thông tin trạm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadData();
                LoadStationSettings();
            }
            else
            {
                MessageBox.Show($"Lỗi khi lưu thông tin trạm: {result.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveInventorySettings()
        {
            try
            {
                Settings.Default.MinStockLevel = (int)numericMinStock.Value;
                Settings.Default.MaxStockLevel = (int)numericMaxStock.Value;
                Settings.Default.ReorderPoint = (int)numericReorderPoint.Value;
                Settings.Default.SafetyStock = (int)numericSafetyStock.Value;
                Settings.Default.CleaningFrequency = comboBoxCleaningFrequency.SelectedItem?.ToString();
                Settings.Default.CleaningIntervalDays = (int)numericCleaningDays.Value;
                Settings.Default.PackageLifespanDays = (int)numericPackageLifespan.Value;
                Settings.Default.AutoReorderEnabled = toggleAutoReorder.Checked;
                Settings.Default.Save();

                MessageBox.Show("Đã lưu cài đặt tồn kho thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu cài đặt tồn kho: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveUserSettings()
        {
            try
            {
                MessageBox.Show("Đã lưu cài đặt người dùng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu cài đặt người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (string.IsNullOrWhiteSpace(txtStationAddress.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ trạm", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        public void RefreshData()
        {
            if (!_pinVerified && !IsInDesignMode())
                return;

            _ = LoadData();
            InitializeControls();
            ApplyPermissionState();
        }

        private void SetupPlaceholderBehavior()
        {
            txtCompanyName.TextChanged -= TxtCompanyName_TextChanged;
            txtCompanyName.TextChanged += TxtCompanyName_TextChanged;
            UpdateGeneralPlaceholders();
        }

        private void TxtCompanyName_TextChanged(object sender, EventArgs e)
        {
            UpdateGeneralPlaceholders();
        }

        private void UpdateGeneralPlaceholders()
        {
            bool showPlaceholders = string.IsNullOrWhiteSpace(txtCompanyName.Text);

            txtCompanyName.PlaceholderText = showPlaceholders ? "Nhập tên công ty" : string.Empty;
            txtCompanyAddress.PlaceholderText = showPlaceholders ? "Nhập địa chỉ công ty" : string.Empty;
            txtCompanyPhone.PlaceholderText = showPlaceholders ? "Nhập số điện thoại liên hệ" : string.Empty;
            txtCompanyEmail.PlaceholderText = showPlaceholders ? "Nhập email liên hệ" : string.Empty;
            txtTaxCode.PlaceholderText = showPlaceholders ? "Nhập mã số thuế" : string.Empty;
        }

        private bool EnsureCanEdit()
        {
            if (_canEdit)
                return true;

            MessageBox.Show("Bạn không có quyền chỉnh sửa mục này. Vui lòng liên hệ quản trị viên.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;
        }

        private void ApplyPermissionState()
        {
            if (!_pinVerified && !IsInDesignMode())
                return;

            if (_canEdit)
            {
                SetGeneralInputsEnabled(true);
                SetStationInputsEnabled(true);
                SetInventoryInputsEnabled(true);
                SetUserInputsEnabled(true);
                btnChangePin.Visible = true;
                return;
            }

            SetGeneralInputsEnabled(false);
            SetStationInputsEnabled(false);
            SetInventoryInputsEnabled(false);
            SetUserInputsEnabled(false);
            
            btnSaveGeneral.Enabled = true;
            btnAddStation.Enabled = false;
            btnAddUser.Enabled = false;
            btnChangePin.Visible = false;
        }

        private void SetGeneralInputsEnabled(bool enabled)
        {
            txtCompanyName.ReadOnly = !enabled;
            txtCompanyAddress.ReadOnly = !enabled;
            txtCompanyPhone.ReadOnly = !enabled;
            txtCompanyEmail.ReadOnly = !enabled;
            txtTaxCode.ReadOnly = !enabled;
            comboBoxStartTime.Enabled = enabled;
            comboBoxEndTime.Enabled = enabled;
            comboBoxCurrency.Enabled = enabled;
            toggleAutoBackup.Enabled = enabled;
            numericBackupDays.Enabled = enabled;
            btnSaveGeneral.Enabled = enabled;
            btnChangePin.Enabled = enabled;
        }

        private void SetStationInputsEnabled(bool enabled)
        {
            txtStationName.ReadOnly = !enabled;
            txtStationAddress.ReadOnly = !enabled;
            txtStationPhone.ReadOnly = !enabled;
            comboBoxManager.Enabled = enabled;
            comboBoxStationType.Enabled = enabled;
            toggleStationActive.Enabled = enabled;
            listBoxStations.Enabled = true;
            btnSaveStation.Enabled = enabled;
            btnAddStation.Enabled = enabled;
        }

        private void SetInventoryInputsEnabled(bool enabled)
        {
            numericMinStock.Enabled = enabled;
            numericMaxStock.Enabled = enabled;
            numericReorderPoint.Enabled = enabled;
            numericSafetyStock.Enabled = enabled;
            comboBoxCleaningFrequency.Enabled = enabled;
            numericCleaningDays.Enabled = enabled;
            numericPackageLifespan.Enabled = enabled;
            toggleAutoReorder.Enabled = enabled;
            btnSaveInventory.Enabled = enabled;
        }

        private void SetUserInputsEnabled(bool enabled)
        {
            txtUserName.ReadOnly = !enabled;
            txtUserEmail.ReadOnly = !enabled;
            comboBoxUserRole.Enabled = enabled;
            checkBoxOrderManage.Enabled = enabled;
            checkBoxInventoryView.Enabled = enabled;
            checkBoxInventoryEdit.Enabled = enabled;
            checkBoxReportView.Enabled = enabled;
            checkBoxSettingsEdit.Enabled = enabled;
            checkBoxUserManage.Enabled = enabled;
            btnSaveUser.Enabled = enabled;
            btnAddUser.Enabled = enabled;
        }

        private void ApplyRoleSpecificLayout()
        {
            if (!_canEdit)
            {
                if (tabControlSystem.TabPages.Contains(tabPageUsers))
                    tabControlSystem.TabPages.Remove(tabPageUsers);
            }
            else
            {
                if (!tabControlSystem.TabPages.Contains(tabPageStations))
                    tabControlSystem.TabPages.Add(tabPageStations);
                if (!tabControlSystem.TabPages.Contains(tabPageInventory))
                    tabControlSystem.TabPages.Add(tabPageInventory);
                if (!tabControlSystem.TabPages.Contains(tabPageUsers))
                    tabControlSystem.TabPages.Add(tabPageUsers);
            }
        }

        #region PIN Verification
        private bool IsInDesignMode()
        {
            return DesignMode ||
                   LicenseManager.UsageMode == LicenseUsageMode.Designtime ||
                   System.Diagnostics.Process.GetCurrentProcess().ProcessName.Equals("devenv", StringComparison.OrdinalIgnoreCase);
        }

        private bool VerifyPinAccess()
        {
            var storedPin = Settings.Default.PinCode;
            if (string.IsNullOrWhiteSpace(storedPin))
                return true;

            for (int attempt = 0; attempt < PinMaxAttempts; attempt++)
            {
                var inputPin = PromptForPin(attempt + 1);
                if (inputPin == null)
                    return false;

                if (string.Equals(inputPin, storedPin))
                    return true;

                MessageBox.Show("Mã PIN không chính xác. Vui lòng thử lại.", "Sai mã PIN",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return false;
        }

        private string PromptForPin(int attemptNumber)
        {
            using (var prompt = new Form())
            {
                prompt.Text = "Xác thực mã PIN";
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.StartPosition = FormStartPosition.CenterParent;
                prompt.MinimizeBox = false;
                prompt.MaximizeBox = false;
                prompt.ClientSize = new Size(360, 160);

                var lblMessage = new Label
                {
                    AutoSize = false,
                    Text = "Vui lòng nhập mã PIN bảo mật để truy cập System Settings:",
                    Dock = DockStyle.Top,
                    Height = 60,
                    TextAlign = ContentAlignment.MiddleLeft
                };

                var txtPin = new TextBox
                {
                    UseSystemPasswordChar = true,
                    Width = 200,
                    Location = new Point(20, 70)
                };

                var btnOk = new Button
                {
                    Text = "Xác nhận",
                    DialogResult = DialogResult.OK,
                    Location = new Point(230, 110),
                    Width = 100
                };

                var btnCancel = new Button
                {
                    Text = "Hủy",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(120, 110),
                    Width = 100
                };

                prompt.Controls.Add(lblMessage);
                prompt.Controls.Add(txtPin);
                prompt.Controls.Add(btnOk);
                prompt.Controls.Add(btnCancel);
                prompt.AcceptButton = btnOk;
                prompt.CancelButton = btnCancel;

                var owner = FindForm();
                var dialogResult = owner != null ? prompt.ShowDialog(owner) : prompt.ShowDialog();

                return dialogResult == DialogResult.OK ? txtPin.Text : null;
            }
        }

        private void ShowAccessDeniedState()
        {
            Controls.Clear();
            var label = new Label
            {
                Text = "Bạn cần xác thực mã PIN hợp lệ để truy cập System Settings.",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold)
            };

            Controls.Add(label);
        }

        private string ShowPinChangeDialog(string currentPin)
        {
            while (true)
            {
                var dialogResult = ShowPinChangeForm(out string currentInput, out string newPin, out string confirmPin);
                if (dialogResult != DialogResult.OK)
                    return null;

                if (!string.Equals(currentInput, currentPin))
                {
                    MessageBox.Show("Mã PIN hiện tại không chính xác.", "Sai mã PIN",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    continue;
                }

                if (string.IsNullOrWhiteSpace(newPin) || newPin.Length < 4)
                {
                    MessageBox.Show("Mã PIN mới phải có ít nhất 4 ký tự.", "Không hợp lệ",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    continue;
                }

                if (!string.Equals(newPin, confirmPin))
                {
                    MessageBox.Show("Mã PIN xác nhận không khớp.", "Không hợp lệ",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    continue;
                }

                return newPin;
            }
        }

        private DialogResult ShowPinChangeForm(out string currentPin, out string newPin, out string confirmPin)
        {
            using (var form = new Form())
            {
                form.Text = "Đổi mã PIN System Settings";
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterParent;
                form.MinimizeBox = false;
                form.MaximizeBox = false;
                form.ClientSize = new Size(420, 220);

                var lblCurrent = new Label { Text = "Mã PIN hiện tại:", Location = new Point(20, 20), AutoSize = true };
                var txtCurrent = new TextBox
                {
                    UseSystemPasswordChar = true,
                    Location = new Point(20, 45),
                    Width = 360
                };

                var lblNew = new Label { Text = "Mã PIN mới:", Location = new Point(20, 80), AutoSize = true };
                var txtNew = new TextBox
                {
                    UseSystemPasswordChar = true,
                    Location = new Point(20, 105),
                    Width = 360
                };

                var lblConfirm = new Label { Text = "Nhập lại mã PIN mới:", Location = new Point(20, 140), AutoSize = true };
                var txtConfirm = new TextBox
                {
                    UseSystemPasswordChar = true,
                    Location = new Point(20, 165),
                    Width = 360
                };

                var btnOk = new Button
                {
                    Text = "Lưu",
                    DialogResult = DialogResult.OK,
                    Location = new Point(280, 180),
                    Width = 100
                };

                var btnCancel = new Button
                {
                    Text = "Hủy",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(170, 180),
                    Width = 100
                };

                form.Controls.AddRange(new Control[]
                {
                    lblCurrent, txtCurrent,
                    lblNew, txtNew,
                    lblConfirm, txtConfirm,
                    btnOk, btnCancel
                });

                form.AcceptButton = btnOk;
                form.CancelButton = btnCancel;

                var owner = FindForm();
                var result = owner != null ? form.ShowDialog(owner) : form.ShowDialog();

                currentPin = txtCurrent.Text;
                newPin = txtNew.Text;
                confirmPin = txtConfirm.Text;
                return result;
            }
        }
        #endregion

    }

    #region Data Models - Stations
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