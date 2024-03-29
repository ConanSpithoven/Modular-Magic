﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterStats : MonoBehaviour
{
    public Stat maxHealth;
    [SerializeField] private float currentHealth;
    public Element element;
    [SerializeField] private Element baseElement;
    public Stat power;
    public Stat armor;
    public Stat movementspeed;

    void Awake()
    {
        currentHealth = maxHealth.GetValue();
    }

    public Element GetBaseElement()
    {
        return baseElement;
    }

    public virtual void TakeDamage(float damageTaken, Element element)
    {
        float elementModifier = CheckElement(this.element, element);
        float totalDamage = damageTaken * elementModifier;
        totalDamage -= armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, float.MaxValue);
        currentHealth -= totalDamage;
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Heal(float healing)
    {
        currentHealth += healing;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth.GetValue());
    }

    public virtual void Die()
    {
        //oof
    }

    public void SetElement(Element newElement)
    {
        element = newElement;
    }

    public float CheckElement(Element ownElement, Element element)
    {
        float modifier = 1f;
        if (element.ElementName == ownElement.ElementName)
        {
            modifier = 0.5f;
        }
        else
        {
            foreach (string strength in element.ElementStrengths)
            {
                if (strength == ownElement.ElementName || strength == "All")
                {
                    modifier = 1.25f;
                }
            }
            foreach (string weakness in element.ElementWeaknesses)
            {
                if (weakness == ownElement.ElementName || weakness == "All")
                {
                    modifier = 0.75f;
                }
            }
            foreach (string strength in ownElement.ElementStrengths)
            {
                if (strength == element.ElementName || strength == "All")
                {
                    modifier = 0.75f;
                }
            }
            foreach (string weakness in ownElement.ElementWeaknesses)
            {
                if (weakness == element.ElementName || weakness == "All")
                {
                    modifier = 1.25f;
                }
            }
        }
        return modifier;
    }

    public float GetCurrentHP()
    {
        return currentHealth;
    }

    public void ChangeCurrentHP(float value, bool adding)
    {
        if (adding)
        {
            currentHealth += value;
        }
        else
        {
            currentHealth -= value;
        }
    }

    public void LoadedPlayer(PlayerData data)
    {
        currentHealth = data.currentHealth;
    }

    public void Setup()
    {
        currentHealth = maxHealth.GetValue();
    }
}
