/// <summary>This is just an exampel, fell free to remove this class.</summary>
public class ExampleCellDataInteraction : ICellInteractionData
{
    public string Msg { get; set; }

    public static void ExampleInteractionCode()
    {
        Cell cell = GridRenderer.GetCellFromMousePosition();
        Grid grid = GridRenderer.GetSingleton().GetGrid();

        // Defines the interaction.
        ExampleCellDataInteraction cellInteraction = new ExampleCellDataInteraction
        {
            Msg = "Example interaction on cell (" + cell.x + "," + cell.y + ")"
        };
        // Call the interaction.
        grid.InteractWithCell(cell, cellInteraction);
    }
}
