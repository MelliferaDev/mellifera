using Player;
using UnityEngine;

namespace Menus
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject menu;
        public bool pauseMenuUp = false;
        
        private InputManager im;
        private LevelManager lm;
        private GameState lastGameState;
        private void Start()
        {
            im = FindObjectOfType<InputManager>();
            lm = FindObjectOfType<LevelManager>();
            lastGameState = lm.currentGameState;
        }

        private void Update()
        {
            if (im.GetPauseBtnClicked())
            {
                pauseMenuUp = !pauseMenuUp;
                if (pauseMenuUp)
                    PauseMenuOn();
                else
                    PauseMenuOff();
            }
        }

        private void PauseMenuOn()
        {
            lastGameState = lm.currentGameState;
            lm.currentGameState = GameState.PAUSED;
            LevelManager.gamePaused = true;
            Time.timeScale = 0f;
                    
            
            menu.SetActive(true);
                    
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void PauseMenuOff()
        {
            lm.currentGameState = lastGameState;
            LevelManager.gamePaused = false;
            Time.timeScale = 1f;
                    
            menu.SetActive(false);
                    
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}