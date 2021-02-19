[System.Serializable]
public class InventoryData
{
    public string[] inventoryContent;
    public int[] itemTypes;

    public InventoryData(string[] inventoryContent, int[] itemTypes)
    {
        this.inventoryContent = inventoryContent;
        this.itemTypes = itemTypes;
    }
}
