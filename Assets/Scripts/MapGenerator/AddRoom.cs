using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    [SerializeField] private Vector3 roomSpawnOffset;
    [SerializeField] private RoomType roomType;

    void Start()
    {
        MapManager.instance.AddRoom(gameObject);
    }

    public Vector3 GetRoomOffset()
    {
        return roomSpawnOffset;
    }

    public RoomType GetRoomType()
    {
        return roomType;
    }
}
