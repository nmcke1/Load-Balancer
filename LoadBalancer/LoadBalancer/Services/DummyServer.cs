using LoadBalancer.Interfaces;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LoadBalancer.Services
{
    internal class DummyServer(string address, int port, int weight) : IServer
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
            Console.WriteLine("Stopping server...");
            cts.Cancel();
            listener.Stop();
        }

        private async Task AcceptClientsAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync(token);
                    _ = Task.Run(() => HandleClientAsync(client), token);
                }
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

        private async Task HandleClientAsync(TcpClient client)
        {
            using (client)
            {
                try
                {
                    var response = $"Hello from IP: {Address.ToString()} Port: {Port}\r\n";
                    NetworkStream stream = client.GetStream();
                    byte[] message = Encoding.UTF8.GetBytes(response);
                    await stream.WriteAsync(message, 0, message.Length);
                    Console.WriteLine("Sent response to client.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error handling client: {ex.Message}");
                }
            }
        }

        public override string ToString()
        {
            return $"\nAddress: {Address.ToString()} \t Port: {Port} \t Weight: {Weight}";
        }
    }
}
