using UnityEngine;

[CreateAssetMenu(fileName = "New itemList", menuName = "Inventory/itemList")]
public class List : ScriptableObject
{
    private string[] content;

    public void SetContent(string[] items)
    {
        content = items;
        Debug.Log("Finished Updating Content");
    }

    public string GetContent(int index)
    {
        return content[index];
    }
}
