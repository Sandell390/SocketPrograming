using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class EchoServer
    {
        public void Start(int port = 9000) 
        {
            var endPoint = new IPEndPoint(IPAddress.Any, port);

            var socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            socket.Bind(endPoint);
            socket.Listen(128);

            _ = Task.Run( () => DoEcho(socket));
        }

        private async Task DoEcho(Socket socket) 
        {
            do
            {
                var clientSocket = await Task.Factory.FromAsync(
                    new Func<AsyncCallback, object, IAsyncResult>(socket.BeginAccept),
                    new Func<IAsyncResult, Socket>(socket.EndAccept),
                    null).ConfigureAwait(false);

                Console.WriteLine("ECho server :: Client Connected");

                using var stream = new NetworkStream(clientSocket, true);
                var buffer = new byte[1024];
                do
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);

                    if (bytesRead == 0)
                    {
                        break;
                    }

                    await stream.WriteAsync(buffer, 0, bytesRead).ConfigureAwait(false);
                } while (true);

            } while (true);
        }

    }
}
