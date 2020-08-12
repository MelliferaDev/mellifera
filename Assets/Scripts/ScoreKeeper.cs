using System;
using JetBrains.Annotations;
using Menus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{

    public Text scoreText;


    public int score;

    public static bool upgradeScreen = false;

    private static int previousScore;

    private void Awake()
    {
        Debug.Log("Keeper, Awake!");
        if (FindObjectsOfType<ScoreKeeper>().Length != 1)
        {
            Debug.Log("Being Destroyed");
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
        
    }

    void Start()
    {
        score = 0;

        if (!EndGame.isGameOver)
        {
            previousScore = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreText == null)
        {
            GameObject st = GameObject.FindGameObjectWithTag("ScoreText");
            if (st != null)
            {
                scoreText = st.GetComponent<Text>();
            }
        }

        if (scoreText != null)
        {
            if (upgradeScreen)
            {
                SetTotalScoreText();
            }
            else
            {

                SetLevelScoreText();
            }
        }
    }

    public int GetLevelScore()
    {
        return score;
    }


    public void IncreaseScore(int amount)
    {
        score += amount;
    }

    public void DecreaseScore(int amount)
    {
        score -= amount;
    }

    public void SaveLevelScore()
    {
        previousScore += score;
        LevelManager lm = FindObjectOfType<LevelManager>();

        UpgradeMenu.totalPoints += 1;

        if (score > lm.GetPollenAvailable())
        {
            UpgradeMenu.totalPoints += 3;
        }
        score = 0;

        upgradeScreen = !upgradeScreen;
    }

    void SetLevelScoreText()
    {
        scoreText.text = "Level Score: " + score;
    }

    void SetTotalScoreText()
    {
        scoreText.text = "Total Score: " + previousScore;
    }

    void SetUpgradeScreen(bool uScreen)
    {
        upgradeScreen = uScreen;
    }

    public void ResetLevelScore()
    {
        score = 0;
    }

    public static int GetTotalScore()
    {
        return previousScore;
    }
    
}
