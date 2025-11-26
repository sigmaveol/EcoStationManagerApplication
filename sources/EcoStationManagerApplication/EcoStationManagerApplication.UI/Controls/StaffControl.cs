using EcoStationManagerApplication.Common.Exporters;
using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.UI.Common;
using EcoStationManagerApplication.UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class StaffControl : UserControl, IRefreshableControl
    {
        private readonly BindingList<CleaningScheduleRow> _cleaningScheduleSource = new BindingList<CleaningScheduleRow>();
        private Dictionary<DateTime, List<CleaningSchedule>> _cleaningSchedulesByDate = new Dictionary<DateTime, List<CleaningSchedule>>();
        private List<WorkShift> _workShifts = new List<WorkShift>();
        private List<DeliveryAssignment> _deliveryAssignments = new List<DeliveryAssignment>();
        private bool _isLoading = false;

        public StaffControl()
        {
            InitializeComponent();
            InitializeCleaningScheduleEvents();
            InitializeWorkShiftEvents();
            InitializeDeliveryEvents();
            InitializeDataGridColumns();
        }

        public void RefreshData()
        {
            _ = LoadAllDataAsync();
        }

        private async void StaffControl_Load(object sender, EventArgs e)
        {
            await LoadAllDataAsync();
        }

        /// <summary>
        /// Load tất cả dữ liệu
        /// </summary>
        private async Task LoadAllDataAsync()
        {
            if (_isLoading) return;

            try
            {
                _isLoading = true;
                await Task.WhenAll(
                    LoadCleaningScheduleDataAsync(),
                    LoadWorkShiftDataAsync(),
                    LoadDeliveryAssignmentDataAsync(),
                    LoadDashboardDataAsync()
                );
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "tải dữ liệu nhân sự");
            }
            finally
            {
                _isLoading = false;
            }
        }

        #region Cleaning Schedule Methods

        /// <summary>
        /// Load cleaning schedules và hiển thị trên CalendarControl
        /// </summary>
        private async Task LoadCleaningScheduleDataAsync()
        {
            _cleaningScheduleSource.Clear();
            _cleaningSchedulesByDate.Clear();

            try
            {
                if (calendarControl == null) {
                    return;                
                }

                // Lấy tất cả lịch vệ sinh
                var allSchedulesResult = await AppServices.CleaningScheduleService.GetAllAsync();
                var allSchedules = allSchedulesResult?.Data?.ToList() ?? new List<CleaningSchedule>();

                // Lấy danh sách nhân viên để map tên
                var staffResult = await AppServices.UserService.GetActiveStaffAsync();
                var staffList = staffResult?.Data?.ToList() ?? new List<User>();
                var staffDict = staffList.ToDictionary(s => s.UserId, s => s.Fullname ?? s.Username);

                // Xóa tất cả events cũ trên calendar
                calendarControl.ClearAllEvents();

                // Nhóm lịch theo ngày và thêm vào calendar
                foreach (var schedule in allSchedules)
                {
                    var dateKey = schedule.CleaningDate.Date;
                    
                    // Thêm vào dictionary để tra cứu sau
                    if (!_cleaningSchedulesByDate.ContainsKey(dateKey))
                    {
                        _cleaningSchedulesByDate[dateKey] = new List<CleaningSchedule>();
                    }
                    _cleaningSchedulesByDate[dateKey].Add(schedule);

                    // Tạo CalendarEvent để hiển thị trên calendar
                    var eventColor = GetStatusColor(schedule.Status);
                    var eventTitle = $"{GetCleaningTypeDisplayName(schedule.CleaningType)} - {GetStatusDisplayName(schedule.Status)}";
                    
                    // Thêm thông tin CleanedDatetime vào description nếu có
                    var description = schedule.Notes ?? "";
                    if (schedule.CleanedDatetime.HasValue && schedule.Status == CleaningStatus.COMPLETED)
                    {
                        if (!string.IsNullOrEmpty(description))
                            description += "\n";
                        description += $"Đã hoàn thành: {schedule.CleanedDatetime.Value:dd/MM/yyyy HH:mm}";
                    }
                    
                    var calendarEvent = new CalendarEvent
                    {
                        Id = schedule.CsId.ToString(),
                        Title = eventTitle,
                        Description = description,
                        EventColor = eventColor,
                        Tag = schedule
                    };


                    calendarControl.AddEvent(dateKey, calendarEvent);

                    // Thêm vào BindingList để hiển thị trong DataGridView (nếu có)
                    var cleanedByName = schedule.CleaningBy.HasValue && staffDict.ContainsKey(schedule.CleaningBy.Value)
                        ? staffDict[schedule.CleaningBy.Value]
                        : "Chưa phân công";

                    // Sử dụng CleanedDatetime từ entity, nếu không có thì fallback về extract từ Notes
                    var cleanedDateTime = schedule.CleanedDatetime.HasValue ? schedule.CleanedDatetime.Value.ToString("dd/MM/yyyy HH:mm") : "";


                    _cleaningScheduleSource.Add(new CleaningScheduleRow
                    {
                        ScheduleId = schedule.CsId,
                        CleaningType = GetCleaningTypeDisplayName(schedule.CleaningType),
                        ScheduledDate = schedule.CleaningDate.ToString("dd/MM/yyyy HH:mm"),
                        CleanedBy = cleanedByName,
                        CleanedDateTime = cleanedDateTime,
                        Status = GetStatusDisplayName(schedule.Status),
                        Notes = schedule.Notes
                    });
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "tải lịch vệ sinh");
            }
        }

        /// <summary>
        /// Lấy màu hiển thị theo trạng thái
        /// </summary>
        private Color GetStatusColor(CleaningStatus status)
        {
            switch (status)
            {
                case CleaningStatus.SCHEDULED:
                    return Color.Blue;
                case CleaningStatus.COMPLETED:
                    return Color.Green;
                case CleaningStatus.OVERDUE:
                    return Color.Red;
                case CleaningStatus.CANCELLED:
                    return Color.Gray;
                default:
                    return Color.Black;
            }
        }

        private string GetCleaningTypeDisplayName(CleaningType type)
        {
            return type == CleaningType.TANK ? "Bồn chứa" : "Bao bì";
        }

        private string GetStatusDisplayName(CleaningStatus status)
        {
            switch (status)
            {
                case CleaningStatus.SCHEDULED:
                    return "Đã lên lịch";
                case CleaningStatus.COMPLETED:
                    return "Đã hoàn thành";
                case CleaningStatus.OVERDUE:
                    return "Quá hạn";
                case CleaningStatus.CANCELLED:
                    return "Đã hủy";
                default:
                    return status.ToString();
            }
        }

        #endregion

        #region Cleaning Schedule Event Handlers

        /// <summary>
        /// Khởi tạo events cho CalendarControl
        /// </summary>
        private void InitializeCleaningScheduleEvents()
        {
            if (calendarControl != null)
            {
                calendarControl.DateDoubleClick += CalendarControl_DateDoubleClick;
                calendarControl.DateClick += CalendarControl_DateClick;
            }
        }

        /// <summary>
        /// Xử lý khi click vào ngày trên calendar
        /// </summary>
        private async void CalendarControl_DateClick(object sender, CalendarDateEventArgs e)
        {
            try
            {
                var selectedDate = e.Date.Date;
                
                // Kiểm tra xem có lịch vệ sinh nào trong ngày này không
                var hasSchedules = _cleaningSchedulesByDate.ContainsKey(selectedDate) && 
                                   _cleaningSchedulesByDate[selectedDate].Any();
                
                var schedules = hasSchedules 
                    ? _cleaningSchedulesByDate[selectedDate] 
                    : new List<CleaningSchedule>();

                // Hiển thị dialog với thông tin lịch và nút thêm lịch
                await ShowCleaningScheduleInfoDialog(selectedDate, schedules);
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "hiển thị thông tin lịch");
            }
        }

        /// <summary>
        /// Xử lý khi double-click vào ngày trên calendar
        /// </summary>
        private async void CalendarControl_DateDoubleClick(object sender, DateTime e)
        {
            try
            {
                var selectedDate = e.Date;
                
                // Kiểm tra xem có lịch vệ sinh nào trong ngày này không
                if (_cleaningSchedulesByDate.ContainsKey(selectedDate) && 
                    _cleaningSchedulesByDate[selectedDate].Any())
                {
                    var schedules = _cleaningSchedulesByDate[selectedDate];
                    
                    // Nếu có nhiều lịch, hiển thị dialog chọn
                    if (schedules.Count == 1)
                    {
                        await ShowCleaningScheduleDialog(schedules[0]);
                    }
                    else
                    {
                        await ShowCleaningScheduleListDialog(selectedDate, schedules);
                    }
                }
                else
                {
                    // Không có lịch, mở form tạo mới
                    using (var form = new CleaningScheduleForm())
                    {
                        form.SetDefaultDate(selectedDate);
                        var owner = this.FindForm();
                        DialogResult result = owner != null
                            ? FormHelper.ShowModalWithDim(owner, form)
                            : form.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            await LoadCleaningScheduleDataAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "xử lý lịch vệ sinh");
            }
        }

        /// <summary>
        /// Hiển thị dialog thông tin lịch vệ sinh khi click vào ngày trên calendar
        /// </summary>
        private async Task ShowCleaningScheduleInfoDialog(DateTime date, List<CleaningSchedule> schedules)
        {
            var dialog = new Form
            {
                Text = $"Lịch vệ sinh - {date:dd/MM/yyyy}",
                Size = new Size(700, 500),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                Padding = new Padding(15)
            };

            // Label tiêu đề
            var lblTitle = new Label
            {
                Text = $"Lịch vệ sinh ngày {date:dd/MM/yyyy}",
                Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold),
                Location = new Point(15, 15),
                AutoSize = true
            };

            // Panel chứa danh sách lịch
            var panelSchedules = new Panel
            {
                Location = new Point(15, 50),
                Size = new Size(660, schedules.Any() ? 300 : 100),
                BorderStyle = BorderStyle.FixedSingle,
                AutoScroll = true
            };

            int yPos = 10;
            
            if (schedules.Any())
            {
                // Lấy danh sách nhân viên để map tên
                var staffResult = await AppServices.UserService.GetActiveStaffAsync();
                var staffList = staffResult?.Data?.ToList() ?? new List<User>();
                var staffDict = staffList.ToDictionary(s => s.UserId, s => s.Fullname ?? s.Username);

                foreach (var schedule in schedules.OrderBy(s => s.CleaningDate))
                {
                    var statusColor = GetStatusColor(schedule.Status);

                    var schedulePanel = new Panel
                    {
                        Location = new Point(10, yPos),
                        Size = new Size(640, 80),
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = Color.White,
                        Padding = new Padding(1)
                    };

                    // Vẽ viền với màu trạng thái
                    schedulePanel.Paint += (s, pe) =>
                    {
                        using (var pen = new Pen(statusColor, 3))
                        {
                            pe.Graphics.DrawRectangle(pen, 0, 0, schedulePanel.Width - 1, schedulePanel.Height - 1);
                        }
                    };

                    var lblType = new Label
                    {
                        Text = $"Loại: {GetCleaningTypeDisplayName(schedule.CleaningType)}",
                        Location = new Point(10, 5),
                        AutoSize = true,
                        Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold)
                    };

                    // Hiển thị giờ: nếu có CleanedDatetime thì dùng CleanedDatetime, ngược lại dùng CleaningDate
                    var displayTime = schedule.CleanedDatetime.HasValue 
                        ? schedule.CleanedDatetime.Value 
                        : schedule.CleaningDate;
                    
                    var lblTime = new Label
                    {
                        Text = $"Giờ: {displayTime:HH:mm}",
                        Location = new Point(10, 25),
                        AutoSize = true
                    };

                    var cleanedByName = schedule.CleaningBy.HasValue && staffDict.ContainsKey(schedule.CleaningBy.Value)
                        ? staffDict[schedule.CleaningBy.Value]
                        : "Chưa phân công";

                    var lblAssigned = new Label
                    {
                        Text = $"Người thực hiện: {cleanedByName}",
                        Location = new Point(10, 45),
                        AutoSize = true
                    };

                    var lblStatus = new Label
                    {
                        Text = $"Trạng thái: {GetStatusDisplayName(schedule.Status)}",
                        Location = new Point(300, 5),
                        AutoSize = true,
                        Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold)
                    };

                    var cleanedDateTime = schedule.CleanedDatetime.HasValue
                        ? schedule.CleanedDatetime.Value.ToString("dd/MM/yyyy HH:mm")
                        : "Chưa vệ sinh";
                    
                    var lblCleaned = new Label
                    {
                        Text = $"Đã vệ sinh: {cleanedDateTime}",
                        Location = new Point(300, 45),
                        AutoSize = true
                    };

                    // Nút xem chi tiết
                    var btnView = new Button
                    {
                        Text = "Chi tiết",
                        Location = new Point(550, 25),
                        Size = new Size(80, 30),
                        Tag = schedule
                    };
                    btnView.Click += async (s, e) =>
                    {
                        dialog.Close();
                        await ShowCleaningScheduleDialog(schedule);
                        await LoadCleaningScheduleDataAsync();
                    };

                    schedulePanel.Controls.Add(lblType);
                    schedulePanel.Controls.Add(lblTime);
                    schedulePanel.Controls.Add(lblAssigned);
                    schedulePanel.Controls.Add(lblStatus);
                    schedulePanel.Controls.Add(lblCleaned);
                    schedulePanel.Controls.Add(btnView);

                    panelSchedules.Controls.Add(schedulePanel);
                    yPos += 90;
                }
            }
            else
            {
                var lblNoSchedule = new Label
                {
                    Text = "Chưa có lịch vệ sinh nào trong ngày này.",
                    Location = new Point(10, 40),
                    AutoSize = true,
                    Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Italic),
                    ForeColor = Color.Gray
                };
                panelSchedules.Controls.Add(lblNoSchedule);
            }

            // Nút Thêm lịch
            var btnAdd = new Button
            {
                Text = "➕ Thêm lịch",
                Location = new Point(15, schedules.Any() ? 360 : 160),
                Size = new Size(150, 40),
                Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 125, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += async (s, e) =>
            {
                dialog.Close();
                using (var form = new CleaningScheduleForm())
                {
                    form.SetDefaultDate(date);
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        await LoadCleaningScheduleDataAsync();
                    }
                }
            };

            // Nút Đóng
            var btnClose = new Button
            {
                Text = "Đóng",
                Location = new Point(575, schedules.Any() ? 360 : 160),
                Size = new Size(100, 40),
                DialogResult = DialogResult.Cancel
            };

            dialog.Controls.Add(lblTitle);
            dialog.Controls.Add(panelSchedules);
            dialog.Controls.Add(btnAdd);
            dialog.Controls.Add(btnClose);

            var owner = this.FindForm();
            if (owner != null)
            {
                FormHelper.ShowModalWithDim(owner, dialog);
            }
            else
            {
                dialog.ShowDialog();
            }
        }

        /// <summary>
        /// Hiển thị dialog danh sách lịch vệ sinh trong ngày
        /// </summary>
        private async Task ShowCleaningScheduleListDialog(DateTime date, List<CleaningSchedule> schedules)
        {
            var dialog = new Form
            {
                Text = $"Lịch vệ sinh - {date:dd/MM/yyyy}",
                Size = new Size(625, 400),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var listView = new ListView
            {
                Location = new Point(15, 15),
                Size = new Size(560, 280),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };

            listView.Columns.Add("ID", 50);
            listView.Columns.Add("Loại", 100);
            listView.Columns.Add("Giờ", 80);
            listView.Columns.Add("Trạng thái", 120);
            listView.Columns.Add("Bồn chứa", 150);

            foreach (var schedule in schedules.OrderBy(s => s.CleaningDate))
            {
                var item = new ListViewItem(schedule.CsId.ToString());
                item.SubItems.Add(GetCleaningTypeDisplayName(schedule.CleaningType));
                item.SubItems.Add(schedule.CleaningDate.ToString("HH:mm"));
                item.SubItems.Add(GetStatusDisplayName(schedule.Status));
                item.Tag = schedule;
                listView.Items.Add(item);
            }

            var btnComplete = new Button
            {
                Text = "Hoàn thành",
                Location = new Point(15, 310),
                Size = new Size(100, 35)
            };
            btnComplete.Click += async (s, e) =>
            {
                if (listView.SelectedItems.Count == 0)
                {
                    UIHelper.ShowWarningMessage("Vui lòng chọn lịch vệ sinh cần hoàn thành.");
                    return;
                }

                var selectedSchedule = listView.SelectedItems[0].Tag as CleaningSchedule;
                if (selectedSchedule != null)
                {
                    dialog.Close();
                    await ShowCleaningScheduleDialog(selectedSchedule);
                }
            };

            var btnAdd = new Button
            {
                Text = "Thêm lịch",
                Location = new Point(130, 310),
                Size = new Size(100, 35)
            };
            btnAdd.Click += (s, e) =>
            {
                dialog.Close();
                using (var form = new CleaningScheduleForm())
                {
                    form.SetDefaultDate(date);
                    var owner = this.FindForm();
                    DialogResult result = owner != null
                        ? FormHelper.ShowModalWithDim(owner, form)
                        : form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        _ = LoadCleaningScheduleDataAsync();
                    }
                }
            };

            var btnClose = new Button
            {
                Text = "Đóng",
                Location = new Point(475, 310),
                Size = new Size(100, 35),
                DialogResult = DialogResult.Cancel
            };

            dialog.Controls.Add(listView);
            dialog.Controls.Add(btnComplete);
            dialog.Controls.Add(btnAdd);
            dialog.Controls.Add(btnClose);

            var owner2 = this.FindForm();
            if (owner2 != null)
            {
                FormHelper.ShowModalWithDim(owner2, dialog);
            }
            else
            {
                dialog.ShowDialog();
            }
        }

        /// <summary>
        /// Hiển thị form để xem/chỉnh sửa/hoàn thành lịch vệ sinh
        /// </summary>
        private async Task ShowCleaningScheduleDialog(CleaningSchedule schedule)
        {
            using (var form = new CleaningScheduleForm(schedule))
            {
                var owner = this.FindForm();
                DialogResult result = owner != null
                    ? FormHelper.ShowModalWithDim(owner, form)
                    : form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    await LoadCleaningScheduleDataAsync();
                }
            }
        }

        #endregion

        #region WorkShift Methods

        /// <summary>
        /// Khởi tạo events cho WorkShift controls
        /// </summary>
        private void InitializeWorkShiftEvents()
        {
            if (btnAddWorkShift != null)
                btnAddWorkShift.Click += BtnAddWorkShift_Click;
            if (btnEditWorkShift != null)
                btnEditWorkShift.Click += BtnEditWorkShift_Click;
            if (btnDeleteWorkShift != null)
                btnDeleteWorkShift.Click += BtnDeleteWorkShift_Click;
            if (btnExportWorkShiftExcel != null)
                btnExportWorkShiftExcel.Click += BtnExportWorkShiftExcel_Click;
            if (btnExportWorkShiftPdf != null)
                btnExportWorkShiftPdf.Click += BtnExportWorkShiftPdf_Click;
            if (txtWorkShiftSearch != null)
                txtWorkShiftSearch.TextChanged += TxtWorkShiftSearch_TextChanged;
            if (cmbWorkShiftRoleFilter != null)
                cmbWorkShiftRoleFilter.SelectedIndexChanged += CmbWorkShiftRoleFilter_SelectedIndexChanged;
            if (dtpWorkShiftDateFilter != null)
                dtpWorkShiftDateFilter.ValueChanged += DtpWorkShiftDateFilter_ValueChanged;
            if (dgvKPI != null)
            {
                dgvKPI.CellDoubleClick += DgvKPI_CellDoubleClick;
                dgvKPI.SelectionChanged += DgvKPI_SelectionChanged;
            }
        }


        /// <summary>
        /// Load và hiển thị WorkShift trong dgvKPI - VERSION CẢI THIỆN
        /// </summary>
        private async Task LoadWorkShiftDataAsync()
        {
            try
            {
                if (dgvKPI == null)
                {
                    System.Diagnostics.Debug.WriteLine("[LoadWorkShiftDataAsync] dgvKPI is NULL!");
                    return;
                }

                System.Diagnostics.Debug.WriteLine("[LoadWorkShiftDataAsync] Starting...");

                var shiftsResult = await AppServices.WorkShiftService.GetAllAsync();
                _workShifts = shiftsResult?.Data?.ToList() ?? new List<WorkShift>();

                System.Diagnostics.Debug.WriteLine($"[LoadWorkShiftDataAsync] Total shifts from service: {_workShifts.Count}");

                // Tự động tính KPI cho các ca cần update (bỏ qua lỗi để không block việc hiển thị)
                try
                {
                    await BatchCalculateKPIAsync(_workShifts);

                    // Reload sau khi tính KPI
                    shiftsResult = await AppServices.WorkShiftService.GetAllAsync();
                    _workShifts = shiftsResult?.Data?.ToList() ?? new List<WorkShift>();
                    System.Diagnostics.Debug.WriteLine($"[LoadWorkShiftDataAsync] After KPI calculation: {_workShifts.Count} shifts");
                }
                catch (Exception kpiEx)
                {
                    System.Diagnostics.Debug.WriteLine($"[LoadWorkShiftDataAsync] KPI calculation error (continuing anyway): {kpiEx.Message}");
                }

                // Lấy danh sách user một lần
                var allUsersResult = await AppServices.UserService.GetAllActiveUsersAsync();
                var allUsers = allUsersResult?.Data?.ToList() ?? new List<User>();
                var staffDict = allUsers.ToDictionary(
                    s => s.UserId, 
                    s => new StaffInfo { Name = s.Fullname ?? s.Username, Role = s.Role }
                );

                System.Diagnostics.Debug.WriteLine($"[LoadWorkShiftDataAsync] Total users: {staffDict.Count}");

                UIHelper.SafeInvoke(this, () =>
                {
                    try
                    {
                        if (dgvKPI == null)
                        {
                            System.Diagnostics.Debug.WriteLine("[LoadWorkShiftDataAsync] dgvKPI is NULL in SafeInvoke!");
                            return;
                        }

                        if (dgvKPI.Columns.Count == 0)
                        {
                            System.Diagnostics.Debug.WriteLine("[LoadWorkShiftDataAsync] Initializing columns...");
                            InitializeDataGridColumns();
                        }

                        dgvKPI.Rows.Clear();
                        System.Diagnostics.Debug.WriteLine($"[LoadWorkShiftDataAsync] Cleared rows. Before filter: {_workShifts.Count} shifts");

                        // ENABLE FILTER - đã sửa logic
                        var filteredShifts = ApplyWorkShiftFilters(_workShifts, staffDict);

                        System.Diagnostics.Debug.WriteLine($"[LoadWorkShiftDataAsync] After filter: {filteredShifts.Count} shifts");

                        int rowsAdded = 0;
                        int rowsFailed = 0;

                        foreach (var shift in filteredShifts.OrderByDescending(s => s.ShiftDate).ThenByDescending(s => s.StartTime))
                        {
                            try
                            {
                                var staffInfo = staffDict.ContainsKey(shift.UserId) 
                                    ? staffDict[shift.UserId] 
                                    : new StaffInfo { Name = $"User ID: {shift.UserId}", Role = UserRole.STAFF };

                                var startTime = shift.StartTime?.ToString(@"hh\:mm") ?? "-";
                                var endTime = shift.EndTime?.ToString(@"hh\:mm") ?? "-";
                                var kpiScore = shift.KpiScore?.ToString("N2") ?? "-";

                                var rowIndex = dgvKPI.Rows.Add(
                                    shift.ShiftId,
                                    staffInfo.Name,
                                    GetRoleDisplayName(staffInfo.Role),
                                    shift.ShiftDate.ToString("dd/MM/yyyy"),
                                    startTime,
                                    endTime,
                                    kpiScore,
                                    shift.Notes ?? ""
                                );

                                dgvKPI.Rows[rowIndex].Tag = shift;
                                rowsAdded++;
                            }
                            catch (Exception ex)
                            {
                                rowsFailed++;
                                System.Diagnostics.Debug.WriteLine($"[LoadWorkShiftDataAsync] Error adding row for shift {shift.ShiftId}: {ex.Message}");
                                System.Diagnostics.Debug.WriteLine($"[LoadWorkShiftDataAsync] Stack trace: {ex.StackTrace}");
                            }
                        }

                        System.Diagnostics.Debug.WriteLine($"[LoadWorkShiftDataAsync] Completed. Rows added: {rowsAdded}, Failed: {rowsFailed}, Total in grid: {dgvKPI.Rows.Count}");
                    }
                    catch (Exception uiEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"[LoadWorkShiftDataAsync] UI update error: {uiEx.Message}");
                        System.Diagnostics.Debug.WriteLine($"[LoadWorkShiftDataAsync] Stack trace: {uiEx.StackTrace}");
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LoadWorkShiftDataAsync] Fatal error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[LoadWorkShiftDataAsync] Stack trace: {ex.StackTrace}");
                UIHelper.ShowExceptionError(ex, "tải danh sách ca làm việc");
            }
        }

        /// <summary>
        /// Tính KPI hàng loạt để tối ưu performance
        /// </summary>
        private async Task BatchCalculateKPIAsync(List<WorkShift> shifts)
        {
            try
            {
                // Chỉ tính cho ca chưa có KPI hoặc trong 7 ngày gần đây
                var shiftsToCalculate = shifts
                    .Where(s => !s.KpiScore.HasValue || (DateTime.Today - s.ShiftDate.Date).Days <= 7)
                    .ToList();

                if (!shiftsToCalculate.Any()) return;

                // Load dữ liệu cần thiết MỘT LẦN
                var allUsersResult = await AppServices.UserService.GetAllActiveUsersAsync();
                var allUsers = allUsersResult?.Data?.ToList() ?? new List<User>();
                var userRoles = allUsers.ToDictionary(u => u.UserId, u => u.Role);

                var allDeliveriesResult = await AppServices.DeliveryService.GetAllAsync();
                var allDeliveries = allDeliveriesResult?.Data?.ToList() ?? new List<DeliveryAssignment>();

                // Load orders từ các status khác nhau để có Order entities
                var ordersReadyResult = await AppServices.OrderService.GetOrdersByStatusAsync(OrderStatus.READY);
                var ordersProcessingResult = await AppServices.OrderService.GetOrdersByStatusAsync(OrderStatus.PROCESSING);
                var ordersConfirmedResult = await AppServices.OrderService.GetOrdersByStatusAsync(OrderStatus.CONFIRMED);
                
                var allOrders = new List<Order>();
                if (ordersReadyResult?.Data != null) allOrders.AddRange(ordersReadyResult.Data);
                if (ordersProcessingResult?.Data != null) allOrders.AddRange(ordersProcessingResult.Data);
                if (ordersConfirmedResult?.Data != null) allOrders.AddRange(ordersConfirmedResult.Data);

                // Tính KPI cho từng ca
                var kpiTasks = shiftsToCalculate.Select(shift => 
                    CalculateSingleKPIAsync(shift, userRoles, allDeliveries, allOrders)
                );

                await Task.WhenAll(kpiTasks);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BatchCalculateKPI] Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Tính KPI cho một ca làm việc
        /// </summary>
        private async Task CalculateSingleKPIAsync(
            WorkShift shift,
            Dictionary<int, UserRole> userRoles,
            List<DeliveryAssignment> allDeliveries,
            List<Order> allOrders,
            int targetOrders = 20)
        {
            try
            {
                if (!userRoles.ContainsKey(shift.UserId)) return;

                var userRole = userRoles[shift.UserId];
                int ordersHandled = 0;
                string kpiType = "";

                switch (userRole)
                {
                    case UserRole.DRIVER:
                        // Đếm đơn giao hàng trong ngày ca làm
                        ordersHandled = allDeliveries.Count(d => 
                            d.DriverId == shift.UserId && 
                            d.AssignedDate.Date == shift.ShiftDate.Date
                        );
                        kpiType = "giao hàng";
                        break;

                    case UserRole.STAFF:
                    case UserRole.MANAGER:
                        // Đếm đơn được xử lý trong ngày ca làm
                        var relevantOrders = allOrders.Where(o => 
                            o.UserId.HasValue &&
                            o.UserId.Value == shift.UserId && 
                            o.LastUpdated.Date == shift.ShiftDate.Date
                        ).ToList();

                        var ordersCreated = relevantOrders.Count();
                        var ordersPrepared = relevantOrders.Count(o => 
                            o.Status == OrderStatus.READY || 
                            o.Status == OrderStatus.PROCESSING
                        );

                        // Logic tính tổng hợp
                        if (ordersPrepared > 0 && ordersCreated > ordersPrepared)
                        {
                            ordersHandled = ordersCreated;
                            kpiType = "xử lý đơn hàng";
                        }
                        else if (ordersPrepared > 0)
                        {
                            ordersHandled = ordersPrepared;
                            kpiType = "chuẩn bị hàng";
                        }
                        else
                        {
                            ordersHandled = ordersCreated;
                            kpiType = "bán hàng";
                        }
                        break;

                    default:
                        return;
                }

                // Tính KPI score
                decimal kpiScore = Math.Min(100, (decimal)ordersHandled / targetOrders * 100);

                // Cập nhật vào database
                var result = await AppServices.WorkShiftService.CalculateKPIAsync(
                    shift.ShiftId, 
                    ordersHandled, 
                    targetOrders
                );

                if (result.Success)
                {
                    shift.KpiScore = kpiScore;
                    System.Diagnostics.Debug.WriteLine(
                        $"[KPI] Shift {shift.ShiftId}: {ordersHandled} {kpiType} = {kpiScore:F2}%"
                    );
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CalculateSingleKPI] Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Áp dụng filters - VERSION ĐÃ SỬA
        /// </summary>
        private List<WorkShift> ApplyWorkShiftFilters(
            List<WorkShift> shifts, 
            Dictionary<int, StaffInfo> staffDict)
        {
            var filtered = shifts.AsEnumerable();

            System.Diagnostics.Debug.WriteLine($"[ApplyWorkShiftFilters] Input: {shifts.Count} shifts");

            // Filter theo search
            if (!string.IsNullOrWhiteSpace(txtWorkShiftSearch?.Text))
            {
                var searchTerm = txtWorkShiftSearch.Text.ToLower();
                System.Diagnostics.Debug.WriteLine($"[ApplyWorkShiftFilters] Applying search filter: '{searchTerm}'");
                filtered = filtered.Where(s =>
                {
                    var staffInfo = staffDict.ContainsKey(s.UserId) ? staffDict[s.UserId] : null;
                    var staffName = staffInfo?.Name?.ToLower() ?? "";
                    
                    return staffName.Contains(searchTerm) ||
                           s.ShiftDate.ToString("dd/MM/yyyy").Contains(searchTerm) ||
                           (s.Notes ?? "").ToLower().Contains(searchTerm) ||
                           s.ShiftId.ToString().Contains(searchTerm);
                });
                System.Diagnostics.Debug.WriteLine($"[ApplyWorkShiftFilters] After search: {filtered.Count()} shifts");
            }

            // Filter theo role
            if (cmbWorkShiftRoleFilter != null && cmbWorkShiftRoleFilter.SelectedIndex > 0)
            {
                var selectedRole = GetRoleFromFilter(cmbWorkShiftRoleFilter.SelectedIndex);
                System.Diagnostics.Debug.WriteLine($"[ApplyWorkShiftFilters] Applying role filter: {selectedRole}");
                filtered = filtered.Where(s =>
                {
                    if (!staffDict.ContainsKey(s.UserId)) return false;
                    return staffDict[s.UserId].Role == selectedRole;
                });
                System.Diagnostics.Debug.WriteLine($"[ApplyWorkShiftFilters] After role: {filtered.Count()} shifts");
            }

            // Filter theo ngày
            if (dtpWorkShiftDateFilter != null && dtpWorkShiftDateFilter.Checked)
            {
                var filterDate = dtpWorkShiftDateFilter.Value.Date;
                System.Diagnostics.Debug.WriteLine($"[ApplyWorkShiftFilters] Applying date filter: {filterDate:dd/MM/yyyy}");
                filtered = filtered.Where(s => s.ShiftDate.Date == filterDate);
                System.Diagnostics.Debug.WriteLine($"[ApplyWorkShiftFilters] After date: {filtered.Count()} shifts");
            }

            var result = filtered.ToList();
            System.Diagnostics.Debug.WriteLine($"[ApplyWorkShiftFilters] Final result: {result.Count} shifts");
            return result;
        }

        /// <summary>
        /// Helper class để lưu thông tin staff
        /// </summary>
        private class StaffInfo
        {
            public string Name { get; set; }
            public UserRole Role { get; set; }
        }

        /// <summary>
        /// Lấy role từ filter index
        /// </summary>
        private UserRole GetRoleFromFilter(int index)
        {
            switch (index)
            {
                case 1: return UserRole.ADMIN;
                case 2: return UserRole.MANAGER;
                case 3: return UserRole.STAFF;
                case 4: return UserRole.DRIVER;
                default: return UserRole.STAFF;
            }
        }

        /// <summary>
        /// Lấy tên hiển thị của role
        /// </summary>
        private string GetRoleDisplayName(UserRole role)
        {
            switch (role)
            {
                case UserRole.ADMIN:
                    return "Quản trị viên";
                case UserRole.MANAGER:
                    return "Quản lý trạm";
                case UserRole.STAFF:
                    return "Nhân viên";
                case UserRole.DRIVER:
                    return "Tài xế";
                default:
                    return role.ToString();
            }
        }

        /// <summary>
        /// Khởi tạo cột cho DataGridView
        /// </summary>
        private void InitializeDataGridColumns()
        {
            // Initialize WorkShift columns (dgvKPI)
            if (dgvKPI != null && dgvKPI.Columns.Count == 0)
            {
                dgvKPI.Columns.Add("ShiftId", "ID");
                dgvKPI.Columns["ShiftId"].Visible = false;
                dgvKPI.Columns.Add("StaffName", "Nhân viên");
                dgvKPI.Columns.Add("Role", "Vai trò");
                dgvKPI.Columns.Add("ShiftDate", "Ngày");
                dgvKPI.Columns.Add("StartTime", "Giờ bắt đầu");
                dgvKPI.Columns.Add("EndTime", "Giờ kết thúc");
                dgvKPI.Columns.Add("KpiScore", "Điểm KPI");
                dgvKPI.Columns.Add("Notes", "Ghi chú");

                dgvKPI.Columns["StaffName"].Width = 200;
                dgvKPI.Columns["Role"].Width = 120;
                dgvKPI.Columns["ShiftDate"].Width = 100;
                dgvKPI.Columns["StartTime"].Width = 100;
                dgvKPI.Columns["EndTime"].Width = 100;
                dgvKPI.Columns["KpiScore"].Width = 100;
                dgvKPI.Columns["Notes"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            // Initialize Delivery columns (dgvAssignments)
            if (dgvAssignments != null && dgvAssignments.Columns.Count == 0)
            {
                dgvAssignments.Columns.Add("AssignmentId", "ID");
                dgvAssignments.Columns["AssignmentId"].Visible = false;
                dgvAssignments.Columns.Add("OrderCode", "Mã đơn");
                dgvAssignments.Columns.Add("DriverName", "Tài xế");
                dgvAssignments.Columns.Add("AssignedDate", "Ngày phân công");
                dgvAssignments.Columns.Add("Status", "Trạng thái");
                dgvAssignments.Columns.Add("CodAmount", "COD");
                dgvAssignments.Columns.Add("PaymentStatus", "Thanh toán");
                dgvAssignments.Columns.Add("Notes", "Ghi chú");

                dgvAssignments.Columns["OrderCode"].Width = 120;
                dgvAssignments.Columns["DriverName"].Width = 150;
                dgvAssignments.Columns["AssignedDate"].Width = 120;
                dgvAssignments.Columns["Status"].Width = 120;
                dgvAssignments.Columns["CodAmount"].Width = 100;
                dgvAssignments.Columns["PaymentStatus"].Width = 100;
                dgvAssignments.Columns["Notes"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        #endregion

        #region WorkShift Event Handlers

        private async void BtnAddWorkShift_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new WorkShiftForm(null))
                {
                    var owner = this.FindForm();
                    DialogResult result = owner != null
                        ? FormHelper.ShowModalWithDim(owner, form)
                        : form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        await LoadWorkShiftDataAsync();
                        await LoadDashboardDataAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "mở form thêm ca làm việc");
            }
        }

        private async void BtnEditWorkShift_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvKPI.SelectedRows.Count == 0)
                {
                    UIHelper.ShowWarningMessage("Vui lòng chọn ca làm việc cần chỉnh sửa.");
                    return;
                }

                var selectedRow = dgvKPI.SelectedRows[0];
                var shift = selectedRow.Tag as WorkShift;

                if (shift != null)
                {
                    using (var form = new WorkShiftForm(shift))
                    {
                        var owner = this.FindForm();
                        DialogResult result = owner != null
                            ? FormHelper.ShowModalWithDim(owner, form)
                            : form.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            await LoadWorkShiftDataAsync();
                            await LoadDashboardDataAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "mở form chỉnh sửa ca làm việc");
            }
        }

        private async void BtnDeleteWorkShift_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvKPI.SelectedRows.Count == 0)
                {
                    UIHelper.ShowWarningMessage("Vui lòng chọn ca làm việc cần xóa.");
                    return;
                }

                var selectedRow = dgvKPI.SelectedRows[0];
                var shift = selectedRow.Tag as WorkShift;

                if (shift != null)
                {
                    var confirm = AppServices.Dialog.ShowConfirm(
                        $"Bạn có chắc chắn muốn xóa ca làm việc của nhân viên vào ngày {shift.ShiftDate:dd/MM/yyyy}?",
                        "Xác nhận xóa");

                    if (confirm)
                    {
                        var result = await AppServices.WorkShiftService.DeleteAsync(shift.ShiftId);
                        AppServices.Dialog.HandleServiceResult(result, () =>
                        {
                            _ = LoadWorkShiftDataAsync();
                            _ = LoadDashboardDataAsync();
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "xóa ca làm việc");
            }
        }

        private async void BtnExportWorkShiftExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvKPI == null || dgvKPI.Rows.Count == 0)
                {
                    AppServices.Dialog.ShowWarning("Không có dữ liệu để xuất. Vui lòng tải dữ liệu trước.");
                    return;
                }

                var saveDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    FileName = $"DanhSachCaLam_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                    Title = "Xuất danh sách ca làm việc ra Excel"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var dataTable = ConvertDataGridViewToDataTable(dgvKPI);
                    
                    var headers = new Dictionary<string, string>
                    {
                        { "StaffName", "Nhân viên" },
                        { "Role", "Vai trò" },
                        { "ShiftDate", "Ngày" },
                        { "StartTime", "Giờ bắt đầu" },
                        { "EndTime", "Giờ kết thúc" },
                        { "KpiScore", "Điểm KPI" },
                        { "Notes", "Ghi chú" }
                    };

                    var excelExporter = new ExcelExporter();
                    excelExporter.ExportToExcel(dataTable, saveDialog.FileName, "Ca làm việc", headers);
                    AppServices.Dialog.ShowSuccess($"Đã xuất Excel thành công!\nFile: {saveDialog.FileName}");
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "xuất Excel ca làm việc");
            }
        }

        private async void BtnExportWorkShiftPdf_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvKPI == null || dgvKPI.Rows.Count == 0)
                {
                    AppServices.Dialog.ShowWarning("Không có dữ liệu để xuất. Vui lòng tải dữ liệu trước.");
                    return;
                }

                var saveDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    FileName = $"DanhSachCaLam_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
                    Title = "Xuất danh sách ca làm việc ra PDF"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var dataTable = ConvertDataGridViewToDataTable(dgvKPI);
                    
                    var headers = new Dictionary<string, string>
                    {
                        { "StaffName", "Nhân viên" },
                        { "Role", "Vai trò" },
                        { "ShiftDate", "Ngày" },
                        { "StartTime", "Giờ bắt đầu" },
                        { "EndTime", "Giờ kết thúc" },
                        { "KpiScore", "Điểm KPI" },
                        { "Notes", "Ghi chú" }
                    };

                    var pdfExporter = new PdfExporter();
                    pdfExporter.ExportToPdf(dataTable, saveDialog.FileName, "BÁO CÁO DANH SÁCH CA LÀM VIỆC", headers);
                    AppServices.Dialog.ShowSuccess($"Đã xuất PDF thành công!\nFile: {saveDialog.FileName}");
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "xuất PDF ca làm việc");
            }
        }

        private async void TxtWorkShiftSearch_TextChanged(object sender, EventArgs e)
        {
            await LoadWorkShiftDataAsync();
        }

        private async void CmbWorkShiftRoleFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            await LoadWorkShiftDataAsync();
        }

        private async void DtpWorkShiftDateFilter_ValueChanged(object sender, EventArgs e)
        {
            await LoadWorkShiftDataAsync();
        }

        private void DgvKPI_SelectionChanged(object sender, EventArgs e)
        {
            // Enable/disable edit và delete buttons dựa trên selection
            bool hasSelection = dgvKPI.SelectedRows.Count > 0;
            if (btnEditWorkShift != null)
                btnEditWorkShift.Enabled = hasSelection;
            if (btnDeleteWorkShift != null)
                btnDeleteWorkShift.Enabled = hasSelection;
        }

        private async void DgvKPI_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                var row = dgvKPI.Rows[e.RowIndex];
                var shift = row.Tag as WorkShift;

                if (shift != null)
                {
                    using (var form = new WorkShiftForm(shift))
                    {
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            await LoadWorkShiftDataAsync();
                            await LoadDashboardDataAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "mở form chỉnh sửa ca làm việc");
            }
        }

        #endregion

        #region Delivery Assignment Methods

        /// <summary>
        /// Khởi tạo events cho Delivery controls
        /// </summary>
        private void InitializeDeliveryEvents()
        {
            if (btnAssignDelivery != null)
                btnAssignDelivery.Click += BtnAssignDelivery_Click;
            if (btnUpdateDeliveryStatus != null)
                btnUpdateDeliveryStatus.Click += BtnUpdateDeliveryStatus_Click;
            if (btnExportDeliveryExcel != null)
                btnExportDeliveryExcel.Click += BtnExportDeliveryExcel_Click;
            if (btnExportDeliveryPdf != null)
                btnExportDeliveryPdf.Click += BtnExportDeliveryPdf_Click;
            if (txtDeliverySearch != null)
                txtDeliverySearch.TextChanged += TxtDeliverySearch_TextChanged;
            if (cmbDeliveryStatusFilter != null)
                cmbDeliveryStatusFilter.SelectedIndexChanged += CmbDeliveryStatusFilter_SelectedIndexChanged;
            if (dtpDeliveryDateFilter != null)
                dtpDeliveryDateFilter.ValueChanged += DtpDeliveryDateFilter_ValueChanged;
            if (dgvAssignments != null)
            {
                dgvAssignments.CellDoubleClick += DgvAssignments_CellDoubleClick;
                dgvAssignments.SelectionChanged += DgvAssignments_SelectionChanged;
            }
        }

        /// <summary>
        /// Load và hiển thị DeliveryAssignment trong dgvAssignments
        /// </summary>
        private async Task LoadDeliveryAssignmentDataAsync()
        {
            try
            {
                if (dgvAssignments == null) return;

                var assignmentsResult = await AppServices.DeliveryService.GetAllAsync();
                _deliveryAssignments = assignmentsResult?.Data?.ToList() ?? new List<DeliveryAssignment>();

                // Debug: Kiểm tra dữ liệu từ service
                System.Diagnostics.Debug.WriteLine($"[LoadDeliveryAssignmentDataAsync] Total assignments from service: {_deliveryAssignments.Count}");

                // Lấy danh sách nhân viên có thể phân công giao hàng (tất cả trừ ADMIN)
                // Bao gồm: STAFF, MANAGER, DRIVER để map tên
                var allUsersResult = await AppServices.UserService.GetAllActiveUsersAsync();
                var driversList = allUsersResult?.Data?
                    .Where(u => u.Role != UserRole.ADMIN)
                    .ToList() ?? new List<User>();
                var driversDict = driversList.ToDictionary(d => d.UserId, d => d.Fullname ?? d.Username);

                // Debug: Kiểm tra drivers mapping
                System.Diagnostics.Debug.WriteLine($"[LoadDeliveryAssignmentDataAsync] Total drivers: {driversDict.Count}");

                // Lấy danh sách đơn hàng để map mã đơn - lấy tất cả đơn hàng
                var ordersResult = await AppServices.OrderService.GetAllAsync();
                var ordersList = ordersResult?.Data?.ToList() ?? new List<OrderDTO>();
                var ordersDict = ordersList.ToDictionary(o => o.OrderId, o => o.OrderCode ?? $"ORD-{o.OrderId:D5}");

                // Debug: Kiểm tra orders mapping
                System.Diagnostics.Debug.WriteLine($"[LoadDeliveryAssignmentDataAsync] Total orders: {ordersDict.Count}");

                UIHelper.SafeInvoke(this, () =>
                {
                    // Đảm bảo columns đã được khởi tạo
                    if (dgvAssignments.Columns.Count == 0)
                    {
                        InitializeDataGridColumns();
                    }

                    dgvAssignments.Rows.Clear();

                    // TẠM THỜI: Bypass filter để test - hiển thị TẤT CẢ assignments
                    var filteredAssignments = _deliveryAssignments; // ApplyDeliveryFilters(_deliveryAssignments, driversDict, ordersDict);

                    // Debug: Kiểm tra sau khi filter
                    System.Diagnostics.Debug.WriteLine($"[LoadDeliveryAssignmentDataAsync] Filtered assignments (bypassed): {filteredAssignments.Count}");
                    System.Diagnostics.Debug.WriteLine($"[LoadDeliveryAssignmentDataAsync] dgvAssignments.Columns.Count: {dgvAssignments.Columns.Count}");

                    // Debug: Kiểm tra mapping DriverId và OrderId
                    var unmappedDriverIds = filteredAssignments
                        .Where(a => !driversDict.ContainsKey(a.DriverId))
                        .Select(a => a.DriverId)
                        .Distinct()
                        .ToList();
                    var unmappedOrderIds = filteredAssignments
                        .Where(a => !ordersDict.ContainsKey(a.OrderId))
                        .Select(a => a.OrderId)
                        .Distinct()
                        .ToList();
                    if (unmappedDriverIds.Any())
                    {
                        System.Diagnostics.Debug.WriteLine($"[LoadDeliveryAssignmentDataAsync] Unmapped DriverIds: {string.Join(", ", unmappedDriverIds)}");
                    }
                    if (unmappedOrderIds.Any())
                    {
                        System.Diagnostics.Debug.WriteLine($"[LoadDeliveryAssignmentDataAsync] Unmapped OrderIds: {string.Join(", ", unmappedOrderIds)}");
                    }

                    int rowsAdded = 0;
                    int rowsFailed = 0;
                    foreach (var assignment in filteredAssignments.OrderByDescending(a => a.AssignedDate))
                    {
                        try
                        {
                            var driverName = driversDict.ContainsKey(assignment.DriverId) 
                                ? driversDict[assignment.DriverId] 
                                : $"User ID: {assignment.DriverId}";
                            var orderCode = ordersDict.ContainsKey(assignment.OrderId) 
                                ? ordersDict[assignment.OrderId] 
                                : $"ORD-{assignment.OrderId:D5}";

                            var rowIndex = dgvAssignments.Rows.Add(
                                assignment.AssignmentId,
                                orderCode,
                                driverName,
                                assignment.AssignedDate.ToString("dd/MM/yyyy HH:mm"),
                                GetDeliveryStatusDisplayName(assignment.Status),
                                assignment.CodAmount.ToString("N0"),
                                GetPaymentStatusDisplayName(assignment.PaymentStatus),
                                assignment.Notes ?? ""
                            );

                            dgvAssignments.Rows[rowIndex].Tag = assignment;
                            rowsAdded++;
                        }
                        catch (Exception ex)
                        {
                            rowsFailed++;
                            System.Diagnostics.Debug.WriteLine($"[LoadDeliveryAssignmentDataAsync] Error adding row for assignment {assignment.AssignmentId}: {ex.Message}");
                            System.Diagnostics.Debug.WriteLine($"[LoadDeliveryAssignmentDataAsync] Stack trace: {ex.StackTrace}");
                        }
                    }
                    
                    System.Diagnostics.Debug.WriteLine($"[LoadDeliveryAssignmentDataAsync] Rows added: {rowsAdded}, Failed: {rowsFailed}, Total in grid: {dgvAssignments.Rows.Count}");
                });
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "tải danh sách phân công giao hàng");
            }
        }

        /// <summary>
        /// Áp dụng filters cho delivery - VERSION ĐÃ SỬA
        /// </summary>
        private List<DeliveryAssignment> ApplyDeliveryFilters(
            List<DeliveryAssignment> assignments,
            Dictionary<int, string> driversDict,
            Dictionary<int, string> ordersDict)
        {
            var filtered = assignments.AsEnumerable();

            // Filter theo search
            if (!string.IsNullOrWhiteSpace(txtDeliverySearch?.Text))
            {
                var searchTerm = txtDeliverySearch.Text.ToLower();
                filtered = filtered.Where(a =>
                {
                    var driverName = driversDict.ContainsKey(a.DriverId) 
                        ? driversDict[a.DriverId].ToLower() 
                        : "";
                    var orderCode = ordersDict.ContainsKey(a.OrderId) 
                        ? ordersDict[a.OrderId].ToLower() 
                        : "";

                    return driverName.Contains(searchTerm) ||
                           orderCode.Contains(searchTerm) ||
                           (a.Notes ?? "").ToLower().Contains(searchTerm) ||
                           a.AssignmentId.ToString().Contains(searchTerm);
                });
            }

            // Filter theo status
            if (cmbDeliveryStatusFilter != null && cmbDeliveryStatusFilter.SelectedIndex > 0)
            {
                var selectedStatus = GetDeliveryStatusFromFilter(cmbDeliveryStatusFilter.SelectedIndex);
                filtered = filtered.Where(a => a.Status == selectedStatus);
            }

            // Filter theo ngày
            if (dtpDeliveryDateFilter != null && dtpDeliveryDateFilter.Checked)
            {
                var filterDate = dtpDeliveryDateFilter.Value.Date;
                filtered = filtered.Where(a => a.AssignedDate.Date == filterDate);
            }

            return filtered.ToList();
        }

        /// <summary>
        /// Lấy DeliveryStatus từ filter index
        /// </summary>
        private DeliveryStatus GetDeliveryStatusFromFilter(int index)
        {
            switch (index)
            {
                case 1: return DeliveryStatus.PENDING;
                case 2: return DeliveryStatus.INTRANSIT;
                case 3: return DeliveryStatus.DELIVERED;
                case 4: return DeliveryStatus.FAILED;
                default: return DeliveryStatus.PENDING;
            }
        }

        /// <summary>
        /// Lấy tên hiển thị của DeliveryStatus
        /// </summary>
        private string GetDeliveryStatusDisplayName(DeliveryStatus status)
        {
            switch (status)
            {
                case DeliveryStatus.PENDING:
                    return "Chờ giao";
                case DeliveryStatus.INTRANSIT:
                    return "Đang giao";
                case DeliveryStatus.DELIVERED:
                    return "Đã giao";
                case DeliveryStatus.FAILED:
                    return "Thất bại";
                default:
                    return status.ToString();
            }
        }

        /// <summary>
        /// Lấy tên hiển thị của PaymentStatus
        /// </summary>
        private string GetPaymentStatusDisplayName(DeliveryPaymentStatus status)
        {
            switch (status)
            {
                case DeliveryPaymentStatus.UNPAID:
                    return "Chưa thanh toán";
                case DeliveryPaymentStatus.PAID:
                    return "Đã thanh toán";
                default:
                    return status.ToString();
            }
        }

        #endregion

        #region Delivery Event Handlers

        private async void BtnAssignDelivery_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy danh sách đơn hàng chưa được phân công hoặc đang chờ
                var pendingOrdersResult = await AppServices.OrderService.GetProcessingOrdersAsync();
                var pendingOrders = pendingOrdersResult?.Data?.ToList() ?? new List<OrderDTO>();

                // Lấy danh sách nhân viên có thể phân công giao hàng (tất cả trừ ADMIN)
                // Bao gồm: STAFF, MANAGER, DRIVER - linh động phân công cho bất kỳ ai
                var allUsersResult = await AppServices.UserService.GetAllActiveUsersAsync();
                var drivers = allUsersResult?.Data?
                    .Where(u => u.Role != UserRole.ADMIN)
                    .ToList() ?? new List<User>();

                if (!drivers.Any())
                {
                    UIHelper.ShowWarningMessage("Không có nhân viên nào trong hệ thống để phân công.");
                    return;
                }

                // Convert Order to OrderDTO
                var orderDTOs = pendingOrders.Select(o => new OrderDTO
                {
                    OrderId = o.OrderId,
                    OrderCode = o.OrderCode ?? $"ORD-{o.OrderId:D5}",
                    CustomerName = o.CustomerName,
                    TotalAmount = o.TotalAmount
                }).ToList();

                if (!orderDTOs.Any())
                {
                    UIHelper.ShowWarningMessage("Không có đơn hàng nào đang chờ phân công.");
                    return;
                }

                // Luôn mở BatchDeliveryAssignmentForm (phân công hàng loạt)
                using (var form = new BatchDeliveryAssignmentForm(orderDTOs, drivers))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        await LoadDeliveryAssignmentDataAsync();
                        await LoadDashboardDataAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "mở form phân công giao hàng");
            }
        }

        private async void BtnUpdateDeliveryStatus_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAssignments.SelectedRows.Count == 0)
                {
                    UIHelper.ShowWarningMessage("Vui lòng chọn phân công giao hàng cần cập nhật.");
                    return;
                }

                var selectedRow = dgvAssignments.SelectedRows[0];
                var assignment = selectedRow.Tag as DeliveryAssignment;

                if (assignment != null)
                {
                    // Mở form cập nhật trạng thái
                    await ShowUpdateDeliveryStatusDialog(assignment);
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "cập nhật trạng thái giao hàng");
            }
        }

        /// <summary>
        /// Hiển thị dialog cập nhật trạng thái giao hàng - VERSION CẢI THIỆN
        /// </summary>
        private async Task ShowUpdateDeliveryStatusDialog(DeliveryAssignment assignment)
        {
            // Lấy thông tin đơn hàng để hiển thị
            var orderResult = await AppServices.OrderService.GetOrderByIdAsync(assignment.OrderId);
            var orderCode = orderResult?.Data?.OrderCode ?? $"ORD-{assignment.OrderId:D5}";

            var dialog = new Form
            {
                Text = "Cập nhật trạng thái giao hàng",
                Size = new Size(500, 450),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                Padding = new Padding(20)
            };

            var lblOrder = new Label
            {
                Text = $"Mã đơn: {orderCode}",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            var lblDriver = new Label
            {
                Text = $"Tài xế ID: {assignment.DriverId}",
                Location = new Point(20, 45),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F)
            };

            var lblStatus = new Label
            {
                Text = "Trạng thái:",
                Location = new Point(20, 80),
                AutoSize = true
            };

            var cmbStatus = new ComboBox
            {
                Location = new Point(20, 105),
                Size = new Size(440, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbStatus.Items.AddRange(new[] { "Chờ giao", "Đang giao", "Đã giao", "Thất bại" });
            
            // Map enum sang index đúng cách
            var statusIndex = GetStatusIndexFromEnum(assignment.Status);
            cmbStatus.SelectedIndex = statusIndex >= 0 ? statusIndex : 0;

            var lblPaymentStatus = new Label
            {
                Text = "Trạng thái thanh toán COD:",
                Location = new Point(20, 150),
                AutoSize = true
            };

            var cmbPaymentStatus = new ComboBox
            {
                Location = new Point(20, 175),
                Size = new Size(440, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbPaymentStatus.Items.AddRange(new[] { "Chưa thanh toán", "Đã thanh toán" });
            cmbPaymentStatus.SelectedIndex = (int)assignment.PaymentStatus;

            // Disable payment status nếu chưa giao thành công
            cmbStatus.SelectedIndexChanged += (s, e) =>
            {
                var selectedStatus = GetStatusFromIndex(cmbStatus.SelectedIndex);
                bool canPay = selectedStatus == DeliveryStatus.DELIVERED;
                cmbPaymentStatus.Enabled = canPay;
                if (!canPay && cmbPaymentStatus.SelectedIndex == (int)DeliveryPaymentStatus.PAID)
                {
                    cmbPaymentStatus.SelectedIndex = (int)DeliveryPaymentStatus.UNPAID;
                }
            };

            // Set initial state
            var currentStatus = GetStatusFromIndex(cmbStatus.SelectedIndex);
            cmbPaymentStatus.Enabled = currentStatus == DeliveryStatus.DELIVERED;

            var lblCodAmount = new Label
            {
                Text = "Số tiền COD:",
                Location = new Point(20, 220),
                AutoSize = true
            };

            var numCodAmount = new NumericUpDown
            {
                Location = new Point(20, 245),
                Size = new Size(440, 30),
                Maximum = 999999999,
                DecimalPlaces = 0,
                Value = assignment.CodAmount
            };

            var lblNotes = new Label
            {
                Text = "Ghi chú:",
                Location = new Point(20, 290),
                AutoSize = true
            };

            var txtNotes = new TextBox
            {
                Location = new Point(20, 315),
                Size = new Size(440, 60),
                Multiline = true,
                Text = assignment.Notes ?? ""
            };

            var btnSave = new Button
            {
                Text = "Lưu",
                Location = new Point(280, 390),
                Size = new Size(100, 35),
                DialogResult = DialogResult.OK
            };

            var btnCancel = new Button
            {
                Text = "Hủy",
                Location = new Point(390, 390),
                Size = new Size(100, 35),
                DialogResult = DialogResult.Cancel
            };

            dialog.Controls.AddRange(new Control[] { 
                lblOrder, lblDriver, lblStatus, cmbStatus, lblPaymentStatus, cmbPaymentStatus, 
                lblCodAmount, numCodAmount, lblNotes, txtNotes, btnSave, btnCancel 
            });

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var newStatus = GetStatusFromIndex(cmbStatus.SelectedIndex);
                    var newPaymentStatus = (DeliveryPaymentStatus)cmbPaymentStatus.SelectedIndex;
                    var newCodAmount = numCodAmount.Value;
                    var newNotes = txtNotes.Text?.Trim();

                    // Validation: Chỉ cho phép thanh toán khi đã giao thành công
                    if (newPaymentStatus == DeliveryPaymentStatus.PAID && newStatus != DeliveryStatus.DELIVERED)
                    {
                        UIHelper.ShowWarningMessage("Chỉ có thể đánh dấu đã thanh toán khi trạng thái là 'Đã giao'.");
                        return;
                    }

                    // Validation: COD amount phải > 0 nếu đã thanh toán
                    if (newPaymentStatus == DeliveryPaymentStatus.PAID && newCodAmount <= 0)
                    {
                        UIHelper.ShowWarningMessage("Số tiền COD phải lớn hơn 0 khi đã thanh toán.");
                        return;
                    }

                    bool hasChanges = false;

                    // Cập nhật trạng thái nếu thay đổi
                    if (newStatus != assignment.Status)
                    {
                        var statusResult = await AppServices.DeliveryService.UpdateStatusAsync(assignment.AssignmentId, newStatus);
                        if (!statusResult.Success)
                        {
                            UIHelper.ShowErrorMessage($"Lỗi cập nhật trạng thái: {statusResult.Message}");
                            return;
                        }
                        assignment.Status = newStatus;
                        hasChanges = true;

                        // Nếu đơn hàng đã được giao thành công, cập nhật trạng thái đơn hàng thành COMPLETED
                        if (newStatus == DeliveryStatus.DELIVERED)
                        {
                            try
                            {
                                var orderStatusResult = await AppServices.OrderService.UpdateOrderStatusAsync(assignment.OrderId, OrderStatus.COMPLETED);
                                if (orderStatusResult.Success)
                                {
                                    System.Diagnostics.Debug.WriteLine($"[UpdateDeliveryStatus] Đã cập nhật Order {assignment.OrderId} thành COMPLETED");
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine($"[UpdateDeliveryStatus] Lỗi cập nhật Order status: {orderStatusResult.Message}");
                                    // Không block việc cập nhật delivery status nếu order status update fail
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"[UpdateDeliveryStatus] Exception khi cập nhật Order status: {ex.Message}");
                                // Không block việc cập nhật delivery status nếu order status update fail
                            }
                        }
                        // Nếu đơn hàng giao thất bại, có thể cập nhật trạng thái đơn hàng về READY hoặc PROCESSING để có thể giao lại
                        else if (newStatus == DeliveryStatus.FAILED)
                        {
                            try
                            {
                                // Lấy thông tin đơn hàng hiện tại để kiểm tra
                                var failedOrderResult = await AppServices.OrderService.GetOrderByIdAsync(assignment.OrderId);
                                if (failedOrderResult?.Success == true && failedOrderResult.Data != null)
                                {
                                    var currentOrderStatus = failedOrderResult.Data.Status;
                                    // Chỉ cập nhật nếu đơn hàng đang ở trạng thái SHIPPED
                                    if (currentOrderStatus == OrderStatus.SHIPPED)
                                    {
                                        var orderStatusResult = await AppServices.OrderService.UpdateOrderStatusAsync(assignment.OrderId, OrderStatus.READY);
                                        if (orderStatusResult.Success)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"[UpdateDeliveryStatus] Đã cập nhật Order {assignment.OrderId} về READY do giao thất bại");
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"[UpdateDeliveryStatus] Exception khi xử lý Order status cho FAILED: {ex.Message}");
                            }
                        }
                    }

                    // Cập nhật trạng thái thanh toán nếu thay đổi
                    if (newPaymentStatus != assignment.PaymentStatus)
                    {
                        var paymentResult = await AppServices.DeliveryService.UpdatePaymentStatusAsync(assignment.AssignmentId, newPaymentStatus);
                        if (!paymentResult.Success)
                        {
                            UIHelper.ShowErrorMessage($"Lỗi cập nhật trạng thái thanh toán: {paymentResult.Message}");
                            return;
                        }
                        assignment.PaymentStatus = newPaymentStatus;
                        hasChanges = true;
                    }

                    // Cập nhật COD amount và Notes nếu thay đổi
                    if (newCodAmount != assignment.CodAmount || newNotes != (assignment.Notes ?? ""))
                    {
                        assignment.CodAmount = newCodAmount;
                        assignment.Notes = newNotes;
                        var updateResult = await AppServices.DeliveryService.UpdateAsync(assignment);
                        if (!updateResult.Success)
                        {
                            UIHelper.ShowErrorMessage($"Lỗi cập nhật thông tin: {updateResult.Message}");
                            return;
                        }
                        hasChanges = true;
                    }

                    if (hasChanges)
                    {
                        UIHelper.ShowSuccessMessage("Đã cập nhật trạng thái thành công!");
                        await LoadDeliveryAssignmentDataAsync();
                        await LoadDashboardDataAsync();
                    }
                    else
                    {
                        UIHelper.ShowWarningMessage("Không có thay đổi nào được thực hiện.");
                    }
                }
                catch (Exception ex)
                {
                    UIHelper.ShowExceptionError(ex, "cập nhật trạng thái giao hàng");
                }
            }
        }

        /// <summary>
        /// Lấy index từ enum DeliveryStatus
        /// </summary>
        private int GetStatusIndexFromEnum(DeliveryStatus status)
        {
            switch (status)
            {
                case DeliveryStatus.PENDING: return 0;
                case DeliveryStatus.INTRANSIT: return 1;
                case DeliveryStatus.DELIVERED: return 2;
                case DeliveryStatus.FAILED: return 3;
                default: return 0;
            }
        }

        /// <summary>
        /// Lấy enum DeliveryStatus từ index
        /// </summary>
        private DeliveryStatus GetStatusFromIndex(int index)
        {
            switch (index)
            {
                case 0: return DeliveryStatus.PENDING;
                case 1: return DeliveryStatus.INTRANSIT;
                case 2: return DeliveryStatus.DELIVERED;
                case 3: return DeliveryStatus.FAILED;
                default: return DeliveryStatus.PENDING;
            }
        }

        private async void BtnExportDeliveryExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAssignments == null || dgvAssignments.Rows.Count == 0)
                {
                    AppServices.Dialog.ShowWarning("Không có dữ liệu để xuất. Vui lòng tải dữ liệu trước.");
                    return;
                }

                var saveDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    FileName = $"DanhSachPhanCongGiaoHang_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                    Title = "Xuất danh sách phân công giao hàng ra Excel"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var dataTable = ConvertDataGridViewToDataTable(dgvAssignments);
                    
                    var headers = new Dictionary<string, string>
                    {
                        { "OrderCode", "Mã đơn" },
                        { "DriverName", "Tài xế" },
                        { "AssignedDate", "Ngày phân công" },
                        { "Status", "Trạng thái" },
                        { "CodAmount", "COD" },
                        { "PaymentStatus", "Thanh toán" },
                        { "Notes", "Ghi chú" }
                    };

                    var excelExporter = new ExcelExporter();
                    excelExporter.ExportToExcel(dataTable, saveDialog.FileName, "Phân công giao hàng", headers);
                    AppServices.Dialog.ShowSuccess($"Đã xuất Excel thành công!\nFile: {saveDialog.FileName}");
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "xuất Excel phân công giao hàng");
            }
        }

        private async void BtnExportDeliveryPdf_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAssignments == null || dgvAssignments.Rows.Count == 0)
                {
                    AppServices.Dialog.ShowWarning("Không có dữ liệu để xuất. Vui lòng tải dữ liệu trước.");
                    return;
                }

                var saveDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    FileName = $"DanhSachPhanCongGiaoHang_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
                    Title = "Xuất danh sách phân công giao hàng ra PDF"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var dataTable = ConvertDataGridViewToDataTable(dgvAssignments);
                    
                    var headers = new Dictionary<string, string>
                    {
                        { "OrderCode", "Mã đơn" },
                        { "DriverName", "Tài xế" },
                        { "AssignedDate", "Ngày phân công" },
                        { "Status", "Trạng thái" },
                        { "CodAmount", "COD" },
                        { "PaymentStatus", "Thanh toán" },
                        { "Notes", "Ghi chú" }
                    };

                    var pdfExporter = new PdfExporter();
                    pdfExporter.ExportToPdf(dataTable, saveDialog.FileName, "BÁO CÁO PHÂN CÔNG GIAO HÀNG", headers);
                    AppServices.Dialog.ShowSuccess($"Đã xuất PDF thành công!\nFile: {saveDialog.FileName}");
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "xuất PDF phân công giao hàng");
            }
        }

        private async void TxtDeliverySearch_TextChanged(object sender, EventArgs e)
        {
            await LoadDeliveryAssignmentDataAsync();
        }

        private async void CmbDeliveryStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            await LoadDeliveryAssignmentDataAsync();
        }

        private async void DtpDeliveryDateFilter_ValueChanged(object sender, EventArgs e)
        {
            await LoadDeliveryAssignmentDataAsync();
        }

        private void DgvAssignments_SelectionChanged(object sender, EventArgs e)
        {
            // Enable/disable update button dựa trên selection
            bool hasSelection = dgvAssignments.SelectedRows.Count > 0;
            if (btnUpdateDeliveryStatus != null)
                btnUpdateDeliveryStatus.Enabled = hasSelection;
        }

        private async void DgvAssignments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                var row = dgvAssignments.Rows[e.RowIndex];
                var assignment = row.Tag as DeliveryAssignment;

                if (assignment != null)
                {
                    await ShowUpdateDeliveryStatusDialog(assignment);
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "cập nhật trạng thái giao hàng");
            }
        }

        #endregion

        #region Dashboard Methods

        /// <summary>
        /// Load và cập nhật thống kê dashboard
        /// </summary>
        private async Task LoadDashboardDataAsync()
        {
            try
            {
                var today = DateTime.Today;

                // Đếm số ca làm hôm nay - lấy tất cả và filter theo ngày
                var allShiftsResult = await AppServices.WorkShiftService.GetAllAsync();
                var allShifts = allShiftsResult?.Data?.ToList() ?? new List<WorkShift>();
                var todayShiftsCount = allShifts.Count(s => s.ShiftDate.Date == today);

                // Đếm số đơn đã giao
                var deliveredResult = await AppServices.DeliveryService.GetByStatusAsync(DeliveryStatus.DELIVERED);
                var deliveredCount = deliveredResult?.Data?.Count() ?? 0;

                // Đếm số đơn trễ (có thể là đơn quá hạn hoặc failed)
                var failedResult = await AppServices.DeliveryService.GetByStatusAsync(DeliveryStatus.FAILED);
                var failedCount = failedResult?.Data?.Count() ?? 0;

                // Tính tổng COD
                var allDeliveriesResult = await AppServices.DeliveryService.GetAllAsync();
                var allDeliveries = allDeliveriesResult?.Data?.ToList() ?? new List<DeliveryAssignment>();
                var totalCOD = allDeliveries.Where(d => d.PaymentStatus == DeliveryPaymentStatus.PAID)
                    .Sum(d => d.CodAmount);

                UIHelper.SafeInvoke(this, () =>
                {
                    if (lblTodayShifts != null)
                        lblTodayShifts.Text = $"Số ca làm hôm nay: {todayShiftsCount}";
                    if (lblDeliveredOrders != null)
                        lblDeliveredOrders.Text = $"Số đơn đã giao: {deliveredCount}";
                    if (lblOverdueOrders != null)
                        lblOverdueOrders.Text = $"Số đơn trễ: {failedCount}";
                    if (lblTotalCOD != null)
                        lblTotalCOD.Text = $"COD: {totalCOD:N0} VNĐ";
                });
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "tải thống kê dashboard");
            }
        }

        #endregion

        #region Cleaning Schedule Row Class

        private class CleaningScheduleRow
        {
            public int ScheduleId { get; set; }
            public string CleaningType { get; set; }
            public string ScheduledDate { get; set; }
            public string CleanedBy { get; set; }
            public string CleanedDateTime { get; set; }
            public string Status { get; set; }
            public string Notes { get; set; }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Convert DataGridView to DataTable for export
        /// </summary>
        private DataTable ConvertDataGridViewToDataTable(DataGridView dgv)
        {
            var dataTable = new DataTable();

            if (dgv == null || dgv.Columns.Count == 0)
                return dataTable;

            // Add columns (skip hidden columns)
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                if (column.Visible)
                {
                    var columnType = column.ValueType ?? typeof(string);
                    dataTable.Columns.Add(column.Name, columnType);
                }
            }

            // Add rows
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;

                var dataRow = dataTable.NewRow();
                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    if (column.Visible)
                    {
                        var cellValue = row.Cells[column.Name].Value;
                        dataRow[column.Name] = cellValue ?? DBNull.Value;
                    }
                }
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }


        #endregion
    }
}
