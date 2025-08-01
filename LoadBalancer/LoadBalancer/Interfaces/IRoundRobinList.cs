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
        /// <summary>
        /// Appends the current list of items to the specified server's storage.
        /// </summary>
        /// <param name="server"> The server that will be appended to the list. </param>
        void AppendList(IServer server);
        
        /// <summary>
        /// Attempts to remove the server node in the list. 
        /// </summary>
        /// <param name="server">The server to be removed</param>
        /// <returns> Returns true if removed successfully, false otherwise. </returns>
        bool RemoveNode(IServer server);

        /// <summary>
        /// Returns the next server node in the list.
        /// </summary>
        /// <returns></returns>
        IServer NextNode();

        /// <summary>
        /// Returns the ToString for each item in the list.
        /// </summary>
        /// <returns> String with details of each item in the list. </returns>
        string ToString();
    }
}
