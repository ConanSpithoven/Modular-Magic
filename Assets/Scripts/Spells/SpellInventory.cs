using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
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
    private Barhandler cooldown1;
    private Barhandler cooldown2;
    private Barhandler cooldown3;
    private Image cooldownIcon1;
    private Image cooldownIcon2;
    private Image cooldownIcon3;
    private float cooldowntime1;
    private float cooldowntime2;
    private float cooldowntime3;

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
            GameManager gameManager = GameManager.instance;
            cooldown1 = gameManager.GetCooldownObj(1).GetComponent<Barhandler>();
            cooldown2 = gameManager.GetCooldownObj(2).GetComponent<Barhandler>();
            cooldown3 = gameManager.GetCooldownObj(3).GetComponent<Barhandler>();
            cooldownIcon1 = gameManager.GetCooldownObj(1).GetComponent<Image>();
            cooldownIcon2 = gameManager.GetCooldownObj(2).GetComponent<Image>();
            cooldownIcon3 = gameManager.GetCooldownObj(3).GetComponent<Image>();
            cooldown1.SetValue(0,1);
            cooldown2.SetValue(0,1);
            cooldown3.SetValue(0,1);
        }
    }

    private void Update()
    {
        switch (casterType)
        {
            case Caster.Player:
                if (slotOne.onCooldown)
                {
                    if (cooldowntime1 <= 0)
                    {
                        slotOne.onCooldown = false;
                    }
                    else
                    {
                        cooldowntime1 -= Time.deltaTime;
                        cooldown1.SetValue(cooldowntime1, slotOne.cooldownTime);
                    }
                }
                if (slotTwo.onCooldown)
                {
                    if (cooldowntime2 <= 0)
                    {
                        slotTwo.onCooldown = false;
                    }
                    else
                    {
                        cooldowntime2 -= Time.deltaTime;
                        cooldown2.SetValue(cooldowntime2, slotTwo.cooldownTime);
                    }
                }
                if (slotThree.onCooldown)
                {
                    if (cooldowntime3 <= 0)
                    {
                        slotThree.onCooldown = false;
                    }
                    else
                    {
                        cooldowntime3 -= Time.deltaTime;
                        cooldown3.SetValue(cooldowntime3, slotThree.cooldownTime);
                    }
                }

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
                slotOne.onCooldown = true;
                cooldowntime1 = slotOne.cooldownTime;
                cooldown1.SetFull();
                break;
            case 2:
                slotTwo.onCooldown = true;
                cooldowntime2 = slotTwo.cooldownTime;
                cooldown2.SetFull();
                break;
            case 3:
                slotThree.onCooldown = true;
                cooldowntime3 = slotThree.cooldownTime;
                cooldown3.SetFull();
                break;
            default:
                break;
        }
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
        CalcCooldownTime(1);
        CalcCooldownTime(2);
        CalcCooldownTime(3);
    }

    private void AddEquipEffect(Spell slot, Equipment item)
    {
        slot.power += item.powerModifier;
        slot.lifetime += item.lifetimeModifier;
        slot.size += item.sizeModifier;
        slot.instances += item.instancesModifier;
        slot.speed += item.speedModifier;
        slot.unique += item.uniqueModifier;
        slot.upgradeLimit += item.upgradeLimitModifier;
    }

    private void RemoveEquipEffect(Spell slot, Equipment item)
    {
        slot.power -= item.powerModifier;
        slot.lifetime -= item.lifetimeModifier;
        slot.size -= item.sizeModifier;
        slot.instances -= item.instancesModifier;
        slot.speed -= item.speedModifier;
        slot.unique -= item.uniqueModifier;
        slot.upgradeLimit -= item.upgradeLimitModifier;
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
            if (oldItem.upgradeLimitModifier != 0)
            {
                slot.upgradeLimit -= oldItem.upgradeLimitModifier;
                PatternManager.instance.UpgradeLimitChange((slot.upgradeLimit + oldItem.upgradeLimitModifier), slot.upgradeLimit, formulaNumber);
                
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
            if (newItem.upgradeLimitModifier != 0)
            {
                slot.upgradeLimit += newItem.upgradeLimitModifier;
                PatternManager.instance.UpgradeLimitChange((slot.upgradeLimit - newItem.upgradeLimitModifier), slot.upgradeLimit, formulaNumber);
                
            }
        }
        CalcCooldownTime(formulaNumber);
    }

    public void CalcCooldownTime(int formulaNumber)
    {
        Spell spell = GetSpell(formulaNumber);
        float totalCooldownTime = spell.GetBaseCooldownTime() + ((((spell.power * 0.2f - 0.2f) * spell.GetBaseCooldownTime()) + ((spell.size * 0.1f - 0.1f) * spell.GetBaseCooldownTime()) - ((spell.speed * 0.1f - 0.1f) * spell.GetBaseCooldownTime())) * (spell.instances * 0.3f + 0.7f))/* * 1 - spell.cdr*/;
        spell.SetCooldown(totalCooldownTime);
    }

    public void SetIcon(int slot, Sprite sprite)
    {
        switch (slot)
        {
            case 1:
                if (cooldownIcon1 != null)
                    cooldownIcon1.sprite = sprite;
                break;
            case 2:
                if (cooldownIcon2 != null)
                    cooldownIcon2.sprite = sprite;
                break;
            case 3:
                if (cooldownIcon3 != null)
                    cooldownIcon3.sprite = sprite;
                break;
        }
    }
}
