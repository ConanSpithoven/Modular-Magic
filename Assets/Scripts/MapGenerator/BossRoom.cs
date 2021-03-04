using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private Transform bossPoint;
    [SerializeField] private Transform clearPoint;
    [SerializeField] private Transform itemPoint;
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
        SpawnItem();
    }

    private void SpawnItem()
    {
        GameObject spawnedItem = Instantiate(Resources.Load<GameObject>("Items/Item"), itemPoint.position, Quaternion.Euler(new Vector3(90, 0, 0)));
        string type;
        int rand = Random.Range(1, 10);
        switch (rand)
        {
            default:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
                type = "Patterns";
                break;
            case 7:
            case 8:
            case 9:
                type = "Consumables";
                break;
            case 10:
                type = "Equipment";
                break;
        }
        int maxIndex = GameManager.instance.GetItemCount(type);
        int index = Random.Range(0, maxIndex);
        Item item = spawnedItem.GetComponent<ItemPickup>().item = Resources.Load<Item>("Items/" + type + "/" + GameManager.instance.GetItem(index, type));
        spawnedItem.GetComponent<SpriteRenderer>().sprite = item.worldIcon;
    }
}
