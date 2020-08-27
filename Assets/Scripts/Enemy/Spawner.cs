using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPos;
    [SerializeField] private int spawnNumber;
    [SerializeField] private int spawnLimit;
    private GameManager gameManager;
    [SerializeField] private int scalingCounter;
    [SerializeField] private float spawnCooldown;
    [SerializeField] private int RequiredScaling;
    [SerializeField] private GameObject spawnedUnit;

    private void Start()
    {
        gameManager = GameManager.instance;
        gameManager.onScalingIncrease += IncreaseScaling;
        StartCoroutine("Spawn");
    }

    public void ReduceCount()
    {
        spawnNumber--;
        spawnNumber = Mathf.RoundToInt(Mathf.Clamp(spawnNumber, 0 , Mathf.Infinity));
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSecondsRealtime(spawnCooldown);
        if (spawnNumber < spawnLimit && scalingCounter >= RequiredScaling)
        {
            GameObject newEnemy = Instantiate(spawnedUnit, spawnPos.position, spawnPos.rotation);
            newEnemy.GetComponent<EnemyManager>().SetSpawner(this);
            newEnemy.GetComponent<EnemyManager>().SetScaling(scalingCounter);
            spawnNumber++;
        }
        StartCoroutine("Spawn");
    }

    private void IncreaseScaling(int newScale)
    {
        scalingCounter = newScale;
    }
}
