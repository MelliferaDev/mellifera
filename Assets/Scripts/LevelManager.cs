using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    // public variables set for each level
    // pollenAvailable can probably be rewritten to count instances of a Pollen object, but that doesn't exist yet
    [SerializeField] int pollenAvailable = 0;
    [SerializeField] int pollenTarget = 0;

    public int pollenCollected = 0;
    public bool canFinishLevel = false;




    // References to other objects
    [SerializeField] Slider pollenSlider;
    [SerializeField] Slider healthSlider;



    // Start is called before the first frame update
    void Start()
    {
        SetupPollenSlider();
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
            canFinishLevel = true;
            FindObjectOfType<PollenTargetSlider>().SetShouldLerpColor(true);
        }
        else
        {
            canFinishLevel = false;
            FindObjectOfType<PollenTargetSlider>().SetShouldLerpColor(false);
        }
        pollenSlider.value = pollenCollected;
    }

    void SetupPollenSlider()
    {
        pollenSlider.maxValue = pollenAvailable;
        pollenSlider.minValue = 0;
        pollenSlider.value = 0;
    }
}
