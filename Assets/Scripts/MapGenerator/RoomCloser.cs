using UnityEngine;

public class RoomCloser : MonoBehaviour
{
    [SerializeField] private GameObject[] doors;
    private bool opened;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player") && !opened)
        {
            OperateDoors(true);
        }
    }

    public void OperateDoors(bool status)
    {
        if (status)
        {
            opened = true;
            foreach (GameObject door in doors)
            {
                door.GetComponent<Transform>().Translate(new Vector3(0, 1.9f, 0));
                door.GetComponent<Transform>().GetChild(0).GetComponent<BoxCollider>().enabled = false;
            }
        }
        else 
        {
            foreach (GameObject door in doors)
            {
                door.GetComponent<Transform>().Translate(new Vector3(0, -1.9f, 0));
            }
        }
    }
}
