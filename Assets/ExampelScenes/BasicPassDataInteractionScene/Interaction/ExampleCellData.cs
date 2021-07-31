/// <summary>This is just an exampel, fell free to remove this class.</summary>
public class ExampleCell : ICellDataComponent
{
    public void OnInteraction(ICellInteractionData interactionData)
    {
        UnityEngine.MonoBehaviour.print(((ExampleCellDataInteraction)interactionData).Msg);
    }
}
