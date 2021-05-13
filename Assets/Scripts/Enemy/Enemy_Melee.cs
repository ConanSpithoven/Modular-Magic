using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Melee : EnemyManager
{
    private Spawner spawner;
    private RoomCreator roomCreator;

    [SerializeField] private float searchSize;

    void Update()
    {
        if (!targetFound)
        {
            TargetFinder(searchSize);
            if (!wanderCooldown)
            {
                Wander();
            }
        }
        else
        {
            if (target == null)
            {
                targetFound = false;
            }
            if (Vector3.Distance(transform.position, target.position) > 1.2f)
            {
                agent.destination = target.position;
            }
            else
            {
                //patrol
                agent.destination = transform.position;
                if (!attackCooldown)
                {
                    Attack();
                }
            }
            Vector3 direction = target.position - transform.position;
            float rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, rotation, 0);
            //check if player not too far away
        }
    }

    public override void Hit(float damageTaken, Element element)
    {
        base.Hit(damageTaken, element);
        TargetFinder(searchSize*5f);
    }

    public override void Attack()
    {
        SetAnimator("Trigger", "Attack");
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
