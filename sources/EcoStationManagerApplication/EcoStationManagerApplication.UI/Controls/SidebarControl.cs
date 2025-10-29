using EcoStationManagerApplication.UI.Common;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class SidebarControl : UserControl
    {

        public event EventHandler<string> MenuClicked;
        public event EventHandler<string> MenuHover;
        public event EventHandler<string> MenuPressed;
        private bool isCollapsed = false;
        private int expandedWidth;
        private const int COLLAPSED_WIDTH = 60;
        private const int AUTO_COLLAPSE_THRESHOLD = 700;
        private Point togglerOriginalLocation;
        private AnchorStyles togglerOriginalAnchor;

        public SidebarControl()
        {
            InitializeComponent();
        }

        private void SidebarControl_Load(object sender, EventArgs e)
        {   
            ThemeManager.ApplySidebarTheme(this);
            expandedWidth = this.Width; // lưu width ban đầu
            togglerOriginalLocation = this.guna2ButtonMenuTogger.Location;
            togglerOriginalAnchor = this.guna2ButtonMenuTogger.Anchor;
            if (this.FindForm() != null)
            {
                this.FindForm().Resize += ParentForm_Resize;
            }
        }

        private void ParentForm_Resize(object sender, EventArgs e)
        {
            var form = sender as Form;
            if (form == null) return;

            if (form.Width < AUTO_COLLAPSE_THRESHOLD && !isCollapsed)
                ToggleSidebar(true);
            else if (form.Width >= AUTO_COLLAPSE_THRESHOLD && isCollapsed)
                ToggleSidebar(false);
        }

        /// <summary>
        /// Thêm một menu item vào panel menu sidebar
        /// </summary>
        /// <param name="text">Tên menu hiển thị</param>
        /// <param name="icon">Hình icon (Image)</param>
        /// <param name="menuName">Tên định danh menu</param
        /// 
        private void ToggleSidebar(bool collapse)
        {
            if (collapse)
            {
                this.Width = COLLAPSED_WIDTH;

                // ẩn trừ toggler
                this.guna2CirclePictureBoxLogoSidebar.Visible = false;
                labelAppName.Visible = false;
                labelUsername.Visible = false;

                // fill toggle
                this.guna2ButtonMenuTogger.Anchor = AnchorStyles.None;
                this.guna2ButtonMenuTogger.Dock = DockStyle.Fill;


                foreach (Control ctrl in guna2PanelMenuSidebar.Controls)
                {
                    if (ctrl is Guna2Button btn)
                    {
                        btn.Tag = btn.Text;
                        btn.Text = "";
                        btn.ImageAlign = HorizontalAlignment.Center;

                        // tooltip display
                        var toolTip = new ToolTip();
                        toolTip.BackColor = Color.FromArgb(0, 98, 102);
                        toolTip.ForeColor = Color.White;
                        toolTip.SetToolTip(btn, btn.Tag?.ToString());
                    }
                }

                isCollapsed = true;
            }
            else
            {
                this.Width = expandedWidth;

                this.guna2CirclePictureBoxLogoSidebar.Visible = true;
                //this.guna2ButtonMenuTogger.Dock = DockStyle.None;
                //this.guna2ButtonMenuTogger.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                labelAppName.Visible = true;
                labelUsername.Visible = true;

                // trả về vị trí và anchor ban đầu
                this.guna2ButtonMenuTogger.Dock = DockStyle.None;
                this.guna2ButtonMenuTogger.Anchor = togglerOriginalAnchor;
                this.guna2ButtonMenuTogger.Location = togglerOriginalLocation;

                foreach (Control ctrl in guna2PanelMenuSidebar.Controls)
                {
                    if (ctrl is Guna2Button btn)
                    {
                        btn.Text = btn.Tag?.ToString() ?? btn.Text;
                        btn.ImageAlign = HorizontalAlignment.Left;
                        btn.TextAlign = HorizontalAlignment.Left;
                        var toolTip = new ToolTip();
                        toolTip.SetToolTip(btn, "");
                    }
                }

                isCollapsed = false;
            }
        }


        public void AddMenuItem(string text, Image icon, string menuName)
        {
            var btn = new Guna2Button
            {
                Text = text,
                Image = icon,
                TextAlign = HorizontalAlignment.Left,
                ImageAlign = HorizontalAlignment.Left,
                Dock = DockStyle.Top,
                Height = 50,
                FillColor = Color.Transparent,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                Tag = menuName, // Lưu menuName vào Tag
                Cursor = Cursors.Hand,
            };

            // Hiệu ứng hover
            btn.MouseEnter += (s, e) =>
            {
                if (btn.FillColor != Color.FromArgb(80, 255, 255, 255)) // Không đổi màu nếu đang được chọn
                {
                    btn.FillColor = Color.FromArgb(40, 255, 255, 255);
                    MenuHover?.Invoke(this, btn.Tag.ToString());
                }
            };

            btn.MouseLeave += (s, e) =>
            {
                if (btn.FillColor != Color.FromArgb(80, 255, 255, 255)) // Không đổi màu nếu đang được chọn
                    btn.FillColor = Color.Transparent;
            };

            // Hiệu ứng click (nhấn chuột)
            btn.MouseDown += (s, e) =>
            {
                btn.FillColor = Color.FromArgb(60, 255, 255, 255);
                MenuPressed?.Invoke(this, btn.Tag.ToString());
            };

            btn.MouseUp += (s, e) =>
            {
                if (btn.FillColor == Color.FromArgb(60, 255, 255, 255))
                    btn.FillColor = Color.FromArgb(40, 255, 255, 255);
            };

            // Sự kiện click chính
            btn.Click += (s, e) =>
            {
                // Reset màu tất cả các button
                foreach (Control control in guna2PanelMenuSidebar.Controls)
                {
                    if (control is Guna2Button otherBtn)
                    {
                        otherBtn.FillColor = Color.Transparent;
                    }
                }

                // Highlight button được chọn
                btn.FillColor = Color.FromArgb(80, 255, 255, 255);

                // Gửi menuName (được lưu trong Tag) cho MainForm
                MenuClicked?.Invoke(this, btn.Tag.ToString());
            };

            // Thêm nút vào panel menu (ở trên cùng)
            guna2PanelMenuSidebar.Controls.Add(btn);
            guna2PanelMenuSidebar.Controls.SetChildIndex(btn, 0);
        }
        /// <summary>
        /// Xóa tất cả menu items
        /// </summary>
        public void ClearMenuItems()
        {
            guna2PanelMenuSidebar.Controls.Clear();
        }

        /// <summary>
        /// Đặt tên người dùng hiển thị
        /// </summary>
        public void SetUsername(string username)
        {
            labelUsername.Text = username;
        }

        /// <summary>
        /// Đặt avatar người dùng
        /// </summary>
        public void SetAvatar(Image avatarImage)
        {
            guna2CirclePictureBoxAvatar.Image = avatarImage;
        }

        /// <summary>
        /// Đặt logo ứng dụng
        /// </summary>
        public void SetLogo(Image logoImage)
        {
            guna2CirclePictureBoxLogoSidebar.Image = logoImage;
        }

        /// <summary>
        /// Đặt tên ứng dụng
        /// </summary>
        public void SetAppName(string appName)
        {
            labelAppName.Text = appName;
        }

        private void guna2ButtonMenuTogger_Click(object sender, EventArgs e)
        {
            ToggleSidebar(!isCollapsed);
        }

        // Sự kiện click avatar (nếu cần)
        private void guna2CirclePictureBoxAvatar_Click(object sender, EventArgs e)
        {
            // Có thể thêm sự kiện click avatar ở đây
            // Ví dụ: mở form thông tin user
        }

        // Sự kiện click user panel (nếu cần)
        private void guna2PanelHighLightUser_Click(object sender, EventArgs e)
        {
            // Có thể thêm sự kiện click user panel ở đây
        }
        
    }

}
