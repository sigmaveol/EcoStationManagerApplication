using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EcoStationManagerApplication.UI.Common;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class SearchControl : UserControl
    {
        public event EventHandler<string> SearchTextChanged;

        public SearchControl()
        {
            InitializeComponent();
        }

        public string PlaceholderText
        {
            get => this.guna2TextBoxSearch.PlaceholderText;
            set => this.guna2TextBoxSearch.PlaceholderText = value;
        }

        public string SearchText
        {
            get => this.guna2TextBoxSearch.Text;
            set => this.guna2TextBoxSearch.Text = value;
        }

        private void guna2TextBoxSearch_TextChanged(object sender, EventArgs e)
        {
            SearchTextChanged?.Invoke(this, this.guna2TextBoxSearch.Text);
        }

        private void guna2TextBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            Helper.HandleCtrlShortcuts(this.guna2TextBoxSearch, e);
        }
    }
}
