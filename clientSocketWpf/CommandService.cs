using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace clientSocketWpf
{
    public class CommandService
    {
        private IPAddress ip = IPAddress.Parse("127.0.0.1");
        private int port = 3535;

        public Task<string> SendToServer(string request)
        {
            return Task.Run(() =>
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    //отправь login на сервер
                    socket.Connect(ip, port);
                    socket.Send(Encoding.UTF8.GetBytes(request));

                    // получаем ответ
                    if (request.StartsWith("checklogin") || request.StartsWith("checklogandpswd"))
                    {
                        byte[] buffer = new byte[1024];
                        int bytes = socket.Receive(buffer);
                        return Encoding.UTF8.GetString(buffer, 0, bytes);
                    }
                    else if (request.StartsWith("get30"))
                    {
                        List<Content> sentences = new List<Content>();
                        byte[] buffer = new byte[6024];
                        int bytes = socket.Receive(buffer);
                        return Encoding.UTF8.GetString(buffer, 0, bytes);
                        
                    }
                    else if (request.StartsWith("getAllContent"))
                    {
                        List<Content> sentences = new List<Content>();
                        byte[] buffer = new byte[6024];
                        //через цикл
                        StringBuilder builder = new StringBuilder();
                        do
                        {
                            int bytes = socket.Receive(buffer);
                            builder.Append(Encoding.UTF8.GetString(buffer, 0, bytes));
                        } while (socket.Available > 0);
                        
                        return builder.ToString();

                    }
                    else if (request.StartsWith("getAllStat"))// getAllStat:abai
                    {
                        List<Statistic> sentences = new List<Statistic>();
                        byte[] buffer = new byte[6024];
                        //через цикл
                        StringBuilder builder = new StringBuilder();
                        do
                        {
                            int bytes = socket.Receive(buffer);
                            builder.Append(Encoding.UTF8.GetString(buffer, 0, bytes));
                        } while (socket.Available > 0);

                        return builder.ToString();
                    }
                    else //request.StartsWith("Add") or request.StartsWith("postStat") 
                    {
                        return null;
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return null;
                }
                finally
                {
                    // закрываем сокет
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
            });
        }
    }
}
