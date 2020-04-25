using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentType equipType;

    public float damage = 0;
    public float lifetime = 0;
    public float size = 0;
    public int instances = 0;
    public float speed = 0;
    public int unique = 0;
    public float cooldownReduction = 0f;
    public Element element = default;

    public override void Use()
    {
        base.Use();
        EquipmentManager.instance.Equip(this);
        RemoveFromInventory();
    }
}
