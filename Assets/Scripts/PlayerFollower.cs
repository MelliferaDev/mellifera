using UnityEngine;

namespace Player
{
    /// <summary>
    /// Attach this to the Camera
    /// </summary>
    public class PlayerFollower : MonoBehaviour
    {
        private Transform playerTransform;
        private bool foundPlayerTranform;
        private Vector3 posOffset;
    
        void Start()
        {
            playerTransform = GameObject.FindWithTag("Player").transform;
            foundPlayerTranform = (playerTransform != null);
        
            posOffset = transform.position - playerTransform.position;
        }

        void Update()
        {
            if (foundPlayerTranform)
            {
                transform.position = playerTransform.position + posOffset;
            }
        }
    }
}
