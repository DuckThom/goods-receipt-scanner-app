using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Goederenontvangst
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            //Ini ini = new Ini("C:\\Program Files\\goederenontvangst\\Settings.ini");

            Application.Run(new MainMenuForm());
        }
    }
}