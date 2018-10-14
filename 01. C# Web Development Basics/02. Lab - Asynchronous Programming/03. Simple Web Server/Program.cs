namespace _03.Simple_Web_Server
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    using static System.Console;

    internal static class Program
    {
        private static void Main()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 80);
            listener.Start();

            WriteLine("Server started.");
            WriteLine("Listening to TCP clients at localhost");

            Task.Run(() => ConnectWithTcpClient(listener)).Wait();
        }

        private static async Task ConnectWithTcpClient(TcpListener listener)
        {
            while (true)
            {
                WriteLine("Waiting for client...");
                TcpClient client = await listener.AcceptTcpClientAsync();

                WriteLine("Client connected.");

                byte[] buffer = new byte[1024];
                client.GetStream().Read(buffer, 0, buffer.Length);

                string message = Encoding.ASCII.GetString(buffer);
                WriteLine(message);

                byte[] data = Encoding.ASCII.GetBytes("Hello from server!");
                client.GetStream().Write(data, 0, data.Length);

                WriteLine("Closing connection.");
                client.GetStream().Dispose();
            }
        }
    }
}