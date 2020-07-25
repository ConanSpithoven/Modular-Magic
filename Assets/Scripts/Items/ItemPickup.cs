using System.Collections;
using TMPro;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    public float radius = 3f;
    [SerializeField] private Transform player = default;
    private float distance = default;
    private bool ready = false;

    private void Start()
    {
        player = FindObjectOfType<PlayerManager>().transform;
        StartCoroutine("Setup");
    }

    private void Update()
    {
        if (!Inventory.instance.full && ready)
        {
            distance = Vector3.Distance(player.position, transform.position);
            if (distance <= radius)
            {
                float step = (1f + radius - distance) * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, player.position, step);
                transform.position = new Vector3(transform.position.x, 0.3f, transform.position.z);
            }
        }
    }

    public virtual void OnPickUp()
    {
        if (Inventory.instance.Add(item))
        {
            LootTextPopup();
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, radius);
    }

    private void OnCollisionStay(Collision col)
    {
        if (col.gameObject.CompareTag("Player") && !Inventory.instance.full && ready)
        {
            OnPickUp();
        }
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player") && !Inventory.instance.full && ready)
        {
            OnPickUp();
        }
    }

    private IEnumerator Setup()
    {
        yield return new WaitForSecondsRealtime(2);
        ready = true;
    }

    private void LootTextPopup()
    {
        GameObject textPopup = Instantiate(Resources.Load<GameObject>("Popups/DmgText"), new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + 3, transform.position.z + Random.Range(-0.5f, 0.5f)), Quaternion.Euler(90, 0, 0));
        TextMeshPro text = textPopup.GetComponent<TextMeshPro>();
        text.text = item.name;
        //rarity color
    }
}
