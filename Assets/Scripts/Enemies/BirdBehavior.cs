using System;
using System.Data.Common;
using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Enemies
{
    // Birds attack you at random no matter where you are as they circle the skies
    public class BirdBehavior : EnemyBehaviour
    {

        public BirdFlyingState currState = BirdFlyingState.Patrolling;
        [SerializeField] private float tipToCenter = 9f;
        [Header("Patrol Settings")] 
        [SerializeField] private Transform patrolCenter;
        [SerializeField] private float currAngle;
        [SerializeField] private float circleSpeed;
        [SerializeField] private float circleRadius;
        [SerializeField] private float patrolHover;
        [SerializeField] private float patrolHoverSpeed;
        [Header("Attack Settings")]
        public float attackDuration = 5f;
        public float attackDelayMin = 5f;
        public float attackDelayMax = 15f;
        public float attackSpeed = 5f;
        [Header("Attack Reactions")]
        public float enemyHealth = 100f;
        [SerializeField] private Slider healthBar;
        [SerializeField] private Slider timeBar;

        private CharacterController ctlr;
        Animator anim;
        private static readonly int BirdMovement = Animator.StringToHash("birdMovement");

        private bool playerInRange;
        private float attackTimer;
        public float attackDelay;
        private Vector3 lastPatrolPos;

        private bool healthBarFound;
        private bool timeBarFound;
        private Image timeBarFill;
        private bool timeBarFillFound = false;
        protected override void Start()
        {
            base.Start();
            
            anim = GetComponent<Animator>();
            anim.SetInteger(BirdMovement, 1);
            ctlr = GetComponent<CharacterController>();
            anim = GetComponent<Animator>();
            
            lastPatrolPos = transform.position;
            
            attackDelay = Random.Range(attackDelayMin, attackDelayMax);
            attackTimer = Time.time - attackDelay;

            playerInRange = false;

            healthBarFound = healthBar != null;
            timeBarFound = timeBar != null;
            
            SetHealthBar(true);
            SetTimeSlider(true, attackDelay);
            if (timeBarFound)
            {
                timeBarFill = timeBar.GetComponentsInChildren<Image>().FirstOrDefault(t => t.name == "Fill");
                timeBarFillFound = timeBarFill != null;
            
                timeBar.gameObject.SetActive(false);
            }
        }

        protected override void Update()
        {
            base.Update();

            if (currState == BirdFlyingState.Patrolling)
            {
                distToPlayer = Utils.Distance2D(patrolCenter.position, player.transform.position);
                distToHive = Utils.Distance2D(transform.position, hive.transform.position);
            }
            
            switch (currState)
            {
                case BirdFlyingState.Patrolling: UpdatePatrolState(); break;
                case BirdFlyingState.Diving: UpdateDivingState(); break;
                case BirdFlyingState.Returning: UpdateReturnState(); break;
                case BirdFlyingState.Dying: UpdateDyingState(); break;
            }

            if(enemyHealth <=0)
            {
                EnemyDefeated();
            }
        }

        ///////////////////////////////////////////////
        //// Basic Movement States ////////////////////

        public override void UpdatePatrolState()
        {

            if (distToPlayer <= circleRadius)
            {
                if (!playerInRange)
                {
                    RearviewCameraBehaviour.RequestRearviewOn();
                    playerInRange = true;
                    attackTimer = Time.time; // time how long the player is in range
                }
                
                SetTimeSlider(true, (Time.time - attackTimer));
            }
            else
            {
                SetTimeSlider(false, 0);
            }

            if (playerInRange && Time.time - attackTimer > attackDelay)
            {
                currState = BirdFlyingState.Diving;
                attackTimer = Time.time; // time how long the bird is diving
                playerInRange = false;
            }

            currAngle += circleSpeed * Time.deltaTime;
            currAngle %= 360;
            
            MoveInCircle(currAngle);
        }

        private void MoveInCircle(float angle)
        {
            float targetX = circleRadius * Mathf.Cos(Mathf.Deg2Rad * angle);
            float targetY = patrolHover * Mathf.Sin(Time.time * patrolHoverSpeed);
            float targetZ = circleRadius * Mathf.Sin(Mathf.Deg2Rad * angle);
            Vector3 localPos = new Vector3(targetX, targetY, targetZ);
            nextPoint = patrolCenter.TransformPoint(localPos);
            lastPatrolPos = nextPoint;
            Vector3 moveDir = (nextPoint - transform.position);


            ctlr.Move((nextPoint - transform.position));
            FaceTarget(transform.position + moveDir.normalized);
        }
        
        /////////////////////////////////////////////////////
        //// Other Movement States //////////////////////////

        private void UpdateDivingState()
        {
            if (distToPlayer <= tipToCenter || Time.time - attackTimer > attackDuration)
            {
                ExitDive();
            }
            
            SetTimeSlider(true, attackDelay);

            float step = Time.deltaTime * attackSpeed;
            nextPoint = player.transform.position;

            ctlr.Move((nextPoint - transform.position) * step);
            FaceTarget(nextPoint, false);
        }

        private void ExitDive()
        {
            // bird has been diving for set amount of time or hit the player
            RearviewCameraBehaviour.RequestRearviewOff();
            attackDelay = Random.Range(attackDelayMin, attackDelayMax);
                
            attackTimer = Time.time; // back to timing how long the player is in range
            attackDelay = Random.Range(attackDelayMin, attackDelayMax);
                
            currState = BirdFlyingState.Returning;
        }

        private void UpdateReturnState()
        {
            float dist = Vector3.Distance(transform.position, lastPatrolPos);
            if (dist < tipToCenter)
            {
                currState = BirdFlyingState.Patrolling;
            }
            
            SetTimeSlider(false, attackDelay);


            float step = Time.deltaTime;// * (attackSpeed);
            nextPoint = lastPatrolPos;
            
            ctlr.Move((nextPoint - transform.position) * step);
            FaceTarget(nextPoint, false);
        }

        /////////////////////////////////////////////////////
        //// Health Stuff and Dying State ///////////////////
        
        private void UpdateDyingState()
        {
            ctlr.Move(Vector3.down * (Time.deltaTime * 10));
        }

        public override void ApplyDamage(float damage)
        {
            enemyHealth -= damage;
            SetHealthBar();
            
            if (enemyHealth <= 0)
            {
                this.EnemyDefeated();
            }

            if (currState == BirdFlyingState.Diving)
            {
                ExitDive();
            }
        }

        //Defeated
        public override void EnemyDefeated()
        {
            currState = BirdFlyingState.Dying;
            transform.Translate(Vector3.down * (Time.deltaTime * 10), Space.World);
            Invoke(nameof(DestroyBird), 1f);
        }

        private void DestroyBird()
        {
            GameObject canvas = null;
            if (healthBarFound)
            {
                canvas = healthBar.transform.GetComponentInParent<Canvas>().gameObject;
            } else if (timeBarFound)
            {
                canvas = timeBar.transform.GetComponentInParent<Canvas>().gameObject;
            }

            if (canvas != null)
            {
                Destroy(canvas);
            }
            Destroy(gameObject);
        }
        
        /////////////////////////////////////////////////////
        //// UI Stuff ///////////////////////////////////////
        private void SetHealthBar(bool init = false)
        {
            if (!healthBarFound) return;

            if (init)
            {
                healthBar.maxValue = enemyHealth;
                healthBar.minValue = 0;
            }
            
            healthBar.value = enemyHealth;

        }

        private void SetTimeSlider(bool on, float time)
        {
            if (!timeBarFound) return;

            if (on)
            {
                if (timeBarFillFound)
                {
                    timeBarFill.color = (currState == BirdFlyingState.Patrolling) ? new Color(1f, 0.57f, 0.05f) : Color.red;
                }
                
                timeBar.gameObject.SetActive(true);
                timeBar.maxValue = attackDelay;
                timeBar.minValue = 0;
                timeBar.value = time;
            }
            else
            {
                timeBar.gameObject.SetActive(false);
            }
        }

        public void OnDrawGizmosSelected()
        {
            if (player != null)
            {
                if (distToPlayer <= circleRadius)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.yellow;
                }
                Gizmos.DrawLine(patrolCenter.position, player.transform.position);
            }
            
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(patrolCenter.position, circleRadius);
        }

        public enum BirdFlyingState
        {
            Patrolling, Diving, Returning, Dying
        }
    }
}
