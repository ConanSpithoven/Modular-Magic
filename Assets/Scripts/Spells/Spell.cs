using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

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
    public Animator animator;
    private NavMeshAgent spellAgent;

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
        float uniqueLimited = Mathf.Clamp(unique, 0f, Mathf.Infinity);
        this.unique = Mathf.RoundToInt(uniqueLimited);
    }

    public void SetSpeed(float speed)
    {
        float speedLimited = Mathf.Clamp(speed, 0.1f, Mathf.Infinity);
        this.speed = speedLimited;
    }

    public void SetSize(float size)
    {
        float sizeLimited = Mathf.Clamp(size, 0.1f, Mathf.Infinity);
        this.size = sizeLimited;
    }

    public void SetShape(int shape)
    {
        this.shape = shape;
    }

    public void SetLifetime(float lifetime)
    {
        float lifetimeLimited = Mathf.Clamp(lifetime, 0.1f, Mathf.Infinity);
        this.lifetime = lifetimeLimited;
    }

    public void SetInstances(int instances)
    {
        float instancesLimited = Mathf.Clamp(instances, 1f, Mathf.Infinity);
        this.instances = Mathf.RoundToInt(instancesLimited);
    }

    public void SetDamage(float damage)
    {
        float damageLimited = Mathf.Clamp(damage, 0.1f, Mathf.Infinity);
        this.power = damageLimited;
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
        if (model != null)
        {
            ElementMaterialModifier();
        }
    }

    private void ElementMaterialModifier()
    {
        SpellMeshModifier();
        string type = "";
        Renderer ren = model.GetComponent<Renderer>();
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
        Renderer newMaterial = Resources.Load<Renderer>("Materials/Elements/Spells/" + type + "/" + element.ElementName);
        ren.sharedMaterials = newMaterial.sharedMaterials;
    }

    private void SpellMeshModifier()
    {
        switch (spellType)
        {
            case SpellType.Projectile:
                ProjectileSpellShapeModifier();
                break;
            case SpellType.AOE:
                MeshSetter();
                break;
            case SpellType.Melee:
                MeleeSpellShapeModifier();
                break;
            case SpellType.Movement:
                MovementSpellShapeModifier();
                break;
            case SpellType.Heal:
                break;
            case SpellType.Shield:
                ShieldSpellShapeModifier();
                break;
            case SpellType.Summon:
                SummonSpellShapeModifier();
                return;
        }
        MeshSetter();
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
                break;
            case 1:
            case 2:
                model.transform.rotation = Quaternion.Euler(90, 0, 0);
                col.isTrigger = true;
                col.size = new Vector3(1,2,1);
                rb.constraints = RigidbodyConstraints.FreezeAll;
                break;
        }
    }

    private void MeleeSpellShapeModifier()
    {
        rb = GetComponent<Rigidbody>();
        CapsuleCollider col = model.GetComponent<CapsuleCollider>();
        Renderer ren = model.GetComponent<Renderer>();

        switch (shape)
        {
            case 0:
                model.transform.localPosition = new Vector3(0,0,1);
                model.transform.rotation = Quaternion.Euler(90,0,180);
                col.center = new Vector3(0,0,0);
                col.radius = 0.1f;
                col.height = 2;
                break;
            case 1:
                model.transform.localPosition = new Vector3(0, 0, -2);
                model.transform.rotation = Quaternion.Euler(90, 0, 0);
                col.center = new Vector3(0, 1.42f, 0);
                col.radius = 0.1f;
                col.height = 0.93f;
                break;
            case 2:
                model.transform.localPosition = new Vector3(0, 1.75f, 0.5f);
                model.transform.rotation = Quaternion.Euler(0, 0, 0);
                col.center = new Vector3(0f, 1.9f, 0.63f);
                col.radius = 0.3f;
                col.height = 0.91f;
                break;
        }
    }

    private void MovementSpellShapeModifier()
    {
        CapsuleCollider cCol = model.GetComponent<CapsuleCollider>();
        BoxCollider bCol = model.GetComponent<BoxCollider>();
        switch (shape)
        {
            case 0:
                model.SetActive(true);
                model.transform.localScale = new Vector3(1.2f,1.2f,1.2f);
                rb = GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.FreezeAll;
                rb.mass = 0;
                cCol.enabled = true;
                bCol.enabled = false;
                cCol.radius = 0.5f;
                cCol.height = 2;
                break;
            case 1:
                model.SetActive(false);
                break;
            case 2:
                model.SetActive(true);
                model.transform.localScale = new Vector3(2f, 1f, 0.5f);
                rb = GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.None;
                rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
                rb.mass = 100;
                cCol.enabled = false;
                bCol.enabled = true;
                bCol.size = new Vector3(1,2,1);
                break;
        }
    }

    private void ShieldSpellShapeModifier()
    {
        rb = GetComponent<Rigidbody>();
        CapsuleCollider cCol = model.GetComponent<CapsuleCollider>();
        BoxCollider bCol = model.GetComponent<BoxCollider>();

        switch (shape)
        {
            case 0:
                model.transform.rotation = Quaternion.Euler(0, 0, 0);
                model.transform.localScale = new Vector3(1.2f ,1.2f ,1.2f);
                cCol.enabled = true;
                bCol.enabled = false;
                cCol.radius = 0.5f;
                cCol.height = 2f;
                break;
            case 1:
            case 2:
                model.transform.rotation = Quaternion.Euler(0, 0, 90);
                model.transform.localScale = new Vector3(1, 1 ,1);
                cCol.enabled = false;
                bCol.enabled = true;
                bCol.size = new Vector3(1.33f, 0.77f, 0.15f);
                break;
        }
    }

    private void SummonSpellShapeModifier()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        spellAgent = GetComponent<NavMeshAgent>();
        model = Resources.Load<GameObject>("Models/Spells/Summon/" + element.ElementName + "/" + shape);
        string path = AssetDatabase.GetAssetPath(gameObject);
        GameObject newPrefab = Instantiate(this.gameObject, new Vector3(0, -100, 0), Quaternion.identity, null);
        GameObject oldModel = newPrefab.transform.Find("Model").gameObject;
        oldModel.transform.SetParent(null, true);
        Destroy(oldModel);
        Vector3 rot = Vector3.zero;

        switch (shape)
        {
            case 0:
                rot = new Vector3(0, 180, 180);
                break;
            case 1:
                rot = new Vector3(0,0,0);
                break;
            case 2:
                rot = new Vector3(0, 0, 0);
                break;
        }
        GameObject newModel = Instantiate(model, new Vector3(0, -100, 0), Quaternion.Euler(rot), null);
        newModel.transform.parent = newPrefab.transform;
        newModel.name = "Model";
        PrefabUtility.SaveAsPrefabAsset(newPrefab, path);
        DestroyImmediate(newPrefab);

        switch (shape)
        {
            case 0:
                animator.enabled = false;
                spellAgent.enabled = false;
                break;
            case 1:
                animator.enabled = false;
                spellAgent.enabled = true;
                GetComponent<SummoningSpell>().GetProjectile().ModifyElement(element, true);
                break;
            case 2:
                animator.enabled = true;
                animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Summon/2/SwordSummon");
                spellAgent.enabled = true;
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
                return;
        }
        GameObject newMesh = Resources.Load<GameObject>("Models/Spells/" + type + "/" + element.ElementName + "/" + shape);
        model.GetComponent<MeshFilter>().sharedMesh = newMesh.GetComponent<MeshFilter>().sharedMesh;
    }

    public void SetOriginShape(int newShape)
    {
        originShape = newShape;
    }

    public void SetOriginElement(Element newElement)
    {
        originElement = newElement;
    }

    public int GetOriginShape()
    {
        return originShape;
    }

    public Element GetOriginElement()
    {
        return originElement;
    }

    public float GetBaseCooldownTime()
    {
        switch (spellType)
        {
            default:
            case SpellType.Projectile:
                switch (shape)
                {
                    default:
                    case 0:
                        return 1;
                    case 1:
                        return 3+lifetime;
                    case 2:
                        return 2;
                }
            case SpellType.AOE:
                switch (shape)
                {
                    default:
                    case 0:
                        return 5;
                    case 1:
                        return 5+lifetime;
                    case 2:
                        return 3;
                }
            case SpellType.Shield:
                switch (shape)
                {
                    default:
                    case 0:
                        return 10+lifetime;
                    case 1:
                        return 8 + lifetime;
                    case 2:
                        return 5 + lifetime;
                }
            case SpellType.Movement:
                switch (shape)
                {
                    default:
                    case 0:
                        return 3;
                    case 1:
                        return 5;
                    case 2:
                        return 6;
                }
            case SpellType.Melee:
                switch (shape)
                {
                    default:
                    case 0:
                        return 1;
                    case 1:
                        return 3;
                    case 2:
                        return 6;
                }
            case SpellType.Heal:
                switch (shape)
                {
                    default:
                    case 0:
                        return 12;
                    case 1:
                        return 10 + lifetime;
                    case 2:
                        return 14;
                }
            case SpellType.Summon:
                switch (shape)
                {
                    default:
                    case 0:
                        return 5 + lifetime;
                    case 1:
                        return 15 + lifetime;
                    case 2:
                        return 10 + lifetime;
                }
        }
    }

    public void SetCooldown(float cdTime)
    {
        cooldownTime = cdTime;
    }
}
