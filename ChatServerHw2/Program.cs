using ChatServerHw2.DataAccess;
using ChatServerHw2.Domain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServerHw2
{
    //class Program
    //{
    //    static async Task Main(string[] args)
    //    {
    //        var messages = new List<Message>();
    //        using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
    //        //using (var context = new ChatContext())
    //        {
    //            var localIp = IPAddress.Parse("127.0.0.1");
    //            var port = 8080;
    //            var endPoint = new IPEndPoint(localIp, port);


    //            socket.Bind(endPoint);
    //            socket.Listen(5); // Максимальное число соединений в очереди.

    //            Console.WriteLine($"Приложение слушает порт {port}.");

    //            while (true)
    //            {
    //                var seconds = (int)DateTime.Now.Subtract(new DateTime(2020, 1, 1)).TotalSeconds;
    //                var incomingSocket = await socket.AcceptAsync(); //Блокирует поток, пока не получит сообщение
    //                Console.WriteLine($"Получено входящее сообщение.");
    //                await GetMessage(incomingSocket, messages);
    //                if (seconds % 10 == 0)
    //                {
    //                    await SendMessages(socket, messages);
    //                }
    //            }
    //        }
    //    }

    //    static public async Task SendMessages(Socket socket, List<Message> messages)
    //    {
    //        //using (var context = new ChatContext())
    //        //{
    //        //var messages = await context.Messages.ToListAsync();
    //        //var json = JsonConvert.SerializeObject(messages);

    //        //var sendData = Encoding.UTF8.GetBytes(json);
    //        //var res = await socket.SendAsync(sendData, SocketFlags.None);
    //        //Console.WriteLine("Сообщения обновились.");
    //        //}
    //        var json = JsonConvert.SerializeObject(messages);

    //        var sendData = Encoding.UTF8.GetBytes(json);
    //        var res = await socket.SendAsync(sendData, SocketFlags.None);
    //        Console.WriteLine("Сообщения обновились.");
    //    }

    //    static public async Task GetMessage(Socket incomingSocket, List<Message> messages)
    //    {
    //        var stringBuilder = new StringBuilder();
    //        while (incomingSocket.Available > 0)
    //        {
    //            var buffer = new byte[1024];
    //            await incomingSocket.ReceiveAsync(buffer, SocketFlags.None);
    //            stringBuilder.Append(Encoding.UTF8.GetString(buffer));
    //        }
    //        //using (var context = new ChatContext())
    //        //{
    //        //    var message = JsonConvert.DeserializeObject<Message>(stringBuilder.ToString());
    //        //    context.Messages.Add(message);
    //        //    await context.SaveChangesAsync();
    //        //}
    //        var message = JsonConvert.DeserializeObject<Message>(stringBuilder.ToString());
    //        messages.Add(message);
    //    }
    //}

    class Program
    {
        static int port = 8005; // порт для приема входящих запросов
        static void Main(string[] args)
        {
            // получаем адреса для запуска сокета
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            var messages = new List<Message>();
            // создаем сокет
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Bind(ipPoint);

                // начинаем прослушивание
                listenSocket.Listen(10);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    Socket handler = listenSocket.Accept();
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[1024]; // буфер для получаемых данных

                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);

                    var messageJson = builder.ToString();
                    var message = JsonConvert.DeserializeObject<Message>(messageJson);

                    messages.Add(message);
                    
                    Console.WriteLine($"{message.User} : {message.Text}");

                    var messagesJson = JsonConvert.SerializeObject(messages);

                    // отправляем ответ
                    data = Encoding.Unicode.GetBytes(messageJson);
                    handler.Send(data);
                    // закрываем сокет
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

