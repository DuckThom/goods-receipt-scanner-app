using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using Microsoft.Win32;

namespace Goederenontvangst
{
    public partial class SendForm : Form
    {
        TcpClient client;
        NetworkStream stream;
        List<ScannedProduct> productList;
        String serverIp;
        Thread t;

        public SendForm(List<ScannedProduct> productList)
        {
            InitializeComponent();

            this.productList = productList;
            this.serverIp = Registry.GetValue("HKEY_CURRENT_USER\\Goederenontvangst", "ServerIP", "(unset)").ToString();

            this.t = new Thread(startThread);
            this.t.Start();
        }

        private void startThread()
        {
            Connect(this.serverIp, this.productList);
        }

        public bool Connect(String server, List<ScannedProduct> productList)
        {
            this.productList = productList;

            try
            {
                setStatus("Verbinden...");

                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                Int32 port = 23207;
                this.client = new TcpClient(server, port);
                this.stream = client.GetStream();

                setStatus("Verbonden");
                Byte[] data = Encoding.ASCII.GetBytes("@HELLO@");

                stream.Write(data, 0, data.Length);

                // Check the response
                if (streamRead() != "@GOSERV@" + server + "@")
                {
                    return returnInFail("Verbonden met verkeerde server", true);
                }

                setStatus("Producten verzenden");
                foreach (ScannedProduct product in productList)
                {
                    if (product.getCount() != "0")
                    {
                        data = Encoding.ASCII.GetBytes("@PROD@");
                        stream.Write(data, 0, data.Length);

                        Thread.Sleep(200);

                        Byte[] prodData = new Byte[16];
                        prodData = Encoding.ASCII.GetBytes(product.getProduct().PadLeft(16, (char) 69));
                        stream.Write(prodData, 0, prodData.Length);

                        Thread.Sleep(200);

                        data = Encoding.ASCII.GetBytes("@COUNT@");
                        stream.Write(data, 0, data.Length);

                        Thread.Sleep(200);

                        Byte[] countData = new Byte[16];
                        countData = Encoding.ASCII.GetBytes(product.getCount().PadLeft(16, (char)69));
                        stream.Write(countData, 0, countData.Length);

                        Thread.Sleep(200);

                        data = Encoding.ASCII.GetBytes("@ENDPROD@");
                        stream.Write(data, 0, data.Length);

                        if (streamRead() != "@RECV@")
                        {
                            return returnInFail("De server heeft een product niet goed ontvangen");
                        }
                    }
                }

                setStatus("Voltooien");
                data = Encoding.ASCII.GetBytes("@FINISH@");
                stream.Write(data, 0, data.Length);

                Thread.Sleep(50);

                return returnInSuccess();
            }
            catch (ArgumentNullException e)
            {
                return returnInFail(e.Message);
            }
            catch (SocketException e)
            {
                return returnInFail(e.Message);
            }
            catch (IOException e)
            {
                return returnInFail(e.Message);
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
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

            return responseData;
        }

        private bool returnInFail(String message, bool close)
        {
            if (close)
            {
                this.stream.Close();
                this.client.Close();
            }

            setStatus("Overdracht mislukt... " + message);
            Thread.Sleep(5000);

            closeForm();

            return false;
        }

        private bool returnInFail(String message)
        {
            return returnInFail(message, false);
        }

        private bool returnInSuccess()
        {
            this.stream.Close();
            this.client.Close();

            // Reset the product list
            this.productList = null;
            this.productList = new List<ScannedProduct>();

            setStatus("Overdracht gelukt!");
            Thread.Sleep(2000);

            closeForm();

            return true;
        }

        public delegate void UpdateTextCallback(string text);

        private void setStatus(string text)
        {
            if (statusLabel.InvokeRequired)
            {
                statusLabel.Invoke(new UpdateTextCallback(this.setStatus), new object[]{ text });
            }
            else
            {
                statusLabel.Text = text;
            }
        }

        public delegate void CloseDelegate();

        private void closeForm()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new CloseDelegate(this.Close));
            }
            else
            {
                this.Close();
            }

            this.t.Abort();
        }
    }
}