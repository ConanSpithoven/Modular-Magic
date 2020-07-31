using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyStats stats;
    [SerializeField] private LayerMask obstacles;
    [SerializeField] private EnemyType type;
    private NavMeshAgent agent;
    private Transform target;
    private bool targetFound = false;
    private bool attackCooldown = false;
    private float attackRate;
    private Animator animator;
    private List<Collider> targets = new List<Collider>();

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        attackRate = (1f / stats.GetAttackSpeed());
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (type)
        {
            case EnemyType.Melee:
                if (!targetFound)
                {
                    TargetFinder();
                }
                else
                {
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
                break;
            case EnemyType.Ranged:
                if (!targetFound)
                {
                    TargetFinder();
                }
                else
                {
                    Vector3 direction = target.position - transform.position;
                    float rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, rotation, 0);
                    if (!attackCooldown)
                    {
                        RangedAttack();
                        StartCoroutine("AttackCooldown");
                    }
                }
                break;
        }
    }

    public void Hit(float damageTaken, Element element)
    {
        stats.TakeDamage(damageTaken, element);
        //aggro?
    }

    private void TargetFinder()
    {
        float searchSize;
        switch (type)
        {
            default:
            case EnemyType.Melee:
                searchSize = 8f;
                break;
            case EnemyType.Ranged:
                searchSize = 12f;
                break;
        }
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, searchSize);
        foreach (Collider col in hitColliders)
        {
            if (col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Summon_Spell"))
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, (col.transform.position - transform.position), out hit,  searchSize * 2, obstacles))
                {
                    if (hit.transform.CompareTag("Player") || hit.transform.CompareTag("Summon_Spell"))
                    {
                        target = hit.transform;
                        targetFound = true;
                    }
                }
            }
        }
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
        StartCoroutine("AttackCooldown");
    }

    private void RangedAttack()
    {
        StartCoroutine("AttackCooldown");
    }

    private IEnumerator AttackCooldown()
    {
        attackCooldown = true;
        yield return new WaitForSeconds(attackRate);
        targets.Clear();
        attackCooldown = false;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!targets.Contains(col))
        {
            targets.Add(col);
        }
        if (col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Summon_Spell"))
        {
            foreach (Collider collider in targets)
            {
                if (collider.gameObject.TryGetComponent(out PlayerManager player))
                {
                    player.Hit(stats.power.GetValue(), stats.element);
                }
                if (collider.gameObject.TryGetComponent(out SummoningSpell summon))
                {
                    summon.ReducePower(stats.power.GetValue(), stats.element);
                }
            }
        }
    }
}
