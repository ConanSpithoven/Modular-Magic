using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Consumable")]
public class Consumable : Item
{
    [SerializeField] private enum ConsumableType { HealingPotion, Medicine}
    [SerializeField] private ConsumableType consumableType;
    [SerializeField] private float potency = 0;

    public override void Use()
    {
        base.Use();
        switch (consumableType)
        {
            case ConsumableType.HealingPotion:
                Inventory.instance.playerManager.Heal(potency);
                break;
            case ConsumableType.Medicine:
                Inventory.instance.playerManager.DOTHeal(potency, 10, 0.5f);
                break;
        }
        RemoveFromInventory();
    }
}
