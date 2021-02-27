using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public delegate void OnScalingIncrease(int timeScaling);
    public OnScalingIncrease onScalingIncrease;

    private int score = 0;
    private int timer = 0;
    private int timerS = 0;
    private int timerM = 0;
    private int timerH = 0;
    private int lives = 3;
    [SerializeField] private float xMax;
    [SerializeField] private float zMax;
    [SerializeField] private float xMin;
    [SerializeField] private float zMin;
    [SerializeField] private int scalingTimer;
    [SerializeField] private Transform LifeList;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerSText;
    [SerializeField] private TextMeshProUGUI timerMText;
    [SerializeField] private TextMeshProUGUI timerHText;
    [SerializeField] private Transform startPos;
    [SerializeField] private SpellDetails spellDetails;
    [SerializeField] private Barhandler playerHealthBar;
    [SerializeField] private GameObject cooldown1;
    [SerializeField] private GameObject cooldown2;
    [SerializeField] private GameObject cooldown3;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private EquipmentUI equipmentUI;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private PatternUI patternUI;
    [SerializeField] private ItemList itemList;
    private GameObject player;
    private SpellInventory spellInventory;
    private List<GameObject> livesList = new List<GameObject>();
    private GameObject livesText;
    private int timeScaling = 0;
    private bool paused = false;
    public bool InSpawnRoom = true;

    #region Singleton

    public static GameManager instance;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("HighScore", 0) != 0)
        {
            PlayerPrefs.SetInt("HighScore", 0);
        }
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of GameManager found!");
            return;
        }
        Spawn();
        StartCoroutine("Timer");
        for (int i = 1; i <= lives; i++)
        {
            GameObject life = Instantiate(Resources.Load<GameObject>("UI/Life"), LifeList.transform);
            livesList.Add(life);
        }
        instance = this;
        if (PlayerPrefs.GetInt("Loading") == 1)
        {
            LoadGame();
            PlayerPrefs.SetInt("Loading", 0);
        }
    }

    private void Start()
    {
        SaveGame();
    }

    private void Update()
    {
        if (player != null)
        {
            Camera.main.transform.position = new Vector3(Mathf.Clamp(player.transform.position.x, xMin, xMax), Camera.main.transform.position.y, Mathf.Clamp(player.transform.position.z-4, zMin, zMax));
        }
        scoreText.text = score + " points";
        UpdateTimer();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!CheckUIStatus())
            {
                TogglePause();
            }
        }
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
        //score--;
        if (timer % scalingTimer == 0 && timer > 0)
        {
            timeScaling++;
            //onScalingIncrease.Invoke(timeScaling);
        }
        StartCoroutine("Timer");
    }

    private void UpdateTimer()
    {
        timerH = TimeSpan.FromSeconds(timer).Hours;
        timerM = TimeSpan.FromSeconds(timer).Minutes;
        timerS = TimeSpan.FromSeconds(timer).Seconds;

        if (timerS > 9)
        {
            timerSText.text = "" + timerS;
        }
        else if (timerM > 0 || timerH > 0)
        {
            timerSText.text = "0" + timerS;
        }
        else
        {
            timerSText.text = "" + timerS;
        }
        if (timerM > 9)
        {
            timerMText.text = "" + timerM + ":";
        }
        else if (timerM > 0)
        {
            timerMText.text = "0" + timerM + ":";
        }
        else
        {
            timerMText.text = "";
        }
        if (timerH > 9)
        {
            timerHText.text = "" + timerH + ":";
        }
        else if (timerH > 0)
        {
            timerHText.text = "0" + timerH + ":";
        }
        else
        {
            timerHText.text = "";
        }
    }

    public int GetScore()
    {
        return score;
    }

    public int GetTime()
    {
        return timer;
    }

    public void ChangeScore(int value, bool adding)
    {
        if (adding)
        {
            //addition popup
            score += value;
        }
        else 
        {
            //deduction popup
            score -= value;
        }
    }

    public void ChangeLives(int value, bool adding)
    {
        if (adding)
        {
            lives += value;
            if (lives <= 3)
            {
                GameObject life = Instantiate(Resources.Load<GameObject>("UI/Life"), LifeList);
                livesList.Add(life);
            }
            else if (lives == 4)
            {
                livesText = Instantiate(Resources.Load<GameObject>("UI/ExtraLives"), LifeList);
                livesList.Add(livesText);
            }
            else
            {
                livesText.GetComponent<TextMeshProUGUI>().text = "+" + (lives - 3);
            }
        }
        else
        {
            lives -= value;
            if (lives >= 4)
            {
                livesText.GetComponent<TextMeshProUGUI>().text = "+" + (lives - 3);
            }
            else if (lives == 3)
            {
                livesList.Remove(livesText);
                Destroy(livesText);
            }
            if (lives > 0)
            {
                Destroy(livesList[lives - 1]);
                livesList.RemoveAt(lives - 1);
                if (player.GetComponent<PlayerStats>().GetCurrentHP() <= 0)
                {
                    Respawn();
                }
            }
            else 
            {
                GameOver();
            }
        }
    }

    private void Spawn()
    {
        GameObject playerPrefab = Resources.Load<GameObject>("PlayerTest");
        player = Instantiate(playerPrefab, startPos.position, startPos.rotation, null);
        spellInventory = player.GetComponent<SpellInventory>();
        playerHealthBar.SetFull();

    }

    public void Respawn()
    {
        StartCoroutine("RespawnRoutine");
    }

    private IEnumerator RespawnRoutine()
    {
        Destroy(player.gameObject);
        //display you died window
        yield return new WaitForSecondsRealtime(3);
        Spawn();
    }

    private void GameOver()
    {
        PlayerPrefs.SetInt("Score", score);
        SceneManager.LoadScene("GameOver");
    }

    public Barhandler GetBarhandler()
    {
        return playerHealthBar;
    }

    public GameObject GetCooldownObj(int slot)
    {
        switch (slot)
        {
            default:
            case 1:
                return cooldown1;
            case 2:
                return cooldown2;
            case 3:
                return cooldown3;
        }
    }

    public void SetCameraBounds(float xmin, float xmax, float zmin, float zmax)
    {
        xMin = xmin;
        xMax = xmax;
        zMin = zmin;
        zMax = zmax;
    }

    public void TogglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            paused = false;
            StartCoroutine("Timer");
            pauseMenu.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            paused = true;
            StopCoroutine("Timer");
            pauseMenu.SetActive(true);
        }
    }

    public bool GetPauseStatus()
    {
        return paused;
    }

    private bool CheckUIStatus()
    {
        int activeUIs = 0;
        if (equipmentUI.GetUIActive())
        {
            equipmentUI.CloseUI();
            activeUIs++;
        }
        if (inventoryUI.GetUIActive())
        {
            inventoryUI.CloseUI();
            activeUIs++;
        }
        if (patternUI.GetUIActive())
        {
            patternUI.CloseUI();
            activeUIs++;
        }
        if (activeUIs > 0)
        {
            return true;
        }
        return false;
    }

    public void DeclareScore()
    {
        PlayerPrefs.SetInt("Score", score);
        if (PlayerPrefs.GetInt("Score") > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }

    public void TogglePatternUI()
    {
        patternUI.TogglePatternUI();
    }

    public bool GetPatterUIActive()
    {
        return patternUI.GetUIActive();
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            InSpawnRoom = false;
        }
    }

    #region Save Load
    public void SaveGame()
    {
        Debug.Log("Saving");
        SaveSystem.SavePlayer(player.GetComponent<PlayerStats>());
        SaveSystem.SaveSeed();
        Inventory inventory = GetComponent<Inventory>();
        SaveSystem.SaveInventory(inventory.SaveInventoryContent(), inventory.SaveInventoryTypes());
        EquipmentManager equipmentManager = GetComponent<EquipmentManager>();
        string[] equips = equipmentManager.SaveEquipment();
        SaveSystem.SaveEquipment(equips);
        string[] formula1 = PatternManager.instance.SavePattern(1);
        string[] formula2 = PatternManager.instance.SavePattern(2);
        string[] formula3 = PatternManager.instance.SavePattern(3);
        SaveSystem.SavePatterns(formula1, formula2, formula3);
        SaveSystem.SaveGameData(score, timer, lives);
    }

    public void LoadGame()
    {
        player.GetComponent<PlayerStats>().LoadPlayer();
        Inventory inventory = GetComponent<Inventory>();
        inventory.LoadInventory();
        EquipmentManager equipmentManager = GetComponent<EquipmentManager>();
        equipmentManager.LoadEquipment();
        PatternManager.instance.LoadPattern();
        LoadGameData();
    }

    private void LoadGameData()
    {
        GameData data = SaveSystem.LoadGameData();
        score = data.score;
        timer = data.totalTime;
        if (data.lives > lives)
        {
            while(lives < data.lives)
            {
                ChangeLives(1, true);
            }
        }
        else if (data.lives < lives)
        {
            while (lives > data.lives)
            {
                ChangeLives(1, false);
            }
        }
    }
    #endregion

    #region ItemList
    public int GetItemCount(string type)
    {
        return itemList.GetItemCount(type);
    }

    public string GetItem(int index, string type)
    {
        return itemList.GetItem(index, type);
    }
    #endregion
}
