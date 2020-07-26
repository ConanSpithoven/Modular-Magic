using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    #region Singleton
    public static PatternManager instance;
    public int activeFormula = 1;

    void Awake()
    {
        instance = this;
    }
    #endregion

    public List<Pattern> currentPattern1;
    public List<Pattern> currentPattern2;
    public List<Pattern> currentPattern3;

    private Spell slotOne;
    private Spell slotTwo;
    private Spell slotThree;

    public delegate void OnPatternChanged(Pattern newPattern, Pattern oldPattern, int formulaNumber);
    public OnPatternChanged onPatternChanged;

    public delegate void OnFormulaChanged(int formulaNumber);
    public OnFormulaChanged onFormulaChanged;

    public delegate void OnUpgradeLimitChanged(int newLimit, int formulaNumber);
    public OnUpgradeLimitChanged onUpgradeLimitChanged;

    Inventory inventory;
    private SpellInventory spellInventory;

    void Start()
    {
        spellInventory = FindObjectOfType<PlayerManager>().transform.GetComponent<SpellInventory>();
        inventory = Inventory.instance;

        slotOne = spellInventory.GetSpell(1);
        slotTwo = spellInventory.GetSpell(2);
        slotThree = spellInventory.GetSpell(3);

        currentPattern1 = new List<Pattern>(slotOne.GetUpgradeLimit());
        currentPattern2 = new List<Pattern>(slotTwo.GetUpgradeLimit());
        currentPattern3 = new List<Pattern>(slotThree.GetUpgradeLimit());
    }

    public void Equip(Pattern newItem)
    {
        Pattern oldItem;
        switch (activeFormula)
        {
            case 1:
                switch (newItem.patternType)
                {
                    case PatternType.Empowerment:
                        int upgradeCount = 0;
                        foreach (Pattern pattern in currentPattern1)
                        {
                            if (pattern.patternType == PatternType.Empowerment)
                            {
                                upgradeCount++;
                            }
                        }
                        if (upgradeCount < slotOne.GetUpgradeLimit())
                        {
                            currentPattern1.Add(newItem);
                            inventory.Remove(newItem);
                        }
                        else 
                        {
                            return;
                        }
                        break;
                    case PatternType.Elemental:
                        foreach (Pattern pattern in currentPattern1)
                        {
                            if (pattern.patternType == PatternType.Elemental)
                            {
                                oldItem = pattern;
                                currentPattern1.Remove(oldItem);
                                inventory.Add(oldItem);
                                currentPattern1.Add(newItem);
                                inventory.Remove(newItem);
                                if (onPatternChanged != null)
                                {
                                    onPatternChanged.Invoke(newItem, oldItem, activeFormula);
                                }
                                return;
                            }
                        }
                        currentPattern1.Add(newItem);
                        inventory.Remove(newItem);
                        break;
                    case PatternType.Variant:
                        foreach (Pattern pattern in currentPattern1)
                        {
                            if (pattern.patternType == PatternType.Variant)
                            {
                                oldItem = pattern;
                                currentPattern1.Remove(oldItem);
                                inventory.Add(oldItem);
                                currentPattern1.Add(newItem);
                                inventory.Remove(newItem);
                                if (onPatternChanged != null)
                                {
                                    onPatternChanged.Invoke(newItem, oldItem, activeFormula);
                                }
                                return;
                            }
                        }
                        currentPattern1.Add(newItem);
                        inventory.Remove(newItem);
                        break;
                }
                break;
            case 2:
                switch (newItem.patternType)
                {
                    case PatternType.Empowerment:
                        int upgradeCount = 0;
                        foreach (Pattern pattern in currentPattern2)
                        {
                            if (pattern.patternType == PatternType.Empowerment)
                            {
                                upgradeCount++;
                            }
                        }
                        if (upgradeCount < slotTwo.GetUpgradeLimit())
                        {
                            currentPattern2.Add(newItem);
                            inventory.Remove(newItem);
                        }
                        else
                        {
                            return;
                        }
                        break;
                    case PatternType.Elemental:
                        foreach (Pattern pattern in currentPattern2)
                        {
                            if (pattern.patternType == PatternType.Elemental)
                            {
                                oldItem = pattern;
                                currentPattern2.Remove(oldItem);
                                inventory.Add(oldItem);
                                currentPattern2.Add(newItem);
                                inventory.Remove(newItem);
                                if (onPatternChanged != null)
                                {
                                    onPatternChanged.Invoke(newItem, oldItem, activeFormula);
                                }
                                return;
                            }
                        }
                        currentPattern2.Add(newItem);
                        inventory.Remove(newItem);
                        break;
                    case PatternType.Variant:
                        foreach (Pattern pattern in currentPattern2)
                        {
                            if (pattern.patternType == PatternType.Variant)
                            {
                                oldItem = pattern;
                                currentPattern2.Remove(oldItem);
                                inventory.Add(oldItem);
                                currentPattern2.Add(newItem);
                                inventory.Remove(newItem);
                                if (onPatternChanged != null)
                                {
                                    onPatternChanged.Invoke(newItem, oldItem, activeFormula);
                                }
                                return;
                            }
                        }
                        currentPattern2.Add(newItem);
                        inventory.Remove(newItem);
                        break;
                }
                break;
            case 3:
                switch (newItem.patternType)
                {
                    case PatternType.Empowerment:
                        int upgradeCount = 0;
                        foreach (Pattern pattern in currentPattern3)
                        {
                            if (pattern.patternType == PatternType.Empowerment)
                            {
                                upgradeCount++;
                            }
                        }
                        if (upgradeCount < slotThree.GetUpgradeLimit())
                        {
                            currentPattern3.Add(newItem);
                            inventory.Remove(newItem);
                        }
                        else
                        {
                            return;
                        }
                        break;
                    case PatternType.Elemental:
                        foreach (Pattern pattern in currentPattern3)
                        {
                            if (pattern.patternType == PatternType.Elemental)
                            {
                                oldItem = pattern;
                                currentPattern3.Remove(oldItem);
                                inventory.Add(oldItem);
                                currentPattern3.Add(newItem);
                                inventory.Remove(newItem);
                                if (onPatternChanged != null)
                                {
                                    onPatternChanged.Invoke(newItem, oldItem, activeFormula);
                                }
                                return;
                            }
                        }
                        currentPattern3.Add(newItem);
                        inventory.Remove(newItem);
                        break;
                    case PatternType.Variant:
                        foreach (Pattern pattern in currentPattern3)
                        {
                            if (pattern.patternType == PatternType.Variant)
                            {
                                oldItem = pattern;
                                currentPattern3.Remove(oldItem);
                                inventory.Add(oldItem);
                                currentPattern3.Add(newItem);
                                inventory.Remove(newItem);
                                if (onPatternChanged != null)
                                {
                                    onPatternChanged.Invoke(newItem, oldItem, activeFormula);
                                }
                                return;
                            }
                        }
                        currentPattern3.Add(newItem);
                        inventory.Remove(newItem);
                        break;
                }
                break;
        }
        if (onPatternChanged != null)
        {
            onPatternChanged.Invoke(newItem, null, activeFormula);
        }
    }

    public void UnEquip(Pattern oldItem)
    {
        switch (activeFormula)
        {
            case 1:
                currentPattern1.Remove(oldItem);
                if (!inventory.full)
                {
                    inventory.Add(oldItem);
                }
                else
                {
                    DropItem(oldItem);
                }
                break;
            case 2:
                currentPattern2.Remove(oldItem);
                if (!inventory.full)
                {
                    inventory.Add(oldItem);
                }
                else
                {
                    DropItem(oldItem);
                }
                break;
            case 3:
                currentPattern3.Remove(oldItem);
                if (!inventory.full)
                {
                    inventory.Add(oldItem);
                }
                else
                {
                    DropItem(oldItem);
                }
                break;
        }
        if (onPatternChanged != null)
        {
            onPatternChanged.Invoke(null, oldItem, activeFormula);
        }
    }

    //on change upgradelimit, re-check empowerment count vs upgradelimit, unequip all over limit
    public void UpgradeLimitChange(int oldLimit, int newLimit, int spellSlot)
    {
        switch (spellSlot)
        {
            case 1:
                if (newLimit < oldLimit)
                {
                    //unequip all patterns in slots above newlimit
                    for (int i = (currentPattern1.Count-1); i >= newLimit; i--)
                    {
                        if (currentPattern1[i].patternType == PatternType.Empowerment)
                        {
                            UnEquip(currentPattern1[i]);
                        }
                    }
                }
                onUpgradeLimitChanged.Invoke(newLimit, 1);
                break;
            case 2:
                if (newLimit < oldLimit)
                {
                    //unequip all patterns in slots above newlimit
                    for (int i = (currentPattern2.Count - 1); i >= newLimit; i--)
                    {
                        if (currentPattern2[i].patternType == PatternType.Empowerment)
                        {
                            UnEquip(currentPattern2[i]);
                        }
                    }
                }
                onUpgradeLimitChanged.Invoke(newLimit, 1);
                break;
            case 3:
                if (newLimit < oldLimit)
                {
                    //unequip all patterns in slots above newlimit
                    for (int i = (currentPattern3.Count - 1); i >= newLimit; i--)
                    {
                        if (currentPattern3[i].patternType == PatternType.Empowerment)
                        {
                            UnEquip(currentPattern3[i]);
                        }
                    }
                }
                onUpgradeLimitChanged.Invoke(newLimit, 1);
                break;
        }
    }

    public void SetActiveFormula(int formulaNumber)
    {
        activeFormula = formulaNumber;
        onFormulaChanged.Invoke(formulaNumber);
    }

    private int GetEmpowermentCount(int formulaNumber)
    {
        int empowermentCount = 0;
        switch (formulaNumber)
        {
            case 1:
                foreach (Pattern pattern in currentPattern1)
                {
                    if (pattern.patternType == PatternType.Empowerment)
                    {
                        empowermentCount++;
                    }
                }
                break;
            case 2:
                foreach (Pattern pattern in currentPattern2)
                {
                    if (pattern.patternType == PatternType.Empowerment)
                    {
                        empowermentCount++;
                    }
                }
                break;
            case 3:
                foreach (Pattern pattern in currentPattern3)
                {
                    if (pattern.patternType == PatternType.Empowerment)
                    {
                        empowermentCount++;
                    }
                }
                break;
        }
        return empowermentCount;
    }

    public List<Pattern> GetEmpowermentPatterns(int formulaNumber)
    {
        List<Pattern> patterns = new List<Pattern>(GetEmpowermentCount(formulaNumber));
        switch (formulaNumber)
        {
            case 1:
                foreach (Pattern pattern in currentPattern1)
                {
                    if (pattern.patternType == PatternType.Empowerment)
                        patterns.Add(pattern);
                }
                break;
            case 2:
                foreach (Pattern pattern in currentPattern2)
                {
                    if (pattern.patternType == PatternType.Empowerment)
                        patterns.Add(pattern);
                }
                break;
            case 3:
                foreach (Pattern pattern in currentPattern3)
                {
                    if (pattern.patternType == PatternType.Empowerment)
                        patterns.Add(pattern);
                }
                break;
        }
        return patterns;
    }

    private void DropItem(Item oldItem)
    {
        GameObject droppedItem = new GameObject(oldItem.name);
        droppedItem.AddComponent<SpriteRenderer>();
        droppedItem.GetComponent<SpriteRenderer>().sprite = oldItem.worldIcon;
        droppedItem.AddComponent<BoxCollider>();
        droppedItem.AddComponent<ItemPickup>();
        droppedItem.GetComponent<ItemPickup>().item = oldItem;
        Vector3 playerPos = GameManager.instance.GetPlayer().transform.position;
        droppedItem.transform.position = new Vector3(playerPos.x, 0.5f, playerPos.z);
        droppedItem.transform.rotation = Quaternion.Euler(90, 0, 0);
        droppedItem.transform.localScale *= 0.5f;
    }
}
