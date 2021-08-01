/// <summary>An interface for handeling data and logic for a cell, a cell can have multiple CellDataComponents.</summary>
public interface ICellDataComponent
{
    /// <summary>Method is called when the cell was interacted with.</summary>
    /// <param name="interactionData">Contains data and logic about this interaction.</param>
    public void OnInteraction(ICellInteractionData interactionData = null);

}
