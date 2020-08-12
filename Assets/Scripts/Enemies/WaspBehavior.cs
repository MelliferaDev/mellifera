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
        public float recoilForce = 0.75f;
        public float recoilRecoverySpeed = 5f;
        public float enemyHealth = 10;
        // public float playerAttack = 2;
        [Header("Hovering")]
        public float patrolSpeed = 5f;
        
        private Rigidbody rb;
        Animator anim;
        private static readonly int AnimState = Animator.StringToHash("animState");

        private bool attackHive;
        
        protected override void Start()
        {
            base.Start();

            rb = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();

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
        }

        ///////////////////////////////////////////////
        //// Basic Movement States ////////////////////
      
        public override void UpdatePatrolState()
        {
            anim.SetInteger(AnimState, 0);
            
            if (Utils.Distance2D(transform.position, nextPoint) <= Mathf.Epsilon)
            {
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
            } else if (distToHive <= minDistance)
            {
                Debug.Log("Waiting to attack: " + (Time.time - hiveAttackTimer));
            }
            
            FaceTarget(nextPoint, false);
            transform.position = Vector3.MoveTowards(transform.position, nextPoint, patrolSpeed * Time.deltaTime);
        }
        
        ///////////////////////////////////////////////
        //// Attacking State //////////////////////////

        private void UpdateAttackState()
        {
            float step = Time.deltaTime * (attackSpeed);

            if (distToPlayer > minDistance && distToHive > minDistance)
            {
                RearviewCameraBehaviour.RequestRearviewOff(); // attacking is done
                currState = WaspFlyingState.Patrolling;
            }
            
            anim.SetInteger(AnimState, 1);
            if (distToPlayer <= minDistance)
            {
                transform.LookAt(player.transform);
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
                attackHive = false;
            }
            else
            {
                transform.LookAt(hive.transform);
                transform.position = Vector3.MoveTowards(transform.position, hive.transform.position, step);
                attackHive = true;
            }
        }

        private void UpdateRecoilState()
        {
            float step = Time.deltaTime * recoilRecoverySpeed;

            // AttackRecoil() applied the recoil, this is actually
            // recovering from said recoil
            if (attackHive)
            {
                transform.position = Vector3.MoveTowards(transform.position, hive.transform.position, -1 * step);
            }
            
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, step);
        }
        
        
        public override void ApplyDamage(float damage)
        {
            enemyHealth -= damage;
            currState = WaspFlyingState.Recoiling;
            rb.AddForce(Vector3.back * recoilForce, ForceMode.VelocityChange);
            Invoke(nameof(FinishAttackRecoil), 1f);
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
            rb.velocity = Vector3.zero;
        }


        public enum WaspFlyingState
        {
            Patrolling, Attacking, Recoiling, Dying
        }
    }
}
