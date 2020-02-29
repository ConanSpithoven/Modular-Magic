using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSpell : MonoBehaviour
{
    private Element element;
    private enum SpellShape { instant, dot, percent }
    private float lifetime = 0f;
    private SpellShape shape = default;
    private float damage = 0f;
    private int instances = 0;
    private float speed = 0f;
    private float size = 0f;
    private GameObject caster = default;
    private SpellInventory spellInventory = default;
    private int spellSlot = 1;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (shape == SpellShape.instant)
        {
            transform.localPosition += new Vector3(0f, -0.1f, 0f);
            transform.localScale += new Vector3(0.05f * Time.deltaTime, 0f, 0.05f * Time.deltaTime);
        }
    }

    public void SetSlot(int spellSlot)
    {
        this.spellSlot = spellSlot;
    }

    public void SetSpellInventory(SpellInventory spellInventory)
    {
        this.spellInventory = spellInventory;
    }

    public void SetElement(Element element)
    {
        this.element = element;
    }

    public void SetLifetime(float lifetime)
    {
        this.lifetime = lifetime;
    }

    public void SetShape(int shape)
    {
        this.shape = (SpellShape)shape;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetInstances(int instances)
    {
        this.instances = instances;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetSize(float size)
    {
        this.size = size;
    }

    public void SetCaster(GameObject caster)
    {
        this.caster = caster;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        switch (shape)
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
