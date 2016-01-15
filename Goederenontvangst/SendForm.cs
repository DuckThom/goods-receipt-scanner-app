using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;

namespace Goederenontvangst
{
    public partial class SendForm : Form
    {
        TcpClient client;
        NetworkStream stream;
        List<ScannedProduct> productList;
        String serverIp;

        public SendForm(List<ScannedProduct> productList, String serverIp)
        {
            InitializeComponent();

            this.productList = productList;
            this.serverIp = serverIp;

            this.Load += new EventHandler(SendForm_Load);
        }

        public void SendForm_Load(object sender, EventArgs e)
        {
            this.Visible = true;

            Connect(this.serverIp, this.productList);
        }

        public bool Connect(String server, List<ScannedProduct> productList)
        {
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

                Byte[] data = System.Text.Encoding.ASCII.GetBytes("@HELLO@");

                stream.Write(data, 0, data.Length);

                // Check the response
                if (streamRead() != "@GOSERV@" + server + "@")
                {
                    return returnInFail("Verbonden met verkeerde server", true);
                }

                foreach (ScannedProduct product in productList)
                {
                    data = System.Text.Encoding.ASCII.GetBytes("@PROD@");
                    stream.Write(data, 0, data.Length);

                    System.Threading.Thread.Sleep(50);

                    data = System.Text.Encoding.ASCII.GetBytes(product.getProduct());
                    stream.Write(data, 0, data.Length);

                    System.Threading.Thread.Sleep(50);

                    data = System.Text.Encoding.ASCII.GetBytes("@COUNT@");
                    stream.Write(data, 0, data.Length);

                    System.Threading.Thread.Sleep(50);

                    data = System.Text.Encoding.ASCII.GetBytes(product.getProduct());
                    stream.Write(data, 0, data.Length);

                    System.Threading.Thread.Sleep(50);

                    data = System.Text.Encoding.ASCII.GetBytes("@ENDPROD@");
                    stream.Write(data, 0, data.Length);

                    if (streamRead() != "@RECV@")
                    {
                        return returnInFail("De server heeft een product niet goed ontvangen");
                    }
                }

                data = System.Text.Encoding.ASCII.GetBytes("@FINISH@");
                stream.Write(data, 0, data.Length);

                System.Threading.Thread.Sleep(50);

                if (streamRead() != "@GOT" + productList.Count + "@")
                {
                    return returnInFail("De server heeft een product niet goed ontvangen", true);
                }

                data = System.Text.Encoding.ASCII.GetBytes("@BYE@");
                stream.Write(data, 0, data.Length);

                System.Threading.Thread.Sleep(50);

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
            System.Threading.Thread.Sleep(1000);
            this.Close();

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

            this.productList = default(List<ScannedProduct>);

            setStatus("Overdracht gelukt!");
            System.Threading.Thread.Sleep(1000);
            this.Close();

            return true;
        }

        private void setStatus(String message)
        {
            this.statusLabel.Text = message;
        }
    }
}