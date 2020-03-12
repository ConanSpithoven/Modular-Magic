using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;
    [SerializeField] private Transform player = default;
    private float distance = default;

    private void Update()
    {
        distance = Vector3.Distance(player.position, transform.position);
        if (distance <= radius)
        {
            Debug.Log("interact");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
