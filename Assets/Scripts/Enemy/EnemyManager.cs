using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private CharacterStats characterStats;

    public void Hit(float damageTaken, Element element)
    {
        characterStats.TakeDamage(damageTaken, element);
    }
}
