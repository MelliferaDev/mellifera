using System;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    // Birds attack you at random no matter where you are as they circle the skies
    public class BirdBehavior : EnemyBehaviour
    {

        public BirdFlyingState currState = BirdFlyingState.Patrolling;

        [Header("Patrol Settings")] 
        [SerializeField] private Transform patrolCenter;
        [SerializeField] private float currAngle;
        [SerializeField] private float circleSpeed;
        [SerializeField] private float circleRadius;
        [SerializeField] private float patrolHover;
        [Header("Attack Settings")]
        public float attackDuration = 5f;
        public float attackDelayMin = 5f;
        public float attackDelayMax = 15f;
        public float attackSpeed = 5f;
        public float attackDist = 5f;
        [Header("Attack Reactions")]
        public float enemyHealth = 100f;
        

        private Rigidbody rb;
        Animator anim;
        private static readonly int BirdMovement = Animator.StringToHash("birdMovement");

        private float attackTimer;
        public float attackDelay;
        private Vector3 lastPatrolPos;

        protected override void Start()
        {
            base.Start();
            anim = GetComponent<Animator>();
            anim.SetInteger(BirdMovement, 1);
            rb = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
            attackTimer = Time.time;
            lastPatrolPos = transform.position;
            attackDelay = Random.Range(attackDelayMin, attackDelayMax);

        }

        protected override void Update()
        {
            base.Update();

            switch (currState)
            {
                case BirdFlyingState.Patrolling: UpdatePatrolState(); break;
                case BirdFlyingState.Diving: UpdateDivingState(); break;
                case BirdFlyingState.Returning: UpdateReturnState(); break;

            }

            if(enemyHealth <=0)
            {
                Destroy(gameObject, .5f);
            }
        }

        //Defeated
        public override void EnemyDefeated()
        {
            //will probably call update disengage here
        }


        ///////////////////////////////////////////////
        //// Basic Movement States ////////////////////

        public override void UpdatePatrolState()
        {
            if (Time.time - attackTimer > attackDelay)
            {
                currState = BirdFlyingState.Diving;
                attackTimer = Time.time;
            }

            if (distToPlayer < attackDist)
            {
                currState = BirdFlyingState.Diving;
                attackTimer = Time.time;
            }
            
            currAngle += circleSpeed * Time.deltaTime;
            currAngle %= 360;
        
            float targetX = circleRadius * Mathf.Cos(Mathf.Deg2Rad * currAngle);
            float targetY = patrolHover * Mathf.Sin(Time.time * 0.1f);
            float targetZ = circleRadius * Mathf.Sin(Mathf.Deg2Rad * currAngle);
            Vector3 localPos = new Vector3(targetX, targetY, targetZ);
            nextPoint = patrolCenter.TransformPoint(localPos);
            lastPatrolPos = nextPoint;
            Vector3 moveDir = (nextPoint - transform.position).normalized;
            
            transform.position = nextPoint;
            FaceTarget(transform.position + moveDir);
        }
        
        /////////////////////////////////////////////////////
        //// Other Movement States //////////////////////////

        private void UpdateDivingState()
        {
            if (Time.time - attackTimer > attackDuration) // will replace with something that gaurentees the bird attacked once
            {
                currState = BirdFlyingState.Returning;
                attackTimer = Time.time;
                attackDelay = Random.Range(attackDelayMin, attackDelayMax);
            }
            
            float step = Time.deltaTime * (attackSpeed);
            nextPoint = player.transform.position;
            
            transform.position = Vector3.MoveTowards(transform.position, nextPoint, step);
            FaceTarget(nextPoint, false);
        }

        private void UpdateReturnState()
        {
            if (Vector3.Distance(transform.position, lastPatrolPos) < Mathf.Epsilon)
            {
                currState = BirdFlyingState.Patrolling;
            }
            
            if (Time.time - attackTimer > attackDelay)
            {
                currState = BirdFlyingState.Diving;
                attackTimer = Time.time;
            }
            
            float step = Time.deltaTime * (attackSpeed);
            nextPoint = lastPatrolPos;
            transform.position = Vector3.MoveTowards(transform.position, nextPoint, step);
            FaceTarget(nextPoint, false);
        }


        public override void ApplySelfDamage(float damage)
        {
            enemyHealth -= damage;
        }
        


        public enum BirdFlyingState
        {
            Patrolling, Diving, Returning, Attacking
        }
    }
}
