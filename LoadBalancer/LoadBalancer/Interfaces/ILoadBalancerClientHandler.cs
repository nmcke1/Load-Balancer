using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancer.Interfaces
{
    internal interface ILoadBalancerClientHandler
    {
        /// <summary>
        /// Handles communication with a TCP client connected to the load balancer
        /// </summary>
        /// <param name="client"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public Task HandleClientAsync(System.Net.Sockets.TcpClient client, IServer server);
    }
}
