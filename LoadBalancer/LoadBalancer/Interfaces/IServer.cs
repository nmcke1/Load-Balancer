using System.Net;

namespace LoadBalancer.Interfaces
{
    /// <summary>
    /// Represents a weighted server running on a particular port and IP.
    /// </summary>
    public interface IServer
    {
        // Properties

        /// <summary>
        /// The IP address of the server.
        /// </summary>
        IPAddress Address { get; }

        /// <summary>
        /// The port the server is runnng on.
        /// </summary>
        int Port { get; }

        /// <summary>
        /// The 'Weight' for each server, determines how much traffic it should get over other servers.
        /// </summary>
        int Weight { get; }

        // Functions

        /// <summary>
        /// Starts up a dummy server to receive connections.
        /// </summary>
        /// <returns></returns>
        void Start();

        /// <summary>
        /// Uses the <see cref="CancellationTokenSource"/> to handle any remaining async actions then closes the listener.
        /// </summary>
        void Stop();

        /// <summary>
        /// ToString method for IServer .
        /// </summary>
        /// <returns> Returns address, port and weight information for the server. </returns>
        string ToString();
    }
}
