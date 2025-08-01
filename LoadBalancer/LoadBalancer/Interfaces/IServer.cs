namespace LoadBalancer.Interfaces
{
    internal interface IServer
    {
        // Properties
        int Weight { get; }

        // Functions
        Task StartUp();
        string ToString();
    }
}
