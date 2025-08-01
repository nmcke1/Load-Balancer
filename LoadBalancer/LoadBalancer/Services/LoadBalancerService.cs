using LoadBalancer.Interfaces;
using LoadBalancer.RoundRobin;

namespace LoadBalancer.Services
{
    internal class LoadBalancerService : ILoadBalancer
    {
        private readonly RoundRobinList serverList = new();

        public LoadBalancerService(IServer[] servers)
        {
            foreach(var server in servers)
            {
                AddServer(server);
            }
        }

        public void AddServer(IServer server)
        {
            serverList.AppendList(server);
        }

        public IServer NextServer()
        {
            return serverList.NextNode();
        }

        public void HealthCheck() => throw new NotImplementedException();

        public void AddServer(IServer server) => serverList.AppendList(server);

        public IServer NextServer() => serverList.NextNode();

        public bool RemoveServer(IServer server) => serverList.RemoveNode(server);

        public string PrintServers() => serverList.ToString();

    }
}
