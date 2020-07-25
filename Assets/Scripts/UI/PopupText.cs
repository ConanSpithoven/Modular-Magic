using UnityEngine;

public class PopupText : MonoBehaviour
{
    [SerializeField] private float distance = 1;
    [SerializeField] private float speed = 1f;
    private float zPos;

    void Start()
    {
        zPos = transform.position.z;
    }

    void Update()
    {
        if (transform.position.z < (zPos + distance))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed *Time.deltaTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
