using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton
    public static EquipmentManager instance;

    void Awake()
    {
        instance = this;
    }
    #endregion

    public Equipment[] currentEquipment;

    public delegate void OnEquipmentChanged (Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    Inventory inventory;

    void Start ()
    {
        inventory = Inventory.instance;

        int numSlots = System.Enum.GetNames(typeof(EquipmentType)).Length;
        currentEquipment = new Equipment[numSlots];
    }

    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipType;

        Equipment oldItem = currentEquipment[slotIndex];

        if (oldItem != null)
        {
            inventory.Add(oldItem);
        }

        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }

        currentEquipment[slotIndex] = newItem;
    }

    public void UnEquip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            Equipment oldItem = currentEquipment[slotIndex];
            if (inventory.full)
            {
                DropItem(oldItem);
            }
            else
            {
                inventory.Add(oldItem);
            }
            currentEquipment[slotIndex] = null;

            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);
            }
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

    public string[] SaveEquipment()
    {
        string[] equips = new string[4];
        for (int i = 0; i < 3; i++)
        {
            if (currentEquipment[i] != null)
            {
                equips[i] = currentEquipment[i].name;
            }
            else
            {
                equips[i] = "none";
            }
        }
        return equips;
    }

    public void LoadEquipment()
    {
        EquipmentData data = SaveSystem.LoadEquipment();
        string[] equips = data.equips;
        for (int i = 0; i < 3; i++)
        {
            if (equips[i] != "none")
            {
                Equipment loadedEquip = Resources.Load<Equipment>("Items/Equipment/" + equips[i]);
                Equip(loadedEquip);
            }
        }
    }
}
