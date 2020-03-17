using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    public float radius = 3f;
    [SerializeField] private Transform player = default;
    private float distance = default;

    private void Update()
    {
        distance = Vector3.Distance(player.position, transform.position);
        if (distance <= radius)
        {
            float step = 2f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.position, step);
        }
    }

    public virtual void OnPickUp()
    {
        if(Inventory.instance.Add(item))
            Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (player == null)
            player = FindObjectOfType<PlayerManager>().transform;
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, radius);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            OnPickUp();
        }
    }
}
