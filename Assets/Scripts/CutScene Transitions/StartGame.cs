using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public GameObject nextText;
    void Start()
    {
        Invoke("showNextText", 1);
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void showNextText()
    {
        nextText.GetComponent<TMP_Text>().text = "Click to Start!";
        nextText.SetActive(true);
    }
}
