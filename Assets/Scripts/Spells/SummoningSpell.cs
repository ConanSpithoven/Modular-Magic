using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SummoningSpell : MonoBehaviour
{
    [SerializeField] private LayerMask obstacles = default;
    private enum SpellShape { chaser, ranged, melee }
    private Element element;
    private float speed = 1f;
    private float damage = 1f;
    private float lifetime = 1f;
    private SpellShape shape = default;
    private int instances = 1;
    private int unique = 0;
    private float size = 1f;
    private SpellInventory spellInventory = default;
    private SpellInventory summonSpellInventory = default;
    private int spellSlot = 1;
    private Transform target = default;
    private Transform caster = default;
    private float currentSize = 0f;
    private float currentDistance = 0f;
    private bool targetFound = false;
    private bool setup = false;
    private Transform FirePos = default;
    private NavMeshAgent agent = default;
    private Animator animator = default;
    private float attackRate = default;
    private bool attackCooldown = default;
    private float hp = default;

    private void Awake()
    {
        gameObject.SetActive(false);
        if (TryGetComponent(out Animator animator))
        {
            this.animator = animator;
        }
    }

    private void Update()
    {
        switch (shape)
        {
            case SpellShape.chaser:
                if (setup)
                {
                    if (!targetFound)
                    {
                        transform.RotateAround(caster.position, Vector3.up, (speed * 35f) * Time.deltaTime);
                        var lookDir = Vector3.Cross(caster.position - transform.position, Vector3.up);
                        transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
                    }
                    else
                    {
                        float step = speed * Time.deltaTime;
                        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
                        Vector3 targetDirection = target.position - transform.position;
                        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 360f, 360f);
                        transform.rotation = Quaternion.LookRotation(newDirection);
                    }
                }
                else 
                {
                    Setup();
                }
                break;
            case SpellShape.ranged:
                TargetFinder();
                if (Vector3.Distance(transform.position, caster.position) > 2f)
                {
                    agent.destination = caster.position;
                }
                else
                {
                    agent.destination = transform.position;
                }
                if (targetFound)
                {
                    Vector3 direction = target.position - transform.position;
                    float rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, rotation, 0);
                    if ((!summonSpellInventory.GetCooldownStatus(1) || !summonSpellInventory.GetCooldownStatus(2) || !summonSpellInventory.GetCooldownStatus(3)) && !attackCooldown)
                    {
                        summonSpellInventory.Attack();
                        StartCoroutine("AttackCooldown");
                    }
                }
                else
                {
                    Vector3 direction = caster.position - transform.position;
                    float rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, rotation, 0);
                }
                break;
            case SpellShape.melee:
                TargetFinder();
                if (Vector3.Distance(transform.position, caster.position) > 2f)
                {
                    agent.destination = caster.position;
                }
                else
                {
                    agent.destination = transform.position;
                }
                if (targetFound)
                {
                    if (Vector3.Distance(transform.position, target.position) > 1.2f)
                    {
                        agent.destination = target.position;
                    }
                    else
                    {
                        agent.destination = transform.position;
                        if (!attackCooldown)
                        {
                            Attack();
                        }
                    }
                    Vector3 direction = target.position - transform.position;
                    float rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, rotation, 0);
                }
                else
                {
                    Vector3 direction = caster.position - transform.position;
                    float rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, rotation, 0);
                }
                break;
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

    public void SetUnique(int unique)
    {
        this.unique = unique;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetSize(float size)
    {
        this.size = size;
    }

    public void SetShape(int shape)
    {
        this.shape = (SpellShape)shape;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetLifetime(float lifetime)
    {
        this.lifetime = lifetime;
    }

    public void SetInstances(int instances)
    {
        this.instances = instances;
    }

    public void SetFirePos(Transform FirePos)
    {
        this.FirePos = FirePos;
    }

    public void SetCaster(Transform caster)
    {
        this.caster = caster;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        switch (shape)
        {
            case SpellShape.chaser:
                StartCoroutine("ChaserTimer");
                transform.localScale *= 0;
                break;
            case SpellShape.ranged:
                agent = GetComponent<NavMeshAgent>();
                attackRate = (1f / (speed / 5f));
                hp = damage * 2f;
                transform.SetParent(null, true);
                summonSpellInventory = GetComponent<SpellInventory>();
                Destroy(gameObject, lifetime*5f);
                break;
            case SpellShape.melee:
                agent = GetComponent<NavMeshAgent>();
                attackRate = (1f / (speed / 3f));
                hp = damage * 5f;
                transform.SetParent(null, true);
                Destroy(gameObject, lifetime * 5f);
                break;
        }
        if (!spellInventory.GetCooldownStatus(spellSlot))
            spellInventory.StartCooldown(spellSlot);
    }

    private void SetupSize()
    {
        currentSize += speed * Time.deltaTime;
        float sizelimited = Mathf.Clamp(currentSize, 0f, size / 5f);
        transform.localScale = new Vector3(sizelimited, sizelimited, sizelimited);
    }

    private void SetupDistance()
    {
        currentDistance += speed * Time.deltaTime;
        float distanceLimited = Mathf.Clamp(currentDistance, 0f, size);
        transform.Translate(new Vector3(0f, distanceLimited * 0.1f, distanceLimited * 0.1f));
    }

    private void Setup()
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

    private void TargetFinder()
    {
        float searchSize;
        if (shape == SpellShape.ranged)
        {
            searchSize = size * 6f;
        }
        else
        {
            searchSize = size * 3f;
        }
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, searchSize);
        Transform closestTarget = transform;
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].gameObject != target)
            {
                if ((hitColliders[i].CompareTag("Enemy") && (gameObject.CompareTag("Attack_Spell") || gameObject.CompareTag("Summon_Spell"))) || (hitColliders[i].CompareTag("Player") && (gameObject.CompareTag("Enemy_Attack_Spell") || gameObject.CompareTag("Summon_Spell"))))
                {
                    if (closestTarget == transform || Vector3.Distance(transform.position, hitColliders[i].transform.position) < Vector3.Distance(transform.position, closestTarget.position))
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(transform.position, hitColliders[i].transform.position - transform.position, out hit, size * 6f, obstacles, QueryTriggerInteraction.Ignore))
                        {
                            if ((hit.transform.gameObject.CompareTag("Enemy") && (gameObject.CompareTag("Attack_Spell") || gameObject.CompareTag("Summon_Spell"))) || (hitColliders[i].CompareTag("Player") && (gameObject.CompareTag("Enemy_Attack_Spell") || gameObject.CompareTag("Enemy_Summon_Spell"))))
                            {
                                closestTarget = hitColliders[i].transform;
                            }
                        }
                    }
                }
            }
            i++;
        }
        if (closestTarget != transform)
        {
            target = closestTarget;
            targetFound = true;
            if(shape == SpellShape.ranged)
            {
                summonSpellInventory.SetTargetFound(true);
                summonSpellInventory.SetTarget(target);
            }
        }
        else
        {
            target = null;
            targetFound = false;
            if(shape == SpellShape.ranged)
            {
                summonSpellInventory.SetTargetFound(false);
                summonSpellInventory.SetTarget(null);
            }
        }
    }

    private IEnumerator ChaserTimer()
    {
        yield return new WaitForSeconds(lifetime * 5f);
        Destroy(gameObject);
    }

    private void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
            StartCoroutine("AttackCooldown");
        }
    }

    private IEnumerator AttackCooldown()
    {
        attackCooldown = true;
        yield return new WaitForSeconds(attackRate);
        attackCooldown = false;
    }

    private void OnCollisionEnter(Collision col)
    {
        if ((col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Enemy_Summon_Spell")) && gameObject.CompareTag("Attack_Spell"))
        {
            if (col.gameObject.TryGetComponent(out EnemyManager enemy))
            {
                enemy.Hit(damage, element);
            }
            if (col.gameObject.TryGetComponent(out SummoningSpell summon))
            {
                summon.ReducePower(damage, element);
            }
            if(shape == SpellShape.chaser)
                Destroy(gameObject);
        }
        else if (((col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Summon_Spell")) && gameObject.CompareTag("Enemy_Attack_Spell")))
        {
            if (col.gameObject.TryGetComponent(out PlayerManager player))
            {
                player.Hit(damage, element);
            }
            if (col.gameObject.TryGetComponent(out SummoningSpell summon))
            {
                summon.ReducePower(damage, element);
            }
            if(shape == SpellShape.chaser)
                Destroy(gameObject);
        }
        else if ((col.gameObject.CompareTag("Shield_Spell") && gameObject.CompareTag("Enemy_Attack_Spell")) || (col.gameObject.CompareTag("Enemy_Shield_Spell") && gameObject.CompareTag("Attack_Spell")))
        {
            col.gameObject.GetComponent<ShieldSpell>().Hit(damage, element);
            if(shape == SpellShape.chaser)
                Destroy(gameObject);
        }
        else if ((col.gameObject.CompareTag("Enemy_Attack_Spell") && gameObject.CompareTag("Attack_Spell")) || (col.gameObject.CompareTag("Attack_Spell") && gameObject.CompareTag("Enemy_Attack_Spell")))
        {
            if (col.gameObject.TryGetComponent(out ProjectileSpell projectile))
            {
                projectile.ReducePower(damage, element);
            }
            if(shape == SpellShape.chaser)
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (shape == SpellShape.chaser)
        {
            if (col.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Attack_Spell"))
            {
                target = col.gameObject.transform;
                targetFound = true;
                StopCoroutine("ChaserTimer");
                transform.SetParent(null, true);
                GetComponent<CapsuleCollider>().enabled = false;
                Destroy(gameObject, lifetime);
            }
            else if (col.gameObject.CompareTag("Player") && gameObject.CompareTag("Enemy_Attack_Spell"))
            {
                target = col.gameObject.transform;
                targetFound = true;
                StopCoroutine("ChaserTimer");
                GetComponentInChildren<CapsuleCollider>().enabled = false;
                Destroy(gameObject, lifetime);
            }
                return;
        }
        else if (shape == SpellShape.melee)
        {
            if ((col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Enemy_Summon_Spell")) && gameObject.CompareTag("Summon_Spell"))
            {
                if (col.gameObject.TryGetComponent(out EnemyManager enemy))
                {
                    enemy.Hit(damage, element);
                }
                if (col.gameObject.TryGetComponent(out SummoningSpell summon))
                {
                    summon.ReducePower(damage, element);
                }
                if(shape == SpellShape.chaser)
                    Destroy(gameObject);
            }
            else if (((col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Summon_Spell")) && gameObject.CompareTag("Enemy_Summon_Spell")))
            {
                if (col.gameObject.TryGetComponent(out PlayerManager player))
                {
                    player.Hit(damage, element);
                }
                if (col.gameObject.TryGetComponent(out SummoningSpell summon))
                {
                    summon.ReducePower(damage, element);
                }
                if(shape == SpellShape.chaser)
                    Destroy(gameObject);
            }
            else if ((col.gameObject.CompareTag("Shield_Spell") && gameObject.CompareTag("Enemy_Summon_Spell")) || (col.gameObject.CompareTag("Enemy_Shield_Spell") && gameObject.CompareTag("Summon_Spell")))
            {
                col.gameObject.GetComponent<ShieldSpell>().Hit(damage, element);
                if(shape == SpellShape.chaser)
                    Destroy(gameObject);
            }
            else if ((col.gameObject.CompareTag("Enemy_Attack_Spell") && gameObject.CompareTag("Summon_Spell")) || (col.gameObject.CompareTag("Attack_Spell") && gameObject.CompareTag("Enemy_Summon_Spell")))
            {
                if (col.gameObject.TryGetComponent(out ProjectileSpell projectile))
                {
                    projectile.ReducePower(damage, element);
                }
                if(shape == SpellShape.chaser)
                    Destroy(gameObject);
            }
        }
    }

    public void ReducePower(float damage, Element element)
    {
        float totalDamage = damage * CheckElement(element);
        this.damage -= totalDamage;
        if (this.damage <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private float CheckElement(Element element)
    {
        float modifier = 1f;
        if (element.ElementName == this.element.ElementName)
        {
            modifier = 0.5f;
        }
        else
        {
            foreach (string strength in element.ElementStrengths)
            {
                if (strength == this.element.ElementName || strength == "All")
                {
                    modifier = 1.25f;
                }
            }
            foreach (string weakness in element.ElementWeaknesses)
            {
                if (weakness == this.element.ElementName || weakness == "All")
                {
                    modifier = 0.75f;
                }
            }
            foreach (string strength in this.element.ElementStrengths)
            {
                if (strength == element.ElementName || strength == "All")
                {
                    modifier = 0.75f;
                }
            }
            foreach (string weakness in this.element.ElementWeaknesses)
            {
                if (weakness == element.ElementName || weakness == "All")
                {
                    modifier = 1.25f;
                }
            }
        }
        return modifier;
    }
}
