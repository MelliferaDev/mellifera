using UnityEngine;

namespace Pickups
{
    public class PowerupBehavior : MonoBehaviour
    {
        public PlayerPowerup powerupType;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerPowerupBehavior powerupReceiver = other.GetComponent<PlayerPowerupBehavior>();

                if (powerupReceiver.CanPickUp())
                {
                    powerupReceiver.GivePowerup(powerupType);
                    Destroy(gameObject);
                }
            }
        }
    }
}
