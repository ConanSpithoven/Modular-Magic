using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pattern", menuName = "Inventory/Pattern")]
public class Pattern : Item
{
    public PatternType patternType;

    public float powerModifier = 0;
    public float lifetimeModifier = 0;
    public float sizeModifier = 0;
    public int instancesModifier = 0;
    public float speedModifier = 0;
    public int uniqueModifier = 0;
    public float cooldownReductionModifier = 0f;
    public int upgradeLimitModifier = 0;
    public Element elementModifier = default;
    public int shapeModifier = 0;

    public override void Use()
    {
        base.Use();
        PatternManager.instance.Equip(this);
    }
}
