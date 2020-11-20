using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorClearStair : MonoBehaviour
{
    private bool stairActive = false;

    private void Awake()
    {
        StartCoroutine("ActiveDelay");
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player") && stairActive)
        {
            stairActive = false;
            GameManager.instance.DeclareScore();
            SceneManager.LoadScene("GameClear");
        }
    }

    private IEnumerator ActiveDelay()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        stairActive = true;
    }
}
