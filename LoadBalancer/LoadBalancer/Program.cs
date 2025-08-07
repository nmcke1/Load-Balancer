using LoadBalancer.Interfaces;
using LoadBalancer.Services;
using LoadBalancer.ClientHandlers;

namespace LoadBalancer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Setup dummy servers
            string ipAddress = "127.0.0.1";
            IServerClientHandler serverClientHandler = new ServerClientHandler();
            IServer s1 = new DummyServer(ipAddress, 8080, 1, serverClientHandler);
            IServer s2 = new DummyServer(ipAddress, 9090, 3, serverClientHandler);
            IServer s3 = new DummyServer(ipAddress, 7070, 2, serverClientHandler);
            IServer[] serverList = [s1, s2, s3];

            // Set up load balancer
            ILoadBalancerClientHandler loadBalanceClientHandler = new LoadBalancerClientHandler();
            ILoadBalancer loadBalancer = new LoadBalancerService(serverList, ipAddress, 2020, loadBalanceClientHandler);
            loadBalancer.Start();

            Console.Write("Press ENTER to stop a server");
            Console.ReadLine();
            var random = new Random();
            var randomIndex = random.Next(0, 2);
            Console.WriteLine($"The following server will now be stopped: {serverList[randomIndex]}\nWill now be stopped");
            serverList[randomIndex].Stop();
            Console.ReadLine();
        }
    }
}
