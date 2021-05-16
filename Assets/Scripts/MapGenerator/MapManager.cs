using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> rooms;

    [SerializeField] private GameObject[] bottomRooms;
    [SerializeField] private GameObject[] topRooms;
    [SerializeField] private GameObject[] leftRooms;
    [SerializeField] private GameObject[] rightRooms;
    [SerializeField] private GameObject closedRoom;
    [SerializeField] private GameObject[] normalInteriors;
    [SerializeField] private GameObject[] TLCornerInteriors;
    [SerializeField] private GameObject[] TRCornerInteriors;
    [SerializeField] private GameObject[] BLCornerInteriors;
    [SerializeField] private GameObject[] BRCornerInteriors;
    [SerializeField] private GameObject[] QuadInteriors;
    [SerializeField] private GameObject lootInteriors;
    [SerializeField] private GameObject boss;
    [SerializeField] private int lootRoomLimit = 1;
    [SerializeField] private int maxRooms = 20;
    [SerializeField] private int minRooms = 8;

    private List<Vector3> roomCoords = new List<Vector3>();
    private int bossRoom;

    private bool minRoomsReached = false;
    private bool maxRoomsReached = false;
    private bool bossSpawned = false;
    [SerializeField] private List<GameObject> normalRoomsNumbers;
    private int lootRoomsSpawned;

    #region# Singelton
    public static MapManager instance;

    private void Awake()
    {
        instance = this;       
    }
    #endregion#

    public void AddRoom(GameObject room)
    {
        rooms.Add(room);
        if (rooms.Count >= minRooms)
        {
            minRoomsReached = true;
        }
        if (rooms.Count == maxRooms)
        {
            maxRoomsReached = true;
        }
        StartCoroutine("CheckSpawnFinish");
    }

    public GameObject GetRooms(int side, int index)
    {
        switch (side)
        {
            default:
            case 0:
                return topRooms[index];
            case 1:
                return bottomRooms[index];
            case 2:
                return rightRooms[index];
            case 3:
                return leftRooms[index];
        }
    }

    public GameObject GetClosedRoom()
    {
        return closedRoom;
    }

    public int GetRoomsLength(int side)
    {
        switch (side)
        {
            default:
            case 0:
                return bottomRooms.Length;
            case 1:
                return topRooms.Length;
            case 2:
                return leftRooms.Length;
            case 3:
                return rightRooms.Length;
        }
    }

    private IEnumerator CheckSpawnFinish()
    {
        int oldRoomCount = rooms.Count;
        yield return new WaitForSeconds(1);
        if (oldRoomCount == rooms.Count && !bossSpawned)
        {
            lootRoomsSpawned = 0;
            GetBossRoomNumber(0);
        }
    }

    private void CalcMapBounds()
    {
        float xMin = 0;
        float xMax = 0;
        float zMin = 0;
        float zMax = 0;

        foreach (GameObject room in rooms)
        {
            if (room.transform.position.x > xMax)
            {
                xMax = room.transform.position.x;
            }
            else if (room.transform.position.x < xMin)
            {
                xMin = room.transform.position.x;
            }
            if (room.transform.position.z > zMax)
            {
                zMax = room.transform.position.z;
            }
            else if (room.transform.position.z < zMin)
            {
                zMin = room.transform.position.z;
            }
        }
        //add room size to camera bounds
        xMax += 12;
        xMin -= 12;
        zMax += 12;
        zMin -= 12;

        GameManager.instance.SetCameraBounds(xMin, xMax, zMin, zMax);
    }

    private void SpawnLootInteriors()
    {
        if (lootRoomsSpawned < lootRoomLimit)
        {
            int rand = Random.Range(0, normalRoomsNumbers.Count);
            int number = rooms.IndexOf(normalRoomsNumbers[rand]);
            if (rooms[number].GetComponent<AddRoom>().GetRoomType() == RoomType.Single)
            {
                Instantiate(lootInteriors, rooms[number].transform.position, Quaternion.identity, rooms[number].transform);
                lootRoomsSpawned++;
                normalRoomsNumbers.Remove(normalRoomsNumbers[rand]);
                Destroy(rooms[number].GetComponentInChildren<RoomCloser>().gameObject);
            }
            SpawnLootInteriors();
        }
        else 
        {
            SpawnNormalInteriors();
        }
    }

    private void SpawnNormalInteriors()
    {
        foreach (GameObject room in normalRoomsNumbers)
        {
            int number = rooms.IndexOf(room);
            GameObject currentRoom = rooms[number];
            switch (currentRoom.GetComponent<AddRoom>().GetRoomType())
            {
                default:
                case RoomType.Single:
                    Instantiate(normalInteriors[Random.Range(0, (normalInteriors.Length - 1))], currentRoom.transform.position, Quaternion.identity, currentRoom.transform);
                    break;
                case RoomType.TLCorner:
                    GameObject TLfloor = TLCornerInteriors[Random.Range(0, TLCornerInteriors.Length - 1)];
                    Instantiate(TLfloor, currentRoom.transform.position, TLfloor.transform.rotation, currentRoom.transform);
                    break;
                case RoomType.TRCorner:
                    GameObject TRfloor = TRCornerInteriors[Random.Range(0, TRCornerInteriors.Length - 1)];
                    Instantiate(TRfloor, currentRoom.transform.position, TRfloor.transform.rotation, currentRoom.transform);
                    break;
                case RoomType.BLCorner:
                    GameObject BLfloor = BLCornerInteriors[Random.Range(0, BLCornerInteriors.Length - 1)];
                    Instantiate(BLfloor, currentRoom.transform.position, BLfloor.transform.rotation, currentRoom.transform);
                    break;
                case RoomType.BRCorner:
                    GameObject BRfloor = BRCornerInteriors[Random.Range(0, BRCornerInteriors.Length - 1)];
                    Instantiate(BRfloor, currentRoom.transform.position, BRfloor.transform.rotation, currentRoom.transform);
                    break;
                case RoomType.Quad:
                    Instantiate(QuadInteriors[Random.Range(0, (QuadInteriors.Length - 1))], currentRoom.transform.position, Quaternion.identity, currentRoom.transform);
                    break;
            }
            
        }
    }

    private void GetBossRoomNumber(int i)
    {
        int bossRoom = rooms.Count - (1 + i);
        if (rooms[bossRoom].GetComponent<AddRoom>().GetRoomType() == RoomType.Single)
        {
            Instantiate(boss, rooms[bossRoom].transform.position, Quaternion.identity, rooms[bossRoom].transform);
            bossSpawned = true;
            this.bossRoom = bossRoom;
            SpawnRooms();
        }
        else
        {
            GetBossRoomNumber(i + 1);
        }
    }

    private void SpawnRooms()
    {
        for (int i = 1; i < rooms.Count; i++)
        {
            if (i != bossRoom)
            {
                if (rooms[i].GetComponent<AddRoom>().GetRoomType() == RoomType.Single)
                {
                    int rand = Random.Range(0, 20);
                    if (rand >= 0 && rand <= 19)
                    {
                        normalRoomsNumbers.Add(rooms[i]);
                    }
                    else if (lootRoomsSpawned < lootRoomLimit)
                    {
                        lootRoomsSpawned++;
                        Instantiate(lootInteriors, rooms[i].transform.position, Quaternion.identity, rooms[i].transform);
                    }
                }
                else
                {
                    normalRoomsNumbers.Add(rooms[i]);
                }
            }
        }
        SpawnLootInteriors();
        CalcMapBounds();
    }

    public void AddRoomCoords(Vector3 coords)
    {
        roomCoords.Add(coords);
    }

    public bool CheckRoomCoordsTaken(Vector3 coords)
    {
        if (roomCoords.Contains(coords))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetMinRoomsReached()
    {
        return minRoomsReached;
    }

    public bool GetMaxRoomsReached()
    {
        return maxRoomsReached;
    }
}
