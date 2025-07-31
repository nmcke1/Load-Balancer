using LoadBalancer.Interfaces;

namespace LoadBalancer.RoundRobin
{
    internal class Node(IServer server)
    {
        public IServer Server = server;
        public Node? Next = null;
    }

}
