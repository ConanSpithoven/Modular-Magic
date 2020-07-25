using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat maxHealth;
    public float currentHealth { get; private set; }
    public Element element;
    [SerializeField] private Element baseElement;
    public Stat power;
    public Stat armor;
    public Stat movementspeed;
    public Stat lifetime;
    public Stat size;
    public Stat speed;
    public Stat instances;
    public Stat unique;
    public Stat cooldownReduction;

    void Awake()
    {
        currentHealth = maxHealth.GetValue();
    }

    public Element GetBaseElement()
    {
        return baseElement;
    }

    public void TakeDamage(float damage)
    {
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 0, float.MaxValue);
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float healing)
    {
        currentHealth += healing;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth.GetValue());
    }

    public virtual void Die()
    { 
        
    }

    public void SetElement(Element newElement)
    {
        element = newElement;
    }
}
