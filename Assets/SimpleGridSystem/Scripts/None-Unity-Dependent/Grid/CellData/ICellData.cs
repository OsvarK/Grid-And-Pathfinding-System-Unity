/// <summary>Interface for handeling cell data and logic.</summary>
public interface ICellData
{
    /// <summary>Method is called when the cell was interacted with.</summary>
    /// <param name="interactionData">Contains data and logic about this interaction.</param>
    public void OnInteraction(ICellInteractionData interactionData);
}
