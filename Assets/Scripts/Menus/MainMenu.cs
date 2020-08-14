using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Stats
    public TextMeshProUGUI statsText;

    private void Start()
    {
        string outText = "High score: " + PlayerPrefs.GetInt("highScore", 0).ToString();
        outText += "\nAverage sting score: " + (PlayerPrefs.GetFloat("runningDDRAverage", 0) * 100f).ToString() + "%";
        outText += "\nTotal pollen collected: " + PlayerPrefs.GetInt("pollenCollected", 0).ToString();

        statsText.text = outText;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
