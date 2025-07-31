using LoadBalancer.RoundRobin;
using LoadBalancer.Services;

namespace LoadBalancer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TestServer s1 = new TestServer("1", 1, 1);
            TestServer s2 = new TestServer("2", 2, 2);
            TestServer s3 = new TestServer("3", 3, 5);


            RoundRobinList list  = new RoundRobinList();
            list.Append(s1);
            list.Append(s2);
            list.Append(s3);

            Console.WriteLine(list.PrintNodes());
        }
    }
}
