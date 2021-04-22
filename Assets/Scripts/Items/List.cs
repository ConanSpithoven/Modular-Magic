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
        index = Mathf.Clamp(index, 0, content.Length-1);
        if (content[index] != null)
        {
            return content[index];
        }
        else
        {
            return null;
        }
    }

    public int GetSize()
    {
        return content.Length;
    }
}
