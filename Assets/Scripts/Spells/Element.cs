using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New element")]
public class Element : ScriptableObject
{
    public string ElementName;
    public string[] ElementStrengths;
    public string[] ElementWeaknesses;
}
