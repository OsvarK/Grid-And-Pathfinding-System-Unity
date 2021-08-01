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
                cells[x, y].AddCellComponent(AStarCell.COMPONENT_KEY, new AStarCell(cells[x, y]));
            }
        }
    }

    /// <summary>Getting the path between two cells using a* search Algorithm.</summary>
    /// <param name="startCell">Where the search starts</param>
    /// <param name="endCell">Where the search ends</param>
    /// <param name="allowDiagonalPassBetweenNoneWalkable">Determines if the A* will search diagonaly, when there is two none walkable blocking each side.</param>
    /// <returns>List of cells, sorted in correct order</returns>
    public List<Cell> GetPath(AStarCell startCell, AStarCell endCell, bool disabelDiagonalPass = true)
    {
        int gridSize = grid.width * grid.height;
        Heap<AStarCell> openSet = new Heap<AStarCell>(gridSize);
        HashSet<AStarCell> closedSet = new HashSet<AStarCell>();

        openSet.Add(startCell);

        while (openSet.Count > 0)
        {
            AStarCell currentCell = openSet.RemoveFirstNode();
            closedSet.Add(currentCell);

            if (currentCell == endCell)
            {
                List<Cell> path = new List<Cell>();
                Cell lookingAt = endCell.CellParent;

                while (lookingAt != startCell.CellParent)
                {
                    path.Add(lookingAt);
                    AStarCell aStarCell = AStarCell.GetComponentFrom(lookingAt);
                    lookingAt = aStarCell.pathParentCell;
                }

                path.Reverse();

                return path;
            }

            foreach (AStarCell neighberingCell in GetNeighberingCells(currentCell, disabelDiagonalPass))
            {

                if (!neighberingCell.Walkable || closedSet.Contains(neighberingCell))
                    continue;

                int moveToNeigborCost = currentCell.GCost + GetDistanceBetweenCells(currentCell, neighberingCell);

                if (moveToNeigborCost < neighberingCell.GCost || !openSet.Contains(neighberingCell))
                {
                    neighberingCell.GCost = moveToNeigborCost;
                    neighberingCell.HCost = GetDistanceBetweenCells(neighberingCell, endCell);
                    neighberingCell.pathParentCell = currentCell.CellParent;

                    if (!openSet.Contains(neighberingCell))
                        openSet.Add(neighberingCell);
                }
            }
        }

        return null;
    }

    /// <summary>Returns all the cells around the given cell.</summary>
    /// <param name="disabelDiagonalPass">Enables or disables diagonal searching between none walkables</param>
    private List<AStarCell> GetNeighberingCells(AStarCell centerCell, bool disabelDiagonalPass = true)
    {
        Cell cell = centerCell.CellParent;

        List<AStarCell> neighbors = new List<AStarCell>();
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
                            AStarCell north = AStarCell.GetComponentFrom(grid.GetCellFromGridCordinates(cell.x, cell.y + 1));
                            AStarCell east = AStarCell.GetComponentFrom(grid.GetCellFromGridCordinates(cell.x + 1, cell.y));

                            if (!north.Walkable && !east.Walkable) { continue; }
                        }
                        // North West
                        else if (x == -1 && y == 1)
                        {
                            AStarCell north = AStarCell.GetComponentFrom(grid.GetCellFromGridCordinates(cell.x, cell.y + 1));
                            AStarCell west = AStarCell.GetComponentFrom(grid.GetCellFromGridCordinates(cell.x - 1, cell.y));

                            if (!north.Walkable && !west.Walkable) { continue; }
                        }
                        // South East
                        else if (x == 1 && y == -1)
                        {
                            AStarCell south = AStarCell.GetComponentFrom(grid.GetCellFromGridCordinates(cell.x, cell.y - 1));
                            AStarCell east = AStarCell.GetComponentFrom(grid.GetCellFromGridCordinates(cell.x + 1, cell.y));

                            if (!south.Walkable && !east.Walkable) { continue; }
                        }
                        // South West
                        else if (x == -1 && y == -1)
                        {
                            AStarCell south = AStarCell.GetComponentFrom(grid.GetCellFromGridCordinates(cell.x, cell.y - 1));
                            AStarCell east = AStarCell.GetComponentFrom(grid.GetCellFromGridCordinates(cell.x - 1, cell.y));

                            if (!south.Walkable && !east.Walkable) { continue; }
                        }
                    }

                    Cell neighborCell = grid.GetCellFromGridCordinates(validX, validY);
                    neighbors.Add(AStarCell.GetComponentFrom(neighborCell));
                }
            }
        }

        return neighbors;
    }

    /// <summary> Retrives the distance between cells. </summary>
    private int GetDistanceBetweenCells(AStarCell cellA, AStarCell cellB)
    {
        int distanceX = Math.Abs(cellA.CellParent.x - cellB.CellParent.x);
        int distanceY = Math.Abs(cellA.CellParent.y - cellB.CellParent.y);

        if (distanceX > distanceY)
            return 14 * distanceY + 10 * (distanceX - distanceY);
        return 14 * distanceX + 10 * (distanceY - distanceX);
    }
}
