using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class Form1 : Form
    {
        TcpListener tcpL;
        int port = 8000;
        IPEndPoint ipep;
        Socket server;
        bool isConnected = false;
        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            client();
            textBox2.AppendText("已連線\r\n");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tcpL.Data(textBox1.Text);

            if (textBox1.Text.Equals("esc"))
            {
                MessageBox.Show("連線已關閉");
                isConnected = false;
                server.Close();
            }
        }

        private void client()
        {
            while (true)
                try
                {
                    ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
                    server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    MessageBox.Show(port.ToString());
                    server.Connect(ipep);
                    isConnected = true;
                    tcpL = new TcpListener(server);
                    break;
                }
                catch (SocketException e)
                {
                    port++;
                }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                tcpL.Data("esc"); 
                server.Close();
            }

            //server.Shutdown(SocketShutdown.Both);

            if(MessageBox.Show("是否關閉程式", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                Application.Exit();
            //Environment.Exit(Environment.ExitCode);
        }

    }

    public class TcpListener
    {
        Socket socket;
        Thread outThread;
        NetworkStream stream;
        StreamWriter writer;
        string data = "";
        static string text = "0";
        bool input = false;


        public TcpListener(Socket s)
        {
            socket = s;
            stream = new NetworkStream(s);
            writer = new StreamWriter(stream);
            outThread = new Thread(new ThreadStart(run));
            outThread.Start();
        }

        public void run()
        {
            while (true)
            {

                if (input)
                {
                    data = text;
                    writer.WriteLine(data);
                    writer.Flush();
                    input = false;
                    Thread.Sleep(10);
                }
            }
        }

        public void Data(string stream)
        {
            text = stream;
            input = true;
        }
    }
}
