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
        #region WorkShift Management Methods

        /// <summary>
        /// Mở form thêm ca làm việc mới
        /// </summary>
        private async void btnAddWorkShift_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy danh sách nhân viên
                var staffResult = await AppServices.UserService.GetActiveStaffAsync();
                if (staffResult?.Success != true || staffResult.Data == null || !staffResult.Data.Any())
                {
                    MessageBox.Show("Không có nhân viên nào để tạo ca làm việc.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var form = new EcoStationManagerApplication.UI.Forms.WorkShiftForm(null))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        await LoadWorkShiftDataAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form thêm ca làm việc: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Mở form sửa ca làm việc
        /// </summary>
        private async void btnEditWorkShift_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvKPI.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn ca làm việc cần sửa.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var selectedRow = dgvKPI.SelectedRows[0];
                var rowData = selectedRow.DataBoundItem as WorkShiftRow;
                if (rowData == null || rowData.ShiftId <= 0)
                {
                    MessageBox.Show("Không tìm thấy thông tin ca làm việc.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Lấy thông tin ca làm việc từ database
                var shiftResult = await AppServices.WorkShiftService.GetByIdAsync(rowData.ShiftId);
                if (shiftResult?.Success != true || shiftResult.Data == null)
                {
                    MessageBox.Show("Không tìm thấy ca làm việc.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var form = new EcoStationManagerApplication.UI.Forms.WorkShiftForm(shiftResult.Data))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        await LoadWorkShiftDataAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form sửa ca làm việc: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xóa ca làm việc
        /// </summary>
        private async void btnDeleteWorkShift_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvKPI.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn ca làm việc cần xóa.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var selectedRow = dgvKPI.SelectedRows[0];
                var rowData = selectedRow.DataBoundItem as WorkShiftRow;
                if (rowData == null || rowData.ShiftId <= 0)
                {
                    MessageBox.Show("Không tìm thấy thông tin ca làm việc.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var confirmResult = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa ca làm việc của {rowData.StaffName} vào ngày {rowData.ShiftDate}?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    var deleteResult = await AppServices.WorkShiftService.DeleteAsync(rowData.ShiftId);
                    if (deleteResult?.Success == true)
                    {
                        MessageBox.Show("Đã xóa ca làm việc thành công.", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadWorkShiftDataAsync();
                    }
                    else
                    {
                        MessageBox.Show($"Lỗi khi xóa ca làm việc: {deleteResult?.Message ?? "Không xác định"}",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa ca làm việc: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xuất danh sách ca làm việc ra Excel
        /// </summary>
        private async void btnExportWorkShiftExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (_workShiftSource == null || _workShiftSource.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "Excel Files|*.xlsx";
                    saveDialog.FileName = $"DanhSachCaLam_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    saveDialog.Title = "Xuất danh sách ca làm việc ra Excel";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        var exporter = new EcoStationManagerApplication.Common.Exporters.ExcelExporter();
                        var headers = new Dictionary<string, string>
                        {
                            { "StaffName", "Tên nhân viên" },
                            { "Role", "Vai trò" },
                            { "StationName", "Trạm làm việc" },
                            { "ShiftDate", "Ngày ca" },
                            { "StartTime", "Giờ bắt đầu" },
                            { "EndTime", "Giờ kết thúc" },
                            { "KpiScore", "KPI (%)" },
                            { "Notes", "Ghi chú" }
                        };

                        exporter.ExportToExcel(_workShiftSource.ToList(), saveDialog.FileName, "Ca làm việc", headers);
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
        /// Xuất danh sách ca làm việc ra PDF
        /// </summary>
        private async void btnExportWorkShiftPdf_Click(object sender, EventArgs e)
        {
            try
            {
                if (_workShiftSource == null || _workShiftSource.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "PDF Files|*.pdf";
                    saveDialog.FileName = $"DanhSachCaLam_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                    saveDialog.Title = "Xuất danh sách ca làm việc ra PDF";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        var exporter = new EcoStationManagerApplication.Common.Exporters.PdfExporter();
                        var headers = new Dictionary<string, string>
                        {
                            { "StaffName", "Tên nhân viên" },
                            { "Role", "Vai trò" },
                            { "StationName", "Trạm làm việc" },
                            { "ShiftDate", "Ngày ca" },
                            { "StartTime", "Giờ bắt đầu" },
                            { "EndTime", "Giờ kết thúc" },
                            { "KpiScore", "KPI (%)" },
                            { "Notes", "Ghi chú" }
                        };

                        exporter.ExportToPdf(_workShiftSource.ToList(), saveDialog.FileName, "DANH SÁCH CA LÀM VIỆC", headers);
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
        /// Lọc danh sách ca làm việc
        /// </summary>
        private async void ApplyWorkShiftFilters()
        {
            try
            {
                await LoadWorkShiftDataAsync();
                FilterWorkShiftData();
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

