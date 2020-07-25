using UnityEngine;

public class UIActivator : MonoBehaviour
{
    [SerializeField] private GameObject activatedObject;

    public void ToggleObject() 
    {
        activatedObject.SetActive(!activatedObject.activeSelf);
    }
}
