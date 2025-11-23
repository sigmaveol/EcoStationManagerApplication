using EcoStationManagerApplication.Common.Exporters;
using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.UI.Common;
using System;
using System.Collections.Generic;
using System.Data;
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

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (_detail == null)
                {
                    UIHelper.ShowWarningMessage("Không có dữ liệu để xuất Excel!");
                    return;
                }

                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                    saveDialog.FileName = $"ChiTietXuatKho_XK{_detail.StockOutId:D6}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    saveDialog.Title = "Xuất chi tiết phiếu xuất kho ra Excel";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Tạo DataTable từ single detail
                        var dataTable = CreateDataTableForExport();

                        // Tạo headers cho Excel
                        var headers = new Dictionary<string, string>
                        {
                            { "Field", "Thông tin" },
                            { "Value", "Giá trị" }
                        };

                        // Tạo title với thông tin phiếu
                        var purposeText = GetPurposeText(_detail.Purpose);
                        var title = $"CHI TIẾT PHIẾU XUẤT KHO\n" +
                                   $"Mã phiếu: XK{_detail.StockOutId:D6}\n" +
                                   $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm}";

                        // Xuất Excel
                        var excelExporter = new ExcelExporter();
                        excelExporter.ExportToExcel(dataTable, saveDialog.FileName, "Chi tiết xuất kho", headers);

                        UIHelper.ShowSuccessMessage($"Đã xuất Excel thành công!\nFile: {saveDialog.FileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "xuất Excel");
            }
        }

        private DataTable CreateDataTableForExport()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Field", typeof(string));
            dataTable.Columns.Add("Value", typeof(string));

            dataTable.Rows.Add("Mã phiếu", $"XK{_detail.StockOutId:D6}");
            dataTable.Rows.Add("Sản phẩm/Bao bì", _detail.ProductName ?? _detail.PackagingName ?? "-");
            dataTable.Rows.Add("Mã lô", _detail.BatchNo ?? "Không có");
            dataTable.Rows.Add("Số lượng", _detail.Quantity.ToString("N2"));
            dataTable.Rows.Add("Mục đích", GetPurposeText(_detail.Purpose));
            dataTable.Rows.Add("Đơn hàng", _detail.OrderId.HasValue ? $"DH{_detail.OrderId.Value:D6}" : "Không có");
            dataTable.Rows.Add("Ngày xuất", _detail.CreatedDate.ToString("dd/MM/yyyy HH:mm"));
            dataTable.Rows.Add("Người xuất", _detail.CreatedBy ?? "-");
            dataTable.Rows.Add("Ghi chú", _detail.Notes ?? "-");

            return dataTable;
        }
    }
}

