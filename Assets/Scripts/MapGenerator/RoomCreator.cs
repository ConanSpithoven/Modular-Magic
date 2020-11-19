using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCreator : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private List<Transform> enemyPoints;
    [SerializeField] private GameObject wall;
    [SerializeField] private int enemyLimit;

    private RoomCloser roomCloser;
    private int enemyCount;
    private bool instantiated = false;
    private bool opened = false;

    private void Start()
    {
        if(spawnPoints.Count > 0)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                if (spawnPoint != null)
                {
                    int rand = Random.Range(1, 3);
                    if (rand == 1)
                    {
                        GameObject Wall = Instantiate(wall, spawnPoint.position, Quaternion.identity, transform);
                        Wall.transform.localScale = new Vector3(wall.transform.localScale.x / transform.localScale.x, wall.transform.localScale.y / transform.localScale.y, wall.transform.localScale.z / transform.localScale.z);
                    }
                }
            }
        }
        if (enemyPoints.Count > 0 && enemyLimit > 0)
        {
            int rand = Random.Range(0, enemyLimit);
            for (int i = 0; i <= rand; i++)
            {
                int enemytype = Random.Range(0, 10);
                int point = Random.Range(0, enemyPoints.Count-1);
                if (enemytype < 7)
                {
                    GameObject enemy = Instantiate(Resources.Load<GameObject>("Enemies/Warrior"), enemyPoints[point].position, Quaternion.identity);
                    enemy.GetComponent<Enemy_Melee>().SetRoomCreator(this);
                    enemyCount++;
                }
                else
                {
                    GameObject enemy = Instantiate(Resources.Load<GameObject>("Enemies/Mage"), enemyPoints[point].position, Quaternion.identity);
                    enemy.GetComponent<Enemy_Ranged>().SetRoomCreator(this);
                    enemyCount++;
                }
            }
        }
        roomCloser = transform.parent.GetComponentInChildren<RoomCloser>();
        instantiated = true;
    }

    public void ReduceCount()
    {
        enemyCount--;
    }

    private void Update()
    {
        if (instantiated && enemyCount <= 0 && !opened)
        {
            opened = true;
            roomCloser.OperateDoors(false);
        }
    }
}
