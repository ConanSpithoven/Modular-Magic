using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    private enum OpeningDir { bottom, top, left, right }
    [SerializeField] private OpeningDir openingSide;
    [SerializeField] private float spawnDelay = 0.1f;

    private MapManager mapManager;
    private int rand;
    private bool spawned = false;

    private void Start()
    {
        Destroy(gameObject, 4f);
        mapManager = MapManager.instance;
        Invoke("Spawn", spawnDelay);
    }

    private void Spawn()
    {
        if (!spawned)
        {
            Physics.SyncTransforms();
            rand = Random.Range(0, mapManager.GetRoomsLength((int)openingSide) - 1);
            GameObject room = mapManager.GetRooms((int)openingSide, rand);
            //Check for possible overlap depending on roomtype
            switch (room.GetComponent<AddRoom>().GetRoomType())
            {
                default:
                case RoomType.Single:
                    Instantiate(room, transform.position, Quaternion.identity);
                    mapManager.AddRoomCoords(transform.position);
                    spawned = true;
                    break;
                case RoomType.TLCorner:
                    if (mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset()) || mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(20, 0, 0)) || mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(0, 0, -20)))
                    {
                        Debug.Log("Overlap!");
                        Spawn();
                    }
                    else
                    {
                        Instantiate(room, transform.position + room.GetComponent<AddRoom>().GetRoomOffset(), Quaternion.identity);
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset());
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(20, 0, 0));
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(0, 0, -20));
                    }
                    break;
                case RoomType.TRCorner:
                    if (mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset()) || mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(-20, 0, 0)) || mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(0, 0, -20)))
                    {
                        Debug.Log("Overlap!");
                        Spawn();
                    }
                    else
                    {
                        Instantiate(room, transform.position + room.GetComponent<AddRoom>().GetRoomOffset(), Quaternion.identity);
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset());
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(-20, 0, 0));
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(0, 0, -20));
                    }
                    break;
                case RoomType.BLCorner:
                    if (mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset()) || mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(20, 0, 0)) || mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(0, 0, 20)))
                    {
                        Debug.Log("Overlap!");
                        Spawn();
                    }
                    else
                    {
                        Instantiate(room, transform.position + room.GetComponent<AddRoom>().GetRoomOffset(), Quaternion.identity);
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset());
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(20, 0, 0));
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(0, 0, 20));
                    }
                    break;
                case RoomType.BRCorner:
                    if (mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset()) || mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(-20, 0, 0)) || mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(0, 0, 20)))
                    {
                        Debug.Log("Overlap!");
                        Spawn();
                    }
                    else
                    {
                        Instantiate(room, transform.position + room.GetComponent<AddRoom>().GetRoomOffset(), Quaternion.identity);
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset());
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(-20, 0, 0));
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(0, 0, 20));
                    }
                    break;
                case RoomType.Quad:
                    if (mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset()) || mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(20, 0, 0)) || mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(0, 0, 20)) || mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(20, 0, 20)))
                    {
                        Debug.Log("Overlap!");
                        Spawn();
                    }
                    else
                    {
                        Instantiate(room, transform.position + room.GetComponent<AddRoom>().GetRoomOffset(), Quaternion.identity);
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset());
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(20, 0, 0));
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(0, 0, 20));
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(20, 0, 20));
                    }
                    break;
            }
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
