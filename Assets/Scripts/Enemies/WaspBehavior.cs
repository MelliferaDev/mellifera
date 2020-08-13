using System;
using System.Runtime.CompilerServices;
using System.Timers;
using UI;
using UnityEngine;

namespace Enemies
{
    public class WaspBehavior : EnemyBehaviour
    {
     /*
     * When the player gets within minDistance of the wasp, the wasp will attack the player.
     * The wasp will hover up and down when it's not attacking.
     */
        public WaspFlyingState currState = WaspFlyingState.Patrolling;
        [Header("Attack Settings")]
        public float minDistance = 5f;
        public float attackSpeed = 5f;
        [Header("Attack Reactions")]
        public float recoilImpactSpeed = 0.75f;
        public float recoilRecoverySpeed = 5f;
        public float recoilMaxTime;
        public float enemyHealth = 10;
        
        // public float playerAttack = 2;
        [Header("Hovering")]
        public float patrolSpeed = 5f;
        
        private CharacterController ctrl;
        Animator anim;
        private static readonly int AnimState = Animator.StringToHash("animState");

        private float patrolStuckTimer;
        private bool attackHive;

        private Vector3 currentRecoil;
        private float recoilTimer;

        private float edgeOfCharacter;
        
        protected override void Start()
        {
            base.Start();

            ctrl = GetComponent<CharacterController>();
            anim = GetComponent<Animator>();

            patrolStuckTimer = Time.time;
            recoilTimer = Time.time;
            currentRecoil = Vector3.zero;

            edgeOfCharacter = 2 * Mathf.Abs(ctrl.center.magnitude + ctrl.radius);
        }

        protected override void Update()
        {
            base.Update();
            if (!LevelManager.gamePaused)
            {
                switch (currState)
                {
                    case WaspFlyingState.Patrolling: UpdatePatrolState(); break;
                    case WaspFlyingState.Attacking: UpdateAttackState(); break;
                    case WaspFlyingState.Recoiling: UpdateRecoilState(); break;
                    case WaspFlyingState.Dying: UpdateDieState(); break;
                }
            }

            if (enemyHealth <=0)
            {
                currState = WaspFlyingState.Dying;
            }
            
            // Debug.DrawLine(transform.position, nextPoint, Color.blue);
            Vector3 target = transform.position + transform.forward.normalized * (edgeOfCharacter);
            Debug.DrawLine(transform.position, target, Color.red);
        }

        ///////////////////////////////////////////////
        //// Basic Movement States ////////////////////
      
        public override void UpdatePatrolState()
        {
            anim.SetInteger(AnimState, 0);
            
            if (Utils.Distance2D(transform.position, nextPoint) <= edgeOfCharacter ||
                (Time.time - patrolStuckTimer) > 25f)
            {
                patrolStuckTimer = Time.time;
                FindNextPoint();
            }

            if (distToPlayer <= minDistance)
            {
                RearviewCameraBehaviour.RequestRearviewOn();
                currState = WaspFlyingState.Attacking;
            }

            if (distToHive <= minDistance && (Time.time - hiveAttackTimer) >= hiveAttackCooldown)
            {
                hiveAttackTimer = Time.time;
                currState = WaspFlyingState.Attacking;
            }
            
            Vector3 toTarget = nextPoint - transform.position;
            MoveInDir(toTarget, patrolSpeed);
            FaceTarget(nextPoint, false);
        }

        private void MoveInDir(Vector3 dir, float speed)
        {
            Vector3 move = dir.normalized * (speed * Time.deltaTime);
            ctrl.Move(move);
        }
        
        ///////////////////////////////////////////////
        //// Attacking State //////////////////////////

        private void UpdateAttackState()
        {
            if (distToPlayer > minDistance && distToHive > minDistance)
            {
                RearviewCameraBehaviour.RequestRearviewOff(); // attacking is done
                currState = WaspFlyingState.Patrolling;
                patrolStuckTimer = Time.time;
            }
            
            anim.SetInteger(AnimState, 1);
            
            Vector3 toTarget = (player.transform.position - transform.position);
            Transform lookAt = null;
            if (distToPlayer <= minDistance)
            {
                toTarget = player.transform.position - transform.position;
                lookAt = player.transform;
                attackHive = false;
            }
            else if (hiveFound)
            {
                toTarget = hive.transform.position - transform.position;
                lookAt = hive.transform;
                attackHive = true;
            }

            if (lookAt != null)
            {
                MoveInDir(toTarget, attackSpeed);
                transform.LookAt(lookAt);
            }
        }

        // AttackRecoil() applied the recoil, this is actually
        // recovering from said recoil
        private void UpdateRecoilState()
        {
            
            float step = Time.deltaTime * recoilRecoverySpeed;
            
            if (Mathf.Abs(currentRecoil.magnitude) <= 0.1f ||
                Time.time - recoilTimer > recoilMaxTime)
            {
                FinishAttackRecoil();
            }
            
            if (attackHive)
            {
                transform.position = 
                    Vector3.MoveTowards(transform.position, hive.transform.position, -1 * step);
            }

            currentRecoil = Vector3.Lerp(currentRecoil, Vector3.zero, step);
            ctrl.Move(currentRecoil);
            
        }
        
        
        public override void ApplyDamage(float damage)
        {
            enemyHealth -= damage;
            
            currState = WaspFlyingState.Recoiling;
            recoilTimer = Time.time;

            Vector3 dir = (transform.position - player.transform.position);
            dir.y = 0;
            currentRecoil = dir.normalized * recoilImpactSpeed;
            
            ctrl.Move(currentRecoil * Time.deltaTime);
        }


        //Defeated/Dying
        public override void EnemyDefeated()
        {
            currState = WaspFlyingState.Dying;
            UpdateDieState();
        }

        private void UpdateDieState()
        {
            anim.SetInteger(AnimState, 2);
            transform.Translate(Vector3.down * Time.deltaTime * 10, Space.World);
            Destroy(gameObject, 1f);
        }

        void FinishAttackRecoil()
        {
            currState = attackHive ? WaspFlyingState.Patrolling : WaspFlyingState.Attacking;
            patrolStuckTimer = Time.time;

            currentRecoil = Vector3.zero;
            ctrl.SimpleMove(Vector3.zero);
        }


        public enum WaspFlyingState
        {
            Patrolling, Attacking, Recoiling, Dying
        }
        
        public void OnDrawGizmos()
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawWireSphere(transform.position, minDistance);
        }

    }
}
