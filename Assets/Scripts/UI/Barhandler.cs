using UnityEngine;
using UnityEngine.UI;

public class Barhandler : MonoBehaviour
{
    private float fillAmount = 1;
    [SerializeField] private float lerpSpeed;
    [SerializeField] private Image mask;

    void Update()
    {
        HandleBar();
    }

    public void SetValue(float value, float max)
    {
        fillAmount = Map(value, 0, max, 0, 1);
    }

    private void HandleBar()
    {
        if (fillAmount != mask.fillAmount)
        {
            mask.fillAmount = Mathf.Lerp(mask.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
        }
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    public void SetFull()
    {
        mask.fillAmount = 1;
        fillAmount = 1;
    }
}
