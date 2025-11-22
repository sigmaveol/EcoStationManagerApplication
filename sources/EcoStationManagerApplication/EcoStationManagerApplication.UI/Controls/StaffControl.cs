using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.UI.Common;
using EcoStationManagerApplication.UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class StaffControl : UserControl, IRefreshableControl
    {
        private readonly BindingList<CleaningScheduleRow> _cleaningScheduleSource = new BindingList<CleaningScheduleRow>();
        private Dictionary<DateTime, List<CleaningSchedule>> _cleaningSchedulesByDate = new Dictionary<DateTime, List<CleaningSchedule>>();

        public StaffControl()
        {
            InitializeComponent();
            InitializeCleaningScheduleEvents();
        }

        public void RefreshData()
        {
            _ = LoadCleaningScheduleDataAsync();
        }

        private async void StaffControl_Load(object sender, EventArgs e)
        {
            await LoadCleaningScheduleDataAsync();
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
                        // Set ngày mặc định
                        form.SetDefaultDate(selectedDate);
                        
                        if (form.ShowDialog() == DialogResult.OK)
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

            dialog.ShowDialog();
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
                    if (form.ShowDialog() == DialogResult.OK)
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

            dialog.ShowDialog();
        }

        /// <summary>
        /// Hiển thị form để xem/chỉnh sửa/hoàn thành lịch vệ sinh
        /// </summary>
        private async Task ShowCleaningScheduleDialog(CleaningSchedule schedule)
        {
            using (var form = new CleaningScheduleForm(schedule))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    await LoadCleaningScheduleDataAsync();
                }
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
    }
}
