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
}
