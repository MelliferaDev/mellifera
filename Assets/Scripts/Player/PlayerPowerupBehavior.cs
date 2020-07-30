using Pickups;
using UnityEngine;

namespace Player
{
    
    // Subject to change for future implementation
    // "powerupDuration" should be set on a per-powerup basis, and it means different things for different powerups:
    // Vortex: Duration of the speed boost
    // Free sting: time you have to hit the target before you lose it
    // Waggle dance: Not sure, may not be applicable
    public class PlayerPowerupBehavior : MonoBehaviour
    {
        [SerializeField] private int vortexDuration = 3; // seconds
        [SerializeField] private PowerupGUI gui;
        
        public static int speedBoost = 100; // This will go in the Vortex specific class once it exists.

        public int vortexCollected;
        public int stingCollected;
        public int waggleCollected;

        private float powerupTimeout;
        private PlayerPowerup curPowerup;

        // Start is called before the first frame update
        void Start()
        {
            curPowerup = PlayerPowerup.None;
        }

        // Update is called once per frame
        void Update()
        {
            if (curPowerup != PlayerPowerup.None)
            {
                powerupTimeout -= Time.deltaTime;
            
                if (powerupTimeout < 0)
                {
                    curPowerup = PlayerPowerup.None;
                }
            }
        }

        public bool CanPickUp()
        {
            return curPowerup == PlayerPowerup.None;
        }

        public void GivePowerup(PlayerPowerup powerupType)
        {
            switch (powerupType)
            {
                case PlayerPowerup.Vortex: vortexCollected++;
                    gui.UpdateGUI(powerupType, vortexCollected);
                    break;
                case PlayerPowerup.FreeSting: stingCollected++;
                    gui.UpdateGUI(powerupType, stingCollected);
                    break;
                case PlayerPowerup.WaggleDance: waggleCollected++;
                    gui.UpdateGUI(powerupType, stingCollected);
                    break;
            }
        }

        public void Activate(PlayerPowerup powerupType)
        {
            switch (powerupType)
            {
                case PlayerPowerup.Vortex: ActivateVortex();
                    break;
                case PlayerPowerup.FreeSting: ActivateSting();
                    break;
                case PlayerPowerup.WaggleDance: ActivateWaggleDance();
                    break;
            }
        }

        private void ActivateVortex()
        {
            if (vortexCollected <= 0) return;
            
            curPowerup = PlayerPowerup.Vortex;
            powerupTimeout = vortexDuration;
            vortexCollected--;
            gui.UpdateGUI(PlayerPowerup.Vortex, vortexCollected);

        }
        
        private void ActivateSting()
        {
            if (stingCollected <= 0) return;
            
            curPowerup = PlayerPowerup.FreeSting;
            powerupTimeout = vortexDuration;
            stingCollected--;
            gui.UpdateGUI(PlayerPowerup.FreeSting, stingCollected);

        }
        
        private void ActivateWaggleDance()
        {
            if (waggleCollected <= 0) return;
            
            curPowerup = PlayerPowerup.Vortex;
            powerupTimeout = vortexDuration;
            waggleCollected--;
            gui.UpdateGUI(PlayerPowerup.WaggleDance, waggleCollected);
        }

        public PlayerPowerup GetActiveCurrentPowerup()
        {
            return curPowerup;
        }
    }

    public enum PlayerPowerup
    {
        None = 0, Vortex = 1, FreeSting = 2, WaggleDance = 3
    }
}