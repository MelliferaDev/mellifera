
using System;
using UnityEngine;

namespace Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private PlayerControl player;
        [SerializeField] private Vector3 flyingOffset = new Vector3(0.0f, 1.5f, -3.5f);
        [SerializeField] private Vector3 landedOffset = new Vector3(0.0f, 1.5f, -3.5f);
        [SerializeField] private Vector3 forwardEulerOffest = new Vector3(30f, 0.0f, 0.0f);
        [SerializeField] private float speed = 3.0f;
        private float currSpeed;
        
        private Vector3 desiredPos;
        private Transform lookAtTransform;
        private Vector3 cameraLag; // this is really just for debugging

        private bool synced = false;
        
        // this probably won't work one day
        public void SyncStart()
        {
            Start();
            synced = true;
        }
        
        public void Start()
        {
            lookAtTransform = player.transform;
            currSpeed = speed;
            synced = false;
        }
        
        void Update()
        {
            if (!synced || LevelManager.gamePaused)
            {
                Follow();
            } 
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

            // is this too hard to control? probably idk... no i think its too hard.... no its fine.... idk anymore
            float targetSpeed = (PlayerPowerupBehavior.GetActiveCurrentPowerup() == PlayerPowerup.Vortex)
                ? (speed + PlayerPowerupBehavior.vortexSpeedBoost) : speed;
            currSpeed = Mathf.Lerp(currSpeed, targetSpeed, Time.deltaTime);
            
            desiredPos = lookAtTransform.position;
            desiredPos += lookAtTransform.right * _offset.x;
            desiredPos += lookAtTransform.up * _offset.y;
            desiredPos += lookAtTransform.forward * _offset.z;
            transform.position = Vector3.Lerp(transform.position, desiredPos, currSpeed * Time.deltaTime);

            Vector3 desiredLook = lookAtTransform.forward + forwardEulerOffest;
            transform.forward = Vector3.Lerp(transform.forward, desiredLook, currSpeed * Time.deltaTime);
            
            if (player.currState == PlayerFlightState.Flying)
            {
                cameraLag = desiredPos - transform.position;
            }
        }

        public void OnDrawGizmos()
        {
            Debug.DrawRay(transform.position, cameraLag, Color.magenta);
        }
    }
}
