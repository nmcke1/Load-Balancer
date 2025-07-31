using LoadBalancer.Interfaces;

namespace LoadBalancer.Services
{
    internal class TestServer(string address, int port) : IServer
    {
        // Properties
        public string Address { get; } = address;
        public int Port { get; } = port;

        // IServer Implementations
        public async Task StartUp()
        {
            Console.WriteLine($"Starting test server at {Address}:{Port}");
            await Task.Delay(1000); // Simulate async operation
        }

        public string toString() {
            return $"\nAddress: {Address} \t Port: {Port}";
        }
    }
}
