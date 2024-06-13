using System; // Importa o namespace System
using System.Collections.Generic; // Importa o namespace para usar Listas
using System.Net; // Importa o namespace para manipulação de endereços IP e conexões de rede
using System.Net.Sockets; // Importa o namespace para operações de soquete de rede
using System.Text; // Importa o namespace para manipulação de strings
using System.Threading; // Importa o namespace para suporte a multithreading

class Server // Declaração da classe chamada Server
{
    private static List<TcpClient> clients = new List<TcpClient>(); // Declaração de uma lista de clientes conectados ao servidor
    private static TcpListener server; // Declaração do servidor TCP

    static void Main(string[] args) // Método principal do servidor
    {
        int port = 12345; // Porta na qual o servidor irá escutar
        server = new TcpListener(IPAddress.Any, port); // Criação de um objeto TcpListener para escutar conexões na porta especificada
        server.Start(); // Inicia o servidor
        Console.WriteLine("Servidor iniciado..."); // Imprime uma mensagem informando que o servidor foi iniciado

        while (true) // Loop infinito para aceitar conexões de clientes
        {
            TcpClient client = server.AcceptTcpClient(); // Aceita uma nova conexão do cliente
            clients.Add(client); // Adiciona o cliente à lista de clientes conectados
            Console.WriteLine("Novo cliente conectado"); // Imprime uma mensagem informando que um novo cliente se conectou

            Thread clientThread = new Thread(HandleClient); // Cria uma nova thread para lidar com as comunicações do cliente
            clientThread.Start(client); // Inicia a thread e passa o objeto TcpClient como parâmetro
        }
    }

    private static void HandleClient(object obj) // Método para lidar com as comunicações de um cliente
    {
        TcpClient client = (TcpClient)obj; // Converte o objeto genérico para TcpClient
        NetworkStream stream = client.GetStream(); // Obtém o fluxo de rede para leitura e escrita
        byte[] buffer = new byte[1024]; // Buffer para armazenar os dados recebidos do cliente
        int byteCount; // Número de bytes recebidos

        while ((byteCount = stream.Read(buffer, 0, buffer.Length)) != 0) // Loop para ler dados do cliente enquanto houver dados disponíveis
        {
            string message = Encoding.UTF8.GetString(buffer, 0, byteCount); // Converte os dados recebidos em uma string
            Console.WriteLine(message); // Imprime a mensagem recebida no console do servidor
            BroadcastMessage(message, client); // Envia a mensagem para todos os outros clientes conectados, exceto o remetente
        }

        clients.Remove(client); // Remove o cliente da lista de clientes conectados
        client.Close(); // Fecha a conexão com o cliente
        Console.WriteLine("Cliente desconectado"); // Imprime uma mensagem informando que o cliente foi desconectado
    }

    private static void BroadcastMessage(string message, TcpClient excludeClient) // Método para enviar uma mensagem para todos os clientes conectados, exceto o cliente especificado
    {
        byte[] buffer = Encoding.UTF8.GetBytes(message); // Converte a mensagem em bytes

        foreach (TcpClient client in clients) // Loop para enviar a mensagem para cada cliente conectado
        {
            if (client != excludeClient) // Verifica se o cliente não é o cliente especificado para ser excluído
            {
                NetworkStream stream = client.GetStream(); // Obtém o fluxo de rede do cliente
                stream.Write(buffer, 0, buffer.Length); // Envia a mensagem para o cliente
            }
        }
    }
}
