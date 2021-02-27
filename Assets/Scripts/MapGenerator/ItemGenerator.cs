using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        GameObject spawnedItem = Instantiate(Resources.Load<GameObject>("Items/Item"), spawnPoint.position, Quaternion.Euler(new Vector3(90,0,0)));
        string type;
        int rand = Random.Range(1,10);
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
        Item item = spawnedItem.GetComponent<ItemPickup>().item = Resources.Load<Item>("Items/"+type+"/"+ GameManager.instance.GetItem(index, type));
        spawnedItem.GetComponent<SpriteRenderer>().sprite = item.worldIcon;
    }
}
