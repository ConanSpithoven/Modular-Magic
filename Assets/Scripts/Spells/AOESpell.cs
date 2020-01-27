using System.Collections;
using UnityEngine;

public class AOESpell : MonoBehaviour
{
    private float damage = 1f;
    private float lifetime = 1f;
    private float size = 1f;
    private float speed = 1f;
    private float currentsize = 0f;
    private string shape = "orb";
    private SpellInventory spellInventory = default;
    private int spellSlot = 1;
    private Transform caster = default;
    private float currentSize = 0f;
    private float currentDistance = 0f;
    private bool hit = true;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (shape == "orb" || shape == "point")
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
        else if (shape == "orbit")
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

    public void SetSlot(int spellSlot)
    {
        this.spellSlot = spellSlot;
    }

    public void SetSpellInventory(SpellInventory spellInventory)
    {
        this.spellInventory = spellInventory;
    }

    public void SetLifetime(float lifetime)
    {
        this.lifetime = lifetime;
    }

    public void SetSize(float size){
        this.size = size;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetShape(string shape)
    {
        this.shape = shape;
    }

    public void SetCaster(Transform caster)
    {
        this.caster = caster;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        transform.localScale *= 0;
        switch (shape)
        {
            case "orb":
                size *= 2f;
                transform.SetParent(null, true);
                break;
            case "orbit":
                break;
            case "point":
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
        if (shape == "shield")
        {
            transform.localScale = new Vector3(sizelimited, sizelimited, sizelimited * 0.2f);
        }
        else
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

    private void OnTriggerEnter(Collider col)
    {
        if ((col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Enemy_Summon_Spell")) && gameObject.CompareTag("Attack_Spell"))
        {
            if (hit)
            {
                if (col.gameObject.TryGetComponent(out EnemyManager enemy))
                {
                    enemy.Hit(damage);
                }
                if (col.gameObject.TryGetComponent(out SummoningSpell summon))
                {
                    summon.ReducePower(damage);
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
                    player.Hit(damage);
                }
                if (col.gameObject.TryGetComponent(out SummoningSpell summon))
                {
                    summon.ReducePower(damage);
                }
                StartCoroutine("BeamDamageCooldown");
            }
        }
        else if ((col.gameObject.CompareTag("Shield_Spell") && gameObject.CompareTag("Enemy_Attack_Spell")) || (col.gameObject.CompareTag("Enemy_Shield_Spell") && gameObject.CompareTag("Attack_Spell")))
        {
            if (hit)
            {
                col.gameObject.GetComponent<ShieldSpell>().Hit(damage);
                StartCoroutine("BeamDamageCooldown");
            }
        }
        else if ((col.gameObject.CompareTag("Enemy_Attack_Spell") && gameObject.CompareTag("Attack_Spell")) || (col.gameObject.CompareTag("Attack_Spell") && gameObject.CompareTag("Enemy_Attack_Spell")))
        {
            if (hit)
            {
                col.gameObject.GetComponent<ProjectileSpell>().ReducePower(damage);
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
                    enemy.Hit(damage);
                }
                if (col.gameObject.TryGetComponent(out SummoningSpell summon))
                {
                    summon.ReducePower(damage);
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
                    player.Hit(damage);
                }
                if (col.gameObject.TryGetComponent(out SummoningSpell summon))
                {
                    summon.ReducePower(damage);
                }
                StartCoroutine("BeamDamageCooldown");
            }
        }
        else if (col.gameObject.CompareTag("Shield_Spell") && gameObject.CompareTag("Enemy_Attack_Spell"))
        {
            if (hit)
            {
                col.gameObject.GetComponent<ShieldSpell>().Hit(damage);
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

    public void ReducePower(float damage)
    {
        this.damage -= damage;
        if (this.damage <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
