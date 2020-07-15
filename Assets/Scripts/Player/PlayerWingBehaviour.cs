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
                wing1Flapping.SetInteger(FlyingState, 0);
                wing2Flapping.SetInteger(FlyingState, 0);
                wing1Flapping.speed = Mathf.Lerp(currSpeed, 10f, Time.deltaTime);
                wing2Flapping.speed = Mathf.Lerp(currSpeed, 10f, Time.deltaTime);

            } else if (player.currState == PlayerFlightState.Landed)
            {
                wing1Flapping.speed = Mathf.Lerp(currSpeed, 0f, Time.deltaTime);
                wing2Flapping.speed = Mathf.Lerp(currSpeed, 0f, Time.deltaTime);
                if (Math.Abs(currSpeed) < Mathf.Epsilon)
                {
                    wing1Flapping.SetInteger(FlyingState, 1);
                    wing2Flapping.SetInteger(FlyingState, 1);
                }
            }
        }
    }
}