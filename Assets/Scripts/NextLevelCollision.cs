
using System;
using UnityEngine;

public class NextLevelCollision : MonoBehaviour
{

    public GameObject nextLevelUI;

    private LevelManager lm;

    private void Start()
    {
        lm = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") 
            && lm.currentGameState == GameState.READY_TO_ADVANCE)
        {
            lm.currentGameState = GameState.PAUSED;
            Time.timeScale = 0;
            nextLevelUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
