
public class Heap<T> where T : IHeapNode<T>
{
    private T[] nodes;
    private int nodeCount;

    /// <param name="maxHeapSize">The maximum size of the heap</param>
    public Heap(int maxHeapSize)
    {
        nodes = new T[maxHeapSize];
    }

    public void Add(T node)
    {
        node.HeapNodeIndex = nodeCount;
        nodes[nodeCount] = node;
        SortUp(node);
        nodeCount++;
    }

    public T RemoveFirstNode()
    {
        T firstNode = nodes[0];
        nodeCount--;
        nodes[0] = nodes[nodeCount];
        nodes[0].HeapNodeIndex = 0;
        SortDown(nodes[0]);
        return firstNode;
    }

    public int Count
    {
        get
        {
            return nodeCount;
        }
    }

    public bool Contains(T node)
    {
        return Equals(nodes[node.HeapNodeIndex], node);
    }

    public void SortDown(T node)
    {
        while (true)
        {
            int childNodeAIndex = node.HeapNodeIndex * 2 + 1;
            int childNodeBIndex = node.HeapNodeIndex * 2 + 2;
            int swapIndex = 0;

            if (childNodeAIndex < nodeCount)
            {
                swapIndex = childNodeAIndex;

                if (childNodeBIndex < nodeCount)
                {
                    if (nodes[childNodeAIndex].CompareTo(nodes[childNodeBIndex]) < 0)
                    {
                        swapIndex = childNodeBIndex;
                    }
                }

                if (node.CompareTo(nodes[swapIndex]) < 0)
                {
                    SwapNodes(node, nodes[swapIndex]);
                } else { return; }
            } else { return; }
        }
    }

    public void SortUp(T node)
    {
        int parentIndex = (node.HeapNodeIndex - 1) / 2;
        while (true)
        {
            T parentNode = nodes[parentIndex];
            if (node.CompareTo(parentNode) > 0)
            {
                SwapNodes(node, parentNode);
            }
            else { break; }

            parentIndex = (node.HeapNodeIndex - 1) / 2;
        }
    }

    public void SwapNodes(T nodeA, T nodeB)
    {
        nodes[nodeA.HeapNodeIndex] = nodeB;
        nodes[nodeB.HeapNodeIndex] = nodeA;
        int nodeAIndex = nodeA.HeapNodeIndex;
        nodeA.HeapNodeIndex = nodeB.HeapNodeIndex;
        nodeB.HeapNodeIndex = nodeAIndex;
    }
}
