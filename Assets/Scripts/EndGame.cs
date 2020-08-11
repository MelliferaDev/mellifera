using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    void Start()
    {
        if(isGameOver)
        {
        ScoreKeeper sk = FindObjectOfType<ScoreKeeper>();
        currentScore = sk.GetTotalScore();

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
            if (currentScoreText == null)
            {
                currentScoreText = GameObject.FindGameObjectWithTag("CurrentScoreText").GetComponent<Text>();
            }
            if (highScoreText == null)
            {
                highScoreText = GameObject.FindGameObjectWithTag("HighScoreText").GetComponent<Text>();
            }

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

    public void SetWinScreen()
    {
        didWin = true;
    }
}
