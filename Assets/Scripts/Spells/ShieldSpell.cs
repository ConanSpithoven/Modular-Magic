using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSpell : MonoBehaviour
{
    private float damage = 1f;
    private float lifetime = 1f;
    private int instances= 1;
    private string shape = "barrier";
    private float speed = 1f;
    private float size = 1f;
    private Transform caster = default;
    private float currentSize = 0f;
    private float currentDistance = 0f;
    private SpellInventory spellInventory = default;
    private int spellSlot = 1;
    private bool setup = false;
    private Vector3 startPos = default;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (shape == "shield")
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
        else if (shape == "deploy")
        {
            if (!setup)
                Setup();
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

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetSize(float size)
    {
        this.size = size;
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
            case "barrier":
                damage += (speed + size) * 0.2f;
                damage *= instances;
                break;
            case "shield":
                damage *= 1.5f;
                transform.localScale *= 0;
                break;
            case "deploy":
                transform.localScale *= 0;
                size *= 2;
                startPos = caster.position;
                break;
            default:
                break;
        }
        if(!spellInventory.GetCooldownStatus(spellSlot))
            spellInventory.StartCooldown(spellSlot);
        Destroy(gameObject, lifetime);
    }

    public void Hit(float damageTaken)
    {
        TakeDamage(damageTaken);
    }

    private void TakeDamage(float damageTaken)
    {
        damage -= damageTaken;
        if (damage <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void Setup()
    {
        if (shape == "shield")
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
        else if (shape == "deploy")
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
        if (shape == "shield" || shape == "deploy")
        {
            transform.localScale = new Vector3(sizelimited, sizelimited, sizelimited * 0.2f);
        }
    }

    private void SetupDistance()
    {
        currentDistance += speed * Time.deltaTime;
        float distanceLimited = Mathf.Clamp(currentDistance, 0f, size);
        transform.Translate(new Vector3(0f, 0f, distanceLimited * 0.1f));
    }

    private void OnCollisionEnter(Collision col)
    {
        if (shape != "orbit") { return; }
        if (col.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Shield_Spell"))
        {
            col.gameObject.GetComponent<EnemyManager>().Hit(damage / 2f);
        }
        else if (col.gameObject.CompareTag("Player") && gameObject.CompareTag("Enemy_Shield_Spell"))
        {
            col.gameObject.GetComponent<PlayerManager>().Hit(damage / 2f);
        }
        else if ((col.gameObject.CompareTag("Shield_Spell") && gameObject.CompareTag("Enemy_Shield_Spell")) || (col.gameObject.CompareTag("enemy_Shield_Spell") && gameObject.CompareTag("Shield_Spell")))
        {
            col.gameObject.GetComponent<ShieldSpell>().Hit(damage / 2f);
        }
        else if ((col.gameObject.CompareTag("Enemy_Attack_Spell") && gameObject.CompareTag("Shield_Spell")) || (col.gameObject.CompareTag("Enemy_Shield_Spell") && gameObject.CompareTag("Shield_Spell")))
        {
            col.gameObject.GetComponent<ProjectileSpell>().ReducePower(damage / 2f);
        }
    }
}
