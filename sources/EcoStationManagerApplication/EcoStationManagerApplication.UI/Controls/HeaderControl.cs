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
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class HeaderControl : UserControl
    {
        private ContextMenuStrip _notificationMenu;
        private List<Notification> _recentNotifications;
        private ContextMenuStrip _userMenu;

        public HeaderControl()
        {
            InitializeComponent();
            LoadUserInfo();
            AppServices.State.OnUserChanged += State_OnUserChanged;
            _notificationMenu = new ContextMenuStrip();
            _userMenu = new ContextMenuStrip();
        }

        private async void HeaderControl_Load(object sender, EventArgs e)
        {
            await RefreshNotificationsAsync();
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
                pictureBoxAvatar.Image = null;
                return;
            }

            lblUserFullname.Text = string.IsNullOrWhiteSpace(user.Fullname) ? user.Username : user.Fullname;
            lblUserRole.Text = RolePermissionHelper.GetRoleDisplayName(user.Role);

            var nameForAvatar = string.IsNullOrWhiteSpace(user.Fullname) ? user.Username : user.Fullname;
            if (!string.IsNullOrWhiteSpace(nameForAvatar))
            {
                pictureBoxAvatar.Image = UIHelper.CreateInitialAvatar(nameForAvatar, pictureBoxAvatar.Size);
            }
        }

        private void userInfoPanel_Click(object sender, EventArgs e)
        {
            _userMenu.Items.Clear();
            var profileItem = new ToolStripMenuItem("Thông tin người dùng");
            profileItem.Click += (s, args) =>
            {
                var form = new UserProfileForm();
                form.StartPosition = FormStartPosition.CenterParent;
                var parentForm = this.FindForm();
                if (parentForm != null)
                {
                    FormHelper.ShowModalWithDim(parentForm, form);
                }
                else
                {
                    form.ShowDialog(this);
                }
            };

            var logoutItem = new ToolStripMenuItem("Đăng xuất");
            logoutItem.Click += (s, args) =>
            {
                if (UIHelper.ShowConfirmDialog("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận"))
                {
                    AppServices.State.Reset();
                    AppServices.State.CurrentUser = null;
                    AppServices.Navigation.ClearCache();
                    AppServices.State.SetState("LogoutRequested", true);

                    var parentForm = this.FindForm();
                    parentForm?.Close();
                }
            };

            _userMenu.Items.Add(profileItem);
            _userMenu.Items.Add(new ToolStripSeparator());
            _userMenu.Items.Add(logoutItem);

            var p = userInfoPanel.PointToScreen(new Point(0, userInfoPanel.Height));
            _userMenu.Show(p);
        }

        private void userInfoPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        

        private async Task RefreshNotificationsAsync()
        {
            await AppServices.NotificationService.GenerateAutoNotificationsAsync();
            var userId = AppServices.State.CurrentUser?.UserId;
            var countResult = await AppServices.NotificationService.GetUnreadCountAsync(userId);
            if (countResult.Success && countResult.Data > 0)
            {
                lblNotificationBadge.Text = countResult.Data.ToString();
                lblNotificationBadge.Visible = true;
            }
            else
            {
                lblNotificationBadge.Visible = false;
            }

            var listResult = await AppServices.NotificationService.GetRecentNotificationsAsync(userId);
            _recentNotifications = listResult.Success && listResult.Data != null ? listResult.Data : new List<Notification>();

            var lowStockResult = await AppServices.InventoryService.GetLowStockItemsAsync();
            if (lowStockResult.Success && lowStockResult.Data != null)
            {
                var hasLowStock = lowStockResult.Data.Any();
                var shown = AppServices.State.GetState<bool>("LowStockPopupShown", false);
                if (hasLowStock && !shown)
                {
                    var parent = this.FindForm();
                    if (parent != null)
                    {
                        FormHelper.ShowModalWithDim(parent, new NotificationForm());
                    }
                    else
                    {
                        NotificationForm.ShowLowStockAlerts();
                    }
                    AppServices.State.SetState("LowStockPopupShown", true);
                }
            }
        }

        private async void btnNotificationBell_Click(object sender, EventArgs e)
        {
            await RefreshNotificationsAsync();
            _notificationMenu.Items.Clear();

            var dbHeader = new ToolStripMenuItem("Thông báo") { Enabled = false };
            _notificationMenu.Items.Add(dbHeader);

            if (_recentNotifications == null || _recentNotifications.Count == 0)
            {
                _notificationMenu.Items.Add(new ToolStripMenuItem("Không có thông báo"));
            }
            else
            {
                foreach (var n in _recentNotifications)
                {
                    var main = string.IsNullOrWhiteSpace(n.Title) ? n.Message : ($"{n.Title}: {n.Message}");
                    var time = n.CreatedAt != default(DateTime) ? n.CreatedAt.ToString("dd/MM HH:mm") : string.Empty;
                    var text = string.IsNullOrEmpty(time) ? main : ($"{main} · {time}");
                    var item = new ToolStripMenuItem(text);
                    if (!n.IsRead)
                    {
                        item.Font = new Font(item.Font, FontStyle.Bold);
                    }
                    item.Tag = n;
                    item.Click += NotificationItem_Click;
                    _notificationMenu.Items.Add(item);
                }
                _notificationMenu.Items.Add(new ToolStripSeparator());
                var markAll = new ToolStripMenuItem("Đánh dấu tất cả đã đọc");
                markAll.Click += MarkAllAsRead_Click;
                _notificationMenu.Items.Add(markAll);
            }

            var p = btnNotificationBell.PointToScreen(new Point(0, btnNotificationBell.Height));
            _notificationMenu.Show(p);
        }

        private async void NotificationItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item && item.Tag is Notification n)
            {
                if (!n.IsRead)
                {
                    var res = await AppServices.NotificationService.MarkAsReadAsync(n.NotificationId);
                    if (res.Success)
                    {
                        n.IsRead = true;
                        await RefreshNotificationsAsync();
                    }
                }

                if (n.Type == NotificationType.LOWSTOCK)
                {
                    var parent = this.FindForm();
                    if (parent != null)
                    {
                        FormHelper.ShowModalWithDim(parent, new NotificationForm());
                    }
                    else
                    {
                        NotificationForm.ShowLowStockAlerts();
                    }
                }
            }
        }

        private async void MarkAllAsRead_Click(object sender, EventArgs e)
        {
            var userId = AppServices.State.CurrentUser?.UserId;
            var res = await AppServices.NotificationService.MarkAllAsReadAsync(userId);
            if (res.Success)
            {
                await RefreshNotificationsAsync();
            }
        }
    }
}
