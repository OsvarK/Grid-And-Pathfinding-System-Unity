public class AStarCell : ICellDataComponent, IHeapNode<AStarCell>
{
    public static readonly string COMPONENT_KEY = "a-star"; // Use this as the key in the cellDataComponents in cell.cs 
    public bool Walkable { get; set; } = true;
    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost { get { return GCost + HCost; } }
    public readonly Cell CellParent;
    public Cell pathParentCell { get; set; }
    public int HeapNodeIndex { get; set ; }

    public AStarCell(Cell cell)
    {
        CellParent = cell;
    }

    public void OnInteraction(ICellInteractionData interactionData)
    {
        return;
    }

    public static AStarCell GetComponentFrom(Cell cell)
    {
        return (AStarCell)cell.GetCellComponent(COMPONENT_KEY);
    }

    public int CompareTo(AStarCell aStarCell)
    {
        int compare = FCost.CompareTo(aStarCell.FCost);
        if (compare == 0)
        {
            compare = HCost.CompareTo(aStarCell.HCost);
        }
        return -compare;
    }
}
