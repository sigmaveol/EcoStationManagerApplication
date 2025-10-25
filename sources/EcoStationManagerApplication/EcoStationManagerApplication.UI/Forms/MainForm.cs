using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EcoStationManagerApplication.UI.Common;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            //(s, e) =>
            //{
            //    ShowContentForm(new DashboardForm());
            //});
            //AddMenuItem("Dashboard", "Dashboard_icon", null);
            //Helper.SetHover();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        //private void AddMenuItem(string text, String resourceName, EventHandler onClick)
        //{
        //    Image icon = null;
        //    var prop = typeof(Properties.Resources).GetProperty(resourceName.ToLower());
        //    if (prop != null)
        //    {
        //        icon = prop.GetValue(null) as Image;
        //    }

        //    Guna.UI2.WinForms.Guna2Button btn = new Guna.UI2.WinForms.Guna2Button();

        //    // Thiết lập cơ bản
        //    btn.Text = text;
        //    btn.Image = icon;
        //    btn.ImageSize = new Size(25, 25);
        //    btn.Font = new Font("Segoe UI", 14, FontStyle.Regular);
        //    btn.TextAlign = HorizontalAlignment.Left;
        //    btn.ImageAlign = HorizontalAlignment.Left;
        //    btn.Image = Properties.Resources.dashboard_icon;
        //    btn.FillColor = this.guna2PanelMenuSidebar.BackColor;  // nền trùng với sidebar
        //    btn.ImageOffset = new Point(10, 0);
        //    btn.Padding = new Padding(30, 0, 0, 0);
        //    btn.Cursor = Cursors.Hand;


        //    btn.Size = new Size(guna2PanelMenuSidebar.Width - 10, 40);
        //    btn.Location = new Point(5, guna2PanelMenuSidebar.Controls.Count * 45); // xếp tự động
        //    btn.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        //    // Hover effect
        //    btn.HoverState.FillColor = Color.FromArgb(40, 40, 40);
        //    btn.HoverState.ForeColor = Color.White;

        //    btn.Click += (s, e) => // sự kiện khi nhấp
        //    {
        //        foreach (Guna2Button b in guna2PanelMenuSidebar.Controls.OfType<Guna2Button>())
        //            b.FillColor = guna2PanelMenuSidebar.BackColor;  // reset
        //        btn.FillColor = Color.FromArgb(60, 60, 60, 60);       // active màu
        //        onClick?.Invoke(s, e);                              // gọi sự kiện chính
        //    };

        //    btn.Tag = text;

        //    guna2PanelMenuSidebar.Controls.Add(btn);
        

    }
}
