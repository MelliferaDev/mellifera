using Player;
using UnityEngine;

namespace Pickups
{
    public class CollectibleBehavior : MonoBehaviour
    {
        [SerializeField] private int pollenAmount = 25;

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
            Debug.Log("hm");
            if (player.currState == PlayerFlightState.Landed)
            {
                // TODO right now we just give all the pollen as soon as they land on it,
                // but we might want to give it over a short amount of time (2-3 seconds?)
                lm.CollectPollen(pollenAmount);
                
                // No more pollen left on this flower
                pollenAmount = 0;
            }
        }
    }
}
