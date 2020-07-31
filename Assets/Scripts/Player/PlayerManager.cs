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
    private CapsuleCollider playerCollider = default;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerstats = GetComponent<PlayerStats>();
        playerCollider = GetComponent<CapsuleCollider>();
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

        if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
        {
            Vector3 movement = new Vector3(horizontal, 0f, vertical);
            movement = movement.normalized * playerstats.movementspeed.GetValue() * Time.deltaTime;
            rb.MovePosition(transform.position + movement);
        }
    }

    public float GetSpeed() {
        return playerstats.movementspeed.GetValue();
    }

    public float GetHP()
    {
        return playerstats.GetCurrentHP();
    }

    #region HPHandlers

    public void Hit(float damageTaken, Element element)
    {
        playerstats.TakeDamage(damageTaken, element);
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
        playerCollider.enabled = status;
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
