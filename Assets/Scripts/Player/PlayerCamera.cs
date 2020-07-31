
using UnityEngine;

namespace Player
{
    // Follows
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private PlayerControl player;
        [SerializeField] private Vector3 flyingOffset = new Vector3(0.0f, 1.5f, -3.5f);
        [SerializeField] private Vector3 landedOffset = new Vector3(0.0f, 1.5f, -3.5f);

        [SerializeField] private float speed = 3.0f;
        
        private Vector3 desiredPos;
        private Transform lookAtTransform;

        private Vector3 cameraLag;

        public void Start()
        {
            lookAtTransform = player.transform;
        }
        
        
        
        void Update()
        {
            //Follow();
            Debug.DrawRay(transform.position, cameraLag, Color.magenta);
            
        }

        public void Follow()
        {
            Vector3 _offset = flyingOffset;

            
            switch (player.currState)
            {
                case PlayerFlightState.Flying:
                    _offset = flyingOffset;
                    break;
                case PlayerFlightState.Landed:
                    _offset = landedOffset;
                    break;
            }

            desiredPos = lookAtTransform.position;
            desiredPos += lookAtTransform.right * _offset.x;
            desiredPos += lookAtTransform.up * _offset.y;
            desiredPos += lookAtTransform.forward * _offset.z;
            
            transform.position = Vector3.Lerp(transform.position, desiredPos, speed * Time.deltaTime);
            transform.forward = Vector3.Lerp(transform.forward, lookAtTransform.forward, speed * Time.deltaTime);
            
            if (player.currState == PlayerFlightState.Flying)
            {
                cameraLag = desiredPos - transform.position;
            }
        }
    }
}
