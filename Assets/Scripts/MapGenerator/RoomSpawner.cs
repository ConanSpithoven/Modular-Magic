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
    private bool roomIsFiller = false;
    private RoomOpenings[] roomOpenings = new RoomOpenings[2];
    private int requiredOpeningCount = 0;
    private int openingMatches = 0;

    private void Start()
    {
        Destroy(gameObject, 4f);
        if (mapManager == null)
        {
            mapManager = MapManager.instance;
        }
        Invoke("Spawn", spawnDelay);
    }

    public void Setup(int requiredOpeningCount, RoomOpenings[] roomOpenings, int openingSide, bool roomIsFiller, bool spawned = false)
    {
        this.openingSide = (OpeningDir)openingSide;
        this.requiredOpeningCount = requiredOpeningCount;
        this.roomOpenings = roomOpenings;
        this.roomIsFiller = roomIsFiller;
        this.spawned = spawned;
    }

    private void Spawn()
    {
        if (!spawned)
        {
            rand = Random.Range(0, mapManager.GetRoomsLength((int)openingSide) - 1);
            GameObject room = mapManager.GetRooms((int)openingSide, rand);
            if (!mapManager.GetMinRoomsReached())
            {
                if (room.GetComponent<AddRoom>().GetOpeningCount() <= 1)
                {
                    Spawn();
                    return;
                }
            }
            if (mapManager.GetMaxRoomsReached())
            {
                room = mapManager.GetClosedRoom();
                Instantiate(room, transform.position, Quaternion.identity);
                mapManager.AddRoomCoords(transform.position);
                spawned = true;
                return;
            }
            if (roomIsFiller)
            {
                if (room.GetComponent<AddRoom>().GetOpeningCount() < requiredOpeningCount)
                {
                    Spawn();
                    return;
                }
                openingMatches = 0;
                foreach (RoomOpenings side in room.GetComponent<AddRoom>().GetRoomOpenings())
                {
                    if (side == roomOpenings[0] || side == roomOpenings[1])
                    {
                        openingMatches++;
                    }
                }
                if (openingMatches < roomOpenings.Length)
                {
                    Spawn();
                    return;
                }
            }
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
                        Spawn();
                    }
                    else
                    {
                        Instantiate(room, transform.position + room.GetComponent<AddRoom>().GetRoomOffset(), Quaternion.identity);
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset());
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(20, 0, 0));
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(0, 0, -20));
                        spawned = true;
                    }
                    break;
                case RoomType.TRCorner:
                    if (mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset()) || mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(-20, 0, 0)) || mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(0, 0, -20)))
                    {
                        Spawn();
                    }
                    else
                    {
                        Instantiate(room, transform.position + room.GetComponent<AddRoom>().GetRoomOffset(), Quaternion.identity);
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset());
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(-20, 0, 0));
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(0, 0, -20));
                        spawned = true;
                    }
                    break;
                case RoomType.BLCorner:
                    if (mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset()) || mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(20, 0, 0)) || mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(0, 0, 20)))
                    {
                        Spawn();
                    }
                    else
                    {
                        Instantiate(room, transform.position + room.GetComponent<AddRoom>().GetRoomOffset(), Quaternion.identity);
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset());
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(20, 0, 0));
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(0, 0, 20));
                        spawned = true;
                    }
                    break;
                case RoomType.BRCorner:
                    if (mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset()) || mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(-20, 0, 0)) || mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(0, 0, 20)))
                    {
                        Spawn();
                    }
                    else
                    {
                        Instantiate(room, transform.position + room.GetComponent<AddRoom>().GetRoomOffset(), Quaternion.identity);
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset());
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(-20, 0, 0));
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(0, 0, 20));
                        spawned = true;
                    }
                    break;
                case RoomType.Quad:
                    if (mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset()) || mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(20, 0, 0)) || mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(0, 0, 20)) || mapManager.CheckRoomCoordsTaken(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(20, 0, 20)))
                    {
                        Spawn();
                    }
                    else
                    {
                        Instantiate(room, transform.position + room.GetComponent<AddRoom>().GetRoomOffset(), Quaternion.identity);
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset());
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(20, 0, 0));
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(0, 0, 20));
                        mapManager.AddRoomCoords(transform.position + room.GetComponent<AddRoom>().GetRoomOffset() + new Vector3(20, 0, 20));
                        spawned = true;
                    }
                    break;
            }
        }
        else
        {
            Destroy(gameObject);
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
                    if (mapManager == null)
                    {
                        mapManager = MapManager.instance;
                    }
                    mapManager.OverlapManager(this, spawner);
                    //Instantiate(mapManager.GetClosedRoom(), transform.position, Quaternion.identity);
                }
            }
            spawned = true;
        }
    }

    //private void OnTriggerStay(Collider col)
    //{
    //    if (col.gameObject.CompareTag("SpawnPoint"))
    //    {
    //        if (col.gameObject.TryGetComponent(out RoomSpawner spawner))
    //        {
    //            if (spawner.spawned == false && spawned == false)
    //            {
    //                if (mapManager == null)
    //                {
    //                    mapManager = MapManager.instance;
    //                }
    //                Instantiate(mapManager.GetClosedRoom(), transform.position, Quaternion.identity);
    //            }
    //        }
    //        spawned = true;
    //    }
    //}

    public int GetOpeningSide()
    {
        return (int)openingSide;
    }
}
