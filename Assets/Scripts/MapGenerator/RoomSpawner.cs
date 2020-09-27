using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    private enum OpeningDir { bottom, top, left, right}
    [SerializeField] private OpeningDir openingSide;

    private MapManager mapManager;
    private int rand;
    private bool spawned = false;

    private void Start()
    {
        Destroy(gameObject, 4f);
        mapManager = MapManager.instance;
        Invoke("Spawn", 0.1f);
    }

    private void Spawn()
    {
        if (!spawned)
        {
            rand = Random.Range(0, mapManager.GetRoomsLength((int)openingSide)-1);
            Instantiate(mapManager.GetRooms((int)openingSide, rand), transform.position, Quaternion.identity);
            spawned = true;
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("SpawnPoint"))
        {
            if (col.gameObject.TryGetComponent(out RoomSpawner spawner))
            {
                if (spawner.spawned == false && spawned == false)
                {
                    Instantiate(mapManager.GetClosedRoom(), transform.position, Quaternion.identity);
                }
            }
            spawned = true;
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("SpawnPoint"))
        {
            if (col.gameObject.TryGetComponent(out RoomSpawner spawner))
            {
                if (spawner.spawned == false && spawned == false)
                {
                    Instantiate(mapManager.GetClosedRoom(), transform.position, Quaternion.identity);
                }
            }
            spawned = true;
        }
    }
}
