using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryDisplayCountDown : MonoBehaviour
{

    public GameObject displayText;

    float countDown;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (countDown > 0)
        {
            countDown -= Time.deltaTime;
        } else
        {
            countDown = 0;
            displayText.SetActive(false);
        }
    }

    public void DisplayFor(float time)
    {
        countDown = time;
        displayText.SetActive(true);
    }
}
