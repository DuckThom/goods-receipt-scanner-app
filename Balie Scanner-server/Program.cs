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
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            startApp();
        }

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow(); 

        static void startApp()
        {
            IntPtr zero = IntPtr.Zero;

            while (zero == IntPtr.Zero)
            {
                Thread.Sleep(100);
                zero = FindWindow(null, "Naamloos - Kladblok");
            }

            SetForegroundWindow(zero);

            for (int i = 0; i < 60; i+=0)
            { // I DID IT MOM! 

                Console.Out.WriteLine("Target: " + zero.ToString() + " Focus: " + GetForegroundWindow().ToString());

                if (zero == GetForegroundWindow())
                {
                    SendKeys.SendWait("{TAB}" + i + "{TAB} {ENTER}");
                    SendKeys.Flush();
                    i++;
                }
                
                Thread.Sleep(1000);
            }

            //Application.Run(new MainForm());
        }
    }
}