using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancer.Interfaces
{
    internal interface IServerClientHandler
    {
        /// <summary>
        /// Handles communication with a TCP client connected to a server.
        /// </summary>
        /// <param name="client"> The connected <see cref="TcpClient"/> to handle. </param>
        /// <returns> A task representing the asynchronous operation. </returns>
        Task HandleClientAsync(TcpClient client, string response);
    }
}
