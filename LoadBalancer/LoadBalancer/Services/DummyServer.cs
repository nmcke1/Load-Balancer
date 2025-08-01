using LoadBalancer.Interfaces;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LoadBalancer.Services
{
    internal class DummyServer(string address, int port, int weight) : IServer
    {
        // Properties
        public int Weight { get; } = weight;

        private TcpListener listener;
        private CancellationTokenSource cts;
        private readonly string response = $"Hello from IP: {address} Port: {port}\r\n";

        // IServer Implementations
        public void Start()
        {
            cts = new CancellationTokenSource();
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            Console.WriteLine($"Dummy server started on port {port}");
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
            return $"\nAddress: {address} \t Port: {port} \t Weight: {Weight}";
        }
    }
}
