
using UnityEngine;

namespace Player
{
    // Follows
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private PlayerControl lookAt;
        [SerializeField] private Vector3 offset = new Vector3(0.0f, 1.5f, -3.5f);
        [SerializeField] private float speed = 3.0f;
        
        private Vector3 desiredPos;
        private Transform lookAtTransform;
        
        private void Start()
        {
            lookAtTransform = lookAt.transform;
        }
        
        void Update()
        {
            Follow();
        }

        void Follow()
        {
            desiredPos = lookAtTransform.position;
            desiredPos += lookAtTransform.right * offset.x;
            desiredPos += lookAtTransform.up * offset.y;
            desiredPos += lookAtTransform.forward * offset.z;
            
            transform.position = Vector3.Lerp(transform.position, desiredPos, speed * Time.deltaTime);
            transform.forward = Vector3.Lerp(transform.forward, lookAtTransform.forward, speed * Time.deltaTime);
            
        }
    }
}
