using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private float HP = 100;

    public void Hit(float damageTaken)
    {
        TakeDamage(damageTaken);
    }

    void TakeDamage(float damageTaken)
    {
        HP -= damageTaken;
        if (HP <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
