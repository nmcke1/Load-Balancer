using LoadBalancer.Interfaces;

namespace LoadBalancer.RoundRobin
{
    /// <summary>
    /// Node in the round robin list
    /// </summary>
    /// <param name="server"></param>
    internal class Node(IServer server)
    {
        public IServer Server = server;
        public Node? Next = null;
    }
}
