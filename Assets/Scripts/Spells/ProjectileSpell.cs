using System.Collections;
using UnityEngine;

public class ProjectileSpell : MonoBehaviour
{
    [SerializeField] private LayerMask obstacles = default;
    private Element element;
    private float speed = 1f;
    private float damage = 1f;
    private float lifetime = 1f;
    private string shape = "ball";
    private int instances = 1;
    private int unique = 0;
    private float size = 1f;
    private bool hit = true;
    private float currentsize = 0f;
    private SpellInventory spellInventory = default;
    private int spellSlot = 1;
    private Transform FirePos = default;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (shape == "line")
        {
            currentsize += 10f * Time.deltaTime;
            float sizelimited = Mathf.Clamp(currentsize, 0f, size);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, sizelimited);
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

    public void SetUnique(int unique) {
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

    public void SetShape(string shape){
        this.shape = shape;
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

    public float GetDamage()
    {
        return damage;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        switch (shape)
        {
            case "ball":
                if (unique >= 1)
                {
                    Collider col = GetComponent<Collider>();
                    col.material.bounciness = 1;
                }
                Rigidbody rb = GetComponent<Rigidbody>();
                transform.localScale *= (size / 5f);
                rb.velocity = transform.forward * speed * Time.deltaTime * 100f;
                Destroy(gameObject, lifetime);
                break;
            case "line":
                transform.localScale = new Vector3(transform.localScale.x * size / 2f, transform.localScale.y * size / 2f, transform.localScale.z);
                speed *= 5;
                size *= 3f;
                damage *= 0.2f;
                Destroy(gameObject, lifetime);
                break;
            case "chain":
                transform.localScale = new Vector3(0.5f, 0.5f, 0f);
                size *= 3f;
                Vector3 targetPos;
                if (spellInventory.GetCasterType() == 1)
                {
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePos.y = FirePos.position.y;
                    targetPos = mousePos;
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
            col.gameObject.GetComponent<EnemyManager>().Hit(damage, element);
        }
        else if (col.gameObject.CompareTag("Player") && gameObject.CompareTag("Enemy_Attack_Spell"))
        {
            col.gameObject.GetComponent<PlayerManager>().Hit(damage, element);
        }
        else if ((col.gameObject.CompareTag("Shield_Spell") && gameObject.CompareTag("Enemy_Attack_Spell")) || (col.gameObject.CompareTag("Enemy_Shield_Spell") && gameObject.CompareTag("Attack_Spell")))
        {
            col.gameObject.GetComponent<ShieldSpell>().Hit(damage, element);
        }
        else if ((col.gameObject.CompareTag("Enemy_Attack_Spell") && gameObject.CompareTag("Attack_Spell")) || (col.gameObject.CompareTag("Attack_Spell") && gameObject.CompareTag("Enemy_Attack_Spell")))
        {
            if (TryGetComponent(out ProjectileSpell projectile))
            {
                projectile.ReducePower(damage, element);
            }
        }
        if (unique > 0 && shape != "line")
        {
            unique -= 1;
        }
        else if (unique <= 0 && shape != "line")
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if(shape == "chain" && col.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject, 0.1f);
        }
        if ((col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Enemy_Summon_Spell")) && gameObject.CompareTag("Attack_Spell")) {
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
                if (shape == "chain" && instances > 0)
                {
                    StartCoroutine("Bounces", col.gameObject);
                }
                else if (shape == "chain" && instances <= 0)
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
                    player.Hit(damage, element);
                }
                if (col.gameObject.TryGetComponent(out SummoningSpell summon))
                {
                    summon.ReducePower(damage, element);
                }
                StartCoroutine("BeamDamageCooldown");
                if (shape == "chain" && instances > 0)
                {
                    StartCoroutine("Bounces", col.gameObject);
                }
                else if (shape == "chain" && instances <= 0)
                {
                    Destroy(gameObject, 0.1f);
                }
            }
        }
        else if ((col.gameObject.CompareTag("Shield_Spell") && gameObject.CompareTag("Enemy_Attack_Spell")) || (col.gameObject.CompareTag("Enemy_Shield_Spell") && gameObject.CompareTag("Attack_Spell")))
        {
            if (hit)
            {
                col.gameObject.GetComponent<ShieldSpell>().Hit(damage, element);
                StartCoroutine("BeamDamageCooldown");
            }
        }
        else if ((col.gameObject.CompareTag("Enemy_Attack_Spell") && gameObject.CompareTag("Attack_Spell")) || (col.gameObject.CompareTag("Attack_Spell") && gameObject.CompareTag("Enemy_Attack_Spell")))
        {
            if (hit)
            {
                if (TryGetComponent(out ProjectileSpell projectile))
                {
                    projectile.ReducePower(damage, element);
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
                    enemy.Hit(damage, element);
                }
                if (col.gameObject.TryGetComponent(out SummoningSpell summon))
                {
                    summon.ReducePower(damage, element);
                }
                StartCoroutine("BeamDamageCooldown");
                if (shape == "chain" && instances > 0)
                {
                    StartCoroutine("Bounces", col.gameObject);
                }
                else if (shape == "chain" && instances <= 0)
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
                    player.Hit(damage, element);
                }
                if (col.gameObject.TryGetComponent(out SummoningSpell summon))
                {
                    summon.ReducePower(damage, element);
                }
                StartCoroutine("BeamDamageCooldown");
                if (shape == "chain" && instances > 0)
                {
                    StartCoroutine("Bounces", col.gameObject);
                }
                else if (shape == "chain" && instances <= 0)
                {
                    Destroy(gameObject, 0.1f);
                }
            }
        }
        else if ((col.gameObject.CompareTag("Shield_Spell") && gameObject.CompareTag("Enemy_Attack_Spell")) || (col.gameObject.CompareTag("Enemy_Shield_Spell") && gameObject.CompareTag("Attack_Spell")))
        {
            if (hit)
            {
                col.gameObject.GetComponent<ShieldSpell>().Hit(damage, element);
                StartCoroutine("BeamDamageCooldown");
            }
        }
        else if ((col.gameObject.CompareTag("Enemy_Attack_Spell") && gameObject.CompareTag("Attack_Spell")) || (col.gameObject.CompareTag("Attack_Spell") && gameObject.CompareTag("Enemy_Attack_Spell")))
        {
            if (hit)
            {
                if (col.gameObject.TryGetComponent(out ProjectileSpell projectile))
                {
                    projectile.ReducePower(damage, element);
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
        if (Physics.Raycast(transform.position, target - transform.position, out hit, size*2f, obstacles, QueryTriggerInteraction.Ignore))
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
                        if (Physics.Raycast(transform.position, target.transform.position - transform.position, out hit, size * 2f, obstacles, QueryTriggerInteraction.Ignore))
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
        }
        return modifier;
    }
}
