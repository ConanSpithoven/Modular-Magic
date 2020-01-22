using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float HP = 100f;
    [SerializeField] private float hpMax = 100f;
    [SerializeField] private float speed = 5f;

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

    public void Hit(float damageTaken)
    {
        TakeDamage(damageTaken);
    }

    public void TakeDamage(float damageTaken)
    {
        HP -= damageTaken;
        if (HP <= 0f)
        {
            Destroy(gameObject);
        }
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
