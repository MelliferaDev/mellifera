using UnityEngine;

namespace Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Debug.Log("OnCollisionEnter: " + hit.gameObject.name);
        }
    }
}