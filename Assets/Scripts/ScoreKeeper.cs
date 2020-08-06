using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{

    public Text scoreText;


    public int score;

    public static bool upgradeScreen = false;

    int previousScore;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        previousScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreText == null)
        {
            scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
        }

        SetLevelScoreText();
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
        score = 0;
    }

    void SetLevelScoreText()
    {
        scoreText.text = "Level Score: " + score;
    }

    void SetTotalScoreText()
    {
        scoreText.text = "Total Score: " + score;
    }

    void SetUpgradeScreen(bool uScreen)
    {
        upgradeScreen = uScreen;
    }
}
