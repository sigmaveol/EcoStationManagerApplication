using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.UI.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class WorkShiftForm : Form
    {
        private readonly WorkShift _editingShift;
        private bool _isEditMode => _editingShift != null;

        public WorkShiftForm(WorkShift shift)
        {
            InitializeComponent();
            _editingShift = shift;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private async void WorkShiftForm_Load(object sender, EventArgs e)
        {
            await LoadStaffListAsync();

            if (_isEditMode)
                LoadShiftToControls();
        }

        //============================================
        // Load Staff into ComboBox
        //============================================
        private async Task LoadStaffListAsync()
        {
            var result = await AppServices.UserService.GetActiveStaffAsync();

            if (result?.Success != true || result.Data == null || !result.Data.Any())
            {
                MessageBox.Show("Không thể tải danh sách nhân viên.",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            cbStaff.DataSource = result.Data;
            cbStaff.DisplayMember = "Fullname";
            cbStaff.ValueMember = "UserId";
        }

        //============================================
        // Load data if in Edit Mode
        //============================================
        private void LoadShiftToControls()
        {
            cbStaff.SelectedValue = _editingShift.UserId;
            dtpShiftDate.Value = _editingShift.ShiftDate;
            dtpStart.Value = DateTime.Today.Add(_editingShift.StartTime ?? TimeSpan.Zero);
            dtpEnd.Value = DateTime.Today.Add(_editingShift.EndTime ?? TimeSpan.Zero);
            numKpi.Value = _editingShift.KpiScore ?? 0;
            txtNotes.Text = _editingShift.Notes;

            Text = "Chỉnh sửa ca làm việc";
        }

        //============================================
        // Validate data
        //============================================
        private bool ValidateForm()
        {
            if (cbStaff.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên.", "Thiếu thông tin",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dtpEnd.Value <= dtpStart.Value)
            {
                MessageBox.Show("Giờ kết thúc phải lớn hơn giờ bắt đầu.",
                                "Thời gian không hợp lệ",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        //============================================
        // Save event
        //============================================
        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            var newShift = new WorkShift
            {
                ShiftId = _editingShift?.ShiftId ?? 0,
                UserId = (int)cbStaff.SelectedValue,
                ShiftDate = dtpShiftDate.Value.Date,
                StartTime = dtpStart.Value.TimeOfDay,
                EndTime = dtpEnd.Value.TimeOfDay,
                KpiScore = numKpi.Value,
                Notes = txtNotes.Text?.Trim()
            };

            bool success = false;
            string message = string.Empty;

            if (_isEditMode)
            {
                var result = await AppServices.WorkShiftService.UpdateAsync(newShift);
                success = result?.Success == true;
                message = result?.Message ?? "Không thể cập nhật ca làm việc";
            }
            else
            {
                var result = await AppServices.WorkShiftService.CreateAsync(newShift);
                success = result?.Success == true;
                message = result?.Message ?? "Không thể tạo ca làm việc";
            }

            if (!success)
            {
                MessageBox.Show($"Không thể lưu ca làm việc: {message}",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Lưu ca làm việc thành công!",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
            => this.Close();
    }
}
