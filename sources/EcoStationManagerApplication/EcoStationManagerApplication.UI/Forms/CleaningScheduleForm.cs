using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.UI.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class CleaningScheduleForm : Form
    {
        private readonly CleaningSchedule _editingSchedule;
        private bool _isEditMode => _editingSchedule != null;
        private bool _isCompletionMode => _editingSchedule != null && _editingSchedule.Status == CleaningStatus.COMPLETED;

        public CleaningScheduleForm(CleaningSchedule schedule = null)
        {
            InitializeComponent();
            _editingSchedule = schedule;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        /// <summary>
        /// Set ngày mặc định khi tạo lịch mới từ calendar
        /// </summary>
        public void SetDefaultDate(DateTime date)
        {
            dtpCleaningDate.Value = date;
            dtpCleaningTime.Value = date;
        }

        private async void CleaningScheduleForm_Load(object sender, EventArgs e)
        {
            await LoadStaffListAsync();
            LoadStatusComboBox();

            if (_isEditMode)
                LoadScheduleToControls();
            else
            {
                // Mặc định cho lịch mới
                dtpCleaningDate.Value = DateTime.Now;
                dtpCleaningTime.Value = DateTime.Now;
                cmbCleaningType.SelectedIndex = 0; // TANK
                cmbStatus.SelectedIndex = 0; // SCHEDULED
            }
        }

        //============================================
        // Load Status ComboBox
        //============================================
        private void LoadStatusComboBox()
        {
            cmbCleaningType.DataSource = Enum.GetValues(typeof(CleaningType));
            cmbStatus.DataSource = Enum.GetValues(typeof(CleaningStatus));
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

            cbCleanedBy.DataSource = result.Data;
            cbCleanedBy.DisplayMember = "Fullname";
            cbCleanedBy.ValueMember = "UserId";
            
            // Nếu là chế độ hoàn thành và có người vệ sinh, chọn người đó
            if (_isCompletionMode && _editingSchedule.CleaningBy.HasValue)
            {
                cbCleanedBy.SelectedValue = _editingSchedule.CleaningBy.Value;
            }
        }

        //============================================
        // Load data if in Edit Mode
        //============================================
        private void LoadScheduleToControls()
        {
            // Load CleaningType - khi dùng DataSource với Enum.GetValues, SelectedItem là enum value trực tiếp
            cmbCleaningType.SelectedItem = _editingSchedule.CleaningType;
            
            dtpCleaningDate.Value = _editingSchedule.CleaningDate.Date;
            dtpCleaningTime.Value = DateTime.Today.Add(_editingSchedule.CleaningDate.TimeOfDay);
            
            // Load Status - khi dùng DataSource với Enum.GetValues, SelectedItem là enum value trực tiếp
            cmbStatus.SelectedItem = _editingSchedule.Status;
            
            if (_editingSchedule.CleaningBy.HasValue)
            {
                cbCleanedBy.SelectedValue = _editingSchedule.CleaningBy.Value;
            }

            txtNotes.Text = ExtractOtherNotes(_editingSchedule.Notes);

            if (_isCompletionMode)
            {
                Text = "Hoàn thành vệ sinh";
                lblCleanedBy.Text = "Người vệ sinh:";
                lblCleanedBy.Enabled = false;
                cbCleanedBy.Enabled = false;
                lblStatus.Enabled = false;
                cmbStatus.Enabled = false; // Không cho chỉnh sửa Status trong completion mode
                btnSave.Enabled = false;
                dtpCleaningTime.Value = DateTime.Now; // Cập nhật thời gian thực tế
            }
            else
            {
                Text = "Chỉnh sửa lịch vệ sinh";
                lblStatus.Visible = true;
                cmbStatus.Visible = true;

            }
        }

        //============================================
        // Extract Other Notes (excluding system info)
        //============================================
        private string ExtractOtherNotes(string notes)
        {
            if (string.IsNullOrWhiteSpace(notes))
                return string.Empty;

            // Lấy phần ghi chú không phải thông tin hệ thống (Người vệ sinh, Ngày giờ)
            var lines = notes.Split('\n');
            var otherNotes = new System.Collections.Generic.List<string>();
            
            foreach (var line in lines)
            {
                if (!line.Contains("Người vệ sinh:") && !line.Contains("Ngày giờ:"))
                {
                    otherNotes.Add(line);
                }
            }
            
            return string.Join("\n", otherNotes);
        }

        //============================================
        // Validate data
        //============================================
        private bool ValidateForm()
        {
            if (cmbCleaningType.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn loại vệ sinh.", "Thiếu thông tin",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (_isCompletionMode && (cbCleanedBy.SelectedItem == null || cbCleanedBy.SelectedValue == null))
            {
                MessageBox.Show("Vui lòng chọn người vệ sinh.", "Thiếu thông tin",
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

            if (_isEditMode)
            {
                // Chế độ chỉnh sửa: Update thông tin lịch
                if (cmbCleaningType.SelectedItem == null || cmbCleaningType.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn loại vệ sinh.", "Thiếu thông tin",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                if (cmbStatus.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn trạng thái.", "Thiếu thông tin",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Khi dùng DataSource với Enum.GetValues, SelectedItem là enum value trực tiếp
                var scheduleType = (CleaningType)cmbCleaningType.SelectedItem;
                var status = (CleaningStatus)cmbStatus.SelectedItem;
                var cleaningDateTime = dtpCleaningDate.Value.Date.Add(dtpCleaningTime.Value.TimeOfDay);
                
                _editingSchedule.CleaningType = scheduleType;
                _editingSchedule.CleaningDate = cleaningDateTime;
                _editingSchedule.Status = status;
                
                if (cbCleanedBy.SelectedItem != null && cbCleanedBy.SelectedValue != null && cbCleanedBy.SelectedValue is int)
                {
                    _editingSchedule.CleaningBy = (int)cbCleanedBy.SelectedValue;
                }
                else
                {
                    _editingSchedule.CleaningBy = null;
                }
                
                if (!string.IsNullOrWhiteSpace(txtNotes.Text))
                {
                    _editingSchedule.Notes = txtNotes.Text.Trim();
                }

                var result = await AppServices.CleaningScheduleService.UpdateAsync(_editingSchedule);

                if (result?.Success != true)
                {
                    MessageBox.Show($"Không thể cập nhật lịch vệ sinh: {result?.Message}",
                                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Đã cập nhật lịch vệ sinh thành công!",
                                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Chế độ tạo mới
                if (cmbCleaningType.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn loại vệ sinh.", "Thiếu thông tin",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                if (cmbStatus.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn trạng thái.", "Thiếu thông tin",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Khi dùng DataSource với Enum.GetValues, SelectedItem là enum value trực tiếp
                var scheduleType = (CleaningType)cmbCleaningType.SelectedItem;
                var status = (CleaningStatus)cmbStatus.SelectedItem;
                var cleaningDateTime = dtpCleaningDate.Value.Date.Add(dtpCleaningTime.Value.TimeOfDay);
                
                var newSchedule = new CleaningSchedule
                {
                    CleaningType = scheduleType,
                    CleaningDate = cleaningDateTime,
                    Status = status,
                    Notes = txtNotes.Text?.Trim()
                };

                if (cbCleanedBy.SelectedItem != null && cbCleanedBy.SelectedValue != null && cbCleanedBy.SelectedValue is int)
                {
                    newSchedule.CleaningBy = (int)cbCleanedBy.SelectedValue;
                }

                var result = await AppServices.CleaningScheduleService.CreateAsync(newSchedule);

                if (result?.Success != true)
                {
                    MessageBox.Show($"Không thể tạo lịch vệ sinh: {result?.Message}",
                                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Đã tạo lịch vệ sinh thành công!",
                                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
            => this.Close();
    }
}

