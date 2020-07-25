using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PatternSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public PatternType patternType;
    public Image icon;
    Pattern pattern;
    PatternManager patternManager;
    private GameObject statcard;
    private bool overSlot = false;

    public void Start()
    {
        patternManager = PatternManager.instance;
    }

    public void Update()
    {
        if (overSlot && pattern != null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (statcard == null)
                {
                    statcard = Instantiate(Resources.Load<GameObject>("Popups/ItemDetails"), transform);
                    statcard.GetComponent<ItemDetailsPopup>().SetupPattern(pattern);
                }
                else
                {
                    Destroy(statcard);
                    statcard = null;
                }
            }
        }
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        overSlot = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        overSlot = false;
        if (statcard != null)
        {
            Destroy(statcard);
            statcard = null;
        }
    }
}
