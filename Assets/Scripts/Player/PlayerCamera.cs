
using UnityEngine;

namespace Player
{
    // Follows
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private PlayerControl lookAt;
        [SerializeField] private Vector3 flyingOffset = new Vector3(0.0f, 1.5f, -3.5f);
        [SerializeField] private Vector3 landedOffset = new Vector3(0.0f, 1.5f, -3.5f);

        [SerializeField] private float speed = 3.0f;
        
        private Vector3 desiredPos;
        private Transform lookAtTransform;
        private PlayerControl playerCtlr;

        private Vector3 cameraLag;
        
        private void Start()
        {
            lookAtTransform = lookAt.transform;
            playerCtlr = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        }
        
        void Update()
        {
            Follow();
            Debug.DrawRay(transform.position, cameraLag, Color.magenta);
            
        }

        void Follow()
        {
            Vector3 _offset = flyingOffset;
            switch (playerCtlr.currState)
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
            
            if (playerCtlr.currState == PlayerFlightState.Flying)
            {
                cameraLag = desiredPos - transform.position;
            }
        }
    }
}
