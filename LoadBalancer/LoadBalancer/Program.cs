using LoadBalancer.RoundRobin;
using LoadBalancer.Services;

namespace LoadBalancer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DummyServer s1 = new DummyServer("127.0.0.1", 8080, 1);
            DummyServer s2 = new DummyServer("127.0.0.1", 9090, 1);

            s1.StartUp();
            s2.StartUp();
            Console.WriteLine("Server running. Press ENTER to clsoe");
            Console.ReadLine();
            s1.Stop();
        }
    }
}
