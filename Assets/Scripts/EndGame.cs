using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    public Text highScoreText;
    public Text currentScoreText;

    public GameObject winScreen;
    public GameObject loseScreen;

    public static bool isGameOver = false;

    int currentScore;
    int storedScore;
    bool didWin = false;

    private bool foundCurrScoreText = false;
    private bool foundHighScoreText = false;

    private ScoreKeeper sk;
    
    void Start()
    {
        foundCurrScoreText = currentScoreText != null;
        foundCurrScoreText = highScoreText != null;
        LoadTextObjects();
        
        if(isGameOver)
        {
            currentScore = ScoreKeeper.GetTotalScore();

            storedScore = PlayerPrefs.GetInt("highScore", 0);
        
            if (didWin)
            {
                winScreen.SetActive(true);
                loseScreen.SetActive(false);
            }
            else
            {
                winScreen.SetActive(false);
                loseScreen.SetActive(true);
            }
        }
    }

    void Update()
    {
        if(isGameOver)
        {
            currentScore = ScoreKeeper.GetTotalScore();
            SetHighScore();
            SetScoreText();
        }

    }

    void SetHighScore()
    {
        if(currentScore > storedScore)
        {
            PlayerPrefs.GetInt("highScore", currentScore);
        }
    }

    void SetScoreText()
    {
        string s = SceneManager.GetActiveScene().name;

        if (!foundCurrScoreText || !foundHighScoreText)
        {
            LoadTextObjects();
        }
        
        if (foundCurrScoreText && foundHighScoreText)
        {
            if (currentScore > storedScore)
            {
                currentScoreText.text = "Total Score: " + currentScore;
                highScoreText.text = "New High Score!";
            }
            else
            {
                currentScoreText.text = "Total Score: " + currentScore;
                highScoreText.text = "High Score: " + storedScore;
            }
        }
    }

    private void LoadTextObjects()
    {
        GameObject csText = GameObject.FindGameObjectWithTag("CurrentScoreText");
        if (!foundCurrScoreText && csText != null)
        {
            currentScoreText = csText.GetComponent<Text>();
            foundCurrScoreText = currentScoreText != null;
        }
        
        GameObject hsText = GameObject.FindGameObjectWithTag("HighScoreText");
        if (!foundHighScoreText & hsText != null)
        {
            highScoreText = hsText.GetComponent<Text>();
            foundHighScoreText = highScoreText != null;
        }
    }

    public void SetWinScreen()
    {
        didWin = true;
    }
}
