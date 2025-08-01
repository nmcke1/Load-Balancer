using LoadBalancer.Interfaces;
using System.Net.Sockets;
using System.Text;

namespace LoadBalancer.ClientHandlers
{
    internal class ServerClientHandler : IServerClientHandler
    {
        public async Task HandleClientAsync(TcpClient client, string response)
        {
            using (client)
            {
                try
                {;
                    NetworkStream stream = client.GetStream();
                    byte[] message = Encoding.UTF8.GetBytes(response);
                    await stream.WriteAsync(message);
                    Console.WriteLine("Sent response to client.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error handling client: {ex.Message}");
                }
            }
        }
    }
}
