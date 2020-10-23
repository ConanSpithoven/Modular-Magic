using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public Transform equipsParent;
    public GameObject equipmentUI;
    EquipmentManager equipmentManager;
    private bool UIActive = false;

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
            if (!GameManager.instance.GetPauseStatus())
            {
                equipmentUI.SetActive(!equipmentUI.activeSelf);
                UIActive = equipmentUI.activeSelf;
            }
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

    public bool GetUIActive()
    {
        return UIActive;
    }

    public void CloseUI()
    {
        equipmentUI.SetActive(false);
        UIActive = false;
    }
}
