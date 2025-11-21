using EcoStationManagerApplication.UI.Common;
using EcoStationManagerApplication.UI.Common.Services;
using EcoStationManagerApplication.UI.Forms;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI
{
    public partial class LoginForm : Form
    {
        private System.Timers.Timer _countdownTimer;
        private int _countdownSeconds = 3;
        private Label _countdownLabel;
        private Panel _successPanel;

        public LoginForm()
        {
            InitializeComponent();
            InitializeForm();
            InitializeSuccessPanel();
        }

        private void InitializeForm()
        {
            // Ẩn label lỗi ban đầu
            labelError.Text = "";
            labelError.Visible = false;

            // Focus vào ô username khi form load
            this.Shown += (s, e) => txtUsername.Focus();
        }

        private void InitializeSuccessPanel()
        {
            // Tạo panel thông báo thành công (ban đầu ẩn)
            _successPanel = new Panel
            {
                Size = new Size(500, 200),
                BackColor = Color.FromArgb(240, 255, 240),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            var successIcon = new Label
            {
                Text = "✓",
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = AppColors.Primary,
                Size = new Size(40, 40),
                Location = new Point(20, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var successMessage = new Label
            {
                Text = "Đăng nhập thành công!",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = AppColors.Primary,
                AutoSize = true,
                Location = new Point(70, 25)
            };

            _countdownLabel = new Label
            {
                Text = $"Tự động chuyển sang màn hình chính sau {_countdownSeconds} giây...",
                Font = new Font("Arial", 9),
                ForeColor = AppColors.Primary,
                AutoSize = true,
                Location = new Point(70, 55)
            };

            var continueButton = new Button
            {
                Text = "Tiếp tục ngay",
                Size = new Size(100, 30),
                Location = new Point(100, 90),
                BackColor = Color.Green,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            continueButton.Click += (s, e) =>
            {
                StopTimerAndOpenMainForm();
            };

            _successPanel.Controls.Add(successIcon);
            _successPanel.Controls.Add(successMessage);
            _successPanel.Controls.Add(_countdownLabel);
            _successPanel.Controls.Add(continueButton);

            // Căn giữa panel
            _successPanel.Location = new Point(
                (this.ClientSize.Width - _successPanel.Width) / 2,
                (this.ClientSize.Height - _successPanel.Height) / 2
            );

            this.Controls.Add(_successPanel);
            _successPanel.BringToFront();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            LoadSavedCredentials();
        }

        private void LoadSavedCredentials()
        {
            try
            {
                // Kiểm tra xem có lưu thông tin đăng nhập không
                if (Properties.Settings.Default.RememberMe)
                {
                    // Load thông tin đã lưu
                    txtUsername.Text = Properties.Settings.Default.SavedUsername ?? "";
                    txtPassword.Text = Properties.Settings.Default.SavedPassword ?? "";
                    checkBoxRemember.Checked = true;

                    // Focus vào password nếu đã có username
                    if (!string.IsNullOrWhiteSpace(txtUsername.Text))
                    {
                        txtPassword.Focus();
                    }
                }
                else
                {
                    // Reset nếu không có Remember me
                    txtUsername.Text = "";
                    txtPassword.Text = "";
                    checkBoxRemember.Checked = false;
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nhưng không hiển thị cho user
                System.Diagnostics.Debug.WriteLine($"Lỗi khi load saved credentials: {ex.Message}");
                // Reset về trạng thái mặc định
                txtUsername.Text = "";
                txtPassword.Text = "";
                checkBoxRemember.Checked = false;
            }
        }

        private void SaveCredentials()
        {
            try
            {
                if (checkBoxRemember.Checked)
                {
                    // Lưu thông tin đăng nhập
                    Properties.Settings.Default.SavedUsername = txtUsername.Text.Trim();
                    Properties.Settings.Default.SavedPassword = txtPassword.Text; // Lưu cả password
                    Properties.Settings.Default.RememberMe = true;
                }
                else
                {
                    // Xóa thông tin đã lưu
                    Properties.Settings.Default.SavedUsername = "";
                    Properties.Settings.Default.SavedPassword = "";
                    Properties.Settings.Default.RememberMe = false;
                }

                // Lưu settings
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                // Log lỗi nhưng không làm ảnh hưởng đến trải nghiệm user
                System.Diagnostics.Debug.WriteLine($"Lỗi khi save credentials: {ex.Message}");
            }
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
                    AppUserContext.CurrentFullname = user.Fullname;

                    // Lưu user vào AppStateManager
                    AppServices.State.CurrentUser = user;

                    // Save credentials if Remember me is checked
                    if (checkBoxRemember.Checked)
                    {
                        SaveCredentials();
                    }

                    // Hiển thị thông báo thành công và bắt đầu đếm ngược
                    ShowSuccessAndStartCountdown(result.Message ?? "Đăng nhập thành công!");

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

        private void ShowSuccessAndStartCountdown(string message)
        {
            // Cập nhật thông báo thành công
            _countdownLabel.Text = $"Đăng nhập thành công! Tự động chuyển sau {_countdownSeconds} giây...";

            // Hiển thị panel thông báo thành công
            _successPanel.Visible = true;
            _successPanel.BringToFront();

            // Focus vào nút "Tiếp tục ngay"
            var continueButton = _successPanel.Controls.OfType<Button>().FirstOrDefault();
            if (continueButton != null)
            {
                continueButton.Focus();
            }

            // Khởi tạo và bắt đầu timer đếm ngược
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            _countdownTimer = new System.Timers.Timer(1000); // 1 giây
            _countdownTimer.Elapsed += OnTimedEvent;
            _countdownTimer.AutoReset = true;
            _countdownTimer.Enabled = true;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            // Giảm thời gian đếm ngược
            _countdownSeconds--;

            // Cập nhật UI từ thread chính
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    UpdateCountdownDisplay();
                }));
            }
            else
            {
                UpdateCountdownDisplay();
            }
        }

        private void UpdateCountdownDisplay()
        {
            if (_countdownSeconds > 0)
            {
                _countdownLabel.Text = $"Đăng nhập thành công! Tự động chuyển sau {_countdownSeconds} giây...";
            }
            else
            {
                StopTimerAndOpenMainForm();
            }
        }

        private void StopTimerAndOpenMainForm()
        {
            // Dừng timer
            if (_countdownTimer != null)
            {
                _countdownTimer.Stop();
                _countdownTimer.Dispose();
                _countdownTimer = null;
            }

            // Ẩn panel thông báo
            _successPanel.Visible = false;

            // Mở MainForm
            OpenMainForm();
        }

        private void OpenMainForm()
        {
            try
            {
                this.Hide();
                var mainForm = new MainForm();
                mainForm.FormClosed += (s, args) =>
                {
                    AppUserContext.Clear(); // Clear context khi đóng MainForm
                    this.Close();
                };
                mainForm.Show();
            }
            catch (Exception mainFormEx)
            {
                // Nếu MainForm không load được, hiển thị lại LoginForm
                this.Show();
                UIHelper.ShowExceptionError(mainFormEx, "mở màn hình chính");
                ShowError("Không thể mở màn hình chính. Vui lòng thử lại.");
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
                if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    txtPassword.Focus();
                }
                else
                {
                    btnLogin.PerformClick();

                }
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