using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class StaffControl
    {
        #region Filter and Search Methods

        private string _workShiftSearchText = "";
        private DateTime? _workShiftFilterDate = null;
        private UserRole? _workShiftFilterRole = null;
        private string _deliverySearchText = "";
        private DeliveryStatus? _deliveryFilterStatus = null;
        private int? _deliveryFilterDriverId = null;
        private DateTime? _deliveryFilterDate = null;

        /// <summary>
        /// Tìm kiếm và lọc danh sách ca làm việc
        /// </summary>
        private void FilterWorkShiftData()
        {
            try
            {
                if (_workShiftSource == null) return;

                var filtered = _workShiftSource.ToList().AsEnumerable();

                // Lọc theo tên nhân viên
                if (!string.IsNullOrWhiteSpace(_workShiftSearchText))
                {
                    filtered = filtered.Where(w => 
                        w.StaffName.IndexOf(_workShiftSearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        w.Role.IndexOf(_workShiftSearchText, StringComparison.OrdinalIgnoreCase) >= 0);
                }

                // Lọc theo ngày
                if (_workShiftFilterDate.HasValue)
                {
                    filtered = filtered.Where(w => w.ShiftDate.Contains(_workShiftFilterDate.Value.ToString("dd/MM/yyyy")));
                }

                // Lọc theo role
                if (_workShiftFilterRole.HasValue)
                {
                    string roleDisplay = GetRoleDisplayName(_workShiftFilterRole.Value);
                    filtered = filtered.Where(w => w.Role == roleDisplay);
                }

                // Cập nhật DataGridView
                var filteredList = new BindingList<WorkShiftRow>(filtered.ToList());
                dgvKPI.DataSource = filteredList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lọc dữ liệu ca làm việc: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Tìm kiếm và lọc danh sách phân công giao hàng
        /// </summary>
        private void FilterDeliveryData()
        {
            try
            {
                if (_deliverySource == null) return;

                var filtered = _deliverySource.ToList().AsEnumerable();

                // Lọc theo mã đơn, tên khách, tài xế
                if (!string.IsNullOrWhiteSpace(_deliverySearchText))
                {
                    filtered = filtered.Where(d =>
                        d.OrderCode.IndexOf(_deliverySearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        d.CustomerName.IndexOf(_deliverySearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        d.DriverName.IndexOf(_deliverySearchText, StringComparison.OrdinalIgnoreCase) >= 0);
                }

                // Lọc theo trạng thái
                if (_deliveryFilterStatus.HasValue)
                {
                    string statusDisplay = GetDeliveryStatusDisplay(_deliveryFilterStatus.Value);
                    filtered = filtered.Where(d => d.Status == statusDisplay);
                }

                // Lọc theo tài xế
                if (_deliveryFilterDriverId.HasValue)
                {
                    // Cần lấy tên tài xế từ driverId
                    // Tạm thời bỏ qua, có thể implement sau
                }

                // Lọc theo ngày
                if (_deliveryFilterDate.HasValue)
                {
                    filtered = filtered.Where(d => d.AssignedDate.Contains(_deliveryFilterDate.Value.ToString("dd/MM/yyyy")));
                }

                // Cập nhật DataGridView
                var filteredList = new BindingList<DeliveryAssignmentRow>(filtered.ToList());
                dgvAssignments.DataSource = filteredList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lọc dữ liệu phân công giao hàng: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Sắp xếp DataGridView theo cột
        /// </summary>
        private void SortDataGridView(DataGridView dgv, string columnName, ListSortDirection direction)
        {
            try
            {
                if (dgv == null || dgv.DataSource == null) return;

                var bindingSource = dgv.DataSource as BindingSource;
                if (bindingSource == null)
                {
                    // Nếu DataSource là BindingList, chuyển sang BindingSource
                    bindingSource = new BindingSource(dgv.DataSource, null);
                    dgv.DataSource = bindingSource;
                }

                bindingSource.Sort = $"{columnName} {(direction == ListSortDirection.Ascending ? "ASC" : "DESC")}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi sắp xếp: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý sự kiện click header để sắp xếp
        /// </summary>
        private void dgvKPI_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.ColumnIndex < 0) return;

                var column = dgvKPI.Columns[e.ColumnIndex];
                if (column == null || string.IsNullOrEmpty(column.Name)) return;

                // Toggle sort direction
                var currentSort = dgvKPI.Tag as string;
                bool isAscending = currentSort != $"{column.Name}_ASC";

                SortDataGridView(dgvKPI, column.Name, isAscending ? ListSortDirection.Ascending : ListSortDirection.Descending);
                dgvKPI.Tag = $"{column.Name}_{(isAscending ? "ASC" : "DESC")}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi sắp xếp: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý sự kiện click header để sắp xếp
        /// </summary>
        private void dgvAssignments_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.ColumnIndex < 0) return;

                var column = dgvAssignments.Columns[e.ColumnIndex];
                if (column == null || string.IsNullOrEmpty(column.Name)) return;

                // Toggle sort direction
                var currentSort = dgvAssignments.Tag as string;
                bool isAscending = currentSort != $"{column.Name}_ASC";

                SortDataGridView(dgvAssignments, column.Name, isAscending ? ListSortDirection.Ascending : ListSortDirection.Descending);
                dgvAssignments.Tag = $"{column.Name}_{(isAscending ? "ASC" : "DESC")}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi sắp xếp: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}

