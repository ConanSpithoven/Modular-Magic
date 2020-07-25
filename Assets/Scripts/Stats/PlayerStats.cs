using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    public Stat upgradeLimit;
    public Stat lifetime;
    public Stat size;
    public Stat speed;
    public Stat instances;
    public Stat unique;
    public Stat cooldownReduction;

    void Start()
    {
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if (newItem != null)
        {
            maxHealth.AddModifier(newItem.maxhealthModifier);
            armor.AddModifier(newItem.armorModifier);
            movementspeed.AddModifier(newItem.movementspeedModifier);
            cooldownReduction.AddModifier(newItem.cooldownReductionModifier);
            upgradeLimit.AddModifier(newItem.upgradeLimitModifier);
        }

        if (oldItem != null)
        {
            maxHealth.RemoveModifier(oldItem.maxhealthModifier);
            armor.RemoveModifier(oldItem.armorModifier);
            movementspeed.RemoveModifier(oldItem.movementspeedModifier);
            cooldownReduction.RemoveModifier(oldItem.cooldownReductionModifier);
            upgradeLimit.RemoveModifier(oldItem.upgradeLimitModifier);
        }
    }

    public override void Die()
    { 
        //oof
    }
}
