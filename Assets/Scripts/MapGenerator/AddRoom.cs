using UnityEngine;

public class AddRoom : MonoBehaviour
{
    [SerializeField] private Vector3 roomSpawnOffset;
    [SerializeField] private RoomType roomType;
    [SerializeField] private int openingCount = 1;

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
        return openingCount;
    }
}
