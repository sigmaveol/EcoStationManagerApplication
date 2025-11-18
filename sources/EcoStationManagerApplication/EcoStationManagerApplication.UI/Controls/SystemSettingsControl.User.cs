using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.UI.Common;
using EcoStationManagerApplication.UI.Helpers;
using EcoStationManagerApplication.UI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class SystemSettingsControl
    {
        #region User Management Methods

        private void LoadUserManagement()
        {
            // Load roles từ UserRole enum
            comboBoxUserRole.Items.Clear();
            foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                comboBoxUserRole.Items.Add(RolePermissionHelper.GetRoleDisplayName(role));
            }
            if (comboBoxUserRole.Items.Count > 0)
                comboBoxUserRole.SelectedIndex = 0;

            // Load permissions for selected role
            LoadRolePermissions(0);

            // Load users into grid
            LoadUsersGrid();
        }

        private void LoadUsersGrid()
        {
            try
            {
                // Check if control is initialized
                if (dataGridUsers == null)
                    return;

                dataGridUsers.DataSource = null;
                dataGridUsers.Rows.Clear();
                dataGridUsers.Columns.Clear();

                if (users == null || users.Count == 0)
                {
                    return;
                }

                // Define columns
                dataGridUsers.AutoGenerateColumns = false;

                dataGridUsers.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "UserId",
                    HeaderText = "ID",
                    DataPropertyName = "UserId",
                    Visible = false
                });
                dataGridUsers.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Username",
                    HeaderText = "Tên đăng nhập",
                    DataPropertyName = "Username",
                    Width = 140,
                    ReadOnly = true
                });
                dataGridUsers.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Fullname",
                    HeaderText = "Họ tên",
                    DataPropertyName = "Fullname",
                    Width = 180,
                    ReadOnly = true
                });
                dataGridUsers.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Role",
                    HeaderText = "Vai trò",
                    DataPropertyName = "Role",
                    Width = 140,
                    ReadOnly = true
                });
                dataGridUsers.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "IsActive",
                    HeaderText = "Trạng thái",
                    DataPropertyName = "IsActive",
                    Width = 120,
                    ReadOnly = true
                });
                dataGridUsers.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "CreatedDate",
                    HeaderText = "Ngày tạo",
                    DataPropertyName = "CreatedDate",
                    Width = 150,
                    ReadOnly = true
                });
                var editColumn = new DataGridViewButtonColumn
                {
                    Name = "Edit",
                    HeaderText = "Chỉnh sửa",
                    Text = "Sửa",
                    UseColumnTextForButtonValue = true,
                    Width = 100
                };
                dataGridUsers.Columns.Add(editColumn);
                dataGridUsers.ReadOnly = false;

                // Create data source with formatted data
                var userList = users.Select(u => new
                {
                    u.UserId,
                    u.Username,
                    Fullname = u.Fullname ?? "",
                    Role = RolePermissionHelper.GetRoleDisplayName(u.Role),
                    IsActive = u.IsActive == ActiveStatus.ACTIVE ? "Hoạt động" : "Vô hiệu hóa",
                    CreatedDate = u.CreatedDate.ToString("dd/MM/yyyy HH:mm")
                }).ToList();

                dataGridUsers.DataSource = userList;

                dataGridUsers.CellContentClick -= DataGridUsers_CellContentClick;
                dataGridUsers.CellContentClick += DataGridUsers_CellContentClick;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading users grid: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Lỗi khi tải danh sách người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRolePermissions(int roleIndex)
        {
            if (roleIndex >= 0 && roleIndex < Enum.GetValues(typeof(UserRole)).Length)
            {
                UserRole selectedRole = (UserRole)roleIndex;
                var permissions = RolePermissionHelper.GetPermissions(selectedRole);

                checkBoxOrderManage.Checked = permissions.Contains(RolePermissionHelper.ORDER_MANAGE);
                checkBoxInventoryView.Checked = permissions.Contains(RolePermissionHelper.INVENTORY_VIEW);
                checkBoxInventoryEdit.Checked = permissions.Contains(RolePermissionHelper.INVENTORY_EDIT);
                checkBoxReportView.Checked = permissions.Contains(RolePermissionHelper.REPORT_VIEW);
                checkBoxSettingsEdit.Checked = permissions.Contains(RolePermissionHelper.SETTINGS_EDIT);
                checkBoxUserManage.Checked = permissions.Contains(RolePermissionHelper.USER_MANAGE);
            }
        }

        private async void SaveUserSettings()
        {
            try
            {
                if (_isCreatingNewUser)
                {
                    await CreateNewUser();
                }
                else
                {
                    await UpdateExistingUser();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu thông tin người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CreateNewUser()
        {
            if (comboBoxUserRole.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn vai trò cho người dùng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxUserRole.Focus();
                return;
            }

            string username = txtUserName.Text.Trim();
            string password = txtUserPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUserName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUserPassword.Focus();
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUserPassword.Focus();
                return;
            }

            try
            {
                UserRole selectedRole = (UserRole)comboBoxUserRole.SelectedIndex;
                var newUser = new User
                {
                    Username = username,
                    Fullname = username,
                    Role = selectedRole,
                    IsActive = toggleActive?.Checked == false ? ActiveStatus.INACTIVE : ActiveStatus.ACTIVE,
                    CreatedDate = DateTime.Now
                };

                var result = await AppServices.UserService.CreateUserAsync(newUser, password);
                if (result.Success)
                {
                    MessageBox.Show($"Đã tạo người dùng '{username}' thành công!",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _isCreatingNewUser = false;
                    await LoadData();
                    LoadUserManagement();
                    ClearUserFields();
                }
                else
                {
                    MessageBox.Show($"Lỗi khi tạo người dùng: {result.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task UpdateExistingUser()
        {
            if (comboBoxUserRole.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn vai trò cho người dùng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxUserRole.Focus();
                return;
            }

            string username = txtUserName.Text.Trim();
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập cần cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUserName.Focus();
                return;
            }

            var user = users?.FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
            if (user == null)
            {
                MessageBox.Show("Không tìm thấy người dùng tương ứng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                user.Role = (UserRole)comboBoxUserRole.SelectedIndex;
                user.IsActive = toggleActive.Checked ? ActiveStatus.ACTIVE : ActiveStatus.INACTIVE;

                var updateResult = await AppServices.UserService.UpdateUserAsync(user);
                if (!updateResult.Success)
                {
                    MessageBox.Show($"Lỗi khi cập nhật người dùng: {updateResult.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string newPassword = txtUserPassword.Text.Trim();
                if (!string.IsNullOrWhiteSpace(newPassword))
                {
                    if (newPassword.Length < 6)
                    {
                        MessageBox.Show("Mật khẩu mới phải có ít nhất 6 ký tự!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtUserPassword.Focus();
                        return;
                    }

                    var resetResult = await AppServices.UserService.ResetPasswordAsync(user.UserId, newPassword);
                    if (!resetResult.Success)
                    {
                        MessageBox.Show($"Lỗi khi đổi mật khẩu: {resetResult.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                MessageBox.Show($"Đã cập nhật người dùng '{user.Username}' thành công!",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _isCreatingNewUser = false;
                await LoadData();
                LoadUserManagement();
                ClearUserFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearUserFields()
        {
            txtUserName.Text = "";
            txtUserName.ReadOnly = false;
            txtUserPassword.Text = "";
            comboBoxUserRole.SelectedIndex = -1;
            checkBoxOrderManage.Checked = false;
            checkBoxInventoryView.Checked = false;
            checkBoxInventoryEdit.Checked = false;
            checkBoxReportView.Checked = false;
            checkBoxSettingsEdit.Checked = false;
            checkBoxUserManage.Checked = false;
            if (toggleActive != null)
                toggleActive.Checked = true;
        }

        private bool ValidateUserSettings()
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUserName.Focus();
                return false;
            }
            if (txtUserName.Text.Length < 3)
            {
                MessageBox.Show("Tên đăng nhập phải có ít nhất 3 ký tự", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUserName.Focus();
                return false;
            }
            if (comboBoxUserRole.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn vai trò cho người dùng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                comboBoxUserRole.Focus();
                return false;
            }
            return true;
        }

        private void SetUserInputsEnabled(bool enabled)
        {
            txtUserName.ReadOnly = !enabled;
            txtUserPassword.ReadOnly = !enabled;
            comboBoxUserRole.Enabled = enabled;
            checkBoxOrderManage.Enabled = enabled;
            checkBoxInventoryView.Enabled = enabled;
            checkBoxInventoryEdit.Enabled = enabled;
            checkBoxReportView.Enabled = enabled;
            checkBoxSettingsEdit.Enabled = enabled;
            checkBoxUserManage.Enabled = enabled;
            btnSaveUser.Enabled = enabled;
            btnAddUser.Enabled = enabled;
            if (toggleActive != null)
                toggleActive.Enabled = enabled;
        }

        private void DataGridUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (dataGridUsers.Columns[e.ColumnIndex].Name == "Edit")
            {
                if (!EnsureCanEdit())
                    return;

                var userIdValue = dataGridUsers.Rows[e.RowIndex].Cells["UserId"].Value;
                if (userIdValue == null || !int.TryParse(userIdValue.ToString(), out int userId))
                {
                    MessageBox.Show("Không thể xác định người dùng cần chỉnh sửa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                LoadUserForEdit(userId);
            }
        }

        private void LoadUserForEdit(int userId)
        {
            var user = users?.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                MessageBox.Show("Không tìm thấy người dùng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _isCreatingNewUser = false;
            txtUserName.Text = user.Username;
            txtUserName.ReadOnly = true;
            txtUserPassword.Text = "";
            comboBoxUserRole.SelectedIndex = (int)user.Role;
            if (toggleActive != null)
                toggleActive.Checked = user.IsActive == ActiveStatus.ACTIVE;
            LoadRolePermissions(comboBoxUserRole.SelectedIndex);

            btnSaveUser.Enabled = true;
            btnAddUser.Enabled = false;
            txtUserPassword.Focus();
        }

        #endregion

        #region Inventory Settings Methods

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

        #endregion

        #region PIN Verification Methods

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

        #region User Management Event Handlers

        private void btnSaveUser_Click(object sender, EventArgs e)
        {
            if (EnsureCanEdit() && ValidateUserSettings())
                SaveUserSettings();
        }

        private void comboBoxUserRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxUserRole.SelectedIndex >= 0)
            {
                LoadRolePermissions(comboBoxUserRole.SelectedIndex);
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (!EnsureCanEdit())
                return;

            try
            {
                _isCreatingNewUser = true;
                ClearUserFields();
                comboBoxUserRole.SelectedIndex = 1; // Mặc định là STAFF (ADMIN=0, STAFF=1, MANAGER=2, DRIVER=3)
                SetUserInputsEnabled(true);
                btnSaveUser.Enabled = true;
                btnAddUser.Enabled = false;
                txtUserName.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi khởi tạo form thêm người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _isCreatingNewUser = false;
            }
        }

        #endregion

        #region Inventory Settings Event Handlers

        private void btnSaveInventory_Click(object sender, EventArgs e)
        {
            if (EnsureCanEdit() && ValidateInventorySettings())
                SaveInventorySettings();
        }

        #endregion

        #region PIN Verification Event Handlers

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

        #endregion
    }

    #region Data Models
    public class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Permissions { get; set; }
    }

    public static class SystemSettingsMockData
    {
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

