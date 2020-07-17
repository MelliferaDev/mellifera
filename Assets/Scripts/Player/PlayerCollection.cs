using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerControl))]
    public class CollectCollectables : MonoBehaviour
    {
        private PlayerControl player;

        private void Start()
        {
            player = GetComponent<PlayerControl>();
        }

        private void OnControllerColliderhit(ControllerColliderHit hit)
        {
            if (hit.gameObject.CompareTag("Collectible"))
            {
                CollectableBehavior cb = hit.gameObject.GetComponent<CollectableBehavior>();
                if (cb == null) return;
                cb.SendMessage("CollisionListener", player);
            }
            
        }
    }
}