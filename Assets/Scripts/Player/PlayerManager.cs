using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float HP = 100f;
    [SerializeField] private float hpMax = 100f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private Element element;

    private float horizontal = default;
    private float vertical = default;
    private int tickAmount = default;
    private float tickSpeed = default;
    private bool allowMovement = true;
    private Rigidbody rb = default;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(allowMovement)
            HandleMovement();
    }

    private void HandleMovement() {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if(Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
        transform.Translate(horizontal * speed * Time.deltaTime, 0f, vertical *speed * Time.deltaTime);
    }

    public float GetSpeed() {
        return speed;
    }

    public float GetHP()
    {
        return HP;
    }

    public void Hit(float damageTaken, Element element)
    {
        float totalDamage = damageTaken * CheckElement(element);
        TakeDamage(totalDamage);
    }

    public void TakeDamage(float damageTaken)
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

    public void Heal(float healingReceived) {
        HP += healingReceived;
        HP = Mathf.Clamp(HP, 0, hpMax);
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
        Heal((hpMax / 100f) * healingReceived);
    }

    public void AllowMovement(bool status) {
        allowMovement = status;
    }
}
