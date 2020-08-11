﻿using System;
using UnityEngine;

namespace Enemies
{
    public class BirdGroundPath : MonoBehaviour
    {
        [SerializeField] private float groundYLevel = 0f;
        [SerializeField] private Color patrolColor;
        [SerializeField] private Color attackColor;
        private TrailRenderer trail;
        private BirdBehavior bird;

        private void Start()
        {
            trail = GetComponent<TrailRenderer>();
            bird = GetComponentInParent<BirdBehavior>();
        }

        private void Update()
        {
            bool activateTrail = (bird.currState == BirdBehavior.BirdFlyingState.Patrolling);
            trail.emitting = activateTrail;
            trail.startColor = (activateTrail ? patrolColor : attackColor);

            Vector3 pos = bird.transform.TransformPoint(Vector3.zero);
            pos.y = groundYLevel;
            transform.position = pos;
        }
    }
}