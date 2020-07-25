using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    public Button removeButton;
    Item item;
    private GameObject statcard;
    private bool overSlot = false;

    public void Update()
    {
        if (overSlot && item != null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (statcard == null)
                {
                    statcard = Instantiate(Resources.Load<GameObject>("Popups/ItemPopup"), transform);
                    switch (item.type)
                    {
                        case ItemType.Consumable:
                            statcard.GetComponent<ItemDetailsPopup>().Setup(item);
                            statcard.GetComponent<ItemDetailsPopup>().SetDescription("A consumable item, primarily used to restore resources or health.", "Consumable");
                            break;
                        case ItemType.Equipment:
                            statcard.GetComponent<ItemDetailsPopup>().Setup(item);
                            statcard.GetComponent<ItemDetailsPopup>().SetDescription("A piece of equipment, will protect or strengthen you when worn.", "Equipment");
                            break;
                        case ItemType.Pattern:
                            statcard.GetComponent<ItemDetailsPopup>().Setup(item);
                            statcard.GetComponent<ItemDetailsPopup>().SetDescription("A spell pattern, can be added to spells to strengthen or change it.", "Pattern");
                            break;
                    }
                }
                else
                {
                    Destroy(statcard);
                    statcard = null;
                }
            }
        }
    }

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        overSlot = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        overSlot = false;
        if (statcard != null)
        {
            Destroy(statcard);
            statcard = null;
        }
    }
}
