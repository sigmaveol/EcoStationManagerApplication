using System;
using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class AddProductForm : Form
    {
        // Khai báo control
        private Label lblTitle;
        private Label lblName;
        private TextBox txtName;
        private Label lblSKU;
        private TextBox txtSKU;
        private Label lblPrice;
        private NumericUpDown numPrice;
        private Label lblStock;
        private NumericUpDown numStock;
        private Label lblAlertLevel;
        private NumericUpDown numAlertLevel;
        private Button btnSave;
        private Button btnCancel;

        public AddProductForm()
        {
            InitializeComponent();
            this.Text = "Thêm Sản Phẩm Mới";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtSKU.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên và Mã SKU.", "Thiếu thông tin");
                return;
            }
            // TODO: Thêm logic lưu sản phẩm
            MessageBox.Show("Đã thêm sản phẩm thành công!", "Thành công");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}