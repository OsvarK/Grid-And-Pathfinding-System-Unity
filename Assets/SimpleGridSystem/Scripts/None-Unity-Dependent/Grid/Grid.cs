using System;

/// <summary>Representation of a grid in a 2D world.</summary>
public class Grid
{
    public readonly int width;
    public readonly int height;
    public readonly int cellSize;
    private Cell[,] gridArray;
    /// <summary>Constructs a grid using the <paramref name="width"/> and <paramref name="height"/> to define its x and y size, 
    /// the <paramref name="cellSize"/> defines the size on each cell in the grid.</summary>
    public Grid(int width, int height, int cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridArray[x, y] = new Cell(x, y, this);
            }
        }
    }
    public int GetCellSize() { return cellSize; }
    public Cell[,] GetGridArray() { return gridArray; }
    /// <summary>Calculates the origin of the cell in worldspace (botten left corner of the cell) from the cells X and Y representation.</summary>
    /// <returns>Representation of X and Y in a 2D cordination system (world space).</returns>
    public Tuple<float, float> GetCellInWorldSpace(int x, int y)
    {
        x *= cellSize;
        y *= cellSize;
        return new Tuple<float, float>(x, y);
    }
    /// <summary>Calculates the center of the cell in worldspace from the cells X and Y representation.</summary>
    /// <returns>Representation of X and Y in a 2D cordination system (world space).</returns>
    public Tuple<float, float> GetCellWorldPositionCenter(int x, int y)
    {
        Tuple<float, float> cellOrigin = GetCellInWorldSpace(x, y);
        float xOrigin = cellOrigin.Item1;
        float yOrigin = cellOrigin.Item2;
        float xCellCenter = xOrigin + cellSize * 0.5f;
        float yCellCenter = yOrigin + cellSize * 0.5f;
        return new Tuple<float, float>(xCellCenter, yCellCenter);
    }
    /// <summary>Calculates the center of the grid in worldspace.</summary>
    /// <returns>Representation of X and Y in a 2D cordination system (world space).</returns>
    public Tuple<float, float> GetGridCenterInWorldSpace()
    {
        float x = (width * cellSize) / 2;
        float y = (height * cellSize) / 2;
        return new Tuple<float, float>(x, y);
    }
    /// <summary>Calculates the grid cordinate from world space x and y.</summary>
    /// <returns>Representation of X and Y in a the grid.</returns>
    public Tuple<int, int> WorldSpaceToGridCordinates(float x, float y)
    {
        int xGrid = (int)(x / GetCellSize());
        int yGrid = (int)(y / GetCellSize());
        return new Tuple<int, int>(xGrid, yGrid);
    }
    /// <summary>Retrives the cell from cordinates x and y.</summary>
    public Cell GetCellFromGridCordinates(int x, int y)
    {
        return gridArray[x, y];
    }
    /// <summary>
    /// Interact with and cell, calls OnInteraction in the cell.
    /// </summary>
    /// <param name="interactionData">Contains data and logic about this interaction.</param>
    public void InteractWithCell(int x, int y, ICellInteractionData interactionData = null)
    {
        Cell cell = GetCellFromGridCordinates(x, y);
        cell.CellData.OnInteraction(interactionData);
    }
    public void InteractWithCell(Cell cell, ICellInteractionData interactionData = null)
    {
        cell.CellData.OnInteraction(interactionData);
    }
    /// <summary>Returns cordinate x and y in a string format (x, y).</summary>
    public static string GridCordinatesAsString(int x, int y)
    {
        return "(" + x + "," + y + ")";
    }
}
