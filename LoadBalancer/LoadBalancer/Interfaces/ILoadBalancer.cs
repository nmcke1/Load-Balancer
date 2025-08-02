using LoadBalancer.RoundRobin;

namespace LoadBalancer.Interfaces
{
    /// <summary>
    /// Represents a load balancer that manages a collection of servers
    /// </summary>
    internal interface ILoadBalancer
    {
        /// <summary>
        /// Adds a new server to the list of available servers.
        /// </summary>
        /// <param name="server"> The <see cref="IServer"/> to be added. </param>
        void AddServer(IServer server);

        /// <summary>
        /// Attempts to remove a particular server from the list.
        /// </summary>
        /// <param name="server"> The server to remove.</param>
        /// <returns> True if removed successfully, false otherwise. </returns>
        bool RemoveServer(IServer server);

        /// <summary>
        /// Gets the next server to send traffic to.
        /// </summary>
        /// <returns> The next <see cref="IServer"/> in the list. </returns>
        IServer NextServer();

        /// <summary>
        /// List of information about each server in the list.
        /// </summary>
        /// <returns> String of information about each server. </returns>
        string PrintServers();

        /// <summary>
        /// Starts the load balancer.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the load balancer.
        /// </summary>
        void Stop();
    }
}
