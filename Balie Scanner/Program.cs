using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace BalieScanner
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            // Check if the file exists before trying to repopulate
            if (File.Exists("\\Backup\\balie_scanner\\productenlijst.txt"))
            {
                List<string> knownProductList = new List<string>();

                string line;
                string directory = "\\Backup\\balie_scanner";
                StreamReader file = new StreamReader(directory + "\\productenlijst.txt");

                while ((line = file.ReadLine()) != null)
                {
                    knownProductList.Add(line);
                }

                file.Close();

                Application.Run(new MainMenuForm(knownProductList));
            }
            else 
            {
                MessageBox.Show("Geen producten lijst gevonden: \\Backup\\balie_scanner\\productenlijst.txt", "Error",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Hand,
                                 MessageBoxDefaultButton.Button1);
            }
        }
    }
}