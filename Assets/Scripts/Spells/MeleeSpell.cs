using System.Collections.Generic;
using UnityEngine;

public class MeleeSpell : Spell
{
    private enum SpellShape { sword, spear, axe }
    private SpellShape variant = default;
    private SpellInventory spellInventory = default;
    private bool invert = false;
    private Vector3 targetPos = default;
    private Vector3 oldPos = default;
    private float currentDistance = default;
    private float travelDistance = default;
    private Transform FirePos = default;
    [SerializeField] private LayerMask layerMask = default;

    List<GameObject> targets = new List<GameObject>();

    private void Update()
    {
        switch (variant)
        {
            case SpellShape.sword: 
                if (transform.localRotation.y < Quaternion.Euler(0, 80, 0).y && transform.localRotation.y > Quaternion.Euler(0, -80, 0).y)
                {
                    if (invert)
                    {
                        transform.Rotate(Vector3.up, -speed * Time.deltaTime);
                    }
                    else
                    {
                        transform.Rotate(Vector3.up, speed * Time.deltaTime);
                    }
                }
                else 
                {
                    Destroy(gameObject);
                }
                break;
            case SpellShape.spear:
                if (transform.position == targetPos)
                {
                    Destroy(gameObject);
                }
                if (currentDistance < travelDistance)
                {
                    float step = speed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
                    currentDistance = Vector3.Distance(oldPos, transform.position);
                }
                else
                {
                    if (!spellInventory.GetCooldownStatus(spellSlot))
                    {
                        spellInventory.StartCooldown(spellSlot);
                    }
                    Destroy(gameObject);
                }
                break;
            case SpellShape.axe:
                if (transform.localRotation.x < Quaternion.Euler(100, 0, 0).x)
                {
                    transform.Rotate(Vector3.right, speed * Time.deltaTime);
                }
                else
                {
                    Destroy(gameObject);
                }
                break;
        }
    }

    public void SetSpellInventory(SpellInventory spellInventory)
    {
        this.spellInventory = spellInventory;
    }

    public void SetDirection(bool invert)
    {
        this.invert = invert;
    }

    public void SetFirePos(Transform FirePos)
    {
        this.FirePos = FirePos;
    }

    public void Activate()
    {
        variant = (SpellShape)shape;
        switch (variant)
        {
            case SpellShape.sword:
                transform.localScale *= (1 + (size * 0.1f));
                speed *= 5;
                speed += 250f;
                break;
            case SpellShape.spear:
                speed *= 1.2f;
                speed += 7.5f;
                transform.localScale *= (1 + (size * 0.15f));
                oldPos = transform.position;
                transform.SetParent(null, true);
                if (spellInventory.GetCasterType() == 0)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(FirePos.position, FirePos.forward, out hit, size * 2f, layerMask))
                    {
                        targetPos = hit.point;
                    }
                    else if (Physics.Raycast(FirePos.position, FirePos.forward, out hit, Mathf.Infinity, layerMask))
                    {
                        targetPos = hit.point;
                    }
                }
                else
                {
                    targetPos = spellInventory.GetTarget().position;
                }
                Vector3 targetDirection = targetPos - transform.position;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 360f, 360f);
                transform.rotation = Quaternion.LookRotation(newDirection);
                transform.Rotate(Vector3.forward, Random.Range(0f, 360f));
                travelDistance = size + 3f;
                break;
            case SpellShape.axe:
                transform.localScale *= (1 + (size * 0.2f));
                speed *= 5f;
                speed += 100f;
                transform.SetParent(null, true);
                break;
        }
        if (!spellInventory.GetCooldownStatus(spellSlot))
            spellInventory.StartCooldown(spellSlot);
    }

    private void OnCollisionEnter(Collision col)
    {
        int i = 0;
        foreach (GameObject collider in targets)
        {
            if (targets[i] == null)
            {
                targets.RemoveAt(i);
            }
            i++;
        }
        if (col.gameObject.CompareTag("Wall"))
            Destroy(gameObject);
        if (variant == SpellShape.axe && ((col.gameObject.CompareTag("Player") && gameObject.CompareTag("Enemy_Attack_Spell")) || (col.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Attack_Spell"))) || col.gameObject.CompareTag("Ground"))
            Destroy(gameObject);
        if (!targets.Contains(col.gameObject))
        {
            targets.Add(col.gameObject);
            if ((col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Enemy_Summon_Spell")) && gameObject.CompareTag("Attack_Spell"))
            {
                if (col.gameObject.TryGetComponent(out EnemyManager enemy))
                {
                    enemy.Hit(power, element);
                }
                if (col.gameObject.TryGetComponent(out SummoningSpell summon))
                {
                    summon.ReducePower(power, element);
                }
            }
            else if (col.gameObject.CompareTag("Player") && gameObject.CompareTag("Enemy_Attack_Spell"))
            {
                if (col.gameObject.TryGetComponent(out PlayerManager player))
                {
                    player.Hit(power, element);
                }
                if (col.gameObject.TryGetComponent(out SummoningSpell summon))
                {
                    summon.ReducePower(power, element);
                }
            }
            else if ((col.gameObject.CompareTag("Shield_Spell") && gameObject.CompareTag("Enemy_Attack_Spell")) || (col.gameObject.CompareTag("Enemy_Shield_Spell") && gameObject.CompareTag("Attack_Spell")))
            {
                col.gameObject.GetComponent<ShieldSpell>().ReducePower(power, element);
            }
            else if ((col.gameObject.CompareTag("Enemy_Attack_Spell") && gameObject.CompareTag("Attack_Spell")) || (col.gameObject.CompareTag("Attack_Spell") && gameObject.CompareTag("Enemy_Attack_Spell")))
            {
                if (col.gameObject.TryGetComponent(out ProjectileSpell projectile))
                {
                    projectile.ReducePower(power, element);
                }
            }
        }
    }
}
