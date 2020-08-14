using System;
using UnityEngine;
using UnityEngine.Animations;

namespace UI
{
    public class RearviewCameraBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject cameraView;
        private static int cameraRequests;

        private Animator anim;
        private int viewState;
        
        
        void Start()
        {
            cameraRequests = 0;
            anim = cameraView.GetComponent<Animator>();
            anim.SetBool("rvActive", true);
            viewState = 0;
        }

        // Update is called once per frame
        void Update()
        {
            // switch (viewState)
            // {
            //     case 0: UpdateActive();
            //         break;
            //     case 1: UpdateInactive();
            //         break;
            // }
        }

        public static void RequestRearviewOn()
        {
            cameraRequests++;
            cameraRequests = Math.Max(cameraRequests, 1);
        }

        public static void RequestRearviewOff()
        {
            cameraRequests--;
            cameraRequests = Math.Min(cameraRequests, 0);
        }


        public void UpdateActive()
        {
            if (cameraRequests <= 0)
            {
                anim.SetBool("rvActive", false);
                viewState = 1;
                //Invoke(nameof(SetViewActive), anim.GetCurrentAnimatorStateInfo(0).length);
            }
        }
        
        public void UpdateInactive()
        {
            if (cameraRequests > 0)
            {
                //SetViewActive();
                anim.SetBool("rvActive", true);
                viewState = 0;
            }
        }
    }
}
