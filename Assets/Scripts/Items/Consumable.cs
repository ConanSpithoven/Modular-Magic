using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Consumable")]
public class Consumable : Item
{
    [SerializeField] private ConsumableType consumableType;
    [SerializeField] private float potency = 0;
    [SerializeField] private int duration = 10;
    [SerializeField] private float speed = 0.5f;

    public override void Use()
    {
        base.Use();
        switch (consumableType)
        {
            case ConsumableType.HealingPotion:
                Inventory.instance.playerManager.Heal(potency);
                break;
            case ConsumableType.Medicine:
                Inventory.instance.playerManager.DOTHeal(potency, duration, speed);
                break;
        }
        RemoveFromInventory();
    }

    public ConsumableType GetConsumableType()
    {
        return consumableType;
    }

    public float GetPotency()
    {
        return potency;
    }

    public int GetDOTTime()
    {
        return Mathf.RoundToInt(duration * speed);
    }
}
