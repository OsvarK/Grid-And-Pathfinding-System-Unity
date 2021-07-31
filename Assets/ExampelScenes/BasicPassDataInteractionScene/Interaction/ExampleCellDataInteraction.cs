/// <summary>This is just an exampel, fell free to remove this class.</summary>
public class ExampleCellDataInteraction : ICellInteractionData
{
    public string Msg { get; set; }

    public static void ExampleInteractionCode()
    {
        Cell cell = GridRenderer.GetCellFromMousePosition();

        if (cell == null)
            return;

        // Defines the interaction and its data.
        ExampleCellDataInteraction cellInteractionData = new ExampleCellDataInteraction
        {
            Msg = "Example interaction on cell (" + cell.x + "," + cell.y + ")"
        };
        // Call the interaction from the component.
        cell.InteractWithCellComponent("interaction", cellInteractionData);
    }
}
