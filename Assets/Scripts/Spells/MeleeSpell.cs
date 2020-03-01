﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSpell : MonoBehaviour
{
    private Element element;
    private enum SpellShape { sword, spear, axe }
    private float damage = 1f;
    private float lifetime = 1f;
    private float size = 1f;
    private float speed = 1f;
    private SpellShape shape = default;
    private SpellInventory spellInventory = default;
    private int spellSlot = 1;
    private bool invert = false;
    private Vector3 targetPos = default;
    private Vector3 oldPos = default;
    private float currentDistance = default;
    private float travelDistance = default;
    private Transform FirePos = default;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        switch (shape)
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

    public void SetLifetime(float lifetime)
    {
        this.lifetime = lifetime;
    }

    public void SetSize(float size)
    {
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

    public void SetShape(int shape)
    {
        this.shape = (SpellShape)shape;
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
        gameObject.SetActive(true);
        transform.localScale *= size;
        switch (shape)
        {
            case SpellShape.sword:
                speed *= 250f;
                break;
            case SpellShape.spear:
                speed *= 7.5f;
                size *= 4f;
                oldPos = transform.position;
                transform.SetParent(null, true);
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
                Vector3 targetDirection = targetPos - transform.position;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 360f, 360f);
                transform.rotation = Quaternion.LookRotation(newDirection);
                transform.Rotate(Vector3.forward, Random.Range(0f, 360f));
                travelDistance = size;
                break;
            case SpellShape.axe:
                speed *= 100f;
                transform.SetParent(null, true);
                break;
        }
        if (!spellInventory.GetCooldownStatus(spellSlot))
            spellInventory.StartCooldown(spellSlot);
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
        }
        else if (col.gameObject.CompareTag("Player") && gameObject.CompareTag("Enemy_Attack_Spell"))
        {
            if (col.gameObject.TryGetComponent(out PlayerManager player))
            {
                player.Hit(damage, element);
            }
            if (col.gameObject.TryGetComponent(out SummoningSpell summon))
            {
                summon.ReducePower(damage, element);
            }
        }
        else if ((col.gameObject.CompareTag("Shield_Spell") && gameObject.CompareTag("Enemy_Attack_Spell")) || (col.gameObject.CompareTag("Enemy_Shield_Spell") && gameObject.CompareTag("Attack_Spell")))
        {
            col.gameObject.GetComponent<ShieldSpell>().Hit(damage, element);
        }
        else if ((col.gameObject.CompareTag("Enemy_Attack_Spell") && gameObject.CompareTag("Attack_Spell")) || (col.gameObject.CompareTag("Attack_Spell") && gameObject.CompareTag("Enemy_Attack_Spell")))
        {
            if (col.gameObject.TryGetComponent(out ProjectileSpell projectile))
            {
                projectile.ReducePower(damage, element);
            }
        }
        if (shape == SpellShape.axe && ((col.gameObject.CompareTag("Player") && gameObject.CompareTag("Enemy_Attack_Spell")) || (col.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Attack_Spell"))) || col.gameObject.CompareTag("Ground"))
            Destroy(gameObject);
    }
}
