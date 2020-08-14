using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{

    public GameObject returnText;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("showNextText", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && returnText.active)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void showNextText()
    {
        returnText.SetActive(true);
    }
}
