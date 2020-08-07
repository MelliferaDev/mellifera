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
        public void ControllerCollisionListener(object[] args)
        {
            PlayerControl player = args[0] as PlayerControl;
            int multiplier = 1;
            if (args.Length > 1)
            {
                multiplier = (int) args[1];
            }

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
                            lm.CollectPollen(collectAmount * multiplier);
                            didCollect = true;
                        }
                        break;
                }

                if (didCollect)
                {
                    // Nothing left to collect
                    collectAmount = 0;
                    if (collectSfx != null) AudioSource.PlayClipAtPoint(collectSfx, player.transform.position);
                    for (int i = 0; i < gameObject.GetComponent<Renderer>().materials.Length; i++)
                    {
                        gameObject.GetComponent<Renderer>().materials[i].DisableKeyword("_EMISSION");
                    }
                    
                }
            }
        }
    }
}
