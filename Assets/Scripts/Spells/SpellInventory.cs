using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

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

    private void Start()
    {
        if (casterType == Caster.Player)
        {
            EquipmentManager.instance.onEquipmentChanged += onEquipmentChange;
            PatternManager.instance.onPatternChanged += OnPatternChange;
        }
    }

    private void Update()
    {
        switch (casterType)
        {
            case Caster.Player:
                if (EventSystem.current.IsPointerOverGameObject()) return;

                if ((Input.GetKey(KeyCode.Alpha1) || Input.GetMouseButton(0)) && slotOne != null && !slotOne.onCooldown)
                {
                    spellManager.ActivateSpell(slotOne, 1);
                }
                if ((Input.GetKey(KeyCode.Alpha2) || Input.GetMouseButton(1)) && slotTwo != null && !slotTwo.onCooldown)
                {
                    spellManager.ActivateSpell(slotTwo, 2);
                }
                if ((Input.GetKey(KeyCode.Alpha3) || Input.GetMouseButton(2)) && slotThree != null && !slotThree.onCooldown)
                {
                    spellManager.ActivateSpell(slotThree, 3);
                }
                break;
            case Caster.Summon:
                if (targetFound && slotOne != null && !slotOne.onCooldown && attack == 1)
                {
                    spellManager.ActivateSpell(slotOne, 1);
                    attack = 0;
                }
                if (targetFound && slotTwo != null && !slotTwo.onCooldown && attack == 2)
                {
                    spellManager.ActivateSpell(slotTwo, 2);
                    attack = 0;
                }
                if (targetFound && slotThree != null && !slotThree.onCooldown && attack == 3)
                {
                    spellManager.ActivateSpell(slotThree, 3);
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
        float totalCooldownTime = ((((spell.power * 0.2f - 0.2f) * spell.cooldownTime) + spell.lifetime + ((spell.size * 0.1f - 0.1f) * spell.cooldownTime) - ((spell.speed * 0.1f - 0.1f) * spell.cooldownTime)) * (spell.instances * 0.3f + 0.7f))/* * spell.cdr*/;
        totalCooldownTime = Mathf.Clamp(totalCooldownTime, 0.1f, 120f);
        yield return new WaitForSeconds(totalCooldownTime);
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
                return 0;
            case Caster.Enemy:
                return 1;
            case Caster.Summon:
                return 2;
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

    public Spell GetSpell(int slot)
    {
        switch (slot)
        {
            default:
            case 1:
                return slotOne;
            case 2:
                return slotTwo;
            case 3:
                return slotThree;
        }
    }

    public void onEquipmentChange(Equipment newItem, Equipment oldItem)
    {
        if (newItem != null)
        {
            //remove old equipment bonusses
            if (oldItem != null)
            {
                if (slotOne != null)
                    RemoveEquipEffect(slotOne, oldItem);
                if (slotTwo != null)
                    RemoveEquipEffect(slotTwo, oldItem);
                if (slotThree != null)
                    RemoveEquipEffect(slotThree, oldItem);
            }
            //add equipment bonusses
            if (slotOne != null)
                AddEquipEffect(slotOne, newItem);
            if (slotTwo != null)
                AddEquipEffect(slotTwo, newItem);
            if (slotThree != null)
                AddEquipEffect(slotThree, newItem);
        }
        else if(oldItem != null)
        {
            //remove old equipment bonusses
            if (slotOne != null)
                RemoveEquipEffect(slotOne, oldItem);
            if (slotTwo != null)
                RemoveEquipEffect(slotTwo, oldItem);
            if (slotThree != null)
                RemoveEquipEffect(slotThree, oldItem);
        }
    }

    private void AddEquipEffect(Spell slot, Equipment item)
    {
        slot.power += item.powerModifier;
        slot.lifetime += item.lifetimeModifier;
        slot.size += item.sizeModifier;
        slot.instances += item.instancesModifier;
        slot.speed += item.speedModifier;
        slot.unique += item.uniqueModifier;
    }

    private void RemoveEquipEffect(Spell slot, Equipment item)
    {
        slot.power -= item.powerModifier;
        slot.lifetime -= item.lifetimeModifier;
        slot.size -= item.sizeModifier;
        slot.instances -= item.instancesModifier;
        slot.speed -= item.speedModifier;
        slot.unique -= item.uniqueModifier;
    }

    public void OnPatternChange(Pattern newItem, Pattern oldItem, int formulaNumber)
    {
        if (oldItem != null)
        {
            Spell slot = GetSpell(formulaNumber);
            slot.power -= oldItem.powerModifier;
            slot.lifetime -= oldItem.lifetimeModifier;
            slot.size -= oldItem.sizeModifier;
            slot.instances -= oldItem.instancesModifier;
            slot.speed -= oldItem.speedModifier;
            slot.unique -= oldItem.uniqueModifier;
            switch (oldItem.patternType)
            {
                default:
                    break;
                case PatternType.Variant:
                    slot.ModifyShape(oldItem.shapeModifier, false);
                    break;
                case PatternType.Elemental:
                    slot.ModifyElement(oldItem.elementModifier, false);
                    break;
            }
        }
        if (newItem != null)
        {
            Spell slot = GetSpell(formulaNumber);
            slot.power += newItem.powerModifier;
            slot.lifetime += newItem.lifetimeModifier;
            slot.size += newItem.sizeModifier;
            slot.instances += newItem.instancesModifier;
            slot.speed += newItem.speedModifier;
            slot.unique += newItem.uniqueModifier;
            switch (newItem.patternType)
            {
                default:
                    break;
                case PatternType.Variant:
                    slot.ModifyShape(newItem.shapeModifier, true);
                    break;
                case PatternType.Elemental:
                    slot.ModifyElement(newItem.elementModifier, true);
                    break;
            }
        }
    }
}
