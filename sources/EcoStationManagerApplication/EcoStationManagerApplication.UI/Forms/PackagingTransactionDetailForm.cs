using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.UI.Common;
using EcoStationManagerApplication.Common.Utilities;
using EcoStationManagerApplication.Common.Exporters;
using System;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class PackagingTransactionDetailForm : Form
    {
        private readonly PackagingTransaction _transaction;

        public PackagingTransactionDetailForm(PackagingTransaction transaction)
        {
            _transaction = transaction;
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Chi tiết giao dịch bao bì";
            lblTitle.Text = "Chi tiết giao dịch bao bì";

            lblReferenceNumber.Text = $"GD{_transaction.TransactionId:D6}";
            lblCustomerName.Text = _transaction.Customer?.Name ?? "-";
            lblCustomerPhone.Text = _transaction.Customer?.Phone ?? "-";
            lblPackagingName.Text = _transaction.Packaging?.Name ?? "-";
            lblBarcode.Text = _transaction.Packaging?.Barcode ?? "-";
            lblTransactionType.Text = _transaction.Type.GetDisplayName();
            lblOwnershipType.Text = _transaction.OwnershipType.GetDisplayName();
            lblQuantity.Text = _transaction.Quantity.ToString("N0");
            lblDepositPrice.Text = FormatHelper.FormatCurrency(_transaction.DepositPrice);
            lblRefundAmount.Text = FormatHelper.FormatCurrency(_transaction.RefundAmount);
            lblCreatedDate.Text = _transaction.CreatedDate.ToString("dd/MM/yyyy HH:mm");
            lblCreatedBy.Text = _transaction.User?.Fullname ?? "-";
            txtNotes.Text = _transaction.Notes ?? "";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (_transaction == null) return;

            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                saveDialog.FileName = $"ChiTietGiaoDichBaoBi_GD{_transaction.TransactionId:D6}.xlsx";
                saveDialog.Title = "Xuất chi tiết giao dịch bao bì ra Excel";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var dataTable = BuildDataTable();
                    var headers = new Dictionary<string, string>
                    {
                        { "TransactionId", "ID" },
                        { "CreatedDate", "Ngày giao dịch" },
                        { "TransactionType", "Loại giao dịch" },
                        { "OwnershipType", "Hình thức" },
                        { "PackagingName", "Tên bao bì" },
                        { "Barcode", "Barcode" },
                        { "CustomerName", "Khách hàng" },
                        { "CustomerPhone", "SĐT KH" },
                        { "Quantity", "Số lượng" },
                        { "DepositPrice", "Tiền cọc" },
                        { "RefundAmount", "Tiền hoàn" },
                        { "CreatedBy", "Người thực hiện" },
                        { "Notes", "Ghi chú" }
                    };

                    var title = $"Mã giao dịch: GD{_transaction.TransactionId:D6}\n" +
                                $"Khách hàng: {_transaction.Customer?.Name ?? "-"} ({_transaction.Customer?.Phone ?? "-"})\n" +
                                $"Bao bì: {_transaction.Packaging?.Name ?? "-"} ({_transaction.Packaging?.Barcode ?? "-"})";

                    var excelExporter = new ExcelExporter();
                    excelExporter.ExportToExcel(dataTable, saveDialog.FileName, "Chi tiết giao dịch bao bì", headers, title);

                    UIHelper.ShowSuccessMessage($"Đã xuất Excel thành công!\nFile: {saveDialog.FileName}");
                }
            }
        }

        private void btnExportExcel_MouseEnter(object sender, EventArgs e)
        {
            if (btnExportExcel.Enabled)
            {
                btnExportExcel.BackColor = System.Drawing.Color.FromArgb(33, 140, 73);
            }
        }

        private void btnExportExcel_MouseLeave(object sender, EventArgs e)
        {
            if (btnExportExcel.Enabled)
            {
                btnExportExcel.BackColor = System.Drawing.Color.FromArgb(31, 107, 59);
            }
        }

        private DataTable BuildDataTable()
        {
            var dt = new DataTable();
            dt.Columns.Add("TransactionId", typeof(int));
            dt.Columns.Add("CreatedDate", typeof(DateTime));
            dt.Columns.Add("TransactionType", typeof(string));
            dt.Columns.Add("OwnershipType", typeof(string));
            dt.Columns.Add("PackagingName", typeof(string));
            dt.Columns.Add("Barcode", typeof(string));
            dt.Columns.Add("CustomerName", typeof(string));
            dt.Columns.Add("CustomerPhone", typeof(string));
            dt.Columns.Add("Quantity", typeof(int));
            dt.Columns.Add("DepositPrice", typeof(decimal));
            dt.Columns.Add("RefundAmount", typeof(decimal));
            dt.Columns.Add("CreatedBy", typeof(string));
            dt.Columns.Add("Notes", typeof(string));

            dt.Rows.Add(
                _transaction.TransactionId,
                _transaction.CreatedDate,
                _transaction.Type.GetDisplayName(),
                _transaction.OwnershipType.GetDisplayName(),
                _transaction.Packaging?.Name ?? "-",
                _transaction.Packaging?.Barcode ?? "-",
                _transaction.Customer?.Name ?? "-",
                _transaction.Customer?.Phone ?? "-",
                _transaction.Quantity,
                _transaction.DepositPrice,
                _transaction.RefundAmount,
                _transaction.User?.Fullname ?? "-",
                _transaction.Notes ?? "-"
            );

            return dt;
        }
    }
}