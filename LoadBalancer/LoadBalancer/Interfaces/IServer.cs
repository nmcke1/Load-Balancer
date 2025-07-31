namespace LoadBalancer.Interfaces
{
    internal interface IServer
    {
        Task StartUp();
        string toString();
    }
}
