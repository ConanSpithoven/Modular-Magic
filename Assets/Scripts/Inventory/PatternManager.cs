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
        Debug.Log("adding " + newItem.name);
        Debug.Log(activeFormula);
        switch (activeFormula)
        {
            case 1:
                if (currentPattern1.Count < slotOne.GetUpgradeLimit())
                {
                    currentPattern1.Add(newItem);
                    inventory.Remove(newItem);
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
        Debug.Log("Unequipping" + oldItem.name);
        Debug.Log(activeFormula);
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
}
