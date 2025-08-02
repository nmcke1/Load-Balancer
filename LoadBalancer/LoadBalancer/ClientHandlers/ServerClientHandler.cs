using LoadBalancer.Interfaces;
using System.Net.Sockets;
using System.Text;

namespace LoadBalancer.ClientHandlers
{
    /// <summary>
    /// Handles clients of the servers
    /// </summary>
    internal class ServerClientHandler : IServerClientHandler
    {
        public async Task HandleClientAsync(TcpClient client, string response)
        {
            using (client)
            {
                try
                {
                    using NetworkStream stream = client.GetStream();
                    using StreamWriter writer = new (stream, Encoding.UTF8);

                    await writer.WriteAsync(response);
                    await writer.FlushAsync();  
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error handling client: {ex.Message}");
                }
            }
        }
    }
}
