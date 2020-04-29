using UnityEngine;

public class PatternUI : MonoBehaviour
{
    public Transform patternsParent;
    public GameObject patternUI;
    public GameObject formula1UI;
    public GameObject formula2UI;
    public GameObject formula3UI;
    PatternManager patternManager;

    PatternSlot[] slots1;
    PatternSlot[] slots2;
    PatternSlot[] slots3;

    void Start()
    {
        patternManager = PatternManager.instance;
        patternManager.onPatternChanged += UpdatePatternUI;
        patternManager.onFormulaChanged += UpdateFormulaUI;

        slots1 = formula1UI.GetComponentsInChildren<PatternSlot>();
        slots2 = formula2UI.GetComponentsInChildren<PatternSlot>();
        slots3 = formula3UI.GetComponentsInChildren<PatternSlot>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Patterns"))
        {
            patternUI.SetActive(!patternUI.activeSelf);
        }
    }

    void UpdatePatternUI(Pattern newItem, Pattern oldItem, int formulaNumber)
    {
        switch (formulaNumber)
        {
            case 1:
                for (int i = 0; i < slots1.Length; i++)
                {
                    if (i < patternManager.currentPattern1.Count)
                    {
                        slots1[i].AddItem(patternManager.currentPattern1[i]);
                    }
                    else
                    {
                        slots1[i].ClearSlot();
                    }
                }
                break;
            case 2:
                for (int i = 0; i < slots2.Length; i++)
                {
                    if (i < patternManager.currentPattern2.Count)
                    {
                        slots2[i].AddItem(patternManager.currentPattern2[i]);
                    }
                    else
                    {
                        slots2[i].ClearSlot();
                    }
                }
                break;
            case 3:
                for (int i = 0; i < slots3.Length; i++)
                {
                    if (i < patternManager.currentPattern3.Count)
                    {
                        slots3[i].AddItem(patternManager.currentPattern3[i]);
                    }
                    else
                    {
                        slots3[i].ClearSlot();
                    }
                }
                break;
        }
    }

    void UpdateFormulaUI(int formulaNumber)
    {
        switch (formulaNumber)
        {
            case 1:
                formula1UI.SetActive(true);
                formula2UI.SetActive(false);
                formula3UI.SetActive(false);
                break;
            case 2:
                formula1UI.SetActive(false);
                formula2UI.SetActive(true);
                formula3UI.SetActive(false);
                break;
            case 3:
                formula1UI.SetActive(false);
                formula2UI.SetActive(false);
                formula3UI.SetActive(true);
                break;
        }
    }
}
