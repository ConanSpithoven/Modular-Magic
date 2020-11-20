using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private Transform bossPoint;
    [SerializeField] private Transform clearPoint;
    [SerializeField] private GameObject wall;
    [SerializeField] private string bossName;

    private RoomCloser roomCloser;
    private int enemyCount;
    private bool instantiated = false;
    private bool cleared = false;

    private void Start()
    {
        if (spawnPoints.Count > 0)
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
        GameObject boss = Instantiate(Resources.Load<GameObject>("Enemies/" + bossName), bossPoint.position, Quaternion.identity);
        boss.GetComponent<Enemy_Boss_Thief>().SetBossRoom(this);
        enemyCount++;

        roomCloser = transform.parent.GetComponentInChildren<RoomCloser>();
        instantiated = true;
    }

    private void Update()
    {
        if (instantiated && enemyCount <= 0 && !cleared)
        {
            cleared = true;
            roomCloser.OperateDoors(false);
            FloorClear();
        }
    }

    public void ReduceCount()
    {
        enemyCount--;
    }

    private void FloorClear()
    {
        Instantiate(Resources.Load<GameObject>("Map/FloorClearStair"), clearPoint.position, Quaternion.identity);
    }
}
