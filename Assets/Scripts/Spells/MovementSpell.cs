using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSpell : Spell
{
    [SerializeField] private LayerMask obstacles = default;
    private enum SpellShape { dash, teleport, push }
    private SpellShape variant = default;
    private float currentDistance = default;
    private float travelDistance = default;
    private GameObject caster = default;
    private SpellInventory spellInventory = default;
    private bool stop = false;
    private Vector3 oldPos = default;
    private Vector3 goal = default;
    private Transform FirePos = default;
    private bool Enabled = default;

    private void Update()
    {
        if (!spellInventory.GetCooldownStatus(spellSlot) && stop)
        {
            StartCooldown();
        }
        if(instances > 0)
        {
            Recaster();
        }
            switch (variant)
        {
            case SpellShape.dash:
                if (currentDistance < travelDistance)
                {
                    float step = speed * Time.deltaTime;
                    caster.transform.position = Vector3.MoveTowards(caster.transform.position, goal, step);
                    currentDistance = Vector3.Distance(oldPos, caster.transform.position);
                }
                else
                {
                    if (Enabled)
                    {
                        GetComponent<MeshRenderer>().enabled = false;
                        GetComponent<Collider>().enabled = false;
                        Enabled = false;
                    }
                    if (!spellInventory.GetCooldownStatus(spellSlot) && instances <= 0)
                    {
                        StartCooldown();
                    }
                }
                break;
            default:
            case SpellShape.teleport:
                if (!spellInventory.GetCooldownStatus(spellSlot) && instances <= 0)
                {
                    StartCooldown();
                }
                break;
        }
    }

    public void SetSpellInventory(SpellInventory spellInventory)
    {
        this.spellInventory = spellInventory;
    }

    public int GetShape()
    {
        return (int)variant;
    }
    
    public void SetCaster(GameObject caster)
    {
        this.caster = caster;
    }

    public void SetFirePos(Transform FirePos)
    {
        this.FirePos = FirePos;
    }

    public void Activate()
    {
        variant = (SpellShape)shape;
        Setup();
        if (caster.TryGetComponent(out PlayerManager playerManager) && variant != SpellShape.push)
        {
            playerManager.AllowMovement(false);
        }
        switch (variant)
        {
            case SpellShape.dash:
                Dash();
                break;
            case SpellShape.teleport:
                Teleport();
                break;
            case SpellShape.push:
                Push();
                Destroy(gameObject, lifetime);
                break;
        }
    }

    private void Setup()
    {
        switch (variant)
        {
            case SpellShape.dash:
                size += damage;
                speed *= 10f;
                RecastCheck();
                break;
            case SpellShape.teleport:
                size += damage + speed;
                RecastCheck();
                break;
            case SpellShape.push:
                speed *= 15f;
                break;
        }
    }

    private void Dash()
    {
        oldPos = caster.transform.position;
        currentDistance = 0f;
        if (!Enabled)
        {
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<Collider>().enabled = true;
            Enabled = true;
        }
        CalcDestination();
    }

    private void Teleport()
    {
        CalcDestination();
        caster.transform.position = Vector3.MoveTowards(caster.transform.position, goal, travelDistance);
    }

    private void Push()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed * Time.deltaTime * 100f;
        if (!spellInventory.GetCooldownStatus(spellSlot) && instances <= 0)
        {
            spellInventory.StartCooldown(spellSlot);
        }
    }

    private void CalcDestination()
    {
        instances--;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.y = 1f;
        RaycastHit hit;
        if (Physics.Raycast(caster.transform.position, FirePos.forward, out hit, size, obstacles, QueryTriggerInteraction.Ignore) && hit.distance < Vector3.Distance(caster.transform.position, mousePos))
        {
            travelDistance = hit.distance - 0.5f;
            goal = hit.point;
        }
        else
        {
            if (size <= Vector3.Distance(caster.transform.position, mousePos))
            {
                travelDistance = size;
                goal = mousePos;
            }
            else
            {
                travelDistance = Vector3.Distance(caster.transform.position, mousePos);
                goal = mousePos;
            }
            
        }
        goal.y = 1f;
    }

    private void StartCooldown()
    {
        if (caster.TryGetComponent(out PlayerManager playerManager))
        {
            playerManager.AllowMovement(true);
        }
        spellInventory.StartCooldown(spellSlot);
        Destroy(gameObject);
    }

    private void RecastCheck()
    {
        if (instances <= 0) { return; }
        else
        {
            StopCoroutine("RecastTimer");
            StartCoroutine("RecastTimer", lifetime);
        }
    }

    private void Recaster()
    {
        switch (spellSlot)
        {
            case 1:
                if ((Input.GetKeyDown(KeyCode.Alpha1) || Input.GetMouseButtonDown(0)))
                    Activate();
                    break;
            case 2:
                if ((Input.GetKeyDown(KeyCode.Alpha2) || Input.GetMouseButtonDown(1)))
                    Activate();
                break;
            case 3:
                if ((Input.GetKeyDown(KeyCode.Alpha3) || Input.GetMouseButtonDown(2)))
                    Activate();
                break;
        }
    }

    private IEnumerator RecastTimer(float lifetime) 
    {
        stop = false;
        yield return new WaitForSeconds(lifetime);
        stop = true;
    }

    private void OnTriggerEnter(Collider col)
    {
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
            col.gameObject.GetComponent<ShieldSpell>().ReducePower(damage, element);
        }
        else if ((col.gameObject.CompareTag("Enemy_Attack_Spell") && gameObject.CompareTag("Attack_Spell")) || (col.gameObject.CompareTag("Attack_Spell") && gameObject.CompareTag("Enemy_Attack_Spell")))
        {
            if (TryGetComponent(out ProjectileSpell projectile))
            {
                projectile.ReducePower(damage, element);
            }
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        Destroy(gameObject);
    }
}
