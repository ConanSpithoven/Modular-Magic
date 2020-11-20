using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorClearStair : MonoBehaviour
{
    private bool stairActive = false;
    private bool OnStair = false;
    [SerializeField] private GameObject KeyHint;

    private void Awake()
    {
        StartCoroutine("ActiveDelay");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && OnStair)
        {
            OnStair = false;
            stairActive = false;
            GameManager.instance.DeclareScore();
            SceneManager.LoadScene("GameClear");
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player") && stairActive)
        {
            KeyHint.SetActive(true);
            OnStair = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player") && stairActive)
        {
            KeyHint.SetActive(false);
            OnStair = false;
        }
    }

    private IEnumerator ActiveDelay()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        stairActive = true;
    }
}
