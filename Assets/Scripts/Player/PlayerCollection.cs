using Pickups;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerControl))]
    public class PlayerCollection : MonoBehaviour
    {
        private PlayerControl player;

        private void Start()
        {
            player = GetComponent<PlayerControl>();
        }

        // Instead of OnCollisionEnter, this is the method that is called
        // when the CharacterController enters a collision. The event is triggered on the
        // CharacterController object only. However, the action to take place
        // is supposed ot be done by CollectibleBehavior. So we are sending a message
        // to the CollectibleBehavior to do the task
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.CompareTag("Collectible"))
            {
                CollectibleBehavior cb = hit.gameObject.GetComponent<CollectibleBehavior>();
                if (cb == null) return;
                cb.SendMessage("ControllerCollisionListener", player);
            }
            
        }
    }
}