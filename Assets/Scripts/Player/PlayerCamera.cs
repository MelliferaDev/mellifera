
using UnityEngine;

namespace Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private PlayerControl lookAt;
        [SerializeField] private float offset = 1.5f;
        [SerializeField] private float distance = 3.5f;
        [SerializeField] private float speed = 3.0f;
        
        private Vector3 desiredPos;
        private Transform lookAtTransform;
        
        private void Start()
        {
            lookAtTransform = lookAt.transform;
        }

        void LateUpdate()
        {
            Follow();
        }

        void Follow()
        {
            desiredPos = lookAtTransform.position + (-transform.forward * distance) + (transform.up * offset);
            if (Vector3.Distance(transform.position, lookAtTransform.position) > distance)
            {
                transform.position = Vector3.Lerp(transform.position, desiredPos, speed * Time.deltaTime);
            }

            if (lookAt.currState == PlayerFlightState.Flying)
            {
                transform.LookAt(lookAtTransform.position + Vector3.up * offset);
            }
            else
            {
                transform.forward = Vector3.Lerp(transform.forward, lookAtTransform.forward, speed * Time.deltaTime);
            }
            
        }
    }
}
