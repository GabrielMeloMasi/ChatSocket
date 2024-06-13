using System; // Importa o namespace System para acesso a classes básicas do C#
using System.Net.Sockets; // Importa o namespace para operações de soquete de rede
using System.Text; // Importa o namespace para manipulação de strings
using System.Threading; // Importa o namespace para suporte a multithreading

class Client // Declaração de uma classe chamada Client
{
    private static TcpClient client; // Declaração do cliente TCP
    private static NetworkStream stream; // Declaração do fluxo de rede associado ao cliente

    static void Main(string[] args) // Método principal do cliente
    {
        string serverIp = "10.199.65.76"; // Endereço IP do servidor
        int port = 12345; // Porta à qual o cliente irá se conectar

        client = new TcpClient(); // Criação de um novo objeto TcpClient
        client.Connect(serverIp, port); // Conecta o cliente ao servidor utilizando o IP e a porta especificados
        Console.WriteLine("Conectado ao servidor"); // Imprime uma mensagem informando que o cliente se conectou ao servidor

        Console.WriteLine("Digite o Nome do Cliente..: "); // Solicita ao usuário que digite o nome do cliente
        string clientName = Console.ReadLine(); // Lê o nome do cliente fornecido pelo usuário

        stream = client.GetStream(); // Obtém o fluxo de rede associado ao cliente

        Thread receiveThread = new Thread(ReceiveMessages); // Cria uma nova thread para receber mensagens do servidor
        receiveThread.Start(); // Inicia a thread

        while (true) // Loop infinito para enviar mensagens para o servidor
        {
            string message = Console.ReadLine(); // Lê a mensagem digitada pelo usuário
            string fullMessage = $"{clientName}: {message}"; // Formata a mensagem incluindo o nome do cliente
            byte[] bufferMessage = Encoding.UTF8.GetBytes(fullMessage); // Converte a mensagem em bytes

            stream.Write(bufferMessage, 0, bufferMessage.Length); // Envia a mensagem para o servidor
        }
    }

    private static void ReceiveMessages() // Método para receber mensagens do servidor
    {
        byte[] buffer = new byte[1024]; // Buffer para armazenar os dados recebidos do servidor
        int byteCount; // Número de bytes recebidos

        while ((byteCount = stream.Read(buffer, 0, buffer.Length)) != 0) // Loop para ler dados do servidor enquanto houver dados disponíveis
        {
            string message = Encoding.UTF8.GetString(buffer, 0, byteCount); // Converte os dados recebidos em uma string
            Console.WriteLine(message); // Imprime a mensagem recebida no console do cliente
        }
    }
}
