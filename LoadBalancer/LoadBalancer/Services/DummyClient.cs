using LoadBalancer.Interfaces;
using System;
using System.IO;
using System.Net.Sockets;

namespace LoadBalancer.Services
{
    /// <summary>
    /// A dummy client used for testing the load balancer
    /// </summary>
    /// <param name="address">IP address the client connects to.</param>
    /// <param name="port">Port the client connects to.</param>
    public class DummyClient(string address, int port) : IClient
    {
        public string ConnectAndReadResponse()
        {
            try
            {
                using TcpClient client = new (address, port);
                using StreamReader reader = new (client.GetStream());

                string response = reader.ReadLine();
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to connect or read: {ex.Message}");
                return null;
            }
        }
    }
}