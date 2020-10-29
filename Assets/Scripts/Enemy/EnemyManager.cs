using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;
    public bool targetFound = false;
    public bool attackCooldown = false;
    public float attackRate;
    public List<Collider> targets = new List<Collider>();

    [SerializeField] private EnemyStats stats;
    [SerializeField] private LayerMask obstacles;

    private Animator animator;
    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = stats.movementspeed.GetValue();
        attackRate = (1f / stats.attackSpeed.GetValue());
        animator = GetComponent<Animator>();
    }

    public virtual void Hit(float damageTaken, Element element)
    {
        stats.TakeDamage(damageTaken, element);
        //aggro?
    }

    public void TargetFinder(float searchSize)
    {
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

    public virtual void Attack()
    {
        
    }

    public void SetAnimator(string type, string name, bool status = false, float value = 0f)
    {
        switch (type)
        {
            default:
            case "Trigger":
                animator.SetTrigger(name);
                break;
            case "Bool":
                animator.SetBool(name, status);
                break;
            case "Float":
                animator.SetFloat(name, value);
                break;
        }
    }

    public virtual void OnTriggerEnter(Collider col)
    {
        if (!targets.Contains(col))
        {
            targets.Add(col);
        }
        int i = 0;
        foreach (Collider collider in targets)
        {
            if (collider != null)
            {
                if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Summon_Spell"))
                {
                    if (targets[i] == null)
                    {
                        targets.RemoveAt(i);
                    }
                    i++;
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

    public void SetScaling(int scaling)
    {
        stats.maxHealth.AddModifier((stats.maxHealth.GetValue() * (scaling * 0.1f)));
        stats.power.AddModifier(stats.power.GetValue() * (scaling * 0.05f));
        if (scaling > 15)
        {
            stats.armor.AddModifier(1 + (stats.armor.GetValue() * ((scaling - 5) * 0.1f)));
        }
        if (scaling > 10)
        {
            stats.attackSpeed.AddModifier(stats.attackSpeed.GetValue() * ((scaling -10) * 0.05f));
        }
        stats.SetScoreValue(Mathf.RoundToInt(10 * (1 + scaling)));
        stats.Setup();
    }

    public float GetDamage()
    {
        return stats.power.GetValue();
    }

    public virtual void OnDeath()
    {
        Destroy(gameObject);
    }
}
