using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellDetails : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI CooldownStat;
    [SerializeField] private TextMeshProUGUI ElementStat;
    [SerializeField] private TextMeshProUGUI PowerStat;
    [SerializeField] private TextMeshProUGUI LifetimeStat;
    [SerializeField] private TextMeshProUGUI SizeStat;
    [SerializeField] private TextMeshProUGUI InstancesStat;
    [SerializeField] private TextMeshProUGUI SpeedStat;
    [SerializeField] private TextMeshProUGUI UpgradeLimitStat;
    [SerializeField] private TextMeshProUGUI UniqueStat;
    
    [SerializeField] private SpellInventory spellInventory;
    private PatternManager patternManager;

    private void Awake()
    {
        if (spellInventory == null)
        {
            spellInventory = FindObjectOfType<SpellInventory>().transform.GetComponent<SpellInventory>();
        }
        if (patternManager == null)
        {
            patternManager = PatternManager.instance;
        }
        UpdateSpell(patternManager.activeFormula);
    }

    private void Start()
    {
        patternManager = PatternManager.instance;
        patternManager.onPatternChanged += PatternChanged;
        patternManager.onFormulaChanged += UpdateSpell;
        patternManager.onUpgradeLimitChanged += UpgradeLimitChanged;
    }

    private void SetStats(Spell spell)
    {
        //cooldown
        CooldownStat.text = "Cooldown: " + spell.cooldownTime;
        //shape and icon
        SetTypes(spell);
        //element
        ElementStat.text = "Element: " + spell.element.ElementName;
        //power
        PowerStat.text = "Power: " + spell.power;
        //lifetime
        LifetimeStat.text = "Duration: " + spell.lifetime;
        //size
        SizeStat.text = "Size: " + spell.size;
        //instances
        InstancesStat.text = "Multicasts: " + spell.instances;
        //speed
        SpeedStat.text = "Speed: " + spell.speed;
        //upgradelimit
        UpgradeLimitStat.text = "Expansion limit: " + spell.upgradeLimit;
        //unique
        UniqueStat.text = "Evolution: " + spell.unique;
    }

    //TODO add element later on, possibly instances/evolution up to a certain point. depends on art limitations
    private void SetTypes(Spell spell)
    {
        switch (spell.spellType)
        {
            case SpellType.Projectile:
                switch (spell.shape)
                {
                    case 0:
                        itemName.text =  spell.element.ElementName + " Missile";
                        itemType.text = "Projectile, Missile";
                        icon.sprite = Resources.Load<Sprite>("SpellIcons/Projectile/Missile");
                        break;
                    case 1:
                        itemName.text = spell.element.ElementName + " Beam";
                        itemType.text = "Projectile, Beam";
                        icon.sprite = Resources.Load<Sprite>("SpellIcons/Projectile/Beam");
                        break;
                    case 2:
                        itemName.text = spell.element.ElementName + " Bolt";
                        itemType.text = "Projectile, Chain";
                        icon.sprite = Resources.Load<Sprite>("SpellIcons/Projectile/Chain");
                        break;
                }
                break;
            case SpellType.AOE:
                switch (spell.shape)
                {
                    case 0:
                        itemName.text = spell.element.ElementName + " Wave";
                        itemType.text = "AoE, Explosion";
                        icon.sprite = Resources.Load<Sprite>("SpellIcons/AoE/Orb");
                        break;
                    case 1:
                        itemName.text = spell.element.ElementName + " Star";
                        itemType.text = "AoE, Orbiter";
                        icon.sprite = Resources.Load<Sprite>("SpellIcons/AoE/Orbit");
                        break;
                    case 2:
                        itemName.text = spell.element.ElementName + " Detonation";
                        itemType.text = "AoE, Detonation";
                        icon.sprite = Resources.Load<Sprite>("SpellIcons/AoE/Point");
                        break;
                }
                break;
            case SpellType.Melee:
                switch (spell.shape)
                {
                    case 0:
                        itemName.text = spell.element.ElementName + " Sword";
                        itemType.text = "Melee, Sword";
                        icon.sprite = Resources.Load<Sprite>("SpellIcons/Melee/Sword");
                        break;
                    case 1:
                        itemName.text = spell.element.ElementName + " Spear";
                        itemType.text = "Melee, Spear";
                        icon.sprite = Resources.Load<Sprite>("SpellIcons/Melee/Spear");
                        break;
                    case 2:
                        itemName.text = spell.element.ElementName + " Axe";
                        itemType.text = "Melee, Axe";
                        icon.sprite = Resources.Load<Sprite>("SpellIcons/Melee/Axe");
                        break;
                }
                break;
            case SpellType.Movement:
                switch (spell.shape)
                {
                    case 0:
                        itemName.text = spell.element.ElementName + " Dash";
                        itemType.text = "Movement, Dash";
                        icon.sprite = Resources.Load<Sprite>("SpellIcons/Movement/Dash");
                        break;
                    case 1:
                        itemName.text = spell.element.ElementName + " Teleportation";
                        itemType.text = "Movement, Teleport";
                        icon.sprite = Resources.Load<Sprite>("SpellIcons/Movement/Teleport");
                        break;
                    case 2:
                        itemName.text = spell.element.ElementName + " Force";
                        itemType.text = "Movement, Push";
                        icon.sprite = Resources.Load<Sprite>("SpellIcons/Movement/Push");
                        break;
                }
                break;
            case SpellType.Heal:
                switch (spell.shape)
                {
                    case 0:
                        itemName.text = spell.element.ElementName + " Healing";
                        itemType.text = "Heal, Instant";
                        icon.sprite = Resources.Load<Sprite>("SpellIcons/Heal/Instant");
                        break;
                    case 1:
                        itemName.text = spell.element.ElementName + " Restoration";
                        itemType.text = "Heal, Regeneration";
                        icon.sprite = Resources.Load<Sprite>("SpellIcons/Heal/DoT");
                        break;
                    case 2:
                        itemName.text = "Blessing of " + spell.element.ElementName;
                        itemType.text = "Heal, Percentile";
                        icon.sprite = Resources.Load<Sprite>("SpellIcons/Heal/Percent");
                        break;
                }
                break;
            case SpellType.Shield:
                switch (spell.shape)
                {
                    case 0:
                        itemName.text = spell.element.ElementName + " Barrier";
                        itemType.text = "Shield, Barrier";
                        icon.sprite = Resources.Load<Sprite>("SpellIcons/Shield/Barrier");
                        break;
                    case 1:
                        itemName.text = "Orbiting Shield of " + spell.element.ElementName;
                        itemType.text = "Shield, Orbiter";
                        icon.sprite = Resources.Load<Sprite>("SpellIcons/Shield/Shield");
                        break;
                    case 2:
                        itemName.text = "Wall of " + spell.element.ElementName;
                        itemType.text = "Shield, Deployable";
                        icon.sprite = Resources.Load<Sprite>("SpellIcons/Shield/Deploy");
                        break;
                }
                break;
            case SpellType.Summon:
                switch (spell.shape)
                {
                    case 0:
                        itemName.text = spell.element.ElementName + " Hawk";
                        itemType.text = "Summoning, Homing";
                        icon.sprite = Resources.Load<Sprite>("SpellIcons/Summon/Chaser");
                        break;
                    case 1:
                        itemName.text = spell.element.ElementName + " Lich";
                        itemType.text = "Summoning, Caster";
                        icon.sprite = Resources.Load<Sprite>("SpellIcons/Summon/Ranged");
                        break;
                    case 2:
                        itemName.text = "Soldier of " + spell.element.ElementName;
                        itemType.text = "Summoning, Soldier";
                        icon.sprite = Resources.Load<Sprite>("SpellIcons/Summon/Melee");
                        break;
                }
                break;
        }
        spellInventory.SetIcon(spell.GetSpellSlot(), icon.sprite);
    }

    private void PatternChanged(Pattern newPattern, Pattern oldPattern, int formulaNumber)
    {
        UpdateSpell(formulaNumber);
    }

    private void UpgradeLimitChanged(int newLimit, int formulaNumber)
    {
        UpdateSpell(formulaNumber);
    }

    public void UpdateSpell(int slot)
    {
        if(spellInventory != null)
            SetStats(spellInventory.GetSpell(slot));
    }
}
