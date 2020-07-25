using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcheTypeSetter : MonoBehaviour
{
    [SerializeField] private int slot = 1;
    [SerializeField] private SpellType spellType;

    public void SetArcheType()
    {
        GameManager.instance.ArcheTypeSwap(slot, spellType);
    }
}
