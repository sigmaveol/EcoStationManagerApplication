using EcoStationManagerApplication.UI.Forms;
using System;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new MainForm());


        }
    }
}
