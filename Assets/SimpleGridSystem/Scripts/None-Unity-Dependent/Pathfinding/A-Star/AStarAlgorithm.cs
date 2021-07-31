using System;
using System.Collections.Generic;
/// <summary>A* Pathfinding search from grid a grid class</summary>
public class AStarAlgorithm
{
    private Grid grid;

    public AStarAlgorithm(Grid grid)
    {
        this.grid = grid;
        
        // Creates the AStarCell data component for every cell in the grid
        Cell[,] cells = grid.GetGridArray();
        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                cells[x, y].AddCellComponent(AStarCell.COMPONENT_KEY, new AStarCell());
            }
        }
    }

    /// <summary>Getting the path between two cells using a* search Algorithm.</summary>
    /// <param name="startCell">Where the search starts</param>
    /// <param name="endCell">Where the search ends</param>
    /// <returns>List of cells, sorted in correct order</returns>
    public List<Cell> GetPath(Cell startCell, Cell endCell)
    {
        List<Cell> openSet = new List<Cell>();
        HashSet<Cell> closedSet = new HashSet<Cell>();

        openSet.Add(startCell);

        while (openSet.Count > 0)
        {
            Cell currentCell = openSet[0];
            AStarCell currentCellData = (AStarCell)currentCell.GetCellComponent(AStarCell.COMPONENT_KEY);

            for (int i = 1; i < openSet.Count; i++)
            {
                Cell comparingCell = openSet[i];
                AStarCell comparingCellData = (AStarCell)comparingCell.GetCellComponent(AStarCell.COMPONENT_KEY);

                if (comparingCellData.FCost < comparingCellData.FCost || ((comparingCellData.FCost == currentCellData.FCost) && (currentCellData.HCost < currentCellData.HCost)))
                {
                    currentCell = comparingCell;
                }
            }

            openSet.Remove(currentCell);
            closedSet.Add(currentCell);

            if (currentCell == endCell)
            {
                List<Cell> path = new List<Cell>();
                Cell lookingAt = endCell;

                while (lookingAt != startCell)
                {
                    path.Add(lookingAt);
                    AStarCell aStarCell = (AStarCell)lookingAt.GetCellComponent(AStarCell.COMPONENT_KEY);
                    lookingAt = aStarCell.ParentCell;
                }

                path.Reverse();

                return path;
            }

            foreach (Cell neighberingCell in grid.GetNeighberingCells(currentCell))
            {
                AStarCell neighberCellData = (AStarCell)neighberingCell.GetCellComponent(AStarCell.COMPONENT_KEY);

                if (!neighberCellData.Walkable || closedSet.Contains(neighberingCell))
                    continue;

                int moveToNeigborCost = currentCellData.GCost + GetDistanceBetweenCells(currentCell, neighberingCell);

                if (moveToNeigborCost < neighberCellData.GCost || !openSet.Contains(neighberingCell))
                {
                    neighberCellData.GCost = moveToNeigborCost;
                    neighberCellData.HCost = GetDistanceBetweenCells(neighberingCell, endCell);
                    neighberCellData.ParentCell = currentCell;

                    if (!openSet.Contains(neighberingCell))
                        openSet.Add(neighberingCell);
                }
            }
        }

        return null;
    }

    /// <summary> Retrives the distance between cells. </summary>
    private int GetDistanceBetweenCells(Cell cellA, Cell cellB)
    {
        int distanceX = Math.Abs(cellA.x - cellB.x);
        int distanceY = Math.Abs(cellA.y - cellB.y);

        if (distanceX > distanceY)
            return 14 * distanceY + 10 * (distanceX - distanceY);
        return 14 * distanceX + 10 * (distanceY - distanceX);
    }
}
