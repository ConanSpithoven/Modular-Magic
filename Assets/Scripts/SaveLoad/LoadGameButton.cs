using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameButton : MonoBehaviour
{
    public void LoadGame()
    {
        SaveSystem.LoadSeed();
        PlayerPrefs.SetInt("Loading", 1);
        SceneManager.LoadScene("GenTest");
    }
}
