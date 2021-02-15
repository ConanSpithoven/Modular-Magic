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
    public float shieldHealth;
    public Element shieldElement;
    private Barhandler Healthbar;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
        Healthbar = gameManager.GetBarhandler();
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

    public override void TakeDamage(float damageTaken, Element element)
    {
        base.TakeDamage(damageTaken, element);
        Healthbar.SetValue(GetCurrentHP(), maxHealth.GetValue());
    }

    public void TakeShieldDamage(float damageTaken, Element element)
    {
        float elementModifier = CheckElement(shieldElement, element);
        float totalDamage = damageTaken * elementModifier;
        totalDamage = Mathf.Clamp(totalDamage, 0, float.MaxValue);
        shieldHealth -= totalDamage;
        if (shieldHealth <= 0)
        {
            GetComponent<PlayerManager>().RemoveShield();
        }
    }

    public override void Heal(float healing)
    {
        base.Heal(healing);
        Healthbar.SetValue(GetCurrentHP(), maxHealth.GetValue());
    }

    public override void Die()
    {
        //respawn and reduce lives
        gameManager.ChangeLives(1, false);
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        //set stats according to data.
    }
}
