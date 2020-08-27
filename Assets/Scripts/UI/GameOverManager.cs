using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highscore;
    [SerializeField] private TextMeshProUGUI score;

    private void Awake()
    {
        score.text = "score: " + PlayerPrefs.GetInt("Score");
        if (PlayerPrefs.GetInt("Score") > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", PlayerPrefs.GetInt("Score"));
            highscore.text = "New Highscore: " + PlayerPrefs.GetInt("score");
        }
        else
        {
            highscore.text = "Highscore: " + PlayerPrefs.GetInt("HighScore");
        }
    }
}
