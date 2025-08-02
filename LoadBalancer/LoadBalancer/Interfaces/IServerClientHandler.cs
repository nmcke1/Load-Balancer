using System.Net.Sockets;

namespace LoadBalancer.Interfaces
{

    public interface IServerClientHandler
    {
        /// <summary>
        /// Handles communication with a TCP client connected to a server.
        /// </summary>
        /// <param name="client"> The connected <see cref="TcpClient"/> to handle. </param>
        /// <returns> A task representing the asynchronous operation. </returns>
        Task HandleClientAsync(TcpClient client, string response);
    }
}
