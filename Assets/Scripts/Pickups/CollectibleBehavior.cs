using Player;
using UnityEngine;

namespace Pickups
{
    public enum CollectibleType
    {
        Pollen, Health
    }

    public class CollectibleBehavior : MonoBehaviour
    {
        [SerializeField] public CollectibleType collectibleType;

        [SerializeField] private int collectAmount = 25;
        [SerializeField] private AudioClip collectSfx;

        private LevelManager lm;
        
        private void Start()
        {
            lm = FindObjectOfType<LevelManager>();
        }

        /// <summary>
        /// A listener method designed to recieve messages from the PlayerCollection
        /// OnControllerColliderHit
        /// </summary>
        /// <param name="player">The PlayerControl from the OnControllerColliderHit</param>
        public void ControllerCollisionListener(PlayerControl player)
        {
            if (player.currState == PlayerFlightState.Landed && collectAmount > 0)
            {
                bool didCollect = false;
                // TODO right now we just give it all as soon as they land on it,
                // but we might want to give it over a short amount of time

                switch (collectibleType)
                {
                    case CollectibleType.Health:
                        if (!lm.HealthIsFull())
                        {
                            lm.IncrementHealth(collectAmount);
                            didCollect = true;
                        }
                        break;
                    case CollectibleType.Pollen:
                        if (!lm.PollenIsFull())
                        {
                            lm.CollectPollen(collectAmount);
                            didCollect = true;
                        }
                        break;
                }

                if (didCollect)
                {
                    // Nothing left to collect
                    collectAmount = 0;
                    if (collectSfx != null) AudioSource.PlayClipAtPoint(collectSfx, player.transform.position);
                }
            }
        }
    }
}
