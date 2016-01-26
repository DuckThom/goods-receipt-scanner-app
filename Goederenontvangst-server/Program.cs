using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace Goederenontvangst_server
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string dbPath = Properties.Settings.Default.DBPath;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (dbPath == "unset")
            {
                dbPath = setDBPath();
            }

            if (File.Exists(dbPath))
            {
                startApp();
            }
            else
            {
                MessageBox.Show(
                    "Database file not found at: " + dbPath,
                    "Database path invalid",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                Application.Exit();
            }
        }

        static void startApp()
        {
            Application.Run(new MainForm());
        }

        static string setDBPath()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.Multiselect = false;
            dialog.Title = "Select the database file";
            dialog.Filter = "Database Files |*.accdb;*.mdb";
            dialog.FilterIndex = 0;

            DialogResult clickedOK = dialog.ShowDialog();

            if (clickedOK == DialogResult.OK)
            {
                Properties.Settings.Default.DBPath = dialog.FileName;
                Properties.Settings.Default.Save();

                return dialog.FileName;
            }
            else
            {
                Application.Exit();

                return "";
            }
        }
    }
}