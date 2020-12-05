using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private float horizontal = default;
    private float vertical = default;
    private int tickAmount = default;
    private float tickSpeed = default;
    private bool allowMovement = true;
    private bool shielded = false;
    private Rigidbody rb = default;
    private PlayerStats playerstats = default;
    private bool allowHit = true;
    private GameObject shield = default;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerstats = GetComponent<PlayerStats>();
    }

    private void Start()
    {
        EquipmentManager.instance.onEquipmentChanged += onEquipmentChange;
    }

    private void FixedUpdate()
    {
        if (allowMovement)
            HandleMovement();
    }

    private void HandleMovement()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
        {
            Vector3 movement = new Vector3(horizontal, 0f, vertical);
            movement = movement.normalized * playerstats.movementspeed.GetValue() * Time.deltaTime;
            rb.MovePosition(transform.position + movement);
        }
    }

    public float GetSpeed()
    {
        return playerstats.movementspeed.GetValue();
    }

    public float GetHP()
    {
        return playerstats.GetCurrentHP();
    }

    #region HPHandlers

    public void Hit(float damageTaken, Element element)
    {
        if (allowHit)
        {
            if (!shielded)
            {
                playerstats.TakeDamage(damageTaken, element);
            }
            else
            {
                playerstats.TakeShieldDamage(damageTaken, element);
            }
        }
    }

    public void Heal(float healingReceived)
    {
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

    public void AllowMovement(bool status)
    {
        allowMovement = status;
        allowHit = status;
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

    public void SetShield(float power, Element element, GameObject shield)
    {
        shielded = true;
        this.shield = shield;
        playerstats.shieldHealth = power;
        playerstats.shieldElement = element;
    }

    public void RemoveShield()
    {
        shielded = false;
        if (shield != null)
        {
            Destroy(shield);
        }
    }
}
