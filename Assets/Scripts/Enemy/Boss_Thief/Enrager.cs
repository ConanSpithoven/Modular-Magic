using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enrager : StateMachineBehaviour
{

    [SerializeField] private float EnrageThreshhold = 10;

    private EnemyStats enemyStats;
    private Enemy_Boss_Thief boss;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyStats = animator.GetComponent<EnemyStats>();
        boss = animator.GetComponent<Enemy_Boss_Thief>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(boss.targetFound)
        {
            if (!boss.rangedAttackCooldown)
            {
                boss.RangedAttack();
            }
            else
            {
                if (boss.alternate)
                {
                    boss.alternate = false;
                    boss.Alternate();
                }
                if (boss.move)
                {
                    boss.MoveToPlayer();
                }
                else
                {
                    boss.ClearTarget();
                    boss.LookAtPlayer();
                }
            }
        }
        if (enemyStats.GetCurrentHP() <= EnrageThreshhold)
        {
            boss.Enrage();
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.ClearTarget();
    }
}
