using LoadBalancer.Interfaces;
using LoadBalancer.Services;
using LoadBalancer.ClientHandlers;
using NUnit.Framework.Internal.Execution;

namespace LoadBalancer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Setup dummy servers
            IServerClientHandler serverClientHandler = new ServerClientHandler();
            IServer s1 = new DummyServer("127.0.0.1", 8080, 1, serverClientHandler);
            IServer s2 = new DummyServer("127.0.0.1", 9090, 3, serverClientHandler);
            IServer s3 = new DummyServer("127.0.0.1", 7070, 2, serverClientHandler);
            IServer[] serverlist = [s1, s2, s3];

            // Set up load balancer
            ILoadBalancerClientHandler loadBalanceClientHandler = new LoadBalancerClientHandler();
            ILoadBalancer loadBalancer = new LoadBalancerService(serverlist, "127.0.0.1", 2020, loadBalanceClientHandler);
            loadBalancer.Start();


            // Running the loadbalancer before and after removal to show how it handles inactive servers
            Console.WriteLine(
                $"\n\nBelow are the servers added to the load balancer: \n{loadBalancer.PrintServers()}" +
                $"\n\nTen requests will be made to the load balancer and we can see how the responses vary across the servers depending on their weight" +
                $"\n\n----- RUNNING 10 REQUESTS -----");
            RunLoadBalacner();

            Console.WriteLine("\n----- Stopping Server2 -----\n Will see that the Server with port 9090 will be removed when it is hit.\n");
            s2.Stop();
            Console.WriteLine("\nWill run 10 more requests to check that the removed server is not used" +
                "\n\n----- RUNNING 10 REQUESTS -----");
            RunLoadBalacner();

        }

        // Helper function to do 10 runs of the load balancer
        static readonly IClient dummyClient = new DummyClient("127.0.0.1", 2020);
        private static void RunLoadBalacner()
        {
            var count = 0;
            while (count < 10)
            {
                Console.WriteLine("\n" + dummyClient.ConnectAndReadResponse());
                count++;
            }
        }
    }
}
