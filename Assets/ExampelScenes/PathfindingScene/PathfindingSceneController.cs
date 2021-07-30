using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>Controlls the logic of the PathfindingScene, rendering cells for pathfinding visualization.</summary>
public class PathfindingSceneController : MonoBehaviour
{
    [SerializeField] PathfindingGUI pathfindingGUI;
    [SerializeField] GameObject visualCellPrefab;

    [SerializeField] Color wallColor = Color.black;
    [SerializeField] Color starColor = Color.green;
    [SerializeField] Color endColor = Color.red;

    private List<VisualCell> visualCells = new List<VisualCell>();
    private VisualCell startCell;
    private VisualCell endCell;

    // Update is called once per frame
    void Update()
    {
        // Interaction with cell
        if (Input.GetMouseButtonDown(1))
        {
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
