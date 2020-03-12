using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSpell : Spell
{
    private enum SpellShape { instant, dot, percent }
    private SpellShape variant = default;
    private GameObject caster = default;
    private SpellInventory spellInventory = default;

    void Update()
    {
        if (variant == SpellShape.instant)
        {
            transform.localPosition += new Vector3(0f, -0.1f, 0f);
            transform.localScale += new Vector3(0.05f * Time.deltaTime, 0f, 0.05f * Time.deltaTime);
        }
    }

    public void SetSpellInventory(SpellInventory spellInventory)
    {
        this.spellInventory = spellInventory;
    }

    public void SetCaster(GameObject caster)
    {
        this.caster = caster;
    }

    public void Activate()
    {
        variant = (SpellShape)shape;
        switch (variant)
        {
            default:
            case SpellShape.instant:
                damage += (speed + size) * 0.2f;
                damage *= instances;
                transform.localPosition = new Vector3(0f, 1.5f, 0f);
                if (caster.TryGetComponent(out PlayerManager player))
                {
                    player.Heal(damage);
                }
                break;
            case SpellShape.dot:
                damage += size;
                instances *= 5;
                if (caster.TryGetComponent(out PlayerManager playerDOT))
                {
                    playerDOT.DOTHeal(damage, instances, (1f / speed));
                }
                break;
            case SpellShape.percent:
                damage += (speed + size) * 0.2f;
                damage *= instances;
                damage = Mathf.Clamp(damage, 0f, 100f);
                if (caster.TryGetComponent(out PlayerManager playerPercent))
                {
                    playerPercent.PercentileHeal(damage);
                }
                break;
        }
        if (!spellInventory.GetCooldownStatus(spellSlot))
            spellInventory.StartCooldown(spellSlot);
        Destroy(gameObject, lifetime);
    }
}
