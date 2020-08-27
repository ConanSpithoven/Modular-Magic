using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButton : MonoBehaviour
{
    [SerializeField] private string target;

    public void OnClick()
    {
        SceneManager.LoadScene(target);
    }
}

