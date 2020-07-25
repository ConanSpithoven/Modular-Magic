using UnityEngine;
using UnityEngine.UI;

public class FormulaSetter : MonoBehaviour
{
    public int formulaNumber= 1;

    public void SetFormula()
    {
        PatternManager.instance.SetActiveFormula(formulaNumber);
    }
}
