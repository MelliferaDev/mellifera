﻿using System;
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
        [Tooltip("attackHiveDist - tipToCenter is the \"real\" distance")]
        public float attackHiveDist = 10f;
        [Header("Attack Reactions")]
        public float enemyHealth = 100f;
        [SerializeField] private Slider healthBar;
        [SerializeField] private Slider timeBar;
        [SerializeField] private Color patrolColor;
        [SerializeField] private Color attackColor;

        private CharacterController ctlr;
        Animator anim;
        private static readonly int BirdMovement = Animator.StringToHash("birdMovement");

        private bool playerInRange;
        
        private float attackTimer;
        public float attackDelay;
        private Vector3 lastPatrolPos;
        private bool attackHive; // true if bird should be attacking hive 
        
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
            // figure out if the player is in the circle
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
                playerInRange = false;
            }

            // State Logic
            if (playerInRange && Time.time - attackTimer > attackDelay)
            {
                currState = BirdFlyingState.Diving;
                attackTimer = Time.time; // time how long the bird is diving
                playerInRange = false;
                attackHive = false;
            } else if (distToHive <= attackHiveDist && Time.time - attackTimer > attackDelay)
            {
                currState = BirdFlyingState.Diving;
                attackTimer = Time.time;
                playerInRange = false;
                attackHive = true;
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
            if ((!attackHive && distToPlayer <= tipToCenter) 
                // || (attackHive && distToHive > attackHiveDist)
                || (Time.time - attackTimer > attackDuration))
            {
                ExitDive();
            }

            if (!attackHive)
            {
                SetTimeSlider(true, attackDelay);
            }

            float step = Time.deltaTime * attackSpeed;
            nextPoint = attackHive ?  
                hive.transform.position :
                player.transform.position;

            ctlr.Move((nextPoint - transform.position) * step);
            FaceTarget(nextPoint, false);
        }

        // bird has been diving for set amount of time or hit the player
        private void ExitDive()
        {
            if (!attackHive)
            {
                RearviewCameraBehaviour.RequestRearviewOff();
            }
            
            attackDelay = Random.Range(attackDelayMin, attackDelayMax);
                
            // back to timing how long the player is in range
            attackTimer = Time.time; 
            attackDelay = Random.Range(attackDelayMin, attackDelayMax);
            
            attackHive = false;
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
                    timeBarFill.color = (currState == BirdFlyingState.Patrolling) ? patrolColor : attackColor;
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

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 v1 = transform.position + transform.forward * circleRadius;
            Vector3 v2 = transform.position + transform.forward * -circleRadius;
            Vector3 v3 = transform.position + transform.right * circleRadius;
            Vector3 v4 = transform.position + transform.right * -circleRadius;

            Gizmos.DrawLine(transform.position, v1);
            Gizmos.DrawLine(transform.position, v2);
            Gizmos.DrawLine(transform.position, v3);
            Gizmos.DrawLine(transform.position, v4);

        }
        
        public void OnDrawGizmosSelected()
        {
            
            if (ctlr != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, transform.position + ctlr.velocity);
            }

            if (patrolCenter != null)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawWireSphere(patrolCenter.position, circleRadius);
            }
            
            if (hive != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, attackHiveDist);
            }
        }

        public enum BirdFlyingState
        {
            Patrolling, Diving, Returning, Dying
        }
    }
}
