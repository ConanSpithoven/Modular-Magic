using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private float horizontal = default;
    private float vertical = default;
    private int tickAmount = default;
    private float tickSpeed = default;
    private bool allowMovement = true;
    private Rigidbody rb = default;
    private PlayerStats playerstats = default;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerstats = GetComponent<PlayerStats>();
    }

    private void Start()
    {
        EquipmentManager.instance.onEquipmentChanged += onEquipmentChange;
    }

    private void Update()
    {
        if (allowMovement)
            HandleMovement();
    }

    private void HandleMovement() {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if(Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
        transform.Translate(horizontal * playerstats.movementspeed.GetValue() * Time.deltaTime, 0f, vertical * playerstats.movementspeed.GetValue() * Time.deltaTime);
    }

    public float GetSpeed() {
        return playerstats.movementspeed.GetValue();
    }

    public float GetHP()
    {
        return playerstats.currentHealth;
    }

    #region HPHandlers

    public void Hit(float damageTaken, Element element)
    {
        float totalDamage = damageTaken * CheckElement(element);
        playerstats.TakeDamage(totalDamage);
    }

    private float CheckElement(Element element)
    {
        float modifier = 1f;
        if (element.ElementName == playerstats.element.ElementName)
        {
            modifier = 0.5f;
        }
        else
        {
            foreach (string strength in element.ElementStrengths)
            {
                if (strength == playerstats.element.ElementName || strength == "All")
                {
                    modifier = 1.25f;
                }
            }
            foreach (string weakness in element.ElementWeaknesses)
            {
                if (weakness == playerstats.element.ElementName || weakness == "All")
                {
                    modifier = 0.75f;
                }
            }
            foreach (string strength in playerstats.element.ElementStrengths)
            {
                if (strength == element.ElementName || strength == "All")
                {
                    modifier = 0.75f;
                }
            }
            foreach (string weakness in playerstats.element.ElementWeaknesses)
            {
                if (weakness == element.ElementName || weakness == "All")
                {
                    modifier = 1.25f;
                }
            }
        }
        return modifier;
    }

    public void Heal(float healingReceived) {
        playerstats.Heal(healingReceived);
    }

    public void DOTHeal(float healingReceived, int tickAmount, float tickSpeed)
    {
        this.tickAmount = tickAmount;
        this.tickSpeed = tickSpeed;
        healingReceived /= tickAmount;
        StartCoroutine("DOTHealTick", healingReceived);
    }

    private IEnumerator DOTHealTick(float healingReceived)
    {
        for (int i = 0; i < tickAmount; i++)
        {
            Heal(healingReceived);
            yield return new WaitForSeconds(tickSpeed);
        }
    }

    public void PercentileHeal(float healingReceived)
    {
        Heal((playerstats.maxHealth.GetValue() / 100f) * healingReceived);
    }

    #endregion

    public void AllowMovement(bool status) {
        allowMovement = status;
    }

    public SpellInventory GetSpellInventory()
    {
        return gameObject.GetComponent<SpellInventory>();
    }

    public void onEquipmentChange(Equipment newItem, Equipment oldItem)
    {
        if (newItem != null)
        {
            if (newItem.equipType == EquipmentType.Cloak)
            {
                playerstats.SetElement(newItem.elementModifier);
            }
        }
        else 
        {
            playerstats.SetElement(playerstats.GetBaseElement());
        }
    }
}
