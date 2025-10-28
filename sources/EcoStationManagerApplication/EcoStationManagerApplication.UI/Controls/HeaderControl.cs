using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class HeaderControl : UserControl
    {
        public UserControl SearchTextBox
        {
            get => this.searchControl;
        }

        public string SearchText
        {
            get { return this.searchControl.SearchText; }
            set { this.searchControl.SearchText = value; }
        }

        public string SearchPlaceHoder
        {
            get { return this.searchControl.PlaceholderText; }
            set { this.searchControl.PlaceholderText = value; }
        }

        public HeaderControl()
        {
            InitializeComponent();
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("vi-VN");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("vi-VN");
            guna2DateTimePicker1.CustomFormat = "dddd, dd/MM/yyyy";
        }
    }
}
