using LoadBalancer.Interfaces;
namespace LoadBalancer.RoundRobin
{
    internal class RoundRobinList : IRoundRobinList
    {
        private readonly object _lock = new();
        private Node? head;
        private int weightCount = 0;

        public void AppendList(IServer server)
        {
            lock (_lock)
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
        }

        public bool RemoveNode(IServer server)
        {
            lock (_lock)
            {
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
                            // Only one node in list
                            if (current.Next == head)
                            {
                                head = null;
                                weightCount = 0; // Reset weightCount
                                return true;
                            }

                            // Find last node to update pointer
                            Node last = head;
                            while (last.Next != head)
                            {
                                last = last.Next!;
                            }

                            head = current.Next!;
                            last.Next = head;
                            weightCount = 0; // Reset here because head changed
                        }
                        else
                        {
                            previous.Next = current.Next;
                            if (current == head)
                            {
                                head = current.Next!;
                                weightCount = 0; // Reset because head changed
                            }
                        }
                        return true;
                    }
                    previous = current;
                    current = current.Next!;
                } while (current != head);

                return false;
            }
        }



        public IServer NextNode()
        {
            lock (_lock)
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
        }

        // Print all nodes in the circular linked list
        public override string ToString()
        {
            lock (_lock)
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
}
