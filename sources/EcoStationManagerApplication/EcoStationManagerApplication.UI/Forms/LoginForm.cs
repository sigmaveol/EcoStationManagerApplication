using EcoStationManagerApplication.UI.Common;
using EcoStationManagerApplication.UI.Forms;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            // Ẩn label lỗi ban đầu
            labelError.Text = "";
            labelError.Visible = false;

            // Focus vào ô username khi form load
            this.Shown += (s, e) => txtUsername.Focus();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // Load thông tin đã lưu nếu có (Remember me)
            LoadSavedCredentials();
        }

        private void LoadSavedCredentials()
        {
            // TODO: Implement load saved credentials from settings if needed
            // For now, leave empty
        }

        private void SaveCredentials()
        {
            // TODO: Implement save credentials to settings if Remember me is checked
            // For now, leave empty
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            await PerformLogin();
        }

        private async Task PerformLogin()
        {
            // Ẩn thông báo lỗi cũ
            labelError.Visible = false;
            labelError.Text = "";

            // Validate input
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                ShowError("Vui lòng nhập tên đăng nhập.");
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ShowError("Vui lòng nhập mật khẩu.");
                txtPassword.Focus();
                return;
            }

            // Disable controls during login
            SetControlsEnabled(false);
            btnLogin.Text = "Đang đăng nhập...";

            try
            {
                // Authenticate user
                var result = await AppServices.UserService.AuthenticateAsync(
                    txtUsername.Text.Trim(),
                    txtPassword.Text
                );

                if (result.Success && result.Data != null)
                {
                    var user = result.Data;
                    
                    // Lưu thông tin user vào AppUserContext
                    AppUserContext.CurrentUserId = user.UserId;
                    AppUserContext.CurrentUsername = user.Username;
                    AppUserContext.CurrentUserRole = user.Role;

                    // Save credentials if Remember me is checked
                    if (checkBoxRemember.Checked)
                    {
                        SaveCredentials();
                    }

                    // Show success message
                    UIHelper.ShowSuccessMessage(result.Message ?? "Đăng nhập thành công!");

                    // Open MainForm
                    this.Hide();
                    var mainForm = new MainForm();
                    mainForm.FormClosed += (s, args) => 
                    {
                        AppUserContext.Clear(); // Clear context khi đóng MainForm
                        this.Close();
                    };
                    mainForm.Show();
                }
                else
                {
                    // Show error message
                    ShowError(result.Message ?? "Tên đăng nhập hoặc mật khẩu không đúng.");
                    txtPassword.Focus();
                    txtPassword.SelectAll();
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "đăng nhập");
                ShowError("Đã xảy ra lỗi khi đăng nhập. Vui lòng thử lại.");
            }
            finally
            {
                // Re-enable controls
                SetControlsEnabled(true);
                btnLogin.Text = "Đăng nhập";
            }
        }

        private void ShowError(string message)
        {
            labelError.Text = message;
            labelError.Visible = true;
        }

        private void SetControlsEnabled(bool enabled)
        {
            txtUsername.Enabled = enabled;
            txtPassword.Enabled = enabled;
            checkBoxRemember.Enabled = enabled;
            btnLogin.Enabled = enabled;
            btnExit.Enabled = enabled;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (UIHelper.ShowConfirmDialog("Bạn có chắc chắn muốn thoát ứng dụng?", "Xác nhận thoát"))
            {
                Application.Exit();
            }
        }

        private void checkBoxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !checkBoxShowPassword.Checked;
            txtPassword.PasswordChar = checkBoxShowPassword.Checked ? '\0' : '●';
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                txtPassword.Focus();
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnLogin.PerformClick();
            }
        }
    }
}
