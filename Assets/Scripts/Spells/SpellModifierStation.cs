using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellModifierStation : MonoBehaviour
{
    private bool InRange = false;
    [SerializeField] private GameObject KeyHint;

    private void Update()
    {
        if (GameManager.instance.InSpawnRoom && InRange)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameManager.instance.TogglePatternUI();
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            KeyHint.SetActive(true);
            InRange = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            KeyHint.SetActive(false);
            InRange = false;
            if (GameManager.instance.GetPatterUIActive())
            {
                GameManager.instance.TogglePatternUI();
            }
        }
    }
}
