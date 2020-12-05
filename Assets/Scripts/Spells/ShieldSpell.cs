using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSpell : Spell
{
    private enum SpellShape { barrier, shield, deploy }
    private SpellShape variant = default;
    private Transform caster = default;
    private float currentSize = 0f;
    private float currentDistance = 0f;
    private SpellInventory spellInventory = default;
    private bool setup = false;
    private Vector3 startPos = default;

    private void Update()
    {
        if (variant == SpellShape.shield)
        {
            if (setup)
            {
                transform.RotateAround(caster.position, Vector3.up, (speed * 35f) * Time.deltaTime);
                return;
            }
            else
            {
                Setup();
            }
        }
        else if (variant == SpellShape.deploy)
        {
            if (!setup)
                Setup();
            return;
        }
    }

    public void SetSpellInventory(SpellInventory spellInventory)
    {
        this.spellInventory = spellInventory;
    }

    public void SetCaster(Transform caster)
    {
        this.caster = caster;
    }

    public void Activate()
    {
        variant = (SpellShape)shape;
        switch (variant)
        {
            case SpellShape.barrier:
                power += (speed + size) * 0.2f;
                power *= instances;
                caster.GetComponent<PlayerManager>().SetShield(power, element, gameObject);
                StartCoroutine("ShieldDeactivator");
                break;
            case SpellShape.shield:
                power *= 1.5f;
                transform.localScale *= 0;
                Destroy(gameObject, lifetime);
                break;
            case SpellShape.deploy:
                transform.localScale *= 0;
                size *= 2;
                startPos = caster.position;
                Destroy(gameObject, lifetime);
                break;
            default:
                break;
        }
        if(!spellInventory.GetCooldownStatus(spellSlot))
            spellInventory.StartCooldown(spellSlot);
    }

    private void Setup()
    {
        if (variant == SpellShape.shield)
        {
            if (currentSize >= size && Vector3.Distance(caster.position, transform.position) >= size)
            {
                setup = true;
                return;
            }
            if (currentSize < size)
                SetupSize();
            if (Vector3.Distance(caster.position, transform.position) < size)
                SetupDistance();
        }
        else if (variant == SpellShape.deploy)
        {
            if (currentSize >= size && Vector3.Distance(startPos, transform.position) >= size)
            {
                setup = true;
                return;
            }
            if (currentSize < size)
                SetupSize();
            if (Vector3.Distance(startPos, transform.position) < size)
                SetupDistance();
        }
    }

    private void SetupSize() 
    {
        currentSize += 2f * speed * Time.deltaTime;
        float sizelimited = Mathf.Clamp(currentSize, 0f, size);
        if (variant == SpellShape.shield || variant == SpellShape.deploy)
        {
            transform.localScale = new Vector3(sizelimited, sizelimited, sizelimited);
        }
    }

    private void SetupDistance()
    {
        currentDistance += speed * Time.deltaTime;
        float distanceLimited = Mathf.Clamp(currentDistance, 0f, size);
        transform.Translate(new Vector3(0f, 0f, distanceLimited * 0.1f));
    }

    private IEnumerator ShieldDeactivator()
    {
        yield return new WaitForSeconds(lifetime);
        caster.GetComponent<PlayerManager>().RemoveShield();
    }

    private void OnCollisionEnter(Collision col)
    {
        if ((col.gameObject.CompareTag("Enemy_Attack_Spell") && gameObject.CompareTag("Shield_Spell")) || (col.gameObject.CompareTag("Attack_Spell") && gameObject.CompareTag("Enemy_Shield_Spell")))
        {
            col.gameObject.GetComponent<ProjectileSpell>().ReducePower(power, element);
        }
    }
}
