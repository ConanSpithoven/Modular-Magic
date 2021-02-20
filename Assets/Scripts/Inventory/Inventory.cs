using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton

    public PlayerManager playerManager;
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    public bool full = false;

    public static Inventory instance;

    public int space = 20;

    public List<Item> items = new List<Item>();

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }

        instance = this;
    }

    private void Start()
    {
        if (playerManager == null)
            playerManager = FindObjectOfType<PlayerManager>().transform.GetComponent<PlayerManager>();
    }

    #endregion

    public bool Add(Item item)
    {
        if (items.Count >= space)
        {
            return false;
        }
        items.Add(item);
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }

        if (items.Count == space)
        {
            full = true;
        }

        return true;
    }

    public void Remove(Item item) 
    {
        items.Remove(item);
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
        if (items.Count < space)
        {
            full = false;
        }
    }

    public string[] SaveInventoryContent()
    {
        string[] inventoryContent = new string[items.Count];
        int i = 0;
        if (items.Count != 0)
        {
            foreach (Item item in items)
            {
                inventoryContent[i] = item.name;
                i++;
            }
        }
        return inventoryContent;
    }

    public int[] SaveInventoryTypes()
    {
        int[] itemTypes = new int[items.Count];
        int i = 0;
        if (items.Count != 0)
        {
            foreach (Item item in items)
            {
                itemTypes[i] = (int)item.type;
                i++;
            }
        }
        return itemTypes;
    }

    public void LoadInventory()
    {
        InventoryData data = SaveSystem.LoadInventory();
        if (data.inventoryContent.Length != 0)
        {
            int i = 0;
            foreach (string itemname in data.inventoryContent)
            {
                string type;
                switch (data.itemTypes[i])
                {
                    default:
                    case 0:
                        type = "Patterns";
                        break;
                    case 1:
                        type = "Consumables";
                        break;
                    case 2:
                        type = "Equipment";
                        break;
                }
                Item loadedItem = Resources.Load<Item>("Items/" + type + "/" + itemname);
                Add(loadedItem);
                i++;
            }
        }
    }
}
