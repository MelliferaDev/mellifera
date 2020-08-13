using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public abstract class EnemyBehaviour : MonoBehaviour
    {
        [SerializeField] protected float rotationSpeed = 10; // 30 for skunks
        [SerializeField] private float patrolMin = 15;
        [SerializeField] private float patrolMax = 20;
        [SerializeField] protected float hiveAttackCooldown = 10f;
        protected Vector3 pointA;
        protected Vector3 pointB;
        private float patrolDistanceX;
        private float patrolDistanceZ;
        protected Vector3 nextPoint;
        
        protected GameObject player;
        protected GameObject hive;
        protected bool hiveFound;
        protected float distToPlayer;
        protected float distToHive;
        protected float hiveAttackTimer;

        protected virtual void Start()
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
            hive = GameObject.FindGameObjectWithTag("HiveParent");
            hiveFound = hive != null;
            
            Vector3 position = transform.position;
            
            pointA = position;
            
            patrolDistanceX = Random.Range(patrolMin, patrolMax);
            patrolDistanceZ = Random.Range(patrolMin, patrolMax);
            int xSign = (int)(Random.Range(0f, 1.9f)) == 0 ? 1 : -1;
            int zSign = (int)(Random.Range(0f, 1.9f)) == 0 ? 1 : -1;
            patrolDistanceX *= xSign;
            patrolDistanceZ *= zSign;
            pointB = new Vector3(position.x + patrolDistanceX, position.y, position.z + patrolDistanceZ);
            
            nextPoint = pointA;
            hiveAttackTimer = Time.time - hiveAttackCooldown - 0.1f;
        }

        protected virtual void Update()
        {
            distToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (hive != null)
            {
                distToHive = Vector3.Distance(transform.position, hive.transform.position);
            }
            else
            {
                // this is the definition of bad coding practices
                // lolll.... it works tho!
                distToHive = Mathf.Infinity;
            }
        }

        public abstract void UpdatePatrolState();
        public abstract void EnemyDefeated();

        public abstract void ApplyDamage(float damage);
        
        protected void FindNextPoint()
        {
            nextPoint = (nextPoint == pointA) ? pointB : pointA;
        }

        protected void FaceTargetReverse(Vector3 target, bool forceGrounded=true)
        {
            Vector3 dirTarget = (target - transform.position).normalized;
            dirTarget *= -1;
            
            if (forceGrounded)
            {
                dirTarget.y = 0;
            }

            if (dirTarget.magnitude > Mathf.Epsilon)
            {
                Quaternion lookRotation = Quaternion.LookRotation(dirTarget);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            }
        }
    
        protected void FaceTarget(Vector3 target, bool forceGrounded=true)
        {
            Vector3 dirTarget = (target - transform.position).normalized;
        
            if (forceGrounded)
            {
                dirTarget.y = 0;
            }

            if (Mathf.Abs(dirTarget.magnitude) > Mathf.Epsilon)
            {
                Quaternion lookRotation = Quaternion.LookRotation(dirTarget);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}