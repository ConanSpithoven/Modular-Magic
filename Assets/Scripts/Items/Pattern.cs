using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pattern", menuName = "Inventory/Pattern")]
public class Pattern : Item
{
    public int shape = 3;
    public float damage = 0;
    public float lifetime = 0;
    public float size = 0;
    public int instances = 0;
    public float speed = 0;
    public int unique = 0;
    public Element element = default;

    public override void Use()
    {
        base.Use();
        //move to patternslot
        SpellFormulaManager.instance.Add(this);
        Inventory.instance.Remove(this);
    }
}
