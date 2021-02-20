using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    class Program
    {
        static void StartClient()
        {
            const string ip = "127.0.0.1";
            const int port = 8080;

            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            tcpSocket.Connect(tcpEndPoint);
            Console.WriteLine("**Вы подключены к серверу!**");
            while (true)
            {
                Console.Write("Введите сообщение: ");
                var message = Console.ReadLine();
                var data = Encoding.Unicode.GetBytes(message);
                tcpSocket.Send(data);
                var buffer = new byte[256];
                var size = 0;
                var answer = new StringBuilder();

                do
                {
                    size = tcpSocket.Receive(buffer);
                    answer.Append(Encoding.Unicode.GetString(buffer, 0, size));
                }
                while (tcpSocket.Available > 0);

                Console.WriteLine("Server: " + Convert.ToString(answer));

                if (message.ToLower() == "-close")
                {
                    tcpSocket.Shutdown(SocketShutdown.Both);
                    tcpSocket.Close();
                    Console.WriteLine("**Вы успешно отключились от сервера...**\nНажмите любую клавишу для выхода...");
                    break;
                }
            }
            Console.ReadKey();
        }
        static void Main(string[] args)
        {
            StartClient();
        }
    }
}
