using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellFormulaManager : MonoBehaviour
{
    #region Singleton

    public static SpellFormulaManager instance;
    private SpellInventory spellInventory;
    private Inventory inventory;

    private void Awake()
    {
        instance = this;
        inventory = Inventory.instance;
    }

    private void OnDrawGizmosSelected()
    {
        if (spellInventory == null)
            spellInventory = FindObjectOfType<PlayerManager>().GetSpellInventory();
    }
    #endregion

    private Spell selectedSpell = null;
    
    public void SelectSpell(int slot)
    {
        selectedSpell = spellInventory.GetSpell(slot);
        //activate spell formula UI(slot, selectedSpell.GetUpgradeLimit())
    }

    public void Add(Pattern pattern)
    {
        if (selectedSpell.AddPattern(pattern))
        {
            if (inventory.onItemChangedCallback != null)
            {
                inventory.onItemChangedCallback.Invoke();
            }
            //patternUI.onItemChangedCallback.Invoke();
        }
    }

    public void Remove(Pattern pattern)
    {
        if (inventory.Add(pattern))
        {
            selectedSpell.RemovePattern(pattern);
            if (inventory.onItemChangedCallback != null)
            {
                inventory.onItemChangedCallback.Invoke();
            }
            //patternUI.onItemChangedCallback.Invoke();
        }
    }
}
