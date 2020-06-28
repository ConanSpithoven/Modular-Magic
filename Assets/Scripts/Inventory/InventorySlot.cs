using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;
    Item item;

    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        DropItem(item);
        Inventory.instance.Remove(item);
    }

    public void UseItem()
    {
        if (item != null)
        {
            item.Use();
        }
    }

    private void DropItem(Item oldItem)
    {
        GameObject droppedItem = new GameObject(oldItem.name);
        droppedItem.AddComponent<SpriteRenderer>();
        droppedItem.GetComponent<SpriteRenderer>().sprite = oldItem.worldIcon;
        droppedItem.AddComponent<BoxCollider>();
        droppedItem.AddComponent<ItemPickup>();
        droppedItem.GetComponent<ItemPickup>().item = oldItem;
        Vector3 playerPos = GameManager.instance.GetPlayer().transform.position;
        droppedItem.transform.position = new Vector3(playerPos.x, 0.5f, playerPos.z);
        droppedItem.transform.rotation = Quaternion.Euler(90, 0, 0);
        droppedItem.transform.localScale *= 0.5f;
    }
}
