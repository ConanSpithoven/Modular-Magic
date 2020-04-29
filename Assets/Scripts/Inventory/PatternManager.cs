using System.Collections;
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

    Inventory inventory;
    [SerializeField] private SpellInventory spellInventory;

    void Start()
    {
        inventory = Inventory.instance;
        slotOne = spellInventory.GetSpell(1);
        slotTwo = spellInventory.GetSpell(2);
        slotThree = spellInventory.GetSpell(3);

        currentPattern1 = new List<Pattern>(slotOne.GetUpgradeLimit());
        currentPattern2 = new List<Pattern>(slotTwo.GetUpgradeLimit());
        currentPattern3 = new List<Pattern>(slotThree.GetUpgradeLimit());
    }

    private void OnDrawGizmosSelected()
    {
        if (spellInventory == null)
            spellInventory = FindObjectOfType<PlayerManager>().transform.GetComponent<SpellInventory>();
    }

    public void Equip(Pattern newItem)
    {
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
                        Pattern oldItem;
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
                }
                break;
            case 2:
                if (currentPattern2.Count < slotTwo.GetUpgradeLimit())
                {
                    currentPattern2.Add(newItem);
                    inventory.Remove(newItem);
                }
                break;
            case 3:
                if (currentPattern3.Count < slotThree.GetUpgradeLimit())
                {
                    currentPattern3.Add(newItem);
                    inventory.Remove(newItem);
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
                inventory.Add(oldItem);
                break;
            case 2:
                currentPattern2.Remove(oldItem);
                inventory.Add(oldItem);
                break;
            case 3:
                currentPattern3.Remove(oldItem);
                inventory.Add(oldItem);
                break;
        }
        if (onPatternChanged != null)
        {
            onPatternChanged.Invoke(null, oldItem, activeFormula);
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
}
