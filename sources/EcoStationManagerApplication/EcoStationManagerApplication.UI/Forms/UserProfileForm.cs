using System;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.UI.Common;
using AppServices = EcoStationManagerApplication.UI.Common.AppServices;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class UserProfileForm : Form
    {
        public UserProfileForm()
        {
            InitializeComponent();
            LoadUserInfo();
        }

        private void LoadUserInfo()
        {
            var user = AppServices.State.CurrentUser;
            if (user == null)
            {
                txtUsername.Text = "";
                txtFullname.Text = "";
                txtRole.Text = "";
                txtRoleDesc.Text = "";
                txtStatus.Text = "";
                txtCreated.Text = "";
                return;
            }

            txtUsername.Text = user.Username;
            txtFullname.Text = user.Fullname;
            txtRole.Text = RolePermissionHelper.GetRoleDisplayName(user.Role);
            txtRoleDesc.Text = RolePermissionHelper.GetRoleDescription(user.Role);
            txtStatus.Text = user.IsActive == ActiveStatus.ACTIVE ? "Đang hoạt động" : "Không hoạt động";
            txtCreated.Text = user.CreatedDate.ToString("dd/MM/yyyy HH:mm");

            var nameForAvatar = string.IsNullOrWhiteSpace(user.Fullname) ? user.Username : user.Fullname;
            if (!string.IsNullOrWhiteSpace(nameForAvatar))
            {
                pictureAvatar.Image = UIHelper.CreateInitialAvatar(nameForAvatar, pictureAvatar.Size);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}