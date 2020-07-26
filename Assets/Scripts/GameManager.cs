using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public int timer = 0;
    private int timerS = 0;
    private int timerM = 0;
    private int timerH = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerSText;
    [SerializeField] private TextMeshProUGUI timerMText;
    [SerializeField] private TextMeshProUGUI timerHText;
    [SerializeField] private Transform startPos;
    [SerializeField] private SpellDetails spellDetails;
    private GameObject player;
    private SpellInventory spellInventory;


    #region Singleton

    public static GameManager instance;

    private void Awake()
    {
        GameObject playerPrefab = Resources.Load<GameObject>("PlayerTest");
        player = Instantiate(playerPrefab, startPos.position, startPos.rotation, null);
        spellInventory = player.GetComponent<SpellInventory>();

        if (instance != null)
        {
            Debug.LogWarning("More than one instance of GameManager found!");
            return;
        }
        StartCoroutine("Timer");
        instance = this;
    }

    private void Update()
    {
        scoreText.text = score + " points";
        if (timerS >= 60)
        {
            timerS -= 60;
            timerM++;
        }
        if (timerM >= 60)
        {
            timerM -= 60;
            timerH++;
        }
        UpdateTimer();
    }

    #endregion

    public void ArcheTypeSwap(int spellSlot, SpellType spellType)
    {
        Spell newSpell = Resources.Load<Spell>("Spells/" + spellSlot + "/" + spellType.ToString());
        Spell oldSpell = spellInventory.GetSpell(spellSlot);
        StatTransfer(oldSpell, newSpell);
        spellInventory.SetSpellSlot(newSpell, spellSlot);
        spellDetails.UpdateSpell(spellSlot);
    }

    private void StatTransfer(Spell oldSpell, Spell newSpell)
    {
        newSpell.power = oldSpell.power;
        newSpell.lifetime = oldSpell.lifetime;
        newSpell.size = oldSpell.size;
        newSpell.instances = oldSpell.instances;
        newSpell.speed = oldSpell.speed;
        newSpell.unique = oldSpell.unique;
        newSpell.spellSlot = oldSpell.spellSlot;
        newSpell.upgradeLimit = oldSpell.upgradeLimit;
        newSpell.SetOriginShape(oldSpell.GetOriginShape());
        newSpell.SetOriginElement(oldSpell.GetOriginElement());
        newSpell.ModifyElement(oldSpell.element, true);
        newSpell.ModifyShape(oldSpell.shape, true);
        spellInventory.CalcCooldownTime(newSpell.spellSlot);
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSecondsRealtime(1);
        timer++;
        timerS++;
        score++;
        StartCoroutine("Timer");
    }

    private void UpdateTimer()
    {
        if (timerS > 9)
        {
            timerSText.text = "" + timerS;
        }
        else 
        {
            timerSText.text = "0" + timerS;
        }
        if (timerM > 9)
        {
            timerMText.text = "" + timerM;
        }
        else
        {
            timerMText.text = "0" + timerM;
        }
        if (timerH > 9)
        {
            timerHText.text = "" + timerH;
        }
        else
        {
            timerHText.text = "0" + timerH;
        }
    }
}
