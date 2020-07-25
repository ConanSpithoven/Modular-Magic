using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterStats : MonoBehaviour
{
    public Stat maxHealth;
    public float currentHealth { get; private set; }
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

    public void TakeDamage(float damageTaken, Element element)
    {
        float elementModifier = CheckElement(element);
        float totalDamage = damageTaken * elementModifier;
        totalDamage -= armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, float.MaxValue);
        currentHealth -= totalDamage;
        DmgTextPopup(totalDamage, elementModifier);
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
        Destroy(gameObject);
    }

    public void SetElement(Element newElement)
    {
        element = newElement;
    }

    private void DmgTextPopup(float damage, float modifier)
    {
        GameObject textPopup = Instantiate(Resources.Load<GameObject>("Popups/DmgText"), new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + 3, transform.position.z + Random.Range(-0.5f, 0.5f)), Quaternion.Euler(90, 0, 0));
        TextMeshPro text = textPopup.GetComponent<TextMeshPro>();
        text.text = "" + damage;
        switch (modifier)
        {
            case 0.5f:
                text.fontSize = 4;
                text.color = Color.cyan;
                break;
            case 0.75f:
                text.fontSize = 5;
                text.color = Color.blue;
                break;
            case 1f:
                text.fontSize = 6;
                text.color = Color.white;
                break;
            case 1.25f:
                text.fontSize = 7;
                text.color = Color.red;
                break;
        }
    }

    private float CheckElement(Element element)
    {
        float modifier = 1f;
        if (element.ElementName == this.element.ElementName)
        {
            modifier = 0.5f;
        }
        else
        {
            foreach (string strength in element.ElementStrengths)
            {
                if (strength == this.element.ElementName || strength == "All")
                {
                    modifier = 1.25f;
                }
            }
            foreach (string weakness in element.ElementWeaknesses)
            {
                if (weakness == this.element.ElementName || weakness == "All")
                {
                    modifier = 0.75f;
                }
            }
            foreach (string strength in this.element.ElementStrengths)
            {
                if (strength == element.ElementName || strength == "All")
                {
                    modifier = 0.75f;
                }
            }
            foreach (string weakness in this.element.ElementWeaknesses)
            {
                if (weakness == element.ElementName || weakness == "All")
                {
                    modifier = 1.25f;
                }
            }
        }
        return modifier;
    }
}
