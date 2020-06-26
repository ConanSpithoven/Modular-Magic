using UnityEngine;

public class Item : ScriptableObject
{
    new public string name = "New Item";
    public ItemType type = ItemType.Consumable;
    public Sprite icon = null;
    [SerializeField] private GameObject worldItem;

    public virtual void Use()
    {
        //Use Item
    }

    public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this);
    }

    public GameObject GetWorldItem()
    {
        return worldItem;
    }
}
