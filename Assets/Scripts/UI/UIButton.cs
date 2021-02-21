using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButton : MonoBehaviour
{
    [SerializeField] private string target;

    public void OnClick()
    {
        PlayerPrefs.SetInt("Loading", 0);
        SceneManager.LoadScene(target);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

