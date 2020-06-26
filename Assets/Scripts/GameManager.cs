using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform startPos;
    private SpellInventory spellInventory;
    private float projectile0Cooldown = 1f;
    private float projectile1Cooldown = 3f;
    private float projectile2Cooldown = 2f;
    private float AoECooldown0 = 5f;
    private float AoECooldown1 = 5f;
    private float AoECooldown2 = 3f;
    private float meleeCooldown0 = 1f;
    private float meleeCooldown1 = 3f;
    private float meleeCooldown2 = 6f;
    private float movementCooldown0 = 3f;
    private float movementCooldown1 = 5f;
    private float movementCooldown2 = 6f;
    private float shieldCooldown0 = 10f;
    private float shieldCooldown1 = 8f;
    private float shieldCooldown2 = 5f;
    private float healCooldown0 = 12f;
    private float healCooldown1 = 10f;
    private float healCooldown2 = 14f;
    private float summonCooldown0 = 5f;
    private float summonCooldown1 = 15f;
    private float summonCooldown2 = 10f;


    #region Singleton

    public static GameManager instance;

    private void Awake()
    {
        GameObject playerPrefab = Resources.Load<GameObject>("PlayerTest");
        GameObject player = Instantiate(playerPrefab, startPos.position, startPos.rotation, null);
        spellInventory = player.GetComponent<SpellInventory>();

        if (instance != null)
        {
            Debug.LogWarning("More than one instance of GameManager found!");
            return;
        }

        instance = this;
    }

    #endregion

    public void ArcheTypeSwap(int spellSlot, SpellType spellType)
    {
        Spell newSpell = Resources.Load<Spell>("Spells/" + spellSlot + "/" + spellType.ToString());
        Spell oldSpell = spellInventory.GetSpell(spellSlot);
        StatTransfer(oldSpell, newSpell);
        spellInventory.SetSpellSlot(newSpell, spellSlot);
    }

    private void StatTransfer(Spell oldSpell, Spell newSpell)
    {
        newSpell.power = oldSpell.power;
        newSpell.lifetime = oldSpell.lifetime;
        newSpell.size = oldSpell.size;
        newSpell.instances = oldSpell.instances;
        newSpell.speed = oldSpell.speed;
        newSpell.unique = oldSpell.unique;
        newSpell.spellSlot = oldSpell.spellSlot;
        newSpell.upgradeLimit = oldSpell.upgradeLimit;
        newSpell.SetOriginShape(oldSpell.GetOriginShape());
        newSpell.SetOriginElement(oldSpell.GetOriginElement());
        newSpell.ModifyElement(oldSpell.element, true);
        newSpell.ModifyShape(oldSpell.shape, true);
        newSpell.cooldownTime = GetTypeCooldown(newSpell);
    }

    private float GetTypeCooldown(Spell spell)
    {
        switch (spell.spellType)
        {
            default:
            case SpellType.Projectile:
                switch (spell.shape)
                {
                    default:
                    case 0:
                        return projectile0Cooldown;
                    case 1:
                        return projectile1Cooldown;
                    case 2:
                        return projectile2Cooldown;
                }
            case SpellType.AOE:
                switch (spell.shape)
                {
                    default:
                    case 0:
                        return AoECooldown0;
                    case 1:
                        return AoECooldown1;
                    case 2:
                        return AoECooldown2;
                }
            case SpellType.Melee:
                switch (spell.shape)
                {
                    default:
                    case 0:
                        return meleeCooldown0;
                    case 1:
                        return meleeCooldown1;
                    case 2:
                        return meleeCooldown2;
                }
            case SpellType.Movement:
                switch (spell.shape)
                {
                    default:
                    case 0:
                        return movementCooldown0;
                    case 1:
                        return movementCooldown1;
                    case 2:
                        return movementCooldown2;
                }
            case SpellType.Shield:
                switch (spell.shape)
                {
                    default:
                    case 0:
                        return shieldCooldown0;
                    case 1:
                        return shieldCooldown1;
                    case 2:
                        return shieldCooldown2;
                }
            case SpellType.Heal:
                switch (spell.shape)
                {
                    default:
                    case 0:
                        return healCooldown0;
                    case 1:
                        return healCooldown1;
                    case 2:
                        return healCooldown2;
                }
            case SpellType.Summon:
                switch (spell.shape)
                {
                    default:
                    case 0:
                        return summonCooldown0;
                    case 1:
                        return summonCooldown1;
                    case 2:
                        return summonCooldown2;
                }
        }
    }
}
