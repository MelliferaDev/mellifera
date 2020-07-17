using System;
using Player;
using UnityEngine;

namespace Pickups
{
    public class CollectableBehavior : MonoBehaviour
    {
        [SerializeField] private int pollenAmount = 25;
        private bool collisionHeard;

        private void Start()
        {
            collisionHeard = false;
        }

        public void CollisionListener(PlayerControl player)
        {
            if (collisionHeard)
            {
                return;
            }
            
            if (player.currState == PlayerFlightState.Landed)
            {
                Debug.Log("b");
                // TODO right now we just give all the pollen as soon as they land on it,
                // but we might want to give it over a short amount of time (2-3 seconds?)
                FindObjectOfType<LevelManager>().CollectPollen(pollenAmount);
                // No more pollen left on this flower
                pollenAmount = 0;
            }

            if (pollenAmount == 0)
            {
                collisionHeard = true;
            }
        }
    }
}
