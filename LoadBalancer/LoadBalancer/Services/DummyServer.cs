using LoadBalancer.Interfaces;
using System.Net;
using System.Net.Sockets;

namespace LoadBalancer.Services
{
    /// <summary>
    /// Dummy server used to test that the load balancer can correctly redistribute traffic
    /// </summary>
    /// <param name="address">IP address to run the server on.</param>
    /// <param name="port">Port to run the server on.</param>
    /// <param name="weight">Weight of the server showing how much more traffic it can accept than other servers.</param>
    /// <param name="clientHandler">Client handler to handle clients connecting to the server</param>
    internal class DummyServer(string address, int port, int weight, IServerClientHandler clientHandler) : IServer
    {
        // Properties
        public int Port { get; } = port;
        public IPAddress Address { get; } = IPAddress.Parse(address);
        public int Weight { get; } = weight; 

        private TcpListener listener;
        private CancellationTokenSource cts;

        // IServer Implementations
        public void Start()
        {
            cts = new CancellationTokenSource();
            listener = new TcpListener(Address, Port);
            listener.Start();

            Console.WriteLine($"Dummy server started on port {Port}");
            Task.Run(() => AcceptClientsAsync(cts.Token));
        }

        public void Stop()
        {
            if (listener is not null)
            {
                Console.WriteLine("Stopping server");
                cts.Cancel();
                listener.Stop();
                return;
            }
            Console.WriteLine("Server not active");
        }

        private async Task AcceptClientsAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync(token);
                    var response = $"Hello from IP: {Address} Port: {Port}\r\n";
                    _ = clientHandler.HandleClientAsync(client, response);
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Server stopped");
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine($"Server Error. {ex.Message}");
            }
            catch (Exception ex)
            {
               Console.WriteLine($"Server error: {ex.Message}");
            }
        }

        public override string ToString()
        {
            return $"\nAddress: {Address} \t Port: {Port} \t Weight: {Weight}";
        }
    }
}
