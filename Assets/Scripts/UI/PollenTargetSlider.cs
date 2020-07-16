using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PollenTargetSlider : MonoBehaviour
{

    

    int pollenInScene;
    int pollenTarget;

    LevelManager lm;
    Image fillSlider;

    [SerializeField] Color basicColor;
    [SerializeField] Color lerpColor;
    [SerializeField] Slider targetSlider;

    bool shouldLerpColor = false;


    // Start is called before the first frame update
    void Start()
    {
        lm = FindObjectOfType<LevelManager>();
        pollenInScene = lm.GetPollenAvailable();
        pollenTarget = lm.GetPollenTarget();
        SetTargetPollenAmount();
        fillSlider = GetComponentInChildren<Image>();
        fillSlider.color = basicColor;

    }

    // Update is called once per frame
    void Update()
    {
        if (shouldLerpColor)
        {
            float t = Mathf.Sin(Time.time * 5);
            t += 1;
            t /= 2;

            fillSlider.color = Color.Lerp(basicColor, lerpColor, t);
        }
        else
        {
            fillSlider.color = basicColor;
        }
    }

    void SetTargetPollenAmount()
    {
        targetSlider.minValue = 0;
        targetSlider.maxValue = 1;
        targetSlider.wholeNumbers = false;
        targetSlider.value = (float)pollenTarget / (float)pollenInScene;
        // temp until the values are properly loaded in
        //targetSlider.value = 0.5f;
    }

    public void SetShouldLerpColor(bool newValue)
    {
        shouldLerpColor = newValue;
    }
}
