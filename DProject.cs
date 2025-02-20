using System;
using System.Windows.Forms;

namespace DProject
{
    static class DProject
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DProjectForm240905());
        }
    }
}