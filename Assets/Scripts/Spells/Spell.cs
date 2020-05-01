using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spell : MonoBehaviour
{
    [Header("General spell settings")]
    public SpellType spellType;
    public float cooldownTime;
    public bool onCooldown;
    public int shape;
    [SerializeField] private int originShape;
    public float power;
    public float lifetime;
    public float size;
    public int instances = 1;
    public float speed;
    public int unique;
    public int spellSlot;
    public Element element;
    [SerializeField]private Element originElement;
    public int upgradeLimit = 5;
    [SerializeField] private GameObject model;
    private Rigidbody rb;

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
        this.power = damage;
    }

    public float GetDamage()
    {
        return power;
    }

    public GameObject GetModel()
    {
        return model;
    }

    public void ReducePower(float damage, Element element)
    {
        float totalDamage = damage * CheckElement(element);
        this.power -= totalDamage;
        if (this.power <= 0f)
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

    public int GetUpgradeLimit()
    {
        return upgradeLimit;
    }

    public void ModifyElement(Element newElement, bool status)
    {
        if (status)
        {
            element = newElement;
        }
        else 
        {
            element = originElement;
        }
        ElementMaterialModifier();
    }

    public void ModifyShape(int newShape, bool status)
    {
        if (status)
        {
            shape = newShape;
        }
        else
        {
            shape = originShape;
        }
        SpellMeshModifier();
    }

    private void ElementMaterialModifier()
    {
        string type = "";
        switch (spellType)
        {
            case SpellType.Projectile:
                type = "Projectile";
                break;
            case SpellType.AOE:
                type = "AoE";
                break;
            case SpellType.Melee:
                type = "Melee";
                break;
            case SpellType.Movement:
                type = "Movement";
                break;
            case SpellType.Heal:
                type = "Heal";
                break;
            case SpellType.Shield:
                type = "Shield";
                break;
            case SpellType.Summon:
                type = "Summon";
                break;
        }
        Debug.Log(element.ElementName);
        GameObject newMaterial = Resources.Load<GameObject>("Materials/Elements/Spells/" + type + "/" + element.ElementName);
        Debug.Log(newMaterial.GetComponent<Renderer>().sharedMaterials);
        model.GetComponent<Renderer>().sharedMaterials = newMaterial.GetComponent<Renderer>().sharedMaterials;
        SpellMeshModifier();
    }

    private void SpellMeshModifier()
    {
        switch (spellType)
        {
            case SpellType.Projectile:
                ProjectileSpellShapeModifier();
                break;
            case SpellType.AOE:
                break;
            case SpellType.Melee:
                break;
            case SpellType.Movement:
                break;
            case SpellType.Heal:
                break;
            case SpellType.Shield:
                break;
            case SpellType.Summon:
                break;
        }
    }

    private void ProjectileSpellShapeModifier()
    {
        rb = GetComponent<Rigidbody>();
        BoxCollider col = model.GetComponent<BoxCollider>();
        
        switch (shape)
        {
            case 0:
                model.transform.rotation = Quaternion.Euler(0,0,0);
                col.isTrigger = false;
                col.size = new Vector3(1,1,1);
                rb.constraints = RigidbodyConstraints.None;
                rb.constraints = RigidbodyConstraints.FreezePositionY;
                MeshSetter();
                break;
            case 1:
            case 2:
                model.transform.rotation = Quaternion.Euler(90, 0, 0);
                col.isTrigger = true;
                col.size = new Vector3(1,2,1);
                rb.constraints = RigidbodyConstraints.FreezeAll;
                MeshSetter();
                break;
        }
    }

    private void MeshSetter()
    {
        string type = "";
        switch (spellType)
        {
            case SpellType.Projectile:
                type = "Projectile";
                break;
            case SpellType.AOE:
                type = "AoE";
                break;
            case SpellType.Melee:
                type = "Melee";
                break;
            case SpellType.Movement:
                type = "Movement";
                break;
            case SpellType.Heal:
                type = "Heal";
                break;
            case SpellType.Shield:
                type = "Shield";
                break;
            case SpellType.Summon:
                type = "Summon";
                break;
        }
        
        if (model.GetComponent<MeshFilter>() != null)
        {
            GameObject newMesh = Resources.Load<GameObject>("Models/Spells/" + type + "/" + element.ElementName + "/" + shape);
            model.GetComponent<MeshFilter>().sharedMesh = newMesh.GetComponent<MeshFilter>().sharedMesh;
        }
    }
}
