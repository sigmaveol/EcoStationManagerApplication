using System;
using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    // Đổi kế thừa từ UserControl sang Form
    public partial class AddOrderForm : Form
    {
        // Khai báo control
        private Label lblTitle;
        private Label lblCustomer;
        private TextBox txtCustomer;
        private Label lblProduct;
        private ComboBox cmbProduct;
        private Label lblQuantity;
        private NumericUpDown numQuantity;
        private Label lblVolume;
        private TextBox txtVolume;
        private Button btnAdd;
        private Button btnCancel;

        public AddOrderForm()
        {
            InitializeComponent(); // Hàm này sẽ được tạo trong file .Designer.cs

            // Tùy chỉnh thêm cho Form
            this.Text = "Thêm Đơn Hàng Mới";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCustomer.Text) || cmbProduct.SelectedIndex <= 0)
            {
                MessageBox.Show("Vui lòng nhập Tên khách hàng và chọn Sản phẩm.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // TODO: Thêm logic lưu đơn hàng vào database

            MessageBox.Show("Thêm đơn hàng thành công!", "Thành công");
            this.DialogResult = DialogResult.OK; // Báo hiệu thành công
            this.Close(); // Đóng form
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close(); // Đóng form
        }
    }
}