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
using datalogic.wireless;
using System.Net;

namespace Goederenontvangst
{
    public partial class SendForm : Form
    {
        Socket client;
        //NetworkStream stream;
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

        private bool Connect(String server, List<ScannedProduct> productList)
        {
            this.productList = productList;

            if (!this.radioSignal1.IsAssociated())
            {
                returnInFail("Niet verbonden met een access point.");
            }

            try
            {
                setStatus("Verbinden...");

                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                Int32 port = 23207;
                IPAddress ipAddr = IPAddress.Parse(this.serverIp);
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

                this.client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote end point.
                this.client.Connect(ipEndPoint);
                
                Byte[] data = Encoding.ASCII.GetBytes("@HELLO@");
                this.client.Send(data, data.Length, SocketFlags.None);

                // Check the response
                if (streamRead(32) != "@GOSERV@" + server + "@")
                {
                    return returnInFail("Verbonden met verkeerde server", true);
                }
                else
                {
                    setStatus("Verbonden");
                }

                setStatus("Producten verzenden");
                foreach (ScannedProduct product in productList)
                {
                    if (product.getCount() != "0")
                    {
                        streamWrite("@PROD@");

                        Thread.Sleep(100);

                        streamWrite(product.getProduct());

                        Thread.Sleep(100);

                        streamWrite("@COUNT@");

                        Thread.Sleep(100);

                        streamWrite(product.getCount());

                        Thread.Sleep(100);

                        streamWrite("@ENDPROD@");

                        if (streamRead() != "@RECV@")
                        {
                            return returnInFail("De server heeft een product niet goed ontvangen");
                        }
                    }
                }

                setStatus("Voltooien");

                streamWrite("@FINISH@");

                Thread.Sleep(50);

                return returnInSuccess();
            }
            catch (ArgumentNullException e)
            {
                return returnInFail("ArgumentNullException: " + e.Message, true);
            }
            catch (SocketException)
            {
                return returnInFail("SocketException: Fout in de verbinding met de server");
            }
            catch (IOException e)
            {
                return returnInFail("IOException: " + e.Message, true);
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
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

            return responseData.Replace("$", "");
        }

        private String streamRead()
        {
            return streamRead(16);
        }

        private bool returnInFail(String message, bool close)
        {
            if (close)
            {
                this.client.Shutdown(SocketShutdown.Both);
                this.client.Close();
            }

            setStatus("Overdracht mislukt... " + message);
            Thread.Sleep(5000);

            closeForm(false);

            return false;
        }

        private bool returnInFail(String message)
        {
            return returnInFail(message, false);
        }

        private bool returnInSuccess()
        {
            this.client.Shutdown(SocketShutdown.Both);
            this.client.Close();

            string path = "\\Backup\\goederenontvangst\\scannerdata.txt";

            if (File.Exists(path))
                File.Delete(path);

            setStatus("Overdracht gelukt!");
            Thread.Sleep(2000);

            closeForm(true);

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

        private void closeForm(bool reset)
        {
            if (reset)
            {
                // Reset the product list
                this.productList.Clear();
                this.productList.TrimExcess();
            }

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

        /**
         * Terminate a current socket session
         * This will probably cause issues, only use it if the transmission is frozen
         */
        private void terminateButton_Click(object sender, EventArgs e)
        {
            setStatus("Stopping transmission, this WILL cause issues");
            this.Update();
            Thread.Sleep(500);
            this.t.Abort();
            Thread.Sleep(1000);
            setStatus("Transmission terminated, app and server restart recommended");
            this.Update();
            Thread.Sleep(2000);
            this.Close();
        }
    }
}