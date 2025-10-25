using EcoStationManagerApplication.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class OrderManagementForm : Form
    {
        private readonly IOrderService _orderService;

        public OrderManagementForm(IOrderService orderService)
        {
            _orderService = orderService;
            InitializeComponent();
        }

        private void OrderManagementForm_Load(object sender, EventArgs e)
        {
            LoadOrdersAsync();
        }

        private async void LoadOrdersAsync()
        {
            try
            {
                var orders = await _orderService.GetAllAsync();
                // Bind data to grid
                //dataGridViewOrders.DataSource = orders;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải đơn hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
