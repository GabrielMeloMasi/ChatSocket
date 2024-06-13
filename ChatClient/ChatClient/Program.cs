using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Client
{
    private static TcpClient client;
    private static NetworkStream stream;

    static void Main(string[] args)
    {
        string serverIp = "192.168.1.5"; // Substitua pelo IP do servidor
        int port = 12345;

        client = new TcpClient();
        client.Connect(serverIp, port);
        Console.WriteLine("Conectado ao servidor");

        stream = client.GetStream();

        Thread receiveThread = new Thread(ReceiveMessages);
        receiveThread.Start();

        while (true)
        {
            string message = Console.ReadLine();
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            stream.Write(buffer, 0, buffer.Length);
        }
    }

    private static void ReceiveMessages()
    {
        byte[] buffer = new byte[1024];
        int byteCount;

        while ((byteCount = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            string message = Encoding.UTF8.GetString(buffer, 0, byteCount);
            Console.WriteLine("Mensagem recebida: " + message);
        }
    }
}
