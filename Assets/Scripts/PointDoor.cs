using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointDoor : MonoBehaviour
{
    [SerializeField] private int doorCost;
    [SerializeField] private TextMeshPro cost;
    private bool active = false;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
        cost.text = doorCost + " points";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && active)
        {
            CheckDoorOpenable();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cost.gameObject.SetActive(true);
            active = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        cost.gameObject.SetActive(false);
        active = false;
    }

    private void CheckDoorOpenable()
    {
        if (gameManager.GetScore() >= doorCost)
        {
            gameManager.ChangeScore(doorCost, false);
            Destroy(gameObject);
        }
    }
}
