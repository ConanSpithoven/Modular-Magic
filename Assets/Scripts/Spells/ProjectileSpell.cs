using System.Collections;
using UnityEngine;

public class ProjectileSpell : Spell
{
    [SerializeField] private LayerMask obstacles = default;
    [SerializeField] private LayerMask chainables = default;
    private Vector3 targetPos = default;
    private enum SpellShape { ball, line, chain }
    private SpellShape variant = default;
    private bool hit = true;
    private float currentsize = 0f;
    private SpellInventory spellInventory = default;
    private Transform FirePos = default;
    private float maxSize = default;

    private void Update()
    {
        if (variant == SpellShape.line)
        {

            RaycastHit hit;
            if (Physics.Raycast(FirePos.position, FirePos.forward, out hit, size * 2f, obstacles, QueryTriggerInteraction.Ignore))
            {
                maxSize = hit.distance / 2f;
            }
            else
            {
                maxSize = size;
            }
            currentsize += 10f * Time.deltaTime;
            currentsize = Mathf.Clamp(currentsize, 0f, maxSize);
            float sizelimited = currentsize;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, sizelimited);
        }
    }

    public void SetFirePos(Transform FirePos)
    {
        this.FirePos = FirePos;
    }

    public void SetSpellInventory(SpellInventory spellInventory)
    {
        this.spellInventory = spellInventory;
    }

    public void Activate()
    {
        variant = (SpellShape)shape;
        switch (variant)
        {
            case SpellShape.ball:
                if (unique >= 1)
                {
                    Collider col = GetModel().GetComponent<Collider>();
                    col.material.bounciness = 1;
                }
                Rigidbody rb = GetComponent<Rigidbody>();
                transform.localScale *= (size / 5f);
                rb.velocity = transform.forward * (speed + 20) * Time.deltaTime * 100f;
                Destroy(gameObject, lifetime);
                break;
            case SpellShape.line:
                transform.localScale = new Vector3(transform.localScale.x * size / 2f, transform.localScale.y * size / 2f, transform.localScale.z);
                speed += 4;
                size += 2f;
                power *= 0.2f;
                Destroy(gameObject, lifetime);
                break;
            case SpellShape.chain:
                transform.localScale = new Vector3(0.5f, 0.5f, 0f);
                size += 2f;
                if (spellInventory.GetCasterType() == 0)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(FirePos.position, FirePos.forward, out hit, size * 2f, chainables))
                    {
                        targetPos = hit.point;
                    }
                    else if (Physics.Raycast(FirePos.position, FirePos.forward, out hit, Mathf.Infinity, chainables))
                    {
                        targetPos = hit.point;
                    }
                }
                else
                {
                    targetPos = spellInventory.GetTarget().position;
                }
                CastChain(FirePos, targetPos);
                break;
        }
        if (!spellInventory.GetCooldownStatus(spellSlot))
            spellInventory.StartCooldown(spellSlot);
    }

    private void OnCollisionEnter(Collision col){
        if (col.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Attack_Spell"))
        {
            col.gameObject.GetComponent<EnemyManager>().Hit(power, element);
        }
        else if (col.gameObject.CompareTag("Player") && gameObject.CompareTag("Enemy_Attack_Spell"))
        {
            col.gameObject.GetComponent<PlayerManager>().Hit(power, element);
        }
        else if ((col.gameObject.CompareTag("Shield_Spell") && gameObject.CompareTag("Enemy_Attack_Spell")) || (col.gameObject.CompareTag("Enemy_Shield_Spell") && gameObject.CompareTag("Attack_Spell")))
        {
            col.gameObject.GetComponent<ShieldSpell>().ReducePower(power, element);
        }
        else if ((col.gameObject.CompareTag("Enemy_Attack_Spell") && gameObject.CompareTag("Attack_Spell")) || (col.gameObject.CompareTag("Attack_Spell") && gameObject.CompareTag("Enemy_Attack_Spell")))
        {
            if (TryGetComponent(out ProjectileSpell projectile))
            {
                projectile.ReducePower(power, element);
            }
        }
        if (unique > 0 && variant == SpellShape.ball)
        {
            unique -= 1;
        }
        else if (unique <= 0 && variant == SpellShape.ball)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if(variant == SpellShape.chain && col.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject, 0.1f);
        }
        if ((col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Enemy_Summon_Spell")) && gameObject.CompareTag("Attack_Spell")) {
            if (hit)
            {
                if (col.gameObject.TryGetComponent(out EnemyManager enemy))
                {
                    enemy.Hit(power, element);
                }
                if (col.gameObject.TryGetComponent(out SummoningSpell summon))
                {
                    summon.ReducePower(power, element);
                }
                StartCoroutine("BeamDamageCooldown");
                if (variant == SpellShape.chain && instances > 0)
                {
                    StartCoroutine("Bounces", col.gameObject);
                }
                else if (variant == SpellShape.chain && instances <= 0)
                {
                    Destroy(gameObject, 0.1f);
                }
            }
        } 
        else if (((col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Summon_Spell")) && gameObject.CompareTag("Enemy_Attack_Spell")))
        {
            if (hit)
            {
                if (col.gameObject.TryGetComponent(out PlayerManager player))
                {
                    player.Hit(power, element);
                }
                if (col.gameObject.TryGetComponent(out SummoningSpell summon))
                {
                    summon.ReducePower(power, element);
                }
                StartCoroutine("BeamDamageCooldown");
                if (variant == SpellShape.chain && instances > 0)
                {
                    StartCoroutine("Bounces", col.gameObject);
                }
                else if (variant == SpellShape.chain && instances <= 0)
                {
                    Destroy(gameObject, 0.1f);
                }
            }
        }
        else if ((col.gameObject.CompareTag("Shield_Spell") && gameObject.CompareTag("Enemy_Attack_Spell")) || (col.gameObject.CompareTag("Enemy_Shield_Spell") && gameObject.CompareTag("Attack_Spell")))
        {
            if (hit)
            {
                col.gameObject.GetComponent<ShieldSpell>().ReducePower(power, element);
                StartCoroutine("BeamDamageCooldown");
            }
        }
        else if ((col.gameObject.CompareTag("Enemy_Attack_Spell") && gameObject.CompareTag("Attack_Spell")) || (col.gameObject.CompareTag("Attack_Spell") && gameObject.CompareTag("Enemy_Attack_Spell")))
        {
            if (hit)
            {
                if (TryGetComponent(out ProjectileSpell projectile))
                {
                    projectile.ReducePower(power, element);
                }
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
                    enemy.Hit(power, element);
                }
                if (col.gameObject.TryGetComponent(out SummoningSpell summon))
                {
                    summon.ReducePower(power, element);
                }
                StartCoroutine("BeamDamageCooldown");
                if (variant == SpellShape.chain && instances > 0)
                {
                    StartCoroutine("Bounces", col.gameObject);
                }
                else if (variant == SpellShape.chain && instances <= 0)
                {
                    Destroy(gameObject, 0.1f);
                }
            }
            
        }
        else if (((col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Summon_Spell")) && gameObject.CompareTag("Enemy_Attack_Spell")))
        {
            if (hit)
            {
                if (col.gameObject.TryGetComponent(out PlayerManager player))
                {
                    player.Hit(power, element);
                }
                if (col.gameObject.TryGetComponent(out SummoningSpell summon))
                {
                    summon.ReducePower(power, element);
                }
                StartCoroutine("BeamDamageCooldown");
                if (variant == SpellShape.chain && instances > 0)
                {
                    StartCoroutine("Bounces", col.gameObject);
                }
                else if (variant == SpellShape.chain && instances <= 0)
                {
                    Destroy(gameObject, 0.1f);
                }
            }
        }
        else if ((col.gameObject.CompareTag("Shield_Spell") && gameObject.CompareTag("Enemy_Attack_Spell")) || (col.gameObject.CompareTag("Enemy_Shield_Spell") && gameObject.CompareTag("Attack_Spell")))
        {
            if (hit)
            {
                col.gameObject.GetComponent<ShieldSpell>().ReducePower(power, element);
                StartCoroutine("BeamDamageCooldown");
            }
        }
        else if ((col.gameObject.CompareTag("Enemy_Attack_Spell") && gameObject.CompareTag("Attack_Spell")) || (col.gameObject.CompareTag("Attack_Spell") && gameObject.CompareTag("Enemy_Attack_Spell")))
        {
            if (hit)
            {
                if (col.gameObject.TryGetComponent(out ProjectileSpell projectile))
                {
                    projectile.ReducePower(power, element);
                }
                StartCoroutine("BeamDamageCooldown");
            }
        }
    }

    private void CastChain(Transform start, Vector3 target)
    {
        transform.position = start.position;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        Vector3 targetDirection = target - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 360f, 360f);
        transform.rotation = Quaternion.LookRotation(newDirection);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 0f);
        float distance = 0f;
        instances--;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, target - transform.position, out hit, size*2f, chainables, QueryTriggerInteraction.Ignore))
        {
            distance = hit.distance/2f;
        }
        else
        {
            distance = size;
            Destroy(gameObject, 0.1f);
        }
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, distance);
    }

    private IEnumerator Bounces(GameObject target)
    {
        yield return new WaitForSeconds(0.1f);
        Collider[] hitColliders = Physics.OverlapSphere(target.transform.position, size*2f);
        Transform closestTarget = target.transform;
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].gameObject != target)
            {
                if ((hitColliders[i].CompareTag("Enemy") && gameObject.CompareTag("Attack_Spell")) || (hitColliders[i].CompareTag("Player") && gameObject.CompareTag("Enemy_Attack_Spell")))
                {
                    if (closestTarget == target.transform || Vector3.Distance(target.transform.position, hitColliders[i].transform.position) < Vector3.Distance(target.transform.position, closestTarget.position))
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(transform.position, target.transform.position - transform.position, out hit, size * 2f, chainables, QueryTriggerInteraction.Ignore))
                        {
                            if ((hit.transform.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Attack_Spell")) || (hitColliders[i].CompareTag("Player") && gameObject.CompareTag("Enemy_Attack_Spell")))
                            {
                                closestTarget = hitColliders[i].transform;
                            }
                        }
                    }
                }
            }
            i++;
        }
        hit = true;
        CastChain(target.transform, closestTarget.position);
    }

    private IEnumerator BeamDamageCooldown()
    {
        hit = false;
        yield return new WaitForSeconds(0.5f / speed);
        hit = true;
    }
}
