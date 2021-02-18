using UnityEngine;
using UnityEditor;

public class ItemUpdater: EditorWindow
{
    public List patterns;
    public List equipment;
    public List consumables;

    [MenuItem("Tools/Item Updater")]
    public static void ShowWindow()
    {
        GetWindow<ItemUpdater>("Item Updater");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Update Patterns"))
        {
            UpdatePatterns();
        }
        if (GUILayout.Button("Update Equipment"))
        {
            UpdateEquipment();
        }
        if (GUILayout.Button("Update Consumables"))
        {
            UpdateConsumables();
        }
    }

    //TODO, shorten this to one function with a switch function
    private void UpdatePatterns()
    {
        Debug.Log("Updating Patterns");
        Item[] items = Resources.LoadAll<Item>("Items/Patterns");
        string[] itemNames = new string[items.Length];
        int i = 0;
        foreach (Item item in items)
        {
            itemNames[i] = item.name;
            Debug.Log("added: " + itemNames[i]);
        }
        patterns.SetContent(itemNames);
    }

    private void UpdateEquipment()
    {
        Debug.Log("Updating Equipment");
        Item[] items = Resources.LoadAll<Item>("Items/Equipment");
        string[] itemNames = new string[items.Length];
        int i = 0;
        foreach (Item item in items)
        {
            itemNames[i] = item.name;
            Debug.Log("added: " + itemNames[i]);
        }
        equipment.SetContent(itemNames);
    }

    private void UpdateConsumables()
    {
        Debug.Log("Updating Consumables");
        Item[] items = Resources.LoadAll<Item>("Items/Consumables");
        string[] itemNames = new string[items.Length];
        int i = 0;
        foreach (Item item in items)
        {
            itemNames[i] = item.name;
            Debug.Log("added: " + itemNames[i]);
        }
        consumables.SetContent(itemNames);
    }
}
