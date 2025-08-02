using System.Net.Sockets;

namespace LoadBalancer.Interfaces
{
    /// <summary>
    /// Represents a client handler for a server that returns a response to the client.
    /// </summary>
    public interface IServerClientHandler
    {
        /// <summary>
        /// Handles communication with a TCP client connected to a server.
        /// </summary>
        /// <param name="client">The connected <see cref="TcpClient"/> to handle. </param>
        /// <param name="response"> response to send back to the client></param>
        /// <returns> A task representing the asynchronous operation. </returns>
        Task HandleClientAsync(TcpClient client, string response);
    }
}
