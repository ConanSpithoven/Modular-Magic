using UnityEngine;

public class Item : ScriptableObject
{
    new public string name = "New Item";
    public ItemType type = ItemType.Consumable;
    public Sprite icon = null;
    public Sprite worldIcon = null;

    public virtual void Use()
    {
        //Use Item
    }

    public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this);
    }
}
