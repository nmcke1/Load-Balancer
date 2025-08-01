using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancer.Interfaces
{
    internal interface IClientHandler
    {
        /// <summary>
        /// Handles communication with a connected TCP client.
        /// </summary>
        /// <param name="client"> The connected <see cref="TcpClient"/> to handle. </param>
        /// <returns> A task representing the asynchronous operation. </returns>
        Task HandleAsync(TcpClient client);
    }
}
