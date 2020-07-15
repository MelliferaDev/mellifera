using System;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

namespace Player
{
    [RequireComponent(typeof(PlayerControl))]
    public class PlayerWingBehaviour : MonoBehaviour
    {
        [SerializeField] private Animator wing1Flapping;
        [SerializeField] private Animator wing2Flapping;
        [SerializeField] private float wingSpeed = 10f;
        [SerializeField] private float wingAcc = 2.5f;

        private PlayerControl player;
        private static readonly int FlyingState = Animator.StringToHash("flyingState");

        private float currSpeed;
        private void Start()
        {
            player = GetComponent<PlayerControl>();
            currSpeed = 0f;
        }

        private void Update()
        {
            if (player.currState == PlayerFlightState.Flying)
            {
                currSpeed = Mathf.Lerp(currSpeed, wingSpeed, wingAcc * Time.deltaTime);

                wing1Flapping.SetInteger(FlyingState, 0);
                wing2Flapping.SetInteger(FlyingState, 0);
            } else if (player.currState == PlayerFlightState.Landed)
            {
                currSpeed = Mathf.Lerp(currSpeed, 0, wingAcc * Time.deltaTime);

                if (Math.Abs(currSpeed) < Mathf.Epsilon)
                {
                    wing1Flapping.SetInteger(FlyingState, 1);
                    wing2Flapping.SetInteger(FlyingState, 1);
                }
            }
            
            Debug.Log(currSpeed);

            wing1Flapping.speed = currSpeed;
            wing2Flapping.speed = currSpeed;
        }
    }
}