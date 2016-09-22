using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections.Specialized;

namespace BalieScanner_server
{
    static class Program
    {
       
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IniFile settings = new IniFile(Application.StartupPath + "\\settings.ini");

            Application.Run(new MainForm(settings));
        }
    }
}