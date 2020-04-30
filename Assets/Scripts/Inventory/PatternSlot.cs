using UnityEngine;
using UnityEngine.UI;

public class PatternSlot : MonoBehaviour
{
    public PatternType patternType;
    public Image icon;
    Pattern pattern;
    PatternManager patternManager;

    public void Start()
    {
        patternManager = PatternManager.instance;
    }

    public void AddItem(Pattern newPattern)
    {
        pattern = newPattern;

        icon.sprite = pattern.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        pattern = null;

        icon.sprite = null;
        icon.enabled = false;
    }

    public void OnButton()
    {
        if (pattern != null)
        {
            patternManager.UnEquip(pattern);
        }
    }

    public Pattern GetPattern()
    {
        return pattern;
    }
}
