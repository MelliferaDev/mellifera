using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiveManager : MonoBehaviour
{

    public int maxHealth = 100;
    public Slider healthSlider;
    public GameObject hiveDefeatedUI;

    int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.value = currentHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.minValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncrementHealth(int amount)
    {
        currentHealth += amount;
        healthSlider.value = (currentHealth / (1.0f * maxHealth)) * 100;

        if (currentHealth <= 0)
        {
            LevelManager.gamePaused = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            hiveDefeatedUI.SetActive(true);
        }
    }

    public void SetHiveUIInactive()
    {
        hiveDefeatedUI.SetActive(false);
    }
}
