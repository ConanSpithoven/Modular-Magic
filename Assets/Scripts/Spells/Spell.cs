using UnityEngine;

[CreateAssetMenu(fileName = "New spell")]
public class Spell : ScriptableObject
{
    [Header("General spell settings")]
    public SpellType spellType;
    public float cooldownTime;
    public bool onCooldown;
    public string shape;
    public float power;
    public float lifetime;
    public float size;
    public int instances;
    public float speed;
    public int unique;
    private int spellSlot;
    public Element element;
    [Header("AOESpell settings")]
    public GameObject aoe;
    [Header("ProjectileSpell settings")]
    public GameObject projectile;
    [Header("UtilitySpell settings")]
    public UtilityType utilityType;
    public GameObject utility;
    [Header("MeleeSpell settings")]
    public GameObject melee;
    [Header("SummoningSpell settings")]
    public GameObject summon;

    public void SetSpellSlot(int spellSlot)
    {
        this.spellSlot = spellSlot;
    }

    public int GetSpellSlot()
    {
        return spellSlot;
    }
}
