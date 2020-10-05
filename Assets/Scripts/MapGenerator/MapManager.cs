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
    [SerializeField] private GameObject[] normalRooms;
    [SerializeField] private GameObject lootRoom;
    [SerializeField] private GameObject boss;
    [SerializeField] private int lootRoomLimit = 1;

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
            int bossRoom = rooms.Count - 1;
            bossSpawned = true;
            Instantiate(boss, rooms[bossRoom].transform.position, Quaternion.identity, rooms[bossRoom].transform);
            for (int i = 1; i <= rooms.Count - 2; i++)
            {
                int rand = Random.Range(0, 20);
                if (rand >= 0 && rand <= 19)
                {
                    normalRoomsNumbers.Add(rooms[i]);
                }
                else if (lootRoomsSpawned < lootRoomLimit)
                {
                    lootRoomsSpawned++;
                    Instantiate(lootRoom, rooms[i].transform.position, Quaternion.identity, rooms[i].transform);
                }
            }
            SpawnLootRooms();
            CalcMapBounds();
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

    private void SpawnLootRooms()
    {
        if (lootRoomsSpawned < lootRoomLimit)
        {
            int rand = Random.Range(0, normalRoomsNumbers.Count);
            int number = rooms.IndexOf(normalRoomsNumbers[rand]);
            Instantiate(lootRoom, rooms[number].transform.position, Quaternion.identity, rooms[number].transform);
            lootRoomsSpawned++;
            normalRoomsNumbers.Remove(normalRoomsNumbers[rand]);
            SpawnLootRooms();
        }
        else 
        {
            SpawnNormalRooms();
        }
    }

    private void SpawnNormalRooms()
    {
        foreach (GameObject room in normalRoomsNumbers)
        {
            int number = rooms.IndexOf(room);
            Instantiate(normalRooms[Random.Range(0, (normalRooms.Length - 1))], rooms[number].transform.position, Quaternion.identity, rooms[number].transform);
        }
    }
}
