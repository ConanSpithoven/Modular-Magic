using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellInventory : MonoBehaviour
{
    private enum Caster { Player, Enemy, Summon };

    [SerializeField] private Caster casterType = default;
    [SerializeField] private Spell slotOne = null;
    [SerializeField] private Spell slotTwo = null;
    [SerializeField] private Spell slotThree = null;
    private bool targetFound = false;
    private int attacks = 1;
    private int attack = 0;
    private SpellManager spellManager;
    private Transform target = default;

    private void Awake()
    {
        spellManager = GetComponent<SpellManager>();

        if (slotOne != null)
        {
            slotOne.onCooldown = false;
            attacks++;
        }
        if (slotTwo != null)
        {
            slotTwo.onCooldown = false;
            attacks++;
        }
        if (slotThree != null)
        {
            slotThree.onCooldown = false;
            attacks++;
        }
    }

    private void Update()
    {
        switch (casterType)
        {
            case Caster.Player:
                if ((Input.GetKeyDown(KeyCode.Alpha1) || Input.GetMouseButtonDown(0)) && slotOne != null && !slotOne.onCooldown)
                {
                    spellManager.ActivateSpell(slotOne, 1);
                }
                if ((Input.GetKeyDown(KeyCode.Alpha2) || Input.GetMouseButtonDown(1)) && slotTwo != null && !slotTwo.onCooldown)
                {
                    spellManager.ActivateSpell(slotTwo, 2);
                }
                if ((Input.GetKeyDown(KeyCode.Alpha3) || Input.GetMouseButtonDown(2)) && slotThree != null && !slotThree.onCooldown)
                {
                    spellManager.ActivateSpell(slotThree, 3);
                }
                break;
            case Caster.Summon:
                if (targetFound && slotOne != null && !slotOne.onCooldown && attack == 1)
                {
                    spellManager.ActivateSpell(slotOne, 1);
                    StartCoroutine("GeneralCooldown");
                    attack = 0;
                }
                if (targetFound && slotTwo != null && !slotTwo.onCooldown && attack == 2)
                {
                    spellManager.ActivateSpell(slotTwo, 2);
                    StartCoroutine("GeneralCooldown");
                    attack = 0;
                }
                if (targetFound && slotThree != null && !slotThree.onCooldown && attack == 3)
                {
                    spellManager.ActivateSpell(slotThree, 3);
                    StartCoroutine("GeneralCooldown");
                    attack = 0;
                }
                break;
        }
    }

    public void SetSpellSlot(Spell spell, int spellSlot)
    {
        switch (spellSlot)
        {
            case 1:
                slotOne = spell;
                slotOne.onCooldown = false;
                break;
            case 2:
                slotTwo = spell;
                slotTwo.onCooldown = false;
                break;
            case 3:
                slotThree = spell;
                slotThree.onCooldown = false;
                break;
        }
    }

    public void StartCooldown(int spellSlot)
    {
        switch (spellSlot)
        {
            case 1:
                StartCoroutine("Cooldown", slotOne);
                break;
            case 2:
                StartCoroutine("Cooldown", slotTwo);
                break;
            case 3:
                StartCoroutine("Cooldown", slotThree);
                break;
            default:
                break;
        }
    }

    private IEnumerator Cooldown(Spell spell)
    {
        spell.onCooldown = true;
        yield return new WaitForSeconds(spell.cooldownTime);
        spell.onCooldown = false;
    }

    public bool GetCooldownStatus(int spellSlot)
    {
        switch (spellSlot)
        {
            case 1:
                if (slotOne != null)
                    return slotOne.onCooldown;
                else
                    return true;
            case 2:
                if (slotTwo != null)
                    return slotTwo.onCooldown;
                else
                    return true;
            case 3:
                if (slotThree != null)
                    return slotThree.onCooldown;
                else
                    return true;
            default:
                return false;
        }
    }

    public void SetTargetFound(bool status)
    {
        targetFound = status;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        spellManager.SetTarget(target);
    }

    public Transform GetTarget()
    {
        return target;
    }

    public int GetCasterType()
    {
        switch (casterType)
        {
            default:
            case Caster.Player:
                return 1;
            case Caster.Enemy:
                return 2;
            case Caster.Summon:
                return 3;
        }
    }

    public void Attack() 
    {
        attack = Random.Range(1, attacks);
        if (GetCooldownStatus(attack))
        {
            Attack();
        }
    }
}
