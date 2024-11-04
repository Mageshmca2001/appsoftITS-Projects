using AVG.ProductionProcess.FunctionalTest.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AVG.ProductionProcess.FunctionalTest
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Application.Run(new Forms.Form());
            //Application.Run(new MainForm());
            // Application.Run(new FunctionalForm());
            Application.Run(new LoginFrom());
        }
    }
}
