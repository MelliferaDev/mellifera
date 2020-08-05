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
                // State Updating logic
                if (distToPlayer <= minDistance && currState != WaspFlyingState.Recoiling)
                {
                    if (currState != WaspFlyingState.Attacking) // attacking just started
                    {
                        RearviewCameraBehaviour.RequestRearviewOn();
                    }
                    currState = WaspFlyingState.Attacking;
                    anim.SetInteger(AnimState, 1);
                } else if (currState == WaspFlyingState.Attacking)
                {
                    RearviewCameraBehaviour.RequestRearviewOff(); // attacking is done
                    currState = WaspFlyingState.Patrolling;
                    anim.SetInteger(AnimState, 0);
                }

                switch (currState)
                {
                    case WaspFlyingState.Patrolling: UpdatePatrolState(); break;
                    case WaspFlyingState.Attacking: UpdateAttackState(); break;
                    case WaspFlyingState.Recoiling: UpdateRecoilState(); break;
                    case WaspFlyingState.Dying: UpdateDieState(); break;
                }
            }

           /* if (enemyHealth <=0)
            {
                anim.SetInteger(AnimState, 2);
                Destroy(gameObject, .5f);
            }*/
        }


    
        ///////////////////////////////////////////////
        //// Basic Movement States ////////////////////
      
        public override void UpdatePatrolState()
        {

            FindNextPoint();
            FaceTarget(nextPoint, false);

            transform.position = Vector3.MoveTowards(transform.position, nextPoint, patrolSpeed * Time.deltaTime);
        }
        
        ///////////////////////////////////////////////
        //// Attacking State //////////////////////////

        private void UpdateAttackState()
        {
            float step = Time.deltaTime * (attackSpeed);

            transform.LookAt(player.transform);
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
        }

        private void UpdateRecoilState()
        {
            float step = Time.deltaTime * recoilRecoverySpeed;
            
            // AttackRecoil() applied the recoil, this recovering from said recoil
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, step);
        }
        
        
        public override void ApplySelfDamage(float damage)
        {
            enemyHealth -= damage;
            currState = WaspFlyingState.Recoiling;
            rb.AddForce(Vector3.back * recoilForce, ForceMode.VelocityChange);
            Invoke(nameof(FinishAttackRecoil), 1f);
        }

        public void WaspDefeated()
        {
            currState = WaspFlyingState.Dying;
        }

        void UpdateDieState()
        {
            Debug.Log("Die State");
            anim.SetInteger(AnimState, 2);
            Destroy(gameObject, 2);
        }

        void FinishAttackRecoil()
        {
            currState = WaspFlyingState.Attacking;
            rb.velocity = Vector3.zero;
        }


        public enum WaspFlyingState
        {
            Patrolling, Attacking, Recoiling, Dying
        }
    }
}
