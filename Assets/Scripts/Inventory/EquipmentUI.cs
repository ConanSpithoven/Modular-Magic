using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public Transform equipsParent;
    public GameObject equipmentUI;
    EquipmentManager equipmentManager;

    EquipmentSlot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        equipmentManager = EquipmentManager.instance;
        equipmentManager.onEquipmentChanged += UpdateEquipmentUI;

        slots = equipsParent.GetComponentsInChildren<EquipmentSlot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Equipment"))
        {
            equipmentUI.SetActive(!equipmentUI.activeSelf);
        }
    }

    void UpdateEquipmentUI(Equipment newItem, Equipment oldItem)
    {
        if (newItem == null)
        {
            slots[(int)oldItem.equipType].ClearSlot();
        }
        else 
        {
            slots[(int)newItem.equipType].AddItem(newItem);
        }
    }
}
