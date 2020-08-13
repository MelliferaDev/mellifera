using Enemies;
using Menus;
using Player;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cursor = UnityEngine.Cursor;

public class LevelManager : MonoBehaviour
{
    public static bool gamePaused = false;
    
    [Header("Player Stats")]
    // pollenAvailable can probably be rewritten to count instances of a Pollen object, but that doesn't exist yet
    [SerializeField] int pollenAvailable = 0; 
    [SerializeField] int pollenTarget = 0;
    [SerializeField] public int startingHealth = 100;
    
    [Header("Level Progression")]
    [SerializeField] public GameState currentGameState = GameState.PLAYING;
    [SerializeField] int pollenCollected = 0;
    [SerializeField] int currentHealth;
    [SerializeField] string nextLevel;
    [SerializeField] private bool startPaused;
    [SerializeField] private AudioClip advanceReadySfx;

    [Header("UI Elements")]
    // References to other objects
    [SerializeField] Slider pollenSlider;
    [SerializeField] Slider healthSlider;
    [SerializeField] GameObject nextLevelUI; // the UI elements to show when the level is over
    [SerializeField] GameObject nextLevelGraphics; // the target graphics to fly to when the next level is unlocked
    [SerializeField] GameObject reloadLevelUI;


    [Header("DDR BOIS")]
    [SerializeField] GameObject ddrCanvas;
    [SerializeField] GameObject uiCanvas;

    [SerializeField] private bool usingUI = true;

    private PollenTargetSlider pollenTargetSlider;

    GameObject ddrTarget;


    // Start is called before the first frame update
    void Start()
    {
        pollenTargetSlider = FindObjectOfType<PollenTargetSlider>();
        gamePaused = startPaused;
        currentGameState = GameState.PLAYING;
        SetupPollenSlider();
        SetupHealthSlider();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPollenLevel();
    }

    public int GetPollenTarget()
    {
        return pollenTarget;
    }

    public int GetPollenAvailable()
    {
        return pollenAvailable;
    }

    void CheckPollenLevel()
    {
        if (pollenCollected >= pollenTarget)
        {
            if (currentGameState == GameState.PLAYING)
            {
                if (advanceReadySfx != null) AudioSource.PlayClipAtPoint(advanceReadySfx, Camera.main.transform.position);
                currentGameState = GameState.READY_TO_ADVANCE;
            }

            if (usingUI)
            {
                pollenTargetSlider.SetShouldLerpColor(true);
                nextLevelGraphics.SetActive(true);
            }
        }
        else
        {
            if (currentGameState == GameState.READY_TO_ADVANCE)
            {
                currentGameState = GameState.PLAYING;
            }

            if (usingUI)
            {
                pollenTargetSlider.SetShouldLerpColor(false);
                nextLevelGraphics.SetActive(false); 
            }
        }

        if (usingUI)
        {
            pollenSlider.value = pollenCollected;
        }
    }

    void SetupPollenSlider()
    {
        if (!usingUI) return;
        pollenSlider.maxValue = pollenAvailable;
        pollenSlider.minValue = 0;
        pollenSlider.value = 0;
    }
    
    public bool PollenIsFull()
    {
        return pollenCollected == pollenAvailable;
    }

    public void CollectPollen(int pollenAmount)
    {
        pollenCollected += pollenAmount;
        FindObjectOfType<ScoreKeeper>().IncreaseScore(pollenAmount);
        pollenCollected = Mathf.Clamp(pollenCollected, 0, pollenAvailable);
        PlayerPrefs.SetInt("pollenCollected", PlayerPrefs.GetInt("pollenCollected") + pollenCollected);

        // every 2 extra pollen collected gives an extra point
        if (pollenCollected > pollenTarget && pollenAmount > 0)
        {
            //UpgradeMenu.totalPoints += 1;
            Debug.Log("Total Points: " + UpgradeMenu.totalPoints);
        }
    }

    void SetupHealthSlider()
    {
        currentHealth = startingHealth + (int)PlayerUpgrades.maxHealthAdd;

        if (!usingUI) return;
        
        healthSlider.maxValue = currentHealth;
        healthSlider.minValue = 0;
        healthSlider.value = currentHealth;
    }

    public bool HealthIsFull()
    {
        return currentHealth == GetMaxHealth();
    }

    private int GetMaxHealth()
    {
        return startingHealth + (int) PlayerUpgrades.maxHealthAdd;
    }

    public void IncrementHealth(int amount)
    {
        currentHealth += amount;
        healthSlider.value = (currentHealth / (1.0f * startingHealth + (int)PlayerUpgrades.maxHealthAdd)) * 100;

        if (currentHealth <= 0)
        {
            gamePaused = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            reloadLevelUI.SetActive(true);
        }
    }

    public void ReloadLevel()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        reloadLevelUI.SetActive(false);
        gamePaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        FindObjectOfType<ScoreKeeper>().ResetLevelScore();
    }

    public void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(nextLevel))
        {
            if (nextLevel == "ExitCutScene")
            {
                EndGame.isGameOver = true;
                FindObjectOfType<EndGame>().SetWinScreen();
            }

            Time.timeScale = 1;
            ScoreKeeper keeper = FindObjectOfType<ScoreKeeper>();

            keeper.SaveLevelScore();
            SceneManager.LoadScene(nextLevel);
        }
    }

    public void ResetNextLevelUI()
    {
        Time.timeScale = 1;
        nextLevelUI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StartDDR(GameObject target)
    {
        Debug.Log("target: " + target);
        ddrTarget = target;
        gamePaused = true;
        uiCanvas.SetActive(false);
        ddrCanvas.SetActive(true);
        FindObjectOfType<DDRManager>().startDDR();
    }

    public void EndDDR(int score, int maxScore)
    {
        Invoke("EndDDRView", 1.5f);
        float prevAvg = PlayerPrefs.GetFloat("runningDDRAverage");
        int prevN = PlayerPrefs.GetInt("timesPlayedDDR");
        float newAvg = ((prevN * prevAvg) + ((1.0f * score) / (1.0f * maxScore))) / (1.0f * prevN + 1);

        PlayerPrefs.SetInt("timesPlayedDDR", prevN + 1);
        PlayerPrefs.SetFloat("runningDDRAverage", newAvg);

        FindObjectOfType<StingBehavior>().FinishSting(score, maxScore, ddrTarget);
    }

    private void EndDDRView()
    {
        gamePaused = false;
        ddrCanvas.SetActive(false);
        uiCanvas.SetActive(true);
    }
}

public enum GameState
{
    PLAYING, READY_TO_ADVANCE, DEAD, PAUSED
}
