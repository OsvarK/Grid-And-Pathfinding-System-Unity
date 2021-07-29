using UnityEngine;

public class GridInteraction : MonoBehaviour
{
    private void Update()
    {
        // On click right mouse button
        if (Input.GetMouseButtonDown(1))
        {
            // Interaction code...
            ExampleInteractionCode();   // <-- EXAMPLE CODE, FELL FREE TO REMOVE!
        }
    }

    /// <summary>Retrives the cell that is under the mouse.</summary>
    private Cell GetCellFromMousePosition()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Grid grid = GridRenderer.GetSingleton().GetGrid();
        System.Tuple<int, int> cellCords = grid.WorldSpaceToGridCordinates(worldPosition.x, worldPosition.y);
        return grid.GetCellFromGridCordinates(cellCords.Item1, cellCords.Item2);
    }


    // #######################################################################################
    // ############################  EXAMPLE CODE, REMOVE THIS  ##############################
    // #######################################################################################
    private void ExampleInteractionCode()
    {
        Cell cell = GetCellFromMousePosition();
        Grid grid = GridRenderer.GetSingleton().GetGrid();
        // EXAMPLE ONLY: im just assigning every cell as an ExampleCell.
        // Cells shoulde be assigned once on the setup.
        Cell[,] cells = grid.GetGridArray();
        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                cells[x, y].CellData = new ExampleCell();
            }
        }
        // Defines the interaction.
        ExampleCellDataInteraction cellInteraction = new ExampleCellDataInteraction
        {
            Msg = "Example interaction on cell (" + cell.x + "," + cell.y + ")"
        };
        // Call the interaction.
        grid.InteractWithCell(cell, cellInteraction);
    }
}
