using System;
using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms // Đảm bảo đúng namespace
{
    // Form này dùng chung layout với AddProductForm
    // nhưng thay đổi logic constructor và nút Save
    public partial class EditProductForm : Form
    {
        private string _productSku;

        // Khai báo control (phải khớp với file .Designer)
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

        public EditProductForm(string sku)
        {
            InitializeComponent();
            _productSku = sku;
            this.Text = "Sửa Sản Phẩm";
            lblTitle.Text = "Sửa Sản Phẩm";
            btnSave.Text = "Cập nhật";

            LoadProductData();
        }

        private void LoadProductData()
        {
            // TODO: Lấy dữ liệu từ database dựa trên _productSku
            // Dữ liệu mẫu:
            if (_productSku == "DG-001")
            {
                txtName.Text = "Dầu gội thiên nhiên 500ml";
                txtSKU.Text = "DG-001";
                txtSKU.ReadOnly = true; // Không cho sửa SKU
                numPrice.Value = 120000;
                numStock.Value = 15;
                numAlertLevel.Value = 10;
            }
            else if (_productSku == "ST-002")
            {
                txtName.Text = "Sữa tắm thảo dược 500ml";
                txtSKU.Text = "ST-002";
                txtSKU.ReadOnly = true;
                numPrice.Value = 150000;
                numStock.Value = 5;
                numAlertLevel.Value = 10;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // TODO: Thêm logic CẬP NHẬT sản phẩm
            MessageBox.Show("Đã cập nhật sản phẩm thành công!", "Thành công");
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