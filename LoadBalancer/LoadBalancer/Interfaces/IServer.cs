namespace LoadBalancer.Interfaces
{
    internal interface IServer
    {
        // Properties

        /// <summary>
        /// The 'Weight' for each server, determines how much traffic it should get over other servers
        /// </summary>
        int Weight { get; }

        // Functions

        /// <summary>
        /// Starts up a dummy server to receive connections
        /// </summary>
        /// <returns></returns>
        void StartUp();

        /// <summary>
        /// ToString method for IServer objects
        /// </summary>
        /// <returns> Returns address, port and weight information for the server. </returns>
        string ToString();
    }
}
