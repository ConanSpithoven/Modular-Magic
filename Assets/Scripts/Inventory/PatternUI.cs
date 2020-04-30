using System.Collections.Generic;
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
        Pattern item;
        if (newItem != null)
        {
            item = newItem;
        }
        else
        {
            item = oldItem;
        }
        switch (formulaNumber)
        {
            case 1:
                switch (item.patternType)
                {
                    case PatternType.Empowerment:
                        List<Pattern> patterns = patternManager.GetEmpowermentPatterns(formulaNumber);
                        int addedEmpowermentPatterns = 0;
                        for (int i = 0; i < slots1.Length; i++)
                        {
                            if (slots1[i].patternType == PatternType.Empowerment)
                            {
                                if (addedEmpowermentPatterns < patterns.Count)
                                {
                                    slots1[i].AddItem(patterns[addedEmpowermentPatterns]);
                                    addedEmpowermentPatterns++;
                                }
                                else
                                {
                                    slots1[i].ClearSlot();
                                }
                            }
                        }
                        break;
                    case PatternType.Elemental:
                        for (int i = 0; i < slots1.Length; i++)
                        {
                            if (slots1[i].patternType == PatternType.Elemental)
                            {
                                if (newItem != null)
                                {
                                    slots1[i].AddItem(newItem);
                                }
                                else 
                                {
                                    slots1[i].ClearSlot();
                                }
                            }
                        }
                        break;
                }
                break;
            case 2:
                switch (item.patternType)
                {
                    case PatternType.Empowerment:
                        List<Pattern> patterns = patternManager.GetEmpowermentPatterns(formulaNumber);
                        int addedEmpowermentPatterns = 0;
                        for (int i = 0; i < slots2.Length; i++)
                        {
                            if (slots2[i].patternType == PatternType.Empowerment)
                            {
                                if (addedEmpowermentPatterns < patterns.Count)
                                {
                                    slots2[i].AddItem(patterns[addedEmpowermentPatterns]);
                                    addedEmpowermentPatterns++;
                                }
                                else
                                {
                                    slots2[i].ClearSlot();
                                }
                            }
                        }
                        break;
                    case PatternType.Elemental:
                        for (int i = 0; i < slots2.Length; i++)
                        {
                            if (slots2[i].patternType == PatternType.Elemental)
                            {
                                if (newItem != null)
                                {
                                    slots2[i].AddItem(newItem);
                                }
                                else
                                {
                                    slots2[i].ClearSlot();
                                }
                            }
                        }
                        break;
                }
                break;
            case 3:
                switch (item.patternType)
                {
                    case PatternType.Empowerment:
                        List<Pattern> patterns = patternManager.GetEmpowermentPatterns(formulaNumber);
                        int addedEmpowermentPatterns = 0;
                        for (int i = 0; i < slots3.Length; i++)
                        {
                            if (slots3[i].patternType == PatternType.Empowerment)
                            {
                                if (addedEmpowermentPatterns < patterns.Count)
                                {
                                    slots3[i].AddItem(patterns[addedEmpowermentPatterns]);
                                    addedEmpowermentPatterns++;
                                }
                                else
                                {
                                    slots3[i].ClearSlot();
                                }
                            }
                        }
                        break;
                    case PatternType.Elemental:
                        for (int i = 0; i < slots3.Length; i++)
                        {
                            if (slots3[i].patternType == PatternType.Elemental)
                            {
                                if (newItem != null)
                                {
                                    slots3[i].AddItem(newItem);
                                }
                                else
                                {
                                    slots3[i].ClearSlot();
                                }
                            }
                        }
                        break;
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
