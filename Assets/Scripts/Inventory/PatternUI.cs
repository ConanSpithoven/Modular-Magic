using System.Collections.Generic;
using UnityEngine;

public class PatternUI : MonoBehaviour
{
    public Transform patternsParent;
    public GameObject patternUI;
    public GameObject formula1UI;
    public GameObject formula2UI;
    public GameObject formula3UI;

    private PatternManager patternManager;

    private List<PatternSlot> slots1;
    private List<PatternSlot> slots2;
    private List<PatternSlot> slots3;

    private List<PatternSlot> empowermentSlots1;
    private List<PatternSlot> empowermentSlots2;
    private List<PatternSlot> empowermentSlots3;

    [SerializeField] private Transform formulaSlotParent1;
    [SerializeField] private Transform formulaSlotParent2;
    [SerializeField] private Transform formulaSlotParent3;

    [SerializeField] private GameObject patternSlot;

    void Start()
    {
        patternManager = PatternManager.instance;
        patternManager.onPatternChanged += UpdatePatternUI;
        patternManager.onFormulaChanged += UpdateFormulaUI;
        patternManager.onUpgradeLimitChanged += UpdateUpgradeLimit;

        slots1 = new List<PatternSlot>(formula1UI.GetComponentsInChildren<PatternSlot>());
        slots2 = new List<PatternSlot>(formula2UI.GetComponentsInChildren<PatternSlot>());
        slots3 = new List<PatternSlot>(formula3UI.GetComponentsInChildren<PatternSlot>());

        empowermentSlots1 = new List<PatternSlot>(formulaSlotParent1.GetComponentsInChildren<PatternSlot>());
        empowermentSlots2 = new List<PatternSlot>(formulaSlotParent2.GetComponentsInChildren<PatternSlot>());
        empowermentSlots3 = new List<PatternSlot>(formulaSlotParent3.GetComponentsInChildren<PatternSlot>());
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
                        for (int i = 0; i < slots1.Count; i++)
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
                        for (int i = 0; i < slots1.Count; i++)
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
                    case PatternType.Variant:
                        for (int i = 0; i < slots1.Count; i++)
                        {
                            if (slots1[i].patternType == PatternType.Variant)
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
                        for (int i = 0; i < slots2.Count; i++)
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
                        for (int i = 0; i < slots2.Count; i++)
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
                    case PatternType.Variant:
                        for (int i = 0; i < slots2.Count; i++)
                        {
                            if (slots2[i].patternType == PatternType.Variant)
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
                        for (int i = 0; i < slots3.Count; i++)
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
                        for (int i = 0; i < slots3.Count; i++)
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
                    case PatternType.Variant:
                        for (int i = 0; i < slots3.Count; i++)
                        {
                            if (slots3[i].patternType == PatternType.Variant)
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

    void UpdateUpgradeLimit(int newLimit, int formulaNumber)
    {
        switch (formulaNumber)
        {
            case 1:
                if (empowermentSlots1.Count < newLimit)
                {
                    //add slots
                    for (int i = (empowermentSlots1.Count + 1); i <= (newLimit); i++)
                    {
                        GameObject newSlot = Instantiate(patternSlot, formulaSlotParent1);
                        empowermentSlots1.Add(newSlot.GetComponent<PatternSlot>());
                        slots1.Add(newSlot.GetComponent<PatternSlot>());
                    }
                }
                else if (empowermentSlots1.Count > newLimit)
                {
                    //remove slots
                    for (int i = (empowermentSlots1.Count-1); i >= newLimit; i--)
                    {
                        GameObject oldSlot = empowermentSlots1[i].gameObject;
                        slots1.Remove(empowermentSlots1[i]);
                        empowermentSlots1.RemoveAt(i);
                        Destroy(oldSlot);
                    }
                }
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
}
