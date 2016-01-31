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

namespace Goederenontvangst_server
{
    public partial class MainForm : Form
    {
        Thread t;
        TcpListener server;
        List<Product> productList;
        NetworkStream stream;
        Product product;

        int _totalPrinted = 0;
        int _totalToPrint = 0;

        string _dbPath = Properties.Settings.Default.DBPath;
        string ipAddress;

        public delegate void UpdateTextCallback(string text);

        public MainForm()
        {
            InitializeComponent();

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

            this.Focus();
        }

        private void startServer()
        {
            try
            {
                // Set the TcpListener on port WTG (23 20 7).
                Int32 port = 23207;
                IPAddress localAddr = IPAddress.Parse("0.0.0.0");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                server.ExclusiveAddressUse = true;

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] data = new Byte[256];

                // Enter the listening loop.
                while (true)
                {
                    setStatus("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                 
                    setStatus("Connected!");

                    // Get a stream object for reading and writing
                    stream = client.GetStream();

                    if (streamRead() == "@HELLO@")
                    {
                        // Send HELLO response
                        data = Encoding.ASCII.GetBytes("@GOSERV@" + this.ipAddress + "@");
                        stream.Write(data, 0, data.Length);

                        string input = streamRead();
                        this.productList = new List<Product>();

                        while (input != "@FINISH@")
                        {
                            if (input == "@PROD@")
                            {
                                this.product = new Product();

                                this.product.setProduct(streamReadFixed());
                            }
                            else if (input == "@COUNT@")
                            {
                                this.product.setCount(streamReadFixed());
                            }
                            else if (input == "@ENDPROD@")
                            {
                                productList.Add(this.product);

                                data = Encoding.ASCII.GetBytes("@RECV@");
                                stream.Write(data, 0, data.Length);

                                this.product = null;
                            }

                            input = streamRead();
                        }

                        UpdateProductsFromDatabase();

                        Thread.Sleep(1000);
                    }
                }
            }
            catch (SocketException e)
            {
                setStatus("Er is een fout opgetreden...");
            }
        }

        private String streamRead()
        {
            // Buffer to store the response bytes.
            Byte[] data = new Byte[256];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = this.stream.Read(data, 0, data.Length);
            responseData = Encoding.ASCII.GetString(data, 0, bytes);

            Console.WriteLine(responseData);

            return responseData;
        }

        private String streamReadFixed()
        {
            // Buffer to store the response bytes.
            Byte[] data = new Byte[16];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = this.stream.Read(data, 0, data.Length);
            responseData = Encoding.ASCII.GetString(data, 0, bytes);

            Console.WriteLine(responseData);

            return responseData.Replace("E", "");
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

        private void UpdateProductsFromDatabase()
        {
            foreach (Product product in this.productList)
            {
                if (product != null)
                {
                    // Create a new connection instance
                    OleDbConnection dbConnection = new OleDbConnection();

                    // Set the provider and data source
                    dbConnection.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + _dbPath.Replace("/", "\\") + ";Persist Security Info=False;";

                    dbConnection.Open();

                    DataSet dataSet = new DataSet();

                    OleDbDataAdapter myAdapter = new OleDbDataAdapter();

                    OleDbCommand command = new OleDbCommand("SELECT * FROM tblArtikelen WHERE Omnivers_nummer = '" + product.getProduct() + "'", dbConnection);

                    myAdapter.SelectCommand = command;
                    myAdapter.Fill(dataSet, "tblArtikelen");

                    if (dataSet != null)
                    {
                        DataRowCollection rowCollection = dataSet.Tables["tblArtikelen"].Rows;

                        foreach (DataRow row in rowCollection)
                        {
                            // Update the location and name in the product object
                            product.setLocation(row["Locatie"].ToString());
                            product.setName(row["Omschrijving1"].ToString());
                            product.setEAN(row["extArtikelcode"].ToString());
                        }
                    }

                    dbConnection.Close();
                }
            }
            // Print the data
            PrintData();
        }

        private void PrintData()
        {
            Console.WriteLine("Printing data");
            setStatus("Printing data");

            foreach (Product product in this.productList)
            {
                if (product.getCount() != null && product.getCount() != "0")
                {
                    _totalToPrint++;
                }
            }

            PrintDocument printDocument = new PrintDocument();
            PaperSize paperSize = new PaperSize();

            paperSize.RawKind = (int)PaperKind.A4;

            printDocument.DefaultPageSettings.Landscape = true;
            printDocument.DefaultPageSettings.PaperSize = paperSize;
            printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);

            printDocument.Print();

            // Sleep for 2 seconds
            Thread.Sleep(2000);

            setStatus("Done!");

            _totalPrinted = 0;
            _totalToPrint = 0;
        }

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics graphic = e.Graphics;

            Font titleFont = new Font("Courier New", 12, FontStyle.Bold);
            Font mainFont = new Font("Courier New", 12);

            SolidBrush brush = new SolidBrush(Color.Black);

            float titleHeight = titleFont.GetHeight();
            float mainHeight = mainFont.GetHeight();

            int startX = 10;
            int startY = 10;
            int offset = 40;
            int count = 0;

            string checkbox = "\u25A1".PadRight(10);
            string header = "Aantal".PadRight(10) + "Controle".PadRight(10) + "Artikel Nr.".PadRight(15) + "Omschrijving".PadRight(45) + "Locatie".PadRight(10) + "EAN".PadRight(16);

            graphic.DrawString(header, titleFont, brush, startX, startY);

            graphic.DrawLine(new Pen(brush), new Point(0, startY + offset), new Point(e.PageBounds.Width, startY + offset));

            offset = offset + (int)titleHeight + 10;

            foreach (Product product in this.productList)
            {
                if (product.getCount() != null && product.getPrint() && product.getCount() != "0")
                {
                    string qty = product.getCount().PadRight(10);
                    string number = product.getProduct().PadRight(15);
                    string name = product.getName().PadRight(45);
                    string location = product.getLocation().PadRight(10);
                    string ean = product.getEAN().PadRight(16);

                    string productLine = qty + checkbox + number + name + location + ean;

                    graphic.DrawString(productLine, mainFont, brush, startX, startY + offset);

                    offset = offset + (int)mainHeight + 5;

                    product.setPrint(false);

                    count++;
                }
            }

            _totalPrinted = _totalPrinted + count;

            if (_totalPrinted < _totalToPrint)
            {
                e.HasMorePages = true;
            }
        }
    }
}