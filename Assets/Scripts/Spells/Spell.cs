using UnityEngine;

public class Spell : MonoBehaviour
{
    [Header("General spell settings")]
    public SpellType spellType;
    public float cooldownTime;
    public bool onCooldown;
    public int shape;
    public float damage;
    public float lifetime;
    public float size;
    public int instances;
    public float speed;
    public int unique;
    public int spellSlot;
    public Element element;
    private GameObject[] upgradeSlots;
    public int upgradeLimit;

    public void SetSlot(int spellSlot)
    {
        this.spellSlot = spellSlot;
    }

    public int GetSpellSlot()
    {
        return spellSlot;
    }

    public void SetElement(Element element)
    {
        this.element = element;
    }

    public void SetUnique(int unique)
    {
        this.unique = unique;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetSize(float size)
    {
        this.size = size;
    }

    public void SetShape(int shape)
    {
        this.shape = shape;
    }

    public void SetLifetime(float lifetime)
    {
        this.lifetime = lifetime;
    }

    public void SetInstances(int instances)
    {
        this.instances = instances;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public float GetDamage()
    {
        return damage;
    }

    public void ReducePower(float damage, Element element)
    {
        float totalDamage = damage * CheckElement(element);
        this.damage -= totalDamage;
        if (this.damage <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public float CheckElement(Element element)
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

    public void AddUpgrade(GameObject upgrade)
    {
        for (int i = 0; i < upgradeLimit-1; i++)
        {
            if (upgradeSlots[i] == null)
            {
                upgradeSlots[i] = upgrade;
            }
        }
        ProcessUpgrade(upgrade.GetComponent<Pattern>(), true);
    }

    public void RemoveUpgrade(GameObject upgrade)
    {
        for (int i = 0; i < upgradeSlots.Length; i++)
        {
            if (upgradeSlots[i] == upgrade)
            {
                upgradeSlots[i] = null;
            }
        }
        ProcessUpgrade(upgrade.GetComponent<Pattern>(), false);
    }

    private void ProcessUpgrade(Pattern upgrade, bool status)
    {
        if (status)
        {
            damage += upgrade.damage;
            lifetime += upgrade.lifetime;
            size += upgrade.size;
            instances += upgrade.instances;
            speed += upgrade.speed;
            unique += upgrade.unique;
        }
        else 
        {
            damage -= upgrade.damage;
            lifetime -= upgrade.lifetime;
            size -= upgrade.size;
            instances -= upgrade.instances;
            speed -= upgrade.speed;
            unique -= upgrade.unique;
        }
    }
}
