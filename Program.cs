using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpClients
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    TcpClient client = new TcpClient();
                    await client.ConnectAsync(IPAddress.Parse("127.0.0.1"), 8080);

                    NetworkStream stream = client.GetStream();

                    string password = "meinPasswort";
                    byte[] passwordBytes = Encoding.ASCII.GetBytes(password);

                    await stream.WriteAsync(passwordBytes, 0, passwordBytes.Length);
                    Console.WriteLine("Passwort gesendet!");

                    byte[] buffer = new byte[256];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    Console.WriteLine($"Server antwortet: {response}");

                    while (true)
                    {
                        Console.Write("Deine Nachricht: ");
                        string message = Console.ReadLine();
                        byte[] messageBytes = Encoding.ASCII.GetBytes(message);

                        await stream.WriteAsync(messageBytes, 0, messageBytes.Length);

                        if (message.ToLower() == "exit")
                        {
                            break;
                        }

                        buffer = new byte[256];
                        bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                        Console.WriteLine($"Server antwortet: {response}");
                    }

                    client.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
