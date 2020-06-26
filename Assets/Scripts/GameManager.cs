using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform startPos;
    private SpellInventory spellInventory;

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
    }
}
