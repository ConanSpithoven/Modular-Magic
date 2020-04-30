using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    public EquipmentType equipmentType;
    public Image icon;
    Equipment equipment;
    EquipmentManager equipmentManager;

    public void Start()
    {
        equipmentManager = EquipmentManager.instance;
    }

    public void AddItem(Equipment newEquipment)
    {
        equipment = newEquipment;

        icon.sprite = equipment.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        if (equipment != null)
        {
            equipmentManager.UnEquip((int)equipment.equipType);
            equipment = null;

            icon.sprite = null;
            icon.enabled = false;
        }
    }

    public Equipment GetEquipment()
    {
        return equipment;
    }
}
