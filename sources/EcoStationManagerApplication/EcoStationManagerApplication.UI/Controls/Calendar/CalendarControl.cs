using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class CalendarControl : UserControl
    {
        #region Events
        public event EventHandler<DateTime> DateSelected;
        public event EventHandler<DateTime> DateDoubleClick;
        public event EventHandler<CalendarDateEventArgs> DateRender;
        public event EventHandler<CalendarDateEventArgs> DateClick;
        #endregion

        #region Private Fields
        private DateTime currentDate;
        private DateTime selectedDate;
        private CalendarDayControl[,] dayControls;
        private Label[] dayHeaders;
        private bool isInitialized = false;
        private Dictionary<DateTime, List<CalendarEvent>> events = new Dictionary<DateTime, List<CalendarEvent>>();
        private DateTime[] currentMonthDays; // Cache các ngày trong tháng
        #endregion

        #region Properties
        public DateTime SelectedDate
        {
            get => selectedDate;
            set
            {
                selectedDate = value;
                currentDate = value;
                if (isInitialized) UpdateCalendar();
            }
        }

        public DateTime CurrentDate
        {
            get => currentDate;
            set
            {
                currentDate = value;
                if (isInitialized) UpdateCalendar();
            }
        }

        public Color PrimaryColor { get; set; } = Color.FromArgb(46, 125, 50);
        public Color HeaderBackground { get; set; } = Color.FromArgb(240, 240, 240);
        public Color WeekendColor { get; set; } = Color.FromArgb(255, 242, 242);
        public Color TodayColor { get; set; } = Color.FromArgb(255, 235, 156);
        public Color SelectedColor { get; set; } = Color.FromArgb(220, 245, 220);
        public Color OtherMonthColor { get; set; } = Color.FromArgb(245, 245, 245);
        #endregion

        public CalendarControl()
        {
            InitializeComponent();

            /// Tạo custom TableLayoutPanel có double buffering
            EnableDoubleBuffering(calendarGrid);
            EnableDoubleBuffering(headerPanel);

            currentDate = DateTime.Today;
            selectedDate = DateTime.Today;
            InitializeCalendar();
            isInitialized = true;
        }

        private void EnableDoubleBuffering(Control control)
        {
            // Sử dụng reflection để set DoubleBuffered property
            System.Reflection.PropertyInfo doubleBufferProperty =
                typeof(Control).GetProperty("DoubleBuffered",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance);

            doubleBufferProperty?.SetValue(control, true, null);
        }

        private void InitializeCalendar()
        {
            CreateDayHeaders();
            CreateDayControls();
            UpdateCalendarFast(); // Dùng method optimized
        }

        private void CreateDayHeaders()
        {
            headerPanel.Controls.Clear();

            dayHeaders = new Label[7];
            string[] days = { "CN", "T2", "T3", "T4", "T5", "T6", "T7" };

            for (int i = 0; i < 7; i++)
            {
                dayHeaders[i] = new Label();
                dayHeaders[i].Text = days[i];
                dayHeaders[i].TextAlign = ContentAlignment.MiddleCenter;
                dayHeaders[i].Dock = DockStyle.Fill;
                dayHeaders[i].Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                dayHeaders[i].ForeColor = Color.Black;
                dayHeaders[i].BackColor = HeaderBackground;
                dayHeaders[i].Margin = new Padding(0);

                if (i == 0 || i == 6)
                {
                    dayHeaders[i].ForeColor = Color.Red;
                }

                headerPanel.Controls.Add(dayHeaders[i], i, 0);
            }
        }

        private void CreateDayControls()
        {
            calendarGrid.Controls.Clear();

            dayControls = new CalendarDayControl[6, 7];

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    var dayControl = new CalendarDayControl();
                    dayControl.Dock = DockStyle.Fill;
                    dayControl.Margin = new Padding(1);

                    // Events
                    dayControl.DateClick += (s, e) => DayControl_DateClick(dayControl, e);
                    dayControl.DateDoubleClick += (s, e) => DayControl_DateDoubleClick(dayControl, e);

                    calendarGrid.Controls.Add(dayControl, col, row);
                    dayControls[row, col] = dayControl;
                }
            }
        }

        #region OPTIMIZED UPDATE METHODS
        private void UpdateCalendar()
        {
            UpdateCalendarFast(); // Chuyển sang method optimized
        }

        private void UpdateCalendarFast()
        {
            if (!isInitialized) return;

            // Tối ưu: chỉ suspend layout 1 lần
            this.SuspendLayout();
            calendarGrid.SuspendLayout();
            headerPanel.SuspendLayout();

            try
            {
                // Cache thông tin tháng hiện tại
                var firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
                var startDate = firstDayOfMonth.AddDays(-(int)firstDayOfMonth.DayOfWeek);
                var today = DateTime.Today;

                // Cập nhật tiêu đề
                lblMonthYear.Text = $"Tháng {currentDate.Month}/{currentDate.Year}";

                // Tối ưu: pre-calculate các ngày trong tháng
                PreCalculateMonthDays(startDate);

                // Batch update tất cả day controls
                UpdateAllDayControls(startDate, today);
            }
            finally
            {
                headerPanel.ResumeLayout();
                calendarGrid.ResumeLayout();
                this.ResumeLayout();

                // Chỉ refresh 1 lần cuối cùng
                this.Invalidate(true);
            }
        }

        private void PreCalculateMonthDays(DateTime startDate)
        {
            // Pre-calculate tất cả ngày để tránh tính toán lặp lại
            currentMonthDays = new DateTime[42]; // 6 rows * 7 columns
            for (int i = 0; i < 42; i++)
            {
                currentMonthDays[i] = startDate.AddDays(i);
            }
        }

        private void UpdateAllDayControls(DateTime startDate, DateTime today)
        {
            // Tối ưu: batch update thay vì lặp từng cell
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    int index = row * 7 + col;
                    DateTime currentDay = currentMonthDays[index];
                    var dayControl = dayControls[row, col];

                    UpdateSingleDayControl(dayControl, currentDay, col, today);
                }
            }
        }

        private void UpdateSingleDayControl(CalendarDayControl dayControl, DateTime currentDay, int col, DateTime today)
        {
            // Tối ưu: chỉ cập nhật nếu có thay đổi
            bool needsUpdate = dayControl.Date != currentDay ||
                              dayControl.IsCurrentMonth != (currentDay.Month == currentDate.Month) ||
                              dayControl.IsToday != (currentDay.Date == today.Date) ||
                              dayControl.IsSelected != (currentDay.Date == selectedDate.Date) ||
                              dayControl.IsWeekend != (col == 0 || col == 6);

            if (!needsUpdate) return;

            // Cập nhật properties
            dayControl.Date = currentDay;
            dayControl.IsCurrentMonth = currentDay.Month == currentDate.Month;
            dayControl.IsToday = currentDay.Date == today.Date;
            dayControl.IsSelected = currentDay.Date == selectedDate.Date;
            dayControl.IsWeekend = col == 0 || col == 6;

            // Tối ưu: chỉ get events nếu thực sự cần
            if (dayControl.IsCurrentMonth || dayControl.Events?.Count > 0)
            {
                var dayEvents = GetEventsForDate(currentDay);
                dayControl.Events = dayEvents;
            }
            else
            {
                dayControl.Events = new List<CalendarEvent>();
            }

            // Tối ưu: chỉ gọi DateRender cho ngày trong tháng hiện tại
            if (dayControl.IsCurrentMonth)
            {
                var args = new CalendarDateEventArgs(currentDay, dayControl);
                DateRender?.Invoke(this, args);
            }
        }
        #endregion

        #region Event Methods
        private void DayControl_DateClick(CalendarDayControl dayControl, EventArgs e)
        {
            if (dayControl.IsCurrentMonth)
            {
                selectedDate = dayControl.Date;

                // Tối ưu: chỉ update selection thay vì toàn bộ calendar
                UpdateSelectionOnly();

                DateSelected?.Invoke(this, selectedDate);

                var args = new CalendarDateEventArgs(dayControl.Date, dayControl);
                DateClick?.Invoke(this, args);
            }
        }

        private void UpdateSelectionOnly()
        {
            // Tối ưu: chỉ cập nhật trạng thái selected
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    var dayControl = dayControls[row, col];
                    bool wasSelected = dayControl.IsSelected;
                    bool shouldBeSelected = dayControl.Date.Date == selectedDate.Date;

                    if (wasSelected != shouldBeSelected)
                    {
                        dayControl.IsSelected = shouldBeSelected;
                        dayControl.Invalidate(); // Chỉ vẽ lại control thay đổi
                    }
                }
            }
        }

        private void DayControl_DateDoubleClick(CalendarDayControl dayControl, EventArgs e)
        {
            DateDoubleClick?.Invoke(this, dayControl.Date);
        }

        private void btnPreviousMonth_Click(object sender, EventArgs e)
        {
            currentDate = currentDate.AddMonths(-1);
            UpdateCalendarFast(); // Dùng method optimized
        }

        private void btnNextMonth_Click(object sender, EventArgs e)
        {
            currentDate = currentDate.AddMonths(1);
            UpdateCalendarFast(); // Dùng method optimized
        }
        #endregion

        #region Public Methods - Event Management (OPTIMIZED)
        public void AddEvent(DateTime date, CalendarEvent calendarEvent)
        {
            var dateKey = date.Date;
            if (!events.ContainsKey(dateKey))
            {
                events[dateKey] = new List<CalendarEvent>();
            }
            events[dateKey].Add(calendarEvent);

            // Tối ưu: chỉ update day control liên quan
            UpdateSingleDayForDate(dateKey);
        }

        public void RemoveEvent(DateTime date, string eventId)
        {
            var dateKey = date.Date;
            if (events.ContainsKey(dateKey))
            {
                events[dateKey].RemoveAll(e => e.Id == eventId);
                UpdateSingleDayForDate(dateKey);
            }
        }

        public void ClearEvents(DateTime date)
        {
            var dateKey = date.Date;
            if (events.ContainsKey(dateKey))
            {
                events[dateKey].Clear();
                UpdateSingleDayForDate(dateKey);
            }
        }

        public void ClearAllEvents()
        {
            events.Clear();
            UpdateCalendarFast();
        }

        private void UpdateSingleDayForDate(DateTime date)
        {
            // Tối ưu: chỉ update day control cho date cụ thể
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    var dayControl = dayControls[row, col];
                    if (dayControl.Date.Date == date.Date)
                    {
                        var dayEvents = GetEventsForDate(date);
                        dayControl.Events = dayEvents;
                        dayControl.Invalidate();
                        break;
                    }
                }
            }
        }

        public List<CalendarEvent> GetEventsForDate(DateTime date)
        {
            // Tối ưu: sử dụng TryGetValue để tránh double lookup
            return events.TryGetValue(date.Date, out var eventList)
                ? eventList
                : new List<CalendarEvent>();
        }

        public void HighlightDate(DateTime date, Color color)
        {
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    if (dayControls[row, col].Date.Date == date.Date)
                    {
                        dayControls[row, col].CustomBackgroundColor = color;
                        dayControls[row, col].Invalidate();
                        break;
                    }
                }
            }
        }

        public void ClearHighlights()
        {
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    if (dayControls[row, col].CustomBackgroundColor.HasValue)
                    {
                        dayControls[row, col].CustomBackgroundColor = null;
                        dayControls[row, col].Invalidate();
                    }
                }
            }
        }
        #endregion

        #region Overrides
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            // Cho phép resize bình thường
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            // Cho phép size changed bình thường
        }
        #endregion
    }

    #region Supporting Classes
    public class CalendarEvent
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }
        public string Description { get; set; }
        public Color EventColor { get; set; } = Color.Blue;
        public object Tag { get; set; }
    }

    public class CalendarDateEventArgs : EventArgs
    {
        public DateTime Date { get; }
        public CalendarDayControl DayControl { get; }

        public CalendarDateEventArgs(DateTime date, CalendarDayControl dayControl)
        {
            Date = date;
            DayControl = dayControl;
        }
    }
    #endregion
}