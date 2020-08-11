using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiveManager : MonoBehaviour
{

    public int maxHealth = 100;
    public int waggleRestoreAmount = 50;
    public Slider healthSlider;
    public GameObject hiveDefeatedUI;
    public GameObject hiveProtector;

    bool hiveInvulnerable;

    int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.value = currentHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.minValue = 0;
        hiveInvulnerable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncrementHealth(int amount)
    {
        if (amount > 0 || !hiveInvulnerable)
        {
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
            healthSlider.value = currentHealth;

            if (currentHealth <= 0)
            {
                LevelManager.gamePaused = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                hiveDefeatedUI.SetActive(true);
            }
        }
    }

    public void SetHiveUIInactive()
    {
        hiveDefeatedUI.SetActive(false);
    }

    public void ActivateHiveDefence()
    {
        IncrementHealth(waggleRestoreAmount);
        hiveInvulnerable = true;
        hiveProtector.SetActive(true);
        Invoke("DeactivateHiveDefence", 5);
    }

    public void DeactivateHiveDefence()
    {
        hiveProtector.SetActive(false);
        hiveInvulnerable = false;
    }
}
