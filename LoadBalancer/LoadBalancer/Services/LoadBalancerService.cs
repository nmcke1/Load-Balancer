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

        public bool RemoveServer(IServer server)
        {
            return serverList.RemoveNode(server);
        }

        public string PrintServers()
        {
            return serverList.ToString();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void HealthCheck()
        {
            throw new NotImplementedException();
        }
    }
}
