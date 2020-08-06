using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public abstract class EnemyBehaviour : MonoBehaviour
    {
        [SerializeField] protected float rotationSpeed = 10; // 30 for skunks
        [SerializeField] private float patrolMin = 15;
        [SerializeField] private float patrolMax = 20;

        protected Vector3 pointA;
        protected Vector3 pointB;
        private float patrolDistanceX;
        private float patrolDistanceZ;
        protected Vector3 nextPoint;
        
        protected GameObject player;
        protected float distToPlayer;

        protected virtual void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            
            Vector3 position = transform.position;
            
            pointA = position;
            patrolDistanceX = Random.Range(patrolMin, patrolMax);
            patrolDistanceZ = Random.Range(patrolMin, patrolMax);
            pointB = new Vector3(position.x + patrolDistanceX, position.y, position.z + patrolDistanceZ);
            nextPoint = pointA;
        }

        protected virtual void Update()
        {
            distToPlayer = Vector3.Distance(transform.position, player.transform.position);
        }


        public abstract void UpdatePatrolState();
        public abstract void EnemyDefeated();

        public abstract void ApplySelfDamage(float damage);
        
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
            
            Quaternion lookRotation = Quaternion.LookRotation(dirTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    
        protected void FaceTarget(Vector3 target, bool forceGrounded=true)
        {
            Vector3 dirTarget = (target - transform.position).normalized;
        
            if (forceGrounded)
            {
                dirTarget.y = 0;
            }
            
            Quaternion lookRotation = Quaternion.LookRotation(dirTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }
}