using UnityEngine;
using UnityEngine.UI;

public class FormulaSetter : MonoBehaviour
{
    public int formulaNumber= 1;
    private Button button;

    public void Awake()
    {
        button = GetComponent<Button>();
    }

    public void SetFormula()
    {
        PatternManager.instance.SetActiveFormula(formulaNumber);
    }
}
