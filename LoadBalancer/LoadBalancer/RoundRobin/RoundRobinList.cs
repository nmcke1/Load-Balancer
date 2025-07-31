using LoadBalancer.Interfaces;
namespace LoadBalancer.RoundRobin
{
    internal class RoundRobinList : IRoundRobinList
    {
        private Node? head;
        private int weightCount = 0;

        // Add a new node to the end of the list
        public void Append(IServer server)
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

        public IServer NextNode()
        {
            if (head != null)
            {
                Node current = head!;

                if (weightCount < current.Server.Weight)
                {
                    weightCount++;
                    return current.Server;
                }

                // Reset the weight count after getting to a new server
                weightCount = 1;
                head = current.Next!;
                return head.Server;
            }

            throw new ArgumentOutOfRangeException("No Servers Available");
        }

        // Print all nodes in the circular linked list
        public string PrintNodes()
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
