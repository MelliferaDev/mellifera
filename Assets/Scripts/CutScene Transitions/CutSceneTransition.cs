using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneTransition : MonoBehaviour
{
    public GameObject nextText;
    public GameObject nextScene;
    void Start()
    {
        Invoke("showNextText", 1);
    }

    
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            nextScene.SetActive(true);
            nextText.SetActive(false);
            Invoke("hideCurrentSlide", .5f);
        }
    }

    private void showNextText()
    {
        nextText.SetActive(true);
    }

    private void hideCurrentSlide()
    {
        gameObject.SetActive(false);
    }
}
