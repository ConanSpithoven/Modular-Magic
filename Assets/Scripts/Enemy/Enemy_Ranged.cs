using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ranged : EnemyManager
{
    private Spawner spawner;
    private RoomCreator roomCreator;

    [SerializeField] private float searchSize;
    [SerializeField] private ProjectileSpell projectilespell = default;

    void Update()
    {
        if (!targetFound)
        {
            TargetFinder(searchSize);
        }
        else
        {
            if (target == null)
            {
                targetFound = false;
            }
            agent.destination = target.position;

            Vector3 direction = target.position - transform.position;
            float rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, rotation, 0);
            if (!attackCooldown)
            {
                Attack();
                StartCoroutine("AttackCooldown");
            }
        }
    }

    public override void Hit(float damageTaken, Element element)
    {
        base.Hit(damageTaken, element);
        TargetFinder(searchSize * 5f);
    }

    public override void Attack()
    {
        GameObject projectileObject = Instantiate(projectilespell.gameObject, (transform.position + transform.forward), transform.rotation);
        ProjectileSpell projectile = projectileObject.GetComponent<ProjectileSpell>();
        projectile.SetDamage(GetDamage());
        projectile.Activate();
        StartCoroutine("AttackCooldown");
    }

    private IEnumerator AttackCooldown()
    {
        attackCooldown = true;
        yield return new WaitForSeconds(attackRate);
        targets.Clear();
        attackCooldown = false;
    }

    public void SetSpawner(Spawner spawner)
    {
        this.spawner = spawner;
    }

    public void SetRoomCreator(RoomCreator roomCreator)
    {
        this.roomCreator = roomCreator;
    }

    public override void OnDeath()
    {
        if (spawner != null)
        {
            spawner.ReduceCount();
        }
        if (roomCreator != null)
        {
            roomCreator.ReduceCount();
        }
        base.OnDeath();
    }
}
