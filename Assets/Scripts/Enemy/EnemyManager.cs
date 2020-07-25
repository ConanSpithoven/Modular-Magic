using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private float HP = 100;
    [SerializeField] private Element element;

    public void Hit(float damageTaken, Element element)
    {
        float elementModifier = CheckElement(element);
        float totalDamage = damageTaken * elementModifier;
        DmgTextPopup(totalDamage, elementModifier);
        TakeDamage(totalDamage);
    }

    void TakeDamage(float damageTaken)
    {
        HP -= damageTaken;
        if (HP <= 0f)
        {
            Destroy(gameObject);
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

    private void DmgTextPopup(float damage, float modifier)
    {
        GameObject textPopup = Instantiate(Resources.Load<GameObject>("Popups/DmgText"), new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + 3, transform.position.z + Random.Range(-0.5f, 0.5f)), Quaternion.Euler(90,0,0));
        TextMeshPro text = textPopup.GetComponent<TextMeshPro>();
        text.text = "" + damage;
        switch (modifier)
        {
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
}
