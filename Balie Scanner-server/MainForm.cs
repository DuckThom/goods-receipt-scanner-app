using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Drawing.Printing;
using System.Data.OleDb;
using System.IO;
using System.Runtime.InteropServices;

namespace BalieScanner_server
{
    public partial class MainForm : Form
    {
        Thread t;
        TcpListener server;
        List<string[]> productList = new List<string[]>();
        Socket client;
        IniFile settings;
        string[] product;

        string _dataPath = Application.StartupPath + "\\Data\\";
        string _dbPath = Properties.Settings.Default.DBPath;
        string ipAddress;

        bool _continue = true;

        public delegate void UpdateTextCallback(string text);

        public MainForm(IniFile settings)
        {
            InitializeComponent();

            this.settings = settings;

            if (!Directory.Exists(this._dataPath))
                Directory.CreateDirectory(this._dataPath);

            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress addr in localIPs)
            {
                if (addr.AddressFamily == AddressFamily.InterNetwork)
                {
                    this.ipLabel.Text = "IP: " + addr.ToString();
                    this.ipAddress = addr.ToString();
                }
            }

            t = new Thread(startServer);
            t.IsBackground = true;
            t.Start();
        }

        private void startServer()
        {
            try
            {
                // Set the TcpListener on port NOE (14 15 5).
                Int32 port = 14155;
                IPAddress localAddr = IPAddress.Parse("0.0.0.0");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                server.ExclusiveAddressUse = true;

                // Start listening for client requests.
                server.Start();

                // Enter the listening loop.
                while (true)
                {
                    setStatus("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    client = server.AcceptSocket();
                 
                    setStatus("Connected!");

                    if (streamRead() == "@HELLO@")
                    {
                        // Send Magazijn server HELLO response
                        streamWrite("@MASERV@" + this.ipAddress + "@", 32);

                        string input = streamRead();
                        this.productList = new List<string[]>();

                        while (input != "@FINISH@")
                        {
                            if (input == "@PROD@")
                            {
                                this.product = new string[2];

                                string product_number = streamRead();

                                this.product[0] = product_number;

                                setStatus("Ontvangen van product: " + product_number);
                            }
                            else if (input == "@COUNT@")
                            {
                                this.product[1] = streamRead();
                            }
                            else if (input == "@ENDPROD@")
                            {
                                productList.Add(this.product);

                                streamWrite("@RECV@");

                                this.product = null;
                            }
                            else
                            {
                                streamWrite("@FAILED@");

                                this._continue = false;

                                break;
                            }

                            input = streamRead();
                        }

                        if (this._continue)
                        {
                            // Save the list to a file to make sure that it's safe if anything goes wrong
                            saveToFile(this.productList);
                            // Update the received product list with data from the database
                            processList(this.productList);
                        }
                        else
                        {
                            setStatus("Received incorrect data");
                        }

                        Thread.Sleep(1000);
                    }
                }
            }
            catch (SocketException e)
            {
                setStatus("Er is een fout opgetreden...");

                MessageBox.Show(e.Message, "SocketException",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);

                this.t.Abort();
                this.Close();
            }
        }

        private void streamWrite(String message, Int16 size)
        {
            Byte[] data = new Byte[size];

            data = Encoding.ASCII.GetBytes(message.PadLeft(size, (char)36));

            this.client.Send(data, data.Length, SocketFlags.None);
        }

        private void streamWrite(String message)
        {
            streamWrite(message, 16);
        }

        private String streamRead(Int16 size)
        {
            // Buffer to store the response bytes.
            Byte[] data = new Byte[size];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = this.client.Receive(data, data.Length, SocketFlags.None);
            responseData = Encoding.ASCII.GetString(data, 0, bytes);

            Console.WriteLine(responseData);

            return responseData.Replace("$", "");
        }

        private String streamRead()
        {
            return streamRead(16);
        }

        private void stopServer()
        {
            server.Stop();
            t.Abort();
        }

        private void setStatus(string text)
        {
            if (statusLabel.InvokeRequired)
            {
                statusLabel.Invoke(new UpdateTextCallback(this.setStatus), new object[] { text });
            }
            else
            {
                statusLabel.Text = text;
            }
        }

        private void saveToFile(List<string[]> list)
        {
            string date = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
            string filePath = this._dataPath + "scannerdata_" + date + ".txt";
            StreamWriter file = new StreamWriter(filePath);

            foreach (string[] product in list)
            {
                // Write product,count to a file
                file.WriteLine(product[0] + "," + product[1]);
            }

            file.Close();
        }

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        private void processList(List<string[]> list)
        {
            IntPtr zero = IntPtr.Zero;
            string[][] arrList = list.ToArray();
            string windowName = this.settings.IniReadValue("Default", "WindowName");
            string keys = this.settings.IniReadValue("Default", "Keys");
            int keyDelay = Int32.Parse(this.settings.IniReadValue("Default", "KeyDelay"));
            int saveDelay = Int32.Parse(this.settings.IniReadValue("Default", "SaveDelay"));

            char[] delimiter = { ',' };

            while (zero == IntPtr.Zero)
            {
                Console.Out.WriteLine("Looking for window: " + windowName);
                Thread.Sleep(100);
                zero = FindWindow(null, windowName);
            }

            SetForegroundWindow(zero);

            // i+=0 is needed to keep the program in a loop until the correct window is in focus again
            for (int i = 0; i < arrList.Length; i+=0)
            { 
                if (zero == GetForegroundWindow())
                {
                    foreach(string key in keys.Split(delimiter))
                    {
                        if (key == "{SPACE}")
                            SendKeys.SendWait(" ");
                        else if (key == "{ARTNR}")
                            SendKeys.SendWait(list[i][0]);
                        else if (key == "{AANTAL}")
                            SendKeys.SendWait(list[i][1]);
                        else
                            SendKeys.SendWait(key);

                        Thread.Sleep(keyDelay);
                    }
                    
                    SendKeys.Flush();
                    
                    i = i + 1;
                }

                Thread.Sleep(saveDelay);
            }
        }

        private void retryButton_Click(object sender, EventArgs e)
        {
            if (this.productList.Count > 0)
            {
                setStatus("Opnieuw uitvoeren");
            }
            else
            {
                MessageBox.Show("De producten lijst is leeg.", "Geen producten",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Warning);
            }
        }
    }
}