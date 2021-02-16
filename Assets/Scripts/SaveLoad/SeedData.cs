using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SeedData
{
    public int seed;

    public SeedData()
    {
        seed = PlayerPrefs.GetInt("Seed");
    }
}
