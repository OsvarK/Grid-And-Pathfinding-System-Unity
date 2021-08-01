using System.Collections.Generic;
/// <summary>Represents a cell in a grid.</summary>
public class Cell
{
    public readonly int x;      // Refrance the grid cordinate Y
    public readonly int y;      // Refrance the grid cordinate X
    public readonly Grid grid;  // Refrance the grid this cell belongs in

    /// <summary>A cell can have many data component, they are stored here.</summary>
    private IDictionary<string, ICellDataComponent> cellDataComponents = new Dictionary<string, ICellDataComponent>();

    public Cell(int x, int y, Grid grid)
    {
        this.x = x;
        this.y = y;
        this.grid = grid;
    }

    public void AddCellComponent(string componentKey, ICellDataComponent cellDataComponent)
    {
        foreach (KeyValuePair<string, ICellDataComponent> component in cellDataComponents)
        {
            if (cellDataComponent.GetType() == component.Value.GetType())
                return; // This component already exist
        }
        cellDataComponents.Add(componentKey, cellDataComponent);
    }

    public void RemoveCellComponent(string componentKey)
    {
        cellDataComponents.Remove(componentKey);
    }

    public void InteractWithCellComponent(string componentKey, ICellInteractionData interactionData = null)
    {
        cellDataComponents[componentKey].OnInteraction(interactionData);
    }

    public ICellDataComponent GetCellComponent(string componentKey)
    {
        return cellDataComponents[componentKey];
    }
}
