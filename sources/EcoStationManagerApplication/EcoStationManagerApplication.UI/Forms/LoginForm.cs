using System;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class LoginForm : Form
    {
    //    private readonly IUserService _userService;

    //    public LoginForm(IUserService userService)
    //    {
    //        _userService = userService;
    //        InitializeComponent();
    //    }

    //    private void LoginForm_Load(object sender, EventArgs e)
    //    {
    //        // Set focus to username textbox
    //        txtUsername.Focus();
    //    }

    //    private async void btnLogin_Click(object sender, EventArgs e)
    //    {
    //        string username = txtUsername.Text.Trim();
    //        string password = txtPassword.Text;

    //        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
    //        {
    //            MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu", "Thông báo",
    //                MessageBoxButtons.OK, MessageBoxIcon.Warning);
    //            return;
    //        }

    //        try
    //        {
    //            btnLogin.Enabled = false;
    //            btnLogin.Text = "ĐANG ĐĂNG NHẬP...";

    //            var user = await _userService.AuthenticateUserAsync(username, password);
    //            if (user)
    //            {
    //                // Lưu thông tin user đăng nhập (có thể dùng static class hoặc dependency injection)
    //                //UserSession.CurrentUser = user;

    //                this.DialogResult = DialogResult.OK;
    //                this.Close();
    //            }
    //            else
    //            {
    //                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng", "Lỗi đăng nhập",
    //                    MessageBoxButtons.OK, MessageBoxIcon.Error);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            MessageBox.Show($"Lỗi đăng nhập: {ex.Message}", "Lỗi",
    //                MessageBoxButtons.OK, MessageBoxIcon.Error);
    //        }
    //        finally
    //        {
    //            btnLogin.Enabled = true;
    //            btnLogin.Text = "ĐĂNG NHẬP";
    //        }
    //    }

    //    private void btnExit_Click(object sender, EventArgs e)
    //    {
    //        Application.Exit();
    //    }

    //    private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
    //    {
    //        if (e.KeyChar == (char)Keys.Enter)
    //        {
    //            btnLogin.PerformClick();
    //        }
    //    }
    }
}