using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancer.Interfaces
{
    /// <summary>
    /// Represents a client that can connect to and read from a TCP server.
    /// </summary>
    internal interface IClient
    {
        /// <summary>
        /// Connects to a specified IP and reads the response.
        /// </summary>
        /// <returns>The response received from the TCP server.</returns>
        public string ConnectAndReadResponse();
    }
}
