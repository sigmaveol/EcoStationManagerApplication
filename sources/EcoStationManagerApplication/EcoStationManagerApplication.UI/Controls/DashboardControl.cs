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
    public partial class DashboardControl : UserControl
    {
        public DashboardControl()
        {
            InitializeComponent();
            LoadRecentActivities();
            LoadTodoList();
            //ThemeManager.ApplyTheme(this);
        }

        private void LoadRecentActivities()
        {
            string[] activities =
            {
            "Đơn hàng ORD00005 đã được tạo (5 phút trước)",
            "Nhập kho IN-2024-004 hoàn tất (15 phút trước)",
            "Cảnh báo: Nước giặt hữu cơ tồn kho thấp (1 giờ trước)"
            };

            foreach (var act in activities)
            {
                var lbl = new Label
                {
                    Text = "• " + act,
                    AutoSize = true,
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.FromArgb(70, 70, 70),
                    Padding = new Padding(0, 3, 0, 3)
                };
                flowLayoutRecent.Controls.Add(lbl);
            }
        }

        private void LoadTodoList()
        {
            string[] tasks =
            {
                "Kiểm tra tồn kho sản phẩm A",
                "Liên hệ nhà cung cấp nước giặt",
                "Tạo báo cáo tuần này"
            };

            foreach (var t in tasks)
            {
                var chk = new Guna2CheckBox
                {
                    Text = t,
                    Font = new Font("Segoe UI", 10),
                    AutoSize = true,
                    Padding = new Padding(5),
                    CheckedState =
                    {
                        FillColor = Color.MediumSeaGreen,
                        BorderColor = Color.MediumSeaGreen
                    }
                };
                flowLayoutTodo.Controls.Add(chk);
            }
        }



    }



}
