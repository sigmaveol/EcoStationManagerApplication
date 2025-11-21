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
        private DateTime? _defaultDate = null;
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
            _defaultDate = date;
        }

        private async void CleaningScheduleForm_Load(object sender, EventArgs e)
        {
            await LoadStaffListAsync();
            LoadComboBox();

            if (_isEditMode)
            {
                LoadScheduleToControls();
            }
            else
            {
                dtpCleaningDate.Value = _defaultDate.HasValue ? _defaultDate.Value : DateTime.Now;
                
                // Set thời gian mặc định (làm tròn xuống 15 phút gần nhất)
                var now = DateTime.Now;
                int roundedMinutes = (now.Minute / 15) * 15;
                var defaultTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, roundedMinutes, 0);
                cmbCleaningTime.Text = defaultTime.ToString("HH:mm");
                
                cmbCleaningType.SelectedItem = CleaningType.TANK;
                cmbStatus.SelectedItem = CleaningStatus.SCHEDULED;
                
                // Đảm bảo các control có thể chỉnh sửa
                dtpCleaningDate.Enabled = true;
                cmbCleaningTime.Enabled = true;
            }
        }

        //============================================
        // Load Status ComboBox
        //============================================
        private void LoadComboBox()
        {
            cmbCleaningType.DataSource = Enum.GetValues(typeof(CleaningType));
            cmbStatus.DataSource = Enum.GetValues(typeof(CleaningStatus));
            LoadTimeComboBox();
        }

        //============================================
        // Load Time ComboBox với các giá trị cách nhau 15 phút
        //============================================
        private void LoadTimeComboBox()
        {
            cmbCleaningTime.Items.Clear();
            
            // Tạo danh sách thời gian cách nhau 15 phút (00:00 đến 23:45)
            for (int hour = 0; hour < 24; hour++)
            {
                for (int minute = 0; minute < 60; minute += 15)
                {
                    string timeText = $"{hour:00}:{minute:00}";
                    cmbCleaningTime.Items.Add(timeText);
                }
            }
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
            
            // Load Status - khi dùng DataSource với Enum.GetValues, SelectedItem là enum value trực tiếp
            cmbStatus.SelectedItem = _editingSchedule.Status;
            
            // Nếu status là COMPLETED và có CleanedDatetime, dùng CleanedDatetime; ngược lại dùng CleaningDate
            DateTime dateTimeToDisplay;
            if (_editingSchedule.Status == CleaningStatus.COMPLETED && _editingSchedule.CleanedDatetime.HasValue)
            {
                dateTimeToDisplay = _editingSchedule.CleanedDatetime.Value;
            }
            else
            {
                dateTimeToDisplay = _editingSchedule.CleaningDate;
            }
            
            dtpCleaningDate.Value = dateTimeToDisplay.Date;
            
            // Load thời gian vào ComboBox (format HH:mm)
            cmbCleaningTime.Text = dateTimeToDisplay.ToString("HH:mm");
            
            if (_editingSchedule.CleaningBy.HasValue)
            {
                cbCleanedBy.SelectedValue = _editingSchedule.CleaningBy.Value;
            }

            txtNotes.Text = ExtractOtherNotes(_editingSchedule.Notes);

            // Đảm bảo các control có thể chỉnh sửa
            dtpCleaningDate.Enabled = true;
            cmbCleaningTime.Enabled = true;

            if (_isCompletionMode)
            {
                Text = "Hoàn thành vệ sinh";
                lblCleanedBy.Text = "Người vệ sinh:";
                lblCleanedBy.Enabled = false;
                cbCleanedBy.Enabled = false;
                lblStatus.Enabled = false;
                cmbStatus.Enabled = false; // Không cho chỉnh sửa Status trong completion mode
                btnSave.Enabled = false;
                // Nếu chưa có CleanedDatetime, cập nhật thời gian thực tế (làm tròn xuống 15 phút gần nhất)
                if (!_editingSchedule.CleanedDatetime.HasValue)
                {
                    var now = DateTime.Now;
                    int roundedMinutes = (now.Minute / 15) * 15;
                    var currentTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, roundedMinutes, 0);
                    cmbCleaningTime.Text = currentTime.ToString("HH:mm");
                }
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
        // Parse Time từ ComboBox (hỗ trợ format HH:mm hoặc H:mm)
        //============================================
        private TimeSpan? ParseTimeFromComboBox()
        {
            string timeText = cmbCleaningTime.Text?.Trim();
            
            if (string.IsNullOrWhiteSpace(timeText))
                return null;

            // Thử parse với format HH:mm hoặc H:mm
            if (TimeSpan.TryParseExact(timeText, "HH:mm", null, out TimeSpan time))
            {
                return time;
            }
            
            if (TimeSpan.TryParseExact(timeText, "H:mm", null, out time))
            {
                return time;
            }

            // Thử parse với format khác (ví dụ: 8:30)
            if (TimeSpan.TryParse(timeText, out time))
            {
                return time;
            }

            return null;
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

            // Validate thời gian
            var timeSpan = ParseTimeFromComboBox();
            if (!timeSpan.HasValue)
            {
                MessageBox.Show("Vui lòng nhập thời gian hợp lệ (định dạng HH:mm, ví dụ: 08:30 hoặc 14:15).", 
                                "Thời gian không hợp lệ",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCleaningTime.Focus();
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
                
                // Parse thời gian từ ComboBox
                var timeSpan = ParseTimeFromComboBox();
                if (!timeSpan.HasValue)
                {
                    MessageBox.Show("Thời gian không hợp lệ.", "Lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                var selectedDateTime = dtpCleaningDate.Value.Date.Add(timeSpan.Value);
                
                _editingSchedule.CleaningType = scheduleType;
                _editingSchedule.Status = status;
                
                // Chỉ set CleanedDatetime khi status là COMPLETED
                if (status == CleaningStatus.COMPLETED)
                {
                    // Khi hoàn thành, set CleanedDatetime với now
                    _editingSchedule.CleanedDatetime = DateTime.Now;
                }
                else
                {
                    // Khi không phải COMPLETED, cập nhật CleaningDate và xóa CleanedDatetime
                    _editingSchedule.CleaningDate = selectedDateTime;
                    _editingSchedule.CleanedDatetime = null;
                }
                
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

                // UIHelper tự động hiển thị message từ Service (Core đã có validation và message)
                UIHelper.HandleServiceResult(result, (success) =>
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                });
                return; // Đóng form nếu thành công, hoặc return nếu lỗi (UIHelper đã hiển thị message)
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
                
                // Parse thời gian từ ComboBox
                var timeSpan = ParseTimeFromComboBox();
                if (!timeSpan.HasValue)
                {
                    MessageBox.Show("Thời gian không hợp lệ.", "Lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                var cleaningDateTime = dtpCleaningDate.Value.Date.Add(timeSpan.Value);
                
                var newSchedule = new CleaningSchedule
                {
                    CleaningType = scheduleType,
                    CleaningDate = cleaningDateTime,
                    Status = status,
                    Notes = txtNotes.Text?.Trim()
                };

                // Chỉ set CleanedDatetime khi status là COMPLETED
                if (status == CleaningStatus.COMPLETED)
                {
                    newSchedule.CleanedDatetime = cleaningDateTime;
                }

                if (cbCleanedBy.SelectedItem != null && cbCleanedBy.SelectedValue != null && cbCleanedBy.SelectedValue is int)
                {
                    newSchedule.CleaningBy = (int)cbCleanedBy.SelectedValue;
                }

                var result = await AppServices.CleaningScheduleService.CreateAsync(newSchedule);

                // UIHelper tự động hiển thị message từ Service (Core đã có validation và message)
                UIHelper.HandleServiceResult(result, (scheduleId) =>
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                });
                return; // Đóng form nếu thành công, hoặc return nếu lỗi (UIHelper đã hiển thị message)
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) => this.Close();
    }
}

