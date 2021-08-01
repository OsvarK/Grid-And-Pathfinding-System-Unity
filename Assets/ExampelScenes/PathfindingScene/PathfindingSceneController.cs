using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>Controlls the logic of the PathfindingScene, rendering cells for pathfinding visualization.</summary>
public class PathfindingSceneController : MonoBehaviour
{
    [SerializeField] PathfindingGUI pathfindingGUI;
    [SerializeField] GameObject visualCellPrefab;

    [SerializeField] Color wallColor = Color.black;
    [SerializeField] Color starColor = Color.green;
    [SerializeField] Color endColor = Color.red;
    [SerializeField] Color finalPathColor = Color.blue;

    private List<VisualCell> visualCells = new List<VisualCell>();
    private VisualCell startCell;
    private VisualCell endCell;
    private AStarAlgorithm aStar;

    private bool diagonalMovement;

    private void Start()
    {
        Grid grid = GridRenderer.GetSingleton().GetGrid();
        aStar = new AStarAlgorithm(grid);
    }


    private bool mouseBtnDown;
    void Update()
    {
        if (Input.GetMouseButtonUp(1)) { mouseBtnDown = false; }
        // Interaction with cell
        if (Input.GetMouseButtonDown(1) || mouseBtnDown)
        {
            mouseBtnDown = true;
            if (Input.GetKey(KeyCode.LeftControl))
            {
                // REMOVE CELL
                Cell cell = GridRenderer.GetCellFromMousePosition();
                int visualCellIndexToRemove = -1;
                int index = 0;
                foreach (VisualCell visualCell in visualCells)
                {
                    if (cell == visualCell.Cell)
                    {
                        AStarCell aStarCell = (AStarCell)cell.GetCellComponent(AStarCell.COMPONENT_KEY);
                        aStarCell.Walkable = true;
                        Destroy(visualCell.CellPrefab);
                        visualCellIndexToRemove = index;
                    }
                    index++;
                }

                if (visualCellIndexToRemove != -1) 
                 visualCells.RemoveAt(visualCellIndexToRemove);

            } else
            {
                // SPAWN CELL
                PathfindingGUI.Tools tool = pathfindingGUI.CurrentTool;
                Cell cell = GridRenderer.GetCellFromMousePosition();

                if (startCell != null && cell == startCell.Cell) { return; }
                if (endCell != null && cell == endCell.Cell) { return; }

                foreach (VisualCell visualCell in visualCells)
                {
                    if (cell == visualCell.Cell)
                        return;
                }

                switch (tool)
                {
                    case PathfindingGUI.Tools.wall:
                        AStarCell aStarCell = (AStarCell)cell.GetCellComponent(AStarCell.COMPONENT_KEY);
                        aStarCell.Walkable = false;
                        visualCells.Add(SpawnVisualCellAtCell(cell, wallColor));
                        break;
                    case PathfindingGUI.Tools.start:
                        if (startCell != null)
                        {
                            Destroy(startCell.CellPrefab);
                        }
                        startCell = SpawnVisualCellAtCell(cell, starColor);
                        break;
                    case PathfindingGUI.Tools.end:
                        if (endCell != null)
                        {
                            Destroy(endCell.CellPrefab);
                        }
                        endCell = SpawnVisualCellAtCell(cell, endColor);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void ToggleDiagonalMovement()
    {
        diagonalMovement = !diagonalMovement;
    }


    private List<VisualCell> visualPath = new List<VisualCell>();
    public void CalculatePath()
    {
        foreach (VisualCell cell in visualPath)
        {
            Destroy(cell.CellPrefab);
        }
        visualPath.Clear();


        if (startCell == null || endCell == null)
        {
            Debug.Log("Reqiures a start and a end cell.");
            return;
        }

        List<Cell> path = aStar.GetPath(AStarCell.GetComponentFrom(startCell.Cell), AStarCell.GetComponentFrom(endCell.Cell), !diagonalMovement);

        if (path == null)
        {
            Debug.Log("No path found");
            return;
        }

        StartCoroutine(StartRenderPath());
        IEnumerator StartRenderPath()
        {
            int i = 0;
            foreach (Cell cell in path)
            {
                yield return 0;
                if (i + 1 == path.Count) { continue; }
                visualPath.Add(SpawnVisualCellAtCell(cell, finalPathColor));
                i++;
            }
        }
    }

    public void ClearGrid()
    {
        foreach (VisualCell visualCell in visualCells)
        {  
            AStarCell aStarCell = (AStarCell)visualCell.Cell.GetCellComponent(AStarCell.COMPONENT_KEY);
            aStarCell.Walkable = true;
            Destroy(visualCell.CellPrefab);
        }

        foreach (VisualCell visualCell in visualPath)
        {
            Destroy(visualCell.CellPrefab);
        }

        visualCells.Clear();
        visualPath.Clear();
    }

    private VisualCell SpawnVisualCellAtCell(Cell cell, Color color)
    {
        Grid grid = GridRenderer.GetSingleton().GetGrid();
        Tuple<float, float> loc = grid.GetCellWorldPositionCenter(cell.x, cell.y);
        GameObject gameObject = Instantiate(visualCellPrefab, new Vector3(loc.Item1, loc.Item2, 0), Quaternion.identity);
        gameObject.GetComponent<SpriteRenderer>().color = color;
        return new VisualCell(cell, gameObject);
    }

    private class VisualCell 
    {
        public Cell Cell { get; private set; }
        public GameObject CellPrefab { get; private set; }

        public VisualCell(Cell cell, GameObject cellPrefab)
        {
            Cell = cell;
            CellPrefab = cellPrefab;
        }

    }
}
