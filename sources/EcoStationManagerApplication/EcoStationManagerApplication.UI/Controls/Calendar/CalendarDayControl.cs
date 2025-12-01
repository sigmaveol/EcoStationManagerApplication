using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class CalendarDayControl : UserControl
    {
        #region Events
        public event EventHandler DateClick;
        public event EventHandler DateDoubleClick;
        #endregion

        #region Properties
        public DateTime Date { get; set; }
        public bool IsCurrentMonth { get; set; }
        public bool IsToday { get; set; }
        public bool IsSelected { get; set; }
        public bool IsWeekend { get; set; }
        public Color? CustomBackgroundColor { get; set; }
        public List<CalendarEvent> Events { get; set; } = new List<CalendarEvent>();
        #endregion

        // Font sizes tính theo % của control height
        private float DayNumberFontSize => this.Height * 0.15f;
        private float EventFontSize => this.Height * 0.10f;
        private int EventHeight => (int)(this.Height * 0.15f);

        public CalendarDayControl()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Cursor = Cursors.Hand;
            // ĐÃ XÓA Size cố định - để control tự co giãn
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var g = e.Graphics;
            var clientRect = this.ClientRectangle;

            // Background
            var bgColor = GetBackgroundColor();
            using (var bgBrush = new SolidBrush(bgColor))
            {
                g.FillRectangle(bgBrush, clientRect);
            }

            // Border
            using (var borderPen = new Pen(Color.LightGray))
            {
                g.DrawRectangle(borderPen, 0, 0, clientRect.Width - 1, clientRect.Height - 1);
            }

            // Day number - font size responsive
            var dayColor = GetDayNumberColor();
            using (var dayBrush = new SolidBrush(dayColor))
            using (var dayFont = new Font("Segoe UI", DayNumberFontSize, GetDayNumberFontStyle()))
            {
                var dayFormat = new StringFormat
                {
                    Alignment = StringAlignment.Far,
                    LineAlignment = StringAlignment.Near
                };
                g.DrawString(Date.Day.ToString(), dayFont, dayBrush,
                    clientRect.Width - 5, 2, dayFormat);
            }

            // Events - responsive layout
            DrawEvents(g, clientRect);
        }

        private Color GetBackgroundColor()
        {
            if (CustomBackgroundColor.HasValue)
                return CustomBackgroundColor.Value;

            if (IsSelected)
                return Color.FromArgb(220, 245, 220);
            if (IsToday)
                return Color.FromArgb(255, 235, 156);
            if (!IsCurrentMonth)
                return Color.FromArgb(245, 245, 245);
            if (IsWeekend)
                return Color.FromArgb(255, 242, 242);

            return Color.White;
        }

        private Color GetDayNumberColor()
        {
            if (!IsCurrentMonth)
                return Color.Gray;
            if (IsWeekend)
                return Color.Red;
            if (IsSelected)
                return Color.FromArgb(46, 125, 50);

            return Color.Black;
        }

        private FontStyle GetDayNumberFontStyle()
        {
            if (IsSelected || IsToday)
                return FontStyle.Bold;
            if (!IsCurrentMonth)
                return FontStyle.Italic;

            return FontStyle.Regular;
        }

        private void DrawEvents(Graphics g, Rectangle clientRect)
        {
            if (Events == null || !Events.Any()) return;

            var startY = (int)(clientRect.Height * 0.25f); // Bắt đầu từ 25% chiều cao
            var eventRect = new Rectangle(3, startY, clientRect.Width - 6, EventHeight);

            for (int i = 0; i < Math.Min(Events.Count, GetMaxEventsToDisplay()); i++)
            {
                var calendarEvent = Events[i];
                using (var eventBrush = new SolidBrush(calendarEvent.EventColor))
                using (var eventFont = new Font("Segoe UI", EventFontSize))
                using (var textBrush = new SolidBrush(Color.White))
                {
                    // Vẽ background event
                    g.FillRectangle(eventBrush, eventRect);

                    // Vẽ text event
                    var textFormat = new StringFormat
                    {
                        Alignment = StringAlignment.Near,
                        LineAlignment = StringAlignment.Center,
                        Trimming = StringTrimming.EllipsisCharacter
                    };

                    g.DrawString(calendarEvent.Title, eventFont, textBrush,
                        eventRect, textFormat);

                    eventRect.Y += EventHeight + 2; // Khoảng cách giữa các event
                }
            }

            // Hiển thị "+ more" nếu có nhiều events
            if (Events.Count > GetMaxEventsToDisplay())
            {
                using (var moreFont = new Font("Segoe UI", EventFontSize))
                using (var moreBrush = new SolidBrush(Color.Gray))
                {
                    g.DrawString($"+ {Events.Count - GetMaxEventsToDisplay()} more",
                        moreFont, moreBrush, 3, eventRect.Y);
                }
            }
        }

        private int GetMaxEventsToDisplay()
        {
            // Số event tối đa hiển thị dựa trên chiều cao control
            var availableHeight = this.Height * 0.7f; // 70% chiều cao cho events
            var eventWithSpacing = EventHeight + 2;
            return Math.Max(1, (int)(availableHeight / eventWithSpacing));
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            DateClick?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            DateDoubleClick?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (IsCurrentMonth && !IsSelected)
            {
                this.CustomBackgroundColor = Color.FromArgb(230, 230, 230);
                this.Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (IsCurrentMonth && !IsSelected)
            {
                this.CustomBackgroundColor = null;
                this.Invalidate();
            }
        }

        // Responsive - tự động vẽ lại khi thay đổi kích thước
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Invalidate(); // Vẽ lại khi resize
        }
    }
}