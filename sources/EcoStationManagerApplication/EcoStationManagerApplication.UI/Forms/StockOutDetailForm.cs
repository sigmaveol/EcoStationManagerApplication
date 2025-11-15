using EcoStationManagerApplication.Models.DTOs;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class StockOutDetailForm : Form
    {
        private StockOutDetail _detail;

        public StockOutDetailForm(StockOutDetail detail)
        {
            _detail = detail;
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Chi tiết phiếu xuất kho";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new Size(600, 500);

            LoadDetailData();
        }

        private void LoadDetailData()
        {
            if (_detail == null) return;

            lblReferenceNumber.Text = $"XK{_detail.StockOutId:D6}";
            lblProductName.Text = _detail.ProductName ?? _detail.PackagingName ?? "-";
            lblBatchNo.Text = _detail.BatchNo ?? "-";
            lblQuantity.Text = _detail.Quantity.ToString("N2");
            lblPurpose.Text = GetPurposeText(_detail.Purpose);
            lblOrderCode.Text = _detail.OrderId.HasValue ? $"DH{_detail.OrderId.Value:D6}" : "-";
            lblCreatedDate.Text = _detail.CreatedDate.ToString("dd/MM/yyyy HH:mm");
            lblCreatedBy.Text = _detail.CreatedBy ?? "-";
            txtNotes.Text = _detail.Notes ?? "";
        }

        private string GetPurposeText(string purpose)
        {
            switch (purpose?.ToUpper())
            {
                case "SALE":
                    return "Bán hàng";
                case "TRANSFER":
                    return "Chuyển kho";
                case "DAMAGE":
                case "WASTE":
                    return "Hao hụt";
                case "SAMPLE":
                    return "Mẫu thử nghiệm";
                default:
                    return purpose ?? "-";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

