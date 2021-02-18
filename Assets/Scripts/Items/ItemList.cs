using UnityEngine;

public class ItemList : MonoBehaviour
{
    [SerializeField] private List patterns;
    [SerializeField] private List equipment;
    [SerializeField] private List consumables;

    public string GetItem(int index, string type)
    {
        switch (type)
        {
            case "Patterns":
                return patterns.GetContent(index);
            case "Equipment":
                return equipment.GetContent(index);
            case "Consumables":
                return consumables.GetContent(index);
            default:
                return null;
        }
    }

    public void UpdateList(string[] items, string type)
    {
        switch (type)
        {
            case "Patterns":
                patterns.SetContent(items);
                Debug.Log("finished updating Patterns");
                break;
            case "Equipment":
                equipment.SetContent(items);
                Debug.Log("finished updating Equipment");
                break;
            case "Consumables":
                consumables.SetContent(items);
                Debug.Log("finished updating Consumables");
                break;
            default:
                Debug.Log("type not found");
                break;
        }
    }
}
