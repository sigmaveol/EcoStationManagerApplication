using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class ProductSelectionForm : Form
    {
        private List<ProductDTO> _products;
        private List<Packaging> _packagings;

        public RefType? SelectedRefType { get; private set; }
        public int? SelectedRefId { get; private set; }
        public string SelectedProductName { get; private set; }
        public string SelectedUnit { get; private set; }
        public decimal Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public DateTime? ExpiryDate { get; private set; }
        public bool IsOK { get; private set; }

        public ProductSelectionForm(List<ProductDTO> products, List<Packaging> packagings)
        {
            _products = products ?? new List<ProductDTO>();
            _packagings = packagings ?? new List<Packaging>();
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            // Ẩn labelError ban đầu
            labelError.Text = "";
            labelError.Visible = false;

            // Thêm danh sách sản phẩm và bao bì vào ComboBox
            cmbProduct.Items.Add("-- Chọn sản phẩm/bao bì --");

            // Thêm sản phẩm với prefix [SP]
            foreach (var product in _products)
            {
                cmbProduct.Items.Add($"[SP] {product.Code} - {product.Name}");
            }

            // Thêm bao bì với prefix [BB]
            foreach (var packaging in _packagings)
            {
                cmbProduct.Items.Add($"[BB] {packaging.Barcode ?? ""} - {packaging.Name}");
            }

            cmbProduct.SelectedIndex = 0;
        }

        private void CmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Có thể cập nhật đơn vị khi chọn sản phẩm nếu cần
        }

        private void ChkHasExpiryDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpExpiryDate.Enabled = chkHasExpiryDate.Checked;
            if (!chkHasExpiryDate.Checked)
            {
                dtpExpiryDate.Value = DateTime.Now;
            }
        }

        private bool ValidateForm()
        {
            labelError.Text = "";
            labelError.Visible = false;

            // Kiểm tra sản phẩm/bao bì
            if (cmbProduct.SelectedItem == null || cmbProduct.SelectedIndex == 0)
            {
                ShowError("Vui lòng chọn sản phẩm hoặc bao bì!");
                cmbProduct.Focus();
                return false;
            }

            var selectedText = cmbProduct.SelectedItem.ToString();
            if (selectedText == "-- Chọn sản phẩm/bao bì --")
            {
                ShowError("Vui lòng chọn sản phẩm hoặc bao bì!");
                cmbProduct.Focus();
                return false;
            }

            // Kiểm tra số lượng
            if (numQuantity.Value <= 0)
            {
                ShowError("Số lượng phải lớn hơn 0!");
                numQuantity.Focus();
                return false;
            }

            // Kiểm tra đơn giá
            if (numUnitPrice.Value < 0)
            {
                ShowError("Đơn giá không được âm!");
                numUnitPrice.Focus();
                return false;
            }

            // Kiểm tra hạn sử dụng nếu checkbox được chọn
            if (chkHasExpiryDate.Checked)
            {
                if (dtpExpiryDate.Value < DateTime.Now.Date)
                {
                    ShowError("Hạn sử dụng không được nhỏ hơn ngày hiện tại!");
                    dtpExpiryDate.Focus();
                    return false;
                }
            }

            return true;
        }

        private void ShowError(string message)
        {
            labelError.Text = message;
            labelError.Visible = true;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            // Validate tất cả các trường trước khi tiếp tục
            if (!ValidateForm())
            {
                this.DialogResult = DialogResult.None;
                return;
            }

            var selectedText = cmbProduct.SelectedItem.ToString();

            // Kiểm tra xem là sản phẩm [SP] hay bao bì [BB]
            if (selectedText.StartsWith("[SP]"))
            {
                // Là sản phẩm
                var productText = selectedText.Substring(5).Trim(); // Bỏ "[SP] "
                var product = _products.FirstOrDefault(p =>
                    $"{p.Code} - {p.Name}".Equals(productText, StringComparison.OrdinalIgnoreCase));
                if (product != null)
                {
                    SelectedRefType = RefType.PRODUCT;
                    SelectedRefId = product.ProductId;
                    SelectedProductName = $"[SP] {product.Code} - {product.Name}";
                    SelectedUnit = product.UnitMeasure ?? "-";
                }
                else
                {
                    ShowError("Không tìm thấy sản phẩm đã chọn!");
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }
            else if (selectedText.StartsWith("[BB]"))
            {
                // Là bao bì
                var packagingText = selectedText.Substring(5).Trim(); // Bỏ "[BB] "
                var packaging = _packagings.FirstOrDefault(p =>
                    $"{p.Barcode ?? ""} - {p.Name}".Equals(packagingText, StringComparison.OrdinalIgnoreCase));
                if (packaging != null)
                {
                    SelectedRefType = RefType.PACKAGING;
                    SelectedRefId = packaging.PackagingId;
                    SelectedProductName = $"[BB] {packaging.Barcode ?? ""} - {packaging.Name}";
                    SelectedUnit = "cái";
                }
                else
                {
                    ShowError("Không tìm thấy bao bì đã chọn!");
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }
            else
            {
                ShowError("Định dạng sản phẩm/bao bì không hợp lệ!");
                this.DialogResult = DialogResult.None;
                return;
            }

            // Gán giá trị sau khi validation thành công
            Quantity = numQuantity.Value;
            UnitPrice = numUnitPrice.Value;
            ExpiryDate = chkHasExpiryDate.Checked ? (DateTime?)dtpExpiryDate.Value.Date : null;
            IsOK = true;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            IsOK = false;
        }
    }
}