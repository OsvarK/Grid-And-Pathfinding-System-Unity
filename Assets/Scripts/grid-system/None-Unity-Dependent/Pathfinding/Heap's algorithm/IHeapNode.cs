using System;

public interface IHeapNode<T> : IComparable<T>
{
    int HeapNodeIndex { get; set; }
}
