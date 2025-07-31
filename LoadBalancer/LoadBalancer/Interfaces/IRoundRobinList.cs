using LoadBalancer.RoundRobin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancer.Interfaces
{
    internal interface IRoundRobinList
    {
        void Append(IServer server);
        // Can probably be removed later, mostly for testing at the minute
        string PrintNodes();
    }
}
