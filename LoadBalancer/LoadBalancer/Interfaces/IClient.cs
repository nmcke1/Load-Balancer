using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancer.Interfaces
{
    internal interface IClient
    {
        /// <summary>
        /// Connects to a specified IP and reads the response.
        /// </summary>
        /// <returns>Returns the response from the TCP connection.</returns>
        public string ConnectAndReadResponse();
    }
}
