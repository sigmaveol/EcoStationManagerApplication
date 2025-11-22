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
    public partial class ContentControl : UserControl
    {
        public ContentControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Panel container bên trong để chứa các UserControl
        /// </summary>
        public Guna2Panel ContentPanel => guna2PanelContent;
    }
}
