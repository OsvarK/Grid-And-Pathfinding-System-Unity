/// <summary>Represents a cell in a grid.</summary>
public class Cell
{
    public readonly int x;      // Refrance the grid cordinate Y
    public readonly int y;      // Refrance the grid cordinate X
    public readonly Grid grid;  // Refrance the grid this cell belongs in

    /// <summary>Defines the cell and its logic & properties.</summary>
    public ICellData CellData { get; set; }

    public Cell(int x, int y, Grid grid)
    {
        this.x = x;
        this.y = y;
        this.grid = grid;
    }
}
