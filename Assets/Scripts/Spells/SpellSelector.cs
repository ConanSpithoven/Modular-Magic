using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSelector : MonoBehaviour
{
    [SerializeField] private Spell spell = null;

    void OnTriggerStay(Collider col){
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            col.GetComponent<SpellInventory>().SetSpellSlot(spell, 1);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            col.GetComponent<SpellInventory>().SetSpellSlot(spell, 2);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            col.GetComponent<SpellInventory>().SetSpellSlot(spell, 3);
        }
    }
}
