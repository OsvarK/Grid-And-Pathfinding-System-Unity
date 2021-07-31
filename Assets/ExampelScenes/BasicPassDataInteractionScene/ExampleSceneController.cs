using UnityEngine;

public class ExampleSceneController : MonoBehaviour
{
    private void Start()
    {
        // Converts all cells in the grid to use the ExampleCell Data & logic.
        Grid grid = GridRenderer.GetSingleton().GetGrid();
        Cell[,] cells = grid.GetGridArray();
        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {

                cells[x, y].AddCellComponent("interaction", new ExampleCell());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Keybindings for this scene
        if (Input.GetMouseButtonDown(1))
        {
            // Interaction code...
            ExampleCellDataInteraction.ExampleInteractionCode();   // <-- EXAMPLE CODE, FELL FREE TO REMOVE!
        }
    }
}
