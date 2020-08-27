using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreText;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("HighScore", 0) != 0)
        {
            highScoreText.text = "HighScore: " + PlayerPrefs.GetInt("HighScore");
        }
        else
        {
            highScoreText.text = "No HighScore set yet";
        }
    }
}
