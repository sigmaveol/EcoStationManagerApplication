using EcoStationManagerApplication.UI.Common;
using EcoStationManagerApplication.UI.Properties;
using Guna.UI2.WinForms;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.Common.Helpers;
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
        private List<Station> stations;
        private List<Role> roles;
        private bool _pinVerified;
        private const int PinMaxAttempts = 3;
        private bool _canEdit;
        private bool _isCreatingNewStation;
        private bool _isCreatingNewUser;
        private Dictionary<string, int> _managerComboBoxMapping = new Dictionary<string, int>();
        private List<User> users;
        private Label labelStationWarning;

        public SystemSettingsControl()
        {
            try
            {
                InitializeComponent();
                stations = new List<Station>();
                users = new List<User>();
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
                stations = new List<Station>();
                roles = SystemSettingsMockData.GetRoles();
            }
        }

        private void SystemSettingsControl_Load(object sender, EventArgs e)
        {
            try
            {
                _pinVerified = true;
                _canEdit = AppServices.State.IsAdmin || AppServices.State.IsManager;
                InitializeControls();
                ApplyPermissionState();
                ApplyRoleSpecificLayout();
                _ = LoadData();
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
                    stations = new List<Station>();
                    if (!string.IsNullOrEmpty(stationsResult?.Message))
                        Console.WriteLine($"Warning: {stationsResult.Message}");
                }
                
                // Load users
                var usersResult = await AppServices.UserService.GetAllAsync();
                if (usersResult?.Success == true && usersResult.Data != null)
                {
                    users = usersResult.Data.ToList();
                }
                else
                {
                    users = new List<User>();
                    if (!string.IsNullOrEmpty(usersResult?.Message))
                        Console.WriteLine($"Warning loading users: {usersResult.Message}");
                }

                // Load roles
                roles = SystemSettingsMockData.GetRoles();

                // Refresh users grid if control is loaded
                if (InvokeRequired)
                {
                    Invoke(new Action(() => { 
                        LoadUsersGrid();
                        LoadStationSettings();
                    }));
                }
                else
                {
                    LoadUsersGrid();
                    LoadStationSettings();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadData: {ex.Message}\n{ex.StackTrace}");
                
                // Initialize empty lists on error
                stations = new List<Station>();
                users = new List<User>();
                roles = SystemSettingsMockData.GetRoles();

                // Refresh users grid even on error
                try
                {
                    if (InvokeRequired)
                        Invoke(new Action(() => LoadUsersGrid()));
                    else
                        LoadUsersGrid();
                }
                catch { }

                // Show error message if not a database/connection error
                if (!ex.Message.Contains("database") && !ex.Message.Contains("connection"))
                {
                    try
                    {
                        if (InvokeRequired)
                            Invoke(new Action(() => ShowError("Lỗi khi tải dữ liệu", ex.Message)));
                        else
                            ShowError("Lỗi khi tải dữ liệu", ex.Message);
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
                        string displayText = $"{user.Fullname ?? user.Username}";
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

        public void RefreshData()
        {
            if (!_pinVerified && !IsInDesignMode())
                return;

            _ = LoadData();
            InitializeControls();
            ApplyPermissionState();
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
    }
}
