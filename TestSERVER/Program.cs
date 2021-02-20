using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TestSERVER
{
    class Program
    {
        static string[] myCommands =
        {
            "-Help",
            "-Close",
            "-ShowName"
        };
        static void StartServer()
        {
            const string ip = "127.0.0.1";
            const int port = 8080;

            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            tcpSocket.Bind(tcpEndPoint);
            tcpSocket.Listen(5);

            while (true)
            {
                var listener = tcpSocket.Accept();
                Console.WriteLine("**На линии один клиент!**");
                while (true)
                {
                    var buffer = new byte[256];
                    var size = 0;
                    var data = new StringBuilder();

                    do
                    {
                        size = listener.Receive(buffer);
                        data.Append(Encoding.Unicode.GetString(buffer, 0, size));
                    }
                    while (listener.Available > 0);

                    var question = Convert.ToString(data);

                    if (question.ToLower() == myCommands[0].ToLower()) //-Help
                    {
                        var msg = new StringBuilder();
                        msg.Append("Available commands: \n\n");
                        for (int i = 0; i < myCommands.Length; i++)
                        {
                            msg.Append(myCommands[i] + "\n");
                        }
                        listener.Send(Encoding.Unicode.GetBytes(Convert.ToString(msg)));
                    }
                    else if (question.ToLower() == myCommands[1].ToLower()) //-Close
                    {
                        listener.Send(Encoding.Unicode.GetBytes("Пока!"));
                        listener.Shutdown(SocketShutdown.Both);
                        listener.Close();
                        Console.WriteLine("**Пользователь отключен!**");
                        break;
                    }
                    else if (question.ToLower() == myCommands[2].ToLower()) //-ShowName
                    {
                        listener.Send(Encoding.Unicode.GetBytes("Прокофьев В.О."));
                    }
                    else //Ответ вручную
                    {
                        Console.WriteLine("Client: " + Convert.ToString(data));
                        Console.Write("Ответ: ");
                        var messageOut = Convert.ToString(Console.ReadLine());
                        listener.Send(Encoding.Unicode.GetBytes(messageOut));
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            StartServer();
        }
    }
}
