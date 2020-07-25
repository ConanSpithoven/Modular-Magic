using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemDetailsPopup : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private GameObject statList;
    [SerializeField] private TextMeshProUGUI flavorText;

    public void Setup(Item item)
    {
        icon.sprite = item.icon;
        itemName.text = item.name;
        flavorText.text = item.flavor;
    }

    public void SetupConsumable(Consumable consumable)
    {
        icon.sprite = consumable.icon;
        itemName.text = consumable.name;
        flavorText.text = consumable.flavor;
        AddConsumableStats(consumable);
    }

    public void SetupEquipment(Equipment equipment)
    {
        icon.sprite = equipment.icon;
        itemName.text = equipment.name;
        flavorText.text = equipment.flavor;
        AddEquipmentStats(equipment);
    }

    public void SetupPattern(Pattern pattern)
    {
        icon.sprite = pattern.icon;
        itemName.text = pattern.name;
        flavorText.text = pattern.flavor;
        AddPatternStats(pattern);
    }

    public void SetDescription(string text, string typeText)
    {
        GameObject statText = Instantiate(Resources.Load<GameObject>("Popups/StatText"), statList.transform);
        TextMeshProUGUI dText = statText.GetComponent<TextMeshProUGUI>();
        dText.text = text;
        itemType.text = typeText;
    }

    private void AddConsumableStats(Consumable consumable)
    {
        GameObject statText = Instantiate(Resources.Load<GameObject>("Popups/StatText"), statList.transform);
        TextMeshProUGUI text = statText.GetComponent<TextMeshProUGUI>();
        switch (consumable.GetConsumableType())
        {
            case ConsumableType.HealingPotion:
                itemType.text = "Potion";
                text.text = "Restores " + consumable.GetPotency() + " health on use.";
                break;
            case ConsumableType.Medicine:
                itemType.text = "Medicine";
                text.text = "Restores " + consumable.GetPotency() + " health over " + consumable.GetDOTTime() + " seconds.";
                break;
        }
    }

    private void AddEquipmentStats(Equipment equipment)
    {
        switch (equipment.equipType)
        {
            case EquipmentType.Staff:
                itemType.text = "Staff";
                break;
            case EquipmentType.Cloak:
                itemType.text = "Cloak";
                GameObject buffText = Instantiate(Resources.Load<GameObject>("Popups/StatText"), statList.transform);
                TextMeshProUGUI btext = buffText.GetComponent<TextMeshProUGUI>();
                btext.text = "Armor element: " + equipment.elementModifier.ElementName;
                break;
            case EquipmentType.Ring:
                itemType.text = "Ring";
                break;
            case EquipmentType.Amulet:
                itemType.text = "Amulet";
                break;
        }
        if (equipment.maxhealthModifier != 0)
        {
            GameObject buffText = Instantiate(Resources.Load<GameObject>("Popups/StatText"), statList.transform);
            TextMeshProUGUI btext = buffText.GetComponent<TextMeshProUGUI>();
            btext.text = "Max Health +" + equipment.maxhealthModifier;
        }
        if (equipment.armorModifier != 0)
        {
            GameObject buffText = Instantiate(Resources.Load<GameObject>("Popups/StatText"), statList.transform);
            TextMeshProUGUI btext = buffText.GetComponent<TextMeshProUGUI>();
            btext.text = "Armor +" + equipment.armorModifier;
        }
        if (equipment.movementspeedModifier != 0)
        {
            GameObject buffText = Instantiate(Resources.Load<GameObject>("Popups/StatText"), statList.transform);
            TextMeshProUGUI btext = buffText.GetComponent<TextMeshProUGUI>();
            btext.text = "Movementspeed +" + equipment.movementspeedModifier;
        }
        if (equipment.powerModifier != 0)
        {
            GameObject buffText = Instantiate(Resources.Load<GameObject>("Popups/StatText"), statList.transform);
            TextMeshProUGUI btext = buffText.GetComponent<TextMeshProUGUI>();
            btext.text = "Spell power +" + equipment.powerModifier;
        }
        if (equipment.lifetimeModifier != 0)
        {
            GameObject buffText = Instantiate(Resources.Load<GameObject>("Popups/StatText"), statList.transform);
            TextMeshProUGUI btext = buffText.GetComponent<TextMeshProUGUI>();
            btext.text = "Spell duration +" + equipment.lifetimeModifier;
        }
        if (equipment.sizeModifier != 0)
        {
            GameObject buffText = Instantiate(Resources.Load<GameObject>("Popups/StatText"), statList.transform);
            TextMeshProUGUI btext = buffText.GetComponent<TextMeshProUGUI>();
            btext.text = "Spell size +" + equipment.sizeModifier;
        }
        if (equipment.instancesModifier != 0)
        {
            GameObject buffText = Instantiate(Resources.Load<GameObject>("Popups/StatText"), statList.transform);
            TextMeshProUGUI btext = buffText.GetComponent<TextMeshProUGUI>();
            btext.text = "Spell multicasts +" + equipment.instancesModifier;
        }
        if (equipment.speedModifier != 0)
        {
            GameObject buffText = Instantiate(Resources.Load<GameObject>("Popups/StatText"), statList.transform);
            TextMeshProUGUI btext = buffText.GetComponent<TextMeshProUGUI>();
            btext.text = "Spell speed +" + equipment.speedModifier;
        }
        if (equipment.uniqueModifier != 0)
        {
            GameObject buffText = Instantiate(Resources.Load<GameObject>("Popups/StatText"), statList.transform);
            TextMeshPro btext = buffText.GetComponent<TextMeshPro>();
            btext.text = "Spell unique aspect +" + equipment.uniqueModifier;
        }
        if (equipment.cooldownReductionModifier != 0)
        {
            GameObject buffText = Instantiate(Resources.Load<GameObject>("Popups/StatText"), statList.transform);
            TextMeshProUGUI btext = buffText.GetComponent<TextMeshProUGUI>();
            btext.text = "Spell castingspeed +" + equipment.cooldownReductionModifier;
        }
        if (equipment.upgradeLimitModifier != 0)
        {
            GameObject buffText = Instantiate(Resources.Load<GameObject>("Popups/StatText"), statList.transform);
            TextMeshProUGUI btext = buffText.GetComponent<TextMeshProUGUI>();
            btext.text = "Spell expansion limit +" + equipment.upgradeLimitModifier;
        }
    }

    private void AddPatternStats(Pattern pattern)
    {
        switch (pattern.patternType)
        {
            case PatternType.Elemental:
                itemType.text = "Elemental Pattern";
                GameObject elementText = Instantiate(Resources.Load<GameObject>("Popups/StatText"), statList.transform);
                TextMeshProUGUI etext = elementText.GetComponent<TextMeshProUGUI>();
                etext.text = "Spell element: " + pattern.elementModifier.ElementName;
                break;
            case PatternType.Variant:
                itemType.text = "Variant Pattern";
                GameObject variantText = Instantiate(Resources.Load<GameObject>("Popups/StatText"), statList.transform);
                TextMeshProUGUI vtext = variantText.GetComponent<TextMeshProUGUI>();
                vtext.text = "Spell variant: " + pattern.shapeModifier;
                break;
            case PatternType.Empowerment:
                itemType.text = "Empowerment Pattern";
                if (pattern.powerModifier != 0)
                {
                    GameObject buffText = Instantiate(Resources.Load<GameObject>("Popups/StatText"), statList.transform);
                    TextMeshProUGUI btext = buffText.GetComponent<TextMeshProUGUI>();
                    btext.text = "Spell power +" + pattern.powerModifier;
                }
                if (pattern.lifetimeModifier != 0)
                {
                    GameObject buffText = Instantiate(Resources.Load<GameObject>("Popups/StatText"), statList.transform);
                    TextMeshProUGUI btext = buffText.GetComponent<TextMeshProUGUI>();
                    btext.text = "Spell duration +" + pattern.lifetimeModifier;
                }
                if (pattern.sizeModifier != 0)
                {
                    GameObject buffText = Instantiate(Resources.Load<GameObject>("Popups/StatText"), statList.transform);
                    TextMeshProUGUI btext = buffText.GetComponent<TextMeshProUGUI>();
                    btext.text = "Spell size +" + pattern.sizeModifier;
                }
                if (pattern.instancesModifier != 0)
                {
                    GameObject buffText = Instantiate(Resources.Load<GameObject>("Popups/StatText"), statList.transform);
                    TextMeshProUGUI btext = buffText.GetComponent<TextMeshProUGUI>();
                    btext.text = "Spell multicasts +" + pattern.instancesModifier;
                }
                if (pattern.speedModifier != 0)
                {
                    GameObject buffText = Instantiate(Resources.Load<GameObject>("Popups/StatText"), statList.transform);
                    TextMeshProUGUI btext = buffText.GetComponent<TextMeshProUGUI>();
                    btext.text = "Spell speed +" + pattern.speedModifier;
                }
                if (pattern.uniqueModifier != 0)
                {
                    GameObject buffText = Instantiate(Resources.Load<GameObject>("Popups/StatText"), statList.transform);
                    TextMeshProUGUI btext = buffText.GetComponent<TextMeshProUGUI>();
                    btext.text = "Spell unique aspect +" + pattern.uniqueModifier;
                }
                if (pattern.cooldownReductionModifier != 0)
                {
                    GameObject buffText = Instantiate(Resources.Load<GameObject>("Popups/StatText"), statList.transform);
                    TextMeshProUGUI btext = buffText.GetComponent<TextMeshProUGUI>();
                    btext.text = "Spell castingspeed +" + pattern.cooldownReductionModifier;
                }
                if (pattern.upgradeLimitModifier != 0)
                {
                    GameObject buffText = Instantiate(Resources.Load<GameObject>("Popups/StatText"), statList.transform);
                    TextMeshProUGUI btext = buffText.GetComponent<TextMeshProUGUI>();
                    btext.text = "Spell expansion limit +" + pattern.upgradeLimitModifier;
                }
                break;
        }
    }
}
