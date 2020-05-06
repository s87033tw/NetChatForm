using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 8000;
            int user = 0;
            Server server = new Server();
            while(true){
                server.run();
                port++;
                user++;
                server.changPort(port,user);
            }

        }
    }

    public class Server
    {

        // IPEndPoint ipep;
        // Socket newSocket;
        // Socket client;
        int port = 8000;
        int user = 0;
        public void run()
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
            Socket newSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            newSocket.Bind(ipep);
            newSocket.Listen(10);
            Socket client = newSocket.Accept();
            new TcpListener(client, user);
            newSocket.Close();
        }

        public void changPort(int port, int user){
            this.port = port;
            this.user = user;
        }
    }

    public class TcpListener
    {
        Socket socket;
        Thread serverThread;
        NetworkStream stream;
        StreamReader reader;
        int userID;

        public TcpListener(Socket s, int user)
        {
            socket = s;
            userID = user;
            stream = new NetworkStream(s);
            reader = new StreamReader(stream);
            serverThread = new Thread(new ThreadStart(run));
            serverThread.Start();
        }

        public void run()
        {
            string IPAddress;
            IPAddress = socket.AddressFamily.ToString();
            Console.WriteLine("Start:"+IPAddress);
            String data = "";

            while (true)
            {
                data = reader.ReadLine();
                Thread.Sleep(10);

                if (!String.IsNullOrEmpty(data))
                {
                    Console.WriteLine(userID+":"+data);
                }

                if (data.Equals("esc"))
                    break;
            }

            Console.WriteLine("Accept over");
        }
    }
}

