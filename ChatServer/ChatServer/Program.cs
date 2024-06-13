using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Server
{
    private static List<TcpClient> clients = new List<TcpClient>();
    private static TcpListener server;

    static void Main(string[] args)
    {
        int port = 12345;
        server = new TcpListener(IPAddress.Any, port);
        server.Start();
        Console.WriteLine("Servidor iniciado...");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            clients.Add(client);
            Console.WriteLine("Novo cliente conectado");

            Thread clientThread = new Thread(HandleClient);
            clientThread.Start(client);
        }
    }

    private static void HandleClient(object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int byteCount;

        while ((byteCount = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            string message = Encoding.UTF8.GetString(buffer, 0, byteCount);
            Console.WriteLine("Mensagem recebida: " + message);
            BroadcastMessage(message, client);
        }

        clients.Remove(client);
        client.Close();
        Console.WriteLine("Cliente desconectado");
    }

    private static void BroadcastMessage(string message, TcpClient excludeClient)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(message);

        foreach (TcpClient client in clients)
        {
            if (client != excludeClient)
            {
                NetworkStream stream = client.GetStream();
                stream.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
