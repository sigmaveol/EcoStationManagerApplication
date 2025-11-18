using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class StaffControl
    {
        #region Delivery Assignment Management Methods

        /// <summary>
        /// Phân công đơn hàng cho tài xế
        /// </summary>
        private async void btnAssignDelivery_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy danh sách đơn hàng chưa phân công
                var ordersResult = await AppServices.OrderService.GetProcessingOrdersAsync();
                if (ordersResult?.Success != true || ordersResult.Data == null || !ordersResult.Data.Any())
                {
                    MessageBox.Show("Không có đơn hàng nào cần phân công.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Lấy danh sách tài xế
                var driversResult = await AppServices.UserService.GetActiveDriversAsync();
                if (driversResult?.Success != true || driversResult.Data == null || !driversResult.Data.Any())
                {
                    MessageBox.Show("Không có tài xế nào để phân công.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var form = new EcoStationManagerApplication.UI.Forms.DeliveryAssignmentForm(
                    ordersResult.Data.ToList(), 
                    driversResult.Data.ToList()))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        await LoadDeliveryAssignmentsAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form phân công: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái giao hàng
        /// </summary>
        private async void btnUpdateDeliveryStatus_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAssignments.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn đơn hàng cần cập nhật trạng thái.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var selectedRow = dgvAssignments.SelectedRows[0];
                var rowData = selectedRow.DataBoundItem as DeliveryAssignmentRow;
                if (rowData == null || rowData.AssignmentId <= 0)
                {
                    MessageBox.Show("Không tìm thấy thông tin phân công giao hàng.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Lấy thông tin phân công từ database
                var assignmentResult = await AppServices.DeliveryService.GetByIdAsync(rowData.AssignmentId);
                if (assignmentResult?.Success != true || assignmentResult.Data == null)
                {
                    MessageBox.Show("Không tìm thấy phân công giao hàng.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // TODO: Mở form dialog để cập nhật trạng thái
                // using (var form = new UpdateDeliveryStatusForm(assignmentResult.Data))
                // {
                //     if (form.ShowDialog() == DialogResult.OK)
                //     {
                //         await LoadDeliveryAssignmentsAsync();
                //     }
                // }

                MessageBox.Show("Chức năng cập nhật trạng thái đang được phát triển.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật trạng thái: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xem chi tiết đơn giao hàng
        /// </summary>
        private async void dgvAssignments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;

                var row = dgvAssignments.Rows[e.RowIndex];
                var rowData = row.DataBoundItem as DeliveryAssignmentRow;
                if (rowData == null) return;

                // Lấy thông tin đơn hàng
                var orderResult = await AppServices.OrderService.GetOrderWithDetailsAsync(rowData.OrderId);
                if (orderResult?.Success != true || orderResult.Data == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin đơn hàng.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // TODO: Mở form chi tiết đơn giao hàng
                // using (var form = new DeliveryDetailForm(orderResult.Data, rowData.AssignmentId))
                // {
                //     form.ShowDialog();
                // }

                MessageBox.Show($"Chi tiết đơn hàng: {rowData.OrderCode}\nKhách hàng: {rowData.CustomerName}\nĐịa chỉ: {rowData.Address}",
                    "Chi tiết đơn giao hàng", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xem chi tiết: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xuất danh sách phân công giao hàng ra Excel
        /// </summary>
        private async void btnExportDeliveryExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (_deliverySource == null || _deliverySource.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "Excel Files|*.xlsx";
                    saveDialog.FileName = $"PhanCongGiaoHang_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    saveDialog.Title = "Xuất danh sách phân công giao hàng ra Excel";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        var exporter = new EcoStationManagerApplication.Common.Exporters.ExcelExporter();
                        var headers = new Dictionary<string, string>
                        {
                            { "OrderCode", "Mã đơn" },
                            { "CustomerName", "Tên khách" },
                            { "Address", "Địa chỉ giao" },
                            { "DriverName", "Tài xế" },
                            { "Status", "Trạng thái" },
                            { "CodAmount", "COD" },
                            { "PaymentStatus", "Thanh toán" },
                            { "AssignedDate", "Ngày phân công" }
                        };

                        exporter.ExportToExcel(_deliverySource.ToList(), saveDialog.FileName, "Phân công giao hàng", headers);
                        MessageBox.Show($"Đã xuất Excel thành công!\nFile: {saveDialog.FileName}",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất Excel: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xuất danh sách phân công giao hàng ra PDF
        /// </summary>
        private async void btnExportDeliveryPdf_Click(object sender, EventArgs e)
        {
            try
            {
                if (_deliverySource == null || _deliverySource.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "PDF Files|*.pdf";
                    saveDialog.FileName = $"PhanCongGiaoHang_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                    saveDialog.Title = "Xuất danh sách phân công giao hàng ra PDF";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        var exporter = new EcoStationManagerApplication.Common.Exporters.PdfExporter();
                        var headers = new Dictionary<string, string>
                        {
                            { "OrderCode", "Mã đơn" },
                            { "CustomerName", "Tên khách" },
                            { "Address", "Địa chỉ giao" },
                            { "DriverName", "Tài xế" },
                            { "Status", "Trạng thái" },
                            { "CodAmount", "COD" },
                            { "PaymentStatus", "Thanh toán" },
                            { "AssignedDate", "Ngày phân công" }
                        };

                        exporter.ExportToPdf(_deliverySource.ToList(), saveDialog.FileName, "DANH SÁCH PHÂN CÔNG GIAO HÀNG", headers);
                        MessageBox.Show($"Đã xuất PDF thành công!\nFile: {saveDialog.FileName}",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất PDF: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Lọc danh sách phân công giao hàng
        /// </summary>
        private async void ApplyDeliveryFilters()
        {
            try
            {
                await LoadDeliveryAssignmentsAsync();
                FilterDeliveryData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lọc dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}

