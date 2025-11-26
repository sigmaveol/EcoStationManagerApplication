using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AppServices = EcoStationManagerApplication.UI.Common.AppServices;
using EcoStationManagerApplication.UI.Common;
using EcoStationManagerApplication.UI.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class HeaderControl : UserControl
    {
        public HeaderControl()
        {
            InitializeComponent();
            LoadUserInfo();
            AppServices.State.OnUserChanged += State_OnUserChanged;
        }

        private void State_OnUserChanged(object sender, EventArgs e)
        {
            LoadUserInfo();
        }

        private void LoadUserInfo()
        {
            var user = AppServices.State.CurrentUser;
            if (user == null)
            {
                lblUserFullname.Text = "";
                lblUserRole.Text = "";
                return;
            }

            lblUserFullname.Text = string.IsNullOrWhiteSpace(user.Fullname) ? user.Username : user.Fullname;
            lblUserRole.Text = RolePermissionHelper.GetRoleDisplayName(user.Role);
        }

        private void userInfoPanel_Click(object sender, EventArgs e)
        {
            var form = new UserProfileForm();
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog(this);
        }

        private void userInfoPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
