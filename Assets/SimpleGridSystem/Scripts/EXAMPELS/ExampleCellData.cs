/// <summary>This is just an exampel, fell free to remove this class.</summary>
public class ExampleCell : ICellData
{
    public void OnInteraction(ICellDataInteraction interactionData)
    {
        UnityEngine.MonoBehaviour.print(((ExampleCellDataInteraction)interactionData).Msg);
    }
}
