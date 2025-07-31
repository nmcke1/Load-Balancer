using LoadBalancer.Interfaces;

namespace LoadBalancer.Services
{
    internal class TestServer(string address, int port, int weight) : IServer
    {
        public int Weight { get; } = weight;

        // IServer Implementations
        public async Task StartUp()
        {
            Console.WriteLine($"Starting test server at {address}:{port}");
            await Task.Delay(1000); // Simulate async operation
        }

        public override string ToString()
        {
            return $"\nAddress: {address} \t Port: {port} \t Weight: {Weight}";
        }
    }
}
