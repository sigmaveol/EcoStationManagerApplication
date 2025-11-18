using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.UI.Common;
using EcoStationManagerApplication.UI.Helpers;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class WorkShiftForm : Form
    {
        private WorkShift _workShift;
        private bool _isEditMode;
        private List<User> _staffList;

        public WorkShiftForm(WorkShift workShift = null)
        {
            _workShift = workShift;
            _isEditMode = workShift != null;
            InitializeComponent();
            InitializeForm();
        }

        private async void InitializeForm()
        {
            this.Text = _isEditMode ? "Sửa ca làm việc" : "Thêm ca làm việc mới";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new System.Drawing.Size(500, 450);

            await LoadStaffList();
            LoadFormData();
        }

        private async Task LoadStaffList()
        {
            try
            {
                var staffResult = await AppServices.UserService.GetActiveStaffAsync();
                _staffList = staffResult?.Data?.ToList() ?? new List<User>();

                cmbStaff.Items.Clear();
                foreach (var staff in _staffList)
                {
                    cmbStaff.Items.Add($"{staff.Fullname ?? staff.Username} ({RolePermissionHelper.GetRoleDisplayName(staff.Role)})");
                }

                if (cmbStaff.Items.Count > 0)
                    cmbStaff.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách nhân viên: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadFormData()
        {
            if (_isEditMode && _workShift != null)
            {
                // Tìm staff trong combobox
                var staff = _staffList?.FirstOrDefault(s => s.UserId == _workShift.UserId);
                if (staff != null)
                {
                    int index = _staffList.IndexOf(staff);
                    if (index >= 0 && index < cmbStaff.Items.Count)
                        cmbStaff.SelectedIndex = index;
                }

                dtpShiftDate.Value = _workShift.ShiftDate;
                dtpShiftDate.Checked = true;
                if (_workShift.StartTime.HasValue)
                {
                    dtpStartTime.Value = DateTime.Today.Add(_workShift.StartTime.Value);
                    dtpStartTime.Checked = true;
                }
                else
                {
                    dtpStartTime.Checked = false;
                }
                if (_workShift.EndTime.HasValue)
                {
                    dtpEndTime.Value = DateTime.Today.Add(_workShift.EndTime.Value);
                    dtpEndTime.Checked = true;
                }
                else
                {
                    dtpEndTime.Checked = false;
                }
                txtNotes.Text = _workShift.Notes ?? "";
            }
            else
            {
                dtpShiftDate.Value = DateTime.Today;
                dtpShiftDate.Checked = true;
                dtpStartTime.Value = DateTime.Today.AddHours(8);
                dtpStartTime.Checked = true;
                dtpEndTime.Value = DateTime.Today.AddHours(17);
                dtpEndTime.Checked = true;
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbStaff.SelectedIndex < 0 || cmbStaff.SelectedIndex >= _staffList.Count)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedStaff = _staffList[cmbStaff.SelectedIndex];
                var shiftDate = dtpShiftDate.Value.Date;

                if (_isEditMode && _workShift != null)
                {
                    // Cập nhật ca làm việc
                    _workShift.UserId = selectedStaff.UserId;
                    _workShift.ShiftDate = shiftDate;
                    _workShift.StartTime = dtpStartTime.Checked && dtpStartTime.Value != null ? dtpStartTime.Value.TimeOfDay : (TimeSpan?)null;
                    _workShift.EndTime = dtpEndTime.Checked && dtpEndTime.Value != null ? dtpEndTime.Value.TimeOfDay : (TimeSpan?)null;
                    _workShift.Notes = txtNotes.Text?.Trim() ?? "";

                    var result = await AppServices.WorkShiftService.UpdateAsync(_workShift);
                    if (result?.Success == true)
                    {
                        MessageBox.Show("Đã cập nhật ca làm việc thành công.", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show($"Lỗi: {result?.Message ?? "Không xác định"}", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Tạo ca làm việc mới
                    var newShift = new WorkShift
                    {
                        UserId = selectedStaff.UserId,
                        ShiftDate = shiftDate,
                        StartTime = dtpStartTime.Checked && dtpStartTime.Value != null ? dtpStartTime.Value.TimeOfDay : (TimeSpan?)null,
                        EndTime = dtpEndTime.Checked && dtpEndTime.Value != null ? dtpEndTime.Value.TimeOfDay : (TimeSpan?)null,
                        Notes = txtNotes.Text?.Trim() ?? ""
                    };

                    var result = await AppServices.WorkShiftService.CreateAsync(newShift);
                    if (result?.Success == true)
                    {
                        MessageBox.Show("Đã tạo ca làm việc thành công.", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show($"Lỗi: {result?.Message ?? "Không xác định"}", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #region Designer Components
        private Guna2ComboBox cmbStaff;
        private Guna2DateTimePicker dtpShiftDate;
        private Guna2DateTimePicker dtpStartTime;
        private Guna2DateTimePicker dtpEndTime;
        private Guna2TextBox txtNotes;
        private Guna2Button btnSave;
        private Guna2Button btnCancel;
        private Label lblStaff;
        private Label lblShiftDate;
        private Label lblStartTime;
        private Label lblEndTime;
        private Label lblNotes;
        #endregion
    }
}

