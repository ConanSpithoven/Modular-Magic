using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField] private List<Item> spawnPool;
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        if (spawnPool.Count > 0)
        {
            GameObject spawnedItem = Instantiate(Resources.Load<GameObject>("Items/Item"), spawnPoint.position, Quaternion.Euler(new Vector3(90,0,0)));
            //change to grab item from resources by name
            Item item = spawnedItem.GetComponent<ItemPickup>().item = spawnPool[Mathf.RoundToInt(Random.Range(0, spawnPool.Count - 1))];
            spawnedItem.GetComponent<SpriteRenderer>().sprite = item.worldIcon;
        }
    }
}
