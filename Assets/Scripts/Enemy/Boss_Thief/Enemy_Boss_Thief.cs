using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss_Thief : EnemyManager
{
    public bool rangedAttackCooldown = false;
    public bool move = true;
    public bool alternate = true;

    [SerializeField] private float rangedAttackRate;
    [SerializeField] private float searchSize;
    [SerializeField] private ProjectileSpell projectilespell = default;

    void Update()
    {
        if (!targetFound)
        {
            TargetFinder(searchSize);
        }
    }

    public void MoveToPlayer()
    {
        if (target == null)
        {
            targetFound = false;
        }
        if (Vector3.Distance(transform.position, target.position) > 2f)
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
        LookAtPlayer();
        //check if player not too far away
    }

    public void LookAtPlayer()
    {
        Vector3 direction = target.position - transform.position;
        float rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, rotation, 0);
    }

    public void ClearTarget()
    {
        agent.destination = transform.position;
    }

    public override void Attack()
    {
        SetAnimator("Trigger", "Melee_Attack");
        StartCoroutine("AttackCooldown");
    }

    public void RangedAttack()
    {
        SetAnimator("Trigger", "Ranged_Attack");
        StartCoroutine("RangedAttackCooldown");
    }

    private IEnumerator AttackCooldown()
    {
        attackCooldown = true;
        yield return new WaitForSeconds(attackRate);
        targets.Clear();
        attackCooldown = false;
    }

    private IEnumerator RangedAttackCooldown()
    {
        rangedAttackCooldown = true;
        yield return new WaitForSeconds(rangedAttackRate);
        targets.Clear();
        rangedAttackCooldown = false;
    }

    public void FireProjectile(int instances)
    {
        float spreadAngle = 15f * instances;
        spreadAngle = Mathf.Clamp(spreadAngle, 30f, 160f);
        float perBulletAngle = spreadAngle / (instances - 1);
        float startAngle = spreadAngle * -0.5f;

        for (int i = 0; i < instances; i++)
        {
            GameObject projectileObject;
            projectileObject = Instantiate(projectilespell.gameObject, (transform.position + transform.forward), transform.rotation);
            projectileObject.transform.Rotate(Vector3.up, startAngle + i * perBulletAngle);
            ProjectileSpell projectile = projectileObject.GetComponent<ProjectileSpell>();
            projectile.SetDamage(GetDamage());
            projectile.SetInstances(instances);
            projectile.Activate();
        }
    }

    public void ResetHit()
    {
        targets.Clear();
    }

    public void Enrage()
    {
        //update attackrates to enraged attackrates
        SetAnimator("Trigger", "Enrage");
    }

    public void Alternate()
    {
        StartCoroutine("MoveAndStop");
    }

    private IEnumerator MoveAndStop()
    {
        move = !move;
        yield return new WaitForSeconds(Random.Range(0.1f, 1.5f));
        alternate = true;
    }
}
