using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace EcoStationManagerApplication.UI.Common
{
    /// <summary>
    /// Helper class để hiển thị form modal với hiệu ứng làm mờ MainForm
    /// </summary>
    public static class FormHelper
    {
        /// <summary>
        /// Hiển thị form modal với hiệu ứng làm mờ form cha
        /// </summary>
        /// <param name="parentForm">Form cha (MainForm)</param>
        /// <param name="childForm">Form con cần hiển thị</param>
        /// <returns>DialogResult của form con</returns>
        public static DialogResult ShowModalWithDim(Form parentForm, Form childForm)
        {
            if (parentForm == null || childForm == null)
                return childForm.ShowDialog();

            // Tạo một form trung gian để làm overlay
            var overlayForm = new Form()
            {
                FormBorderStyle = FormBorderStyle.None,
                BackColor = Color.Black,
                Opacity = 0.5,
                ShowInTaskbar = false,
                StartPosition = FormStartPosition.Manual,
                Size = parentForm.ClientSize,
                Location = parentForm.PointToScreen(Point.Empty),
                Owner = parentForm
            };

            try
            {
                overlayForm.Show();

                // Hiển thị form con
                childForm.StartPosition = FormStartPosition.CenterParent;
                var result = childForm.ShowDialog(overlayForm);
                return result;
            }
            finally
            {
                overlayForm.Close();
                overlayForm.Dispose();
            }
        }
    }
}

