using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public EquipmentType equipmentType;
    public Image icon;
    [SerializeField] private TextMeshProUGUI itemName;
    Equipment equipment;
    EquipmentManager equipmentManager;
    private GameObject statcard;
    private bool overSlot = false;

    public void Start()
    {
        equipmentManager = EquipmentManager.instance;
    }

    public void Update()
    {
        if (overSlot && equipment != null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (statcard == null)
                {
                    statcard = Instantiate(Resources.Load<GameObject>("Popups/ItemDetails"), transform);
                    statcard.GetComponent<ItemDetailsPopup>().SetupEquipment(equipment);
                }
                else
                {
                    Destroy(statcard);
                    statcard = null;
                }
            }
        }
    }

    public void AddItem(Equipment newEquipment)
    {
        equipment = newEquipment;

        itemName.text = equipment.name;
        icon.sprite = equipment.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        if (equipment != null)
        {
            equipmentManager.UnEquip((int)equipment.equipType);
            equipment = null;

            itemName.text = "";
            icon.sprite = null;
            icon.enabled = false;
        }
    }

    public Equipment GetEquipment()
    {
        return equipment;
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
