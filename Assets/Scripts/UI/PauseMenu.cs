using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void ReturnButton()
    {
        GameManager.instance.TogglePause();
    }

    public void QuitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
