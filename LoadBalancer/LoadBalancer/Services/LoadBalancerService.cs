using LoadBalancer.Interfaces;
using LoadBalancer.RoundRobin;
using System.Net;
using System.Net.Sockets;

namespace LoadBalancer.Services
{
    /// <summary>
    /// Service which provides the load balancer functionality
    /// </summary>
    internal class LoadBalancerService : ILoadBalancer
    {
        private readonly int  port;
        private readonly ILoadBalancerClientHandler clientHandler;
        private readonly IPAddress ip;
        private readonly RoundRobinList serverList = new();
        private TcpListener listener;
        private CancellationTokenSource cts;

        public LoadBalancerService(IServer[] servers, string ip, int port, ILoadBalancerClientHandler clientHandler)
        {
            this.ip = IPAddress.Parse(ip);
            this.port = port;
            this.clientHandler = clientHandler;
            foreach(var server in servers)
            {
                server.Start();
                AddServer(server);
            }
        }

        public void Start()
        {
            cts = new CancellationTokenSource();
            listener = new TcpListener(ip, port);
            listener.Start();

            Console.WriteLine($"Load balancer started on port {port}");
            Task.Run(() => AcceptClientsAsync(cts.Token));
        }

        public void Stop()
        {
            if (listener is not null)
            {
                Console.WriteLine("Stopping load balancer");
                cts.Cancel();
                listener.Stop();
                return;
            }
            Console.WriteLine("Loadbalancer not active");
        }

        private async Task AcceptClientsAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                TcpClient client = await listener.AcceptTcpClientAsync(token);
                var server = NextServer();

                try
                {
                    await clientHandler.HandleClientAsync(client, server);
                }
                catch (Exception)
                {
                    bool removed = RemoveServer(server);
                    if (removed)
                        Console.WriteLine($"Removed server {server} \nDue to failure.");
                }
            }

        }

        public void AddServer(IServer server) => serverList.AppendList(server);
        public IServer NextServer() => serverList.NextNode();
        public bool RemoveServer(IServer server) => serverList.RemoveNode(server);
        public string PrintServers() => serverList.ToString();

    }
}
