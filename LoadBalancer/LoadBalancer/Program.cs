using LoadBalancer.Interfaces;
using LoadBalancer.Services;
using RoundRobin;

namespace LoadBalancer
{
    internal class Program
    {
        static void Main(string[] args)
        {

            DummyServer s1 = new DummyServer("1", 1);
            DummyServer s2 = new DummyServer("2", 2);
            DummyServer s3 = new DummyServer("3", 3);

            int[] weights = [0, 1, 4];

            RoundRobinList<IServer> list = new([s1, s2, s3], weights);

            foreach(int i in weights)
            {
                for (int j = 0; j < i+1; j++)
                {
                    Console.WriteLine(list.Next().ToString());
                }
            }

        }
    }
}
