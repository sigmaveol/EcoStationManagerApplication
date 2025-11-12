using System;
using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms // Đảm bảo đúng namespace
{
    public partial class StockMovementForm : Form
    {
        public enum MovementType { StockIn, StockOut }
        private string _productSku;

        // Khai báo control
        private Label lblTitle;
        private Label lblProductInfo;
        private GroupBox grpAction;
        private RadioButton radStockIn;
        private RadioButton radStockOut;
        private Label lblQuantity;
        private NumericUpDown numQuantity;
        private Label lblNote;
        private TextBox txtNote;
        private Button btnSave;
        private Button btnCancel;


        public StockMovementForm(string sku, MovementType type)
        {
            InitializeComponent();
            _productSku = sku;
            this.Text = "Nhập/Xuất Kho";

            // TODO: Lấy tên sản phẩm từ SKU
            lblProductInfo.Text = $"Sản phẩm: {sku}"; // (Nên thay bằng tên thật)

            if (type == MovementType.StockIn)
            {
                radStockIn.Checked = true;
            }
            else
            {
                radStockOut.Checked = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string action = radStockIn.Checked ? "Nhập kho" : "Xuất kho";
            int quantity = (int)numQuantity.Value;

            // TODO: Thêm logic lưu lịch sử nhập/xuất và cập nhật tồn kho
            MessageBox.Show($"{action} {quantity} sản phẩm [{_productSku}] thành công.", "Thành công");
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