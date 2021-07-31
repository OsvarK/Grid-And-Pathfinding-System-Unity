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
    /// <param name="allowDiagonalPassBetweenNoneWalkable">Determines if the A* will search diagonaly, when there is two none walkable blocking each side.</param>
    /// <returns>List of cells, sorted in correct order</returns>
    public List<Cell> GetPath(Cell startCell, Cell endCell, bool disabelDiagonalPass = true)
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

            foreach (Cell neighberingCell in GetNeighberingCells(currentCell, disabelDiagonalPass))
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

    /// <summary>Returns all the cells around the given cell.</summary>
    /// <param name="disabelDiagonalPass">Enables or disables diagonal searching between none walkables</param>
    private List<Cell> GetNeighberingCells(Cell cell, bool disabelDiagonalPass = true)
    {
        List<Cell> neighbors = new List<Cell>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int validX = cell.x + x;
                int validY = cell.y + y;

                if (validX >= 0 && validX < grid.width && validY >= 0 && validY < grid.height)
                {
                    if (disabelDiagonalPass && Math.Abs(x) == 1 && Math.Abs(y) == 1)
                    {
                        // North East
                        if (x == 1 && y == 1)
                        {
                            AStarCell north = (AStarCell)grid.GetCellFromGridCordinates(cell.x, cell.y + 1).GetCellComponent(AStarCell.COMPONENT_KEY);
                            AStarCell east = (AStarCell)grid.GetCellFromGridCordinates(cell.x + 1, cell.y).GetCellComponent(AStarCell.COMPONENT_KEY);

                            if (!north.Walkable && !east.Walkable) { continue; }
                        }
                        // North West
                        else if (x == -1 && y == 1)
                        {
                            AStarCell north = (AStarCell)grid.GetCellFromGridCordinates(cell.x, cell.y + 1).GetCellComponent(AStarCell.COMPONENT_KEY);
                            AStarCell west = (AStarCell)grid.GetCellFromGridCordinates(cell.x - 1, cell.y).GetCellComponent(AStarCell.COMPONENT_KEY);

                            if (!north.Walkable && !west.Walkable) { continue; }
                        }
                        // South East
                        else if (x == 1 && y == -1)
                        {
                            AStarCell south = (AStarCell)grid.GetCellFromGridCordinates(cell.x, cell.y - 1).GetCellComponent(AStarCell.COMPONENT_KEY);
                            AStarCell east = (AStarCell)grid.GetCellFromGridCordinates(cell.x + 1, cell.y).GetCellComponent(AStarCell.COMPONENT_KEY);

                            if (!south.Walkable && !east.Walkable) { continue; }
                        }
                        // South West
                        else if (x == -1 && y == -1)
                        {
                            AStarCell south = (AStarCell)grid.GetCellFromGridCordinates(cell.x, cell.y - 1).GetCellComponent(AStarCell.COMPONENT_KEY);
                            AStarCell east = (AStarCell)grid.GetCellFromGridCordinates(cell.x - 1, cell.y).GetCellComponent(AStarCell.COMPONENT_KEY);

                            if (!south.Walkable && !east.Walkable) { continue; }
                        }
                    }

                    neighbors.Add(grid.GetCellFromGridCordinates(validX, validY));
                }
            }
        }

        return neighbors;
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
