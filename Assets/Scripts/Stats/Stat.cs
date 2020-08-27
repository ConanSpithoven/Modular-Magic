using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    [SerializeField] private float currentValue;

    private List<float> modifiers = new List<float>();

    public float GetValue()
    {
        Modified();
        return currentValue;
    }

    public void AddModifier(float modifier)
    {
        if (modifier != 0)
            modifiers.Add(modifier);
        Modified();
    }

    public void RemoveModifier(float modifier)
    {
        if (modifier != 0)
            modifiers.Remove(modifier);
        Modified();
    }

    private void Modified()
    {
        currentValue = baseValue;
        modifiers.ForEach(x => currentValue += x);
    }
}
