using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentType equipType;

    public float maxhealthModifier = 0;
    public float powerModifier = 0;
    public float armorModifier = 0;
    public float lifetimeModifier = 0;
    public float sizeModifier = 0;
    public int instancesModifier = 0;
    public float speedModifier = 0;
    public float movementspeedModifier = 0;
    public int uniqueModifier = 0;
    public float cooldownReductionModifier = 0f;
    public int upgradeLimitModifier = 0;
    public Element elementModifier = default;

    public override void Use()
    {
        base.Use();
        EquipmentManager.instance.Equip(this);
        RemoveFromInventory();
    }
}
