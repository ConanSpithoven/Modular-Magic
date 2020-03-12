using System.Collections;
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
                size *= 2f;
                transform.SetParent(null, true);
                break;
            case SpellShape.orbit:
                break;
            case SpellShape.point:
                size *= 1.5f;
                break;
        }
        if (!spellInventory.GetCooldownStatus(spellSlot))
            spellInventory.StartCooldown(spellSlot);
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
        if ((col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Enemy_Summon_Spell")) && gameObject.CompareTag("Attack_Spell"))
        {
            if (hit)
            {
                if (col.gameObject.TryGetComponent(out EnemyManager enemy))
                {
                    enemy.Hit(damage, element);
                }
                if (col.gameObject.TryGetComponent(out SummoningSpell summon))
                {
                    summon.ReducePower(damage, element);
                }
                StartCoroutine("BeamDamageCooldown");
            }
        }
        else if (((col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Summon_Spell")) && gameObject.CompareTag("Enemy_Attack_Spell")))
        {
            if (hit)
            {
                if (col.gameObject.TryGetComponent(out PlayerManager player))
                {
                    player.Hit(damage, element);
                }
                if (col.gameObject.TryGetComponent(out SummoningSpell summon))
                {
                    summon.ReducePower(damage, element);
                }
                StartCoroutine("BeamDamageCooldown");
            }
        }
        else if ((col.gameObject.CompareTag("Shield_Spell") && gameObject.CompareTag("Enemy_Attack_Spell")) || (col.gameObject.CompareTag("Enemy_Shield_Spell") && gameObject.CompareTag("Attack_Spell")))
        {
            if (hit)
            {
                col.gameObject.GetComponent<ShieldSpell>().ReducePower(damage, element);
                StartCoroutine("BeamDamageCooldown");
            }
        }
        else if ((col.gameObject.CompareTag("Enemy_Attack_Spell") && gameObject.CompareTag("Attack_Spell")) || (col.gameObject.CompareTag("Attack_Spell") && gameObject.CompareTag("Enemy_Attack_Spell")))
        {
            if (hit)
            {
                col.gameObject.GetComponent<ProjectileSpell>().ReducePower(damage, element);
                StartCoroutine("BeamDamageCooldown");
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if ((col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Enemy_Summon_Spell")) && gameObject.CompareTag("Attack_Spell"))
        {
            if (hit)
            {
                if (col.gameObject.TryGetComponent(out EnemyManager enemy))
                {
                    enemy.Hit(damage, element);
                }
                if (col.gameObject.TryGetComponent(out SummoningSpell summon))
                {
                    summon.ReducePower(damage, element);
                }
                StartCoroutine("BeamDamageCooldown");
            }

        }
        else if (((col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Summon_Spell")) && gameObject.CompareTag("Enemy_Attack_Spell")))
        {
            if (hit)
            {
                if (col.gameObject.TryGetComponent(out PlayerManager player))
                {
                    player.Hit(damage, element);
                }
                if (col.gameObject.TryGetComponent(out SummoningSpell summon))
                {
                    summon.ReducePower(damage, element);
                }
                StartCoroutine("BeamDamageCooldown");
            }
        }
        else if (col.gameObject.CompareTag("Shield_Spell") && gameObject.CompareTag("Enemy_Attack_Spell"))
        {
            if (hit)
            {
                col.gameObject.GetComponent<ShieldSpell>().ReducePower(damage, element);
                StartCoroutine("BeamDamageCooldown");
            }
        }
    }

    private IEnumerator BeamDamageCooldown()
    {
        hit = false;
        yield return new WaitForSeconds(0.5f / speed);
        hit = true;
    }
}
