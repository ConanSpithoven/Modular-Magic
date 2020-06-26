using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    public float radius = 3f;
    [SerializeField] private Transform player = default;
    private float distance = default;

    private void Start()
    {
        player = FindObjectOfType<PlayerManager>().transform;
    }

    private void Update()
    {
        if (!Inventory.instance.full)
        {
            distance = Vector3.Distance(player.position, transform.position);
            if (distance <= radius)
            {
                float step = ((1f+ radius) - distance) * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, player.position, step);
                transform.position = new Vector3(transform.position.x, 0.3f, transform.position.z);
            }
        }
    }

    public virtual void OnPickUp()
    {
        if(Inventory.instance.Add(item))
            Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, radius);
    }

    private void OnCollisionStay(Collision col)
    {
        if (col.gameObject.CompareTag("Player") && !Inventory.instance.full)
        {
            OnPickUp();
        }
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player") && !Inventory.instance.full)
        {
            OnPickUp();
        }
    }
}
