using LoadBalancer.Interfaces;
namespace LoadBalancer.RoundRobin
{
    internal class RoundRobinList : IRoundRobinList
    {
        private Node? head;
        private int weightCount = 0;

        // Add a new node to the end of the list
        public void AppendList(IServer server)
        {
            Node newNode = new(server);

            if (head != null)
            {
                Node current = head;

                // Find the last node (node that points to head)
                while (current!.Next != head)
                {
                    current = current.Next!;
                }

                current.Next = newNode;
                newNode.Next = head; // Keep the list circular
            }
            else
            {
                // If list is empty, new node points to itself
                head = newNode;
                newNode.Next = head;
            }
        }


        public bool RemoveNode(IServer server)
        {
            // Empty list
            if (head == null)
                return false;

            Node current = head;
            Node? previous = null;

            do
            {
                if (current.Server == server)
                {
                    if (previous == null)
                    {
                        // Only one node in the list
                        if (current.Next == head)
                        {
                            head = null;
                            return true;
                        }

                        // Find the last node to update its Next pointer
                        Node last = head;
                        while (last.Next != head)
                        {
                            last = last.Next!;
                        }

                        head = current.Next!;
                        last.Next = head;
                    }
                    else
                    { 
                      // Sets the previous node to point to the node after the removed node, removes node from chain 
                        previous.Next = current.Next;
                        if (current == head)
                        {
                            head = current.Next!;
                        }
                    }
                    return true;
                }
                previous = current;
                current = current.Next!;
            } while (current != head);
            return false; 
        }


        public IServer NextNode()
        {
            if (head != null)
            {
                Node current = head!;

                if (weightCount < current.Server.Weight)
                {
                    // Increment weight count to continue using the same node
                    weightCount++;
                    return current.Server;
                }

                // Reset the weight count after exceding the weight for the node
                weightCount = 1;
                head = current.Next!;
                return head.Server;
            }
            throw new ArgumentOutOfRangeException("No Servers Available");
        }

        // Print all nodes in the circular linked list
        public string ToString()
        {
            if (head == null)
            {
                return "List is empty";
            }

            Node current = head;
            string message = "";

            do
            {
                message += current.Server.ToString();
                current = current.Next!;
            } while (current != head);

            return message;
        }
    }
}
