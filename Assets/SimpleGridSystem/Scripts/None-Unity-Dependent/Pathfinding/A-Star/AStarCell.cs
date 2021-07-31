public class AStarCell : ICellDataComponent
{
    public static readonly string COMPONENT_KEY = "a-star";
    public bool Walkable { get; set; } = true;
    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost { get { return GCost + HCost; } }
    public Cell ParentCell { get; set; }

    public void OnInteraction(ICellInteractionData interactionData)
    {
        return;
    }
}
