using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarSlider : MonoBehaviour
{

    // I think we want the pollen required to come from the LevelManager and the health to come from the player

    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        // this is temporary stuff just to test that the bars are working properly
        if (Input.GetKeyDown(KeyCode.W))
        {
            SetValue(slider.value - 1);
        } else if (Input.GetKeyDown(KeyCode.S))
        {
            SetValue(slider.value + 1);
        }
    }

    public void SetMaxValue(float newMax)
    {
        slider.maxValue = newMax;
    } 

    public void SetValue(float value)
    {
        slider.value = Mathf.Clamp(value, 0, slider.maxValue);
    }
}
