using System;
using UnityEngine;

namespace NPCs
{
    [RequireComponent(typeof(NPCBehaviour))]
    public class NPCWingBehaviour : MonoBehaviour
    {
        [SerializeField] private Animator wing1Flapping;
        [SerializeField] private Animator wing2Flapping;
        [SerializeField] private float wingSpeed = 10f;
        [SerializeField] private float wingAcc = 2.5f;

        private NPCBehaviour bee;

        private static readonly int FlyingState = Animator.StringToHash("flyingState");

        private float currSpeed;

        private void Start()
        {
            bee = GetComponent<NPCBehaviour>();
            currSpeed = 0f;
        }

        private void Update()
        {
            if (bee.currState == NPCState.Flying)
            {
                currSpeed = Mathf.Lerp(currSpeed, wingSpeed, wingAcc * Time.deltaTime);

                wing1Flapping.SetInteger(FlyingState, 0);
                wing2Flapping.SetInteger(FlyingState, 0);
            }
            else if (bee.currState == NPCState.Pollinating)
            {
                currSpeed = Mathf.Lerp(currSpeed, 3, wingAcc * Time.deltaTime);

                if (Math.Abs(currSpeed) < Mathf.Epsilon)
                {
                    wing1Flapping.SetInteger(FlyingState, 1);
                    wing2Flapping.SetInteger(FlyingState, 1);
                }
            }

            wing1Flapping.speed = currSpeed;
            wing2Flapping.speed = currSpeed;
        }
    }
}