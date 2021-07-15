using UnityEngine;

public class AddRoom : MonoBehaviour
{
    [SerializeField] private Vector3 roomSpawnOffset;
    [SerializeField] private RoomType roomType;
    [SerializeField] private RoomOpenings[] roomOpenings;

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

    public int GetOpeningCount()
    {
        return roomOpenings.Length;
    }

    public RoomOpenings[] GetRoomOpenings()
    {
        return roomOpenings;
    }
}
