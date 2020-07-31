using Enemies;
using Menus;
using Player;
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
            currentGameState = GameState.READY_TO_ADVANCE;
            if (usingUI)
            {
                pollenTargetSlider.SetShouldLerpColor(true);
                nextLevelGraphics.SetActive(true);
            }
        }
        else
        {
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
    
    public void CollectPollen(int pollenAmount)
    {
        pollenCollected += pollenAmount;
        // every 2 extra pollen collected gives an extra point
        if (pollenCollected > pollenTarget && pollenAmount > 0)
        {
            UpgradeMenu.totalPoints += 1;
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

    public void IncrementHealth(int amount)
    {
        currentHealth += amount;
        healthSlider.value = (currentHealth / (1.0f * startingHealth+ (int)PlayerUpgrades.maxHealthAdd)) * 100 ;

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
    }

    public void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(nextLevel))
        {
            Time.timeScale = 1;
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
        ddrTarget = target;
        gamePaused = true;
        uiCanvas.SetActive(false);
        ddrCanvas.SetActive(true);
        FindObjectOfType<DDRManager>().startDDR();
    }

    public void EndDDR(int score, int maxScore)
    {
        gamePaused = false;
        ddrCanvas.SetActive(false);
        uiCanvas.SetActive(true);
        FindObjectOfType<StingBehavior>().FinishSting(score, maxScore, ddrTarget);

    }
}

public enum GameState
{
    PLAYING, READY_TO_ADVANCE, DEAD, PAUSED
}
