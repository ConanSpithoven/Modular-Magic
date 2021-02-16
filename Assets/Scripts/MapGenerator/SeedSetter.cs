using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class SeedSetter : MonoBehaviour
{
    public TextMeshProUGUI textSeed;

    public void SubmitSeed()
    {
        int seed;
        string SeedText = textSeed.text;
        Debug.Log("text seed: " + SeedText + ".");
        string SeedNumber = Regex.Replace(SeedText, "[^0-9]", "");
        if (!int.TryParse(SeedNumber, out seed))
        {
            Debug.Log("parsing failed");
            seed = Random.Range(0, 1000000000);
        }
        Random.InitState(seed);
        PlayerPrefs.SetInt("Seed", seed);
    }
}
