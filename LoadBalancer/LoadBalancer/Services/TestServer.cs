using LoadBalancer.Interfaces;

namespace LoadBalancer.Services
{
    internal class TestServer(string address, int port, int weight) : IServer
    {
        // Properties
        public int Weight { get; } = weight;

        // IServer Implementations
        public void StartUp()
        {
            Console.WriteLine($"Starting test server at {address}:{port}");
        }

        public override string ToString()
        {
            return $"\nAddress: {address} \t Port: {port} \t Weight: {Weight}";
        }
    }
}
