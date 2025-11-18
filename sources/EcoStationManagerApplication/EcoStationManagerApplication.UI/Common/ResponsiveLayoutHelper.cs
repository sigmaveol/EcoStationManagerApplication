using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Helpers
{
    public static class ResponsiveLayoutHelper
    {
        /// <summary>
        /// Thiết lập FlowLayoutPanel responsive - tự động wrap và điều chỉnh kích thước
        /// </summary>
        public static void SetupResponsiveFlowPanel(FlowLayoutPanel flowPanel,
            List<Control> childControls,
            Size defaultSize,
            Size minSize,
            int horizontalSpacing = 10,
            int verticalSpacing = 10)
        {
            if (flowPanel == null || childControls == null || !childControls.Any())
                return;

            // Thiết lập FlowLayoutPanel
            flowPanel.WrapContents = true;
            flowPanel.AutoSize = true;
            flowPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            // Thiết lập kích thước mặc định cho các control con
            foreach (var control in childControls)
            {
                control.Size = defaultSize;
                control.MinimumSize = minSize;
                control.MaximumSize = new Size(defaultSize.Width * 2, defaultSize.Height); // Giới hạn max
            }

            // Gắn sự kiện Resize
            flowPanel.Parent.Resize += (sender, e) =>
            {
                AdjustFlowPanelLayout(flowPanel, childControls, defaultSize, minSize, horizontalSpacing);
            };

            // Áp dụng ngay lần đầu
            AdjustFlowPanelLayout(flowPanel, childControls, defaultSize, minSize, horizontalSpacing);
        }

        /// <summary>
        /// Điều chỉnh layout của FlowLayoutPanel dựa trên kích thước container
        /// </summary>
        private static void AdjustFlowPanelLayout(FlowLayoutPanel flowPanel,
            List<Control> childControls,
            Size defaultSize,
            Size minSize,
            int horizontalSpacing)
        {
            if (flowPanel.Width <= 0) return;

            int containerWidth = flowPanel.Width;
            int totalDefaultWidth = (defaultSize.Width * childControls.Count) + (horizontalSpacing * (childControls.Count + 1));

            // Chỉ điều chỉnh khi container nhỏ hơn tổng kích thước mặc định
            if (containerWidth < totalDefaultWidth)
            {
                int optimalWidth = CalculateOptimalSize(containerWidth, childControls.Count, minSize.Width, horizontalSpacing);
                ApplyControlSizes(childControls, optimalWidth, defaultSize.Height);
            }
            else
            {
                // Reset về kích thước mặc định
                ApplyControlSizes(childControls, defaultSize.Width, defaultSize.Height);
            }
        }

        /// <summary>
        /// Tính toán kích thước tối ưu cho các control
        /// </summary>
        private static int CalculateOptimalSize(int containerWidth, int controlCount, int minWidth, int spacing)
        {
            // Tính số control trên mỗi hàng (tối thiểu 1)
            int maxControlsPerRow = Math.Max(1, (containerWidth - spacing) / (minWidth + spacing));

            // Tính kích thước tối ưu cho mỗi control
            int optimalWidth = (containerWidth - (spacing * (maxControlsPerRow + 1))) / maxControlsPerRow;

            return Math.Max(minWidth, optimalWidth);
        }

        /// <summary>
        /// Áp dụng kích thước mới cho tất cả controls
        /// </summary>
        private static void ApplyControlSizes(List<Control> controls, int width, int height)
        {
            foreach (var control in controls)
            {
                if (control.InvokeRequired)
                {
                    control.Invoke(new Action(() => control.Size = new Size(width, height)));
                }
                else
                {
                    control.Size = new Size(width, height);
                }
            }
        }

        /// <summary>
        /// Thiết lập TableLayoutPanel responsive với column percentage
        /// </summary>
        public static void SetupResponsiveTablePanel(TableLayoutPanel tablePanel, int columnCount)
        {
            if (tablePanel == null) return;

            tablePanel.ColumnCount = columnCount;
            tablePanel.RowCount = 1;

            // Thiết lập tất cả columns với cùng tỷ lệ phần trăm
            float percentage = 100f / columnCount;
            for (int i = 0; i < columnCount; i++)
            {
                tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, percentage));
            }

            tablePanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        }

        /// <summary>
        /// Thiết lập control tự động co giãn theo tỷ lệ phần trăm của parent
        /// </summary>
        public static void SetupPercentageSizing(Control control, float widthPercentage = 100f, float heightPercentage = 100f)
        {
            if (control.Parent == null) return;

            control.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            control.Dock = DockStyle.None;

            control.Parent.Resize += (sender, e) =>
            {
                int newWidth = (int)(control.Parent.Width * widthPercentage / 100f);
                int newHeight = (int)(control.Parent.Height * heightPercentage / 100f);

                control.Size = new Size(newWidth, newHeight);
                control.Location = new Point(
                    (control.Parent.Width - newWidth) / 2,
                    (control.Parent.Height - newHeight) / 2
                );
            };

            // Áp dụng ngay lần đầu
            control.Size = new Size(
                (int)(control.Parent.Width * widthPercentage / 100f),
                (int)(control.Parent.Height * heightPercentage / 100f)
            );
        }

    }
}