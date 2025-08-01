using LoadBalancer.Interfaces;
using System.Net.Sockets;
using System.Text;

namespace LoadBalancer.ClientHandlers
{
    internal class LoadBalancerClientHandler : ILoadBalancerClientHandler
    {
        public async Task HandleClientAsync(TcpClient client, IServer server)
        {
            using (client)
            using (TcpClient serverConnection = new())
            {
                // Connect to backend server
                await serverConnection.ConnectAsync(server.Address, server.Port);

                using NetworkStream clientStream = client.GetStream();
                using NetworkStream serverStream = serverConnection.GetStream();

                using StreamReader serverReader = new(serverStream, Encoding.UTF8, leaveOpen: true);
                using StreamWriter clientWriter = new(clientStream, Encoding.UTF8, leaveOpen: true);

                string response = await serverReader.ReadLineAsync();

                if (response != null)
                {
                    // Forward that response to the original client
                    await clientWriter.WriteLineAsync(response);
                    await clientWriter.FlushAsync();
                }
            }
        }
    }
}
