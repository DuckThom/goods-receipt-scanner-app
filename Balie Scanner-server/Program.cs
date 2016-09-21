using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace BalieScanner_server
{
    static class Program
    {
        public static IniFile settings = new IniFile(Application.StartupPath + "\\settings.ini");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            startApp();
        }

        static void startApp()
        {
            Application.Run(new MainForm(settings));
        }
    }
}