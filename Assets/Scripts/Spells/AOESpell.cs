using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOESpell : Spell
{
    private enum SpellShape { orb, orbit, point }
    private float currentsize = 0f;
    private SpellShape variant = default;
    private SpellInventory spellInventory = default;
    private Transform caster = default;
    private float currentSize = 0f;
    private float currentDistance = 0f;
    private bool hit = true;

    private List<Collider> targets = new List<Collider>();

    void Update()
    {
        if (variant == SpellShape.orb || variant == SpellShape.point)
        {
            if (currentsize < size)
            {
                currentsize += 10f * speed * Time.deltaTime;
                float sizelimited = Mathf.Clamp(currentsize, 0f, size);
                transform.localScale = new Vector3(sizelimited, sizelimited, sizelimited);
            }
            else if (transform.localScale.y != 0.3f)
            {
                if (variant == SpellShape.orb)
                {
                    transform.SetParent(null, true);
                }
                transform.localScale = new Vector3(transform.localScale.x, 0.3f, transform.localScale.z);
                transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
                Destroy(gameObject, lifetime);
            }
            return;
        }
        else if (variant == SpellShape.orbit)
        {
            if (currentSize >= size && Vector3.Distance(caster.position, transform.position) >= 2f)
            {
                transform.RotateAround(caster.position, Vector3.up, (speed * 35f) * Time.deltaTime);
                return;
            }
            if (currentSize < size)
                SetupSize();
            if (Vector3.Distance(caster.position, transform.position) < 2f)
                SetupDistance();
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
        transform.localScale *= 0;
        switch (variant)
        {
            case SpellShape.orb:
                size *= 1.4f;
                size += 5f;
                transform.localPosition = new Vector3(0, transform.localPosition.y, 0);
                break;
            case SpellShape.orbit:
                lifetime *= 1.3f;
                lifetime += 5f;
                break;
            case SpellShape.point:
                size *= 0.8f;
                size += 3f;
                transform.SetParent(null, true);
                break;
        }
        if (!spellInventory.GetCooldownStatus(spellSlot))
            spellInventory.StartCooldown(spellSlot);
        if(variant == SpellShape.orbit)
            Destroy(gameObject, lifetime);
    }

    private void SetupSize()
    {
        currentSize += 2f * speed * Time.deltaTime;
        float sizelimited = Mathf.Clamp(currentSize, 0f, size);
        transform.localScale = new Vector3(sizelimited, sizelimited, sizelimited);
    }

    private void SetupDistance()
    {
        currentDistance += speed * Time.deltaTime;
        float distanceLimited = Mathf.Clamp(currentDistance, 0f, size);
        transform.Translate(new Vector3(0f, 0f, distanceLimited * 0.1f));
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!targets.Contains(col))
        {
            targets.Add(col);
        }
        if (hit)
        {
            int i = 0;
            foreach (Collider collider in targets)
            {
                if (targets[i] == null)
                {
                    targets.RemoveAt(i);
                }
                i++;
                if ((collider.gameObject.CompareTag("Enemy") || collider.gameObject.CompareTag("Enemy_Summon_Spell")) && gameObject.CompareTag("Attack_Spell"))
                {
                    if (collider.gameObject.TryGetComponent(out EnemyManager enemy))
                    {
                        enemy.Hit(power, element);
                    }
                    if (collider.gameObject.TryGetComponent(out SummoningSpell summon))
                    {
                        summon.ReducePower(power, element);
                    }
                    StartCoroutine("BeamDamageCooldown");
                }
                else if (((collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Summon_Spell")) && gameObject.CompareTag("Enemy_Attack_Spell")))
                {
                    if (collider.gameObject.TryGetComponent(out PlayerManager player))
                    {
                        player.Hit(power, element);
                    }
                    if (collider.gameObject.TryGetComponent(out SummoningSpell summon))
                    {
                        summon.ReducePower(power, element);
                    }
                    StartCoroutine("BeamDamageCooldown");
                }
                else if ((collider.gameObject.CompareTag("Shield_Spell") && gameObject.CompareTag("Enemy_Attack_Spell")) || (collider.gameObject.CompareTag("Enemy_Shield_Spell") && gameObject.CompareTag("Attack_Spell")))
                {
                    if (collider.gameObject.TryGetComponent(out ShieldSpell shield))
                    {
                        shield.ReducePower(power, element);
                    }
                    StartCoroutine("BeamDamageCooldown");
                }
                else if ((collider.gameObject.CompareTag("Enemy_Attack_Spell") && gameObject.CompareTag("Attack_Spell")) || (collider.gameObject.CompareTag("Attack_Spell") && gameObject.CompareTag("Enemy_Attack_Spell")))
                {
                    if (collider.gameObject.TryGetComponent(out ProjectileSpell projectile))
                    {
                        projectile.ReducePower(power, element);
                    }
                    StartCoroutine("BeamDamageCooldown");
                }
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (!targets.Contains(col))
        {
            targets.Add(col);
        }
        if (hit)
        {
            int i = 0;
            foreach (Collider collider in targets)
            {
                if (targets[i] == null)
                {
                    targets.RemoveAt(i);
                }
                i++;
                if ((collider.gameObject.CompareTag("Enemy") || collider.gameObject.CompareTag("Enemy_Summon_Spell")) && gameObject.CompareTag("Attack_Spell"))
                {
                    if (collider.gameObject.TryGetComponent(out EnemyManager enemy))
                    {
                        enemy.Hit(power, element);
                    }
                    if (collider.gameObject.TryGetComponent(out SummoningSpell summon))
                    {
                        summon.ReducePower(power, element);
                    }
                    StartCoroutine("BeamDamageCooldown");
                }
                else if (((collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Summon_Spell")) && gameObject.CompareTag("Enemy_Attack_Spell")))
                {
                    if (collider.gameObject.TryGetComponent(out PlayerManager player))
                    {
                        player.Hit(power, element);
                    }
                    if (collider.gameObject.TryGetComponent(out SummoningSpell summon))
                    {
                        summon.ReducePower(power, element);
                    }
                    StartCoroutine("BeamDamageCooldown");
                }
                else if ((collider.gameObject.CompareTag("Shield_Spell") && gameObject.CompareTag("Enemy_Attack_Spell")) || (collider.gameObject.CompareTag("Enemy_Shield_Spell") && gameObject.CompareTag("Attack_Spell")))
                {
                    collider.gameObject.GetComponent<ShieldSpell>().ReducePower(power, element);
                    StartCoroutine("BeamDamageCooldown");
                }
                else if ((collider.gameObject.CompareTag("Enemy_Attack_Spell") && gameObject.CompareTag("Attack_Spell")) || (collider.gameObject.CompareTag("Attack_Spell") && gameObject.CompareTag("Enemy_Attack_Spell")))
                {
                    if (collider.gameObject.TryGetComponent(out ProjectileSpell projectile))
                    {
                        projectile.ReducePower(power, element);
                    }
                    StartCoroutine("BeamDamageCooldown");
                }
            }
        }
    
    }

    private void OnTriggerExit(Collider col)
    {
        if (targets.Contains(col))
        {
            targets.Remove(col);
        }
    }

    private IEnumerator BeamDamageCooldown()
    {
        hit = false;
        yield return new WaitForSeconds(0.5f / speed);
        hit = true;
    }
}
